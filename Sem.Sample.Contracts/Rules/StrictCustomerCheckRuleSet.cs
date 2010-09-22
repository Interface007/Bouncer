// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StrictCustomerCheckRuleSet.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This ruleset defines some rules for the MyCustomer class - you might add as many rules as you want.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sample.Contracts.Rules
{
    using System.Collections.Generic;

    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Sample.Contracts.Entities;

    /// <summary>
    /// This ruleset defines some rules for the MyCustomer class - you might add as many rules as you want.
    /// </summary>
    internal class StrictCustomerCheckRuleSet : RuleCollection<MyCustomer, object>
    {
        protected override IEnumerable<RuleBase<MyCustomer, object>> GetRuleList()
        {
            var ruleset = new List<RuleBase<MyCustomer, object>>
            {
                new IsNotNullRule<MyCustomer>(),
                new CanNotEnterRule(),
            };

            return ruleset;
        }
    }
}
