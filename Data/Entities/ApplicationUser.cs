using Microsoft.AspNetCore.Identity;
using System;

namespace ArchiveDocuments.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid SOrganizationId { get; set; }
        public string EmployeesName { get; set; }
    }

    public class ApplicationUserInfo
    {
        public ApplicationUserInfo(string name)
        {
            var splitName = name.Split(" ");
            FirstName = splitName.Length >= 2 ? splitName[1] : "";
            LastName = splitName.Length >= 1 ? splitName[0] : "";
            BadgeName = $"{FirstName[0]}";
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string BadgeName { get; }

    }
}
