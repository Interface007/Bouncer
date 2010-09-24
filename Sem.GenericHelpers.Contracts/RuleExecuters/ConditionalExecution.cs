// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionalExecution.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ConditionalExecution type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Properties;

    /// <summary>
    /// Check class including the data to perform rule checking. After asserting, the methods <see cref="ExecuteOnFailure"/>
    /// and <see cref="ExecuteOnSuccess"/> can be used to execute code is the assert did fail (at least one rule has been 
    /// violated) or succeed (no rule has been violated).
    /// </summary>
    /// <typeparam name="TData">The data type to be checked.</typeparam>
    public class ConditionalExecution<TData> : RuleExecuter<TData, ConditionalExecution<TData>>, IConditionalExecution
    {
        private bool conditionIsTrue = true;
        
        private bool assertExecuted;

        public ConditionalExecution(string valueName, TData value)
            : this(valueName, value, null)
        {
        }

        public ConditionalExecution(Expression<Func<TData>> data)
            : this(data, null)
        {
        }

        public ConditionalExecution(string valueName, TData value, IEnumerable<ContractMethodRuleAttribute> methodAttributes)
            : base(valueName, value, methodAttributes)
        {
        }
        
        public ConditionalExecution(Expression<Func<TData>> data, IEnumerable<ContractMethodRuleAttribute> methodAttributes)
            : base(data, methodAttributes)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the evaluation of the rule(s) have returned "true". 
        /// This also includes the "parent executor", that might check other data.
        /// </summary>
        public bool ConditionIsTrue
        {
            get
            {
                var parent = this.PreviousExecuter as IConditionalExecution;
                if (parent != null)
                {
                    return this.conditionIsTrue && parent.ConditionIsTrue;
                }

                return this.conditionIsTrue;
            }
        }

        /// <summary>
        /// Executes code ifthe last result of an <see cref="ConditionalExecution{TData}.Assert()"/> was "true".
        /// If no assert has been classed on this class, this method will execute <see cref="ConditionalExecution{TData}.AssertAll()"/> implicitly.
        /// </summary>
        /// <param name="action">The code to be executed.</param>
        /// <returns>this instance</returns>
        public ConditionalExecution<TData> ExecuteOnSuccess(Action action)
        {
            if (!this.assertExecuted)
            {
                this.AssertAll();
            }

            if (this.ConditionIsTrue && action != null)
            {
                action.Invoke();
            }

            return this;
        }

        /// <summary>
        /// Executes code ifthe last result of an <see cref="ConditionalExecution{TData}.Assert()"/> was "false".
        /// If no assert has been classed on this class, this method will execute <see cref="ConditionalExecution{TData}.AssertAll()"/> implicitly.
        /// </summary>
        /// <param name="action">The code to be executed.</param>
        /// <returns>this instance</returns>
        public ConditionalExecution<TData> ExecuteOnFailure(Action action)
        {
            if (!this.assertExecuted)
            {
                this.AssertAll();
            }

            if (!this.ConditionIsTrue && action != null)
            {
                action.Invoke();
            }

            return this;
        }

        /// <summary>
        /// Creates a <see cref="ConditionalExecution{TDataNew}"/> for executing code if the data violates no rules 
        /// by specifying a lambda expression:
        /// <para>Bouncer.ConditionalExecution(() => MessageOneOk).ConditionalExecution(() => MessageTwo).Assert().ExecuteOnSuccess(() => Console.WriteLine("Success"));</para>
        /// This way you can build up validation chains that can be executed with a 
        /// single <see cref="RuleExecuter{TDataNew,TResultClass}.Assert()"/> method call.
        /// The expression will be executed only once. 
        /// </summary>
        /// <typeparam name="TDataNew">The type of data the expression returns.</typeparam>
        /// <param name="data">The expression to get the content of the variable.</param>
        /// <returns>A <see cref="ConditionalExecution{TDataNew}"/> to check the rules.</returns>
        public ConditionalExecution<TDataNew> ForExecution<TDataNew>(Expression<Func<TDataNew>> data)
        {
            var newExecuter = new ConditionalExecution<TDataNew>(data, this.MethodRuleAttributes)
                {
                    PreviousExecuter = this
                };

            return newExecuter;
        }

        /// <summary>
        /// Performs the rule execution result check. 
        /// </summary>
        /// <param name="validationResult">The rule validation result structure with information about the rule validation process.</param>
        protected override void AfterInvoke(RuleValidationResult validationResult)
        {
            this.assertExecuted = true;

            if (validationResult == null)
            {
                throw new ArgumentNullException("validationResult", Resources.ErrorMessageForRuleResultIsNull);
            } 
            
            this.conditionIsTrue &= validationResult.Result;
        }
    }
}