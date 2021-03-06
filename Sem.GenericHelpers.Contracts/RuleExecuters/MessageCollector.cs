﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageCollector.cs" company="Sven Erik Matzen">
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
    using System.Reflection;

    using Attributes;
    using Properties;

    /// <summary>
    /// Check class including the data to perform rule checking. Each rule violation
    /// adds a new entry to the <see cref="RuleExecuter{TData,TResultClass}.Results"/> list (this is a <see cref="List{T}"/>
    /// of <see cref="RuleValidationResult"/>).
    /// </summary>
    /// <typeparam name="TData">The data type to be checked.</typeparam>
    public class MessageCollector<TData> 
        : RuleExecuter<TData, MessageCollector<TData>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollector{TData}"/> class. 
        /// </summary>
        /// <param name="valueName"> The name of the value that will be checked. </param>
        /// <param name="value"> The value to be checked. </param>
        public MessageCollector(string valueName, TData value)
            : this(valueName, value, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollector{TData}"/> class. 
        /// </summary>
        /// <param name="valueName"> The name of the value that will be checked. </param>
        /// <param name="value"> The value to be checked. </param>
        public MessageCollector(string valueName, TData value, MethodBase methodBase)
            : this(valueName, value, null, methodBase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollector{TData}"/> class.
        /// </summary>
        /// <param name="data"> The data to be checked. </param>
        public MessageCollector(Expression<Func<TData>> data)
            : this(data, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollector{TData}"/> class.
        /// </summary>
        /// <param name="data"> The data to be checked. </param>
        public MessageCollector(Expression<Func<TData>> data, MethodBase methodBase)
            : this(data, null, methodBase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollector{TData}"/> class.
        /// </summary>
        /// <param name="valueName"> The name of the value that will be checked.  </param>
        /// <param name="value"> The value to be checked.  </param>
        /// <param name="methodAttributes"> The method rule attributes. </param>
        public MessageCollector(string valueName, TData value, IEnumerable<ContractMethodRuleAttribute> methodAttributes)
            : base(valueName, value, methodAttributes, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollector{TData}"/> class.
        /// </summary>
        /// <param name="valueName"> The name of the value that will be checked.  </param>
        /// <param name="value"> The value to be checked.  </param>
        /// <param name="methodAttributes"> The method rule attributes. </param>
        public MessageCollector(string valueName, TData value, IEnumerable<ContractMethodRuleAttribute> methodAttributes, MethodBase methodBase)
            : base(valueName, value, methodAttributes, methodBase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollector{TData}"/> class.
        /// </summary>
        /// <param name="data"> The data to be checked. </param>
        /// <param name="methodAttributes"> The method rule attributes. </param>
        public MessageCollector(Expression<Func<TData>> data, IEnumerable<ContractMethodRuleAttribute> methodAttributes)
            : base(data, methodAttributes, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCollector{TData}"/> class.
        /// </summary>
        /// <param name="data"> The data to be checked. </param>
        /// <param name="methodAttributes"> The method rule attributes. </param>
        public MessageCollector(Expression<Func<TData>> data, IEnumerable<ContractMethodRuleAttribute> methodAttributes, MethodBase methodBase)
            : base(data, methodAttributes, methodBase)
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
            var newExecuter = new MessageCollector<TDataNew>(data, this.MethodRuleAttributes, this.ExplicitMethodInfo)
                {
                    PreviousExecuter = this,
                };

            return newExecuter;
        }

        /// <summary>
        /// Adds the entry to the <see cref="RuleExecuter{TData,TResultClass}.Results"/>, if the validadtion did fail.
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
                this.AddExecutionResult(validationResult);
            }
        }
    }
}