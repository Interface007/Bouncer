// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InternalCacheStructures.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the InternalCacheStructures type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Unity.Interceptors.WithMethodResultCaching
{
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Unity.Interceptors;

    [TestClass]
    public class InternalCacheStructures : TestBase
    {
        [TestMethod]
        public void ShouldBeRefreshedAfter100Calls()
        {
            var target = MethodCallTarget();
            
            // initialize two entries into the cache
            var initialTime = target.CachedTime();                
            var initialInt = target.CachedMethod();                

            // the time entry is 1 second valid
            Thread.Sleep(1000);

            // query the int value (not touching the time value) 100 times (we already called it 
            // twice, so we should count to 98, now)
            for (var i = 0; i < 98; i++)
            {
                target.CachedMethod();

                // cache count should stay at "2"
                Assert.AreEqual(2, MethodResultCaching.ValueCount);                
            }
            
            // this time cache should clean up old entries
            var actual = target.CachedMethod();
            Assert.AreEqual(initialInt, actual);                

            // since we cleaned up the cached time entry, this should not be "2" any more.
            Assert.AreNotEqual(2, MethodResultCaching.ValueCount);

            // also the return value of "CachedTime" should change
            Assert.AreNotEqual(initialTime, target.CachedTime());
        }

        [TestMethod]
        public void InvalidatingMethodShouldClearCachedValue()
        {
            var target = MethodCallTarget();

            // initialize entry into the cache
            var initialTime = target.CachedTime();
            
            Thread.Sleep(250);

            // value should stay the same
            Assert.AreEqual(initialTime, target.CachedTime());

            target.InvalidateCachedTime();
            
            // now it should change, because we did invalidate the cached entry
            Assert.AreNotEqual(initialTime, target.CachedTime());
        }
    }
}
