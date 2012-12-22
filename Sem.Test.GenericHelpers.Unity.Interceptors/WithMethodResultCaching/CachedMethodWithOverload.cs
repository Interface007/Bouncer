// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachedMethodWithOverload.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the CachedMethodWithOverload type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Unity.Interceptors.WithMethodResultCaching
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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