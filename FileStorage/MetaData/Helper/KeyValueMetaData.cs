﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamiteXml;

namespace FileStorage.MetaData.Helper
{
    /// <summary>
    /// KeyValueMetaData helper class, can be used to store string key/value pairs as custom meta data
    /// </summary>
    [Serializable]
    public class KeyValueMetaData : ICustomMetaData
    {
        /// <summary>
        /// Empty constructor is required for de-serialization
        /// </summary>
        public KeyValueMetaData()
        {
        }

        // note we explicitly us the SerializableDictionary
        private SerializableDictionary<string, string> _it = new SerializableDictionary<string, string>();

        public SerializableDictionary<string, string> KeyValues
        { 
            get
            {
                return _it;
            }
            set
            {
                _it = value;
            }
        }

        #region ICustomMetaData Members

        public string GetInfo()
        {
            var sb = new StringBuilder();
            foreach (var pair in KeyValues)
            {
                sb.AppendLine(String.Format("[Key {0} -> Value {1}]", pair.Key, pair.Value));
            }
            return sb.ToString();
        }

        #endregion
    }
}
