﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegerGreaterThanRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IntegerGreaterThanRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;

    /// <summary>
    /// Rule that checks whether an integer is greater than the parameter.
    /// </summary>
    public class IntegerGreaterThanRule : RuleBase<int, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerGreaterThanRule"/> class.
        /// </summary>
        public IntegerGreaterThanRule()
        {
            this.CheckExpression = (target, parameter) => target > parameter;
            this.Message = Resources.IntegerGreaterThanRuleStandardMessage;
        }
    }
}