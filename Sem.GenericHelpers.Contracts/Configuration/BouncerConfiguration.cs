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

    using Attributes;
    using Rules;

    /// <summary>
    /// Configuration section object for the things you can configure via the app.config
    /// </summary>
    public class BouncerConfiguration
    {
        /// <summary>
        /// List of actions to invoke after a rule validation.
        /// </summary>
        private static readonly IList<Action<RuleValidationResult>> AfterInvokeAction = new List<Action<RuleValidationResult>>();

        /// <summary>
        /// Locking object for thread save access to the AfterInvokeAction list.
        /// </summary>
        private static readonly object AfterInvokeActionSync = new object();

        /// <summary>
        /// Gets or sets a value indicating whether rules are being executed or not.
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

        /// <summary>
        /// Gets the configured rules for a specific property from the configuration file (app.config or web.config)
        /// </summary>
        /// <param name="info">The property info to get the rules for.</param>
        /// <returns>A list of configures rules for the provided property info.</returns>
        public static IEnumerable<ContractRuleAttribute> GetConfiguredRules(PropertyInfo info)
        {
            if (info == null)
            {
                return new List<ContractRuleAttribute>();
            }

            var targetType = info.DeclaringType;

            var configuredRuleInformations = ConfigReader.GetConfig<BouncerConfiguration>().Rules;
            if (configuredRuleInformations == null)
            {
                return new List<ContractRuleAttribute>();
            }

            return from ruleEntry in configuredRuleInformations
                   where ruleEntry.TargetType == targetType && ruleEntry.TargetProperty == info.Name
                   select
                       new ContractRuleAttribute(ruleEntry.Rule.GetType())
                       {
                           IncludeInContext = ruleEntry.Context,
                           Namespace = ruleEntry.Namespace,
                           Parameter = ruleEntry.Parameter,
                           ExceptionType = ruleEntry.ExceptionType,
                       };
        }

        /// <summary>
        /// Adds an action to be invoked after rule execution that accepts the result of the rule validation.
        /// </summary>
        /// <param name="action">The action to be added.</param>
        public static void AddAfterInvokeAction(Action<RuleValidationResult> action)
        {
            lock (AfterInvokeActionSync)
            {
                AfterInvokeAction.Add(action);
            }
        }

        /// <summary>
        /// Creates an list of currently registered <see cref="Action{RuleValidationResult}"/> to be 
        /// invoked after a rule has veen validated.
        /// </summary>
        /// <returns>A copy of the current list of elements.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This does not return a reference to the internal list of actions, but makes a copy of the current representation of that list. This is definitely not what a property is appropiate for.")]
        public static IEnumerable<Action<RuleValidationResult>> GetAfterInvokeActions()
        {
            lock (AfterInvokeActionSync)
            {
                // this does make a copy of the list
                return AfterInvokeAction.ToList();
            }
        }
    }
}
