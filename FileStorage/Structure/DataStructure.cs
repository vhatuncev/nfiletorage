using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileStorage.Structure
{
    [Serializable]
    public struct DataFileHeaderStruct
    {
        public int versionMajor;            // 4 bytes
        public int versionMinor;            // 4 bytes
        public int fileStorageFeatures;     // 4 bytes  (32 possible file features)
        public char[] text;                 // 88 bytes, reserved / used for some informative data
    }

    [Serializable]
    public struct DataFileDataStruct
    {
        public Guid DataIdentification;                 // 16 bytes; the unique identification of the guid 
        public int DataStateEnumID;                     // 4 bytes; state of the file, for example; active, deleted
        public Int64 BinaryDataSizeInBytes;             // 8 bytes; the size in bytes
        public Int64 MetaDataOffsetInFileFromBegin;     // 8 bytes; pointer to meta data block
        public Int64 ReservedB;                         // 8 bytes; for future use
    }

    /// <summary>
    /// Extends the FileStruct, by adding a file data offset, only used in memory (this is not persisted)
    /// </summary>
    public struct InterpretedDataFileDataStruct
    {
        public Guid DataIdentification;                 //
        public int DataStateEnumID;                     // for example; active, deleted
        public Int64 BinaryDataSizeInBytes;             //
        public Int64 MetaDataOffsetInFileFromBegin;     // 8 bytes; pointer to meta data block
        public Int64 ReservedB;                         // for future use
        public Int64 BinaryDataOffset;
    }
}
