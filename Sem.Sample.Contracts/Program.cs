﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sample.Contracts
{
    using System;

    using Sem.GenericHelpers.Contracts.Configuration;
    using Sem.Sample.Contracts.Entities;

    public class Program
    {
        public static void Main(string[] args)
        {
            var businessComponent = new MyBusinessComponent();
            var businessComponentSave = new MyBusinessComponentSave();

            Util.TryCall(
                "We'll call a method that relies on\n\n" +
                "customer.InternalId.ToString()\n\n" +
                "while InternalId is not set, this will cause an exception (do\n" +
                "you see any hint, what property causes the exception?):",
                () => businessComponent.WriteCustomerProperties(new MyCustomer { EMailAddress = Resources.ValidEmailAddress }));

            Util.TryCall(
                "This call will use a method including the statement\n\n" +
                "Bouncer.For(() => customer).Assert();\n\n" +
                "The bouncer will tell you the root of the error inside the message of the \n" +
                "exception and will throw the exception in the first method that can detect the \n" +
                "issue. Also it will give you the property name:",
                () => businessComponentSave.WriteCustomerProperties(new MyCustomer { EMailAddress = Resources.ValidEmailAddress }));

            Util.TryCall(
                "In this example we will print out a list of messages from the statement\n\n" +
                "var results = Bouncer.ForMessages(() => saveBusinessObject).Assert();\n\n" +
                "This does not throw an exception, but executes all checks and returns \n" +
                "a list of issues.",
                () => businessComponentSave.CheckCustomerProperties(new MyCustomer { EMailAddress = Resources.ValidEmailAddress }));

            AddLogging(
                "Rule executers do also support central inspection of rule evaluation results.\n" +
                "In our example we can activate logging to the screen by adding an System.Action\n" +
                "to the Bouncer.AfterInvokeAction list, that will simply log some information to\n" +
                "the console in ConsoleColor.Yellow.\n\n" +
                "You might wonder why you get more logs than rule violations: the logging will\n" +
                "get any validation result - even the successfully validated data.\n\n" +
                "Enter >>L<< to activate logging.");

            Util.TryCall(
                "Lets execute a completely new rule (\".ForMessages(() => customer)\" again).\n\n" +
                "Rule   : (x, y) => x.FullName != \"Sven\"\n" +
                "Message: \"Sven cannot enter this method\"\n\n" +
                "This does not throw an exception, but executes all checks and returns \n" +
                "a list of issues.",
                () => businessComponentSave.CheckCustomerWithCustomRule(new MyCustomer { FullName = "Sven" }));

            Util.TryCall(
                "We'll call a method containing a MethodRuleAttribute that defines a\n" +
                "rule for its parameter. This way we nearly completely are declarative.\n" +
                "Unfortunately we cannot enforce the parameter name to match to the method\n" +
                "signature at compile time.",
                () => businessComponentSave.CheckCustomerWithWithMethodAttributes(string.Empty, 1000, new MyCustomer { EMailAddress = Resources.ValidEmailAddress }));

            Util.TryCall(
                "We also can use a string as a context description for the call. E.g. we can\n" +
                "tell the engine that we currently are creating new data, so we cannot set the\n" +
                "internal id (this will be generated by the database while inserting the data).\n" +
                "In this case this 'context' will activate some rules and deactivate others.\n" +
                "In our case, one rule for InternalId will be replaced with the exact opposit:\n" +
                "for InternalId => 'IsNullRule' instead of 'IsNotNullRule'.\n" +
                "The rules for specifying the full name and a valid email address (the string\n" +
                "\"don't@spam.me\" does contain a '-char) are still active.",
                () => businessComponentSave.TryInsertCustomer(new MyCustomer { FullName = "Sven", EMailAddress = "don't@spam.me" }));

            Util.TryCall(
                "Now configurable rules. The following rule validation is only inside the \n" +
                "app.config. You can simply add/remove additional checks inside the \n" +
                "configuration file.",
                () => businessComponentSave.WriteCustomerConfiguration(new MyCustomer
                    {
                        InternalId = new CustomerId(),
                        FullName = "Sven",
                        EMailAddress = Resources.ValidEmailAddress,
                        PhoneNumber = "00000000000"
                    }));

            var checkCount = 0;
            BouncerConfiguration.AddAfterInvokeAction(x => { checkCount++; });

            Util.TryCall(
                    "This time we will look for performance. How much impact does Bouncer\n" +
                    "have for executing a method call with one parameter?",
                    () => businessComponentSave.InsertCustomer(new MyCustomer
                        {
                            FullName = "Karl Klammer",
                            EMailAddress = Resources.ValidEmailAddress
                        }),
                    100,
                    () => Console.WriteLine(@"In this example we had {0} rule checks per call.", checkCount / 100));

            checkCount = 0;
            Util.TryCall(
                    "And another test with more parameters. This one will run much faster\n" +
                    "for the first call, because the configuration and some type information\n" +
                    "has already been cached.",
                    () => businessComponentSave.InsertCustomer2(new MyCustomer
                        {
                            FullName = "Karl Klammer",
                            EMailAddress = Resources.ValidEmailAddress,
                            InternalId = new CustomerId()
                        },
                        "someConnectionString",
                        1564,
                        new CustomerId()),
                    100,
                    () => Console.WriteLine(@"In this time we had {0} rule checks per call.", checkCount / 100));
        }

        private static void AddLogging(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.WriteLine(message);

            var input = Console.ReadLine();
            if (input == null)
            {
                return;
            }

            if (input.ToUpperInvariant() == "L")
            {
                BouncerConfiguration.AddAfterInvokeAction(x =>
                    {
                        var c = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(
                            @"LOG: " + x.RuleType.Name + 
                            @"  => " + x.Result + 
                            (x.Result ? " ... ok" :
                            @" for " + x.ValueName + 
                            @" ... " + x.Message));
                        Console.ForegroundColor = c;
                    });
            }
        }
    }
}
