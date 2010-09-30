namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;

    internal interface IGenericBuilder
    {
        /// <summary>
        /// Shortcut for creating an instance of CheckData
        /// </summary>
        /// <returns>a new instance of CheckData</returns>
        IRuleExecuter GetExecutedCheckData();

        /// <summary>
        /// Shortcut for creating an instance of MessageCollector
        /// </summary>
        /// <returns>a new instance of MessageCollector</returns>
        IRuleExecuter GetExecutedMessageCollector();
        
        /// <summary>
        /// Creates an instance of the type provided by <paramref name="executorType"/>.
        /// </summary>
        /// <param name="executorType">the type to be created.</param>
        /// <returns>a new instance of <paramref name="executorType"/></returns>
        IRuleExecuter GetResultExecutor(Type executorType);
    }
}