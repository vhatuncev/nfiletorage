using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamiteXml;

namespace FileStorage.MetaData
{
    [Serializable]
    public class MetaDataContainer : IDynamiteXml
    {
        public MetaDataContainer()
        {
            // empty constructor is required for serialization    
        }

        public MetaDataContainer(ICustomMetaData customMetaData, DateTime creationDateUTC, Int64 binaryDataSizeInBytes)
        {
            CustomMetaDataString = DynamiteXmlLogic.Serialize(customMetaData);
            CreationDateUTC = creationDateUTC;
            BinarySizeInBytes = binaryDataSizeInBytes;
        }

        /// <summary>
        /// Short version to prevent the metadata to become very big
        /// </summary>
        private string _c;

        public string CustomMetaDataString
        {
            get
            {
                return _c;
            }
            set
            {
                _c = value;
            }
        }

        /// <summary>
        /// Short version to prevent the metadata to become very big
        /// </summary>
        private DateTime _ct;

        public DateTime CreationDateUTC
        {
            get
            {
                return _ct;
            }
            set
            {
                _ct = value;
            }
        }

        /// <summary>
        /// Short version to prevent the metadata growing rapidly
        /// </summary>
        private Int64 _bs;

        public Int64 BinarySizeInBytes
        {
            get
            {
                return _bs;
            }
            set
            {
                _bs = value;
            }
        }

        public ICustomMetaData CustomMetaData
        {
            get
            {
                return DynamiteXmlLogic.Deserialize(CustomMetaDataString) as ICustomMetaData;
            }
        }
    }
}