using System;
using System.Collections.Generic;

namespace ArchiveDocuments.Models
{
    public class TreeModel
    {
        public TreeModel(Guid id, string value, IEnumerable<TreeModel> items = null, bool opened = false)
        {
            Id = id;
            Value = value;
            Opened = opened;
            Items = items;
            
        }

        public Guid Id { get; internal set; }
        public string Value { get; internal set; }
        public bool Opened { get; internal set; }
        public IEnumerable<TreeModel> Items { get; internal set; }
    }
}
