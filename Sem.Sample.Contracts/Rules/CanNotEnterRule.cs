namespace Sem.Sample.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Sample.Contracts.Entities;

    internal class CanNotEnterRule : RuleBase<MyCustomer, object>
    {
        public CanNotEnterRule()
        {
            this.Message = "Sven cannot enter this method";
            this.CheckExpression = (x, y) => x.FullName != "Sven";
        }
    }
}
