// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractMethodRuleAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Attribute to attach rules to methods. To attach rules to classes and properties,
//   use the .
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Attributes
{
    using System;

    /// <summary>
    /// Attribute to attach rules to methods. To attach rules to classes and properties, 
    /// use the <see cref="ContractRuleAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ContractMethodRuleAttribute : ContractRuleBaseAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractMethodRuleAttribute"/> class. You can specify what 
        /// parameter of the method should be checked with what rule.
        /// </summary>
        /// <param name="ruleType"> The rule type. </param>
        /// <param name="methodArgumentName"> The method argument name. </param>
        public ContractMethodRuleAttribute(Type ruleType, string methodArgumentName)
            : base(ruleType)
        {
            this.MethodArgumentName = methodArgumentName;
        }

        /// <summary>
        /// Gets the name of the method argument that should be checked with this rule.
        /// </summary>
        public string MethodArgumentName { get; private set; }
    }

    /// <summary>
    /// Attribute to attach rules to methods. To attach rules to classes and properties, 
    /// use the <see cref="ContractRuleAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public sealed class ContractParameterRuleAttribute : ContractRuleBaseAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractMethodRuleAttribute"/> class. You can specify what 
        /// parameter of the method should be checked with what rule.
        /// </summary>
        /// <param name="ruleType"> The rule type. </param>
        public ContractParameterRuleAttribute(Type ruleType)
            : base(ruleType)
        {
        }
    }
}