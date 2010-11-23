// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleBaseInformation.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleBaseInformation type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rule
{
    using System;

    using Sem.GenericHelpers.Contracts.Properties;

    /// <summary>
    /// This is the minimum ionformation a rule must implement.
    /// </summary>
    public abstract class RuleBaseInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleBaseInformation"/> class.
        /// </summary>
        protected RuleBaseInformation()
        {
            this.Message = Resources.RuleBaseInformationStandardMessage;
        }

        /// <summary>
        /// Gets or sets the message to report a rule violation.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the type for the exception to throw in case of this rule being violated.
        /// </summary>
        public Type Exception { get; set; }
    }
}
