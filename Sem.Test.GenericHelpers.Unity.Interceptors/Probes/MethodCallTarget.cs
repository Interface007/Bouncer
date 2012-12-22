namespace Sem.Test.GenericHelpers.Unity.Interceptors.Probes
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

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

        public int Calls { get
        {
            return this.cachedMethodCalls.Sum(x => x.Value);
        }}

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

        [Cache(CachingAction.CachePublic, Lifetime = 1)]
        [CacheDependency(typeof(DateTime))]
        public DateTime CachedTime()
        {
            this.Increase();
            return DateTime.Now;
        }

        [Cache(CachingAction.CachePublic)]
        public int CachedMethodWithOverload()
        {
            this.Increase();
            return 32;
        }

        [Cache(CachingAction.CachePublic)]
        public int CachedMethodWithOverload(string value)
        {
            this.Increase();
            return 42;
        }

        [Cache(CachingAction.Invalidate)]
        [CacheDependency(typeof(DateTime))]
        public void InvalidateCachedTime()
        {
        }

        [Cache(CachingAction.CachePublic)]
        public int CachedMethodWithOverload(int value)
        {
            this.Increase();
            return 7;
        }

        private void Increase([CallerMemberName] string methodName = "")
        {
            this.cachedMethodCalls.AddOrUpdate(methodName, s => 1, (s, i) => i + 1);
        }

        private void Increase(Expression<Func<IMethodCallTarget, object>> method)
        {
            var methodName = ((MethodCallExpression)((UnaryExpression)method.Body).Operand).Method.Name;
            
            // ReSharper disable ExplicitCallerInfoArgument
            this.Increase(methodName);
            // ReSharper restore ExplicitCallerInfoArgument
        }
    }
}