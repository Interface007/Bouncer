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
    using Sem.GenericHelpers.Contracts.Rule;

    /// <summary>
    /// This rule does check a value to not be null. In case of the value being null, the rule is
    /// violated.
    /// </summary>
    /// <typeparam name="TData">The type of data to be checked.</typeparam>
    public class IsNotNullRule<TData> : RuleBase<TData, object>
        where TData : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsNotNullRule{TData}"/> class. 
        /// </summary>
        public IsNotNullRule()
        {
            this.CheckExpression = (target, parameter) => target != null;
            this.Message = Resources.IsNotNullRuleStandardMessage;
        }
    }
}
