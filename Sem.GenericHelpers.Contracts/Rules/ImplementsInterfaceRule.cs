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

    using Sem.GenericHelpers.Contracts.Properties;

    public class ImplementsInterfaceRule<TData> : RuleBase<TData, Type>
        where TData : class
    {
        public ImplementsInterfaceRule()
        {
            Message = Resources.ImplementsInterfaceRuleStandardMessage;
            CheckExpression = 
                (data, interfaceToImplement) 
                    => data != null
                    && data.GetType().Implements(interfaceToImplement);
        }
    }
}
