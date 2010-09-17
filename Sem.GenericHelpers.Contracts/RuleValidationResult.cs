// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleValidationResult.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleValidationResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;

    /// <summary>
    /// Describes the result of a rule validation.
    /// </summary>
    public class RuleValidationResult
    {
        public RuleValidationResult(Type ruleType, string message, string valueName, bool result)
        {
            this.Result = result;
            this.RuleType = ruleType;
            this.Message = message;
            this.ValueName = valueName;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the last result was true (the rule is valid) or false (the rule is violated).
        /// </summary>
        public bool Result { get; protected set; }

        /// <summary>
        /// Gets or sets the message about the rule violation.
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the rule that has been validated.
        /// </summary>
        public Type RuleType { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the data (the variable/argument name) the rule has been applied to.
        /// </summary>
        public string ValueName { get; protected set; }

        /// <summary>
        /// Returns a message about the rule validation.
        /// </summary>
        /// <returns>The message about rule validation result.</returns>
        public override string ToString()
        {
            return this.Message;
        }
    }
}