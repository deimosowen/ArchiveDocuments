using ArchiveDocuments.Data.Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ArchiveDocuments.Models
{
    public class AddDocumentRequestModel
    {
        public SelectList OrganizationList { get; internal set; }
        public SelectList DocumentList { get; internal set; }
        public DServicesDocument Document { get; internal set; }
    }

    public class AddDocumentMetadata
    {
        public Guid Id { get; internal set; }
        public bool? IsRequired { get; internal set; }
        public string Length { get; internal set; }
        public string Name { get; internal set; }
        public MetadataType MetadataType { get; internal set; }
    }
}
