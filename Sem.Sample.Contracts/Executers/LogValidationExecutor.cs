// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogValidationExecutor.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Check class including the data to perform rule checking
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sample.Contracts.Executers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.RuleExecuters;

    /// <summary>
    /// Check class including the data to perform rule checking. Also handle some exceptions while
    /// rule validation.
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class LogValidationExecutor<TData> : RuleExecuter<TData, LogValidationExecutor<TData>>
    {
        public LogValidationExecutor(string valueName, TData value)
            : base(valueName, value, null, null)
        {
        }

        public LogValidationExecutor(Expression<Func<TData>> data)
            : base(data, null, null)
        {
        }

        public LogValidationExecutor(Expression<Func<TData>> data, IEnumerable<ContractMethodRuleAttribute> methodRuleAttributes)
            : base(data, methodRuleAttributes, null)
        {
        }

        public LogValidationExecutor(string valueName, TData value, IEnumerable<ContractMethodRuleAttribute> methodRuleAttributes)
            : base(valueName, value, methodRuleAttributes, null)
        {
        }

        public bool ExceptionHandled { get; set; }

        protected override void AfterInvoke(RuleValidationResult validationResult)
        {
            Console.WriteLine(@"check " + validationResult.RuleType.Name + @": " + validationResult.Result);
            validationResult.SkipProcessing = true;
        }
    }
}