// ReSharper disable CheckNamespace
namespace Sem.Sync.Test.ContractsAlternate
// ReSharper restore CheckNamespace
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Exceptions;
    using Sem.Test.GenericHelpers.Contracts.Tests;

    [TestClass]
    public class BouncerAttributedRuleTestAlternateNamespace
    {
        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFail()
        {
            Bouncer.ForCheckData(() => BouncerAttributedRuleTest.MessageFailNamespace).Assert();
        }

        [TestMethod]
        public void AddRuleForTypeOk()
        {
            Bouncer.ForCheckData(() => BouncerAttributedRuleTest.MessageOneOk).Assert();
        }
    }
}