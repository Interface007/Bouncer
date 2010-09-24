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

    public class IntegerLowerThanRule : RuleBase<int, int>
    {
        public IntegerLowerThanRule()
        {
            this.CheckExpression = (target, parameter) => target < parameter;
            this.Message = Resources.IntegerLowerThanRuleStandardMessage;
        }
    }
}
