// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractRuleAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContractRuleAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Attributes
{
    using System;

    /// <summary>
    /// Attribute to attach rules to classes and properties of classes. To attach rules to method parameters, 
    /// use the <see cref="ContractMethodRuleAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class ContractRuleAttribute : ContractRuleBaseAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractRuleAttribute"/> class. 
        /// </summary>
        /// <param name="ruleType">Type of the rule to be executed. </param>
        public ContractRuleAttribute(Type ruleType)
            : base(ruleType)
        {
        }
    }
}
