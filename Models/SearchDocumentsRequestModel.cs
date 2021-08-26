using ArchiveDocuments.Data.Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArchiveDocuments.Models
{
    public class SearchDocumentsRequestModel
    {
        public SearchDocumentsRequestModel()
        {
            Documents = new List<DocumentsModel>();
        }

        public SelectList OrganizationList { get; internal set; }
        public SelectList DocumentList { get; internal set; }
        public List<DocumentsModel> Documents { get; internal set; }
        public string Query { get; internal set; }
        public Guid? OrganizationId { get; set; }
        public Guid? DocumentId { get; set; }
    }

    public class DocumentsModel
    {
        public Guid Id { get; internal set; }
        public string Name { get; internal set; }
        public string OrganizationName { get; internal set; }
        public string Description { get; internal set; }
        public string Folder { get; internal set; }
        public string Service { get; internal set; }
        public int YearStart { get; internal set; }
        public int YearStop { get; internal set; }   
        public IEnumerable<DocumentMetadata> Metadata { get; internal set; }
        public List<DServicesDocumentFile> Files { get; internal set; }
        public string FileContent { get; internal set; }
    }

    public class DocumentMetadata
    {
        public string Name { get; internal set; }
        public string Value { get; internal set; }
    }
}
