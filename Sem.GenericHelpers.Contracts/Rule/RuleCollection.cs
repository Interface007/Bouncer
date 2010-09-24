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

    public abstract class RuleCollection<TData, TParameter> : IEnumerable<RuleBase<TData, TParameter>>
    {
        public IEnumerator<RuleBase<TData, TParameter>> GetEnumerator()
        {
            return this.GetRuleList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Generation of the rule list might be complex logic - properties should not include complex logic, so a property is not appropriate.")]
        protected abstract IEnumerable<RuleBase<TData, TParameter>> GetRuleList();
    }
}
