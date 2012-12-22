// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTest1.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the UnitTest1 type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Unity.Interceptors.WithPerfCounter
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Unity.Interceptors;

    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// The performance counter factory should be instanciable without throwing exceptions.
        /// This will fail if the security exceptions in case of missing permissions are not being caught correctly.
        /// </summary>
        [TestMethod]
        public void PerfCounterFactoryShouldBeInstantiable()
        {
            var perfCounterFactory = new PerfCounter<CachePerfCounter>();
            Assert.IsNotNull(perfCounterFactory);
        }

        [TestMethod]
        public void PerfCounterShouldBeCallable()
        {
            var perfCounterFactory = new PerfCounter<CachePerfCounter>();
            var cacheMiss = perfCounterFactory[CachePerfCounter.CacheMiss];
            cacheMiss.Increment();
        }
    }
}
