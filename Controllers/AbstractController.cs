using ArchiveDocuments.Data.Concrete;
using ArchiveDocuments.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace ArchiveDocuments.Controllers
{
    [Authorize]
    public abstract class AbstractController : Controller
    {
        protected AbstractController(ArchiveContext context)
        {
            _context = context;
        }

        protected readonly ArchiveContext _context;
        protected UserInfo UserInfo => _context.AspNetUsers.Select(s => new UserInfo
        {
            UserId = s.Id,
            UserName = s.EmployeesName
        }).Single(s => s.UserId == Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));

        public SelectList GetFolderReference(Guid id) => new(_context.DFolders.AsNoTracking()
            .Where(w => w.SOrganizationId == id)
            .Select(s => new { Name = s.FolderName, Id = s.Id }), "Id", "Name");

        public SelectList GetServicesReference(Guid folderId, Guid documentId) => new(_context.DServices.AsNoTracking()
            .Where(w => w.DFolderId == folderId && w.SDocumentId == documentId)
            .Select(s => new { Name = s.CaseNumber, Id = s.Id }), "Id", "Name");
    }
}
