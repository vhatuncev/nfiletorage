using System;
using System.Text;
using FileStorage;
using FileStorage.Enums.Behaviours;
using IntegrationTests.Models;
using NUnit.Framework;

namespace IntegrationTests.ContainerTests
{
    [TestFixture]
    public class AddObjectsTests : BaseTestClass
    {
        [Test(Description = "Tests that the string is stored inside container")]
        public void Stores_Strings()
        {
            const string test = "this is custom test string";
            var id = Guid.NewGuid();

            FileStorageFacade.Create(ContainerName, CreateFileStorageBehaviour.IgnoreWhenExists);
            FileStorageFacade.StoreString(ContainerName, id, test, null, AddFileBehaviour.OverrideWhenAlreadyExists);

            string stored = FileStorageFacade.GetStringData(ContainerName, id);

            Assert.AreEqual(test, stored, "The stored object is not the same");
        }

        [Test(Description = "Tests that the string is stored inside container when specifying encoding")]
        public void Stores_String_With_Encoding()
        {
            const string test = "testфывфвфывйэж äæì";
            var id = Guid.NewGuid();

            FileStorageFacade.Create(ContainerName, CreateFileStorageBehaviour.IgnoreWhenExists);
            FileStorageFacade.StoreString(ContainerName, id, Encoding.UTF8, test, null, AddFileBehaviour.OverrideWhenAlreadyExists);

            string stored = FileStorageFacade.GetStringData(ContainerName, id, Encoding.UTF8);

            Assert.AreEqual(test, stored, "The stored object is not the same");
        }
    }
}
