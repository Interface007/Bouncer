namespace Sem.Test.GenericHelpers.Unity.Interceptors.WithMethodResultCaching
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NonCachedMethod : TestBase
    {
        [TestMethod]
        public void ShouldBeCalledTwiceWithSameParameters()
        {
            var target = MethodCallTarget();
            var dateTime = new DateTime(2012, 12, 1);
            var result1 = target.NonCachedMethodWithSimpleParameters(string.Empty, 1, dateTime);
            var result2 = target.NonCachedMethodWithSimpleParameters(string.Empty, 1, dateTime);

            Assert.AreEqual(result1, result2, "both call results must be equal");
            Assert.AreEqual(2, target.GetCalls(x => x.NonCachedMethodWithSimpleParameters(string.Empty, 1, dateTime)), "Method must be called twice");
        }

        [TestMethod]
        public void ShouldBeCalledTwiceWithDifferentParameters()
        {
            var target = MethodCallTarget();
            var dateTime = new DateTime(2012, 12, 1);
            var result1 = target.NonCachedMethodWithSimpleParameters(string.Empty, 1, dateTime);
            var result2 = target.NonCachedMethodWithSimpleParameters(string.Empty, 2, dateTime);

            Assert.AreEqual(result1, result2, "both call results must be equal");
            Assert.AreEqual(2, target.GetCalls(x => x.NonCachedMethodWithSimpleParameters(string.Empty, 1, dateTime)), "Method must be called twice");
        }
    }
}