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
    /// Tests for attributed rules.
    /// </summary>
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

        /// <summary>
        /// Tests whether an object with only valid data does not throw an exception.
        /// </summary>
        [TestMethod]
        public void AddRuleForTypeOk()
        {
            var x = Bouncer
                .ForCheckData(() => MessageOneOk)
                .Assert();

            Assert.IsNotNull(x);
        }

        /// <summary>
        /// Tests if the count of messages for a completely valid
        /// object for context "Read" is 0.
        /// </summary>
        [TestMethod]
        public void ContractContextReadOk()
        {
            var z = new SubscriberOne();
            z.ContractContextRead(MessageOneOk);
            Assert.AreEqual("0", z.Content);
        }

        /// <summary>
        /// Tests if the count of messages for a completely valid
        /// object for context "Insert" is 0.
        /// </summary>
        [TestMethod]
        public void ContractContextInsertOk()
        {
            var z = new SubscriberOne();
            z.ContractContextInsert(MessageOneOk);
            Assert.AreEqual("0", z.Content);
        }

        /// <summary>
        /// Tests if the count of messages for an object which 
        /// violates one rule for context "Read" is 1 in context "Read".
        /// </summary>
        [TestMethod]
        public void ContractContextReadFail()
        {
            var z = new SubscriberOne();
            z.ContractContextRead(MessageContextReadFail);
            Assert.AreEqual("1", z.Content);
        }

        /// <summary>
        /// Tests if the count of messages for an object which 
        /// violates one rule for context "Insert" is 0 in context "Read".
        /// </summary>
        [TestMethod]
        public void ContractContextInsertViolationNotFail()
        {
            var z = new SubscriberOne();
            z.ContractContextRead(MessageContextInsertFail);
            Assert.AreEqual("0", z.Content);
        }

        /// <summary>
        /// Tests if the count of messages for an object which 
        /// violates one rule for context "Insert" is 1 in context "Insert".
        /// </summary>
        [TestMethod]
        public void ContractContextInsertViolationVail()
        {
            var z = new SubscriberOne();
            z.ContractContextInsert(MessageContextInsertFail);
            Assert.AreEqual("1", z.Content);
        }

        /// <summary>
        /// Test the correct message for the regex rule violation.
        /// </summary>
        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect()
        {
            var counter = 0;
            Bouncer.AddAfterInvokeAction((v) => { counter++; });

            var message = Bouncer
                .ForMessages(() => MessageOneFailRegEx)
                .Assert()
                .Results;

            Assert.IsTrue(message.First().Message.Contains("MessageOneFailRegEx.MustBeOfRegExPatter must be  of reg ex '.ell.!'"));
            Assert.AreEqual(7, counter);
        }

        /// <summary>
        /// Test the execute on success method for a valid object (expression must be executed)
        /// </summary>
        [TestMethod]
        public void CheckExecuteOnSuccessWithSuccess()
        {
            var result = false;

            Bouncer
                .ForExecution(() => MessageOneOk)
                .ForExecution(() => MessageOneOk)
                .Assert()
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Test if assert runs with two valid objects
        /// </summary>
        [TestMethod]
        public void CheckRuleForCheckMultipleSuccess()
        {
            var counter = 0;
            Bouncer.AddAfterInvokeAction((v) => { counter++; });

            var x = Bouncer
                .ForCheckData(() => MessageOneOk)
                .ForCheckData(() => MessageOneOk)
                .Assert();

            Assert.IsNotNull(x);
            Assert.AreEqual(14, counter);
        }

        /// <summary>
        /// Check for multiple objects where the last one (topmost of the stack) does fail.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void CheckRuleForCheckMultipleFail1()
        {
            var x = Bouncer
                .ForCheckData(() => MessageOneOk)
                .ForCheckData(() => MessageOneFailRegEx)
                .Assert();
        }

        /// <summary>
        /// Check for multiple objects where the last one (topmost of the stack) does fail.
        /// In this case we catch the exception and the count of evaluations must be less than 
        /// the count of rules per class.
        /// </summary>
        [TestMethod]
        public void CheckRuleForCheckMultipleFail1Count()
        {
            var counter = 0;
            Bouncer.AddAfterInvokeAction(v => counter++);

            try
            {
                var x = Bouncer
                    .ForCheckData(() => MessageOneOk)
                    .ForCheckData(() => MessageOneFailRegEx)
                    .Assert();
            }
            catch { }

            Assert.IsTrue(counter < 7);
        }

        /// <summary>
        /// Test for the topmost rule on the stack being ok and the 2nd 
        /// being violated.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void CheckRuleForCheckMultipleFailSecondViolated()
        {
            var x = Bouncer
                .ForCheckData(() => MessageOneFailRegEx)
                .ForCheckData(() => MessageOneOk)
                .Assert();
        }


        /// <summary>
        /// Test for the topmost rule on the stack being ok and the 2nd 
        /// being violated. In this case we eat the exception and
        /// check the amout of rule executions that have been done (must
        /// be more than the count of rules in one class)
        /// </summary>
        [TestMethod]
        public void CheckRuleForCheckMultipleFailSecondViolatedCount()
        {
            var counter = 0;
            Bouncer.AddAfterInvokeAction(v => counter++);

            try
            {
                var x = Bouncer
                    .ForCheckData(() => MessageOneFailRegEx)
                    .ForCheckData(() => MessageOneOk)
                    .Assert();
            }
            catch { }

            Assert.IsTrue(counter > 7);
        }

        /// <summary>
        /// The execution for "ExecuteOnSuccess" must only be done, if all 
        /// rules of all objects are ok.
        /// </summary>
        [TestMethod]
        public void CheckRuleConditionalExecutionOkAnFailMustNotExecute()
        {
            var result = false;
            Bouncer
                .ForExecution(() => MessageOneOk)
                .ForExecution(() => MessageOneFailRegEx)
                .Assert()
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsFalse(result);
        }

        /// <summary>
        /// The execution for "ExecuteOnSuccess" must only be done, if all 
        /// rules of all objects are ok.
        /// </summary>
        [TestMethod]
        public void CheckRuleConditionalExecutionOkAnFailMustNotExecute2()
        {
            var result = false;
            Bouncer
                .ForExecution(() => MessageOneFailRegEx)
                .ForExecution(() => MessageOneOk)
                .Assert()
                .ExecuteOnSuccess(() => { result = true; });

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Check of generation of validation result list for name value pair.
        /// </summary>
        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect2()
        {
            var message = Bouncer
                .ForMessages(MessageOneFailRegEx, "_MessageOneFailRegEx")
                .Assert()
                .Results;

            Assert.IsTrue(message.First().Message.Contains("_MessageOneFailRegEx.MustBeOfRegExPatter must be  of reg ex '.ell.!'"));
        }

        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect3()
        {
            var messages = Bouncer
                .ForMessages("hello", "theValue")
                .Assert(new ConfigurationValidatorBaseRule<string>(new System.Configuration.StringValidator(8)))
                .Results;
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