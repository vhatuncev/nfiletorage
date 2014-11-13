using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileStorage.Structure
{
    [Serializable]
    public struct IndexFileHeaderStruct
    {
        public int versionMajor;    // 4 bytes
        public int versionMinor;    // 4 bytes
        public char[] text;         // 92 bytes, reserved / used for some informative data
    }

    [Serializable]
    public struct AllocationTableStruct
    {
        public Int64[] pointers;    // 256 pointers, for each byte of the guid
    }
}
