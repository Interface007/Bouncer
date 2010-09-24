// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyBusinessComponentSave.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MyBusinessComponentSave type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sample.Contracts.Entities
{
    using System;
    using System.Threading;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Sample.Contracts.Executers;
    using Sem.Sample.Contracts.Rules;

    [ContractContext("Read")]
    internal class MyBusinessComponentSave : MyBusinessComponent
    {
        /// <summary>
        /// We need to use the type MySaveCustomer in order to correctly resolve type inference for  
        /// Bouncer.ForCheckData(() => customer).Assert();
        /// </summary>
        /// <param name="customer">this customer type does have rule-attributes attached to its properties</param>
        internal new void WriteCustomerProperties(MyCustomer customer)
        {
            Bouncer
                .ForCheckData(() => customer)
                .Assert();

            Console.WriteLine(
                Resources.CallingCustomerInfo,
                GetTheName(customer),
                FormatTheId(customer));
        }

        /// <summary>
        /// This time we will not prevent executing the method code, but 
        /// just collect the violated rules and print them to the console.
        /// </summary>
        /// <param name="customer">This object is decorated with attributes.</param>
        internal void CheckCustomerProperties(MyCustomer customer)
        {
            var results = Bouncer
                .ForMessages(() => customer)
                .Assert();

            Util.PrintEntries(results);
        }

        /// <summary>
        /// This time we will not prevent executing the method code, but 
        /// just collect the violated rules and print them to the console.
        /// The rule is a custom one that checks for a specific string inside
        /// a specific property. You might reuse such logic by inheriting 
        /// from RuleBase and initialize the CheckExpression inside the
        /// constructor.
        /// </summary>
        /// <param name="customer">This object is decorated with attributes.</param>
        internal void CheckCustomerWithCustomRule(MyCustomer customer)
        {
            var results = Bouncer
                .ForMessages(() => customer)
                .Assert(new RuleBase<MyCustomer, object>
                    {
                        Message = "Sven cannot enter this method",
                        CheckExpression = (x, y) => x.FullName != "Sven"
                    });

            Util.PrintEntries(results);
            Console.WriteLine(@"---> ForMessages did return the validation results, but  <---");
            Console.WriteLine(@"--->   did not cause any exception. So 'customer Sven'   <---");
            Console.WriteLine(@"--->   did enter the method  and did execute all code.   <---");
        }

        /// <summary>
        /// This method performs some rule checking by specifying the rules inside 
        /// method parameters. 
        /// </summary>
        /// <param name="customerId">the attribute for customer id declares this to be not null</param>
        /// <param name="amount">the amounte must not be > 99</param>
        /// <param name="theCustomer">This object is decorated with attributes.</param>
        [ContractMethodRule(typeof(IntegerLowerThanRule), "amount", Parameter = 100)]
        [ContractMethodRule(typeof(StringNotNullOrEmptyRule), "customerId")]
        internal void CheckCustomerWithWithMethodAttributes(string customerId, int amount, MyCustomer theCustomer)
        {
            Bouncer
                .ForCheckData(() => customerId)
                .ForCheckData(() => amount)
                .ForCheckData(() => theCustomer)
                .Assert();

            var results = Bouncer
                .ForMessages(() => customerId)
                .ForMessages(() => amount)
                .ForMessages(() => theCustomer)
                .Assert();

            Util.PrintEntries(results);
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
        [ContractContext("Read", false)]
        [ContractMethodRule(typeof(StrictCustomerCheckRuleSet), "customer")]
        internal void InsertCustomer(MyCustomer customer)
        {
            var results = Bouncer
                .ForMessages(() => customer)
                .Assert();

            Util.PrintEntries(results);
        }

        /// <summary>
        /// We need to use the type MySaveCustomer in order to correctly resolve type inference for  
        /// Bouncer.ForCheckData(() => customer).Assert();
        /// </summary>
        /// <param name="customer">this customer type does have rule-attributes attached to its properties</param>
        [ContractContext("Config")]
        internal new void WriteCustomerConfiguration(MyCustomer customer)
        {
            Bouncer
                .For(() => customer)
                .Enforce();

            Console.WriteLine(
                Resources.CallingCustomerInfo,
                GetTheName(customer),
                FormatTheId(customer));
        }

        public void WriteCustomerCustomExecutor(MyCustomer customer)
        {
            Bouncer
                .For(() => customer)
                .LogResult();

            Console.WriteLine(
                Resources.CallingCustomerInfo,
                GetTheName(customer),
                FormatTheId(customer));
        }
    }
}