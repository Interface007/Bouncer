namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class IsNullRuleTest
    {
        [TestMethod]
        public void IsNullRuleTestMustFail()
        {
            Assert.IsTrue(new IsNullRule<IsNullRuleTest>().CheckExpression(null, null));
        }
        
        [TestMethod]
        public void IsNullRuleTestMustPass()
        {
            Assert.IsFalse(new IsNullRule<IsNullRuleTest>().CheckExpression(this, null));
        }
    }
}