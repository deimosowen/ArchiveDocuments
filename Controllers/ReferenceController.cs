using ArchiveDocuments.Data.Concrete;
using ArchiveDocuments.Data.Entities;
using ArchiveDocuments.Data.Entities.Models;
using ArchiveDocuments.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArchiveDocuments.Controllers
{
    public class ReferenceController : AbstractController
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        public ReferenceController(ArchiveContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Employees
        public IActionResult Employees()
        {
            return View("Employees/Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeesJsonAsync(DataTableParameters dataTableParameters)
        {
            var data = await _context.AspNetUsers
                .Where(w => dataTableParameters.Search.Value == null ||
                            w.EmployeesName.StartsWith(dataTableParameters.Search.Value))
                .Skip(dataTableParameters.Start)
                .Take(dataTableParameters.Length)
                .AsNoTracking().Select(s => new
                {
                    s.Id,
                    s.EmployeesName,
                    s.Sorganization.OrganizationName,
                    isLock = !s.LockoutEnd.HasValue
                }).OrderBy(o => o.EmployeesName).ToListAsync();

            return Json(new
            {
                draw = dataTableParameters.Draw,
                recordsTotal = await _context.AspNetUsers.CountAsync(),
                recordsFiltered = data.Count(),
                data
            });
        }

        public IActionResult EmployeeCreate()
        {
            ViewBag.OrganizationList = new SelectList(_context.SOrganizations.Select(s => new
            {
                s.Id,
                s.OrganizationName
            }).OrderBy(o => o.OrganizationName), "Id", "OrganizationName");
            return View("Employees/Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeCreate(ApplicationUser applicationUser)
        {
            var user = await _userManager.CreateAsync(applicationUser, applicationUser.PasswordHash);
            
            await _userManager.AddToRoleAsync(applicationUser, "user");
            return RedirectToAction(nameof(Employees), new { actionStatus = ActionStatus.AddSuccess });
        }

        public async Task<IActionResult> EmployeeEdit(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            ViewBag.OrganizationList = new SelectList(_context.SOrganizations.Select(s => new
            {
                s.Id,
                s.OrganizationName
            }).OrderBy(o => o.OrganizationName), "Id", "OrganizationName");
            return View("Employees/Edit", user);
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeEdit(ApplicationUser applicationUser)
        {
            var update = _context.AspNetUsers.Single(s => s.Id == applicationUser.Id);
            update.EmployeesName = applicationUser.EmployeesName;
            update.SorganizationId = applicationUser.SOrganizationId;
            update.PhoneNumber = applicationUser.PhoneNumber;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Employees), new { actionStatus = ActionStatus.EditSuccess });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeDelete(Guid id)
        {
            _context.Remove(await _context.AspNetUsers.FindAsync(id));
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Employees), new { actionStatus = ActionStatus.DeleteSuccess });
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckValid(Guid? id, string userName, string email)
        {
            if (!string.IsNullOrWhiteSpace(userName))
                return Json(new { valid = !_context.AspNetUsers.Any(a => a.UserName == userName) });

            if (!string.IsNullOrWhiteSpace(email))
                return Json(new { valid = !_context.AspNetUsers.Any(a => a.Email == email && a.Id != id) });

            return Json(new { valid = true });
        }
        #endregion

        #region Documents

        public IActionResult Documents()
        {
            return View("Documents/Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetDocumentsJsonAsync(DataTableParameters dataTableParameters)
        {
            var data = await _context.SDocuments
                .Where(w => dataTableParameters.Search.Value == null ||
                            w.DocumentName.StartsWith(dataTableParameters.Search.Value))
                .Skip(dataTableParameters.Start)
                .Take(dataTableParameters.Length)
                .AsNoTracking().Select(s => new
                {
                    s.Id,
                    s.DocumentName,
                    s.EmployeesNameAdd,
                    Metadata = s.SDocumentMetadata.Count,
                }).OrderBy(o => o.DocumentName).ToListAsync();

            return Json(new
            {
                draw = dataTableParameters.Draw,
                recordsTotal = await _context.SDocuments.CountAsync(),
                recordsFiltered = data.Count(),
                data
            });
        }

        public IActionResult DocumentCreate()
        {
            return View("Documents/Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DocumentCreate(SDocument document)
        {
            try
            {
                document.EmployeesNameAdd = UserInfo.UserName;
                foreach (var metadata in document.SDocumentMetadata)
                    metadata.EmployeesNameAdd = UserInfo.UserName;

                await _context.SDocuments.AddAsync(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Documents), new { actionStatus = ActionStatus.AddSuccess });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction(nameof(Documents), new { actionStatus = ActionStatus.AddError});
            }
        }

        public async Task<IActionResult> DocumentEdit(Guid id)
        {
            var document = await _context.SDocuments.Include(i=>i.SDocumentMetadata).FirstAsync(f=>f.Id == id);
            return View("Documents/Edit", document);
        }

        [HttpPost]
        public async Task<IActionResult> DocumentEdit(SDocument document)
        {
            try
            {
                var updateDocument = await _context.SDocuments
                    .Include(i => i.SDocumentMetadata)
                    .SingleAsync(s => s.Id == document.Id);

                updateDocument.DocumentName = document.DocumentName;
                foreach (var metadata in document.SDocumentMetadata)
                {
                    metadata.EmployeesNameAdd = UserInfo.UserName;
                    metadata.SDocumentId = updateDocument.Id;
                }

                _context.SDocumentMetadata.RemoveRange(updateDocument.SDocumentMetadata);
                _context.SDocumentMetadata.AddRange(document.SDocumentMetadata);
                await _context.SaveChangesAsync(); 

                return RedirectToAction(nameof(Documents), new { actionStatus = ActionStatus.EditSuccess });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction(nameof(Documents), new { actionStatus = ActionStatus.EditError });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DocumentDelete(Guid id)
        {
            try
            {
                var document = await _context.SDocuments
                    .Include(i => i.DServices)
                    .Include(i => i.SDocumentMetadata)
                    .FirstAsync(f => f.Id == id);
                if (document.DServices.Any())
                    return RedirectToAction(nameof(Documents), new { actionStatus = ActionStatus.DeleteError });

                if (document.SDocumentMetadata.Any()) 
                    _context.RemoveRange(document.SDocumentMetadata);

                _context.Remove(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Documents), new { actionStatus = ActionStatus.DeleteSuccess });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction(nameof(Documents), new { actionStatus = ActionStatus.DeleteError });
            }
        }

        #endregion
    }
}
