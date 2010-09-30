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

    public sealed class ConfiguredRuleInformation : IXmlSerializable
    {
        public RuleBaseInformation Rule { get; set; }

        public Type TargetType { get; set; }
        
        public Type ExceptionType { get; set; }
        
        public string TargetProperty { get; set; }

        public string Context { get; set; }

        public string Parameter { get; set; }

        public string Namespace { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

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