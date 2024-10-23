using Lab3Logic.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Web3Lab.Pages
{
    public class MenuModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MenuModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var tableExists = _context.Database.ExecuteSqlRaw(
                "IF OBJECT_ID('dbo.Tasks', 'U') IS NOT NULL SELECT 1 ELSE SELECT 0") == 1;
            _context.Database.Migrate();
            
        }

        public IActionResult OnPostProjects()
        {
            return RedirectToPage("Projects");
        }

        public IActionResult OnPostEmployees()
        {
            return RedirectToPage("Employees");
        }

        public IActionResult OnPostTasks()
        {
            return RedirectToPage("Tasks");
        }
    }
}
