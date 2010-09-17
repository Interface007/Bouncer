namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.Test.GenericHelpers.Contracts.Entities;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerTestExecution
    {
        [TestMethod]
        public void CheckRuleExecutionDirectly1()
        {
            var messageOne = new AttributedSampleClass { MustBeLengthMin = "123456" };
            var result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleExecutionDirectly2()
        {
            var messageOne = new AttributedSampleClass { MustBeLengthMin = "1" };
            var result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .ExecuteOnFailure(() => { result = true; });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleExecution1()
        {
            var messageOne = new MessageOne("sometext");
            var result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "sometext")
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleExecutionFailure1()
        {
            var messageOne = new MessageOne("sometext");
            var result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "sometext")
                .ExecuteOnFailure(() => { result = true; });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckRuleExecution1A()
        {
            var messageOne = new MessageOne("sometext");
            var result = false;
            Bouncer
                .ForExecution(messageOne, "messageOne")
                .Assert(x => x.Content == "sometext")
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleExecution2()
        {
            var messageOne = new MessageOne("sometext");
            var result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "othertext")
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckRuleExecutionFailure2()
        {
            var messageOne = new MessageOne("sometext");
            var result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "othertext")
                .ExecuteOnFailure(() => { result = true; });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleExecution3()
        {
            var messageOne = new MessageOne("sometext");
            var result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "othertext")
                .Assert(x => x.Content == "sometext")
                .Assert(x => x.Content == "othertext")
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckRuleExecution4()
        {
            var messageOne = new MessageOne("sometext");
            var result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "sometext")
                .Assert(x => x.Content == "othertext")
                .Assert(x => x.Content == "sometext")
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckRuleExecution5()
        {
            var messageOne = new MessageOne("sometext");
            var result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "sometext")
                .Assert(x => x.Content == "sometext")
                .Assert(x => x.Content == "sometext")
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleExecution6()
        {
            var messageOne = new MessageOne("sometext");
            var result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "othertext")
                .Assert(x => x.Content == "mytext")
                .Assert(x => x.Content == "yourtext")
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsFalse(result);
        }
    }
}
