// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BouncerAttributedRuleTest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This is a test class for BouncerTest and is intended
//   to contain all BouncerTest Unit Tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System;
    using System.Collections;
    using System.Linq;

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
    public class BouncerAttributedRuleTest
    {
        #region entities
        public static readonly AttributedSampleClass MessageOneOk = new AttributedSampleClass
            {
                MustBeLengthMin = "6chars",
            };

        private static readonly AttributedSampleClass MessageOneFailRegEx = new AttributedSampleClass
            {
                MustBeOfRegExPatter = "hello",
            };

        private static readonly AttributedSampleClass MessageOneFailMin = new AttributedSampleClass
            {
                MustBeLengthMin = "6",
            };

        private static readonly AttributedSampleClass MessageOneFailMax = new AttributedSampleClass
            {
                MustBeLengthMax = "6charactersAndMore",
            };

        private static readonly AttributedSampleClass MessageOneFailRegExNull = new AttributedSampleClass
            {
                MustBeOfRegExPatter = null,
            };

        private static readonly AttributedSampleClass MessageOneFailMinNull = new AttributedSampleClass
            {
                MustBeLengthMin = null,
            };

        private static readonly AttributedSampleClass MessageOneFailMaxNull = new AttributedSampleClass
            {
                MustBeLengthMax = null,
            };

        private static readonly AttributedSampleClass MessageOneFailMinMax = new AttributedSampleClass
            {
                MustBeLengthMinMax = "manycharactersarehere",
            };

        public static readonly AttributedSampleClass MessageFailNamespace = new AttributedSampleClass
            {
                MustBeLengthAndNamespace = "m",
            };

        public static readonly AttributedSampleClass MessageContextReadFail = new AttributedSampleClass
            {
                MustBeLengthAndContextRead = "m",
            };

        public static readonly AttributedSampleClass MessageContextInsertFail = new AttributedSampleClass
            {
                MustBeLengthAndContextInsert = "m",
            };
        
        #endregion 

        [TestMethod]
        public void AddRuleForTypeOk()
        {
            var x = Bouncer.ForCheckData(() => MessageOneOk).Assert();
            Assert.IsNotNull(x);
        }

        [TestMethod]
        public void ContractContextReadOk()
        {
            var z = new SubscriberOne();
            z.ContractContextRead(MessageOneOk);
            Assert.AreEqual("0", z.Content);
        }

        [TestMethod]
        public void ContractContextInsertOk()
        {
            var z = new SubscriberOne();
            z.ContractContextRead(MessageOneOk);
            Assert.AreEqual("0", z.Content);
        }

        [TestMethod]
        public void ContractContextReadFail()
        {
            var z = new SubscriberOne();
            z.ContractContextRead(MessageContextReadFail);
            Assert.AreEqual("1", z.Content);
        }

        [TestMethod]
        public void ContractContextInsertFail()
        {
            var z = new SubscriberOne();
            z.ContractContextRead(MessageContextInsertFail);
            Assert.AreEqual("0", z.Content);

            z.ContractContextInsert(MessageContextInsertFail);
            Assert.AreEqual("1", z.Content);
        }

        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect()
        {
            var message = Bouncer.ForMessages(() => MessageOneFailRegEx).Assert().Results;
            Assert.IsTrue(message.First().Message.Contains("MessageOneFailRegEx.MustBeOfRegExPatter must be  of reg ex '.ell.!'"));
        }

        [TestMethod]
        public void CheckRuleExecution11()
        {
            var result = false;
            Bouncer
                .ForExecution(() => MessageOneOk)
                .ForExecution(() => MessageOneOk)
                .Assert()
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleForCheckMultipleSuccess()
        {
            var x = Bouncer
                .ForCheckData(() => MessageOneOk)
                .ForCheckData(() => MessageOneOk)
                .Assert();

            Assert.IsNotNull(x);
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void CheckRuleForCheckMultipleFail1()
        {
            var x = Bouncer
                .ForCheckData(() => MessageOneOk)
                .ForCheckData(() => MessageOneFailRegEx)
                .Assert();

            Assert.IsNotNull(x);
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void CheckRuleForCheckMultipleFail2()
        {
            var x = Bouncer
                .ForCheckData(() => MessageOneFailRegEx)
                .ForCheckData(() => MessageOneOk)
                .Assert();

            Assert.IsNotNull(x);
        }

        [TestMethod]
        public void CheckRuleExecution11A()
        {
            var result = false;
            Bouncer
                .ForExecution(() => MessageOneOk)
                .ForExecution(() => MessageOneFailRegEx)
                .Assert()
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckRuleExecution11B()
        {
            var result = false;
            Bouncer
                .ForExecution(() => MessageOneFailRegEx)
                .ForExecution(() => MessageOneOk)
                .Assert()
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect2()
        {
            var message = Bouncer.ForMessages(MessageOneFailRegEx, "_MessageOneFailRegEx").Assert().Results;
            Assert.IsTrue(message.First().Message.Contains("_MessageOneFailRegEx.MustBeOfRegExPatter must be  of reg ex '.ell.!'"));
        }

        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect3()
        {
            var messages = Bouncer.ForMessages("hello", "theValue").Assert(new ConfigurationValidatorBaseRule<string>(new System.Configuration.StringValidator(8))).Results;
            var actual = messages.First().Message;
            Assert.AreEqual("The rule Sem.GenericHelpers.Contracts.Rules.ConfigurationValidatorBaseRule`1 did fail for value name >>theValue<<: The validator System.Configuration.StringValidator did throw an exception.", actual);
        }

        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect3A()
        {
            var messages = Bouncer
                .ForMessages("hello", "theValue")
                .Assert(
                    new ConfigurationValidatorBaseRule<string>
                        {
                            ConfigurationValidator = new System.Configuration.StringValidator(8)
                        }
                ).Results;
            var actual = messages.First().Message;
            Assert.AreEqual("The rule Sem.GenericHelpers.Contracts.Rules.ConfigurationValidatorBaseRule`1 did fail for value name >>theValue<<: The validator System.Configuration.StringValidator did throw an exception.", actual);
        }

        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect4()
        {
            var messages = Bouncer.ForMessages("hello", "theValue").Assert(new ConfigurationValidatorBaseRule<string>(new System.Configuration.StringValidator(8))).Results;
            var actual = messages.First().Message;
            Assert.AreEqual(messages.First().ToString(), actual);
        }

        [TestMethod]
        public void AddRuleForTypeMustSucceedRegExCollect4()
        {
            var configurationValidatorBaseRule = new ConfigurationValidatorBaseRule<string>(new System.Configuration.StringValidator(8));
            var messages = Bouncer.ForMessages("hello I have more than 8 chars", "theValue").Assert(configurationValidatorBaseRule).Results;
            var actual = messages.ToList().Count;
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailRegEx()
        {
            Bouncer.ForCheckData(() => MessageOneFailRegEx).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailMin()
        {
            Bouncer.ForCheckData(() => MessageOneFailMin).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailMax()
        {
            Bouncer.ForCheckData(() => MessageOneFailMax).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailRegExNull()
        {
            Bouncer.ForCheckData(() => MessageOneFailRegExNull).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailMinNull()
        {
            Bouncer.ForCheckData(() => MessageOneFailMinNull).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailMaxNull()
        {
            Bouncer.ForCheckData(() => MessageOneFailMaxNull).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailMinMaxNull()
        {
            Bouncer.ForCheckData(() => MessageOneFailMinMax).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MethodAttributeInValid()
        {
            var test = new SubscriberOne();
            test.Handle(new MessageOne("hello"));
            Assert.AreEqual("hello0", test.Content);
        }

        [TestMethod]
        public void MethodAttributeWithSuccess()
        {
            var test = new SubscriberOne();
            test.Handle2(null);
            Assert.AreEqual("1", test.Content);
        }

        [TestMethod]
        public void MethodAttributeWithSuccess2()
        {
            var test = new SubscriberOne();
            test.Handle3(null);
            Assert.AreEqual("1", test.Content);
        }

        [TestMethod]
        public void ClassLevelRuleSetIEnumerable()
        {
            var x = new CustomRuleSet();
            Assert.IsNotNull(((IEnumerable)x).GetEnumerator());
        }
    }
}