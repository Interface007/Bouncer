// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingBehavior.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the LoggingBehavior type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Unity.TestSample
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity.InterceptionExtension;

    public class LoggingBehavior : IInterceptionBehavior
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            Console.Write(@"Method " + input.MethodBase.Name + @" invoked.");
            return getNext().Invoke(input, getNext);
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}