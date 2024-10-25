using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Task = Lab3Logic.StructDataBase.Task;

namespace Web3Lab.Pages.TaskPage
{
    public class TaskModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TaskModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Task> Tasks { get; set; } = new List<Task>();
        public List<Employee> Employees { get; set; } = new List<Employee>();

        public void OnGet()
        {
            LoadTasks();
            LoadEmployees();
        }

        private void LoadTasks()
        {
            Tasks = _context.Tasks
                .Include(t => t.Author)
                .Include(t => t.Contractor)
                .ToList();
        }

        private void LoadEmployees()
        {
            Employees = _context.Employees.ToList();
        }

        public IActionResult OnPostDeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Задача успешно удалена!";
            }
            else
            {
                TempData["ErrorMessage"] = "Задача не найдена.";
            }
            return RedirectToPage();
        }
    }
}
