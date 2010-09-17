namespace Sem.Test.GenericHelpers.Contracts.Entities
{
    using System.Linq;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Attributes;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts;

    public class SubscriberOne : IHandleThis<MessageOne>
    {
        public string Content { get; set; }
        public int CountOfEvents { get; set; }

        /// <summary>
        /// This method rule attribute does contain an invalid rule (the type is not a rule at all)
        /// </summary>
        /// <param name="message"></param>
        [MethodRule(typeof(SubscriberOne), "message")]
        public void Handle(MessageOne message)
        {
            var result = Bouncer.ForMessages(() => message).Assert().Results;

            this.Content = message.Content + result.ToList().Count;
            this.CountOfEvents++;
        }

        [MethodRule(typeof(IsNotNullRule<>), "message")]
        public void Handle2(MessageOne message)
        {
            var result = Bouncer.ForMessages(() => message).Assert().Results;

            this.Content = result.ToList().Count.ToString();
            this.CountOfEvents++;
        }

        [MethodRule(typeof(IsNotNullRule<MessageOne>), "message")]
        public void Handle3(MessageOne message)
        {
            var result = Bouncer.ForMessages(() => message).Assert().Results;

            this.Content = result.ToList().Count.ToString();
            this.CountOfEvents++;
        }

        [ContractContext("Read")]
        public void ContractContextRead(AttributedSampleClass sample)
        {
            var result = Bouncer.ForMessages(() => sample).Assert().Results;

            this.Content = result.ToList().Count.ToString();
            this.CountOfEvents++;
        }

        [ContractContext("Insert")]
        public void ContractContextInsert(AttributedSampleClass sample)
        {
            var result = Bouncer.ForMessages(() => sample).Assert().Results;

            this.Content = result.ToList().Count.ToString();
            this.CountOfEvents++;
        }
    }
}