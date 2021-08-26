using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchiveDocuments.Models
{
    public class TreeModel
    {
        public Guid Id { get; internal set; }
        public string Value { get; internal set; }
        public bool Opened { get; internal set; }
        public List<TreeModel> Items { get; internal set; }
        public object Icon { get; set; }
    }
}
