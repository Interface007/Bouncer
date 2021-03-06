// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamAttributes.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Represents the attributes of a file stream.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.IO.Ads
{
    using System;

    /// <summary>
    /// Represents the attributes of a file stream.
    /// </summary>
    [Flags]
    public enum FileStreamAttributes
    {
        /// <summary>
        /// No attributes.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Set if the stream contains data that is modified when read.
        /// </summary>
        ModifiedWhenRead = 1,
        
        /// <summary>
        /// Set if the stream contains security data.
        /// </summary>
        ContainsSecurity = 2,
        
        /// <summary>
        /// Set if the stream contains properties.
        /// </summary>
        ContainsProperties = 4,
        
        /// <summary>
        /// Set if the stream is sparse.
        /// </summary>
        Sparse = 8,
    }
}
