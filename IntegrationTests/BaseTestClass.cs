using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IntegrationTests
{
    public abstract class BaseTestClass
    {
        protected const string StorageFileAppendix = ".FileStorage.data.fc";
        protected const string IndexFileAppendix = ".FileStorage.index.fc";
    }
}
