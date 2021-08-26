using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchiveDocuments.Models
{
    public class UserInfo
    {
        public Guid UserId { get; internal set; }
        public string UserName { get; internal set; }
    }
}
