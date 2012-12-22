// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodResultCaching.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MethodResultCaching type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class MethodResultCaching : IInterceptionBehavior
    {
        /// <summary>
        /// Full name of a "List of String" to detect the serialization strategy.
        /// </summary>
        private static readonly string TypeNameListOfString = typeof(List<string>).FullName;

        /// <summary>
        /// Cache instance for values.
        /// </summary>
        private static readonly ConcurrentDictionary<string, CacheMetaValue<object>> ValueCache = new ConcurrentDictionary<string, CacheMetaValue<object>>();

        /// <summary>
        /// Cache instance for meta information.
        /// </summary>
        private static readonly ConcurrentDictionary<string, CacheMetaBase> MetaCache = new ConcurrentDictionary<string, CacheMetaBase>();

        /// <summary>
        /// Full name of a "IEnumerable of String" to detect the serialization strategy.
        /// </summary>
        private static readonly string TypeNameIEnumerableOfString = typeof(IEnumerable<string>).FullName;

        /// <summary>
        /// A factory instance for performance counter.
        /// </summary>
        private static readonly PerfCounter<CachePerfCounter> PerfCounterFactory = new PerfCounter<CachePerfCounter>();

        /// <summary>
        /// The performance counter instance to cache miss events.
        /// </summary>
        private static readonly PerformanceCounter CacheMiss = PerfCounterFactory[CachePerfCounter.CacheMiss];

        /// <summary>
        /// The performance counter instance to cache miss events.
        /// </summary>
        private static readonly PerformanceCounter CacheHit = PerfCounterFactory[CachePerfCounter.CacheHit];

        /// <summary>
        /// The parameter collection.
        /// </summary>
        private static readonly ParameterCollection ParameterCollection = new ParameterCollection(new object[] { }, new ParameterInfo[] { }, x => false);

        /// <summary>
        /// Counts the calls to be able to cleanup the cache each X calls.
        /// </summary>
        private static int callCounter;

        /// <summary>
        /// A service locator instance.
        /// </summary>
        private readonly IUnityContainer resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodResultCaching"/> class.
        /// </summary>
        /// <param name="resolverInstance"> The resolver instance (service locator). </param>
        public MethodResultCaching(IUnityContainer resolverInstance)
        {
            this.resolver = resolverInstance;
        }

        /// <summary>
        /// Gets the count of cached items inside the value cache.
        /// </summary>
        public static int ValueCount
        {
            get
            {
                return ValueCache.Count();
            }
        }

        /// <summary>
        /// Gets the count of cached items inside the value cache.
        /// </summary>
        public static IEnumerable<CacheEntryValue> Values
        {
            get
            {
                return ValueCache.Select(x => new CacheEntryValue
                                                  {
                                                      Name = x.Key,
                                                      CreationDate = x.Value.CreationDate,
                                                      LocalIdentifier = x.Value.LocalIdentifier,
                                                      ValidUntil = x.Value.ValidUntil,
                                                      Object = x.Value.Object,
                                                  });
            }
        }

        /// <summary>     
        /// Gets the count of cached items inside the meta-data cache.
        /// </summary>
        public static int MetaCount
        {
            get
            {
                return MetaCache.Count();
            }
        }

        /// <summary>
        /// Gets all dependency names currently inside the cache.
        /// </summary>
        public static IEnumerable<CacheEntryStatistic> CurrentDependencyNames
        {
            get
            {
                return MetaCache
                    .Select(x => x.Value)
                    .SelectMany(x => x.DependencyValues)
                    .GroupBy(x => x, x => x)
                    .Select(x => new CacheEntryStatistic
                        {
                            Name = x.Key,
                            Count = x.Count(),
                        })
                    .OrderBy(x => x.Name);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this behavior will actually do anything when invoked.
        /// </summary>
        /// <remarks>
        /// This is used to optimize interception. If the behaviors won't actually
        ///             do anything (for example, PIAB where no policies match) then the interception
        ///             mechanism can be skipped completely.
        /// </remarks>
        public bool WillExecute
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Performs a full clear of the meta cache and the value cache.
        /// </summary>
        public static void Clear()
        {
            callCounter = 0;
            MetaCache.Clear();
            ValueCache.Clear();
        }

        /// <summary>
        /// Provides ability to invalidate all items that share a specific dependency.
        /// TODO: since the user of this class might be load balanced, the cache cleanup must be propagated to all other servers, too - in eSuite this might be done from the CRM or by implementing some kind of messaging between the servers
        /// </summary>
        /// <param name="dependencyName"> The name of the dependency that will select the items to be deleted. </param>
        public static void Invalidate(string dependencyName)
        {
            // NOTE: using AppFabric Cache might be an issue with this kind of caching
            // Since the cache key cannot be predicted by a single dependency name, we need to search the cache for 
            // all entries that do contain a specific dependency. In case of AppFabric Cache this might be solved
            // by enabeling "local cache" for the cache client with notification-based invalidation.
            // (http://msdn.microsoft.com/en-us/library/ee790983.aspx)
            // When using that approach, you need to keep in mind that "local cache"-objects are shared instances
            // (so two times read with the same key will result in two references to ONLY ONE object instance), 
            // while objects read from the cache servers are deserialized objects that do not write back to the 
            // cache server automatically (so two times read with the same key will result in two object instances).
            var name1 = dependencyName;
            foreach (var entry in MetaCache.Select(x => x.Value)
                .Where(entry => entry.DependencyValues.Any(x => x == name1)).ToArrayEx())
            {
                RemoveFromCache(entry);
            }
        }

        /// <summary>
        /// Implement this method to execute your behavior processing.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target.</param>
        /// <param name="getNext">Delegate to execute to get the next delegate in the behavior chain.</param>
        /// <returns>
        /// Return value from the target.
        /// </returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (getNext == null)
            {
                return null;
            }

            if (input == null)
            {
                return null;
            }

            try
            {
                var argumentTypes = new Type[input.Arguments.Count];
                for (var i = 0; i < input.Arguments.Count; i++)
                {
                    argumentTypes[i] = input.Arguments.GetParameterInfo(i).ParameterType;
                }

                Interlocked.Increment(ref callCounter);
                if (callCounter > 100)
                {
                    CleanupOldEntries();
                    callCounter = 0;
                }

                // ReSharper disable ConvertIfStatementToNullCoalescingExpression
                var target = input.Target;
                var declaringType = target.GetType();
                var targetMethodInfo = declaringType.GetMethod(input.MethodBase.Name, argumentTypes);
                if (targetMethodInfo == null)
                {
                    // ReSharper restore ConvertIfStatementToNullCoalescingExpression
                    targetMethodInfo = declaringType.GetMethods().First(x => x.Name == input.MethodBase.Name && x.GetParameters().Length == argumentTypes.Length);
                }

                var customAttributes = targetMethodInfo.GetCustomAttributes(true);

                if (targetMethodInfo.IsSpecialName && input.MethodBase.Name.StartsWith("get_", StringComparison.Ordinal))
                {
                    var memberInfo = declaringType.GetProperty(input.MethodBase.Name.Replace("get_", string.Empty));
                    if (memberInfo != null)
                    {
                        customAttributes = customAttributes.Union(memberInfo.GetCustomAttributes(true)).ToArrayEx();
                    }
                }

                var cachingAttribute = customAttributes.OfType<CacheAttribute>().FirstOrDefault();

                // we don't have the caching attribute on this method, so we don't cache any result
                if (cachingAttribute == null)
                {
                    return getNext().Invoke(input, getNext);
                }

                var cachingAction = cachingAttribute.CachingAction;

                switch (cachingAction)
                {
                    case CachingAction.CachePublic:
                    case CachingAction.CachePrivate:
                        var result = this.HandleCaching(input, getNext, customAttributes, cachingAttribute);
                        return input.CreateMethodReturn(result.Object);

                    case CachingAction.AppendManagement:
                        var result2 = this.HandleAppendedManagement(input, targetMethodInfo);
                        return input.CreateMethodReturn(result2.Object);
                }

                var methodReturn = getNext().Invoke(input, getNext);

                switch (cachingAction)
                {
                    case CachingAction.Invalidate:
                        foreach (var cacheEntry in SearchMetaOnlyInCache(customAttributes))
                        {
                            RemoveFromCache(cacheEntry);
                        }

                        break;
                }

                return methodReturn;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return getNext().Invoke(input, getNext);
            }
        }

        /// <summary>
        /// Returns the interfaces required by the behavior for the objects it intercepts.
        /// </summary>
        /// <returns> The required interfaces. </returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// Updates the cache with the result of a method call.
        /// </summary>
        /// <param name="func"> The method call which should be updated. </param>
        /// <typeparam name="TResult">The type of the return parameter of the method call.</typeparam>
        public void UpdateCache<TResult>(Expression<Func<TResult>> func)
        {
            func.ThrowIfParameterNull("func");

            // todo: this is just the beginning of this implementation
            // this has not been tested, so don't assume this to be valid code!
            var expression = func.Body as MethodCallExpression;
            if (expression == null)
            {
                return;
            }

            CacheMetaBase meta;

            var methodInfo = expression.Method;
            var cacheAttrib = methodInfo.GetCustomAttributes(typeof(CacheAttribute), true).FirstOrDefault() as CacheAttribute;
            if (cacheAttrib == null)
            {
                return;
            }

            var key = this.BuildCacheKey(methodInfo, ParameterCollection);

            // skip processing, if we don't have that value
            if (!MetaCache.TryGetValue(key + ".Meta", out meta))
            {
                return;
            }

            // skip processing, if we have that value and it is still more than 2 minutes valid
            if (meta.ValidUntil > DateTime.UtcNow.AddMinutes(2))
            {
                return;
            }

            CacheMetaValue<object> value;
            if (!ValueCache.TryGetValue(key, out value))
            {
                return;
            }

            value.Object = func.Compile().Invoke();
            value.ValidUntil = DateTime.UtcNow.AddMinutes(5);
        }

        /// <summary>
        /// Currently out parameters are not supported, so we validate the parameters here.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target.</param>
        /// <exception cref="InvalidOperationException"> In case of out parameters found in the method signature. </exception>
        private static void ValidateInputParameters(IMethodInvocation input)
        {
            for (var i = 0; i < input.Arguments.Count; i++)
            {
                var parameterInfo = input.Arguments.GetParameterInfo(i);
                if (parameterInfo.IsOut)
                {
                    throw new InvalidOperationException("The method {0} cannot be decorated with a CachingAttribute since it uses OUT-parameters.");
                }
            }
        }

        /// <summary>
        /// Serializes a value of an object so it can be hashed for the cache-key-building
        /// </summary>
        /// <param name="parameterValue"> The input parameter value. </param>
        /// <param name="parameterType">the type of the object</param>
        /// <returns> A string representation of the object (may skip some object state that is not relevant to identify the object instance). </returns>
        /// <exception cref="InvalidOperationException"> If the object type is currently not supported </exception>
        private static string SerializeParameter(object parameterValue, Type parameterType)
        {
            if (parameterValue == null)
            {
                return "NULL";
            }

            var fullTypeName = parameterType.FullName;
            switch (fullTypeName)
            {
                case "System.String":
                case "System.Int":
                case "System.Int32":
                case "System.Int64":
                case "System.Boolean":
                case "System.Guid":
                    return parameterValue.ToString();

                case "System.DateTime":
                case "System.Nullable`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]":
                    return ((DateTime)parameterValue).Ticks.ToString(CultureInfo.InvariantCulture);

                case "System.String[]":
                    return string.Join(">,<", (string[])parameterValue);

                default:
                    if (fullTypeName == TypeNameListOfString || fullTypeName == TypeNameIEnumerableOfString)
                    {
                        return SerializeParameter(((IEnumerable<string>)parameterValue).ToArrayEx(), typeof(string[]));
                    }

                    if (parameterType.IsArray)
                    {
                        // handle enum arrays
                        var array = parameterValue as Array;
                        if (array != null)
                        {
                            return array.Length > 0 && array.GetValue(0) is Enum ? string.Join(">,<", array.OfType<Enum>()) : string.Empty;
                        }
                    }

                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The type {0} cannot be serialized in this context", fullTypeName));
            }
        }

        /// <summary>
        /// Saves an entry into the cache.
        /// </summary>
        /// <param name="obj"> The object to be cached. </param>
        private static void AddToCache(CacheMetaValue<object> obj)
        {
            ValueCache.AddOrUpdate(obj.Key, obj, (key, value) => value);
            MetaCache.AddOrUpdate(obj.Key + ".Meta", obj.CloneBase(), (key, value) => value);
        }

        /// <summary>
        /// Gets an entry from the cache.
        /// </summary>
        /// <param name="key">The key of the item to get.</param>
        /// <returns>A cached item.</returns>
        private static CacheMetaValue<object> GetFromCache(string key)
        {
            CacheMetaValue<object> value;
            ValueCache.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Deletes items from the cache.
        /// </summary>
        /// <param name="obj"> The cache entry to be deleted. </param>
        private static void RemoveFromCache(CacheMetaBase obj)
        {
            CacheMetaBase meta;
            CacheMetaValue<object> value;
            MetaCache.TryRemove(obj.Key + ".Meta", out meta);
            ValueCache.TryRemove(obj.Key, out value);
        }

        /// <summary>
        /// Performs a cleanup of cached values by requesting those that are timed out and 
        /// removed them from cache (only processes cached values that do contain a meta-object).
        /// </summary>
        private static void CleanupOldEntries()
        {
            var toDelete = MetaCache.Where(value => value.Value.ValidUntil < DateTime.UtcNow).ToArrayEx();

            foreach (var value in toDelete)
            {
                RemoveFromCache(value.Value);
            }
        }

        /// <summary>
        /// Searches meta information from the cache by searching all entities matching the dependencies from the attributes.
        /// </summary>
        /// <param name="customAttributes"> The custom attributes of the calling method. </param>
        /// <returns> A list of meta-entities. </returns>
        private static IEnumerable<CacheMetaBase> SearchMetaOnlyInCache(IEnumerable<object> customAttributes)
        {
            var dependencies = customAttributes.OfType<CacheDependencyAttribute>().Select(x => x.EntityType.FullName);
            return MetaCache
                .Where(entry => entry.Value.DependencyValues.Any(x => dependencies.Any(y => y == x)))
                .Select(x => x.Value)
                .ToArrayEx();
        }

        /// <summary>
        /// Gets the custom attributes of a method parameter.
        /// </summary>
        /// <param name="typeHint"> The type hint. </param>
        /// <param name="input"> The input. </param>
        /// <param name="position"> The position. </param>
        /// <returns> The <see cref="IEnumerable{T}"/>. </returns>
        private static IEnumerable<object> CustomAttributes(Type typeHint, IMethodInvocation input, int position)
        {
            return typeHint == null
                    ? Enumerable.Empty<object>()
                    : typeHint.GetMethod((string)input.Inputs[position + 1]).GetCustomAttributes(true);
        }

        /// <summary>
        /// The custom attributes.
        /// </summary>
        /// <param name="lambdaHint">
        /// The lambda hint.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        private static IEnumerable<object> CustomAttributes(LambdaExpression lambdaHint)
        {
            if (lambdaHint == null)
            {
                return Enumerable.Empty<object>();
            }

            var customAttributes = new object[0];
            var customAttributeHint = lambdaHint.Body;
            var methodCall = customAttributeHint as MethodCallExpression;
            if (methodCall != null)
            {
                customAttributes = methodCall.Method.GetCustomAttributes(true);
            }

            var propertyCall = customAttributeHint as MemberExpression;
            if (propertyCall != null)
            {
                customAttributes = propertyCall.Member.GetCustomAttributes(true);
            }

            return customAttributes;
        }

        /// <summary>
        /// The value <see cref="CachingAction.AppendManagement"/> of the <see cref="CachingAction"/> specifies that this method is a
        /// caching method of the target that should be replaced by the caching behavior of this implementation. This enables us to take
        /// advantage of the cache-management functionality (method attributes / cache invalidation / etc.) without explicitly referencing
        /// and relying on this component.
        /// </summary>
        /// <param name="input"> The input. </param>
        /// <param name="methodInfo"> The method info of the method that has been called. </param>
        /// <returns> The result for the method call. </returns>
        private CacheMetaValue<object> HandleAppendedManagement(IMethodInvocation input, MethodInfo methodInfo)
        {
            // create a name for the entry
            var parameterInfos = methodInfo.GetParameters();
            var name = (string)input.Inputs[parameterInfos.First(x => x.GetCustomAttributes(typeof(CacheManagementIsNameAttribute), true).Any()).Position];

            var position = parameterInfos.First(x => x.GetCustomAttributes(typeof(CacheManagementIsDependencyHintAttribute), true).Any()).Position;
            var hint = input.Inputs[position];

            var customAttributes = CustomAttributes(hint as LambdaExpression)
                .Union(CustomAttributes(hint as Type, input, position))
                .ToArrayEx();

            // try to get from cache
            var result = GetFromCache(name);

            // if there is a cached value and it's valid
            if (result == null
                || result.Object == null
                || result.ValidUntil < DateTime.UtcNow
                || !customAttributes
                        .OfType<CacheValidationProviderAttribute>()
                        .All(validatorAttribute => ((ICacheItemValidationProvider)this.resolver.Resolve(validatorAttribute.CacheValidationProviderType)).IsValid(result)))
            {
                CacheMiss.Increment();
                var creation = (Func<object>)input.Inputs[parameterInfos.First(x => x.GetCustomAttributes(typeof(CacheManagementIsCreationAttribute), true).Any()).Position];

                // get the cached metavalue entry
                result = new CacheMetaValue<object>(creation())
                             {
                                 Key = name,
                                 DependencyValues = customAttributes
                                     .OfType<CacheDependencyAttribute>()
                                     .Select(dependency => dependency.EntityType.FullName)
                                     .Union(
                                         customAttributes
                                     .OfType<CacheDependencyProviderAttribute>()
                                     .SelectMany(this.GetDependenciesFromProviders))
                                     .ToList(),
                                 ValidUntil = DateTime.UtcNow.AddMinutes(5)
                             };

                // send the fresh item to the cache, the return
                AddToCache(result);
            }
            else
            {
                CacheHit.Increment();
            }

            return result;
        }

        /// <summary>
        /// Handles the caching (get/set) of cached method results.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target.</param>
        /// <param name="getNext">Delegate to execute to get the next delegate in the behavior chain.</param>
        /// <param name="customAttributes"> The custom attributes of the method implementation. </param>
        /// <param name="cachingAttribute"> The caching attribute of the method implementation. </param>
        /// <returns> A cache meta value including the method return value (may come from cache). </returns>
        private CacheMetaValue<object> HandleCaching(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext, object[] customAttributes, CacheAttribute cachingAttribute)
        {
            ValidateInputParameters(input);

            // create a name for the entry
            var name = this.BuildCacheKey(input.MethodBase, input.Arguments);

            // try to get from cache
            var result = GetFromCache(name);

            // if there is a cached value and it's valid
            if (result == null
                || result.Object == null
                || result.ValidUntil < DateTime.UtcNow
                || !customAttributes
                        .OfType<CacheValidationProviderAttribute>()
                        .All(validatorAttribute => ((ICacheItemValidationProvider)this.resolver.Resolve(validatorAttribute.CacheValidationProviderType)).IsValid(result)))
            {
                CacheMiss.Increment();

                // get the cached metavalue entry
                result = this.BuildResult(input, getNext, customAttributes, cachingAttribute, name);

                // send the fresh item to the cache, the return
                AddToCache(result);
            }
            else
            {
                CacheHit.Increment();
            }

            return result;
        }

        /// <summary>
        /// Creates a method that returns a <see cref="CacheMetaValue{Object}"/> for the method result cache.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target.</param>
        /// <param name="getNext">Delegate to execute to get the next delegate in the behavior chain.</param>
        /// <param name="customAttributes"> The custom attributes of the current method. </param>
        /// <param name="cachingAttribute"> Caching information. </param>
        /// <param name="key"> The caching key. </param>
        /// <returns> A <see cref="CacheMetaValue{Object}"/> for the method result cache. </returns>
        private CacheMetaValue<object> BuildResult(
            IMethodInvocation input,
            GetNextInterceptionBehaviorDelegate getNext,
            object[] customAttributes,
            CacheAttribute cachingAttribute,
            string key)
        {
            var returnValue = getNext().Invoke(input, getNext).ReturnValue;
            var metaValue = new CacheMetaValue<object>
                {
                    Object = returnValue,
                    Key = key,
                    DependencyValues = customAttributes
                                        .OfType<CacheDependencyAttribute>()
                                        .Select(dependency => dependency.EntityType.FullName)
                                    .Union(
                                       customAttributes
                                        .OfType<CacheDependencyProviderAttribute>()
                                        .SelectMany(this.GetDependenciesFromProviders))
                                    .ToList(),
                    ValidUntil = DateTime.UtcNow.AddSeconds(cachingAttribute.Lifetime)
                };

            return metaValue;
        }

        /// <summary>
        /// Creates a cache-key for the method call by including the (...).ToString() of all parameters.
        /// </summary>
        /// <param name="methodInfo">The method info for to create a key for.</param>
        /// <param name="arguments">argument parameter collection</param>
        /// <returns>A unique cache key for the method call.</returns>
        private string BuildCacheKey(MethodBase methodInfo, IParameterCollection arguments)
        {
            var typeName = (methodInfo.DeclaringType == null) ? string.Empty : methodInfo.DeclaringType.FullName;
            var methodName = methodInfo.Name;

            var key = new StringBuilder();
            key.Append(typeName);
            key.Append("-");
            key.Append(methodName);

            for (var index = 0; index < arguments.Count; index++)
            {
                var info = arguments.GetParameterInfo(index);
                var keyValue = arguments[index];
                key.Append("<");
                key.Append(index);
                key.Append(":");
                key.Append(SerializeParameter(keyValue, info.ParameterType));
                key.Append(">");
            }

            var keyString = key.ToString();
            return keyString.Length > 200 ? keyString.GetSHA1Hash() : keyString;
        }

        /// <summary>
        /// Gets dependencies from a dependency-provider attribute.
        /// </summary>
        /// <param name="cachingDepProvid"> The dependency-provider attribute. </param>
        /// <returns> A list of dependency-names </returns>
        private IEnumerable<string> GetDependenciesFromProviders(CacheDependencyProviderAttribute cachingDepProvid)
        {
            var instance = this.resolver.Resolve(cachingDepProvid.CacheDependencyProviderType) as ICacheDependencyProvider;

            return instance == null
                    ? Enumerable.Empty<string>()
                    : instance.Dependency();
        }
    }
}
