namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Special caching type to include meta information (date and time when the cache value is invalid).
    /// </summary>
    /// <typeparam name="TResult"> The type of value that is cached. </typeparam>
    [Serializable]
    [DebuggerDisplay("{Key} = {Object}")]
    public class CacheMetaValue<TResult> : CacheMetaBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheMetaValue{TResult}"/> class.
        /// </summary>
        public CacheMetaValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheMetaValue{TResult}"/> class.
        /// </summary>
        /// <param name="cachedValue"> The cached value. </param>
        public CacheMetaValue(TResult cachedValue)
        {
            this.Object = cachedValue;
        }

        /// <summary>
        /// Gets or sets the cached object value.
        /// </summary>
        public TResult Object { get; set; }

        /// <summary>
        /// Sets the internal object of this meta value.
        /// </summary>
        /// <param name="value"> The value to be set. </param>
        public override void SetObject(object value)
        {
            var cacheValue = value as CacheMetaValue<TResult>;
            if (cacheValue != null)
            {
                this.DependencyValues = cacheValue.DependencyValues;
                this.CreationDate = cacheValue.CreationDate;
                this.Key = cacheValue.Key;
                this.LocalIdentifier = cacheValue.LocalIdentifier;
                this.Object = cacheValue.Object;
                this.ValidUntil = cacheValue.ValidUntil;
                return;
            }

            this.Object = (TResult)value;
        }
    }
}