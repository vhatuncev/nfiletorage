using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileStorage.Factories
{
    public static class FilenameFactory
    {
        public static string GetFileStorageIndexFilename(string fileStorageName)
        {
            return fileStorageName + ".FileStorage.index.fc";
        }

        public static string GetFileStorageDataFilename(string fileStorageName)
        {
            return fileStorageName + ".FileStorage.data.fc";
        }
    }
}
