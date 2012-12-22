namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface to be implemented by providers that generate lists of dependency names.
    /// </summary>
    public interface ICacheDependencyProvider
    {
        /// <summary>
        /// Generates a list of dependency name.
        /// </summary>
        /// <returns> A list of dependency names.</returns>
        IEnumerable<string> Dependency();
    }
}