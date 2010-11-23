// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleBase.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the RuleBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Rule
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// This base class provides the basis for all rule classes.
    /// </summary>
    /// <typeparam name="TData">The type of data to be checked.</typeparam>
    /// <typeparam name="TParameter">The parameter type for the checl.</typeparam>
    [Serializable]
    public class RuleBase<TData, TParameter> : RuleBaseInformation
    {
        /// <summary>
        /// Gets or sets the expression to perform the check.
        /// </summary>
        [XmlIgnore]
        public Func<TData, TParameter, bool> CheckExpression { get; set; }
    }
}