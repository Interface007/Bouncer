// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachedMethodWithParameters.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the CachedMethodWithParameters type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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
            Assert.AreEqual(2, target.Calls, "Method must be called twice");
        }
    }
}