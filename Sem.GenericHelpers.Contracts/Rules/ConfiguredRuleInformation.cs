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

    public class ConfiguredRuleInformation : IXmlSerializable
    {
        public RuleBaseInformation Rule { get; set; }

        public Type TargetType { get; set; }

        public string TargetProperty { get; set; }

        public string Context { get; set; }

        public string Parameter { get; set; }

        public string Namespace { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader == null)
            {
                return;
            }

            var element = XElement.Parse(reader.ReadOuterXml());
            
            this.TargetType = Type.GetType(GetAttribute(element, "TargetType"));
            this.TargetProperty = GetAttribute(element, "TargetProperty");
            
            this.Rule = Type.GetType(GetAttribute(element, "Rule")).CreateRule(this.TargetType);
            
            this.Context = GetAttribute(element, "Context");
            this.Parameter = GetAttribute(element, "Parameter");
            this.Namespace = GetAttribute(element, "Namespace");
        }

        public void WriteXml(XmlWriter writer)
        {
            if (writer == null)
            {
                return;
            }

            var targetType = this.TargetType;
            if (targetType != null)
            {
                writer.WriteAttributeString("TargetType", targetType.FullName + ", " + targetType.Assembly.GetName().Name);
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

        private static string GetAttribute(XElement element, string attributeName)
        {
            var xAttribute = element.Attribute(attributeName);
            if (xAttribute == null)
            {
                return string.Empty;
            }

            return xAttribute.Value;
        }
    }
}