namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Configuration;
    using Sem.GenericHelpers.Contracts.RuleExecuters;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// Summary description for BouncerAfterInvokeActionTest
    /// </summary>
    [TestClass]
    public class BouncerAfterInvokeActionTest
    {
        /// <summary>
        /// You can add actions to the list of "AfterInvokeActions". The added actions
        /// will be executed always after invoking a rule evaluation and get the
        /// rule evaluation result as a parameter.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Test if we get the action executed if the rule is violated and an exception is thrown.
        /// </summary>
        [TestMethod]
        public void AfterInvokeTestRuleViolated()
        {
            var ok = true;
            var isNotNull = new IsNotNullRule<object>();
            
            // we will have one failing test, so "&= false" should set this variable to "false"
            BouncerConfiguration
                .AddAfterInvokeAction(x => { ok &= x.Result; });

            try
            {
                new CheckData<object>(() => null)
                    .Assert(isNotNull);
            }
            catch (Exception)
            {
            }
            
            Assert.IsFalse(ok);
        }

        /// <summary>
        /// Test if the action is invoked if the rule is not violated.
        /// </summary>
        [TestMethod]
        public void AfterInvokeTestRuleIsValid()
        {
            var ok = false;

            // we should have one successfull test, so "|= x.Result" should set the variable to true
            BouncerConfiguration.AddAfterInvokeAction(x => { ok |= x.Result; });
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
