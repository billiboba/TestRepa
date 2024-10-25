using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Web3Lab.Pages
{
    public class EditProjectModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditProjectModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Project Project { get; set; }

        public IActionResult OnGet(int id)
        {
            Console.WriteLine($"Идентификатор проекта: {id}");

            Project = _context.Projects.FirstOrDefault(p => p.Id == id);

            if (Project != null)
            {
                Project.ProjectEmployees = _context.ProjectEmployees
                    .Where(pe => pe.ProjectId == Project.Id)
                    .Include(pe => pe.Employee)
                    .ToList();
            }

            if (Project == null)
            {
                Console.WriteLine("Проект не найден!");
                return NotFound();
            }

            Console.WriteLine($"Загруженные данные проекта: {Project.Name}, {Project.ClientCompany}");
            return Page();
        }

        public IActionResult OnPost()
        {
            Console.WriteLine($"Обновление проекта с идентификатором: {Project.Id}");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); 
                }
                return Page();
            }

            var projectToUpdate = _context.Projects
                .Include(p => p.ProjectEmployees)
                .FirstOrDefault(p => p.Id == Project.Id);

            if (projectToUpdate == null)
            {
                Console.WriteLine("Проект не найден при обновлении!");
                TempData["ErrorMessage"] = "Проект не найден!";
                return RedirectToPage("Projects");
            }

            projectToUpdate.Name = Project.Name;
            projectToUpdate.ClientCompany = Project.ClientCompany;
            projectToUpdate.ContractorCompany = Project.ContractorCompany;
            projectToUpdate.StartDate = Project.StartDate;
            projectToUpdate.EndDate = Project.EndDate;
            projectToUpdate.Priority = Project.Priority;
            projectToUpdate.ProjectManager = Project.ProjectManager;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Проект успешно обновлен!";
                Console.WriteLine("Данные проекта обновлены успешно!");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при обновлении проекта: " + ex.Message;
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            return RedirectToPage("Projects");
        }





    }
}
