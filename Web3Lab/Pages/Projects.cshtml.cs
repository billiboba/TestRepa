using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Web3Lab.Pages
{
    public class ProjectsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProjectsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Project> Projects { get; set; } = new List<Project>();

        [BindProperty]
        public string SortOption { get; set; }

        public void OnGet()
        {
            LoadProjects();
        }

        private void LoadProjects()
        {
            Projects = _context.Projects
                .Include(p => p.ProjectEmployees)
                .ThenInclude(pe => pe.Employee)
                .ToList();
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
            var project = _context.Projects.Find(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }

            TempData["SuccessMessage"] = "Проект успешно удален!";
            return RedirectToPage();
        }

        public IActionResult OnPostSortProjects()
        {
            switch (SortOption)
            {
                case "Priority":
                    Projects = _context.Projects.OrderBy(p => p.Priority).ToList();
                    break;
                case "Name":
                    Projects = _context.Projects.OrderBy(p => p.Name).ToList();
                    break;
                case "Date":
                    Projects = _context.Projects.OrderBy(p => p.StartDate).ToList();
                    break;
            }
            return Page();
        }
    }
}
