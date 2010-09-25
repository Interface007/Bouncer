namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.RuleExecuters;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Entities;
    using Sem.Test.GenericHelpers.Contracts.Executors;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerTestRuleExecutionErrors
    {
        [TestMethod]
        public void CheckRuleWithExpressionCausingNullRefException()
        {
            var executor = new MessageCollector<string>(() => (string)null);
            var results = executor.Assert(x => x.Length > 2).Results;
            Assert.AreEqual(1, results.ToList().Count);
        }

        [TestMethod]
        public void CheckRuleWithNullRule()
        {
            var executor = new MessageCollector<string>(() => (string)null);
            var result = executor.ExecuteRuleExpression(null, "hello", "you");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleWithExecutorVetoYes()
        {
            var nonveto = string.Empty;
            var executor = new VetoExecutor<string>(() => nonveto);
            var result = executor.ExecuteRuleExpression(new StringMaxLengthRule(), 1, "you");
            Assert.IsTrue(executor.LastValidation);
        }

        [TestMethod]
        public void CheckRuleWithExecutorVetoNo()
        {
            var nonveto = string.Empty;
            var executor = new VetoExecutor<string>(() => nonveto);
            var result = executor.ExecuteRuleExpression(new StringMaxLengthRule(), 1, "veto");
            Assert.IsTrue(executor.LastValidation);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CheckRuleWithExpressionCausingFormatException()
        {
            var executor = new MessageCollector<string>(() => "Hello");
            var result = executor.Assert(x => bool.Parse(x.Length.ToString())).Results;
        }
    }
}
