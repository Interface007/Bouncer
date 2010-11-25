// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationValidatorBaseRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ConfigurationValidatorBaseRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using System;
    using System.Configuration;
    using System.Globalization;

    using Properties;
    using Rule;
    using RuleExecuters;

    /// <summary>
    /// Implements a rule for a given <see cref="ConfigurationValidatorBase"/>. Using this rule. you can reuse the 
    /// validator implementations of the System.Configuration namespace. There is a performance-overhead in using this 
    /// type of rule, because the method <see cref="ConfigurationValidatorBase.Validate"/> (which executes the check)
    /// does raise an exception if the rule is violated. So using this rule for collecting <see cref="RuleValidationResult"/>
    /// with the <see cref="MessageCollector{TData}"/> may throw hidden exceptions that might have a performance impact.
    /// </summary>
    /// <typeparam name="TData">The type of data to be validated.</typeparam>
    public class ConfigurationValidatorBaseRule<TData> : RuleBase<TData, object>
    {
        /// <summary>
        /// The instance of the configuratioon validator to use for the validation.
        /// </summary>
        private ConfigurationValidatorBase configurationValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValidatorBaseRule{TData}"/> class. 
        /// </summary>
        /// <param name="validator">Sets the configuration validator to be used in this rule.</param>
        public ConfigurationValidatorBaseRule(ConfigurationValidatorBase validator)
        {
            this.ConfigurationValidator = validator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValidatorBaseRule{TData}"/> class.
        /// </summary>
        public ConfigurationValidatorBaseRule()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="ConfigurationValidatorBase"/> instance for this rule instance.
        /// </summary>
        public ConfigurationValidatorBase ConfigurationValidator
        {
            get
            {
                return this.configurationValidator;
            }

            set
            {
                this.configurationValidator = value;

                this.CheckExpression = CheckExpression = (data, parameter) =>
                {
                    try
                    {
                        this.configurationValidator.Validate(data);
                        return true;
                    }
                    catch (ArgumentException)
                    {
                        return false;
                    }
                };

                var type = this.ConfigurationValidator.GetType();
                this.Message = string.Format(CultureInfo.CurrentCulture, Resources.ConfigurationValidatorBaseRuleStandardMessage, type.Namespace + "." + type.Name); 
            }
        }
    }
}
