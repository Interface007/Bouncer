// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationTest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Summary description for ConfigurationTest
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Configuration;
    using Sem.Test.GenericHelpers.Contracts.Entities;

    /// <summary>
    /// Summary description for ConfigurationTest
    /// </summary>
    [TestClass]
    public class ConfigurationTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ReadExistingConfig()
        {
            var result = ConfigReader.GetConfig<BouncerConfiguration>();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReadNonExistingConfig()
        {
            var result = ConfigReader.GetConfig<MessageOne>();
            Assert.IsNotNull(result);
            Assert.AreEqual(new MessageOne().Content, result.Content);
        }
    }
}
