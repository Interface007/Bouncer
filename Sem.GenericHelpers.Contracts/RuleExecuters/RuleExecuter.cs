// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleExecuter.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleExecuter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Configuration;
    using Sem.GenericHelpers.Contracts.Exceptions;
    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// This is the abstract base class for rule execution classes. Each rule execution class provides common 
    /// functionality by inheriting from this class. The functionality is providing multiple Assert methods 
    /// that will trigger rule validation and invocation of execution class specific logic via abstract and 
    /// virtual methods.
    /// </summary>
    /// <typeparam name="TData">The data type to be checked - inheriting classes should have this as a generic type parameter, too.</typeparam>
    /// <typeparam name="TResultClass">The type of result class, which is the inheriting class itself.</typeparam>
    public abstract class RuleExecuter<TData, TResultClass> : IRuleExecuter
        where TResultClass : RuleExecuter<TData, TResultClass>
    {
        private const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        #region data

        /// <summary>
        /// Gets or sets a pointer to the Assert()-method of the previously built <see cref="RuleExecuter{TData,TResultClass}"/>.
        /// Only the Assert()-method can be invoked, because all other Assert methods do rely on the data type, 
        /// which may differ from the current.
        /// </summary>
        internal IRuleExecuter PreviousExecuter { get; set; }

        /// <summary>
        /// Gets a list of <see cref="ContractMethodRuleAttribute"/> for the current method (the one that did create the 
        /// instance of the <see cref="RuleExecuter{TData,TResultClass}"/>).
        /// </summary>
        protected IEnumerable<ContractMethodRuleAttribute> MethodRuleAttributes { get; private set; }

        private string callingNamespace;

        /// <summary>
        /// The result list of <see cref="RuleValidationResult"/>. Each violated rule while
        /// asserting adds a new entry to this list.
        /// </summary>
        private readonly List<RuleValidationResult> executionResults = new List<RuleValidationResult>();
        private readonly object executionResultsLock = new object();

        protected void AddExecutionResult(RuleValidationResult result)
        {
            lock (this.executionResultsLock)
            {
                executionResults.Add(result);
            }
        }

        protected void AddExecutionResults(IEnumerable<RuleValidationResult> results)
        {
            lock (this.executionResultsLock)
            {
                executionResults.AddRange(results);
            }
        }

        /// <summary>
        /// Gets or sets the "name" of the value - normally this is the name of a method parameter.
        /// </summary>
        private string ValueName { get; set; }

        /// <summary>
        /// Gets or sets the data to be tested.
        /// </summary>
        private TData Value { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ContractMethodRuleAttribute"/> for the current method (the one that did create the 
        /// instance of the <see cref="RuleExecuter{TData,TResultClass}"/>).
        /// </summary>
        private IEnumerable<string> Context { get; set; }

        /// <summary>
        /// The name of the namespace this INSTANCE (the inherited class) is declared in - this may differ is it is a "custom" executor.
        /// </summary>
        private readonly string myNamespace = typeof(TResultClass).Namespace;

        /// <summary>
        /// The root namespace of the classes of this assembly is equal to the name of the assembly.
        /// </summary>
        private static readonly string BouncerNameSpace = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        /// A cache for the attributes of properties.
        /// </summary>
        private static readonly Dictionary<PropertyInfo, IEnumerable<ContractRuleAttribute>> PropertyAttributeCache = new Dictionary<PropertyInfo, IEnumerable<ContractRuleAttribute>>();

        private static readonly object PropertyAttributeCacheSync = new object();

        /// <summary>
        /// A cache for the attributes of methods.
        /// </summary>
        private static readonly Dictionary<MethodBase, List<ContractMethodRuleAttribute>> RuleAttributeCache = new Dictionary<MethodBase, List<ContractMethodRuleAttribute>>();

        private static readonly object RuleAttributeCacheSync = new object();

        private static readonly bool SuppressAll = ConfigReader.GetConfig<BouncerConfiguration>().SuppressAll;

        private readonly Type targetType;

        private readonly object callingNamespaceLock = new object();

        #endregion

        #region ctors

        protected RuleExecuter(Expression<Func<TData>> data, IEnumerable<ContractMethodRuleAttribute> methodRuleAttributes)
            : this(GetMemberName(data), GetMemberValue(data), methodRuleAttributes)
        {
        }

        protected RuleExecuter(string valueName, TData value, IEnumerable<ContractMethodRuleAttribute> methodRuleAttributes)
        {
            if (SuppressAll)
            {
                return;
            }

            var currentMethodInfo = GetCurrentMethodInfo(2);

            this.MethodRuleAttributes = methodRuleAttributes ?? GetRuleAttributesFromCurrentMethod(currentMethodInfo);
            this.Context = GetContext(currentMethodInfo);
            this.Value = value;
            this.ValueName = valueName;
            this.targetType = typeof(TData);
        }

        #endregion

        #region asserts

        /// <summary>
        /// Checks an anonymous rule (built on the fly) with the specified check expression.
        /// </summary>
        /// <param name="rule">The expression that returns a boolean. If that boolean is "false", the rule is "violated".</param>
        /// <returns>The class instance this method belongs to. This way you can invoke multiple methods of this class in one line of code.</returns>
        public TResultClass Assert(Func<TData, bool> rule)
        {
            var ruleClass = new RuleBase<TData, object> { CheckExpression = (data, parameter) => rule.Invoke(data) };
            return this.Assert(ruleClass);
        }

        /// <summary>
        /// Checks a rule.
        /// </summary>
        /// <param name="rule">The rule to be checked.</param>
        /// <returns>The class instance this method belongs to. This way you can invoke multiple methods of this class in one line of code.</returns>
        public TResultClass Assert(RuleBase<TData, object> rule)
        {
            return this.Assert(rule, null);
        }

        /// <summary>
        /// Checks (depending on the inheriting rule executer) a list of rules.
        /// </summary>
        /// <param name="ruleCollection">The set of rules to be checked.</param>
        /// <returns>The class instance this method belongs to. This way you can invoke multiple methods of this class in one line of code.</returns>
        public TResultClass Assert(IEnumerable<RuleBase<TData, object>> ruleCollection)
        {
            return this.Assert(ruleCollection, null);
        }

        /// <summary>
        /// Checks an anonymous rule (build on the fly from an expression) with a parameter.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter for the check.</typeparam>
        /// <param name="rule">The method that returns true or false and accepts the data (of type <see cref="TData"/>) and a check parameter <paramref name="ruleParameter"/>.</param>
        /// <param name="ruleParameter">The parameter for rule checking (the second parameter of the <paramref name="rule"/>).</param>
        /// <returns>The class instance this method belongs to. This way you can invoke multiple methods of this class in one line of code.</returns>
        public TResultClass Assert<TParameter>(Func<TData, TParameter, bool> rule, TParameter ruleParameter)
        {
            var ruleClass = new RuleBase<TData, object> { CheckExpression = (data, parameter) => rule.Invoke(data, ruleParameter) };
            return this.Assert(ruleClass, ruleParameter);
        }

        /// <summary>
        /// Checks a rule with a parameter.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter for the check.</typeparam>
        /// <param name="rule">The rule to be checked.</param>
        /// <param name="ruleParameter">The parameter for rule checking (<see cref="RuleBase{TData,TParameter}.CheckExpression"/>).</param>
        /// <returns>The class instance this method belongs to. This way you can invoke multiple methods of this class in one line of code.</returns>
        public TResultClass Assert<TParameter>(RuleBase<TData, TParameter> rule, TParameter ruleParameter)
        {
            this.ExecuteRuleExpression(rule, ruleParameter, this.ValueName);
            return (TResultClass)this;
        }

        /// <summary>
        /// Checks (depending on the inheriting rule executer) a list of rules.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter object.</typeparam>
        /// <param name="ruleCollection">The set of rules to be checked.</param>
        /// <param name="ruleParameter">The rule parameter object - some rules do need parameters for the check. 
        ///                             E.g. <see cref="IntegerLowerThanRule"/> needs an interger to compare with.</param>
        /// <returns>The class instance this method belongs to. This way you can invoke multiple methods of this class in one line of code.</returns>
        public TResultClass Assert<TParameter>(IEnumerable<RuleBase<TData, TParameter>> ruleCollection, TParameter ruleParameter)
        {
            if (ruleCollection != null)
            {
                foreach (var rule in ruleCollection)
                {
                    this.ExecuteRuleExpression(rule, ruleParameter, this.ValueName);
                }
            }

            return (TResultClass)this;
        }

        /// <summary>
        /// Checks all attribute based rules - rules that have been attached to types, properties of the current method.
        /// </summary>
        /// <returns>The class instance this method belongs to. This way you can invoke multiple methods of this class in one line of code.</returns>
        public TResultClass Assert()
        {
            if (SuppressAll)
            {
                return (TResultClass)this;
            }


            var actions = new List<Action>();
            actions.AddRange(this.AssertForProperties());
            actions.AddRange(this.AssertForMethodAttributes());
            actions.AddRange(this.AssertForType());

            this.GetCallingNamespace();

            foreach (var action in actions)
            {
                action.Invoke();
            }

            return (TResultClass)this;
        }

        public virtual IRuleExecuter AssertAll()
        {
            return this.Assert();
        }

        #endregion

        public virtual IEnumerable<RuleValidationResult> Results
        {
            get
            {
                var results = this.executionResults;

                var previousExecuter = this.PreviousExecuter;
                return
                    previousExecuter != null
                    ? results.Concat(previousExecuter.Results)
                    : results;
            }
        }

        public virtual void AddRange(IEnumerable<RuleValidationResult> results)
        {
            this.AddExecutionResults(results);
        }

        /// <summary>
        /// Evaluates the rule check expression for a rule attribute (property or method attribute).
        /// The attributes of methods and properties need to be executed by reflection, so we need a 
        /// public method for that purpose.
        /// </summary>
        /// <param name="ruleExecuter">The rule executer (inherits from <see cref="RuleExecuter{TData,TResultClass}"/>) that will invoke the rule validation.</param>
        /// <param name="ruleAttribute">The attribute that defines the rule.</param>
        /// <param name="propertyName">The name of the data to be validated by the rule.</param>
        /// <returns>A new instance of <see cref="RuleValidationResult"/>.</returns>
        RuleValidationResult IRuleExecuter.InvokeRuleExecutionForAttribute(IRuleExecuter ruleExecuter, ContractRuleBaseAttribute ruleAttribute, string propertyName)
        {
            return this.InvokeRuleExecutionForAttribute(ruleExecuter, ruleAttribute, propertyName);
        }

        protected RuleValidationResult InvokeRuleExecutionForAttribute(IRuleExecuter ruleExecuter, ContractRuleBaseAttribute ruleAttribute, string propertyName)
        {
            if (ruleAttribute == null || ruleExecuter == null)
            {
                return null;
            }

            var typeAttributeRuleType = ruleAttribute.RuleType;
            if (typeAttributeRuleType.Implements(typeof(IEnumerable)))
            {
                return null;
            }

            // the following line would be more specific - but seems to be hard to be implemented
            ////if (!ruleAttribute.Type.Implements(typeof(RuleBase<,>)))
            if (!typeAttributeRuleType.IsSubclassOf(typeof(RuleBaseInformation)))
            {
                throw new ArgumentException("The attribute does not contain a valid rule.");
            }

            var assertMethod = ruleExecuter.GetType().GetMethod("ExecuteRuleExpression", DefaultBindingFlags);

            var parameter = ruleAttribute.Parameter;

            assertMethod = assertMethod.MakeGenericMethod(parameter != null ? parameter.GetType() : typeof(object));

            try
            {
                // create an instance of the rule and invoke the Assert statement
                var rule = typeAttributeRuleType.CreateRule(ruleExecuter.ValueType);
                rule.Exception = ruleAttribute.ExceptionType;

                if (!string.IsNullOrEmpty(ruleAttribute.Message))
                {
                    rule.Message = ruleAttribute.Message;
                }

                var invokeResult = (bool)assertMethod.Invoke(ruleExecuter, new[] { rule, parameter, propertyName });

                var ruleType = rule.GetType();
                var result = new RuleValidationResult(ruleType, string.Format(CultureInfo.CurrentCulture, Resources.RuleValidationResultStandardMessage, ruleType.Namespace + "." + ruleType.Name, propertyName, string.Format(CultureInfo.CurrentCulture, rule.Message, parameter, propertyName)), propertyName, invokeResult);

                return result;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        Type IRuleExecuter.ValueType
        {
            get
            {
                return this.ValueType;
            }
        }

        protected Type ValueType
        {
            get
            {
                return targetType;
            }
        }

        /// <summary>
        /// Strong typed rule invocation.
        /// </summary>
        /// <typeparam name="TParameter">The type of the rule parameter.</typeparam>
        /// <param name="rule">The rule to be evaluated.</param>
        /// <param name="ruleParameter">The parameter for invoking the rule.</param>
        /// <param name="valueName">The name of the data to be checked.</param>
        /// <returns>True if the rule check is ok, false if the rule is violated.</returns>
        internal bool ExecuteRuleExpression<TParameter>(RuleBase<TData, TParameter> rule, TParameter ruleParameter, string valueName)
        {
            // if there is no rule, we cannot say that the rule is validated
            if (rule == null || SuppressAll)
            {
                return true;
            }

            // let the concrete execution class decide if we want to execute the expression
            if (!this.BeforeInvoke(rule, ruleParameter, valueName))
            {
                return true;
            }

            var validationResult = false;
            try
            {
                // execute the expression
                validationResult = rule.CheckExpression(this.Value, ruleParameter);
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception ex)
            {
                if (!this.HandleInvokeException(ex, rule, ruleParameter, valueName, this.Value))
                {
                    throw;
                }
            }

            var ruleType = rule.GetType();
            var result = new RuleValidationResult(
                ruleType,
                string.Format(CultureInfo.CurrentCulture, Resources.RuleValidationResultStandardMessage, ruleType.Namespace + "." + ruleType.Name, valueName, string.Format(CultureInfo.CurrentCulture, rule.Message, ruleParameter, valueName)),
                valueName,
                validationResult);

            if (!result.SkipProcessing)
            {
                foreach (var action in BouncerConfiguration.GetAfterInvokeActions())
                {
                    action.Invoke(result);
                }
            }

            result.Exception =
                rule.Exception != null
                ? (Exception)rule.Exception.GetConstructor(Type.EmptyTypes).Invoke(new object[] { })
                : new RuleValidationException(result.RuleType, result.Message, result.ValueName);

            this.AfterInvoke(result);

            if (this.PreviousExecuter != null)
            {
                this.PreviousExecuter.AssertAll();
            }

            return validationResult;
        }

        #region overridables

        /// <summary>
        /// The action to be done for each rule validation result. This method must be overridden with the
        /// logic, the inheriting class should implement - e.g. throwing an exception, adding messages 
        /// to a list or log etc. 
        /// </summary>
        /// <param name="validationResult">The result of a rule validation.</param>
        protected abstract void AfterInvoke(RuleValidationResult validationResult);

        /// <summary>
        /// This method will be invoked before a rule is validated. Overriding classes can return "false" in order to
        /// prevent execution of this rule.
        /// </summary>
        /// <typeparam name="TParameter">The type of the rule parameter.</typeparam>
        /// <param name="rule">The rule that will be executed.</param>
        /// <param name="ruleParameter">The parameter of the rule.</param>
        /// <param name="valueName">The name of the data.</param>
        /// <returns>True if the rule validation should be executed, false if the validation should be prevented (this rule will be skipped).</returns>
        protected virtual bool BeforeInvoke<TParameter>(RuleBase<TData, TParameter> rule, object ruleParameter, string valueName)
        {
            return true;
        }

        /// <summary>
        /// This method will be called in case of an exception while evaluating the rule. Such exceptions will be caught to be
        /// able to build a full list of result entries for a validation (<see cref="MessageCollector{TData}"/>).
        /// </summary>
        /// <typeparam name="TParameter"> The type of the rule parameter. </typeparam>
        /// <param name="ex"> The exception that has been thrown while executing the validation. </param>
        /// <param name="rule"> The rule that did throw the exception. </param>
        /// <param name="ruleParameter"> The parameter data involved in the rule validation. </param>
        /// <param name="valueName"> The name of the data to be evaluated. </param>
        /// <param name="value"> The value to be evaluated. </param>
        /// <returns> True if the exception has been handled inside this method, false if the exception should be rethrown in the base class. </returns>
        protected virtual bool HandleInvokeException<TParameter>(Exception ex, RuleBase<TData, TParameter> rule, object ruleParameter, string valueName, object value)
        {
            return false;
        }

        #endregion

        /// <summary>
        /// Gets the role attributes (a list of <see cref="ContractMethodRuleAttribute"/>) from the current method. The
        /// current method is the first method inside the stack trace from a class, that is not inside the 
        /// namespace of <see cref="Bouncer"/> and not inside a class implementing an interface called "IRuleExecuter".
        /// </summary>
        /// <param name="methodInfo">The reflected information about the calling method.</param>
        /// <returns>A list of <see cref="ContractMethodRuleAttribute"/> with all attributes of the current method.</returns>
        private static IEnumerable<ContractMethodRuleAttribute> GetRuleAttributesFromCurrentMethod(MethodBase methodInfo)
        {
            lock (RuleAttributeCacheSync)
            {
                if (!RuleAttributeCache.ContainsKey(methodInfo))
                {
                    var customAttributes = methodInfo.GetCustomAttributes(typeof(ContractMethodRuleAttribute), true);
                    var methodRuleAttributes = (from x in customAttributes select (ContractMethodRuleAttribute)x).ToList();

                    var newRules = new List<ContractMethodRuleAttribute>();
                    foreach (var methodRuleAttribute in methodRuleAttributes)
                    {
                        if (!methodRuleAttribute.RuleType.Implements(typeof(IEnumerable)))
                        {
                            continue;
                        }

                        var ruleCollection = (IEnumerable)methodRuleAttribute.RuleType.GetConstructor(new Type[] { }).Invoke(null);
                        var attribute = methodRuleAttribute;
                        newRules.AddRange(from object rule in ruleCollection select new ContractMethodRuleAttribute(rule.GetType(), attribute.MethodArgumentName) { Namespace = attribute.Namespace, IncludeInContext = attribute.IncludeInContext, Message = attribute.Message, Parameter = attribute.Parameter });
                    }

                    methodRuleAttributes.AddRange(newRules);
                    RuleAttributeCache.Add(methodInfo, methodRuleAttributes);
                }

                return RuleAttributeCache[methodInfo];
            }
        }

        private static string GetMemberName(Expression<Func<TData>> data)
        {
            if (data == null)
            {
                return "null value";
            }

            var member = data.Body as MemberExpression;
            return member != null ? member.Member.Name : "anonymous value";
        }

        private static TData GetMemberValue(Expression<Func<TData>> data)
        {
            try
            {
                if (data == null)
                {
                    return default(TData);
                }

                var exprBody = data.Body as MemberExpression;
                if (exprBody != null)
                {
                    var exprConst = exprBody.Expression as ConstantExpression;
                    if (exprConst != null)
                    {
                        return (TData)((FieldInfo)exprBody.Member).GetValue(exprConst.Value);
                    }
                }

                return data.Compile().Invoke();
            }
            catch (NullReferenceException)
            {
                // suppress null reference exceptions while invoking the
                // expression - in this case one member of the property 
                // paths is null and we simply assume the result to be
                // null, too.
                return default(TData);
            }
        }

        private static MethodBase GetCurrentMethodInfo(int skipFrames)
        {
            var stack = new StackTrace(skipFrames, false);
            var methodInfo = stack.GetFrame(0).GetMethod();

            for (var i = 0; i < stack.FrameCount; i++)
            {
                var declaringType = methodInfo.DeclaringType;
                if (declaringType.Namespace != null && !declaringType.Namespace.StartsWith(BouncerNameSpace, StringComparison.Ordinal) && declaringType.GetInterface("IRuleExecuter", false) == null)
                {
                    break;
                }

                methodInfo = stack.GetFrame(i).GetMethod();
            }

            return methodInfo;
        }

        private static IEnumerable<string> GetContext(MethodBase currentMethodInfo)
        {
            var attributes = new List<string>();

            AddContextInfo(currentMethodInfo.DeclaringType.GetCustomAttributes(typeof(ContractContextAttribute), true), attributes);
            AddContextInfo(currentMethodInfo.GetCustomAttributes(typeof(ContractContextAttribute), true), attributes);

            return attributes;
        }

        private static void AddContextInfo(object[] typeAttribs, List<string> attributes)
        {
            foreach (ContractContextAttribute typeAttrib in typeAttribs)
            {
                var context = typeAttrib.Context;

                if (typeAttrib.Active)
                {
                    attributes.Add(context);
                    continue;
                }

                if (attributes.Contains(context))
                {
                    attributes.RemoveAll(x => x == context);
                }
            }
        }

        private static IEnumerable<ContractRuleAttribute> GetPropertyRuleAttributes(Type targetType, PropertyInfo info)
        {
            IEnumerable<ContractRuleAttribute> ruleAttributes;
            lock (PropertyAttributeCacheSync)
            {
                if (!PropertyAttributeCache.ContainsKey(info))
                {
                    var customAttributes = from x in info.GetCustomAttributes(typeof(ContractRuleAttribute), true) select x as ContractRuleAttribute;
                    var configuredRules = BouncerConfiguration.GetConfiguredRules(info, targetType);
                    PropertyAttributeCache.Add(info, customAttributes.Concat(configuredRules));
                }

                ruleAttributes = PropertyAttributeCache[info];
            }

            return ruleAttributes;
        }

        /// <summary>
        /// Performs a stack trace frame walk to find the first frame, that does not match
        /// the namespace of this class.
        /// </summary>
        /// <returns>the calling namespace name</returns>
        private string GetCallingNamespace()
        {
            if (string.IsNullOrEmpty(this.callingNamespace))
            {
                lock (this.callingNamespaceLock)
                {
                    if (string.IsNullOrEmpty(this.callingNamespace))
                    {
                        var stackTrace = new StackTrace(false);
                        var stackFrames = stackTrace.GetFrames();
                        if (stackFrames != null)
                        {
                            foreach (var stackFrame in stackFrames)
                            {
                                var name = stackFrame.GetMethod().DeclaringType.Namespace;

                                if (name == null
                                    || name.StartsWith(this.myNamespace, StringComparison.Ordinal)
                                    || name.StartsWith("System."))
                                {
                                    continue;
                                }

                                this.callingNamespace = name;
                                break;
                            }
                        }
                    }
                }
            }

            return this.callingNamespace;
        }

        /// <summary>
        /// Creates and initializes a rule executor of this type using a generic parameter of <paramref name="valueType"/>.
        /// </summary>
        /// <param name="valueType">The type of the value that should be checked.</param>
        /// <param name="name">The name of the value that should be checked.</param>
        /// <param name="value">The value that should be checked.</param>
        /// <returns>A new instance of the rule executer.</returns>
        private IRuleExecuter CreateRuleExecuter(Type valueType, string name, object value)
        {
            var type = this.GetType();
            var genericTypeDefinition = type.GetGenericTypeDefinition();
            var makeGenericType = genericTypeDefinition.MakeGenericType(valueType);
            var constructorInfo = makeGenericType.GetConstructor(new[] { typeof(string), valueType, typeof(IEnumerable<ContractMethodRuleAttribute>) });
            return constructorInfo.Invoke(new[] { name, value, this.MethodRuleAttributes }) as IRuleExecuter;
        }

        /// <summary>
        /// Checks the rules attached to the type of <see cref="TData"/>.
        /// </summary>
        private IEnumerable<Action> AssertForType()
        {
            return new Action[] { () => this.Assert(RegisteredRules.GetRulesForType<TData, object>()) };
        }

        /// <summary>
        /// Checks the rules of the current method that do match to the <see cref="ValueName"/>.
        /// </summary>
        private IEnumerable<Action> AssertForMethodAttributes()
        {
            var ruleAttributes = from methodAttribute in this.MethodRuleAttributes where methodAttribute.MethodArgumentName == this.ValueName select methodAttribute;

            // first we need to construct a new rule executer type, unfortunately this is a generic type, 
            // so we need to do it via reflection (we don't have the type of the property at design time)
            var ruleExecuter = this.CreateRuleExecuter(this.targetType, this.ValueName, this.Value);
            return this.InvokeForAllAttributes(ruleExecuter, ruleAttributes, this.ValueName);
        }

        /// <summary>
        /// Checks the rules attached to the properties of the <see cref="TData"/>.
        /// </summary>
        private IEnumerable<Action> AssertForProperties()
        {
            // preallocate the calling namespace - this would  not be accessible from the worker 
            // threads of the parallel foreach.
            this.GetCallingNamespace();

            return from propertyInfo in this.targetType.GetProperties()
                   select new Action(
                               () =>
                               {
                                   var ruleAttributes = GetPropertyRuleAttributes(this.targetType, propertyInfo);
                                   if (ruleAttributes.Count() == 0)
                                   {
                                       return;
                                   }

                                   var propertyName = this.ValueName + "." + propertyInfo.Name;
                                   var propertyValue = !this.targetType.IsValueType && Equals(this.Value, null) ? null : propertyInfo.GetValue(this.Value, null);

                                   // first we need to construct a new rule executer type, unfortunately this is a generic type, 
                                   // so we need to do it via reflection (we don't have the type of the property at design time)
                                   var ruleExecuter = this.CreateRuleExecuter(propertyInfo.PropertyType, propertyName, propertyValue);
                                   foreach (var action in this.InvokeForAllAttributes(ruleExecuter, ruleAttributes, propertyName))
                                   {
                                       action.Invoke();
                                   } 
                               });
        }

        private IEnumerable<Action> InvokeForAllAttributes(IRuleExecuter ruleExecuter, IEnumerable<ContractRuleBaseAttribute> ruleAttributes, string propertyName)
        {
            return from ruleAttribute in ruleAttributes
                   select new Action(() =>

                                // now enumerate the attributes of the property (there might be more than one)
                                ////foreach (var ruleAttribute in ruleAttributes)
                                {
                                    // here we filter by namespace
                                    var namespaceFilter = ruleAttribute.Namespace;
                                    if (!string.IsNullOrEmpty(namespaceFilter))
                                    {
                                        var name = this.GetCallingNamespace();
                                        if (!(name == namespaceFilter || name.StartsWith(namespaceFilter, StringComparison.Ordinal)))
                                        {
                                            return;
                                        }
                                    }

                                    // here we filter by context
                                    var contextFilter = ruleAttribute.IncludeInContext;
                                    if (!string.IsNullOrEmpty(contextFilter))
                                    {
                                        if ((from x in this.Context where x == contextFilter select x).Count() == 0)
                                        {
                                            return;
                                        }
                                    }

                                    var result = this.InvokeRuleExecutionForAttribute(ruleExecuter, ruleAttribute, propertyName);

                                    // we need to explicitly call "AfterInvoke" here, because "ruleExecuter" is not euqal to this,
                                    // so the call of "AfterInvoke" while rule execution does update a different instance of RuleExecuter.
                                    if (result != null && !result.SkipProcessing)
                                    {
                                        this.AfterInvoke(result);
                                    }
                                });
        }
    }
}