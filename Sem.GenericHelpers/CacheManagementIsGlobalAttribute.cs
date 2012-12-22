namespace Sem.GenericHelpers
{
    using System;

    /// <summary>
    /// Declares a method parameter as the boolean identifier for the result being "global cachable".
    /// If the value of the specified parameter is "false", the result of the method depends on the
    /// user currently in context, so the cache key must include the identifier for this user.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class CacheManagementIsGlobalAttribute : Attribute
    {
    }
}