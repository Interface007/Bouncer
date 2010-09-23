// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionHandlerExecutor.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Check class including the data to perform rule checking
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Executors
{
    using System;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.RuleExecuters;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// Check class including the data to perform rule checking. Also handle some exceptions while
    /// rule validation.
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class ExceptionHandlerExecutor<TData> : RuleExecuter<TData, ExceptionHandlerExecutor<TData>>
    {
        public ExceptionHandlerExecutor(string valueName, TData value)
            : base(valueName, value, null)
        {
        }

        public ExceptionHandlerExecutor(Expression<Func<TData>> data)
            : base(data, null)
        {
        }

        protected override bool HandleInvokeException<TParameter>(Exception ex, RuleBase<TData, TParameter> rule, object ruleParameter, string valueName, object value)
        {
            this.ExceptionHandled = valueName == "handle";
            return this.ExceptionHandled;
        }

        protected override void AfterInvoke(RuleValidationResult validationResult)
        {
        }

        public bool ExceptionHandled { get; set; }
    }
}