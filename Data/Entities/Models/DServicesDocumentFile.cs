using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Data.Entities.Models
{
    /// <summary>
    /// Файлы к документу
    /// </summary>
    public partial class DServicesDocumentFile
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
        /// имя файла
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// расширение файла
        /// </summary>
        public string FileExt { get; set; }
        /// <summary>
        /// размер файла
        /// </summary>
        public int FileSize { get; set; }
        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime DateAdd { get; set; }
        /// <summary>
        /// Сотрудник добавивший
        /// </summary>
        public string EmployeesNameAdd { get; set; }

        /// <summary>
        /// Сотрудник добавивший
        /// </summary>
        public string FileContent { get; set; }

        public virtual DServicesDocument DServicesDocument { get; set; }
    }
}
