namespace Sem.Test.GenericHelpers.Unity.Interceptors.Probes
{
    using System;
    using System.Linq.Expressions;

    public interface IMethodCallTarget
    {
        int CachedMethod();

        int GetCalls(Expression<Func<IMethodCallTarget, object>> method);

        int CachedMethodWithSimpleParameters(string value1, int value2, DateTime value3);

        int NonCachedMethodWithSimpleParameters(string value1, int value2, DateTime value3);

        DateTime CachedTime();

        int CachedMethodWithOverload();

        int CachedMethodWithOverload(int value);

        int CachedMethodWithOverload(string value);

        int Calls { get; }

        void InvalidateCachedTime();
    }
}