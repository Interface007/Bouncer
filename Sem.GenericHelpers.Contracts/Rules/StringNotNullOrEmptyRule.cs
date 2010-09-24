// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringNotNullOrEmptyRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the StringNotNullOrEmptyRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;

    public class StringNotNullOrEmptyRule : RuleBase<string, object>
    {
        public StringNotNullOrEmptyRule()
        {
            this.CheckExpression = (target, parameter) => !string.IsNullOrEmpty(target);
            this.Message = Resources.StringNotNullOrEmptyRuleStandardMessage;
        }
    }
}