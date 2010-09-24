// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodRuleAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Attribute to attach rules to methods. To attach rules to classes and properties,
//   use the <see cref="ContractRuleAttribute" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Attributes
{
    using System;

    /// <summary>
    /// Attribute to attach rules to methods. To attach rules to classes and properties, 
    /// use the <see cref="ContractRuleAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ContractMethodRuleAttribute : ContractRuleBaseAttribute
    {
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
}