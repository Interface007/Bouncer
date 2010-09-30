namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System;
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
        private const string expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ConfiguredRuleInformation TargetType=\"Sem.Test.GenericHelpers.Contracts.Tests.ConfiguresRuleTest, Sem.Test.GenericHelpers.Contracts\" TargetProperty=\"propname\" Rule=\"Sem.GenericHelpers.Contracts.Rules.StringNotNullOrEmptyRule, Sem.GenericHelpers.Contracts\" Context=\"context\" Parameter=\"param\" Namespace=\"namespace\" />";

        private const string expected2 = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ConfiguredRuleInformation TargetType=\"Sem.Test.GenericHelpers.Contracts.Tests.ConfiguresRuleTest, Sem.Test.GenericHelpers.Contracts\" ExceptionType=\"System.NullReferenceException, mscorlib\" TargetProperty=\"propname\" Rule=\"Sem.GenericHelpers.Contracts.Rules.StringNotNullOrEmptyRule, Sem.GenericHelpers.Contracts\" Context=\"context\" Parameter=\"param\" Namespace=\"namespace\" />";

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

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestIXmlSerializableWriteXml2()
        {
            var rule = new ConfiguredRuleInformation
                {
                    Context = "context",
                    Namespace = "namespace",
                    Parameter = "param",
                    Rule = new StringNotNullOrEmptyRule(),
                    TargetType = this.GetType(),
                    TargetProperty = "propname",
                    ExceptionType = typeof(NullReferenceException)
                };

            var serializer = new XmlSerializer(typeof(ConfiguredRuleInformation));
            var writer = new StringWriter();
            serializer.Serialize(writer, rule);
            var result = writer.GetStringBuilder().ToString();

            Assert.AreEqual(expected2, result);
        }

        [TestMethod]
        public void TestIXmlSerializableReadXml()
        {
            var serializer = new XmlSerializer(typeof(ConfiguredRuleInformation));
            var reader = new StringReader(expected);
            var rule = (ConfiguredRuleInformation)serializer.Deserialize(reader);

            Assert.AreEqual(rule.Context, "context");
            Assert.AreEqual(rule.Namespace, "namespace");
            Assert.AreEqual(rule.Parameter, "param");
            Assert.AreEqual(rule.Rule.GetType(), new StringNotNullOrEmptyRule().GetType());
            Assert.AreEqual(rule.TargetType.FullName, this.GetType().FullName);
            Assert.AreEqual(rule.TargetProperty, "propname");
        }
    }
}
