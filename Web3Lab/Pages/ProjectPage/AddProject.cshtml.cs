using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web3Lab.Pages.ProjectPage
{
    public class AddProjectModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddProjectModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string ClientCompany { get; set; }
        [BindProperty]
        public string ContractorCompany { get; set; }
        [BindProperty]
        public DateTime StartDate { get; set; }
        [BindProperty]
        public DateTime EndDate { get; set; }
        [BindProperty]
        public int Priority { get; set; }
        [BindProperty]
        public string ProjectManager { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var project = new Project
            {
                Name = Name,
                ClientCompany = ClientCompany,
                ContractorCompany = ContractorCompany,
                StartDate = StartDate,
                EndDate = EndDate,
                Priority = Priority,
                ProjectManager = ProjectManager
            };

            _context.Projects.Add(project);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Проект успешно добавлен!";

            return RedirectToPage("Projects");
        }
    }
}
