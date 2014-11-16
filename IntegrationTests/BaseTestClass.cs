using System;
using System.IO;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public abstract class BaseTestClass
    {
        protected const string StorageFileAppendix = ".FileStorage.data.fc";
        protected const string IndexFileAppendix = ".FileStorage.index.fc";
        protected string ContainerName;
        protected string IndexFile;
        protected string ContainerFile;

        [SetUp]
        public void SetUp()
        {
            ContainerName = "Integration-" + DateTime.Now.Ticks;
            ContainerFile = ContainerName + StorageFileAppendix;
            IndexFile = ContainerName + IndexFileAppendix;
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(IndexFile);
            File.Delete(ContainerFile);
        }
    }
}
