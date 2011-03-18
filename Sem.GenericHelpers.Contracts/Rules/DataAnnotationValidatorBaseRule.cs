// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAnnotationValidatorBaseRule.cs" company="">
//   
// </copyright>
// <summary>
//   Implements a rule for a given . Using this rule. you can reuse the
//   validator implementations of the System.Configuration namespace. There is a performance-overhead in using this
//   type of rule, because the method  (which executes the check)
//   does raise an exception if the rule is violated. So using this rule for collecting
//   with the  may throw hidden exceptions that might have a performance impact.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Globalization;

    using Sem.GenericHelpers.Contracts.Properties;
    using Sem.GenericHelpers.Contracts.Rule;
    using Sem.GenericHelpers.Contracts.RuleExecuters;

    /// <summary>
    /// Implements a rule for a given <see cref="ConfigurationValidatorBase"/>. Using this rule. you can reuse the 
    /// validator implementations of the System.Configuration namespace. There is a performance-overhead in using this 
    /// type of rule, because the method <see cref="ConfigurationValidatorBase.Validate"/> (which executes the check)
    /// does raise an exception if the rule is violated. So using this rule for collecting <see cref="RuleValidationResult"/>
    /// with the <see cref="MessageCollector{TData}"/> may throw hidden exceptions that might have a performance impact.
    /// </summary>
    /// <typeparam name="TData">The type of data to be validated.</typeparam>
    public class DataAnnotationValidatorBaseRule<TData> : RuleBase<TData, object>
    {
        /// <summary>
        /// The instance of the configuratioon validator to use for the validation.
        /// </summary>
        private ValidationAttribute configurationValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValidatorBaseRule{TData}"/> class. 
        /// </summary>
        /// <param name="validator">Sets the configuration validator to be used in this rule.</param>
        public DataAnnotationValidatorBaseRule(ValidationAttribute validator)
        {
            this.ConfigurationValidator = validator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValidatorBaseRule{TData}"/> class.
        /// </summary>
        public DataAnnotationValidatorBaseRule()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="ConfigurationValidatorBase"/> instance for this rule instance.
        /// </summary>
        public ValidationAttribute ConfigurationValidator
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
                            return this.configurationValidator.IsValid(parameter);
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