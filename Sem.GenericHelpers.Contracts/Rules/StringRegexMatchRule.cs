// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringRegexMatchRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the StringRegexMatchRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;

    /// <summary>
    /// Rule that validates whether a string matches a regular expression (fails if the string is null or the string does not match the expression)
    /// </summary>
    public class StringRegexMatchRule : RuleBase<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringRegexMatchRule"/> class.
        /// </summary>
        public StringRegexMatchRule()
        {
            this.CheckExpression = (target, parameter) => target != null && new Regex(parameter).IsMatch(target);
            this.Message = Resources.StringRegexMatchRuleStandardMessage;
        }
    }
}
