namespace Sem.GenericHelpers.Unity.Interceptors
{
    /// <summary>
    /// Provides statistical information about a cache entry
    /// </summary>
    public class CacheEntryStatistic
    {
        /// <summary>
        /// Gets or sets the name of the cache entry.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the count of occurrence.
        /// </summary>
        public int Count { get; set; }
    }
}