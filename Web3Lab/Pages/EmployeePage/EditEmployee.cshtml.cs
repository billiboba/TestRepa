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
                TempData["ErrorMessage"] = "��������� �� ������!";
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            Console.WriteLine($"���������� ���������� � ���������������: {Employee.Id}");

            var employeeToUpdate = _context.Employees.FirstOrDefault(e => e.Id == Employee.Id);

            if (employeeToUpdate == null)
            {
                Console.WriteLine("��������� �� ������ ��� ����������!");
                TempData["ErrorMessage"] = "��������� �� ������!";
                return RedirectToPage("Employees");
            }

            employeeToUpdate.FirstName = Employee.FirstName;
            employeeToUpdate.LastName = Employee.LastName;
            employeeToUpdate.MiddleName = Employee.MiddleName;
            employeeToUpdate.Email = Employee.Email;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "��������� ������� ��������!";
                Console.WriteLine("������ ���������� ��������� �������!");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "������ ��� ���������� ����������: " + ex.Message;
                Console.WriteLine($"������: {ex.Message}");
            }

            return RedirectToPage("Employees");
        }

    }
}
