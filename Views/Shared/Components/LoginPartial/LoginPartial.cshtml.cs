using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchiveDocuments.Data.Concrete;
using ArchiveDocuments.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArchiveDocuments.Views.Shared.Components.LoginPartial
{
    public class LoginPartial : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ArchiveContext _context;

        public LoginPartial(UserManager<ApplicationUser> userManager, ArchiveContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(Request.HttpContext.User);
            var userInfo = await _context.AspNetUsers.Select(s => new
            {
                emp = new ApplicationUserInfo(s.EmployeesName),
                id = s.Id
            }).FirstAsync(f => f.id == user.Id);

            return View("LoginPartial", userInfo.emp);
        }
    }
}