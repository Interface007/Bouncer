// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsOneOfRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IsOneOfRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using System.Linq;

    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;

    /// <summary>
    /// Rule that searches an IEnumerable whether it contains the data to be checked. Fails if the value is NOT found.
    /// </summary>
    /// <typeparam name="TData">The type of data to be compared</typeparam>
    public class IsOneOfRule<TData> : RuleBase<TData, TData[]>
        where TData : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsOneOfRule{TData}"/> class. 
        /// </summary>
        public IsOneOfRule()
        {
            CheckExpression = (parameterValue, listOfStrings) => listOfStrings.Contains(parameterValue);
            Message = Resources.IsOneOfRuleStandardMessage;
        }
    }
}
