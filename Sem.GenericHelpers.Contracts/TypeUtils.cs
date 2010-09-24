// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeUtils.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Extension methods for System.Type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts
{
    using System;
    using System.Linq;

    using Sem.GenericHelpers.Contracts.Rule;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// Extension methods for System.Type
    /// </summary>
    internal static class TypeUtils
    {
        /// <summary>
        /// Checks whether the type does implement a certain interface.
        /// </summary>
        /// <param name="toTest">The type to be checked.</param>
        /// <param name="interfaceToImplement">The interface <paramref name="toTest"/> should implement.</param>
        /// <returns>True if the interface is implemented by this type.</returns>
        internal static bool Implements(this Type toTest, Type interfaceToImplement)
        {
            return (from i in toTest.GetInterfaces()
                    where (i.IsGenericType && i.GetGenericTypeDefinition() == interfaceToImplement)
                          || i == interfaceToImplement
                    select i).Count() != 0;
        }

        /// <summary>
        /// Creates and initializes a rule using a generic parameter of <paramref name="valueType"/>.
        /// </summary>
        /// <param name="ruleType">The type of rule to be created.</param>
        /// <param name="valueType">The type of the value that should be checked with the rule.</param>
        /// <returns>A new rule instance of the specified type.</returns>
        internal static RuleBaseInformation CreateRule(this Type ruleType, Type valueType)
        {
            var constructorInfo = ruleType.GetConstructor(Type.EmptyTypes);
            
            if (constructorInfo.ContainsGenericParameters)
            {
                return ruleType
                    .GetGenericTypeDefinition()
                    .MakeGenericType(valueType)
                    .GetConstructor(new Type[] { })
                    .Invoke(null) as RuleBaseInformation;
            }
            
            return (RuleBaseInformation)constructorInfo.Invoke(null);
        }
    }
}
