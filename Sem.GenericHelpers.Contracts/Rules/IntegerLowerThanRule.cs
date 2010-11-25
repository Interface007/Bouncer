// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegerLowerThanRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IntegerLowerThanRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;

    /// <summary>
    /// Rule that checks whether an integer is lower than the parameter.
    /// </summary>
    public class IntegerLowerThanRule : RuleBase<int, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerLowerThanRule"/> class.
        /// </summary>
        public IntegerLowerThanRule()
        {
            this.CheckExpression = (target, parameter) => target < parameter;
            this.Message = Resources.IntegerLowerThanRuleStandardMessage;
        }
    }
}
