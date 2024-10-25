using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Web3Lab.Pages.EmployeePage
{
    public class EditEmployeeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditEmployeeModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public IActionResult OnGet(int id)
        {
            Employee = _context.Employees.FirstOrDefault(e => e.Id == id);

            if (Employee == null)
            {
                TempData["ErrorMessage"] = "Сотрудник не найден!";
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            Console.WriteLine($"Обновление сотрудника с идентификатором: {Employee.Id}");

            var employeeToUpdate = _context.Employees.FirstOrDefault(e => e.Id == Employee.Id);

            if (employeeToUpdate == null)
            {
                Console.WriteLine("Сотрудник не найден при обновлении!");
                TempData["ErrorMessage"] = "Сотрудник не найден!";
                return RedirectToPage("Employees");
            }

            employeeToUpdate.FirstName = Employee.FirstName;
            employeeToUpdate.LastName = Employee.LastName;
            employeeToUpdate.MiddleName = Employee.MiddleName;
            employeeToUpdate.Email = Employee.Email;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Сотрудник успешно обновлен!";
                Console.WriteLine("Данные сотрудника обновлены успешно!");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при обновлении сотрудника: " + ex.Message;
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            return RedirectToPage("Employees");
        }

    }
}
