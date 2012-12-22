namespace Sem.Test.GenericHelpers.Unity.Interceptors.WithMethodResultCaching
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CachedMethodWithParameters : TestBase
    {
        [TestMethod]
        public void ShouldNotBeCalledTwiceWithSameParameters()
        {
            var target = MethodCallTarget();
            var dateTime = new DateTime(2012, 12, 1);
            var result1 = target.CachedMethodWithSimpleParameters(string.Empty, 1, dateTime);
            var result2 = target.CachedMethodWithSimpleParameters(string.Empty, 1, dateTime);

            Assert.AreEqual(result1, result2, "both call results must be equal");
            Assert.AreEqual(1, target.GetCalls(x => x.CachedMethodWithSimpleParameters(string.Empty, 1, dateTime)), "Method must only be called once");
        }

        [TestMethod]
        public void ShouldBeCalledTwiceWithDifferentParameters()
        {
            var target = MethodCallTarget();
            var dateTime = new DateTime(2012, 12, 1);
            var result1 = target.CachedMethodWithSimpleParameters(string.Empty, 1, dateTime);
            var result2 = target.CachedMethodWithSimpleParameters(string.Empty, 2, dateTime);

            Assert.AreEqual(result1, result2, "both call results must be equal");
            Assert.AreEqual(2, target.GetCalls(x => x.CachedMethodWithSimpleParameters(string.Empty, 1, dateTime)), "Method must be called twice");
        }
    }

    [TestClass]
    public class CachedMethodWithOverload : TestBase
    {
        [TestMethod]
        public void ExactOverloadShouldBeFound()
        {
            var target = MethodCallTarget();
            var resultVoid = target.CachedMethodWithOverload();
            var resultInt = target.CachedMethodWithOverload(7);
            var resultString = target.CachedMethodWithOverload(string.Empty);

            Assert.AreEqual(3, target.Calls);

            Assert.AreEqual(32, resultVoid, "both call results must be equal");
            Assert.AreEqual(7, resultInt, "both call results must be equal");
            Assert.AreEqual(42, resultString, "both call results must be equal");

            var actualVoid = target.CachedMethodWithOverload();
            var actualInt = target.CachedMethodWithOverload(7);
            var actualString = target.CachedMethodWithOverload(string.Empty);

            Assert.AreEqual(3, target.Calls);

            Assert.AreEqual(resultVoid, actualVoid, "both call results must be equal");
            Assert.AreEqual(resultInt, actualInt, "both call results must be equal");
            Assert.AreEqual(resultString, actualString, "both call results must be equal");
        }
    }
}