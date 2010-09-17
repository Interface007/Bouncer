// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsNotNullRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IsNotNullRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Properties;

    public class IsNotNullRule<TData> : RuleBase<TData, object>
        where TData : class
    {
        public IsNotNullRule()
        {
            this.CheckExpression = (target, parameter) => target != null;
            this.Message = Resources.IsNotNullRuleStandardMessage;
        }
    }
}
