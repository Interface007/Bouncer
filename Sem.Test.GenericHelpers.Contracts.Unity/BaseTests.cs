namespace Sem.Test.GenericHelpers.Contracts.Unity
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Test.GenericHelpers.Contracts.Unity.TestSample;

    [TestClass]
    public abstract class BaseTests
    {
        public abstract ICalculator calculator { get; }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WithRuleAtInterfaceViolated()
        {
            this.calculator.Add(101, 0, 103);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WithRuleAtMethodViolated()
        {
            this.calculator.Add(0, 102, 103);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void WithRuleAtParameterViolated()
        {
            this.calculator.Add(101, 102, 0);
        }

        [TestMethod]
        public void WithAllRulesOk()
        {
            var sum = this.calculator.Add(11, 11, 11);

            Assert.AreEqual(33, sum);
        }
    }
}
