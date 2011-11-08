using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System.ComponentModel.DataAnnotations;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Exceptions;

    public class TestableClass
    {
        public const string ThePhoneNumberMustMatchARealPhoneNumber = "The phone number must match a real phone number!";

        [RegularExpression(@"^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$", ErrorMessage = ThePhoneNumberMustMatchARealPhoneNumber)]
        public string PhoneNumber { get; set; }
    }

    /// <summary>
    /// Summary description for DataAnnotationTests
    /// </summary>
    [TestClass]
    public class DataAnnotationTests
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void CheckRegularExpressionReturnsError()
        {
            var probe = new TestableClass
                {
                    PhoneNumber = "Hello"
                };

            Bouncer.For(() => probe).Ensure();
        }

        [TestMethod]
        public void CheckRegularExpressionReturnsMessages()
        {
            var probe = new TestableClass
                {
                    PhoneNumber = "Hello"
                };

            var msg=Bouncer.For(() => probe).Messages();
            Assert.IsNotNull(msg);
            Assert.AreEqual(1, msg.Count());
            Assert.AreEqual("The rule Sem.GenericHelpers.Contracts.Rules.DataAnnotationValidatorBaseRule`1 did fail for value name >>probe.PhoneNumber<<: The phone number must match a real phone number!", msg.First().Message);
        }

        [TestMethod]
        public void CheckRegularExpressionReturnsNoError()
        {
            var probe = new TestableClass
                {
                    PhoneNumber = "123-123-1234"
                };

            Bouncer.For(() => probe).Ensure();
        }
    }
}
