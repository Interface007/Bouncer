// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyCustomer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class inherits from MyCustomer in order to be able to
//   call the exact same code for "GetTheName" and "FormatTheId".
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sample.Contracts.Entities
{
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// This class inherits from MyCustomer in order to be able to 
    /// call the exact same code for "GetTheName" and "FormatTheId".
    /// </summary>
    internal class MyCustomer 
    {
        /// <summary>
        /// Gets or sets the internal id must be set for read, update and delete, but must not be set for insert.
        /// </summary>
        [ContractRule(typeof(IsNullRule<CustomerId>), IncludeInContext = "Create")]
        [ContractRule(typeof(IsNotNullRule<CustomerId>), IncludeInContext = "Read")]
        [ContractRule(typeof(IsNotNullRule<CustomerId>), IncludeInContext = "Update")]
        [ContractRule(typeof(IsNotNullRule<CustomerId>), IncludeInContext = "Delete")]
        public CustomerId InternalId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the Customer.
        /// This property will be checked to not be null or empty. Additionally we 
        /// alter the message to the message string specified inside the parameter
        /// </summary>
        [ContractRule(typeof(StringNotNullOrEmptyRule), Message = "You need to set the value of the property {1}.")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the eMail-address of the customer.
        /// This property must be a valid email address - that can be checked by a regular expression.
        /// </summary>
        [ContractRule(typeof(StringRegexMatchRule), Parameter = @"^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$")]
        public string EMailAddress { get; set; }
    }
}