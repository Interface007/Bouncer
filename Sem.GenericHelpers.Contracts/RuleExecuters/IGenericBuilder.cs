// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGenericBuilder.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   An interface to access some of the basic operations of a generic rule executer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;

    /// <summary>
    /// An interface to access some of the basic operations of a generic rule executer.
    /// </summary>
    internal interface IGenericBuilder
    {
        /// <summary>
        /// Shortcut for creating an instance of CheckData
        /// </summary>
        /// <returns>a new instance of CheckData</returns>
        IRuleExecuter GetExecutedCheckData();

        /// <summary>
        /// Shortcut for creating an instance of MessageCollector
        /// </summary>
        /// <returns>a new instance of MessageCollector</returns>
        IRuleExecuter GetExecutedMessageCollector();
        
        /// <summary>
        /// Creates an instance of the type provided by <paramref name="executorType"/>.
        /// </summary>
        /// <param name="executorType">the type to be created.</param>
        /// <returns>a new instance of <paramref name="executorType"/></returns>
        IRuleExecuter GetResultExecutor(Type executorType);
    }
}