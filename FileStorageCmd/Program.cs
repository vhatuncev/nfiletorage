using System;
using System.Collections.Generic;
using System.Linq;
using FileStorage.Enums.Behaviours;
using FileStorage.Enums.Features;
using FileStorage.Factories;
using FileStorage.Handler;
using FileStorage.Helper;
using FileStorage.Structure;
using FileStorageCmd.UIProgress;
using NConsoler;

using FileStorage;

namespace FileStorageCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Consolery.Run(typeof(Program), args);
        }

        [Action]
        public static void Add
            (
            [Required] string fileStorageName,
            [Required] string filename,
            [Required] string fileIdenticationOrRandomOrFilename
            )
        {
            try
            {
                var startDateTime = DateTime.Now;
                Guid fileIdentification;
                if (fileIdenticationOrRandomOrFilename.ToLower().Equals("random"))
                {
                    fileIdentification = Guid.NewGuid();
                }
                else if (fileIdenticationOrRandomOrFilename.ToLowerInvariant().Equals("filename"))
                {
                    fileIdentification = filename.ToNFileStorageDataIdentifier();
                }
                else
                {
                    fileIdentification = new Guid(fileIdenticationOrRandomOrFilename);
                }
                

                if (fileIdenticationOrRandomOrFilename.ToLowerInvariant().Equals("filename"))
                {
                    string filenameInStorage = fileIdentification.ToNFileStorageOrigFileName();
                    Console.WriteLine(string.Format("Storing file identifier {0} as {1}", fileIdentification, filenameInStorage));
                } 
                else
                {
                    Console.WriteLine(string.Format("Storing file identifier {0}", fileIdentification));    
                }

                FileStorageFacade.StoreFile(fileStorageName, fileIdentification, filename, null, AddFileBehaviour.ThrowExceptionWhenAlreadyExists);
                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64) timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        [Action]
        public static void Count
            (
            [Required] string fileStorageName
            )
        {
            try
            {
                ProgressNotifier progressNotifier = new ProgressNotifier("Count");

                DateTime startDateTime = DateTime.Now;
                List<Guid> dataIdentifiers = FileStorageFacade.GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName, new FileStorageHandler.ExposeProgressDelegate(progressNotifier.ShowProgress));
                Console.WriteLine(string.Format("{0} files found", dataIdentifiers.LongCount()));
                TimeSpan timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64) timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        [Action]
        public static void Del
            (
            [Required] string fileStorageName,
            [Required] string fileIdenticationGuid
            )
        {
            try
            {
                var startDateTime = DateTime.Now;
                var fileIdentification = new Guid(fileIdenticationGuid);
                FileStorageFacade.DeleteDataIdentifier(fileStorageName, fileIdentification, DeleteFileBehaviour.ThrowExceptionWhenNotExists);
                Console.WriteLine(string.Format("File {0} deleted from index (data is not yet purged)", fileIdentification));
                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64)timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        [Action]
        public static void Dir
            (
            [Required] string fileStorageName,
            [Required] string quickOrVerbose
            )
        {
            bool verbose = quickOrVerbose.ToLower().Equals("verbose");

            var sourceFileStorageHandler = FileStorageHandler.Open(fileStorageName);
            var features = FileStorageFeatureFactory.CreateFileStorageFeatureList(sourceFileStorageHandler.DataFileHeaderStruct.fileStorageFeatures);
            if (verbose && !sourceFileStorageHandler.SupportsFeature(FileStorageFeatureEnum.StoreMetaData))
            {
                Console.WriteLine();
                Console.WriteLine("The datastore does not support metadata feature and thus the verbose output is not available.");
                Console.WriteLine("Hint: Use the replicate option to promote this FileStorage to one that does support this feature.");
                Console.WriteLine("Press a key to continue outputting quick dir");
                Console.ReadKey();
                verbose = false;
            } 
            else
            {
                Console.WriteLine(sourceFileStorageHandler.DataFileHeaderStruct.fileStorageFeatures);
            }

            Int64 totalSize = 0;

            try
            {
                ProgressNotifier progressNotifier = new ProgressNotifier("Dir");
                var count = 0;
                DateTime startDateTime = DateTime.Now;
                List<Guid> dataIdentifiers = FileStorageFacade.GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName, new FileStorageHandler.ExposeProgressDelegate(progressNotifier.ShowProgress));

                if (verbose)
                {
                    Console.WriteLine("Data identifier                      | Text identifier  | Creation date     | Size ");
                    Console.WriteLine("-------------------------------------+------------------+-------------------+-----------------");
                } 
                else
                {
                    Console.WriteLine("Data identifier                      | Text identifier   ");
                    Console.WriteLine("-------------------------------------+-------------------");
                }


                foreach (Guid currentDataIdentifier in dataIdentifiers)
                {
                    string interpretedString = currentDataIdentifier.ToNFileStorageOrigFileName();

                    if (verbose)
                    {
                        var metaDataContainer = FileStorageFacade.GetMetaData(fileStorageName, currentDataIdentifier);
                        Int64 binarySizeInBytes = metaDataContainer.BinarySizeInBytes;
                        totalSize += binarySizeInBytes;
                        DateTime creationDate = metaDataContainer.CreationDateUTC;
                        Console.WriteLine(string.Format("{0} | {1} | {2} | {3:0,0,0,0}", currentDataIdentifier, interpretedString, creationDate.ToString("yyyyMMdd hh:mm:ss"), binarySizeInBytes));
                    } 
                    else
                    {
                        Console.WriteLine(string.Format("{0} | {1} | ", currentDataIdentifier, interpretedString));
                    }

                    count++;
                }

                if (verbose)
                {
                    Console.WriteLine(string.Format("{0} files found ({1:0,0,0,0} bytes)", count, (int) totalSize));
                }
                else
                {
                    Console.WriteLine(string.Format("{0} files found", count));    
                }
                
                TimeSpan timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64)timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
                Console.WriteLine(string.Format("An error occured; {0}", e.StackTrace));
            }
        }

        [Action]
        public static void Exists
            (
            [Required] string fileStorageName,
            [Required] string fileIdenticationGuid
            )
        {
            try
            {
                var startDateTime = DateTime.Now;
                var fileIdentification = new Guid(fileIdenticationGuid);
                bool exists = FileStorageFacade.Exists(fileStorageName, fileIdentification);
                if (exists)
                {
                    Console.WriteLine(string.Format("True"));
                }
                else
                {
                    Console.WriteLine(string.Format("False"));
                }
                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64)timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        [Action]
        public static void Merge
            (
            [Required] string sourcefileStorageName,
            [Required] string destinationfileStorageName
            )
        {
            try
            {
                Console.WriteLine(string.Format("[Source {0}]", sourcefileStorageName));
                Console.WriteLine(string.Format("[Destination {0}]", destinationfileStorageName));
                Console.WriteLine();
                Console.WriteLine("Press enter to start merging (files will be overriden when they already exist) ... ");
                Console.ReadLine();

                ProgressNotifier progressNotifierPhase1 = new ProgressNotifier("Reading indexes");
                ProgressNotifier progressNotifierPhase2 = new ProgressNotifier("Writing");
                DateTime startDateTime = DateTime.Now;
                FileStorageFacade.Replicate(sourcefileStorageName, destinationfileStorageName, ReplicateBehaviour.AddToExistingStorage, AddFileBehaviour.OverrideWhenAlreadyExists, new FileStorageHandler.ExposeProgressDelegate(progressNotifierPhase1.ShowProgress), new FileStorageHandler.ExposeProgressDelegate(progressNotifierPhase2.ShowProgress));

                TimeSpan timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64) timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        [Action]
        public static void Meta
            (
            [Required] string fileStorageName,
            [Required] string fileIdenticationGuid
            )
        {
            try
            {
                var startDateTime = DateTime.Now;
                var fileIdentification = new Guid(fileIdenticationGuid);
                var metaDataContainer = FileStorageFacade.GetMetaData(fileStorageName, fileIdentification);
                if (metaDataContainer == null)
                {
                    Console.WriteLine(string.Format("No metadata found"));
                }
                else
                {
                    Console.WriteLine(string.Format("Creation time UTC {0}", metaDataContainer.CreationDateUTC));
                    Console.WriteLine(string.Format("File size {0:0,0,0,0} bytes", metaDataContainer.BinarySizeInBytes));
                    Console.WriteLine(string.Format("Custom meta data type {0}", metaDataContainer.CustomMetaData.GetType().ToString()));
                    Console.WriteLine(string.Format("Custom meta data {0}", metaDataContainer.CustomMetaData.GetInfo()));
                }

                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64)timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        /// <summary>
        /// Makes a clone of a source filestorage, and re-allocates the data identifiers in the destination filestorage (the first 
        /// data identifier will get 0001, the next one 0002, etc.) in an optimized way. Next to outputting a new target filestorage 
        /// with new indexes, the method also produces a sql file that should be used to update your DAL logic, since the references 
        /// to the data identifiers also need to be updated when the ID of the data identifier is updated.
        /// </summary>
        [Action]
        public static void Defrag
            (
            [Required] string sourceFileStorageName, 
            [Required] string destinationFileStorageName, 
            [Required] string sqlTable, 
            [Required] string sqlColumn
            )
        {
            try
            {
                var startDateTime = DateTime.Now;
                var progressNotifierPhase1 = new ProgressNotifier("Reading indexes");
                var progressNotifierPhase2 = new ProgressNotifier("Writing");

                FileStorageFacade.DefragDataIdentifiers(sourceFileStorageName, destinationFileStorageName, sqlTable, sqlColumn, progressNotifierPhase1.ShowProgress, progressNotifierPhase2.ShowProgress);
                Console.WriteLine();
                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("File storage optimization finished"));
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64)timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        /// <summary>
        /// Upgrades the source file storage to new file storage with the most up
        /// to date version number.
        /// </summary>
        [Action]
        public static void Upgrade
            (
            [Required] string sourceFileStorageName,
            [Required] string destinationFileStorageName
            )
        {
            try
            {
                var startDateTime = DateTime.Now;
                var progressNotifierPhase1 = new ProgressNotifier("Reading indexes");
                var progressNotifierPhase2 = new ProgressNotifier("Writing");

                FileStorageFacade.Upgrade(sourceFileStorageName, destinationFileStorageName, progressNotifierPhase1.ShowProgress, progressNotifierPhase2.ShowProgress);

                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine();
                Console.WriteLine(string.Format("File storage upgrade finished"));
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64)timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        [Action]
        public static void New
            (
            [Required] string fileStorageName
            )
        {
            try
            {
                var startDateTime = DateTime.Now;
                FileStorageFacade.Create(fileStorageName, CreateFileStorageBehaviour.ThrowExceptionWhenExists);
                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("File storage {0} created", fileStorageName));
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64)timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        [Action]
        public static void Replicate
            (
            [Required] string sourcefileStorageName,
            [Required] string destinationfileStorageName
            )
        {
            try
            {
                Console.WriteLine(string.Format("[Source {0}]", sourcefileStorageName));
                Console.WriteLine(string.Format("[Destination {0}]", destinationfileStorageName));
                Console.WriteLine();
                Console.WriteLine("Press enter to replicate ... ");
                Console.ReadLine();

                var progressNotifierPhase1 = new ProgressNotifier("Reading indexes");
                var progressNotifierPhase2 = new ProgressNotifier("Writing");

                var startDateTime = DateTime.Now;
                FileStorageFacade.Replicate(sourcefileStorageName, destinationfileStorageName, ReplicateBehaviour.ReplicateToNewStorage, AddFileBehaviour.ThrowExceptionWhenAlreadyExists, new FileStorageHandler.ExposeProgressDelegate(progressNotifierPhase1.ShowProgress), new FileStorageHandler.ExposeProgressDelegate(progressNotifierPhase2.ShowProgress));

                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64) timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        [Action]
        public static void RestoreIndexFile
            (
            [Required] string fileStorageName
            )
        {
            try
            {
                var startDateTime = DateTime.Now;

                var progressNotifier = new ProgressNotifier("RestoreIndexFile");
                FileStorageFacade.RestoreIndexFile(fileStorageName, AddFileBehaviour.OverrideWhenAlreadyExists, new FileStorageHandler.ExposeProgressDelegate(progressNotifier.ShowProgress));

                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64) timeSpan.TotalMilliseconds));

                Console.WriteLine("[Index is restored succesfully]");
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        [Action]
        public static void Get
            (
            [Required] string fileStorageName,
            [Required] string fileExtension,
            [Required] string fileIdenticationGuid
            )
        {
            try
            {
                var startDateTime = DateTime.Now;
                var fileIdentification = new Guid(fileIdenticationGuid);
                var outputFile = fileIdenticationGuid + "." + fileExtension;
                Console.WriteLine(string.Format("Dumping {0} ...", outputFile));
                FileStorageFacade.ExportToFile(fileStorageName, fileIdentification, outputFile, ExportFileBehaviour.ThrowExceptionWhenAlreadyExists);
                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64) timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }

        [Action]
        public static void GetAll
            (
            [Required] string fileStorageName,
            [Required] string fileExtension
            )
        {
            try
            {
                var startDateTime = DateTime.Now;
                var progressNotifier = new ProgressNotifier("RetrieveAll");
                var dataIdentifiers = FileStorageFacade.GetAllDataIdentifiersBasedUponFileStorageIndexFile(fileStorageName, new FileStorageHandler.ExposeProgressDelegate(progressNotifier.ShowProgress));

                Console.WriteLine("Dumping ...");
                foreach (var currentdataIdentifier in dataIdentifiers)
                {
                    var outputFile = currentdataIdentifier + "." + fileExtension;
                    FileStorageFacade.ExportToFile(fileStorageName, currentdataIdentifier, outputFile, ExportFileBehaviour.ThrowExceptionWhenAlreadyExists);
                    Console.Write(".");
                }
                Console.WriteLine("[Finished]");
                Console.WriteLine(string.Format("{0} files found", dataIdentifiers.LongCount()));
                var timeSpan = DateTime.Now - startDateTime;
                Console.WriteLine(string.Format("This operation took {0} msecs", (Int64)timeSpan.TotalMilliseconds));
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("An error occured; {0}", e.Message));
            }
        }
    }
}
