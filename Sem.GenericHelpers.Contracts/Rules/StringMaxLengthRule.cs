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

    public class StringMaxLengthRule : RuleBase<string, int>
    {
        public StringMaxLengthRule()
        {
            this.CheckExpression = (target, parameter) => target != null && target.Length <= parameter;
            this.Message = Resources.StringMaxLengthRuleStandardMessage;
        }
    }
}