using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web3Lab.Pages
{
    public class AddProjectModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddProjectModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // —войства дл€ прив€зки данных формы
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

        // ћетод дл€ обработки данных формы при POST-запросе
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // —оздание нового проекта с полученными данными
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

            // ƒобавление проекта в базу данных
            _context.Projects.Add(project);
            _context.SaveChanges();

            // ”станавливаем сообщение об успешном добавлении проекта
            TempData["SuccessMessage"] = "ѕроект успешно добавлен!";

            // ѕеренаправление на страницу списка проектов
            return RedirectToPage("Projects");
        }
    }
}
