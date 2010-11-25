﻿// --------------------------------------------------------------------------------------------------------------------
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

    using Attributes;
    using Exceptions;
    using Properties;

    /// <summary>
    /// Check class including the data to perform rule checking. A validation error will
    /// cause a <see cref="RuleValidationException"/>.
    /// </summary>
    /// <typeparam name="TData">The data type to be checked.</typeparam>
    public class CheckData<TData> : RuleExecuter<TData, CheckData<TData>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckData{TData}"/> class. 
        /// </summary>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="value">The value to be validated itself. </param>
        public CheckData(string valueName, TData value)
            : this(valueName, value, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckData{TData}"/> class.
        /// </summary>
        /// <param name="data"> The expression that determines the data to be validated. </param>
        public CheckData(Expression<Func<TData>> data)
            : this(data, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckData{TData}"/> class.
        /// </summary>
        /// <param name="valueName">The name of the value.</param>
        /// <param name="value">The value to be validated itself. </param>
        /// <param name="methodAttributes"> The method attributes. </param>
        public CheckData(string valueName, TData value, IEnumerable<ContractMethodRuleAttribute> methodAttributes)
            : base(valueName, value, methodAttributes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckData{TData}"/> class.
        /// </summary>
        /// <param name="data"> The expression that determines the data to be validated. </param>
        /// <param name="methodAttributes"> The method attributes. </param>
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

            throw validationResult.Exception;
        }
    }
}