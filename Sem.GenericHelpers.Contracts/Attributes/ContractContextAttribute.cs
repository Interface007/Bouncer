// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractContextAttribute.cs" company="Sven Erik Matzen">
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
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ContractContextAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractContextAttribute"/> class.
        /// </summary>
        /// <param name="context"> The context in which the rule should be validated - use string.Empty to always validate. </param>
        public ContractContextAttribute(string context)
            : this(context, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractContextAttribute"/> class. 
        /// Using this ctor you might disable rules in a specific context.
        /// </summary>
        /// <param name="context"> The context in which the rule should be validated - use string.Empty to always validate.  </param>
        /// <param name="active"> Specifies whether the rule should be active in this context. </param>
        public ContractContextAttribute(string context, bool active)
        {
            this.Context = context;
            this.Active = active;
        }

        /// <summary>
        /// Gets the context value to be set or reset.
        /// </summary>
        public string Context { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this context value should be switched on (=true) or off (=false).
        /// </summary>
        public bool Active { get; private set; }
    }
}