// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleCollection.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleCollection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rule
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a base class for a collection of rules.
    /// </summary>
    /// <typeparam name="TData">The data type that will be validated by this rule set</typeparam>
    /// <typeparam name="TParameter">The parameter type to invoke the rules.</typeparam>
    public abstract class RuleCollection<TData, TParameter> : IEnumerable<RuleBase<TData, TParameter>>
    {
        /// <summary>
        /// Returns an enumerator for the list.
        /// </summary>
        /// <returns> The enumerator for the rule set </returns>
        public IEnumerator<RuleBase<TData, TParameter>> GetEnumerator()
        {
            return this.GetRuleList().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator for the list.
        /// </summary>
        /// <returns> The enumerator for the rule set </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Abstract method to enable the inheriting classes to provide an enumerator.
        /// </summary>
        /// <returns> The enumerator for the rule set </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Generation of the rule list might be complex logic - properties should not include complex logic, so a property is not appropriate.")]
        protected abstract IEnumerable<RuleBase<TData, TParameter>> GetRuleList();
    }
}
