namespace Sem.Test.GenericHelpers.Unity.Interceptors
{
    using System;
    using System.Linq.Expressions;

    public interface IMethodCallTarget
    {
        int CachedMethod();

        int GetCalls(Expression<Func<IMethodCallTarget, object>> method);

        int CachedMethodWithSimpleParameters(string value1, int value2, DateTime value3);

        int NonCachedMethodWithSimpleParameters(string value1, int value2, DateTime value3);
    }
}