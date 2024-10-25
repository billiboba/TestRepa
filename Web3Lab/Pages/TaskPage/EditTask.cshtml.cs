using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Web3Lab.Pages.TaskPage
{
    public class EditTaskModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditTaskModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int Id { get; set; }

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

        public SelectList EmployeesSelectList { get; set; }
        public IActionResult OnGet(int id)
        {
            var task = _context.Tasks
                .Include(t => t.Author)
                .Include(t => t.Contractor)
                .FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                TempData["ErrorMessage"] = "Задача не найдена.";
                return RedirectToPage("/TaskPage/Tasks");
            }

            Id = task.Id;
            Name = task.Name;
            AuthorId = task.AuthorId;
            ContractorId = task.ContractorId;
            Status = task.Status;
            Comment = task.Comment;
            Priority = task.Priority;

            EmployeesSelectList = new SelectList(_context.Employees.ToList(), "Id", "FullName", AuthorId);
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                EmployeesSelectList = new SelectList(_context.Employees.ToList(), "Id", "FullName");
                return Page();
            }

            var taskToUpdate = _context.Tasks.Find(Id);

            if (taskToUpdate == null)
            {
                TempData["ErrorMessage"] = "Задача не найдена.";
                return RedirectToPage("/TaskPage/Tasks");
            }

            taskToUpdate.Name = Name;
            taskToUpdate.AuthorId = AuthorId;
            taskToUpdate.ContractorId = ContractorId;
            taskToUpdate.Status = Status;
            taskToUpdate.Comment = Comment;
            taskToUpdate.Priority = Priority;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Задача успешно обновлена!";

            return RedirectToPage("/TaskPage/Tasks");
        }
    }
}
