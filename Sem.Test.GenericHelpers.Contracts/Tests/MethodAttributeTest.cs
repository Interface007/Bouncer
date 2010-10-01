namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Entities;

    /// <summary>
    /// Summary description for MethodAttributeTest
    /// </summary>
    [TestClass]
    public class MethodAttributeTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            // The method InsertCustomer has a RuleCollection "StrictCustomerCheckRuleSet" attached and runs in the context of "Create"
            // StrictCustomerCheckRuleSet does contain rule CanNotEnterRule                         => violation!
            // StrictCustomerCheckRuleSet does contain rule IsNotNullRule                           => ok
            // MyCustomer does contain rule IsNotNullRule for property InternalId in context "Read" => ok, violation suppressed by context
            // MyCustomer does contain rule IsNullRule for property InternalId in context "Create"  => ok
            // MyCustomer does contain rule StringRegexMatchRule for property EMailAddress          => violation!
            var results = new MyBusinessComponentSave().InsertCustomer(new MyCustomer { FullName = "Sven", EMailAddress = "don't@spam.me" }).ToList();
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Exists(x => x.RuleType == typeof(StringRegexMatchRule)));
            Assert.IsTrue(results.Exists(x => x.RuleType == typeof(Rules.CanNotEnterRule)));
        }

        [TestMethod]
        public void TestMethod2()
        {
            // The method InsertCustomer has a RuleCollection "StrictCustomerCheckRuleSet" attached and runs in the context of "Create"
            // StrictCustomerCheckRuleSet does contain rule CanNotEnterRule                         => ok
            // StrictCustomerCheckRuleSet does contain rule IsNotNullRule                           => ok
            // MyCustomer does contain rule IsNotNullRule for property InternalId in context "Read" => ok, but suppressed by context
            // MyCustomer does contain rule IsNullRule for property InternalId in context "Create"  => violation!
            // MyCustomer does contain rule StringRegexMatchRule for property EMailAddress          => ok
            var results = new MyBusinessComponentSave().InsertCustomer(new MyCustomer { FullName = "Mary", EMailAddress = "dont@spam.me", InternalId = new CustomerId() }).ToList();
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(typeof(IsNullRule<CustomerId>), results[0].RuleType);
            Assert.AreEqual("customer.InternalId", results[0].ValueName);
        }
    }
}
