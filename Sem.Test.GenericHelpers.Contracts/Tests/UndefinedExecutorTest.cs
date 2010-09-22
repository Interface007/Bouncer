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
    public class UndefinedExecutorTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void CheckRuleSet1()
        {
            var messageOne = new MessageOne("sometext");
            TestMethod2(messageOne);
        }

        [MethodRule(typeof(IsNullRule<MessageOne>), "messageOne")]
        private static void TestMethod2(MessageOne messageOne)
        {
            Bouncer
                .For(() => messageOne)
                .Enforce();
        }
    }
}
