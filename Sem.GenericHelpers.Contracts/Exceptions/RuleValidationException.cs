// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleValidationException.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleValidationException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    using Sem.GenericHelpers.Contracts.RuleExecuters;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// This exception is thrown in case of a violated <see cref="RuleBase{TData,TParameter}"/> while 
    /// executing checks inside a <see cref="RuleExecuter{TData,TResultClass}"/>.
    /// </summary>
    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "This exception should not be thrown as a result of an expection - it's designed to only me thrown while rule validation.")]
    public class RuleValidationException : ArgumentException
    {
        public Type Rule { get; set; }

        internal RuleValidationException(Type ruleType, string message, string parameterName)
            : base(message, parameterName)
        {
            this.Rule = ruleType;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                info.AddValue("Rule", this.Rule);
                base.GetObjectData(info, context);
            }
        }
    }
}