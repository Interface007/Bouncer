// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckData.cs" company="Sven Erik Matzen">
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
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Exceptions;
    using Sem.GenericHelpers.Contracts.Properties;

    /// <summary>
    /// Check class including the data to perform rule checking. A validation error will
    /// cause a <see cref="RuleValidationException"/>.
    /// </summary>
    /// <typeparam name="TData">The data type to be checked.</typeparam>
    public class CheckData<TData> : RuleExecuter<TData, CheckData<TData>>
    {
        public CheckData(string valueName, TData value)
            : this(valueName, value, null)
        {
        }

        public CheckData(Expression<Func<TData>> data)
            : this(data, null)
        {
        }

        public CheckData(string valueName, TData value, IEnumerable<ContractMethodRuleAttribute> methodAttributes)
            : base(valueName, value, methodAttributes)
        {
        }

        public CheckData(Expression<Func<TData>> data, IEnumerable<ContractMethodRuleAttribute> methodAttributes)
            : base(data, methodAttributes)
        {
        }

        /// <summary>
        /// Creates a <see cref="CheckData{TDataNew}"/> for executing rules by specifying a lambda expression:
        /// <para>Bouncer.ForCheckData(() => MessageOneOk).ForCheckData(() => MessageTwo).Assert();</para>
        /// This way you can build up validation chains that can be executed with a 
        /// single <see cref="RuleExecuter{TDataNew,TResultClass}.Assert()"/> method call.
        /// The expression will be executed only once. 
        /// </summary>
        /// <typeparam name="TDataNew">The type of data the expression returns.</typeparam>
        /// <param name="data">The expression to get the content of the variable.</param>
        /// <returns>A <see cref="CheckData{TDataNew}"/> to execute the tests with.</returns>
        public CheckData<TDataNew> ForCheckData<TDataNew>(Expression<Func<TDataNew>> data)
        {
            var newExecuter = new CheckData<TDataNew>(data, this.MethodRuleAttributes)
                {
                    PreviousExecuter = this
                };

            return newExecuter;
        }

        /// <summary>
        /// Performs the rule execution result check. If the rule execution results in "false",
        /// this method will throw the <see cref="RuleValidationException"/>.
        /// </summary>
        /// <param name="validationResult">The rule validation result structure with information about the rule validation process.</param>
        /// <exception cref="RuleValidationException">If the validation result is "false".</exception>
        protected override void AfterInvoke(RuleValidationResult validationResult)
        {
            if (validationResult == null)
            {
                throw new ArgumentNullException("validationResult", Resources.ErrorMessageForRuleResultIsNull);
            }

            if (validationResult.Result)
            {
                return;
            }

            throw new RuleValidationException(validationResult.RuleType, validationResult.Message, validationResult.ValueName);
        }
    }
}