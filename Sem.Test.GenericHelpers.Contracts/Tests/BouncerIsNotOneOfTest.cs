namespace Sem.Test.GenericHelpers.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerIsNotOneOfTest
    {
        [TestMethod]
        public void CheckParameterIsNotOneOfMustFail1()
        {
            Assert.IsFalse(new IsNotOneOfRule<string>().CheckExpression("2", new[] { "2", "3" }));
        }
        [TestMethod]
        public void CheckParameterIsNotOneOfMustPass0()
        {
            Assert.IsTrue(new IsNotOneOfRule<string>().CheckExpression(null, new[] { "2", "3" }));
        }

        [TestMethod]
        public void CheckParameterIsNotOneOfMustPass1()
        {
            Assert.IsTrue(new IsNotOneOfRule<string>().CheckExpression("3", new[] { "2", "1" }));
        }

        [TestMethod]
        public void CheckParameterIsNotOneOfMustPass2()
        {
            Assert.IsTrue(new IsNotOneOfRule<string>().CheckExpression("0", new[] { "1", "2" }));
        }
    }
}