namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;

    /// <summary>
    /// Specifies for a method or property that the result of the call should be cached or should invalidate caches.
    /// Use <see cref="CacheDependencyAttribute"/> to specify the types related to this operation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CacheAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheAttribute"/> class to tell interceptors that the result 
        /// of this method should participate in caching.
        /// </summary>
        /// <param name="cachingAction"> The caching action applied to the method call - to add the result to the cache or 
        /// to remove all cached values with the same dependencies. </param>
        public CacheAttribute(CachingAction cachingAction)
        {
            this.CachingAction = cachingAction;
            this.Lifetime = 5 * 60;
        }

        /// <summary>
        /// Gets the action to be performed in context of caching - to add the result to the cache or 
        /// to remove all cached values with the same dependencies.
        /// </summary>
        public CachingAction CachingAction { get; private set; }

        /// <summary>
        /// Gets or sets the lifetime in seconds of the cached entry - defaults to 5 minutes.
        /// </summary>
        public int Lifetime { get; set; }
    }
}