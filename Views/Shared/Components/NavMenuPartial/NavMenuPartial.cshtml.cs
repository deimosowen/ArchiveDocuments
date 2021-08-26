using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArchiveDocuments.Views.Shared.Components.NavMenuPartial
{
    public class NavMenuPartial : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("NavMenuPartial");
        }

        public static string Index => "Index";
        public static string Documents => "Documents";
        public static string Search => "Search";
        public static string Employees => "Employees";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string DocumentsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Documents);
        public static string SearchNavClass(ViewContext viewContext) => PageNavClass(viewContext, Search);
        public static string EmployeesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Employees);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.RouteData.Values["Action"]?.ToString() ??  System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

    }
}