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

    /// <summary>
    /// Defines an interface to execute rule check expressions.
    /// </summary>
    public interface IRuleExecuter
    {
        /// <summary>
        /// Gets the results of a check if the rule check did not throw an exception.
        /// </summary>
        IEnumerable<RuleValidationResult> Results { get; }

        /// <summary>
        /// Gets the type of the value to be checked.
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Performs a check of all collected assertions.
        /// </summary>
        /// <returns>Returns itself in oder to be able to perform additional actions in a fluent interface.</returns>
        IRuleExecuter AssertAll();

        /// <summary>
        /// Adds a range of validation results to the current results.
        /// </summary>
        /// <param name="results"> The results to be added. </param>
        void AddRange(IEnumerable<RuleValidationResult> results);
    }
}