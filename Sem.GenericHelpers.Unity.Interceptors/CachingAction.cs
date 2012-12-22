namespace Sem.GenericHelpers.Unity.Interceptors
{
    /// <summary>
    /// Defines caching behavior.
    /// </summary>
    public enum CachingAction
    {
        /// <summary>
        /// Dont's interact with the cache.
        /// </summary>
        None,

        /// <summary>
        /// Cache the result and share it with everybody.
        /// </summary>
        CachePublic,

        /// <summary>
        /// Cache, but don't share this entry.
        /// </summary>
        CachePrivate,

        /// <summary>
        /// Delete all entries from the cache that depend on any dependency methioned for this method.
        /// </summary>
        Invalidate,

        AppendManagement
    }
}