namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Exceptions;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Entities;
    using Sem.Test.GenericHelpers.Contracts.Rules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerTest
    {
        [TestMethod]
        public void CheckRuleSet1()
        {
            var messageOne = new MessageOne("sometext");
            Bouncer.ForCheckData(() => messageOne).Assert(new SampleRuleSet<MessageOne>());
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void CheckRuleSet1Invalid()
        {
            Bouncer.ForCheckData(() => new MessageOne("hello")).Assert(new SampleRuleSet<MessageOne>());
        }

        [TestMethod]
        public void CheckIntValid0Ax()
        {
            const string SomeParameter = "";
            Bouncer.ForCheckData(SomeParameter, "someParameter")
                                .Assert(new IsNotNullRule<string>())
                                .Assert(x => x.ToString() != "0000-00-00")
                                .Assert(new ImplementsInterfaceRule<string>(), typeof(IComparable<>));
        }

        [TestMethod]
        public void CheckIntValid0A()
        {
            Bouncer.ForCheckData(0, "myInt").Assert(x => x == 0);
        }

        [TestMethod]
        public void CheckIntValid1A()
        {
            Bouncer.ForCheckData(1, "myInt").Assert(x => x == 1);
        }

        [TestMethod]
        public void CheckIntValid0B()
        {
            Bouncer.ForCheckData(0, "myInt").Assert(x => x == 0);
        }

        [TestMethod]
        public void CheckIntValid1B()
        {
            Bouncer.ForCheckData(1, "myInt").Assert(x => x == 1);
        }

        [TestMethod]
        public void CheckIntValid0()
        {
            Bouncer.ForCheckData(0, "var0").Assert(x => x == 0);
        }

        [TestMethod]
        public void CheckIntValid1()
        {
            Bouncer.ForCheckData(1, "var1").Assert(x => x == 1);
        }

        [TestMethod]
        public void CheckIntValidWithParameter1()
        {
            Bouncer.ForCheckData(1, "var1").Assert((x, y) => x == 1 && y == 7, 7);
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void CheckIntInvalid()
        {
            Bouncer.ForCheckData(0, "var0").Assert(x => x == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void CheckIntInvalidWithParameter()
        {
            Bouncer.ForCheckData(0, "var0").Assert((x, y) => x == 1, 7);
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void CheckIntInvalidWithParameter2()
        {
            Bouncer.ForCheckData(0, "var0").Assert((x, y) => y == 8, 7);
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void DeferredExcutor()
        {
            var var0 = "var0";
            Bouncer
                .For(() => var0)
                .Assert();
        }
    }
}
