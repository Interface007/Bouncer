namespace Sem.Test.GenericHelpers.Contracts.Entities
{
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Rules;

    // this asstribute adds a custum ruleset (inherits from ClassLevelRuleSet<TTargetClass>) to the
    // default rules defindes for this class.
    [ContractRule(typeof(CustomRuleSet))]
    public class AttributedSampleClass
    {
        public AttributedSampleClass()
        {
            this.MustBeOfRegExPatter = "Hello!";
            this.MustBeLengthMax = "12345";
            this.MustBeLengthMin = "12345";
            this.MustBeLengthMinMax = "12345";
            this.MustBeLengthAndNamespace = "hello";
            this.MustBeLengthAndContextRead = "hello";
            this.MustBeLengthAndContextInsert = "hello";
        }

        public AttributedSampleClass(string content)
        {
            this.MustBeOfRegExPatter = content;
        }

        [ContractRule(typeof(StringRegexMatchRule), Parameter = ".ell.!", Message = "{1} must be  of reg ex '.ell.!'")]
        public string MustBeOfRegExPatter { get; set; }

        [ContractRule(typeof(StringMinLengthRule), Parameter = 3)]
        public string MustBeLengthMin { get; set; }

        [ContractRule(typeof(StringMaxLengthRule), Parameter = 6)]
        public string MustBeLengthMax { get; set; }

        [ContractRule(typeof(StringMinLengthRule), Parameter = 3)]
        [ContractRule(typeof(StringMaxLengthRule), Parameter = 6)]
        [ContractRule(typeof(StringNotNullOrEmptyRule))]
        public string MustBeLengthMinMax { get; set; }

        [ContractRule(typeof(StringMinLengthRule), Parameter = 3, Namespace = "Sem.Sync.Test.ContractsAlternate")]
        public string MustBeLengthAndNamespace { get; set; }

        [ContractRule(typeof(StringMinLengthRule), Parameter = 3, IncludeInContext = "Read")]
        public string MustBeLengthAndContextRead { get; set; }

        [ContractRule(typeof(StringMinLengthRule), Parameter = 3, IncludeInContext = "Insert")]
        public string MustBeLengthAndContextInsert { get; set; }

        public override string ToString()
        {
            return this.MustBeOfRegExPatter;
        }
    }
}
