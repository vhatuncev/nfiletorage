using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamiteXml;

namespace Example
{
    [Serializable]
    public class DummyObject : IDynamiteXml
    {
        public string Color
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }
    }
}
