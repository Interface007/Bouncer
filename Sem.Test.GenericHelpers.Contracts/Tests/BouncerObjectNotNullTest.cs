namespace Sem.Test.GenericHelpers.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerObjectNotNullTest
    {
        [TestMethod]
        public void CheckObjectNotNullTestMustFail()
        {
            Assert.IsFalse(new IsNotNullRule<string>().CheckExpression(null, null));
        }

        [TestMethod]
        public void CheckObjectNotNullTestMustPass1()
        {
            Assert.IsTrue(new IsNotNullRule<string>().CheckExpression(string.Empty, null));
        }
    }
}