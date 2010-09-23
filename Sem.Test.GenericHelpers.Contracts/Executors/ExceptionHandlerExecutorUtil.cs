// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionHandlerExecutorUtil.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ExceptionHandlerExecutorUtil type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.Contracts.Executors
{
    using Sem.GenericHelpers.Contracts.RuleExecuters;

    public static class ExceptionHandlerExecutorUtil
    {
        public static void Execute<TData>(this DeferredExecution<TData> exec)
        {
            new CheckData<TData>(exec.ValueName, exec.Value, exec.MethodRuleAttributes).Assert();
        }
    }
}