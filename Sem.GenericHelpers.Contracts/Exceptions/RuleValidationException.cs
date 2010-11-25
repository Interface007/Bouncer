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

    using Rule;
    using RuleExecuters;

    /// <summary>
    /// This exception is thrown in case of a violated <see cref="RuleBase{TData,TParameter}"/> while 
    /// executing checks inside a <see cref="RuleExecuter{TData,TResultClass}"/>.
    /// </summary>
    [Serializable]
    public class RuleValidationException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleValidationException"/> class.
        /// </summary>
        public RuleValidationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleValidationException"/> class. 
        /// </summary>
        /// <param name="message">The message that describes the violation.</param>
        public RuleValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleValidationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the violation.</param>
        /// <param name="innerException"> The inner exception that did cause the violation. </param>
        public RuleValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleValidationException"/> class.
        /// </summary>
        /// <param name="ruleType"> The rule type that did detect the violation. </param>
        /// <param name="message">The message that describes the violation.</param>
        /// <param name="parameterName"> The parameter name. </param>
        internal RuleValidationException(Type ruleType, string message, string parameterName)
            : base(message, parameterName)
        {
            this.Rule = ruleType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleValidationException"/> class.
        /// </summary>
        /// <param name="info"> The exception info. </param>
        /// <param name="context"> The streaming context for the serialization. </param>
        protected RuleValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.Rule = (Type)info.GetValue("Rule", typeof(Type));
            }
        }

        /// <summary>
        /// Gets the Rule type that did detect the validation exception.
        /// </summary>
        public Type Rule { get; private set; }

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

        /// <summary>
        /// Serialization interface for the exception.
        /// </summary>
        /// <param name="info"> The exception info. </param>
        /// <param name="context"> The streaming context for the serialization. </param>
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