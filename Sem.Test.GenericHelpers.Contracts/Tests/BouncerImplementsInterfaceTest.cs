namespace Sem.Test.GenericHelpers.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Entities;
    using Sem.Test.GenericHelpers.Contracts.Tests;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerImplementsInterfaceTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustFail12()
        {
            Assert.IsFalse(new ImplementsInterfaceRule<object>().CheckExpression(this, typeof(IHandleThis<BouncerTest>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustFail1()
        {
            Assert.IsFalse(new ImplementsInterfaceRule<object>().CheckExpression(this, typeof(IHandleThis<BouncerTest>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustFail2()
        {
            Assert.IsFalse(new ImplementsInterfaceRule<object>().CheckExpression(this, typeof(IHandleThis<>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustFail3()
        {
            Assert.IsFalse(new ImplementsInterfaceRule<object>().CheckExpression(null, typeof(IHandleThis<BouncerTest>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustFail4()
        {
            Assert.IsFalse(new ImplementsInterfaceRule<object>().CheckExpression(null, typeof(IHandleThis<>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustPass1()
        {
            Assert.IsTrue(new ImplementsInterfaceRule<object>().CheckExpression(new SubscriberOne(), typeof(IHandleThis<>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustPass2()
        {
            Assert.IsTrue(new ImplementsInterfaceRule<object>().CheckExpression(new SubscriberOne(), typeof(IHandleThis<MessageOne>)));
        }
    }

    public interface IHandleThis<T>
    {

    }
}