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
        [Required(ErrorMessage = "�������� ������� �����������.")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "��������-�������� �����������.")]
        public string ClientCompany { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "��������-����������� �����������.")]
        public string ContractorCompany { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "���� ������ �����������.")]
        public DateTime StartDate { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "���� ��������� �����������.")]
        public DateTime EndDate { get; set; }

        [BindProperty]
        [Range(0, 2, ErrorMessage = "��������� ������ ���� �� 0 �� 2.")]
        public int Priority { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "������������ ������� ����������.")]
        public string ProjectManager { get; set; }

        public IActionResult OnPost()
        {
            if (StartDate > EndDate)
            {
                ModelState.AddModelError(string.Empty, "���� ������ �� ����� ���� ����� ���� ���������.");
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

            TempData["SuccessMessage"] = "������ ������� ��������!";

            return RedirectToPage("Projects");
        }
    }
}
