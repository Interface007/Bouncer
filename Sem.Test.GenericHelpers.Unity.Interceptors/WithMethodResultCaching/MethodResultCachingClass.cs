// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodResultCachingClass.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MethodResultCachingClass type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Unity.Interceptors.WithMethodResultCaching
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Unity.Interceptors;

    [TestClass]
    public class MethodResultCachingClass
    {
        [TestMethod]
        public void ShoulBeInstanciableWithoutException()
        {
            var target = new MethodResultCaching(null);
            Assert.IsNotNull(target);
        }
    }
}
