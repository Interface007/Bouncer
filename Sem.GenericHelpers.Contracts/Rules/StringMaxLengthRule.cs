// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringMaxLengthRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the StringMaxLengthRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;

    /// <summary>
    /// This rule does check the length of the value to be lower or equal than the value of the parameter. If the
    /// length of the value is greater than the value of the parameter, the rule is violated.
    /// </summary>
    public class StringMaxLengthRule : RuleBase<string, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringMaxLengthRule"/> class.
        /// </summary>
        public StringMaxLengthRule()
        {
            this.CheckExpression = (target, parameter) => target != null && target.Length <= parameter;
            this.Message = Resources.StringMaxLengthRuleStandardMessage;
        }
    }
}