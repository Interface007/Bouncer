namespace Sem.GenericHelpers.Unity.Interceptors
{
    /// <summary>
    /// Interface to be implemented by providers that validates a cached value.
    /// </summary>
    public interface ICacheItemValidationProvider
    {
        /// <summary>
        /// Validates the entity.
        /// </summary>
        /// <param name="value"> The value to be validated. </param>
        /// <returns> True, if the value is still valid. </returns>
        bool IsValid(CacheMetaBase value);
    }
}