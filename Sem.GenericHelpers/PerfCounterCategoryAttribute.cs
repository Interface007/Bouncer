namespace Sem.GenericHelpers
{
    using System;

    /// <summary>
    /// Describes the performance counter category an enum defines.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    public sealed class PerfCounterCategoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerfCounterCategoryAttribute"/> class.
        /// </summary>
        /// <param name="name"> The name of the category. </param>
        public PerfCounterCategoryAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the help text for the category.
        /// </summary>
        public string Help { get; set; }
    }
}