// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsNotOneOfRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IsNotOneOfRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using System.Collections.Generic;
    using System.Linq;

    using Properties;
    using Rule;

    /// <summary>
    /// Rule that searches an IEnumerable whether it contains the data to be checked. Fails if the value is found.
    /// </summary>
    /// <typeparam name="TData">The type of data to be compared</typeparam>
    public class IsNotOneOfRule<TData> : RuleBase<TData, IEnumerable<TData>>
        where TData : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsNotOneOfRule{TData}"/> class.
        /// </summary>
        public IsNotOneOfRule()
        {
            CheckExpression = (data, listOfStrings) => !listOfStrings.Contains(data);
            Message = Resources.IsNotOneOfRuleStandardMessage;
        }
    }
}
