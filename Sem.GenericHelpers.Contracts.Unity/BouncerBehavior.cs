// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BouncerBehavior.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the BouncerBehavior type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.Unity
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity.InterceptionExtension;

    using Sem.GenericHelpers.Contracts;

    /// <summary>
    /// This interceptor implements a full parameter check for method calls by applying all
    /// declarative Bouncer rules.
    /// </summary>
    public class BouncerBehavior : IInterceptionBehavior
    {
        /// <summary>
        /// Gets a value indicating whether this behavior will actually do anything when invoked.
        /// </summary>
        /// <remarks>
        /// This is used to optimize interception. If the behaviors won't actually
        ///             do anything (for example, PIAB where no policies match) then the interception
        ///             mechanism can be skipped completely.
        /// </remarks>
        public bool WillExecute
        {
            get { return true; }
        }

        /// <summary>
        /// This method will check the parapeters and invoke the target method (or the next interceptor in the chain).
        /// </summary>
        /// <param name="input"> The input parameters. </param>
        /// <param name="getNext"> A delegate that points to the next interception method in the interception chain. </param>
        /// <returns> The result of the method call. </returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            Bouncer
                .For(input.Inputs, i => input.Arguments.ParameterName(i), n => input.Arguments[n], input.MethodBase)
                .Ensure();

            return getNext().Invoke(input, getNext);
        }

        /// <summary>
        /// Returns the interfaces required by the behavior for the objects it intercepts.
        /// </summary>
        /// <returns> The required interfaces. </returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }
    }
}