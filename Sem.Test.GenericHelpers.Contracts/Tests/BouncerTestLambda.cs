// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BouncerTestLambda.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This is a test class for BouncerTest and is intended
//   to contain all BouncerTest Unit Tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.Test.GenericHelpers.Contracts.Entities;
    using Sem.Test.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// This is a test class for BouncerTest and is intended
    /// to contain all BouncerTest Unit Tests
    /// </summary>
    [TestClass]
    public class BouncerTestLambda
    {
        [TestMethod]
        public void CheckRuleSet1()
        {
            var messageOne = new MessageOne("sometext");
            Bouncer.ForCheckData(() => messageOne).Assert(new SampleRuleSet<MessageOne>());
        }
    }
}
