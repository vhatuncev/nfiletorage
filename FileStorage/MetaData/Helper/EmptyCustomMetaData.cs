using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamiteXml;

namespace FileStorage.MetaData.Helper
{
    /// <summary>
    /// Empty custom meta data helper class, will be used as the default customer metadata, when the
    /// code specifies 'null' as custom meta data. 
    /// </summary>
    [Serializable]
    public class EmptyCustomMetaData : ICustomMetaData
    {
        /// <summary>
        /// Empty constructor is required for de-serialization
        /// </summary>
        public EmptyCustomMetaData()
        {
        }

        #region ICustomMetaData Members

        public string GetInfo()
        {
            return "{Empty; no meta data}";
        }

        #endregion
    }
}
