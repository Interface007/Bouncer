namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;

    /// <summary>
    /// Declares a method parameter as the cache-name-providing parameter (must be string or must
    /// return a qualified string when calling ".ToString()" on that object instance).
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class CacheManagementIsNameAttribute : Attribute
    {
    }
}