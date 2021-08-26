using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Data.Entities.Models
{
    /// <summary>
    /// Папки
    /// </summary>
    public partial class DFolder
    {
        public DFolder()
        {
            DServices = new HashSet<DService>();
        }

        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Организация
        /// </summary>
        public Guid SOrganizationId { get; set; }
        /// <summary>
        /// Наименование папки
        /// </summary>
        public string FolderName { get; set; }
        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime DateAdd { get; set; }
        /// <summary>
        /// Сотрудник добавивший
        /// </summary>
        public string EmployeesNameAdd { get; set; }

        public virtual SOrganization SOrganization { get; set; }
        public virtual ICollection<DService> DServices { get; set; }
    }
}
