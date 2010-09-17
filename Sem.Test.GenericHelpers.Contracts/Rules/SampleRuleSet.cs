namespace Sem.Test.GenericHelpers.Contracts.Rules
{
    using System.Collections.Generic;

    using Sem.GenericHelpers.Contracts.Rules;

    public class SampleRuleSet<TData> : RuleCollection<TData, object>
        where TData : class
    {
        protected override IEnumerable<RuleBase<TData, object>> GetRuleList()
        {
            var ruleset = new List<RuleBase<TData, object>>
                {
                    new IsNotNullRule<TData>(),

                    new RuleBase<TData, object> { CheckExpression = (data, parameter) => data.ToString() != "hello", },
                    new RuleBase<TData, object> { CheckExpression = (data, parameter) => !data.ToString().Contains("'"), },
                    new RuleBase<TData, object> { CheckExpression = (data, parameter) => data.ToString().Length < 1024, },
                };

            return ruleset;
        }
    }
}