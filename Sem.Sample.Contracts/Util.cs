namespace Sem.Sample.Contracts
{
    using System;

    using Sem.GenericHelpers.Contracts.RuleExecuters;
    using Sem.Sample.Contracts.Entities;

    internal static class Util
    {
        internal static void PrintEntries(MessageCollector<MyCustomer> results)
        {
            Console.ForegroundColor = ConsoleColor.White;
            
            foreach (var result in results.Results)
            {
                Console.WriteLine("----------");
                Console.WriteLine(result);
            }

            Console.WriteLine("----------");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void TryCall(string description, Action y)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.WriteLine(description);
            Console.WriteLine();

            try
            {
                y.Invoke();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Exception caught:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace.Substring(0, 300));
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("press enter to execute next sample...");
            Console.ReadLine();
        }
    }
}
