using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Task = Lab3Logic.StructDataBase.Task;

namespace Web3Lab.Pages.TaskPage
{
    public class AddTaskModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddTaskModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public int? AuthorId { get; set; }

        [BindProperty]
        public int? ContractorId { get; set; }

        [BindProperty]
        public TaskStat Status { get; set; }

        [BindProperty]
        public string Comment { get; set; }

        [BindProperty]
        public int Priority { get; set; }

        [BindProperty]
        public int ProjectId { get; set; }

        public SelectList EmployeesSelectList { get; set; }
        public SelectList ProjectsSelectList { get; set; }

        public void OnGet()
        {
            var employees = _context.Employees.ToList();
            if (employees.Any())
            {
                EmployeesSelectList = new SelectList(employees, "Id", "FirstName");
            }
            else
            {
                EmployeesSelectList = new SelectList(new List<Employee>(), "Id", "FirstName");
            }

            var projects = _context.Projects.ToList();
            if (projects.Any())
            {
                ProjectsSelectList = new SelectList(projects, "Id", "Name");
            }
            else
            {
                ProjectsSelectList = new SelectList(new List<Project>(), "Id", "Name");
            }
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                EmployeesSelectList = new SelectList(_context.Employees.ToList(), "Id", "FullName");
                ProjectsSelectList = new SelectList(_context.Projects.ToList(), "Id", "Name");
                return Page();
            }

            var task = new Task
            {
                Name = Name,
                AuthorId = AuthorId,
                ContractorId = ContractorId,
                Status = Status,
                Comment = Comment,
                Priority = Priority,
                ProjectId = ProjectId
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Задача успешно добавлена!";
            return RedirectToPage("Tasks");
        }
    }
}
