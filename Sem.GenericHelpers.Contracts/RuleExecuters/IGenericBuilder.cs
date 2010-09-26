namespace Sem.GenericHelpers.Contracts.RuleExecuters
{
    public interface IGenericBuilder
    {
        IRuleExecuter GetExecutedCheckData();
        IMessageCollector GetExecutedMessageCollector();
    }
}