// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeferredExecution.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DeferredExecution type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts.Attributes;

    public class DeferredExecution<TData> : RuleExecuter<TData, DeferredExecution<TData>>
    {
        public DeferredExecution(string valueName, TData value)
            : this(valueName, value, null)
        {
        }

        public DeferredExecution(Expression<Func<TData>> data)
            : this(data, null)
        {
        }

        public DeferredExecution(string valueName, TData value, IEnumerable<MethodRuleAttribute> methodAttributes)
            : base(valueName, value, methodAttributes)
        {
        }

        public DeferredExecution(Expression<Func<TData>> data, IEnumerable<MethodRuleAttribute> methodAttributes)
            : base(data, methodAttributes)
        {
        }

        public CheckData<TData> Enforce()
        {
            return new CheckData<TData>(this.ValueName, this.Value, this.MethodRuleAttributes).Assert();
        }

        public IEnumerable<RuleValidationResult> Preview()
        {
            return new MessageCollector<TData>(this.ValueName, this.Value, this.MethodRuleAttributes).Assert().Results;
        }

        protected override bool BeforeInvoke<TParameter>(Rules.RuleBase<TData, TParameter> rule, object ruleParameter, string valueName)
        {
            throw new NotImplementedException("You cannot use this rule executer directly, use the methods Enforce() and Preview() to process the rules.");
        }

        protected override void AfterInvoke(RuleValidationResult validationResult)
        {
            throw new NotImplementedException("You cannot use this rule executer directly, use the methods Enforce() and Preview() to process the rules.");
        }
    }
}
