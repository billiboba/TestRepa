using Lab3Logic.StructDataBase;
using Microsoft.EntityFrameworkCore;
using Lab3Logic.Data;
using Task = Lab3Logic.StructDataBase.Task;
using Lab3Logic.View;

namespace Lab3BD.View
{
    public class LogicDataBase
    {
        public static void AddProject(ApplicationDbContext context, string name, string clientCompany, string contractorCompany,
        DateTime startD, DateTime endD, int priority, string projectManager)
        {
            var project = new Project
            {
                Name = name,
                ClientCompany = clientCompany,
                ContractorCompany = contractorCompany,
                StartDate = startD,
                EndDate = endD,
                Priority = priority,
                ProjectManager = projectManager
            };

            if (string.IsNullOrEmpty(project.Name) || string.IsNullOrEmpty(project.ClientCompany) ||
                string.IsNullOrEmpty(project.ContractorCompany) || string.IsNullOrEmpty(project.ProjectManager))
            {
                throw new ArgumentException("Все параметры должны быть заполнены.");
            }

            if (project.EndDate <= project.StartDate)
            {
                throw new ArgumentException("Дата окончания должна быть больше даты начала.");
            }

            context.Projects.Add(project);
            context.SaveChanges();
            Console.WriteLine("Проект добавлен.");
        }

        public static void EditProject(ApplicationDbContext context, int projectId, string name,string clientCompany,string contractorCompany,
        DateTime startD, DateTime endD, int priority, string projectManager)
        {
            var project = context.Projects.Find(projectId);
            if (project != null)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    project.Name = name;
                }

                if (!string.IsNullOrEmpty(clientCompany))
                {
                    project.ClientCompany = clientCompany;
                }

                if (!string.IsNullOrEmpty(contractorCompany))
                {
                    project.ContractorCompany = contractorCompany;
                }
                if (endD > startD)
                {
                    project.StartDate = startD;
                    project.EndDate = endD;
                }
                else
                {
                    Console.WriteLine("Дата окончания должна быть больше даты начала.");
                    return;
                }
                project.Priority = priority;

                if (!string.IsNullOrEmpty(projectManager))
                {
                    project.ProjectManager = projectManager;
                }
                context.SaveChanges();
                Console.WriteLine("Проект успешно обновлен.");
            }
            else
            {
                Console.WriteLine($"Проект с ID {projectId} не найден.");
            }
        }

        public static void DeleteProject(ApplicationDbContext context, int projectId)
        {
            
            var project = context.Projects.Find(projectId);

            if (project != null)
            {
                context.Projects.Remove(project);
                context.SaveChanges();
                Console.WriteLine("Проект удален.");
            }
            else
            {
                Console.WriteLine("Проект не найден.");
            }
        }

        public static void FilterAndSortProjects(ApplicationDbContext context)
        {
            bool running = true;
            do
            {
                string[] choice = { "Отсортировать по приоритету", "Отсортировать по алфавиту", "Отсортировать по дате", "Выйти" };
                int selectedOption = 0;
                do
                {
                    Console.Clear();
                    Display.view(choice, selectedOption);
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (selectedOption > 0)
                                selectedOption--;
                            break;
                        case ConsoleKey.DownArrow:
                            if (selectedOption < choice.Length - 1)
                                selectedOption++;
                            break;
                        case ConsoleKey.Enter:
                            switch (selectedOption)
                            {
                                case 0:
                                    Console.Clear();
                                    var minPriority = context.Projects.Min(p => p.Priority);
                                    var maxPriority = context.Projects.Max(p => p.Priority);
                                    var projects = context.Projects
                                        .Where(p => p.Priority >= minPriority && p.Priority <= maxPriority)
                                        .OrderBy(p => p.Priority)
                                        .ToList();

                                    foreach (var project in projects)
                                    {
                                        Console.WriteLine($"Проект: {project.Name}, Приоритет: {project.Priority}");
                                    }
                                    Console.ReadKey();
                                    break;
                                case 1:
                                    Console.Clear();
                                    var minAlf = context.Projects.Min(p => p.Name);
                                    var maxAlf = context.Projects.Max(p => p.Name);
                                    var projectsAlf = context.Projects
                                    .OrderBy(p => p.Name)
                                    .ToList();
                                    foreach (var project in projectsAlf)
                                    {
                                        Console.WriteLine($"Проект: {project.Name}, Приоритет: {project.Priority}");
                                    }
                                    Console.ReadKey();
                                    break;
                                case 2:
                                    Console.Clear();
                                    var minDate = context.Projects.Min(p => p.StartDate);
                                    var maxDate = context.Projects.Max(p => p.StartDate);
                                    var projectsDate = context.Projects
                                    .OrderBy(p => p.StartDate)
                                    .ToList();
                                    foreach (var project in projectsDate)
                                    {
                                        Console.WriteLine($"Проект: {project.Name}, Приоритет: {project.Priority}");
                                    }
                                    Console.ReadKey();
                                    break;
                                case 3:
                                    running = false;
                                    break;

                            }
                            break;
                    }
                } while (running);
            } while (running);
        }


        public static void AddEmployee(ApplicationDbContext context, string firstName, string lastName,string middleName, string email)
        {
            if (string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(middleName) ||
                string.IsNullOrEmpty(email))
            {
                return;
            }
            var employee = new Employee()
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                Email = email
            };
            context.Employees.Add(employee);
            context.SaveChanges();
            Console.WriteLine("Сотрудник добавлен.");
        }

        public static void EditEmployee(ApplicationDbContext context, int id, string firstName, string lastName, string middleName, string email)
        {
            var employee = context.Employees.Find(id);

            if (employee != null)
            {
                if (!string.IsNullOrEmpty(firstName))
                    employee.FirstName = firstName;

                if (!string.IsNullOrEmpty(lastName))
                    employee.LastName = lastName;

                if (!string.IsNullOrEmpty(middleName))
                    employee.MiddleName = middleName;

                if (!string.IsNullOrEmpty(email))
                    employee.Email = email;

                context.SaveChanges();
                Console.WriteLine("Сотрудник обновлен.");
            }
            else
            {
                Console.WriteLine("Сотрудник не найден.");
            }
        }

        public static void DeleteEmployee(ApplicationDbContext context, int employeeId)
        {
            var employee = context.Employees.Find(employeeId);

            if (employee != null)
            {
                var tasksAsAuthor = context.Tasks.Where(t => t.AuthorId == employeeId).ToList();
                var tasksAsContractor = context.Tasks.Where(t => t.ContractorId == employeeId).ToList();

                foreach (var task in tasksAsAuthor)
                {
                    task.AuthorId = null;
                }

                foreach (var task in tasksAsContractor)
                {
                    task.ContractorId = null;
                }

                context.SaveChanges();

                context.Employees.Remove(employee);
                context.SaveChanges();

                Console.WriteLine("Сотрудник и его связи с задачами удалены.");
                Console.WriteLine("Нажмите любую клавишу, чтобы выйти...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Сотрудник не найден.");
                Console.WriteLine("Нажмите любую клавишу, чтобы выйти...");
                Console.ReadKey();
            }
        }

        public static void AddEmployeeToProject(ApplicationDbContext context, int projectId, int employeeId)
        {
            if (!context.Projects.Any() || !context.Employees.Any())
            {
                Console.WriteLine("Вы не можете добавить сотрудника в проект, так как не существует проектов или сотрудников.");
                Console.ReadKey();
                return;
            }
            while (true)
            {
                if (projectId <= 0 || !context.Projects.Any(p => p.Id == projectId))
                {
                    Console.WriteLine("Введите корректный ID проекта:");
                    if (int.TryParse(Console.ReadLine(), out projectId) && context.Projects.Any(p => p.Id == projectId))
                    {
                        break;
                    }
                    Console.WriteLine("Неверный ID проекта. Пожалуйста, попробуйте еще раз.");
                }
                else
                {
                    break;
                }
            }
            while (true)
            {
                if (employeeId <= 0 || !context.Employees.Any(e => e.Id == employeeId))
                {
                    Console.WriteLine("Введите корректный ID сотрудника:");
                    if (int.TryParse(Console.ReadLine(), out employeeId) && context.Employees.Any(e => e.Id == employeeId))
                    {
                        break;
                    }
                    Console.WriteLine("Неверный ID сотрудника. Пожалуйста, попробуйте еще раз.");
                }
                else
                {
                    break;
                }
            }
            if (context.ProjectEmployees.Any(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId))
            {
                Console.WriteLine("Сотрудник уже добавлен в этот проект.");
            }
            else
            {
                var projectEmployee = new ProjectEmployee { ProjectId = projectId, EmployeeId = employeeId };
                context.ProjectEmployees.Add(projectEmployee);
                context.SaveChanges();
                Console.WriteLine("Сотрудник добавлен в проект.");
            }

            Console.WriteLine("Нажмите любую клавишу, чтобы выйти...");
            Console.ReadKey();
        }

        public static void RemoveEmployeeFromProject(ApplicationDbContext context, int projectId, int employeeId)
        {
            var projectEmployee = context.ProjectEmployees.SingleOrDefault(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);
            if (projectEmployee != null)
            {
                context.ProjectEmployees.Remove(projectEmployee);
                context.SaveChanges();
                Console.WriteLine("Сотрудник удален из проекта.");
            }
            else
            {
                Console.WriteLine("Запись не найдена.");
            }
        }

        


        public static void AddTask(ApplicationDbContext context)
        {
            var task = new Task();
            var employees = context.Employees.ToList();
            while (string.IsNullOrEmpty(task.Name))
            {
                Console.Write("Введите название задачи: ");
                task.Name = Console.ReadLine();
                if (string.IsNullOrEmpty(task.Name))
                {
                    Console.WriteLine("Название задачи не может быть пустым. Попробуйте снова.");
                }
            }
            Console.WriteLine("Список доступных Авторов задач:");
            foreach (var employee in employees)
            {
                Console.WriteLine($"ID: {employee.Id}");
            }
            Console.Write("Введите ID автора задачи (сотрудника): ");
            if (int.TryParse(Console.ReadLine(), out int authorId))
            {
                var employee = context.Employees.Find(authorId);
                if (employee != null)
                {
                    task.AuthorId = authorId;
                    task.Author = employee;
                }
                else
                {
                    Console.WriteLine("Сотрудник с указанным ID не найден. Задача не создана.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод ID.");
                return;
            }
            Console.WriteLine("Список доступных сотрудников для выбора исполнителя задачи:");
            foreach (var employee in employees)
            {
                Console.WriteLine($"ID: {employee.Id}");
            }
            Console.Write("Введите ID исполнителя задачи (сотрудника): ");
            if (int.TryParse(Console.ReadLine(), out int contractorId))
            {
                var contractor = context.Employees.Find(contractorId);
                if (contractor != null)
                {
                    task.ContractorId = contractorId;
                    task.Contractor = contractor;
                }
                else
                {
                    Console.WriteLine("Исполнитель с указанным ID не найден.");
                    return;
                }
            }
            Console.WriteLine("Введите статус задачи (0 = ToDo, 1 = InProgress, 2 = Done): ");
            if (Enum.TryParse(Console.ReadLine(), out TaskStat status))
            {
                task.Status = status;
            }
            else
            {
                Console.WriteLine("Неверный ввод статуса.");
                return;
            }

            Console.Write("Введите комментарий к задаче: ");
            task.Comment = Console.ReadLine();

            Console.Write("Введите приоритет задачи (целое число): ");
            if (int.TryParse(Console.ReadLine(), out int priority))
            {
                task.Priority = priority;
            }
            else
            {
                Console.WriteLine("Неверный ввод приоритета.");
                return;
            }

            context.Tasks.Add(task);
            context.SaveChanges();
            Console.WriteLine("Задача добавлена.");
        }
        public static void EditTask(ApplicationDbContext context)
        {
            var tasks = context.Tasks.ToList();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Задач для редактирования нет...");
            }
            else
            {
                foreach (var t in tasks)
                {
                    Console.WriteLine($"Список задач для редактирования: {t.Id} , {t.Name}");
                    Console.Write("Введите ID задачи для редактирования: ");
                    int editTaskId = int.Parse(Console.ReadLine());
                    var task = context.Tasks.Find(editTaskId);
                    if (task == null)
                    {
                        Console.WriteLine("Задача не найдена.");
                        return;
                    }

                    Console.Write("Введите новое название задачи (или Enter для пропуска): ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newName))
                        task.Name = newName;

                    Console.WriteLine("Введите нового автора задачи (или Enter для пропуска):");
                    string newAuthorIdStr = Console.ReadLine();
                    if (int.TryParse(newAuthorIdStr, out int newAuthorId))
                    {
                        var newAuthor = context.Tasks.Find(newAuthorId);
                        if (newAuthor != null)
                        {
                            task.AuthorId = newAuthorId;
                        }
                        else
                        {
                            Console.WriteLine("Автор не найден, пропуск изменения автора.");
                        }
                    }
                    Console.Write("Введите новый статус задачи (0 = ToDo, 1 = InProgress, 2 = Done) или Enter для пропуска: ");
                    if (Enum.TryParse(Console.ReadLine(), out TaskStat newStatus))
                    {
                        task.Status = newStatus;
                    }

                    Console.Write("Введите новый комментарий (или Enter для пропуска): ");
                    string newComment = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newComment))
                        task.Comment = newComment;

                    Console.Write("Введите новый приоритет (или Enter для пропуска): ");
                    if (int.TryParse(Console.ReadLine(), out int newPriority))
                    {
                        task.Priority = newPriority;
                    }

                    context.SaveChanges();
                    Console.WriteLine("Задача обновлена.");
                }
            }
        }
        public static void DeleteTask(ApplicationDbContext context)
        {
            var tasks = context.Tasks.ToList();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Задач для удаления нет");
            }
            else
            {
                foreach (var t in tasks)
                {
                    Console.WriteLine($"Доступные задачи для удаления: {t.Id}");
                }
                Console.WriteLine("Введите ID задачи для удаления: ");
                int taskId = int.Parse(Console.ReadLine()); 
                var task = context.Tasks.Find(taskId);
                if (task != null)
                {
                    context.Tasks.Remove(task);
                    context.SaveChanges();
                    Console.WriteLine("Задача удалена.");
                }
                else
                {
                    Console.WriteLine("Задача не найдена.");
                }
            }    
        }
        public static void AddTaskToProject(ApplicationDbContext context)
        {
            var projects = context.Projects.ToList();
            foreach (var p in projects)
            {
                Console.WriteLine($"Id Проектов: {p.Id}");
            }

            var tasks = context.Tasks.ToList();
            foreach (var t in tasks)
            {
                Console.WriteLine($"Id задач: {t.Id}");
            }

            if (tasks.Count == 0 || projects.Count == 0)
            {
                Console.WriteLine("Задач или проектов не существует...");
            }
            else
            {
                Console.WriteLine("Введите ID проекта:");
                int projectId = int.Parse(Console.ReadLine());

                Console.WriteLine("Введите ID задачи:");
                int taskId = int.Parse(Console.ReadLine());

                var project = context.Projects.Include(p => p.Tasks).FirstOrDefault(p => p.Id == projectId);
                var task = context.Tasks.Find(taskId);

                if (project != null && task != null)
                {
                    project.Tasks.Add(task);
                    context.SaveChanges();
                    Console.WriteLine("Задача добавлена в проект.");
                }
                else
                {
                    Console.WriteLine("Проект или задача не найдены.");
                }
            }
        }
        public static void RemoveTaskFromProject(ApplicationDbContext context)
        {
            var projects = context.Projects.ToList();
            foreach (var p in projects)
            {
                Console.WriteLine($"Id Проектов {p.Id}");
            }
            var tasks = context.Tasks.ToList();
            foreach (var t in tasks)
            {
                Console.WriteLine($"Id задач для удаления: {t.Id}");
            }
            if( tasks.Count == 0 || projects.Count == 0)
            {
                Console.WriteLine("Задач или проектов не существует...");
            }
            else
            {
                Console.WriteLine("Введите ID проекта:");
                int projectId = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите ID задачи:");
                int taskId = int.Parse(Console.ReadLine());
                var project = context.Projects.Include(p => p.Tasks).FirstOrDefault(p => p.Id == projectId);
                var task = context.Tasks.Find(taskId);

                if (project != null && task != null && project.Tasks.Contains(task))
                {
                    project.Tasks.Remove(task);
                    context.SaveChanges();

                    Console.WriteLine("Задача удалена из проекта.");
                }
                else
                {
                    Console.WriteLine("Проект, задача не найдены или задача не принадлежит этому проекту.");
                }
            }
            
        }

        public static void FilterTasksByStatus(ApplicationDbContext context, TaskStat status)
        {
            var tasks = context.Tasks.Where(t => t.Status == status).ToList();

            Console.WriteLine($"Задачи со статусом {status}:");
            foreach (var task in tasks)
            {
                Console.WriteLine($"Задача: {task.Name}, Автор: {task.Author.FirstName} {task.Author.LastName}, Исполнитель: {task.Contractor.FirstName} {task.Contractor.LastName}, Статус: {task.Status}, Приоритет: {task.Priority}");
            }
        }
        public static void SortTasksByPriority(ApplicationDbContext context)
        {
            var tasks = context.Tasks.ToList();
            if(tasks.Count == 0)
            {
                Console.WriteLine("Сортировать нечего. Задач нет");
            }
            else
            {
                bool running = true;
                do
                {
                    string[] choice = { "Отсортировать по приоритету", "Отсортировать по статусу","Выйти" };
                    int selectedOption = 0;
                    do
                    {
                        Console.Clear();
                        Display.view(choice, selectedOption);
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.UpArrow:
                                if (selectedOption > 0)
                                    selectedOption--;
                                break;
                            case ConsoleKey.DownArrow:
                                if (selectedOption < choice.Length - 1)
                                    selectedOption++;
                                break;
                            case ConsoleKey.Enter:
                                switch (selectedOption)
                                {
                                    case 0:
                                        Console.Clear();
                                        var minPriority = context.Tasks.Min(p => p.Priority);
                                        var maxPriority = context.Tasks.Max(p => p.Priority);
                                        var tasksPriority = context.Tasks
                                            .Where(p => p.Priority >= minPriority && p.Priority <= maxPriority)
                                            .OrderBy(p => p.Priority)
                                            .ToList();

                                        foreach (var tP in tasksPriority)
                                        {
                                            Console.WriteLine($"Проект: {tP.Name}, Приоритет: {tP.Priority}");
                                        }
                                        Console.ReadKey();
                                        break;
                                    case 1:
                                        Console.Clear();
                                        var tasksStatuc = context.Tasks
                                        .OrderBy(t => t.Status)
                                        .ToList();
                                        foreach (var tS in tasksStatuc)
                                        {
                                            Console.WriteLine($"Проект: {tS.Name}, Статус: {tS.Status}");
                                        }
                                        Console.ReadKey();
                                        break;
                                    case 2:
                                        running = false;
                                        break;

                                }
                                break;
                        }
                    } while (running);
                } while (running);
            }
        }
        public static void ViewAllTasks(ApplicationDbContext context)
        {
            var tasks = context.Tasks
                .Include(t => t.Author)
                .Include(t => t.Contractor)
                .Include(t => t.Project)
                .ToList();

            if (tasks == null || tasks.Count == 0)
            {
                Console.WriteLine("Задачи отсутствуют.");
                return;
            }

            foreach (var task in tasks)
            {
                string projectName = task.Project != null ? task.Project.Name : "Проект не назначен";
                string authorName = task.Author != null ? $"{task.Author.FirstName} {task.Author.LastName}" : "Автор не назначен";
                string contractorName = task.Contractor != null ? $"{task.Contractor.FirstName} {task.Contractor.LastName}" : "Исполнитель не назначен";

                Console.WriteLine($"Задача: {task.Name} \nПроект: {projectName} \nАвтор: {authorName} " +
                    $"\nИсполнитель: {contractorName} \nСтатус: {task.Status} \n" +
                    $"Приоритет: {task.Priority} \nКомментарий: {task.Comment}");
                Console.WriteLine("-------------------------------------");
            }
        }

    }
}
