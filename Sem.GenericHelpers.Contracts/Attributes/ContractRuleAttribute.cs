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
    /// use the <see cref="MethodRuleAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ContractRuleAttribute : ContractRuleBaseAttribute
    {
        public ContractRuleAttribute(Type ruleType)
            : base(ruleType)
        {
        }
    }
}
