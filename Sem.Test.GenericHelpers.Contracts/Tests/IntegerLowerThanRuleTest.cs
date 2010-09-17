namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class IntegerLowerThanRuleTest
    {
        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustFail()
        {
            Assert.IsFalse(new IntegerLowerThanRule().CheckExpression(1, 0));
        }
        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustFail2()
        {
            Assert.IsFalse(new IntegerLowerThanRule().CheckExpression(1, 1));
        }

        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustPass1()
        {
            Assert.IsTrue(new IntegerLowerThanRule().CheckExpression(1, 2));
        }

        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustPass2()
        {
            Assert.IsTrue(new IntegerLowerThanRule().CheckExpression(-3, -2));
        }
    }
}