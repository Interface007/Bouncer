// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigIsFirstInterceptor.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ConfigIsFirstInterceptor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Unity.TestSample
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    using Sem.GenericHelpers.Contracts.Unity;

    public class VirtualMethodIsFirstInterceptor : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.AddNewExtension<Interception>();

            Container.RegisterType<ICalculator, Calculator>(
                new Interceptor<VirtualMethodInterceptor>(),
                new InterceptionBehavior<BouncerBehavior>(),
                new InterceptionBehavior<LoggingBehavior>());
        }
    }
}