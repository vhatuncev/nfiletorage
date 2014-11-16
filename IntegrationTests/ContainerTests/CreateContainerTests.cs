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
        [Test(Description = "Test for container creation")]
        public void Creates_Container()
        {
            FileStorageFacade.Create(ContainerName, CreateFileStorageBehaviour.IgnoreWhenExists);

            var containerExists = File.Exists(ContainerFile);
            var indexExists = File.Exists(IndexFile);

            Assert.IsTrue(containerExists, "Container file was not created");
            Assert.IsTrue(indexExists, "Index file was not created");
        }

        [Test(Description = "Should throw exception if container with such name already exists")]
        public void Throws_Exception_If_Exists()
        {
            FileStorageFacade.Create(ContainerName, CreateFileStorageBehaviour.IgnoreWhenExists);

            Assert.Throws<Exception>(() => FileStorageFacade.Create(ContainerName, CreateFileStorageBehaviour.ThrowExceptionWhenExists), "Exception is not thrown when ThrowExceptionWhenExists option is specified");
        }
    }
}
