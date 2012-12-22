namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System.Diagnostics;

    /// <summary>
    /// Defines the performance counter category "Sem.Unity.Interceptors.MethodResultCache".
    /// </summary>
    [PerfCounterCategory("Sem.Unity.Interceptors.MethodResultCache")]
    public enum CachePerfCounter
    {
        /// <summary>
        /// Defines the performance counter for cache miss events.
        /// </summary>
        [PerfCounterType(PerformanceCounterType.CounterDelta64, Name = "# of cache miss events")]
        CacheMiss,

        /// <summary>
        /// Defines the performance counter for cache hit events.
        /// </summary>
        [PerfCounterType(PerformanceCounterType.CounterDelta64, Name = "# of cache hit events")]
        CacheHit
    }
}