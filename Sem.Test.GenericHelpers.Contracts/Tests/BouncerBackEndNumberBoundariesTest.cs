namespace Sem.Test.GenericHelpers.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerBackEndNumberBoundariesTest
    {
        public static RuleBase<int, object> BackEndNumberBoundaries()
        {
            return new RuleBase<int, object>
            {
                CheckExpression = (data, parameter) => data < 16000 && data > -16000,
                Message = "The provided value is not one of the expected values",
            };
        }

        [TestMethod]
        public void CheckParameterBackEndNumberBoundariesMustFail()
        {
            Assert.IsFalse(BackEndNumberBoundaries().CheckExpression(20000, null));
        }

        [TestMethod]
        public void CheckParameterBackEndNumberBoundariesMustPass1()
        {
            Assert.IsTrue(BackEndNumberBoundaries().CheckExpression(10000, null));
        }
    }
}