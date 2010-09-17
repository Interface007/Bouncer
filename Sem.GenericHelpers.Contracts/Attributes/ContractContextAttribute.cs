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
        public ContractContextAttribute(string context)
            : this(context, true)
        {
        }

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