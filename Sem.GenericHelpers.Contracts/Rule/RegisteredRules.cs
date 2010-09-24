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

    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rules;

    public static class RegisteredRules
    {
        internal static readonly IList<KeyValuePair<Type, RuleBaseInformation>> TypeRegisteredRules = new List<KeyValuePair<Type, RuleBaseInformation>>();

        private static readonly object TypeRegisteredRulesSync = new object();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Passing the base type RuleBaseInformation would result in mandatory specification of the data type - this way we can infer the type from usage.")]
        public static void Register<TData, TParameter>(RuleBase<TData, TParameter> rule)
        {
            lock (TypeRegisteredRulesSync)
            {
                TypeRegisteredRules.Add(new KeyValuePair<Type, RuleBaseInformation>(typeof(TData), rule));
            }
        }

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

        public static IEnumerable<RuleBase<TData, TParameter>> GetRulesForType<TData, TParameter>()
        {
            var valueType = typeof(TData);

            // build a list of "registered" rules
            List<RuleBase<TData, TParameter>> rulesForType;
            lock (TypeRegisteredRulesSync)
            {
                rulesForType = (from x in TypeRegisteredRules
                                where x.Key == valueType
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

        internal static void Clear()
        {
            lock (TypeRegisteredRulesSync)
            {
                TypeRegisteredRules.Clear();
            }
        }
    }
}
