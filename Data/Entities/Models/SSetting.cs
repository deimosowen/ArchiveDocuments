using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Data.Entities.Models
{
    /// <summary>
    /// Таблица с настройками
    /// </summary>
    public partial class SSetting
    {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование параметра
        /// </summary>
        public string ParamName { get; set; }
        /// <summary>
        /// Значение параметра
        /// </summary>
        public string ParamValue { get; set; }
    }
}
