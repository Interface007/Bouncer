namespace Sem.GenericHelpers.Unity.Interceptors
{
    using System;

    /// <summary>
    /// Declares a method parameter as an expression that points to a method or member of a class
    /// which does have the dependency attributes attached that describe the dependencies of the
    /// value being cached.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class CacheManagementIsDependencyHintAttribute : Attribute
    {
    }
}