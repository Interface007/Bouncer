namespace Sem.Test.GenericHelpers.Contracts.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rule;
    using Sem.GenericHelpers.Contracts.RuleExecuters;

    /// <summary>
    /// Check class including the data to perform rule checking
    /// </summary>
    /// <typeparam name="TData">the data type to be checked</typeparam>
    public class VetoExecutor<TData> : RuleExecuter<TData, VetoExecutor<TData>>
    {
        private bool cummulatedValidationResult = true;

        #region ctors
        
        public VetoExecutor(string valueName, TData value)
            : base(valueName, value, null)
        {
        }

        public VetoExecutor(string valueName, TData value, IEnumerable<ContractMethodRuleAttribute> methodRuleAttributes)
            : base(valueName, value, methodRuleAttributes)
        {
        }

        public VetoExecutor(Expression<Func<TData>> data)
            : base(data, null)
        {
        }

        public VetoExecutor(Expression<Func<TData>> data, IEnumerable<ContractMethodRuleAttribute> methodRuleAttributes)
            : base(data, methodRuleAttributes)
        {
        }
        
        #endregion ctors

        protected override bool BeforeInvoke<TParameter>(RuleBase<TData, TParameter> rule, object ruleParameter, string valueName)
        {
            return !valueName.StartsWith("veto");
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

    public static class VetoExecutorExtension
    {
        public static VetoExecutor<TData> UseVeto<TData>(this GenericBuilder<TData> builder)
        {
            return new VetoExecutor<TData>(builder.myData);
        }
    }
}