namespace Sem.Test.GenericHelpers.Contracts.Rules
{
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Entities;

    internal class CanNotEnterRule : RuleBase<MyCustomer, object>
    {
        public CanNotEnterRule()
        {
            this.Message = "Sven cannot enter this method";
            this.CheckExpression = (x, y) => x.FullName != "Sven";
        }
    }
}
