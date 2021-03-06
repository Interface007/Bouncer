// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestBase.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the TestBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Unity.Interceptors
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    using Sem.GenericHelpers.Unity.Interceptors;
    using Sem.Test.GenericHelpers.Unity.Interceptors.Probes;

    public class TestBase
    {
        protected static IMethodCallTarget MethodCallTarget()
        {
            var container = new UnityContainer();
            container.AddNewExtension<Interception>();
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterType<IMethodCallTarget, MethodCallTarget>(new Interceptor<InterfaceInterceptor>(), new InterceptionBehavior<MethodResultCaching>());

            var target = container.Resolve<IMethodCallTarget>();
            MethodResultCaching.Clear();

            return target;
        }
    }
}