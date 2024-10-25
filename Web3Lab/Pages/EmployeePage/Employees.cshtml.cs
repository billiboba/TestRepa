using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Web3Lab.Pages.EmployeePage
{
    public class EmployeesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EmployeesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Employee> Employees { get; set; } = new List<Employee>();

        public void OnGet()
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            Employees = _context.Employees
                .Include(e => e.ProjectEmployees)
                .ThenInclude(pe => pe.Project)
                .ToList();
        }

        public IActionResult OnPostAddEmployee()
        {
            return RedirectToPage("AddEmployee");
        }

        public IActionResult OnPostEditEmployee(int id)
        {
            return RedirectToPage("EditEmployee", new { id });
        }

        public IActionResult OnPostDeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }

            TempData["SuccessMessage"] = "Сотрудник успешно удален!";
            return RedirectToPage();
        }
    }
}
