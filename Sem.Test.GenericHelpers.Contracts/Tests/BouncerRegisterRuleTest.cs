namespace Sem.Test.GenericHelpers.Contracts
{
    using System;
    using System.Collections.Generic;

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
    public class BouncerRegisterRuleTest
    {
        #region preparation
        private readonly MessageOne _MessageOneOk = new MessageOne("Hello");

        private readonly MessageOne _MessageOneFail = new MessageOne("hello");

        private readonly MessageTwo _MessageTwoOk = new MessageTwo(7) { SubMessage = new MessageOne("hello") };

        private readonly MessageTwo _MessageTwoFail = new MessageTwo(7) { SubMessage = new MessageOne("hell") };

        public static RuleBase<MessageTwo, object> TestRule1()
        {
            return new RuleBase<MessageTwo, object>
            {
                CheckExpression = (data, parameter) => data != null,
            };
        }

        public static RuleBase<MessageTwo, object> TestRule2()
        {
            return new RuleBase<MessageTwo, object>
            {
                CheckExpression = (data, parameter) => data.SubMessage.Content.Length > 4,
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            RegisteredRules.Clear();
        }

        [TestInitialize]
        public void InitTest()
        {
            RegisteredRules.Register(new IsNotNullRule<string>());
            RegisteredRules.Register(TestRule2());
            RegisteredRules.RegisterCollection(new SampleRuleSet<MessageOne>());
        }
        #endregion preparation

        [TestMethod]
        public void AddRuleForType1()
        {
            Bouncer.ForCheckData(() => "2").Assert();
        }

        [TestMethod]
        public void AddRuleForType2()
        {
            Bouncer.ForCheckData(() => this._MessageTwoOk).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForType2MustFail()
        {
            Bouncer.ForCheckData(() => this._MessageTwoFail).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFail()
        {
            Bouncer.ForCheckData(() => (string)null).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFail2()
        {
            Bouncer.ForCheckData(() => this._MessageOneFail).Assert();
        }

        [TestMethod]
        public void AddRuleForType3()
        {
            Bouncer.ForCheckData(() => this._MessageOneOk).Assert();
        }
    }
}