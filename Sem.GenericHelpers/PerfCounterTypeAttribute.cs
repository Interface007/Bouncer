namespace Sem.GenericHelpers
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Describes a perf counter a member of an enum defines
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PerfCounterTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerfCounterTypeAttribute"/> class.
        /// </summary>
        /// <param name="counterType"> The counter type of the performance counter. </param>
        public PerfCounterTypeAttribute(PerformanceCounterType counterType)
        {
            this.CounterType = counterType;
        }

        /// <summary>
        /// Gets or sets the name of the performance counter - this must be unique inside one category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the HelpText of the performance counter.
        /// </summary>
        public string HelpText { get; set; }

        /// <summary>
        /// Gets the type of counter to instanciate (describes the algorithm to calculate the value).
        /// </summary>
        public PerformanceCounterType CounterType { get; private set; }
    }
}