using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchiveDocuments.Models
{
    public enum ActionStatus
    {
        Success,
        AddSuccess,
        EditSuccess,
        DeleteSuccess,
        Error,
        AddError,
        EditError,
        DeleteError
    }
}
