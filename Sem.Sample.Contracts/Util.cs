// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Util.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Util type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sample.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Sem.GenericHelpers.Contracts;

    internal static class Util
    {
        internal static void PrintEntries(IEnumerable<RuleValidationResult> results)
        {
            Console.ForegroundColor = ConsoleColor.White;
            
            foreach (var result in results)
            {
                Console.WriteLine(@"----------");
                Console.WriteLine(result);
            }

            Console.WriteLine(@"----------");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void TryCall(string description, Action y, int count = 1, Action postExecution = null)
        {
            var stopwatch = new Stopwatch();
            var firstCall = 0;
            var additionalCalls = 0;
            var additionalCallsTotal = 0;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.WriteLine(description);
            Console.WriteLine();

            try
            {
                stopwatch.Reset();
                stopwatch.Start();
                y.Invoke();
                stopwatch.Stop();
                firstCall = (int)stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();
                stopwatch.Start();
                for (var i = 1; i < count; i++)
                {
                    y.Invoke();
                }
                stopwatch.Stop();
                if (count > 1)
                {
                    additionalCallsTotal = (int)stopwatch.Elapsed.TotalMilliseconds;
                    additionalCalls = additionalCallsTotal / (count - 1);
                }

                if (postExecution != null)
                {
                    postExecution();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(@"Exception caught:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(@"Stacktrace:");
                var stackTrace = ex.StackTrace;
                var indexOfStart = stackTrace.IndexOf("(");
                var indexOfEnd = stackTrace.IndexOf(")");
                while (stackTrace.Contains("(") && indexOfEnd > indexOfStart)
                {
                    stackTrace = stackTrace.Substring(0, indexOfStart) + stackTrace.Substring(indexOfEnd + 1);
                    
                    indexOfStart = stackTrace.IndexOf("(");
                    indexOfEnd = stackTrace.IndexOf(")");
                }

                stackTrace = stackTrace
                    .Replace(@"Sem.Sample.Contracts.", string.Empty)
                    .Replace(@"Sem.GenericHelpers.Contracts.", string.Empty)
                    .Replace(@"C:\CodePlex15\bouncer\", string.Empty)
                    .Replace(@" in ", "\n    in ");

                Console.WriteLine(stackTrace.Substring(0, stackTrace.Length > 300 ? 300 : stackTrace.Length));
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            if (count > 1)
            {
                Console.WriteLine(@"first call: {0}ms, additional calls: {1}ms per call ({2}ms total)", firstCall, additionalCalls, additionalCallsTotal);
            }

            Console.WriteLine(@"press enter to execute next sample...");
            Console.ReadLine();
        }
    }
}
