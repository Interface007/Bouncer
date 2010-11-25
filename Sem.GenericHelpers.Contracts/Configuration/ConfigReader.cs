// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigReader.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ConfigReader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml.Serialization;

    /// <summary>
    /// Generic configuration reader. This reader enables a much easier configuration implementation.
    /// </summary>
    /// <example>
    /// To enable the configuration node, map it to this reader:
    ///     <code>
    ///         &lt;configSections>
    ///             &lt;section name="BouncerConfiguration"
    ///                 type="Sem.GenericHelpers.Contracts.Configuration.ConfigReader, Sem.GenericHelpers.Contracts"/>
    ///         &lt;/configSections>
    ///     </code>
    /// Then insert the configuration node as usual:
    ///     <code>
    ///         &lt;BouncerConfiguration>
    ///           &lt;SuppressAll>false&lt;/SuppressAll>
    ///           &lt;Rules>
    ///             &lt;ConfiguredRuleInformation Rule="Sem.GenericHelpers.Contracts.Rules.StringRegexMatchRule, Sem.GenericHelpers.Contracts"
    ///                                        TargetType="Sem.Sample.Contracts.Entities.MyCustomer, Sem.Sample.Contracts"
    ///                                        TargetProperty="PhoneNumber" 
    ///                                        Parameter="^((\+[0-9]{2,4}( [0-9]+? | ?\([0-9]+?\) ?))|(\(0[0-9 ]+?\) ?)|(0[0-9]+? ?( |-|\/) ?))[0-9]+?[0-9 \/-]*[0-9]$" 
    ///                                        Context="Config"/>
    ///           &lt;/Rules>
    ///         &lt;/BouncerConfiguration>
    ///     </code>
    /// To read this configuration, you just need to call this line:
    /// <code>var configuredRuleInformations = ConfigReader.GetConfig&lt;BouncerConfiguration>().Rules;</code>
    /// As you can see, the read operation is handled by the general ConfigReader instead of a specific configuration handler. You can use 
    /// any xml-serializable class as an configuration class, this way.
    /// </example>
    public class ConfigReader : ConfigurationSection
    {
        /// <summary>
        /// The locking object to sync thread concurrent access to the configuration.
        /// </summary>
        private static readonly object Sync = new object();

        /// <summary>
        /// The dictionary of configuration nodes already read.
        /// </summary>
        private static readonly Dictionary<Type, object> Current = new Dictionary<Type, object>();

        /// <summary>
        /// The current configuration class type.
        /// </summary>
        private static Type currentType;
        
        /// <summary>
        /// Gets the deserialized configuration object for a given type. See <see cref="ConfigReader"/> for an example.
        /// </summary>
        /// <typeparam name="TResult">The type to be deserialized from the configuration.</typeparam>
        /// <returns>The deserialized data from the configuration.</returns>
        public static TResult GetConfig<TResult>() 
            where TResult : class, new()
        {
            lock (Sync)
            {
                currentType = typeof(TResult);
                if (!Current.ContainsKey(currentType))
                {
                    ConfigurationManager.GetSection(currentType.Name);
                    if (!Current.ContainsKey(currentType))
                    {
                        Current.Add(currentType, new TResult());
                    }
                }

                return (TResult)Current[currentType];
            }
        }

        /// <summary>
        /// Deserializes the configuration node.
        /// </summary>
        /// <param name="reader"> The xml reader accessing the configuration xml. </param>
        protected override void DeserializeSection(System.Xml.XmlReader reader)
        {
            var serializer = new XmlSerializer(currentType);
            Current.Add(currentType, serializer.Deserialize(reader));
        }
    }
}