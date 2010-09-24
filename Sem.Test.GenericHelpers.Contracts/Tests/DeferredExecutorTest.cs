// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UndefinedExecutorTest.cs" company="">
//   
// </copyright>
// <summary>
//   Summary description for UndefinedExecutorTest
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Exceptions;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Entities;

    /// <summary>
    /// Summary description for UndefinedExecutorTest
    /// </summary>
    [TestClass]
    public class DeferredExecutorTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void EnforceTest()
        {
            var messageOne = new MessageOne("sometext");
            TestMethod1(messageOne);
        }

        [TestMethod]
        public void PreviewTest()
        {
            var messageOne = new MessageOne("sometext");
            var result = TestMethod2(messageOne);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void PreviewTestMultiple()
        {
            var messageOne = new AttributedSampleClass("sometext");
            var messageTwo = new AttributedSampleClass("sometext");
            var messageThree = new AttributedSampleClass("sometext");

            var result = Bouncer
                .For(() => messageOne)
                .For(() => messageTwo)
                .For(() => messageThree)
                .Preview();

            Assert.AreEqual(18, result.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void AssertTest()
        {
            var messageOne = new MessageOne("sometext");
            TestMethod3(messageOne);
        }

        [TestMethod]
        public void AssertTestValidData()
        {
            var messageOne = new MessageOne("hello");
            Bouncer
                .For(() => messageOne)
                .Enforce();
        }

        [ContractMethodRule(typeof(IsNullRule<MessageOne>), "messageOne")]
        private static void TestMethod1(MessageOne messageOne)
        {
            Bouncer
                .For(() => messageOne)
                .Enforce();
        }

        [ContractMethodRule(typeof(IsNullRule<MessageOne>), "messageOne")]
        private static IEnumerable<RuleValidationResult> TestMethod2(MessageOne messageOne)
        {
            return Bouncer
                .For(() => messageOne)
                .Preview();
        }

        [ContractMethodRule(typeof(IsNullRule<MessageOne>), "messageOne")]
        private static void TestMethod3(MessageOne messageOne)
        {
            Bouncer
                .For(() => messageOne)
                .Assert();
        }
    }
}
