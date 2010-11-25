// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImplementsInterfaceRule.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ImplementsInterfaceRule type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using System;

    using Properties;
    using Rule;

    /// <summary>
    /// A rule to check whether the object does implement a certain interface.
    /// </summary>
    /// <typeparam name="TData">The type of the object to be checked.</typeparam>
    public class ImplementsInterfaceRule<TData> : RuleBase<TData, Type>
        where TData : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImplementsInterfaceRule{TData}"/> class.
        /// </summary>
        public ImplementsInterfaceRule()
        {
            Message = Resources.ImplementsInterfaceRuleStandardMessage;

            // here we cannot use the typeof(TData), because we need the type of the real object.
            CheckExpression = 
                (data, interfaceToImplement) 
                    => data != null
                    && data.GetType().Implements(interfaceToImplement);
        }
    }
}
