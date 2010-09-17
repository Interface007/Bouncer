﻿// --------------------------------------------------------------------------------------------------------------------
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

    public class StringRegexMatchRule : RuleBase<string, string>
    {
        public StringRegexMatchRule()
        {
            this.CheckExpression = (target, parameter) => target != null && new Regex(parameter).IsMatch(target);
            this.Message = Resources.StringRegexMatchRuleStandardMessage;
        }
    }
}
