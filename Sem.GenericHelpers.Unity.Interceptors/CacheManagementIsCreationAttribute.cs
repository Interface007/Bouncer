namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;

    /// <summary>
    /// Declares a method parameter as the function/expression that is able to create the content
    /// that should be cached.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class CacheManagementIsCreationAttribute : Attribute
    {
    }
}