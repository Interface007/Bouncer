// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WhenLastInterceptor.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the UnitTest1 type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Unity
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Test.GenericHelpers.Contracts.Unity.TestSample;

    [TestClass]
    public class WhenLastInterceptor
    {
        private readonly ICalculator calculator = new UnityContainer()
                                                    .AddNewExtension<ConfigIsLastInterceptor>()
                                                    .Resolve<ICalculator>();

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
            var sum = this.calculator.Add(101, 102, 103);

            Assert.AreEqual(306, sum);
        }
    }
}