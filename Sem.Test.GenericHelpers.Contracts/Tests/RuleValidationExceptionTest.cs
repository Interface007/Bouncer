using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    using Sem.GenericHelpers.Contracts.Exceptions;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Test.GenericHelpers.Contracts.Entities;

    /// <summary>
    /// Summary description for RuleValidationExceptionTest
    /// </summary>
    [TestClass]
    public class RuleValidationExceptionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            var x = new RuleValidationException(typeof(IsNullRule<MyCustomer>), "hello", "customer");
            var z = new BinaryFormatter();
            var buffer = new MemoryStream();
            z.Serialize(buffer, x);
            var result = new StringBuilder();

            foreach (var character in buffer.ToArray())
            {
                if (character > 32 && character < 128)
                {
                    result.Append(Encoding.ASCII.GetString(new[]{character}));
                }
            }

            Assert.IsTrue(result.ToString().EndsWith("Sem.GenericHelpers.Contracts.Exceptions.RuleValidationExceptionClassNameMessageDataInnerExceptionHelpURLStackTraceStringRemoteStackTraceStringRemoteStackIndexExceptionMethodHResultSourceWatsonBucketsParamNameSystem.Collections.IDictionarySystem.Exception?Sem.GenericHelpers.Contracts.Exceptions.RuleValidationExceptionhelloWcustomer"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var x = new RuleValidationException(typeof(IsNullRule<MyCustomer>), "hello", "customer");
            var z = new BinaryFormatter();
            var buffer = new MemoryStream();
            z.Serialize(buffer, x);
            var result = new StringBuilder();

            foreach (var character in buffer.ToArray())
            {
                if (character > 32 && character < 128)
                {
                    result.Append(Encoding.ASCII.GetString(new[]{character}));
                }
            }

            Assert.IsTrue(result.ToString().EndsWith("Sem.GenericHelpers.Contracts.Exceptions.RuleValidationExceptionClassNameMessageDataInnerExceptionHelpURLStackTraceStringRemoteStackTraceStringRemoteStackIndexExceptionMethodHResultSourceWatsonBucketsParamNameSystem.Collections.IDictionarySystem.Exception?Sem.GenericHelpers.Contracts.Exceptions.RuleValidationExceptionhelloWcustomer"));
        }
    }
}
