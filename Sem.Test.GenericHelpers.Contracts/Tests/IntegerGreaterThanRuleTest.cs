namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class IntegerGreaterThanRuleTest
    {
        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustFail1()
        {
            Assert.IsFalse(new IntegerGreaterThanRule().CheckExpression(0, 1));
        }
        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustFail2()
        {
            Assert.IsFalse(new IntegerGreaterThanRule().CheckExpression(0, 0));
        }

        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustPass1()
        {
            Assert.IsTrue(new IntegerGreaterThanRule().CheckExpression(2, 1));
        }

        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustPass2()
        {
            Assert.IsTrue(new IntegerGreaterThanRule().CheckExpression(-2, -3));
        }
    }
}