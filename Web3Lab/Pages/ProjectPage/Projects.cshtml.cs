using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
                .FirstOrDefault(p => p.Id == id);

            if (project != null)
            {
                _context.ProjectEmployees.RemoveRange(project.ProjectEmployees);

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
        public IActionResult OnPostSortProjects()
        {
            IQueryable<Project> query = _context.Projects
                .Include(p => p.ProjectEmployees)
                .ThenInclude(pe => pe.Employee);

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

            return RedirectToPage(); 
        }


        public IActionResult OnPostRemoveEmployeeFromProject(int projectId, int employeeId)
        {
            var projectEmployee = _context.ProjectEmployees
                .FirstOrDefault(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

            if (projectEmployee != null)
            {
                _context.ProjectEmployees.Remove(projectEmployee);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}
