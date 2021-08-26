using System;
using System.Linq;
using System.Security.Claims;
using ArchiveDocuments.Data.Concrete;
using ArchiveDocuments.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchiveDocuments.Controllers
{
    [Authorize]
    public class AbstractController : Controller
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
    }
}
