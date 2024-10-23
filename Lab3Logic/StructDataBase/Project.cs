namespace Lab3Logic.StructDataBase
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientCompany { get; set; }
        public string ContractorCompany { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public string ProjectManager { get; set; }

        public ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
