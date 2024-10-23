using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web3Lab.Pages
{
    public class EmployeeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EmployeeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Employees.Add(Employee);
            _context.SaveChanges();

            return RedirectToPage("/Success"); 
        }
    }
}
