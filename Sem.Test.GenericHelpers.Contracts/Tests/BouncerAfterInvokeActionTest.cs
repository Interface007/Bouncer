namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.RuleExecuters;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// Summary description for BouncerAfterInvokeActionTest
    /// </summary>
    [TestClass]
    public class BouncerAfterInvokeActionTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void AfterInvokeTest01()
        {
            var ok = true;
            var isNotNull = new IsNotNullRule<object>();
            
            // we will have one failing test, so "&= false" should set this variable to "false"
            Bouncer.AddAfterInvokeAction(x => { ok &= x.Result; });
            try
            {
                new CheckData<object>(() => null).Assert(isNotNull);
            }
            catch (Exception)
            {
            }
            
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public void AfterInvokeTest02()
        {
            var ok = false;

            // we should have one successfull test, so "|= x.Result" should set the variable to true
            Bouncer.AddAfterInvokeAction(x => { ok |= x.Result; });
            var isNotNull = new IsNotNullRule<object>();
            try
            {
                new CheckData<object>(() => this).Assert(isNotNull);
            }
            catch (Exception)
            {
            }
            
            Assert.IsTrue(ok);
        }
    }
}
