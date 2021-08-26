using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Data.Entities.Models
{
    /// <summary>
    /// Документ к делу
    /// </summary>
    public partial class DServicesDocument
    {
        public DServicesDocument()
        {
            DServicesDocumentFiles = new HashSet<DServicesDocumentFile>();
            DServicesMetadata = new HashSet<DServicesMetadatum>();
        }

        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Дело
        /// </summary>
        public Guid DServicesId { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime DateAdd { get; set; }
        /// <summary>
        /// Сотрудник добавивший
        /// </summary>
        public string EmployeesNameAdd { get; set; }

        public virtual DService DServices { get; set; }
        public virtual ICollection<DServicesDocumentFile> DServicesDocumentFiles { get; set; }
        public virtual ICollection<DServicesMetadatum> DServicesMetadata { get; set; }
    }
}
