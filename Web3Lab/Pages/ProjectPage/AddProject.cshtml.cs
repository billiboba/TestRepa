using Lab3Logic.Data;
using Lab3Logic.StructDataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Название проекта обязательно.")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Компания-заказчик обязательна.")]
        public string ClientCompany { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Компания-исполнитель обязательна.")]
        public string ContractorCompany { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Дата начала обязательна.")]
        public DateTime StartDate { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Дата окончания обязательна.")]
        public DateTime EndDate { get; set; }

        [BindProperty]
        [Range(0, 2, ErrorMessage = "Приоритет должен быть от 0 до 2.")]
        public int Priority { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Руководитель проекта обязателен.")]
        public string ProjectManager { get; set; }

        public IActionResult OnPost()
        {
            if (StartDate > EndDate)
            {
                ModelState.AddModelError(string.Empty, "Дата начала не может быть позже даты окончания.");
            }

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
