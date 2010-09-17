namespace Sem.Test.GenericHelpers.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.RuleExecuters;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Entities;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerIsOneOfTest
    {
        [TestMethod]
        public void CheckParameterIsOneOfMustFail1()
        {
            Assert.IsFalse(new IsOneOfRule<string>().CheckExpression("1", new[] { "2", "3" }));
        }
        [TestMethod]
        public void CheckParameterIsOneOfMustFail2()
        {
            Assert.IsFalse(new IsOneOfRule<string>().CheckExpression(null, new[] { "2", "3" }));
        }

        [TestMethod]
        public void CheckParameterIsOneOfMustPass1()
        {
            Assert.IsTrue(new IsOneOfRule<string>().CheckExpression("1", new[] { "2", "1" }));
        }

        [TestMethod]
        public void CheckParameterIsOneOfMustPass2()
        {
            Assert.IsTrue(new IsOneOfRule<string>().CheckExpression("1", new[] { "1", "2" }));
        }

        [TestMethod]
        public void CheckParameterIsOneOfMustPass3()
        {
            var y = new MessageTwo(8);
            new CheckData<string>(() => y.SubMessage.Content).Assert(x => string.IsNullOrEmpty(x));
        }
    }
}