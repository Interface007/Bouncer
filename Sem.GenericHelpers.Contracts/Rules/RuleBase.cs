// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleBase.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rules
{
    using System;

    public class RuleBase<TData, TParameter> : RuleBaseInformation
    {
        public Func<TData, TParameter, bool> CheckExpression { get; set; }
    }
}