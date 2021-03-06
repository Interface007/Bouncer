﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bouncer.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Bouncer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using RuleExecuters;

    using Sem.GenericHelpers.Contracts.Attributes;

    /// <summary>
    /// Bouncer http://en.wiktionary.org/wiki/bouncer : „A member of security personnel employed by bars, 
    /// nightclubs, etc to maintain order and deal with patrons who cause trouble.“).
    /// <para>A bouncer can be placed on top of a method to protect against "problematic" data.</para>
    /// </summary>
    public static class Bouncer
    {
        /// <summary>
        /// Creates a <see cref="CheckData{TData}"/> for executing rules by specifying a lambda expression:
        /// <para>Bouncer.ForCheckData(() => MessageOneOk).Assert();</para>
        /// The expression will be executed only once. Specifying lambda expression provides the benefit 
        /// of strong typing for the data name, because the lambda expression can be inspected for the 
        /// variable name.
        /// </summary>
        /// <typeparam name="TData">The type of data the expression returns.</typeparam>
        /// <param name="data">The expression to get the content of the variable.</param>
        /// <returns>A <see cref="CheckData{TData}"/> to execute the tests with.</returns>
        public static CheckData<TData> ForCheckData<TData>(Expression<Func<TData>> data)
        {
            return new CheckData<TData>(data, null);
        }

        /// <summary>
        /// Creates a <see cref="CheckData{TData}"/> for executing rules by specifying a name and the data:
        /// <para>Bouncer.ForCheckData(0, "myInt").Assert();</para>
        /// With this overload you have to specify the name by a string (no compiler-checks!).
        /// You should (if possible) use the overload for a lambda expression instead.
        /// </summary>
        /// <typeparam name="TData">The type of data to be checked.</typeparam>
        /// <param name="data">The data to be checked.</param>
        /// <param name="name">The name of the parameter/variable to be checked.</param>
        /// <returns>A <see cref="CheckData{TData}"/> to execute the tests with.</returns>
        public static CheckData<TData> ForCheckData<TData>(TData data, string name)
        {
            return new CheckData<TData>(name, data, null);
        }

        /// <summary>
        /// Creates a <see cref="MessageCollector{TData}"/> for collecting warnings about rule violations 
        /// by specifying a lambda expression:
        /// <para>var result = Bouncer.ForMessages(() => MessageOneOk).Assert().Results;</para>
        /// The expression will be executed only once. Specifying lambda expression provides the benefit 
        /// of strong typing for the data name, because the lambda expression can be inspected for the 
        /// variable name.
        /// </summary>
        /// <typeparam name="TData">The type of data the expression returns.</typeparam>
        /// <param name="data">The expression to get the content of the variable.</param>
        /// <returns>A <see cref="MessageCollector{TData}"/> to check the rules.</returns>
        public static MessageCollector<TData> ForMessages<TData>(Expression<Func<TData>> data)
        {
            return new MessageCollector<TData>(data, (IEnumerable<ContractMethodRuleAttribute>)null);
        }

        /// <summary>
        /// Creates a data structure for executing rules that collects the result of the rules
        /// as a collection of <see cref="RuleValidationResult"/>. 
        /// <para>var results = Bouncer.ForMessages(0, "myInt").Assert().Results;</para>
        /// With this overload you have to specify the name of the data structure manually.
        /// You should (if possible) use the overload for a lambda expression.
        /// </summary>
        /// <typeparam name="TData">The type of data to be checked.</typeparam>
        /// <param name="data">The data to be checked.</param>
        /// <param name="name">The name of the parameter/variable to be checked.</param>
        /// <returns>A <see cref="MessageCollector{TData}"/> to check the rules.</returns>
        public static MessageCollector<TData> ForMessages<TData>(TData data, string name)
        {
            return new MessageCollector<TData>(name, data, (IEnumerable<ContractMethodRuleAttribute>)null);
        }

        /// <summary>
        /// Creates a <see cref="ConditionalExecution{TData}"/> for executing code if the data violates no rules 
        /// by specifying a lambda expression:
        /// <para>Bouncer.ConditionalExecution(() => MessageOneOk).Assert().ExecuteOnSuccess(() => Console.WriteLine("Success"));</para>
        /// The expression will be executed only once. Specifying lambda expression provides the benefit 
        /// of strong typing for the data name, because the lambda expression can be inspected for the 
        /// variable name.
        /// </summary>
        /// <typeparam name="TData">The type of data the expression returns.</typeparam>
        /// <param name="data">The expression to get the content of the variable.</param>
        /// <returns>A <see cref="ConditionalExecution{TData}"/> to check the rules.</returns>
        public static ConditionalExecution<TData> ForExecution<TData>(Expression<Func<TData>> data)
        {
            return new ConditionalExecution<TData>(data, null);
        }

        /// <summary>
        /// Creates a rule execution class for a value/name pair that executes some code
        /// if all rules are validated successfully. 
        /// With this overload you have to specify the name of the data structure manually.
        /// You should (if possible) use the overload for a lambda expression.
        /// </summary>
        /// <typeparam name="TData">The type of data to be checked.</typeparam>
        /// <param name="data">The data to be checked.</param>
        /// <param name="name">The name of the parameter/variable to be checked.</param>
        /// <returns>A <see cref="ConditionalExecution{TData}"/> to check the rules.</returns>
        public static ConditionalExecution<TData> ForExecution<TData>(TData data, string name)
        {
            return new ConditionalExecution<TData>(name, data, null);
        }

        /// <summary>
        /// Accepts data to be validated with all attached rules.
        /// </summary>
        /// <param name="data">The data to be validated.</param>
        /// <typeparam name="TData">The type of data to be validated.</typeparam>
        /// <returns>A <see cref="GenericBuilder{TData}"/> to add more data via <see cref="GenericBuilder{TData}.For{TDataNew}"/> or execute the check via <see cref="GenericBuilder{TData}.Ensure"/>.</returns>
        public static GenericBuilder<TData> For<TData>(Expression<Func<TData>> data)
        {
            return new GenericBuilder<TData>(data);
        }

        public static IEnumerable<CheckData<object>> For(IList parameters, Func<int, string> parameterNameSelector, Func<string, object> parameterValueSelector, MethodBase methodBase)
        {
            var name = parameterNameSelector(0);
            var current = new CheckData<object>(name, parameterValueSelector(name), methodBase);
            yield return current;

            for (var i = 1; i < parameters.Count; i++)
            {
                var name2 = parameterNameSelector(i);
                current = current.ForCheckData(parameterValueSelector(name2), name2);
                yield return current;
            }
        }

        public static void Ensure(this IEnumerable<CheckData<object>> checks)
        {
            foreach (var checkData in checks)
            {
                checkData.AssertAll();
            }
        }
    }
}