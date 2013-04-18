// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassFileSystem.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the unit tests for the class FileSystem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Test.GenericHelpers.IO
{
    using System;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.IO.Ads;

    /// <summary>
    /// The tests for the class <see cref="FileSystem"/>.
    /// </summary>
    public class ClassFileSystem
    {
        /// <summary>
        /// The test for the method <see cref="FileSystem.WriteAlternateDataStream"/>.
        /// </summary>
        [TestClass]
        public class WriteAlternateDataStream
        {
            /// <summary>
            /// Gets or sets the test context.
            /// </summary>
            public TestContext TestContext { get; set; }

            /// <summary>
            /// Ensures that writing to ADS does not alter the "standard content" of a file.
            /// </summary>
            [TestMethod]
            public void WritingToAdsDoesNotAlterStandardContent()
            {
                var filePath = Path.Combine(this.TestContext.TestDir, "9FA6770568844B2B94EB567BCEF8503A.txt");
                var fileContent = Guid.NewGuid().ToString("D");
                File.WriteAllText(filePath, fileContent);

                var info = new FileInfo(filePath);
                Assert.AreEqual(0, info.ListAlternateDataStreams().Count);

                info.WriteAlternateDataStream("Sem.TestData", "TestData");

                var actual = File.ReadAllText(filePath);
                Assert.AreEqual(fileContent, actual);
            }

            /// <summary>
            /// Ensures that writing to ADS does alter ADS content.
            /// </summary>
            [TestMethod]
            public void WritingToAdsDoesAlterAdsContent()
            {
                var filePath = Path.Combine(this.TestContext.TestDir, "0A8B518C27BE4C4280BCEB7F72C7D385.txt");
                var fileContent = Guid.NewGuid().ToString("D");
                File.WriteAllText(filePath, fileContent);

                var info = new FileInfo(filePath);
                Assert.AreEqual(0, info.ListAlternateDataStreams().Count);

                info.WriteAlternateDataStream("Sem.TestData", "TestData");
                Assert.AreEqual(1, info.ListAlternateDataStreams().Count);

                var actual = info.ReadAlternateDataStreamText("Sem.TestData");

                Assert.AreEqual("TestData", actual);
            }
        }
    }
}
