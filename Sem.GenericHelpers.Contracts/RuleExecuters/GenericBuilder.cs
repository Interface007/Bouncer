// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericBuilder.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The GenericBuilder class provides an inverted interface for the Bouncer functionality. Instead of
//   first collecting a set of classes that will execute the rules, this one will collect all data
//   and decide the classes for rule execution when the <see cref="Ensure" /> or <see cref="Messages" />
//   method is being called.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// The GenericBuilder class provides an inverted interface for the Bouncer functionality. Instead of
    /// first collecting a set of classes that will execute the rules, this one will collect all data
    /// and decide the classes for rule execution when the <see cref="Ensure"/> or <see cref="Messages"/>
    /// method is being called.
    /// </summary>
    /// <typeparam name="TData">The type of data to be added to the list of data expressions.</typeparam>
    public class GenericBuilder<TData> : IGenericBuilder
    {
        /// <summary>
        /// The expression that returns the data to be validated.
        /// </summary>
        internal readonly Expression<Func<TData>> MyData;

        /// <summary>
        /// The data expressions that should be validated as a list of builder
        /// </summary>
        private readonly List<IGenericBuilder> dataExpressions = new List<IGenericBuilder>();

        private MethodBase explicitMethodInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericBuilder{TData}"/> class.
        /// </summary>
        /// <param name="data"> The data to be checked. </param>
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
            foreach (var dataExpression in this.dataExpressions)
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
            return new CheckData<TData>(this.MyData, this.explicitMethodInfo).Assert();
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
            return new MessageCollector<TData>(this.MyData, this.explicitMethodInfo).Assert();
        }

        /// <summary>
        /// This method provides a way to use a rule executor that is not part of this library.
        /// <see cref="Use{TExecutor}"/> for an example
        /// </summary>
        /// <param name="executorType"> The executor type. </param>
        /// <returns>The executor of the desired type.</returns>
        public IRuleExecuter Use(Type executorType)
        {
            var resultExecutor = ((IGenericBuilder)this).GetResultExecutor(executorType);

            if (resultExecutor != null)
            {
                resultExecutor.AssertAll();
                foreach (var dataExpression in this.dataExpressions)
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
        /// <param name="executorType">The type of the executor to be created.</param>
        /// <returns>The new created executor.</returns>
        IRuleExecuter IGenericBuilder.GetResultExecutor(Type executorType)
        {
            var genericTypeDefinition = executorType.GetGenericTypeDefinition();
            var makeGenericType = genericTypeDefinition.MakeGenericType(typeof(TData));
            var constructorInfo = makeGenericType.GetConstructor(new[] { typeof(Expression<Func<TData>>) });

            return constructorInfo.Invoke(new[] { this.MyData }) as IRuleExecuter;
        }

        /// <summary>
        /// Creates a constructor that is not part of the library.
        /// </summary>
        /// <example>
        /// <code>
        ///     var result = Bouncer
        ///                .For(() => attributedSampleClass)
        ///                .Use&lt;VetoExecutor&lt;AttributedSampleClass>>()
        ///                .LastValidation;
        /// </code>
        /// </example>
        /// <typeparam name="TExecutor">The executor to use.</typeparam>
        /// <returns>The newly constructes executor </returns>
        public TExecutor Use<TExecutor>()
             where TExecutor : class, IRuleExecuter
        {
            return this.Use(typeof(TExecutor)) as TExecutor;
        }
    }
}