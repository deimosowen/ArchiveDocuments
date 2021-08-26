using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Data.Entities.Models
{
    /// <summary>
    /// Справочник Метаданных к документу
    /// </summary>
    public partial class SDocumentMetadatum
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Документ
        /// </summary>
        public Guid SDocumentId { get; set; }
        /// <summary>
        /// Наименование поля
        /// </summary>
        public string MetadataName { get; set; }
        /// <summary>
        /// Тип (1 число 2 строка 3 дата 4 текст)
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Обязательность
        /// </summary>
        public bool? IsBinding { get; set; }
        /// <summary>
        /// Длина поля
        /// </summary>
        public string MetadataLength { get; set; }
        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime DateAdd { get; set; }
        /// <summary>
        /// Сотрудник добавивший
        /// </summary>
        public string EmployeesNameAdd { get; set; }

        public virtual SDocument SDocument { get; set; }
    }
}
