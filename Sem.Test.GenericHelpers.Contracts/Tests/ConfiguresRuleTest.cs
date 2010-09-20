﻿namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System.IO;
    using System.Xml.Serialization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// Summary description for ConfiguresRuleTest
    /// </summary>
    [TestClass]
    public class ConfiguresRuleTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestIXmlSerializableGetSchema()
        {
            var rule = new ConfiguredRuleInformation();
            Assert.IsNull(rule.GetSchema());
        }

        [TestMethod]
        public void TestIXmlSerializableWriteXml()
        {
            var rule = new ConfiguredRuleInformation
                {
                    Context = "context",
                    Namespace = "namespace",
                    Parameter = "param",
                    Rule = new StringNotNullOrEmptyRule(),
                    TargetType = this.GetType(),
                    TargetProperty = "propname"
                };

            var serializer = new XmlSerializer(typeof(ConfiguredRuleInformation));
            var writer = new StringWriter();
            serializer.Serialize(writer, rule);
            var result = writer.GetStringBuilder().ToString();

            const string expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ConfiguredRuleInformation TargetType=\"Sem.Test.GenericHelpers.Contracts.Tests.ConfiguresRuleTest\" TargetProperty=\"propname\" Rule=\"Sem.GenericHelpers.Contracts.Rules.StringNotNullOrEmptyRule\" Context=\"context\" Parameter=\"param\" />";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestIXmlSerializableReadXml()
        {
            const string expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ConfiguredRuleInformation TargetType=\"Sem.Test.GenericHelpers.Contracts.Tests.ConfiguresRuleTest\" TargetProperty=\"propname\" Rule=\"Sem.GenericHelpers.Contracts.Rules.StringNotNullOrEmptyRule\" Context=\"context\" Parameter=\"param\" />";

            var serializer = new XmlSerializer(typeof(ConfiguredRuleInformation));
            var reader = new StringReader(expected);
            var rule = (ConfiguredRuleInformation)serializer.Deserialize(reader);

            Assert.AreEqual(rule.Context, "context");
            Assert.AreEqual(rule.Namespace, "namespace");
            Assert.AreEqual(rule.Parameter, "param");
            Assert.AreEqual(rule.Rule.GetType(), new StringNotNullOrEmptyRule().GetType());
            Assert.AreEqual(rule.TargetType, this.GetType());
            Assert.AreEqual(rule.TargetProperty, "propname");
        }
    }
}