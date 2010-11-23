// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractRuleBaseAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContractRuleAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Attributes
{
    using System;

    using Sem.GenericHelpers.Contracts.Rule;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// Attribute to attach rules to classes and properties of classes. To attach rules to method parameters, 
    /// use the <see cref="ContractMethodRuleAttribute"/>.
    /// </summary>
    public abstract class ContractRuleBaseAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractRuleBaseAttribute"/> class.
        /// </summary>
        /// <param name="ruleType"> The rule type. </param>
        protected ContractRuleBaseAttribute(Type ruleType)
        {
            this.RuleType = ruleType;
        }

        /// <summary>
        /// Gets the type of the rule. This must inherit from <see cref="RuleBase{TData,TParameter}"/>.
        /// </summary>
        public Type RuleType { get; private set; }

        /// <summary>
        /// Gets or sets the type of the exception, this rule might throw.
        /// </summary>
        public Type ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the parameter value of the rule. Many rules do have parameters like the <see cref="StringRegexMatchRule"/>,
        /// which expects a regular expression as the parameter.
        /// </summary>
        public object Parameter { get; set; }

        /// <summary>
        /// Gets or sets the namespace this rule will be executed in. The method calling <see cref="Bouncer"/> must reside 
        /// under this namespace in order to execute this rule. If this property is not set, the rule will be executed in 
        /// any namespace. If this property is set to "MyProg.Test", the rule will be executed in the class
        /// "MyProg.Test.SampleClass" as well as in "MyProg.TestMe.MyClass" and "MyProg.Test.SubSpace.MyClass". If you want 
        /// the rule not to be executed in "MyProg.TestMe.MyClass", you can use "MyProg.Test." as the Namespace property.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the message that will override the rule default message.
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Gets or sets the context string which must be available for the execution. Use the <see cref="ContractContextAttribute"/> to
        /// specify a context as type or method level.
        /// </summary>
        public string IncludeInContext { get; set; }
    }
}
