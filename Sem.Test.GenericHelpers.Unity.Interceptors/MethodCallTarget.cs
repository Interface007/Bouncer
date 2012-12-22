namespace Sem.Test.GenericHelpers.Unity.Interceptors
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq.Expressions;

    using Sem.GenericHelpers.Unity.Interceptors;

    public class MethodCallTarget : IMethodCallTarget
    {
        private readonly ConcurrentDictionary<string, int> cachedMethodCalls;

        public MethodCallTarget()
        {
            this.cachedMethodCalls = new ConcurrentDictionary<string, int>();
        }

        [Cache(CachingAction.CachePublic)]
        public int CachedMethod()
        {
            this.Increase(x => x.CachedMethod());
            return 42;
        }

        public int GetCalls(Expression<Func<IMethodCallTarget, object>> method)
        {
            var methodName = ((MethodCallExpression)((UnaryExpression)method.Body).Operand).Method.Name;
            int value;
            return this.cachedMethodCalls.TryGetValue(methodName, out value) ? value : 0;
        }

        [Cache(CachingAction.CachePublic)]
        public int CachedMethodWithSimpleParameters(string value1, int value2, DateTime value3)
        {
            this.Increase(x => x.CachedMethodWithSimpleParameters(value1, value2, value3));
            return 42;
        }

        public int NonCachedMethodWithSimpleParameters(string value1, int value2, DateTime value3)
        {
            this.Increase(x => x.NonCachedMethodWithSimpleParameters(value1, value2, value3));
            return 42;
        }

        private void Increase(Expression<Func<IMethodCallTarget, object>> method)
        {
            var methodName = ((MethodCallExpression)((UnaryExpression)method.Body).Operand).Method.Name;
            this.cachedMethodCalls.AddOrUpdate(methodName, s => 1, (s, i) => i + 1);
        }
    }
}