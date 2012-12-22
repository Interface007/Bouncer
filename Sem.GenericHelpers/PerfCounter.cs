namespace Sem.GenericHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// This class has been incorporated from a private project of Sven Erik Matzen with 
    /// non-exclusive permission to use the source code in this project.
    /// </summary>
    /// <typeparam name="TPerfCounters">The type of the enum that defines the performance counters in the group the concrete class represents.</typeparam>
    public class PerfCounter<TPerfCounters>
    {
        /// <summary>
        /// The resolved handle to the type of <typeparamref name="TPerfCounters"/>.
        /// </summary>
        private readonly Type definitionType;

        /// <summary>
        /// The name of the category <typeparamref name="TPerfCounters"/> represents.
        /// </summary>
        private readonly string categoryName;

        /// <summary>
        /// A cache for performance counter instances to prevent reflection.
        /// </summary>
        private readonly Dictionary<TPerfCounters, PerformanceCounter> cache = new Dictionary<TPerfCounters, PerformanceCounter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PerfCounter{TPerfCounters}"/> class.
        /// </summary>
        /// <exception cref="InvalidOperationException"> In case of the enum not defined correctly. </exception>
        public PerfCounter()
        {
            this.definitionType = typeof(TPerfCounters);
            var attrib = this.definitionType.GetCustomAttributes(typeof(PerfCounterCategoryAttribute), true).FirstOrDefault() as PerfCounterCategoryAttribute;
            if (attrib == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The enum {0} has not bee decorated with a PerfCounterCategoryAttribute.",
                        this.definitionType));
            }

            this.categoryName = attrib.Name;
            if (!PerformanceCounterCategory.Exists(this.categoryName))
            {
                this.SetupCategory();
            }
        }

        /// <summary>
        /// Gets one of the defined performance counter instances.
        /// </summary>
        /// <param name="counterName"> The name of the counter represented by the enum member. </param>
        /// <returns>An instance of the perf counter defined by the enum member.</returns>
        public PerformanceCounter this[TPerfCounters counterName]
        {
            get
            {
                PerformanceCounter performanceCounter;

                if (!this.cache.TryGetValue(counterName, out performanceCounter))
                {
                    var member = this.definitionType.GetMember(counterName.ToString()).FirstOrDefault();
                    if (member == null)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "there is no such counter {0} registered", counterName));
                    }

                    var attrib = member.GetCustomAttributes(typeof(PerfCounterTypeAttribute), true).Cast<PerfCounterTypeAttribute>().FirstOrDefault();
                    if (attrib == null)
                    {
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "the member {0} does not have the required attribute PerformanceCounterCategoryNameAttribute", counterName));
                    }

                    performanceCounter = new PerformanceCounter(this.categoryName, attrib.Name, false);
                    this.cache.Add(counterName, performanceCounter);
                }

                return performanceCounter;
            }
        }

        /// <summary>
        /// Performs a setup of the perf counter group (deletes the group previously).
        /// </summary>
        public void SetupCategory()
        {
            this.Uninstall();

            var creationDataCollection = new CounterCreationDataCollection();
            var member = this.definitionType.GetFields();
            foreach (var fieldInfo in member)
            {
                var attribs = fieldInfo.GetCustomAttributes(typeof(PerfCounterTypeAttribute), true).Cast<PerfCounterTypeAttribute>();
                foreach (var attrib in attribs)
                {
                    creationDataCollection.Add(new CounterCreationData(attrib.Name ?? fieldInfo.Name, attrib.HelpText ?? fieldInfo.Name, attrib.CounterType));
                }
            }

            var attribName = this.definitionType.GetCustomAttributes(typeof(PerfCounterCategoryAttribute), true).FirstOrDefault() as PerfCounterCategoryAttribute;

            try
            {
                // Create the category.
                PerformanceCounterCategory.Create(
                    this.categoryName,
                    attribName == null ? string.Empty : attribName.Help,
                    PerformanceCounterCategoryType.SingleInstance,
                    creationDataCollection);
            }
            catch (UnauthorizedAccessException)
            {
                Debug.WriteLine("Issue with creating the performance counter. You should start the environment inside a previleged session (run as administartor) in order to create the perf-counters, after that start you can simply continue using a normal session.");
            }
        }

        /// <summary>
        /// Uninstalls the perf counter group.
        /// </summary>
        public void Uninstall()
        {
            if (!PerformanceCounterCategory.Exists(this.categoryName))
            {
                return;
            }

            try
            {
                PerformanceCounterCategory.Delete(this.categoryName);
            }
            catch (UnauthorizedAccessException)
            {
                Debug.WriteLine("Issue with deleting the performance counter. You should start the environment inside a previleged session (run as administartor) in order to delete the perf-counters, after that start you can simply continue using a normal session.");
            }
        }
    }
}