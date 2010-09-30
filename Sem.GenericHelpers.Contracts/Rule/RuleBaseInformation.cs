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

    public abstract class RuleBaseInformation
    {
        public string Message { get; set; }

        public Type Exception { get; set; }

        protected RuleBaseInformation()
        {
            this.Message = Resources.RuleBaseInformationStandardMessage;
        }
    }
}
