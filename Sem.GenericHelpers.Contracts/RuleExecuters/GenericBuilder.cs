namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class GenericBuilder<TData> : IGenericBuilder
    {
        private readonly List<IGenericBuilder> dataExpressions = new List<IGenericBuilder>();

        private readonly Expression<Func<TData>> myData;

        public GenericBuilder(Expression<Func<TData>> data)
        {
            this.myData = data;
            this.dataExpressions.Add(this);
        }

        public GenericBuilder<TData> For<TDataNew>(Expression<Func<TDataNew>> data)
        {
            var newData = new GenericBuilder<TDataNew>(data);
            this.dataExpressions.Add(newData);
            return this;
        }

        public void Ensure()
        {
            this.GetExecutedCheckData().AssertAll();
            foreach (var dataExpression in dataExpressions)
            {
                dataExpression.GetExecutedCheckData();
            }
        }

        public IRuleExecuter GetExecutedCheckData()
        {
            return new CheckData<TData>(myData).Assert();
        }

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

        public IMessageCollector GetExecutedMessageCollector()
        {
            return new MessageCollector<TData>(myData).Assert();
        }
    }
}