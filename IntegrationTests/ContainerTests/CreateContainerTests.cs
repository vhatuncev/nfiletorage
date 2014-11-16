using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.Enums.Behaviours;
using NUnit;
using NUnit.Framework;
using FileStorage;

namespace IntegrationTests.ContainerTests
{
    [TestFixture]
    public class CreateContainerTests : BaseTestClass
    {
        private string _containerName;
        private string _containerFile;
        private string _indexFile;

        [SetUp]
        public void SetUp()
        {
            _containerName = "Integration_CreateContainer-" + DateTime.Now.Ticks;
            _containerFile = _containerName + StorageFileAppendix;
            _indexFile = _containerName + IndexFileAppendix;
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(_containerFile);
            File.Delete(_indexFile);
        }

        [Test(Description = "Test for container creation")]
        public void Creates_Container()
        {
            FileStorageFacade.Create(_containerName, CreateFileStorageBehaviour.IgnoreWhenExists);

            var containerExists = File.Exists(_containerFile);
            var indexExists = File.Exists(_indexFile);

            Assert.IsTrue(containerExists, "Container file was not created");
            Assert.IsTrue(indexExists, "Index file was not created");
        }

        [Test(Description = "Should throw exception if container with such name already exists")]
        public void Throws_Exception_If_Exists()
        {
            FileStorageFacade.Create(_containerName, CreateFileStorageBehaviour.IgnoreWhenExists);

            Assert.Throws<Exception>(() => FileStorageFacade.Create(_containerName, CreateFileStorageBehaviour.ThrowExceptionWhenExists), "Exception is not thrown when ThrowExceptionWhenExists option is specified");
        }
    }
}
