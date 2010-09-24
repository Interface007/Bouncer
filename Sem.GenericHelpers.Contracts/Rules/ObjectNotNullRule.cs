// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectNotNullRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ObjectNotNullRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;

    public class ObjectNotNullRule : RuleBase<object, object>
    {
        public ObjectNotNullRule()
        {
            this.CheckExpression = (target, parameter) => target != null;
            this.Message = Resources.ObjectNotNullRuleStandardMessage;
        }
    }
}