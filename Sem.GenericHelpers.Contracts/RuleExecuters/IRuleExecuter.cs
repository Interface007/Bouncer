// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRuleExecuter.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IRuleExecuter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Collections.Generic;

    using Sem.GenericHelpers.Contracts.Attributes;

    public interface IRuleExecuter
    {
        /// <summary>
        /// Evaluates the rule check expression for a rule attribute (property or method attribute).
        /// The attributes of methods and properties need to be executed by reflection, so we need a 
        /// public method for that purpose.
        /// </summary>
        /// <param name="ruleExecuter">The rule executer (inherits from <see cref="RuleExecuter{TData,TResultClass}"/>) that will invoke the rule validation.</param>
        /// <param name="ruleAttribute">The attribute that defines the rule.</param>
        /// <param name="propertyName">The name of the data to be validated by the rule.</param>
        /// <returns>A new instance of <see cref="RuleValidationResult"/>.</returns>
        RuleValidationResult InvokeRuleExecutionForAttribute(IRuleExecuter ruleExecuter, ContractRuleBaseAttribute ruleAttribute, string propertyName);

        IRuleExecuter AssertAll();

        IEnumerable<RuleValidationResult> Results { get; }

        void AddRange(IEnumerable<RuleValidationResult> results);

        Type ValueType { get; }
    }
}