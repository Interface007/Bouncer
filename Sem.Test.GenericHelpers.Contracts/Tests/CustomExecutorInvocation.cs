// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomExecutorInvocation.cs" company="">
//   
// </copyright>
// <summary>
//   Summary description for CustomExecutorInvocation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Test.GenericHelpers.Contracts.Entities;

    /// <summary>
    /// Summary description for CustomExecutorInvocation
    /// </summary>
    [TestClass]
    public class CustomExecutorInvocation
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            var entity = new MessageOne("Hello!");
            var business = new SubscriberOne();

            var result = business.Handle4(entity);

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestMethod2()
        {
            var entity = new MessageOne("Hello!");
            var business = new SubscriberOne();

            business.Handle5(entity);
        }
    }
}
