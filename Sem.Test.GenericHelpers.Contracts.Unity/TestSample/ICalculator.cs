// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICalculator.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ICalculator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Unity.TestSample
{
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// This interface declares a method with a rule.
    /// </summary>
    public interface ICalculator
    {
        /// <summary>
        /// A method decorated with a contract attribute that declares the method to must have y > 3
        /// </summary>
        /// <param name="x"> The x can be of any value. </param>
        /// <param name="y"> The y must be greater than 2. </param>
        /// <param name="z"> The z  can be of any value. </param>
        /// <returns> the sum of both values </returns>
        [ContractMethodRule(typeof(IntegerGreaterThanRule), "y", Parameter = 2)]
        int Add(
            int x,
            int y,
            int z);
    }
}