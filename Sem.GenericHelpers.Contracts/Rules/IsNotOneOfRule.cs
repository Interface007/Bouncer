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
    using System.Linq;

    using Sem.GenericHelpers.Contracts.Properties;

    public class IsNotOneOfRule<TData> : RuleBase<TData, TData[]>
        where TData : class
    {
        public IsNotOneOfRule()
        {
            CheckExpression = (data, listOfStrings) => !listOfStrings.Contains(data);
            Message = Resources.IsNotOneOfRuleStandardMessage;
        }
    }
}
