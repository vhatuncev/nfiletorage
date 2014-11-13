using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileStorage.Enums.Features;

namespace FileStorage.Factories
{
    public static class FileStorageFeatureFactory
    {
        public static List<FileStorageFeatureEnum> GetDefaultFeatures()
        {
            var result = new List<FileStorageFeatureEnum>
                             {
                                 FileStorageFeatureEnum.StoreMetaData
                             };
            return result;
        }

        public static int CreateFileStorageFeatureValue(List<FileStorageFeatureEnum> features)
        {
            var result = 0;

            //
            // ensure the list is distinct
            //
            features = features.Distinct().ToList();

            foreach (var feature in features)
            {
                result += (int) feature;
            }

            return result;
        }

        public static List<FileStorageFeatureEnum> CreateFileStorageFeatureList(int fileStorageFeatureAsValue)
        {
            var result = new List<FileStorageFeatureEnum>();

            var type = typeof (FileStorageFeatureEnum);
            foreach (FileStorageFeatureEnum currentFileStorageFeature in Enum.GetValues(type))
            {
                var currentValue = (int) currentFileStorageFeature;
                var isCurrentFeatureEnabled = (fileStorageFeatureAsValue & currentValue) == currentValue;
                if (isCurrentFeatureEnabled)
                {
                    result.Add(currentFileStorageFeature);
                }
            }

            return result;
        }
    }
}
