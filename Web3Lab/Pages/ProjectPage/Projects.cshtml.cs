using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Task = Lab3Logic.StructDataBase.Task;

namespace Web3Lab.Pages.ProjectPage
{
    public class ProjectsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProjectsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Project> Projects { get; set; } = new List<Project>();
        public List<Employee> AllEmployees { get; set; } = new List<Employee>();

        [BindProperty]
        public int SelectedEmployeeId { get; set; }
        [BindProperty]
        public int SelectedProjectId { get; set; }
        [BindProperty]
        public int SelectedTaskId { get; set; } 
        [BindProperty]
        public string NewTaskName { get; set; } 
        [BindProperty]
        public string SortOption { get; set; }

        public void OnGet()
        {
            LoadProjects();
            LoadAllEmployees();
        }

        private void LoadProjects()
        {
            Projects = _context.Projects
                .Include(p => p.ProjectEmployees)
                    .ThenInclude(pe => pe.Employee)
                .Include(p => p.Tasks)
                .ToList();
        }

        private void LoadAllEmployees()
        {
            AllEmployees = _context.Employees.ToList();
        }

        public IActionResult OnPostAddProject()
        {
            return RedirectToPage("AddProject");
        }
        public IActionResult OnPostEditProject(int id)
        {
            return RedirectToPage("EditProject", new { id });
        }
        public IActionResult OnPostDeleteProject(int id)
        {
            var project = _context.Projects
                .Include(p => p.ProjectEmployees)
                .Include(p => p.Tasks)
                .FirstOrDefault(p => p.Id == id);

            if (project != null)
            {
                _context.ProjectEmployees.RemoveRange(project.ProjectEmployees);
                _context.Tasks.RemoveRange(project.Tasks);
                _context.Projects.Remove(project);

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Проект успешно удален!";
            }
            else
            {
                TempData["ErrorMessage"] = "Проект не найден!";
            }

            return RedirectToPage();
        }
        public IActionResult OnPostRemoveEmployeeFromProject(int projectId, int employeeId)
        {
            var projectEmployee = _context.ProjectEmployees
                .FirstOrDefault(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

            if (projectEmployee != null)
            {
                _context.ProjectEmployees.Remove(projectEmployee);

                var tasksWithAuthor = _context.Tasks.Where(t => t.AuthorId == employeeId).ToList();
                var tasksWithContractor = _context.Tasks.Where(t => t.ContractorId == employeeId).ToList();

                _context.Tasks.RemoveRange(tasksWithAuthor);
                _context.Tasks.RemoveRange(tasksWithContractor);

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Сотрудник и связанные задачи успешно удалены!";
            }
            else
            {
                TempData["ErrorMessage"] = "Сотрудник не найден в проекте.";
            }

            return RedirectToPage();
        }


        public IActionResult OnPostAddTaskToProject(int projectId)
        {
            if (string.IsNullOrWhiteSpace(NewTaskName))
            {
                TempData["ErrorMessage"] = "Введите название задачи для добавления в проект.";
                return RedirectToPage();
            }

            if (SelectedEmployeeId == 0)
            {
                TempData["ErrorMessage"] = "Пожалуйста, выберите автора задачи.";
                return RedirectToPage();
            }

            var project = _context.Projects.Find(projectId);
            if (project != null)
            {
                var newTask = new Task
                {
                    Name = NewTaskName,
                    ProjectId = projectId,
                    AuthorId = SelectedEmployeeId
                };

                _context.Tasks.Add(newTask);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Задача успешно добавлена в проект!";
            }

            return RedirectToPage();
        }


        public IActionResult OnPostRemoveTaskFromProject(int projectId, int taskId)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId && t.ProjectId == projectId);

            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Задача успешно удалена из проекта!";
            }

            return RedirectToPage();
        }
        public IActionResult OnPostAddEmployeeToProject(int projectId)
        {
            if (SelectedEmployeeId == 0)
            {
                TempData["ErrorMessage"] = "Пожалуйста, выберите сотрудника для добавления в проект.";
                return RedirectToPage();
            }

            var project = _context.Projects
                .Include(p => p.ProjectEmployees)
                .FirstOrDefault(p => p.Id == projectId);

            if (project != null && !_context.ProjectEmployees.Any(pe => pe.ProjectId == projectId && pe.EmployeeId == SelectedEmployeeId))
            {
                var projectEmployee = new ProjectEmployee
                {
                    ProjectId = projectId,
                    EmployeeId = SelectedEmployeeId
                };

                _context.ProjectEmployees.Add(projectEmployee);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Сотрудник успешно добавлен в проект!";
            }
            else
            {
                TempData["ErrorMessage"] = "Не удалось добавить сотрудника в проект.";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostSortProjects()
        {
            IQueryable<Project> query = _context.Projects
                .Include(p => p.ProjectEmployees)
                    .ThenInclude(pe => pe.Employee)
                .Include(p => p.Tasks);

            switch (SortOption)
            {
                case "Priority":
                    query = query.OrderBy(p => p.Priority);
                    break;
                case "Name":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "Date":
                    query = query.OrderBy(p => p.StartDate);
                    break;
            }

            Projects = query.ToList();
            LoadAllEmployees();

            return Page();
        }
    }
}
