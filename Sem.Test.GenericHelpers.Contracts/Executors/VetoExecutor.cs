namespace Sem.Test.GenericHelpers.Contracts.Executors
{
    using System;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Rule;
    using Sem.GenericHelpers.Contracts.RuleExecuters;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// Check class including the data to perform rule checking
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class VetoExecutor<TData> : RuleExecuter<TData, MessageCollector<TData>>
    {
        private bool cummulatedValidationResult = true;

        public VetoExecutor(string valueName, TData value)
            : base(valueName, value, null)
        {
        }

        public VetoExecutor(Expression<Func<TData>> data)
            : base(data, null)
        {
        }

        protected override bool BeforeInvoke<TParameter>(RuleBase<TData, TParameter> rule, object ruleParameter, string valueName)
        {
            return valueName != "veto";
        }

        protected override void AfterInvoke(RuleValidationResult validationResult)
        {
            cummulatedValidationResult &= validationResult.Result;
        }

        public bool LastValidation
        {
            get
            {
                return cummulatedValidationResult;
            }
        }
    }
}