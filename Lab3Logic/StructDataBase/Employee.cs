namespace Lab3Logic.StructDataBase
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }

        public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
        public ICollection<Task> AuthoredTasks { get; set; }
        public ICollection<Task> ContractorTasks { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
