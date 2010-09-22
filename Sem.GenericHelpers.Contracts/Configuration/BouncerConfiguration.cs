// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BouncerConfiguration.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Configuration section object for the things you can configure via the app.config
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// Configuration section object for the things you can configure via the app.config
    /// </summary>
    public class BouncerConfiguration
    {
        /// <summary>
        /// Gets or sets a value whether rules are being executed or not.
        /// </summary>
        /// <remarks>
        /// You might decide to active rules from time to time and switch them off under certain conditions. While
        /// you can do that with configured rules <see cref="Rules"/> by commenting them in the config file, you
        /// can switch off rule processing in general by activating this config value (set it to "true").
        /// </remarks>
        public bool SuppressAll { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="ConfiguredRuleInformation"/> that will be added at the type level 
        /// for distinct properties if the <see cref="ConfiguredRuleInformation.Parameter"/> property has been 
        /// set - they are added at type level if the <see cref="ConfiguredRuleInformation.Parameter"/> property
        /// has not been set.
        /// </summary>
        public List<ConfiguredRuleInformation> Rules { get; set; }

        public static IEnumerable<ContractRuleAttribute> GetConfiguredRules(PropertyInfo info, Type targetType)
        {
            return from ruleEntry in ConfigReader.GetConfig<BouncerConfiguration>().Rules
                   where ruleEntry.TargetType == targetType && ruleEntry.TargetProperty == info.Name
                   select
                       new ContractRuleAttribute(ruleEntry.Rule.GetType())
                       {
                           IncludeInContext = ruleEntry.Context,
                           Namespace = ruleEntry.Namespace,
                           Parameter = ruleEntry.Parameter,
                       };
        }
    }
}
