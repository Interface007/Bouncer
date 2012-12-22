namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;

    /// <summary>
    /// Attaches a type implementing the <see cref="ICacheItemValidationProvider"/> to a method. Using this
    /// interface a validation (e.g. based on the content of a table) can be implemented.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class CacheValidationProviderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheValidationProviderAttribute"/> class.
        /// </summary>
        /// <param name="cacheValidationProviderType"> The cache validation provider type that must implement <see cref="ICacheItemValidationProvider"/>. </param>
        public CacheValidationProviderAttribute(Type cacheValidationProviderType)
        {
            this.CacheValidationProviderType = cacheValidationProviderType;
        }

        /// <summary>
        /// Gets the validation provider type that implements <see cref="ICacheItemValidationProvider"/>.
        /// </summary>
        public Type CacheValidationProviderType { get; private set; }

        /// <summary>
        /// Gets an instance of the validation provider type that implements <see cref="ICacheItemValidationProvider"/>.
        /// </summary>
        public ICacheItemValidationProvider Instance
        {
            get
            {
                return Activator.CreateInstance(this.CacheValidationProviderType) as ICacheItemValidationProvider;
            }
        }
    }
}