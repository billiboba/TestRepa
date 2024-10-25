using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Web3Lab.Pages
{
    public class EmployeesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EmployeesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Employee> Employees { get; set; } = new List<Employee>();
        [BindProperty]
        public Employee NewEmployee { get; set; }

        [BindProperty]
        public string SortOption { get; set; }

        public void OnGet()
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            Employees = _context.Employees.ToList();
        }

        public IActionResult OnPostAddEmployee()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Ошибка валидации формы.";
                return Page();
            }

            if (NewEmployee == null)
            {
                TempData["ErrorMessage"] = "Данные сотрудника не переданы.";
                return Page();
            }

            try
            {
                _context.Employees.Add(NewEmployee);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Сотрудник успешно добавлен!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при добавлении сотрудника: " + ex.Message;
                return Page();
            }

            return RedirectToPage();
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

        public IActionResult OnPostSortEmployees()
        {
            switch (SortOption)
            {
                case "LastName":
                    Employees = _context.Employees.OrderBy(e => e.LastName).ToList();
                    break;
                case "FirstName":
                    Employees = _context.Employees.OrderBy(e => e.FirstName).ToList();
                    break;
                case "Email":
                    Employees = _context.Employees.OrderBy(e => e.Email).ToList();
                    break;
            }
            return Page();
        }
    }
}
