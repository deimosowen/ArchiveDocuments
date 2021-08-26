using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Data.Entities.Models
{
    /// <summary>
    /// Метаданные к документу
    /// </summary>
    public partial class DServicesMetadatum
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Документ
        /// </summary>
        public Guid DServicesDocumentId { get; set; }
        /// <summary>
        /// Наименование поля
        /// </summary>
        public string MetadataName { get; set; }
        /// <summary>
        /// Значение поля
        /// </summary>
        public string MetadataValue { get; set; }
        /// <summary>
        /// Обязательность
        /// </summary>
        public bool? IsBinding { get; set; }

        public virtual DServicesDocument DServicesDocument { get; set; }
    }
}
