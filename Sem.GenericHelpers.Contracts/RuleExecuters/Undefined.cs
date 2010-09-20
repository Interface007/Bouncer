using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Contracts.Attributes;

    internal class Undefined<TData> : RuleExecuter<TData, Undefined<TData>>
    {
        public Undefined(string valueName, TData value)
            : this(valueName, value, null)
        {
        }

        public Undefined(Expression<Func<TData>> data)
            : this(data, null)
        {
        }

        public Undefined(string valueName, TData value, IEnumerable<MethodRuleAttribute> methodAttributes)
            : base(valueName, value, methodAttributes)
        {
        }

        public Undefined(Expression<Func<TData>> data, IEnumerable<MethodRuleAttribute> methodAttributes)
            : base(data, methodAttributes)
        {
        }

        protected override void AfterInvoke(RuleValidationResult validationResult)
        {
        }
    }
}
