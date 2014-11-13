using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileStorage.Enums.Behaviours
{
    public enum AddFileBehaviour
    {
        ThrowExceptionWhenAlreadyExists,
        OverrideWhenAlreadyExists,
        SkipWhenAlreadyExists
    }
}
