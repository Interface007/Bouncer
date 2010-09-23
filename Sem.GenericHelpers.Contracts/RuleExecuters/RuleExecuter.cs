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

    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Configuration;
    using Sem.GenericHelpers.Contracts.Properties;
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
        #region data

        /// <summary>
        /// Gets or sets the "name" of the value - normally this is the name of a method parameter.
        /// </summary>
        public string ValueName { get; internal set; }

        /// <summary>
        /// Gets or sets the data to be tested.
        /// </summary>
        public TData Value { get; internal set; }

        /// <summary>
        /// Gets or sets a pointer to the Assert()-method of the previously built <see cref="RuleExecuter{TData,TResultClass}"/>.
        /// Only the Assert()-method can be invoked, because all other Assert methods do rely on the data type, 
        /// which may differ from the current.
        /// </summary>
        internal IRuleExecuter PreviousExecuter { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="MethodRuleAttribute"/> for the current method (the one that did create the 
        /// instance of the <see cref="RuleExecuter{TData,TResultClass}"/>).
        /// </summary>
        public IEnumerable<MethodRuleAttribute> MethodRuleAttributes { get; internal set; }

        /// <summary>
        /// Gets or sets a list of <see cref="MethodRuleAttribute"/> for the current method (the one that did create the 
        /// instance of the <see cref="RuleExecuter{TData,TResultClass}"/>).
        /// </summary>
        internal IEnumerable<string> Context { get; set; }

        /// <summary>
        /// The name of the namespace this INSTANCE (the inherited class) is declared in.
        /// </summary>
        private readonly string myNamespace = typeof(TResultClass).Namespace;

        /// <summary>
        /// The namespace of the <see cref="Bouncer"/> class.
        /// </summary>
        private static readonly string BouncerNameSpace = typeof(Bouncer).Namespace;

        /// <summary>
        /// A cache for the attributes of properties.
        /// </summary>
        private static readonly Dictionary<PropertyInfo, IEnumerable<ContractRuleAttribute>> PropertyAttributeCache = new Dictionary<PropertyInfo, IEnumerable<ContractRuleAttribute>>();
        private static readonly object PropertyAttributeCacheSync = new object();

        /// <summary>
        /// A cache for the attributes of methods.
        /// </summary>
        private static readonly Dictionary<MethodBase, List<MethodRuleAttribute>> RuleAttributeCache = new Dictionary<MethodBase, List<MethodRuleAttribute>>();
        private static readonly object RuleAttributeCacheSync = new object();

        private static readonly bool SuppressAll = ConfigReader.GetConfig<BouncerConfiguration>().SuppressAll;

        private readonly Type targetType;

        #endregion

        #region ctors
        protected RuleExecuter(string valueName, TData value, IEnumerable<MethodRuleAttribute> methodRuleAttributes)
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
            targetType = typeof(TData);
        }

        protected RuleExecuter(Expression<Func<TData>> data, IEnumerable<MethodRuleAttribute> methodRuleAttributes)
            : this(GetMemberName(data), GetMemberValue(data), methodRuleAttributes)
        {
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

            this.AssertForProperties();
            this.AssertForMethodAttributes();
            this.AssertForType();
            
            return (TResultClass)this;
        }

        public virtual IRuleExecuter AssertAll()
        {
            return this.Assert();
        }
        #endregion

        /// <summary>
        /// Evaluates the rule check expression for a rule attribute (property or method attribute).
        /// The attributes of methods and properties need to be executed by reflection, so we need a 
        /// public method for that purpose.
        /// </summary>
        /// <param name="ruleExecuter">The rule executer (inherits from <see cref="RuleExecuter{TData,TResultClass}"/>) that will invoke the rule validation.</param>
        /// <param name="ruleAttribute">The attribute that defines the rule.</param>
        /// <param name="propertyName">The name of the data to be validated by the rule.</param>
        /// <returns>A new instance of <see cref="RuleValidationResult"/>.</returns>
        public RuleValidationResult InvokeRuleExecutionForAttribute(IRuleExecuter ruleExecuter, ContractRuleBaseAttribute ruleAttribute, string propertyName)
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

            var assertMethod = ruleExecuter.GetType().GetMethod("ExecuteRuleExpression");

            var parameter = ruleAttribute.Parameter;

            assertMethod = assertMethod.MakeGenericMethod(
                    parameter != null
                        ? parameter.GetType()
                        : typeof(object));

            try
            {
                // create an instance of the rule and invoke the Assert statement
                var rule = typeAttributeRuleType.CreateRule(ruleExecuter.GetValueType());

                var ruleType = rule.GetType();
                if (!string.IsNullOrEmpty(ruleAttribute.Message))
                {
                    rule.Message = ruleAttribute.Message;
                }

                var invokeResult = (bool)assertMethod.Invoke(ruleExecuter, new[] { rule, parameter, propertyName });

                var result = new RuleValidationResult(
                    ruleType,
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.RuleValidationResultStandardMessage,
                        ruleType.Namespace + "." + ruleType.Name,
                        propertyName,
                        string.Format(CultureInfo.CurrentCulture, rule.Message, parameter, propertyName)),
                    propertyName,
                    invokeResult);

                return result;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        public Type GetValueType()
        {
            return this.targetType;
        }

        /// <summary>
        /// Strong typed rule invocation.
        /// </summary>
        /// <typeparam name="TParameter">The type of the rule parameter.</typeparam>
        /// <param name="rule">The rule to be evaluated.</param>
        /// <param name="ruleParameter">The parameter for invoking the rule.</param>
        /// <param name="valueName">The name of the data to be checked.</param>
        /// <returns>True if the rule check is ok, false if the rule is violated.</returns>
        public virtual bool ExecuteRuleExpression<TParameter>(RuleBase<TData, TParameter> rule, TParameter ruleParameter, string valueName)
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
                string.Format(
                    CultureInfo.CurrentCulture,
                    Resources.RuleValidationResultStandardMessage,
                    ruleType.Namespace + "." + ruleType.Name,
                    valueName,
                    string.Format(CultureInfo.CurrentCulture, rule.Message, ruleParameter, valueName)),
                valueName,
                validationResult);

            foreach (var action in Bouncer.GetAfterInvokeActions())
            {
                if (!result.SkipProcessing)
                {
                    action.Invoke(result);
                }
            }

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
        /// <typeparam name="TParameter">The type of the rule parameter.</typeparam>
        /// <param name="ex">The exception that has been thrown while executing the validation.</param>
        /// <param name="rule">The rule that did throw the exception.</param>
        /// <param name="ruleParameter">The parameter data involved in the rule validation.</param>
        /// <param name="valueName">The name of the data to be evaluated.</param>
        /// <returns>True if the exception has been handled inside this method, false if the exception should be rethrown in the base class.</returns>
        protected virtual bool HandleInvokeException<TParameter>(Exception ex, RuleBase<TData, TParameter> rule, object ruleParameter, string valueName, object value)
        {
            return false;
        }

        #endregion

        /// <summary>
        /// Gets the role attributes (a list of <see cref="MethodRuleAttribute"/>) from the current method. The
        /// current method is the first method inside the stack trace from a class, that is not inside the 
        /// namespace of <see cref="Bouncer"/> and not inside a class implementing an interface called "IRuleExecuter".
        /// </summary>
        /// <param name="methodInfo">The reflected information about the calling method.</param>
        /// <returns>A list of <see cref="MethodRuleAttribute"/> with all attributes of the current method.</returns>
        private static IEnumerable<MethodRuleAttribute> GetRuleAttributesFromCurrentMethod(MethodBase methodInfo)
        {
            lock (RuleAttributeCacheSync)
            {
                if (!RuleAttributeCache.ContainsKey(methodInfo))
                {
                    var customAttributes = methodInfo.GetCustomAttributes(typeof(MethodRuleAttribute), true);
                    var methodRuleAttributes = (from x in customAttributes select (MethodRuleAttribute)x).ToList();

                    var newRules = new List<MethodRuleAttribute>();
                    foreach (var methodRuleAttribute in methodRuleAttributes)
                    {
                        if (!methodRuleAttribute.RuleType.Implements(typeof(IEnumerable)))
                        {
                            continue;
                        }

                        var ruleCollection = (IEnumerable)methodRuleAttribute.RuleType.GetConstructor(new Type[] { }).Invoke(null);
                        var attribute = methodRuleAttribute;
                        newRules.AddRange(
                            from object rule in ruleCollection 
                            select new MethodRuleAttribute(rule.GetType(), attribute.MethodArgumentName)
                                {
                                    Namespace = attribute.Namespace, 
                                    IncludeInContext = attribute.IncludeInContext, 
                                    Message = attribute.Message, 
                                    Parameter = attribute.Parameter
                                });
                    }

                    methodRuleAttributes.AddRange(newRules);
                    RuleAttributeCache.Add(methodInfo, methodRuleAttributes);
                }

                return RuleAttributeCache[methodInfo];
            }
        }

        private static string GetMemberName(Expression<Func<TData>> data)
        {
            var member = data.Body as MemberExpression;
            return member != null
                       ? member.Member.Name
                       : "anonymous value";
        }

        private static TData GetMemberValue(Expression<Func<TData>> data)
        {
            try
            {
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
                methodInfo = stack.GetFrame(i).GetMethod();
                var declaringType = methodInfo.DeclaringType;
                if (declaringType.Namespace != null
                    && !declaringType.Namespace.StartsWith(BouncerNameSpace, StringComparison.Ordinal)
                    && declaringType.GetInterface("IRuleExecuter", false) == null)
                {
                    break;
                }
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

        /// <summary>
        /// Performs a stack trace frame walk to find the first frame, that does not match
        /// the namespace of this class.
        /// </summary>
        /// <returns>the calling namespace name</returns>
        private string GetCallingNamespace()
        {
            var stackTrace = new StackTrace(false);
            var stackFrames = stackTrace.GetFrames();
            var name = string.Empty;
            if (stackFrames != null)
            {
                foreach (var stackFrame in stackFrames)
                {
                    var callingNamespace = stackFrame.GetMethod().DeclaringType.Namespace;

                    if (callingNamespace != null && !callingNamespace.StartsWith(this.myNamespace, StringComparison.Ordinal))
                    {
                        name = callingNamespace;
                        break;
                    }
                }
            }

            return name;
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
            return this.GetType()
                .GetGenericTypeDefinition()
                .MakeGenericType(valueType)
                .GetConstructor(new[] { typeof(string), valueType, typeof(IEnumerable<MethodRuleAttribute>) })
                .Invoke(new[] { name, value, this.MethodRuleAttributes }) as IRuleExecuter;
        }

        /// <summary>
        /// Checks the rules attached to the type of <see cref="TData"/>.
        /// </summary>
        private void AssertForType()
        {
            this.Assert(RegisteredRules.GetRulesForType<TData, object>());
        }

        /// <summary>
        /// Checks the rules of the current method that do match to the <see cref="ValueName"/>.
        /// </summary>
        private void AssertForMethodAttributes()
        {
            var ruleAttributes =
                from methodAttribute in this.MethodRuleAttributes
                where methodAttribute.MethodArgumentName == this.ValueName
                select methodAttribute;

            // first we need to construct a new rule executer type, unfortunately this is a generic type, 
            // so we need to do it via reflection (we don't have the type of the property at design time)
            var ruleExecuter = this.CreateRuleExecuter(this.targetType, this.ValueName, this.Value);
            InvokeForAllAttributes(ruleExecuter, ruleAttributes, this.ValueName);
        }

        /// <summary>
        /// Checks the rules attached to the properties of the <see cref="TData"/>.
        /// </summary>
        private void AssertForProperties()
        {
            foreach (var propertyInfo in this.targetType.GetProperties())
            {
                var ruleAttributes = GetPropertyRuleAttributes(this.targetType, propertyInfo);
                if (ruleAttributes.Count() == 0)
                {
                    continue;
                }

                var propertyName = this.ValueName + "." + propertyInfo.Name;
                var propertyValue = !this.targetType.IsValueType && Equals(this.Value, null) ? null : propertyInfo.GetValue(this.Value, null);
                
                // first we need to construct a new rule executer type, unfortunately this is a generic type, 
                // so we need to do it via reflection (we don't have the type of the property at design time)
                var ruleExecuter = this.CreateRuleExecuter(propertyInfo.PropertyType, propertyName, propertyValue);
                InvokeForAllAttributes(ruleExecuter, ruleAttributes, propertyName);
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

        private void InvokeForAllAttributes(IRuleExecuter ruleExecuter, IEnumerable<ContractRuleBaseAttribute> ruleAttributes, string propertyName)
        {
            // now enumerate the attributes of the property (there might be more than one)
            foreach (var ruleAttribute in ruleAttributes)
            {
                // here we filter by namespace
                var namespaceFilter = ruleAttribute.Namespace;
                if (!string.IsNullOrEmpty(namespaceFilter))
                {
                    var callingNamespace = this.GetCallingNamespace();
                    if (!(callingNamespace == namespaceFilter || callingNamespace.StartsWith(namespaceFilter, StringComparison.Ordinal)))
                    {
                        continue;
                    }
                }

                // here we filter by context
                var contextFilter = ruleAttribute.IncludeInContext;
                if (!string.IsNullOrEmpty(contextFilter))
                {
                    if ((from x in this.Context where x == contextFilter select x).Count() == 0)
                    {
                        continue;
                    }
                }

                var result = this.InvokeRuleExecutionForAttribute(ruleExecuter, ruleAttribute, propertyName);

                // we need to explicitly call "AfterInvoke" here, because "ruleExecuter" is not euqal to this,
                // so the call of "AfterInvoke" while rule execution does update a different instance of RuleExecuter.
                if (result != null && !result.SkipProcessing)
                {
                    this.AfterInvoke(result);
                }
            }
        }
    }
}
