using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sem.Test.GenericHelpers.Contracts.Tests
{
    using System.ComponentModel.DataAnnotations;

    using Sem.GenericHelpers.Contracts;

    public class TypeWithAnnotations
    {
        [StringLength(30)]
        public string Name { get; set; }
    }

    /// <summary>
    /// Summary description for DataAnnotationsTest
    /// </summary>
    [TestClass]
    public class DataAnnotationsTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            var target = new TypeWithAnnotations
                {
                    ////    00000000011111111112222222222333333333344444444445555555555666666666677777777778
                    ////    12345678901234567890123456789012345678901234567890123456789012345678901234567890
                    Name = "Hello I have more than 30 characters and I need to throw an exception."
                };

            var x = Bouncer
                .ForMessages(() => target)
                .AssertAll()
                .Results;

            Assert.AreEqual(1, x.Count());
        }
    }
}
