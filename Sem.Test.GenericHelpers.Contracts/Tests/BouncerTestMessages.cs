// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BouncerTestMessages.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This is a test class for BouncerTest and is intended
//   to contain all BouncerTest Unit Tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.Test.GenericHelpers.Contracts;
    using Sem.Test.GenericHelpers.Contracts.Entities;

    /// <summary>
    /// This is a test class for BouncerTest and is intended
    /// to contain all BouncerTest Unit Tests
    /// </summary>
    [TestClass]
    public class BouncerTestMessages
    {
        private static readonly AttributedSampleClass MessageOneFailRegEx = new AttributedSampleClass
        {
            MustBeOfRegExPatter = "hello",
            MustBeLengthMax = "this is a very long string",
            MustBeLengthMin = "1",
        };

        private static readonly AttributedSampleClass MessageOneFailRegEx2 = new AttributedSampleClass
        {
            MustBeOfRegExPatter = "hello",
            MustBeLengthMax = "this is a very long string",
            MustBeLengthMin = "1",
        };

        [TestMethod]
        public void CheckRuleSet1()
        {
            var messages = Bouncer.ForMessages(() => MessageOneFailRegEx).Assert();
            Assert.AreEqual(3, messages.Results.ToList().Count);
        }

        [TestMethod]
        public void CheckRuleSet2()
        {
            var messages = Bouncer
                .ForMessages(() => MessageOneFailRegEx)
                .ForMessages(() => MessageOneFailRegEx2)
                .Assert();

            Assert.AreEqual(6, messages.Results.ToList().Count);
        }
    }
}
