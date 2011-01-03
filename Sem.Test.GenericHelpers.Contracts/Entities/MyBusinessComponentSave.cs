// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyBusinessComponentSave.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MyBusinessComponentSave type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Sem.Test.GenericHelpers.Contracts.Entities
{
    using System.Collections.Generic;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rule;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Rules;

    [ContractContext("Read")]
    internal class MyBusinessComponentSave 
    {
        /// <summary>
        /// This time we will not prevent executing the method code, but 
        /// just collect the violated rules and print them to the console.
        /// </summary>
        /// <param name="customer"></param>
        internal IEnumerable<RuleValidationResult> CheckCustomerProperties(MyCustomer customer)
        {
            return Bouncer.ForMessages(() => customer).Assert().Results;
        }

        /// <summary>
        /// This time we will not prevent executing the method code, but 
        /// just collect the violated rules and print them to the console.
        /// The rule is a custom one that checks for a specific string inside
        /// a specific property. You might reuse such logic by inheriting 
        /// from RuleBase and initialize the CheckExpression inside the
        /// constructor.
        /// </summary>
        /// <param name="customer"></param>
        internal IEnumerable<RuleValidationResult> CheckCustomerWithCustomRule(MyCustomer customer)
        {
            var results = Bouncer
                .ForMessages(() => customer)
                .Assert(new RuleBase<MyCustomer, object>
                    {
                        Message = "Sven cannot enter this method",
                        CheckExpression = (x, y) => x.FullName != "Sven"
                    }).Results;

            return results;
        }
        
        /// <summary>
        /// This method performs some rule checking by specifying the rules inside 
        /// method parameters. 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="amount"></param>
        /// <param name="theCustomer"></param>
        [ContractMethodRule(typeof(IntegerLowerThanRule), "amount", Parameter = 100)]
        [ContractMethodRule(typeof(StringNotNullOrEmptyRule), "customerId")]
        public IEnumerable<RuleValidationResult> CheckCustomerWithWithMethodAttributes(string customerId, int amount, MyCustomer theCustomer)
        {
            var results = Bouncer
                .ForMessages(() => customerId)
                .ForMessages(() => amount)
                .ForMessages(() => theCustomer)
                .Assert().Results;

            return results;
        }

        /// <summary>
        /// This is a little bit more complex:
        /// 1) we add the context "Create", which enforces the Internal ID to be NULL
        /// 2) we remove the context "Read", which would have enforces the Internal ID to NOT be NULL
        /// 3) we add a set of rules from the custom class StrictCustomerCheckRuleSet, which contains a 
        ///    set of two rules (might be much more) that can be changed at a single point for all 
        ///    methods decorated with the attribute [MethodRule(typeof(StrictCustomerCheckRuleSet), "customer")]
        /// </summary>
        /// <param name="customer">The customer to be created (does not have an internal id)</param>
        [ContractContext("Create")]
        [ContractContext("Read", false)]    // this is important, because the class has an attribute "[ContractContext("Read")]"
        [ContractMethodRule(typeof(StrictCustomerCheckRuleSet), "customer")]
        internal IEnumerable<RuleValidationResult> InsertCustomer(MyCustomer customer)
        {
            var results = Bouncer.ForMessages(() => customer).Assert().Results;
            return results;
        }

        public static string GetStackTraceRuleValidationException()
        {
            try
            {
                MyCustomer.ThrowRuleValidationException();
            }
            catch (Exception ex)
            {
                return ex.StackTrace;
            }

            return string.Empty;
        }
    }
}