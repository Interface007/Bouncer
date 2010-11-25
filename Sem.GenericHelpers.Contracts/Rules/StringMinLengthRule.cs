// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringMinLengthRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the StringMinLengthRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;

    /// <summary>
    /// Rule that checks whether a string length is equal or greater than the parameter value.
    /// </summary>
    public class StringMinLengthRule : RuleBase<string, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringMinLengthRule"/> class.
        /// </summary>
        public StringMinLengthRule()
        {
            this.CheckExpression = (target, parameter) => target != null && target.Length >= parameter;
            this.Message = Resources.StringMinLengthRuleStandardMessage;
        }
    }
}