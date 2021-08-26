using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Data.Entities.Models
{
    /// <summary>
    /// Справочник документов
    /// </summary>
    public partial class SDocument
    {
        public SDocument()
        {
            DServices = new HashSet<DService>();
            SDocumentMetadata = new HashSet<SDocumentMetadatum>();
        }

        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Наименование документа
        /// </summary>
        public string DocumentName { get; set; }
        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime DateAdd { get; set; }
        /// <summary>
        /// Сотрудник добавивший
        /// </summary>
        public string EmployeesNameAdd { get; set; }

        public virtual ICollection<DService> DServices { get; set; }
        public virtual ICollection<SDocumentMetadatum> SDocumentMetadata { get; set; }
    }
}
