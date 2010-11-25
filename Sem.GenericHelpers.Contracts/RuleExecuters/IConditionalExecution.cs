// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConditionalExecution.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IConditionalExecution type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    /// <summary>
    /// Interface to access the basic information about the conditional execution rule executor 
    /// (rule execution determines whether code will be executed or not.)
    /// </summary>
    public interface IConditionalExecution
    {
        /// <summary>
        /// Gets a value indicating whether the result of the validation was true or false.
        /// </summary>
        bool ConditionIsTrue { get; }
    }
}