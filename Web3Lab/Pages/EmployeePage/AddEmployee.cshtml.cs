using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web3Lab.Pages.EmployeePage
{
    public class AddEmployeeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddEmployeeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string MiddleName { get; set; }
        [BindProperty]
        public string Email { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var employee = new Employee
            {
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
                Email = Email
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Сотрудник успешно добавлен!";

            return RedirectToPage("Employees");
        }
    }
}
