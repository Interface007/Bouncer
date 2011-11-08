// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Calculator.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements a concrete class of interface <see cref="ICalculator" />, which adds a
//   contract attribute to specify thet x must be greater than x, too.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Unity.TestSample
{
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// Implements a concrete class of interface <see cref="ICalculator"/>, which adds a
    /// contract attribute to specify thet x must be greater than x, too.
    /// </summary>
    public class Calculator : ICalculator
    {
        /// <summary>
        /// Method that contains a method rule.
        /// </summary>
        /// <param name="x"> The x must be greater than 3. </param>
        /// <param name="y"> The y follows the declaration for the interface. </param>
        /// <param name="y"> The z contains a rule assigned to the parameter directly. </param>
        /// <returns>
        /// </returns>
        [ContractMethodRule(typeof(IntegerGreaterThanRule), "x", Parameter = 1)]
        public virtual int Add(
            int x, 
            int y,
            [ContractParameterRule(typeof(IntegerGreaterThanRule), Parameter = 3)]int z)
        {
            return x + y + z;
        }

        public string Description { get; set; }
    }
}