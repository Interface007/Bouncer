// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMessageCollector.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the IMessageCollector type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System.Collections.Generic;

    public interface IMessageCollector
    {
        IEnumerable<RuleValidationResult> Results { get; }
    }
}