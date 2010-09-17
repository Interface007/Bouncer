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

    public class IsOneOfRule<TData> : RuleBase<TData, TData[]>
        where TData : class
    {
        public IsOneOfRule()
        {
            CheckExpression = (parameterValue, listOfStrings) => listOfStrings.Contains(parameterValue);
            Message = Resources.IsOneOfRuleStandardMessage;
        }
    }
}
