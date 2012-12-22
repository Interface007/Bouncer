namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;

    /// <summary>
    /// Defines that the result of the method depends on the underlying persistance type.
    /// This means for a database entity, that each time the content of the table is
    /// changed, a cached result of this method might become invalid and must be deleted from cache.
    /// </summary>
    /// <remarks>Since this attribute is interpreted by interface-interceptors, only the directly attached 
    /// attributes to public methods implementing an interface and being called through a 
    /// "unity-created instance" (built by the DI container) will be seen. When you call other methods 
    /// from inside the decorated method, the attributes of all indirectly called methods (e.g. private 
    /// methods of the target class) will NOT be seen by the interceptor. You MUST decorate all 
    /// dependencies directly to enable the method cache interceptor to delete the appropriate entries 
    /// from the cache.</remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class CacheDependencyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheDependencyAttribute"/> class and declares
        /// a dependency of the method's output to some <see cref="Type"/> - most likely a data-access-entity.
        /// </summary>
        /// <param name="entityType"> The entity type this method depends on. </param>
        public CacheDependencyAttribute(Type entityType)
        {
            this.EntityType = entityType;
        }

        /// <summary>
        /// Gets the entity type this method depends on. When this attribute is combined with an 
        /// <see cref="CacheAttribute"/> having <see cref="CacheAttribute.CachingAction"/> equal 
        /// to <see cref="CachingAction.CachePublic"/> or <see cref="CachingAction.CachePrivate"/>, 
        /// the return value of the method will be cached. If the <see cref="CacheAttribute.CachingAction"/> 
        /// is equal to <see cref="CachingAction.Invalidate"/>, after execution of the method all 
        /// cached values that do have at least one matching dependency to the value of this property 
        /// will be deleted from the cache.
        /// </summary>
        public Type EntityType { get; private set; }
    }
}