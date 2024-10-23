using Lab3Logic.Data;
using Lab3BD.View;
using Lab3Logic.StructDataBase;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lab3Logic.View
{
    public class Display
    {
        public static Employee GetEmployeeFromInput()
        {
            var employee = new Employee();
            while (string.IsNullOrEmpty(employee.FirstName))
            {
                Console.Write("Введите имя сотрудника: ");
                employee.FirstName = Console.ReadLine();
                if (string.IsNullOrEmpty(employee.FirstName))
                {
                    Console.WriteLine("Имя не может быть пустым.");
                }
            }
            while (string.IsNullOrEmpty(employee.LastName))
            {
                Console.Write("Введите фамилию сотрудника: ");
                employee.LastName = Console.ReadLine();
                if (string.IsNullOrEmpty(employee.LastName))
                {
                    Console.WriteLine("Фамилия не может быть пустой.");
                }
            }
            while (string.IsNullOrEmpty(employee.MiddleName))
            {
                Console.Write("Введите отчество сотрудника: ");
                employee.MiddleName = Console.ReadLine();
                if (string.IsNullOrEmpty(employee.MiddleName))
                {
                    Console.WriteLine("Отчество не может быть пустым.");
                }
            }
            while (string.IsNullOrEmpty(employee.Email))
            {
                Console.Write("Введите email сотрудника: ");
                employee.Email = Console.ReadLine();
                if (string.IsNullOrEmpty(employee.Email))
                {
                    Console.WriteLine("Email не может быть пустым.");
                }
            }
            return employee;
        }
        public static Project GetProjectFromInput()
        {
            var project = new Project();

            while (string.IsNullOrEmpty(project.Name))
            {
                Console.Write("Введите название проекта: ");
                project.Name = Console.ReadLine();
                if (string.IsNullOrEmpty(project.Name))
                {
                    Console.WriteLine("Название проекта не может быть пустым.");
                }
            }

            while (string.IsNullOrEmpty(project.ClientCompany))
            {
                Console.Write("Введите название компании-заказчика: ");
                project.ClientCompany = Console.ReadLine();
                if (string.IsNullOrEmpty(project.ClientCompany))
                {
                    Console.WriteLine("Название компании-заказчика не может быть пустым.");
                }
            }

            while (string.IsNullOrEmpty(project.ContractorCompany))
            {
                Console.Write("Введите название компании-исполнителя: ");
                project.ContractorCompany = Console.ReadLine();
                if (string.IsNullOrEmpty(project.ContractorCompany))
                {
                    Console.WriteLine("Название компании-исполнителя не может быть пустым.");
                }
            }

            DateTime startD;
            while (true)
            {
                Console.Write("Введите дату начала (в формате yyyy-MM-dd): ");
                string startDateInput = Console.ReadLine();
                if (DateTime.TryParseExact(startDateInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out startD))
                {
                    project.StartDate = startD;
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный формат даты начала. Пожалуйста, попробуйте снова.");
                }
            }

            DateTime endD;
            while (true)
            {
                Console.Write("Введите дату окончания (в формате yyyy-MM-dd): ");
                string endDateInput = Console.ReadLine();
                if (DateTime.TryParseExact(endDateInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out endD))
                {
                    if (endD > startD)
                    {
                        project.EndDate = endD;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Дата окончания должна быть больше даты начала.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный формат даты окончания. Пожалуйста, попробуйте снова.");
                }
            }

            int priority;
            while (true)
            {
                Console.Write("Введите приоритет проекта (целое число): ");
                string priorityInput = Console.ReadLine();
                if (int.TryParse(priorityInput, out priority))
                {
                    project.Priority = priority;
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный ввод приоритета. Пожалуйста, введите целое число.");
                }
            }

            while (string.IsNullOrEmpty(project.ProjectManager))
            {
                Console.Write("Введите имя руководителя проекта: ");
                project.ProjectManager = Console.ReadLine();
                if (string.IsNullOrEmpty(project.ProjectManager))
                {
                    Console.WriteLine("Имя руководителя проекта не может быть пустым.");
                }
            }

            return project;
        }

        public static void view(string[] menuOptions, int selectedOption)
        {
            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("> " + menuOptions[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("  " + menuOptions[i]);
                }
            }
        }
        public static void ViewProjects(ApplicationDbContext context)
        {
            Console.Clear();
            bool running = true;
            do
            {
                string[] choice = { "Добавить проект", "Удалить проект", "Редактировать проект", "Посмотреть проекты", "Отсортировать проекты", "Выйти" };
                int selectedOption = 0;
                do
                {
                    var projects = context.Projects.Include(p => p.ProjectEmployees).ThenInclude(pe => pe.Employee).ToList();
                    Console.Clear();
                    view(choice, selectedOption);

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
                                    var projectAdd = GetProjectFromInput();
                                    if (projectAdd != null)
                                    {
                                        Lab3BD.View.LogicDataBase.AddProject(context, projectAdd.Name, projectAdd.ClientCompany, projectAdd.ContractorCompany,
                                                                 projectAdd.StartDate, projectAdd.EndDate, projectAdd.Priority, projectAdd.ProjectManager);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Проект не был добавлен из-за ошибок ввода.");
                                    }
                                    Console.ReadKey();
                                    break;
                                case 1:
                                    Console.Clear();
                                    var projectsDelete = context.Projects.ToList();
                                    if (projectsDelete.Count == 0)
                                    {
                                        Console.WriteLine("Проектов для удаления нет.");
                                        Console.WriteLine("Нажмите любую клавишу, чтобы выйти...");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("ID проектов:");
                                        foreach (var prj in projectsDelete)
                                        {
                                            Console.WriteLine($"ID проекта: {prj.Id}");
                                        }
                                        Console.Write("Введите ID проекта для удаления: ");
                                        int projectIdDelete = int.Parse(Console.ReadLine());
                                        LogicDataBase.DeleteProject(context, projectIdDelete);
                                    }
                                    Console.ReadKey();
                                    break;
                                case 2:
                                    Console.Clear();
                                    var projectsEd = context.Projects.ToList();
                                    if (projects.Count == 0)
                                    {
                                        Console.WriteLine("Проектов для редактирования нет.");
                                        Console.WriteLine("Нажмите любую клавишу, чтобы выйти...");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("ID проектов:");
                                        foreach (var prj in projects)
                                        {
                                            Console.WriteLine($"ID проекта: {prj.Id}");
                                        }
                                        Console.WriteLine("Введите Id проекта для редактирования: ");
                                        int projectIdEdit = int.Parse(Console.ReadLine());
                                        var projectEdit = GetProjectFromInput();
                                        if (projectEdit != null)
                                        {
                                            LogicDataBase.EditProject(context, projectIdEdit, projectEdit.Name, projectEdit.ClientCompany, projectEdit.ContractorCompany,
                                                                projectEdit.StartDate, projectEdit.EndDate, projectEdit.Priority, projectEdit.ProjectManager);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Проект не был добавлен из-за ошибок ввода.");
                                        }
                                    }
                                    Console.ReadKey();
                                    break;
                                case 3:
                                    Console.Clear();
                                    if (projects.Count == 0)
                                    {
                                        Console.WriteLine("Проектов нет. Нажмите любую клавишу, чтобы выйти...");
                                        Console.ReadKey();
                                    }
                                    foreach (var project in projects)
                                    {
                                        Console.WriteLine($"Id проекта: {project.Id}\nПроект: {project.Name} \nЗаказчик: {project.ClientCompany}" +
                                            $" \nИсполнитель: {project.ContractorCompany} \nДата начала: {project.StartDate} " +
                                            $"\nДата окончания: {project.EndDate}");
                                        Console.WriteLine("Сотрудники проекта:");
                                        foreach (var pe in project.ProjectEmployees)
                                        {
                                            Console.WriteLine($"- {pe.Employee.FirstName} {pe.Employee.LastName}");
                                        }
                                        Console.WriteLine("-------------------------------------------------");
                                    }
                                    Console.ReadKey();
                                    break;
                                case 4:
                                    Console.Clear();
                                    LogicDataBase.FilterAndSortProjects(context);
                                    Console.ReadKey();
                                    break;
                                case 5:
                                    running = false;
                                    break;
                            }
                            break;
                    }
                } while (running);
            } while (running);


        }
        public static void ViewEmployees(ApplicationDbContext context)
        {
            Console.Clear();
            bool running = true;
            do
            {
                string[] choice = { "Добавить сотрудника","Добавить сотрудника в проект", "Удалить сотрудника",
                    "Удалить сотрудника из проекта", "Редактировать сотрудника",
                    "Посмотреть сотрудников", "Выйти" };
                int selectedOption = 0;
                do
                {
                    var employees = context.Employees.ToList();
                    Console.Clear();
                    view(choice, selectedOption);

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
                                    var employeeAdd = GetEmployeeFromInput();
                                    LogicDataBase.AddEmployee(context, employeeAdd.FirstName, employeeAdd.LastName, employeeAdd.MiddleName, employeeAdd.Email);
                                    Console.ReadKey();
                                    break;
                                case 1:
                                    Console.Clear();
                                    var projectsAddProj = context.Projects.ToList();
                                    var employeeAddProj = context.Employees.ToList();
                                    Console.WriteLine("ID проектов:");
                                    foreach (var prj in projectsAddProj)
                                    {
                                        Console.WriteLine($"ID проекта: {prj.Id}");
                                    }

                                    Console.WriteLine("ID сотрудников:");
                                    foreach (var emp in employeeAddProj)
                                    {
                                        Console.WriteLine($"ID сотрудника: {emp.Id}");
                                    }
                                    Console.WriteLine("Введите id проекта: ");
                                    int idPrj = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Введите id сотрудника: ");
                                    int idEmp = int.Parse(Console.ReadLine());
                                    LogicDataBase.AddEmployeeToProject(context, idPrj,idEmp);
                                    Console.ReadKey();
                                    break;
                                case 2:
                                    Console.Clear();
                                    Console.WriteLine("ID сотрудников: ");
                                    var employeeDelete = context.Employees.ToList();
                                    foreach (var emp in employees)
                                    {
                                        Console.WriteLine($"ID: {emp.Id}");
                                    }

                                    if (employees.Count == 0)
                                    {
                                        Console.WriteLine("Сотрудников нет");
                                        Console.WriteLine("Нажмите любую клавишу для выхода...");
                                        Console.ReadKey();
                                    }
                                    Console.WriteLine("Введите id сотрудника для удаления: ");
                                    int idDelete = int.Parse(Console.ReadLine());
                                    LogicDataBase.DeleteEmployee(context, idDelete);
                                    Console.ReadKey();
                                    break;
                                case 3:
                                    Console.Clear();
                                    var projectsDelProj = context.Projects.ToList();
                                    var employeeDelProj = context.Employees.ToList();
                                    Console.WriteLine("ID проектов:");
                                    foreach (var prj in projectsDelProj)
                                    {
                                        Console.WriteLine($"ID проекта: {prj.Id}");
                                    }

                                    Console.WriteLine("ID сотрудников:");
                                    foreach (var emp in employeeDelProj)
                                    {
                                        Console.WriteLine($"ID сотрудника: {emp.Id}");
                                    }
                                    Console.WriteLine("Введите id проекта: ");
                                    int idDelPrj = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Введите id сотрудника: ");
                                    int idDelEmp = int.Parse(Console.ReadLine());
                                    LogicDataBase.RemoveEmployeeFromProject(context,idDelPrj,idDelEmp);
                                    Console.ReadKey();
                                    break;
                                case 4:
                                    Console.Clear();
                                    var employee = context.Employees.ToList();
                                    Console.WriteLine("ID сотрудников для редактирования: ");
                                    foreach (var emp in employees)
                                    {
                                        Console.WriteLine($"ID сотрудника {emp.Id}");
                                    }
                                    if (employees.Count == 0)
                                    {
                                        Console.WriteLine("Сотрудников для изменения нет");
                                        Console.WriteLine("Нажмите любую клавишу для выхода");
                                        Console.ReadKey();
                                    }
                                    Console.WriteLine("Введите id сотрудика для редактирования: ");
                                    int id = int.Parse(Console.ReadLine());
                                    var employeeEdit = GetEmployeeFromInput();
                                    LogicDataBase.EditEmployee(context,id, employeeEdit.FirstName, employeeEdit.LastName, 
                                        employeeEdit.MiddleName, employeeEdit.Email);
                                    Console.ReadKey();
                                    break;
                                case 5:
                                    Console.Clear();
                                    if (employees.Count == 0)
                                    {
                                        Console.WriteLine("Сотрудников нет. Нажмите любую клавишу, чтобы выйти...");
                                        Console.ReadKey();
                                    }
                                    foreach (var employe in employees)
                                    {
                                        Console.WriteLine($"Id сотрудника: {employe.Id} \nИмя: {employe.FirstName} \nФамилия: {employe.LastName}" +
                                            $" \nОтчество: {employe.MiddleName} \nEmail: {employe.Email}");
                                        Console.WriteLine("-------------------------------------------------");
                                    }
                                    Console.ReadKey();
                                    break;
                                case 6:
                                    running = false;
                                    break;
                            }
                            break;
                    }
                } while (running);
            } while (running);

        }
        public static void ViewTasks(ApplicationDbContext context)
        {
            string[] taskMenuOptions = { "Добавить задачу", "Редактировать задачу", "Удалить задачу",
                "Добавить задачу в проект", "Фильтрация по статусу", "Сортировка по приоритету",
                "Просмотреть все задачи","Удалить задачу из проекта", "Назад" };
            int selectedTaskOption = 0;
            bool taskMenuRunning = true;

            do
            {
                Console.Clear();
                Display.view(taskMenuOptions, selectedTaskOption);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedTaskOption > 0)
                            selectedTaskOption--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedTaskOption < taskMenuOptions.Length - 1)
                            selectedTaskOption++;
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedTaskOption)
                        {
                            case 0:
                                LogicDataBase.AddTask(context);
                                Console.ReadKey();
                                break;
                            case 1:
                                LogicDataBase.EditTask(context);
                                Console.ReadKey();
                                break;
                            case 2:
                                LogicDataBase.DeleteTask(context);
                                Console.ReadKey();
                                break;
                            case 3:
                                LogicDataBase.AddTaskToProject(context);
                                Console.ReadKey();
                                break;
                            case 4:
                                Console.WriteLine("Выберите статус: 0 = ToDo, 1 = InProgress, 2 = Done");
                                if (Enum.TryParse(Console.ReadLine(), out TaskStat status))
                                {
                                    LogicDataBase.FilterTasksByStatus(context, status);
                                }
                                else
                                {
                                    Console.WriteLine("Неверный статус.");
                                }
                                Console.ReadKey();
                                break;
                            case 5:
                                LogicDataBase.SortTasksByPriority(context);
                                Console.ReadKey();
                                break;
                            case 6:
                                LogicDataBase.ViewAllTasks(context);
                                Console.ReadKey();
                                break;
                            case 7:
                                LogicDataBase.RemoveTaskFromProject(context);
                                Console.ReadKey();
                                break;
                            case 8:
                                taskMenuRunning = false;
                                break;
                        }
                        break;
                }
            } while (taskMenuRunning);

        }
    }
}