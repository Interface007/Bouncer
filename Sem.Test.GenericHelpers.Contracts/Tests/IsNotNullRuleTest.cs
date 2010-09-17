namespace Sem.Test.GenericHelpers.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// This is a test class for BouncerTest and is intended
    /// to contain all BouncerTest Unit Tests
    /// </summary>
    [TestClass]
    public class BouncerIsNotNullTest
    {
        [TestMethod]
        public void IsNotNullRuleMustFail()
        {
            Assert.IsFalse(new IsNotNullRule<BouncerIsNotNullTest>().CheckExpression(null, null));
        }

        [TestMethod]
        public void IsNotNullRuleMustPass1()
        {
            Assert.IsTrue(new IsNotNullRule<BouncerIsNotNullTest>().CheckExpression(this, null));
        }
    }
}