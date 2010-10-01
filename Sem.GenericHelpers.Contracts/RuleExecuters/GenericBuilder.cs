namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// The GenericBuilder class provides an inverted interface for the Bouncer functionality. Instead of
    /// first collecting a set of classes that will execute the rules, this one will collect all data
    /// and decide the classes for rule execution when the <see cref="Ensure"/> or <see cref="Messages"/>
    /// method is being called.
    /// </summary>
    /// <typeparam name="TData">The type of data to be added to the list of data expressions.</typeparam>
    public class GenericBuilder<TData> : IGenericBuilder
    {
        private readonly List<IGenericBuilder> dataExpressions = new List<IGenericBuilder>();

        internal readonly Expression<Func<TData>> MyData;

        public GenericBuilder(Expression<Func<TData>> data)
        {
            this.MyData = data;
            this.dataExpressions.Add(this);
        }

        /// <summary>
        /// Adds a new expression to the list of data to be validated.
        /// </summary>
        /// <typeparam name="TDataNew">The type of data to be validated.</typeparam>
        /// <param name="data">The data to be validated.</param>
        /// <returns>The current instance of the builder - this.</returns>
        public GenericBuilder<TData> For<TDataNew>(Expression<Func<TDataNew>> data)
        {
            var newData = new GenericBuilder<TDataNew>(data);
            this.dataExpressions.Add(newData);
            return this;
        }

        /// <summary>
        /// Performs a check of all data inside the list of data-expressions using the <see cref="CheckData{TData}"/> class.
        /// </summary>
        public void Ensure()
        {
            ((IGenericBuilder)this).GetExecutedCheckData().AssertAll();
            foreach (var dataExpression in dataExpressions)
            {
                dataExpression.GetExecutedCheckData();
            }
        }

        /// <summary>
        /// Performs an <see cref="RuleExecuter{TData,TResultClass}.Assert()"/> on new instances of the <see cref="CheckData{TData}"/> class
        /// for each member of the data-expressions.
        /// </summary>
        /// <returns>The rule executer (the <see cref="CheckData{TData}"/> instance) for this call.</returns>
        IRuleExecuter IGenericBuilder.GetExecutedCheckData()
        {
            return new CheckData<TData>(this.MyData).Assert();
        }

        /// <summary>
        /// Performs a check of all data inside the list of data-expressions using the <see cref="MessageCollector{TData}"/> class.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="RuleValidationResult"/> for all data collected in the list of data-expressions.</returns>
        public IEnumerable<RuleValidationResult> Messages()
        {
            var executionList = this.dataExpressions.ToList();

            var list = new List<RuleValidationResult>();
            foreach (var executionItem in executionList)
            {
                list.AddRange(executionItem.GetExecutedMessageCollector().Results);
            }

            return list;
        }

        /// <summary>
        /// Performs an <see cref="RuleExecuter{TData,TResultClass}.Assert()"/> on new instances of the <see cref="MessageCollector{TData}"/> class
        /// for each member of the data-expressions.
        /// </summary>
        /// <returns>The rule executer (the <see cref="MessageCollector{TData}"/> instance) for this call.</returns>
        IRuleExecuter IGenericBuilder.GetExecutedMessageCollector()
        {
            return new MessageCollector<TData>(this.MyData).Assert();
        }

        public IRuleExecuter Use(Type executorType)
        {
            var resultExecutor = ((IGenericBuilder)this).GetResultExecutor(executorType);

            if (resultExecutor != null)
            {
                resultExecutor.AssertAll();
                foreach (var dataExpression in dataExpressions)
                {
                    var executer = dataExpression.GetResultExecutor(executorType).AssertAll();
                    resultExecutor.AddRange(executer.Results);
                }
            }

            return resultExecutor;
        }

        /// <summary>
        /// This method is more generic, but needs reflection to resolve the generic parameters and build a new type.
        /// </summary>
        /// <param name="executorType"></param>
        /// <returns></returns>
        IRuleExecuter IGenericBuilder.GetResultExecutor(Type executorType)
        {
            var genericTypeDefinition = executorType.GetGenericTypeDefinition();
            var makeGenericType = genericTypeDefinition.MakeGenericType(typeof(TData));
            var constructorInfo = makeGenericType.GetConstructor(new[] { typeof(Expression<Func<TData>>) });

            return constructorInfo.Invoke(new[] { this.MyData }) as IRuleExecuter;
        }

        public TExecutor Use<TExecutor>()
             where TExecutor : class, IRuleExecuter
        {
            return this.Use(typeof(TExecutor)) as TExecutor;
        }
    }
}