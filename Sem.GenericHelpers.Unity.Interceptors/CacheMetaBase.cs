namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base type for a cache meta value that attaches additional information to a cached entity.
    /// </summary>
    [Serializable]
    public class CacheMetaBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheMetaBase"/> class.
        /// </summary>
        public CacheMetaBase()
        {
            this.CreationDate = DateTime.UtcNow;
            this.DependencyValues = new List<string>();
        }

        /// <summary>
        /// Gets or sets a datetime value when the cached value becomes invalid..
        /// </summary>
        public DateTime ValidUntil { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets an Instance-key, that is combined with each item-key in case of non-global 
        /// values (all values that depend on the current user)
        /// </summary>
        public string LocalIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a list of names of dependencies the value of this entity depends on. If
        /// any of the dependencies changes, this cached value must be invalidated.
        /// </summary>
        public List<string> DependencyValues { get; set; }

        /// <summary>
        /// Gets or sets the cache-key of this entity.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Sets the internal object of this meta value.
        /// </summary>
        /// <param name="value"> The value to be set. </param>
        public virtual void SetObject(object value)
        {
        }

        /// <summary>
        /// Creates a clone of the base information.
        /// </summary>
        /// <returns>A clone.</returns>
        public CacheMetaBase CloneBase()
        {
            return new CacheMetaBase
                       {
                           CreationDate = this.CreationDate,
                           DependencyValues = this.DependencyValues.ToList(),
                           Key = this.Key,
                           LocalIdentifier = this.LocalIdentifier,
                           ValidUntil = this.ValidUntil,
                       };
        }
    }
}