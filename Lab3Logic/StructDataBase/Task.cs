namespace Lab3Logic.StructDataBase
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AuthorId { get; set; }
        public Employee Author { get; set; }
        public int? ContractorId { get; set; }
        public Employee Contractor { get; set; }
        public TaskStat Status { get; set; }
        public string Comment { get; set; }
        public int Priority { get; set; }
        public int? ProjectId { get; set; }
        public Project Project { get; set; }


    }
    public enum TaskStat
    {
        Todo = 0,
        InProgress = 1,
        Done = 2
    }
}
