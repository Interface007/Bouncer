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
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Text;

    using Sem.GenericHelpers.Contracts.Rule;
    using Sem.GenericHelpers.Contracts.RuleExecuters;

    /// <summary>
    /// This exception is thrown in case of a violated <see cref="RuleBase{TData,TParameter}"/> while 
    /// executing checks inside a <see cref="RuleExecuter{TData,TResultClass}"/>.
    /// </summary>
    [Serializable]
    public class RuleValidationException : ArgumentException
    {
        public Type Rule { get; private set; }

        public RuleValidationException()
        {
        }

        public RuleValidationException(string message)
            : base(message)
        {
        }

        public RuleValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RuleValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.Rule = (Type)info.GetValue("Rule", typeof(Type));
            }
        }

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

        /// <summary>
        /// This exception does not include the methods of this library inside the stack trace,
        /// because it is thrown in cases when the calling function does need to be fixed.
        /// </summary>
        public override string StackTrace
        {
            get
            {
                var stackTrace = base.StackTrace
                    .Replace("\r", string.Empty)
                    .Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                var stackResult = new StringBuilder();
                foreach (var stackEntry in stackTrace.Where(stackEntry => !stackEntry.StartsWith("   at Sem.GenericHelpers.Contracts.", StringComparison.Ordinal)))
                {
                    stackResult.AppendLine(stackEntry);
                }

                return stackResult.ToString();
            }
        }
    }
}