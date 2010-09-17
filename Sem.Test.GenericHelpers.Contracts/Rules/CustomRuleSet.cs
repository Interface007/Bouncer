namespace Sem.Test.GenericHelpers.Contracts.Rules
{
    using System.Collections.Generic;

    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Entities;

    public class CustomRuleSet : RuleCollection<AttributedSampleClass, object>
    {
        protected override IEnumerable<RuleBase<AttributedSampleClass, object>> GetRuleList()
        {
            return new List<RuleBase<AttributedSampleClass, object>>
                {
                    new IsNotNullRule<AttributedSampleClass>()
                };
        }
    }
}