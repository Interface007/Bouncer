// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfiguredRuleInformation.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ConfiguredRuleInformation type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using System;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using Sem.GenericHelpers.Contracts.Rule;

    /// <summary>
    /// Configuration class for a single rule configuration.
    /// </summary>
    public sealed class ConfiguredRuleInformation : IXmlSerializable
    {
        /// <summary>
        /// Gets or sets the base information of the configured rule.
        /// </summary>
        public RuleBaseInformation Rule { get; set; }

        /// <summary>
        /// Gets or sets the target type of the rule.
        /// </summary>
        public Type TargetType { get; set; }

        /// <summary>
        /// Gets or sets the type of exception to be thrown.
        /// </summary>
        public Type ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the target property to be checked with this rule.
        /// </summary>
        public string TargetProperty { get; set; }

        /// <summary>
        /// Gets or sets the context in which the rule should be executed.
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets the execution parameter of the rule.
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// Gets or sets a namespace filter where to execute the rule.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Explicit implementation of IXmlSerializable
        /// </summary>
        /// <returns>returns simply null</returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Explicit implementation of IXmlSerializable
        /// </summary>
        /// <param name="reader"> The reader for the config section node. </param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            var element = XElement.Parse(reader.ReadOuterXml());
            
            this.TargetType = Type.GetType(GetAttribute(element, "TargetType"));
            this.ExceptionType = Type.GetType(GetAttribute(element, "ExceptionType"));
            this.TargetProperty = GetAttribute(element, "TargetProperty");
            
            this.Rule = Type.GetType(GetAttribute(element, "Rule")).CreateRule(this.TargetType);
            
            this.Context = GetAttribute(element, "Context");
            this.Parameter = GetAttribute(element, "Parameter");
            this.Namespace = GetAttribute(element, "Namespace");
        }

        /// <summary>
        /// Explicit implementation of IXmlSerializable
        /// </summary>
        /// <param name="writer"> The writer for the config section node. </param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            var targetType = this.TargetType;
            if (targetType != null)
            {
                writer.WriteAttributeString("TargetType", targetType.FullName + ", " + targetType.Assembly.GetName().Name);
            }

            var exceptionType = this.ExceptionType;
            if (exceptionType != null)
            {
                writer.WriteAttributeString("ExceptionType", exceptionType.FullName + ", " + exceptionType.Assembly.GetName().Name);
            }

            writer.WriteAttributeString("TargetProperty", this.TargetProperty);

            if (this.Rule != null)
            {
                var ruleType = this.Rule.GetType();
                writer.WriteAttributeString("Rule", ruleType.FullName + ", " + ruleType.Assembly.GetName().Name);
            }

            writer.WriteAttributeString("Context", this.Context);
            writer.WriteAttributeString("Parameter", this.Parameter);
            writer.WriteAttributeString("Namespace", this.Namespace);
        }

        /// <summary>
        /// Reads the value of an <see cref="XAttribute"/> and returns a default value if the attribute is not set.
        /// </summary>
        /// <param name="element"> The element which attribute should be read. </param>
        /// <param name="attributeName"> The name of the attribute to be read. </param>
        /// <returns>The value of the attribute if it is set, the default value otherwise.</returns>
        private static string GetAttribute(XElement element, string attributeName)
        {
            var elementAttribute = element.Attribute(attributeName);
            return 
                elementAttribute != null 
                ? elementAttribute.Value 
                : string.Empty;
        }
    }
}