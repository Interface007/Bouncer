// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachedMethodWithoutParameters.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the CachedMethodWithoutParameters type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Unity.Interceptors.WithMethodResultCaching
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CachedMethodWithoutParameters : TestBase
    {
        [TestMethod]
        public void ShouldNotBeCalledTwice()
        {
            var target = MethodCallTarget();
            var result1 = target.CachedMethod();
            var result2 = target.CachedMethod();

            Assert.AreEqual(result1, result2, "both call results must be equal");
            Assert.AreEqual(1, target.GetCalls(x => x.CachedMethod()), "Method must only be called once");
        }
    }
}
