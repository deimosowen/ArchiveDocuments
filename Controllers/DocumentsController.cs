using ArchiveDocuments.Data.Concrete;
using ArchiveDocuments.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ArchiveDocuments.Data.Entities.Models;
using ArchiveDocuments.Services;
using IronOcr;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NinjaNye.SearchExtensions;

namespace ArchiveDocuments.Controllers
{
    public class DocumentsController : AbstractController
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly IFtpService _ftpService;

        public DocumentsController(ArchiveContext context, ILogger<DocumentsController> logger, IFtpService ftpService)
            : base(context)
        {
            _logger = logger;
            _ftpService = ftpService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IEnumerable<TreeModel> GetTreeDocuments()
        {
            return _context.SOrganizations.Select(org => new TreeModel
            {
                Id = org.Id,
                Value = org.OrganizationName,
                Icon = new
                {
                    Folder = "dxi dxi-folder",
                    OpenFolder = "dxi dxi-folder-open",
                    File = "dxi dxi-folder"
                },
                Items = org.DFolders.Select(folder => new TreeModel
                {
                    Id = folder.Id,
                    Value = folder.FolderName,
                    Icon = new
                    {
                        Folder = "dxi dxi-folder",
                        OpenFolder = "dxi dxi-folder-open",
                        File = "dxi dxi-folder"
                    },
                    Items = folder.DServices.Select(service => new TreeModel
                    {
                        Id = service.Id,
                        Value = service.CaseNumber,
                        Icon = new
                        {
                            Folder = "dxi dxi-folder",
                            OpenFolder = "dxi dxi-folder-open",
                            File = "dxi dxi-folder"
                        },
                        Items = service.DServicesDocuments.Select(doc => new TreeModel
                        {
                            Id = doc.Id,
                            Value = doc.Description,
                            Icon = new
                            {
                                Folder = "dxi dxi-folder",
                                OpenFolder = "dxi dxi-folder-open",
                                File = "dxi dxi-file-outline"
                            },
                        }).ToList(),
                    }).ToList()
                }).ToList()
            }).AsParallel().ToArray();
        }

        public IActionResult Search(string query, Guid? organizationId, Guid? documentId)
        {
            return View(new SearchDocumentsRequestModel
            {
                Query = query,
                DocumentId = documentId,
                OrganizationId = organizationId,
                OrganizationList =
                    new SelectList(
                        _context.SOrganizations.AsNoTracking()
                            .Select(s => new { Name = s.OrganizationName, Id = s.Id }), "Id", "Name", organizationId),
                DocumentList =
                    new SelectList(
                        _context.SDocuments.AsNoTracking().Select(s => new { Name = s.DocumentName, Id = s.Id }), "Id",
                        "Name", documentId),
                Documents = !string.IsNullOrEmpty(query)
                    ? _context.DServicesDocuments.AsNoTracking()
                        .Where(w => !documentId.HasValue || w.DServices.SDocumentId == documentId)
                        .Select(s => new DocumentsModel
                        {
                            Id = s.Id,
                            Name = s.DServices.SDocument.DocumentName,
                            Description = s.Description,
                            OrganizationName = s.DServices.DFolder.SOrganization.OrganizationName,
                            Folder = s.DServices.DFolder.FolderName,
                            Service = s.DServices.CaseNumber,
                            YearStart = s.DServices.YearStart,
                            YearStop = s.DServices.YearStop,
                            Metadata = s.DServicesMetadata.Select(ss => new DocumentMetadata
                                { Name = ss.MetadataName, Value = ss.MetadataValue }),
                            FileContent = string.Join(' ', s.DServicesDocumentFiles.Select(ss=>ss.FileContent))
                        }).ToList()
                        .Search(s => s.Name.ToLower(),
                            s => s.Folder.ToLower(),
                            s => s.Service.ToLower(),
                            s => s.FileContent.ToLower(),
                            s => s.Description.ToLower())
                        .Containing(query.ToLower().Split(" "))
                        //.SearchChildren(s => s.Metadata)
                        //.With(s => s.Value.ToLower())
                        //.Containing(query.ToLower())
                        .AsParallel()
                        .ToList()
                    : new List<DocumentsModel>()
            });
        }

        [HttpGet("[controller]/View/{id:guid}")]
        public IActionResult ViewDocument(Guid id)
        {
            return View(_context.DServicesDocuments
                .Select(s => new DocumentsModel
                {
                    Id = s.Id,
                    Description = s.Description,
                    YearStop = s.DServices.YearStop,
                    YearStart = s.DServices.YearStart,
                    Service = s.DServices.CaseNumber,
                    Folder = s.DServices.DFolder.FolderName,
                    Name = s.DServices.SDocument.DocumentName,
                    Metadata = s.DServicesMetadata.Select(ss => new DocumentMetadata
                        { Name = ss.MetadataName, Value = ss.MetadataValue }),
                    OrganizationName = s.DServices.DFolder.SOrganization.OrganizationName,
                    Files = s.DServicesDocumentFiles.ToList()
                })
                .Single(s => s.Id == id));
        }

        [HttpGet("[controller]/Add")]
        public IActionResult AddDocument()
        {
            return View(new AddDocumentRequestModel
            {
                OrganizationList =
                    new SelectList(
                        _context.SOrganizations.AsNoTracking()
                            .Select(s => new { Name = s.OrganizationName, Id = s.Id }), "Id", "Name"),
                DocumentList =
                    new SelectList(
                        _context.SDocuments.AsNoTracking().Select(s => new { Name = s.DocumentName, Id = s.Id }), "Id",
                        "Name"),
                Document = new DServicesDocument()
            });
        }

        public IActionResult AddDocumentFolderModal(Guid id)
        {
            return PartialView("Modal/_AddFolder", new DFolder()
            {
                SOrganizationId = id
            });
        }

        [HttpPost]
        public IActionResult AddDocumentFolder(DFolder dFolder)
        {
            dFolder.EmployeesNameAdd = UserInfo.UserName;
            _context.DFolders.Add(dFolder);
            _context.SaveChanges();
            return Json(new { id = dFolder.Id, name = dFolder.FolderName });
        }

        public IActionResult AddDocumentServiceModal(Guid folderId, Guid documentId)
        {
            return PartialView("Modal/_AddService", new DService()
            {
                DFolderId = folderId,
                SDocumentId = documentId
            });
        }

        [HttpPost]
        public IActionResult AddDocumentService(DService dService)
        {
            _context.DServices.Add(dService);
            _context.SaveChanges();
            return Json(new { id = dService.Id, name = dService.CaseNumber });
        }

        public IActionResult GetMetadataReference(Guid id)
        {
            return PartialView("_PartialMetadata", _context.SDocumentMetadata.Where(w => w.SDocumentId == id).Select(
                s => new AddDocumentMetadata
                {
                    Id = s.Id,
                    IsRequired = s.IsBinding,
                    Length = s.MetadataLength,
                    Name = s.MetadataName,
                    MetadataType = (MetadataTypeEnum)s.Type
                }).ToList());
        }

        public SelectList GetFolderReference(Guid id) => new(_context.DFolders.AsNoTracking()
            .Where(w => w.SOrganizationId == id).Select(s => new { Name = s.FolderName, Id = s.Id }), "Id", "Name");

        public SelectList GetServicesReference(Guid folderId, Guid documentId) => new(_context.DServices.AsNoTracking()
            .Where(w => w.DFolderId == folderId && w.SDocumentId == documentId)
            .Select(s => new { Name = s.CaseNumber, Id = s.Id }), "Id", "Name");

        [HttpPost]
        public async Task<IActionResult> AddDocument(DServicesDocument document, IFormFileCollection fileCollection, bool textRecognising)
        {
            document.DServices = null;
            document.EmployeesNameAdd = UserInfo.UserName;
            await _context.DServicesDocuments.AddAsync(document);
            await _context.SaveChangesAsync();

            foreach (var file in fileCollection)
            {
                var documentFile = new DServicesDocumentFile()
                {
                    FileName = Path.GetFileNameWithoutExtension(file.FileName),
                    FileExt = Path.GetExtension(file.FileName),
                    FileSize = (int)file.Length,
                    EmployeesNameAdd = UserInfo.UserName,
                    //ContentType = file.ContentType,
                    DServicesDocumentId = document.Id
                };

                if (textRecognising)
                {
                    var ocr = new IronTesseract
                    {
                        Language = OcrLanguage.Russian
                    };
                    // Ocr.AddSecondaryLanguage(OcrLanguage.English);
                    using var input = new OcrInput(file.OpenReadStream());
                    //Input.Deskew();
                    input.Binarize();
                    var result = ocr.Read(input);
                    documentFile.FileContent = result.Text;
                }

                await _context.DServicesDocumentFiles.AddAsync(documentFile);
                await _context.SaveChangesAsync();

                await _ftpService.UploadFileAsync(file.OpenReadStream(),
                    $"{document.Id}/{documentFile.Id}{documentFile.FileExt}");
            }

            return Json(new { id = document.Id });
        }

        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var file = await _context.DServicesDocumentFiles.SingleAsync(s => s.Id == id);
            return File(await _ftpService.DownloadFileAsync($"{file.DServicesDocumentId}/{file.Id}{file.FileExt}"),
                "image/jpeg", $"{file.FileName}{file.FileExt}");
        }
    }
}