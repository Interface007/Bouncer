// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsNullRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IsNullRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;

    /// <summary>
    /// Implements a simple null check where the object MUST be null.
    /// </summary>
    /// <typeparam name="TData">the type of object to be checked.</typeparam>
    public class IsNullRule<TData> : RuleBase<TData, object>
        where TData : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsNullRule{TData}"/> class.
        /// </summary>
        public IsNullRule()
        {
            this.CheckExpression = (target, parameter) => target == null;
            this.Message = Resources.IsNullRuleStandardMessage;
        }
    }
}
