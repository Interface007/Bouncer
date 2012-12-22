namespace Sem.Test.GenericHelpers.Unity.Interceptors
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    using Sem.GenericHelpers.Unity.Interceptors;

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