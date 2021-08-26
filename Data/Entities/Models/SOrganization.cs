using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Data.Entities.Models
{
    /// <summary>
    /// Справочник Организации
    /// </summary>
    public partial class SOrganization
    {
        public SOrganization()
        {
            AspNetUsers = new HashSet<AspNetUser>();
            DFolders = new HashSet<DFolder>();
        }

        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Организация
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string OrganizationAddress { get; set; }
        /// <summary>
        /// Телефон
        /// </summary>
        public string OrganizationTel { get; set; }
        /// <summary>
        /// Электронная почта
        /// </summary>
        public string OrganizationEmail { get; set; }

        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public virtual ICollection<DFolder> DFolders { get; set; }
    }
}
