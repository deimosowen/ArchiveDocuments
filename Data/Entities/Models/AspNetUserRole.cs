using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Data.Entities.Models
{
    public partial class AspNetUserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual AspNetRole Role { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
