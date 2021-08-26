using ArchiveDocuments.Data.Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public MetadataTypeEnum MetadataType { get; internal set; }
    }

    public enum MetadataTypeEnum
    {
        Number = 1,
        String = 2,
        Date = 3,
        Text = 4
    }
}
