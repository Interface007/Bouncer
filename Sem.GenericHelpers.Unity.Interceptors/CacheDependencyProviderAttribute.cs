namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;

    /// <summary>
    /// Attaches a type implementing the <see cref="ICacheDependencyProvider"/> to a method. Using this
    /// interface a type can generate a list of dependency names.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class CacheDependencyProviderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheDependencyProviderAttribute"/> class.
        /// </summary>
        /// <param name="cacheDependencyProviderType"> The cache dependency provider type (must implement <see cref="ICacheDependencyProvider"/>). </param>
        public CacheDependencyProviderAttribute(Type cacheDependencyProviderType)
        {
            this.CacheDependencyProviderType = cacheDependencyProviderType;
        }

        /// <summary>
        /// Gets the dependency provider that implement <see cref="ICacheDependencyProvider"/>.
        /// </summary>
        public Type CacheDependencyProviderType { get; private set; }

        /// <summary>
        /// Gets or sets ctor parameters for the provider.
        /// </summary>
        public object[] CtorParameters { get; set; }
    }
}