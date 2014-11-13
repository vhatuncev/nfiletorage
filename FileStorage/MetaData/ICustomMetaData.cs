using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileStorage.MetaData
{
    public interface ICustomMetaData : DynamiteXml.IDynamiteXml
    {
        string GetInfo();
    }
}
