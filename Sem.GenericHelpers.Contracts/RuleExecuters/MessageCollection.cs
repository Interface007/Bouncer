// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageCollection.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Check class including the data to perform rule checking
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Properties;

    public interface IMessageCollector
    {
        IEnumerable<RuleValidationResult> Results { get; }
    }

    /// <summary>
    /// Check class including the data to perform rule checking. Each rule violation
    /// adds a new entry to the <see cref="Results"/> list (this is a <see cref="List{T}"/>
    /// of <see cref="RuleValidationResult"/>).
    /// </summary>
    /// <typeparam name="TData">The data type to be checked.</typeparam>
    public class MessageCollector<TData> : RuleExecuter<TData, MessageCollector<TData>>, IMessageCollector
    {
        /// <summary>
        /// The result list of <see cref="RuleValidationResult"/>. Each violated rule while
        /// asserting adds a new entry to this list.
        /// </summary>
        private readonly List<RuleValidationResult> myResults = new List<RuleValidationResult>();

        public IEnumerable<RuleValidationResult> Results
        {
            get
            {
                var results = this.myResults;

                var previousExecuter = this.PreviousExecuter as IMessageCollector;
                if (previousExecuter != null)
                {
                    return results.Concat(previousExecuter.Results);
                }

                return results;
            }
        }

        public MessageCollector(string valueName, TData value)
            : this(valueName, value, null)
        {
        }

        public MessageCollector(Expression<Func<TData>> data)
            : this(data, null)
        {
        }

        public MessageCollector(string valueName, TData value, IEnumerable<MethodRuleAttribute> methodAttributes)
            : base(valueName, value, methodAttributes)
        {
        }

        public MessageCollector(Expression<Func<TData>> data, IEnumerable<MethodRuleAttribute> methodAttributes)
            : base(data, methodAttributes)
        {
        }

        /// <summary>
        /// Creates a <see cref="MessageCollector{TData}"/> for collecting warnings about rule violations 
        /// by specifying a lambda expression:
        /// <para>var result = Bouncer.ForMessages(() => MessageOneOk).ForMessages(() => MessageOneOk).Assert().Results;</para>
        /// This way you can build up validation chains that can be executed with a 
        /// single <see cref="RuleExecuter{TDataNew,TResultClass}.Assert()"/> method call.
        /// The expression will be executed only once. 
        /// </summary>
        /// <typeparam name="TDataNew">The type of data the expression returns.</typeparam>
        /// <param name="data">The expression to get the content of the variable.</param>
        /// <returns>A <see cref="MessageCollector{TDataNew}"/> to check the rules.</returns>
        public MessageCollector<TDataNew> ForMessages<TDataNew>(Expression<Func<TDataNew>> data)
        {
            var newExecuter = new MessageCollector<TDataNew>(data, this.MethodRuleAttributes)
                {
                    PreviousExecuter = this,
                };

            return newExecuter;
        }

        /// <summary>
        /// Adds the entry to the <see cref="Results"/>, if the validadtion did fail.
        /// </summary>
        /// <param name="validationResult">The rule validation result structure with information about the rule validation process.</param>
        protected override void AfterInvoke(RuleValidationResult validationResult)
        {
            if (validationResult == null)
            {
                throw new ArgumentNullException("validationResult", Resources.ErrorMessageForRuleResultIsNull);
            }

            if (!validationResult.Result)
            {
                this.myResults.Add(validationResult);
            }
        }
    }
}