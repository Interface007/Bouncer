// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteredRules.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RegisteredRules type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Attributes;

    /// <summary>
    /// This static container class does provide a list of dynamically registered rules.
    /// The registration of rules may happen in code or via configuration.
    /// </summary>
    public static class RegisteredRules
    {
        /// <summary>
        /// List of registered rules.
        /// </summary>
        internal static readonly IList<KeyValuePair<Type, RuleBaseInformation>> TypeRegisteredRules = new List<KeyValuePair<Type, RuleBaseInformation>>();

        /// <summary>
        /// Locking object for thread save access to the list of registered rules.
        /// </summary>
        private static readonly object TypeRegisteredRulesSync = new object();

        /// <summary>
        /// Registers a rule for execution. The execution will happen in the normal processing of the statement
        ///     <code>
        ///         Bouncer
        ///             .For(() => customer)
        ///             .Ensure();
        ///     </code>
        /// </summary>
        /// <param name="rule"> The rule to be registered. </param>
        /// <typeparam name="TData"> The type of data to be validated. </typeparam>
        /// <typeparam name="TParameter"> The type of parameter for the validation. </typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Passing the base type RuleBaseInformation would result in mandatory specification of the data type - this way we can infer the type from usage.")]
        public static void Register<TData, TParameter>(RuleBase<TData, TParameter> rule)
        {
            lock (TypeRegisteredRulesSync)
            {
                TypeRegisteredRules.Add(new KeyValuePair<Type, RuleBaseInformation>(typeof(TData), rule));
            }
        }

        /// <summary>
        /// Registers an <see cref="IEnumerable{T}"/> of rules.
        /// </summary>
        /// <param name="ruleCollection"> The rule collection. </param>
        /// <typeparam name="TData"> The type of data to be validated. </typeparam>
        /// <typeparam name="TParameter"> The type of parameter for the validation. </typeparam>
        public static void RegisterCollection<TData, TParameter>(IEnumerable<RuleBase<TData, TParameter>> ruleCollection)
        {
            if (ruleCollection == null)
            {
                return;
            }

            foreach (var rule in ruleCollection)
            {
                Register(rule);
            }
        }

        /// <summary>
        /// Gets the rules for a given type of data to be validated and a given type of parameter.
        /// </summary>
        /// <typeparam name="TData"> The type of data to be validated. </typeparam>
        /// <typeparam name="TParameter"> The type of parameter for the validation. </typeparam>
        /// <returns> A list of rules. </returns>
        public static IEnumerable<RuleBase<TData, TParameter>> GetRulesForType<TData, TParameter>()
        {
            var valueType = typeof(TData);

            // build a list of "registered" rules
            List<RuleBase<TData, TParameter>> rulesForType;
            lock (TypeRegisteredRulesSync)
            {
                rulesForType = (from x in TypeRegisteredRules
                                where 
                                x.Key == valueType
                                && x.Value as RuleBase<TData, TParameter> != null
                                select x.Value as RuleBase<TData, TParameter>).ToList();
            }

            // get all class-level rule-attributes and enumerate to build list of rules
            // to be excuted for this object instance.
            var attribs = valueType.GetCustomAttributes(typeof(ContractRuleAttribute), true);
            foreach (ContractRuleAttribute attrib in attribs)
            {
                var ruleCollection = attrib.RuleType.GetConstructor(new Type[] { }).Invoke(null) as RuleCollection<TData, TParameter>;
                if (ruleCollection != null)
                {
                    rulesForType.AddRange(ruleCollection);
                }
            }

            return rulesForType;
        }

        /// <summary>
        /// Clears the list of registered rules.
        /// </summary>
        internal static void Clear()
        {
            lock (TypeRegisteredRulesSync)
            {
                TypeRegisteredRules.Clear();
            }
        }
    }
}
