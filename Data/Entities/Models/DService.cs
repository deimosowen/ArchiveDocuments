using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Data.Entities.Models
{
    /// <summary>
    /// Дела
    /// </summary>
    public partial class DService
    {
        public DService()
        {
            DServicesDocuments = new HashSet<DServicesDocument>();
        }

        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Папка
        /// </summary>
        public Guid DFolderId { get; set; }
        /// <summary>
        /// Документ
        /// </summary>
        public Guid SDocumentId { get; set; }
        /// <summary>
        /// Год начала
        /// </summary>
        public int YearStart { get; set; }
        /// <summary>
        /// Год окончания
        /// </summary>
        public int YearStop { get; set; }
        /// <summary>
        /// Количество страниц
        /// </summary>
        public int CountPage { get; set; }
        /// <summary>
        /// Номер дела
        /// </summary>
        public string CaseNumber { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        public virtual DFolder DFolder { get; set; }
        public virtual SDocument SDocument { get; set; }
        public virtual ICollection<DServicesDocument> DServicesDocuments { get; set; }
    }
}
