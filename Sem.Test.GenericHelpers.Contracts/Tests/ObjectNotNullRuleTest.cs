namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class ObjectNotNullRuleTest
    {
        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustFail()
        {
            Assert.IsFalse(new ObjectNotNullRule().CheckExpression(null, 0));
        }
        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustFail2()
        {
            Assert.IsFalse(new ObjectNotNullRule().CheckExpression(null, null));
        }

        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustPass1()
        {
            Assert.IsTrue(new ObjectNotNullRule().CheckExpression(this, null));
        }

        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustPass2()
        {
            Assert.IsTrue(new ObjectNotNullRule().CheckExpression(this, this));
        }
    }
}