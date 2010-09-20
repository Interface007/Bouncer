namespace Sem.GenericHelpers.Contracts.Configuration
{
    using System.Collections.Generic;

    using Sem.GenericHelpers.Contracts.Rules;

    public class BouncerConfiguration
    {
        public bool SuppressAll { get; set; }

        public List<ConfiguredRuleInformation> Rules { get; set; }
    }
}
