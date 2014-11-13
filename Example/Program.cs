using System;
using FileStorage;
using FileStorage.Enums.Behaviours;

namespace Example
{
    class Program
    {
        public static void Main(String[] arg)
        {
            ProcessUserInput();
        }

        private const string fileStorageName = "example";

        private static void ProcessUserInput()
        {
            bool continueLoop = true;
            while (continueLoop) 
            {
                ShowOptions();

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine(string.Format("Key {0} pressed", consoleKeyInfo.KeyChar));

                try {
                    switch (consoleKeyInfo.KeyChar)
                    {
                        case '1':
                            Console.WriteLine("Creating container ...");
                            CreateContainer();
                            Console.WriteLine("Done");
                            break;
                        case '2':
                            StoreWebImageInContainer();
                            break;
                        case '3':
                            DumpContainerFilesToFileSystem();
                            break;
                        case '4':
                            DeleteAllFilesFromContainer();
                            break;
                        case '5':
                            ListAllfilesInContainer();
                            break;
                        case '6':
                            AddObjects();
                            break;
                        case '7':
                            RetrieveObjects();
                            break;
                        case 'x':
                            continueLoop = false;
                            break;
                    }
                } 
                catch (Exception e) 
                {
                    Console.WriteLine(string.Format("Exception {0} occured", e.Message));
                }
            }
        }

        private static void ShowOptions()
        {
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Use one of the following options;");
            Console.WriteLine("");
            Console.WriteLine("1. Create a new 'example' FileStorage ");
            Console.WriteLine("2. Download an image from the web and store it in the FileStorage");
            Console.WriteLine("3. Dump all files in the container to the file system");
            Console.WriteLine("4. Remove all files from the container");
            Console.WriteLine("5. List all files in the container");
            Console.WriteLine("6. Insert objects in the container");
            Console.WriteLine("7. Retrieve object in the container");
            Console.WriteLine("");
            Console.WriteLine("x. Exit file container demo");
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("");
        }

        public static void CreateContainer() 
        {
            FileStorageFacade.Create(fileStorageName, CreateFileStorageBehaviour.ThrowExceptionWhenExists);
            Console.WriteLine(FileStorageFacade.GetInfo(fileStorageName));
        }

        public static void StoreWebImageInContainer()
        {
            var uniqueIdentifier = Guid.NewGuid();
            FileStorageFacade.StoreHttpRequest(fileStorageName, uniqueIdentifier, "http://www.prijsvaneenhuis.nl/img/spandoek/NFileStorage_banner.jpg", null, AddFileBehaviour.ThrowExceptionWhenAlreadyExists, "NFileStorage");
            Console.WriteLine(FileStorageFacade.GetInfo(fileStorageName));
        }

        private static void DumpContainerFilesToFileSystem()
        {
            var dataIdentifiers = FileStorageFacade.GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName);
            foreach (var dataIdentifier in dataIdentifiers)
            {
                var outputFile = dataIdentifier + ".jpg";
                Console.WriteLine(string.Format("Exporting {0}", outputFile));
                FileStorageFacade.ExportToFile(fileStorageName, dataIdentifier, outputFile, ExportFileBehaviour.SkipWhenAlreadyExists); 
            }
        }

        private static void DeleteAllFilesFromContainer()
        {
            foreach (Guid dataIdentifier in FileStorageFacade.GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName))
            {
                FileStorageFacade.DeleteDataIdentifier(fileStorageName, dataIdentifier, DeleteFileBehaviour.IgnoreWhenNotExists);
            }
            Console.WriteLine(FileStorageFacade.GetInfo(fileStorageName));
        }

        private static void ListAllfilesInContainer()
        {
            Console.WriteLine(FileStorageFacade.GetInfo(fileStorageName));

            foreach (Guid dataIdentifier in FileStorageFacade.GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName))
            {
                Console.WriteLine(dataIdentifier.ToString());
            }
        }

        private static void AddObjects()
        {
            {
                DummyObject object1 = new DummyObject()
                                          {
                                              Color = "blue",
                                              Name = "sky"
                                          };

                Guid dataIdentifier = Guid.NewGuid();
                FileStorageFacade.StoreObject(fileStorageName, dataIdentifier, object1, null, AddFileBehaviour.OverrideWhenAlreadyExists);
            }
            {
                DummyObject object2 = new DummyObject()
                {
                    Color = "green",
                    Name = "grass"
                };

                Guid dataIdentifier = Guid.NewGuid();
                FileStorageFacade.StoreObject(fileStorageName, dataIdentifier, object2, null, AddFileBehaviour.OverrideWhenAlreadyExists);
            }
        }

        private static void RetrieveObjects()
        {
            var dataIdentifiers = FileStorageFacade.GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName);
            foreach (var dataIdentifier in dataIdentifiers)
            {
                try
                {
                    object deserializedObject = FileStorageFacade.GetObjectData(fileStorageName, dataIdentifier);
                    if (deserializedObject == null)
                    {
                        Console.WriteLine(string.Format("{0} is null", dataIdentifier));
                    }
                    else
                    {
                        if (deserializedObject is DummyObject)
                        {
                            DummyObject dummyObject = deserializedObject as DummyObject;
                            Console.WriteLine(string.Format("{0} is a DummyObject: color {1} name {2}", dataIdentifier, dummyObject.Color, dummyObject.Name));
                        }
                        else
                        {
                            Console.WriteLine(string.Format("{0} is an object, of type other than DummyObject", dataIdentifier));
                        }
                    }
                } 
                catch (Exception)
                {
                    Console.WriteLine(string.Format("{0} is not an object", dataIdentifier));
                }
            }
        }

    }
}
