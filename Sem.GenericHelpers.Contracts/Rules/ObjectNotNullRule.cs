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

    /// <summary>
    /// This rule implements a check if the object is null. The rule is violated if the object is null.
    /// The rule parameter is not checked.
    /// </summary>
    public class ObjectNotNullRule : RuleBase<object, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectNotNullRule"/> class. 
        /// </summary>
        public ObjectNotNullRule()
        {
            this.CheckExpression = (target, parameter) => target != null;
            this.Message = Resources.ObjectNotNullRuleStandardMessage;
        }
    }
}