// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogValidationExecutorUtil.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ExceptionHandlerExecutorUtil type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sample.Contracts.Executers
{
    using Sem.GenericHelpers.Contracts.RuleExecuters;

    public static class LogValidationExecutorUtil
    {
        public static void LogResult<TData>(this DeferredExecution<TData> exec)
        {
            new LogValidationExecutor<TData>(exec.ValueName, exec.Value, exec.MethodRuleAttributes).Assert();
        }
    }
}