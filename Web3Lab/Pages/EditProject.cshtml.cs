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
            Console.WriteLine($"������������� �������: {id}");

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
                Console.WriteLine("������ �� ������!");
                return NotFound();
            }

            Console.WriteLine($"����������� ������ �������: {Project.Name}, {Project.ClientCompany}");
            return Page();
        }

        public IActionResult OnPost()
        {
            Console.WriteLine($"���������� ������� � ���������������: {Project.Id}");

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
                Console.WriteLine("������ �� ������ ��� ����������!");
                TempData["ErrorMessage"] = "������ �� ������!";
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
                TempData["SuccessMessage"] = "������ ������� ��������!";
                Console.WriteLine("������ ������� ��������� �������!");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "������ ��� ���������� �������: " + ex.Message;
                Console.WriteLine($"������: {ex.Message}");
            }

            return RedirectToPage("Projects");
        }





    }
}
