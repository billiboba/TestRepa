//using Lab3BD.View;
//using Lab3Logic.Data;
//using Lab3Logic.View;
//using Microsoft.EntityFrameworkCore;

//class Program
//{
//    static void Main(string[] args)
//    {
//        using (var context = new ApplicationDbContext())
//        {
//            var tableExists = context.Database.ExecuteSqlRaw(
//            "IF OBJECT_ID('dbo.Tasks', 'U') IS NOT NULL SELECT 1 ELSE SELECT 0") == 1;

//            if (!tableExists)
//            {
//                Console.WriteLine("Таблица 'Tasks' отсутствует. Создаю таблицу...");
//                context.Database.Migrate();
//            }
//            else
//            {
//                Console.WriteLine("Таблица 'Tasks' уже существует.");
//            }
//            Console.WriteLine("Таблица Task создана (если отсутствовала).");
//            string[] menuOptions = { "Проекты", "Сотрудники", "Задачи", "Выйти" };
//            int selectedOption = 0;
//            bool running = true;
//            do
//            {
//                Console.Clear();
//                Display.view(menuOptions, selectedOption);
//                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
//                switch (keyInfo.Key)
//                {
//                    case ConsoleKey.UpArrow:
//                        if (selectedOption > 0)
//                            selectedOption--;
//                        break;
//                    case ConsoleKey.DownArrow:
//                        if (selectedOption < menuOptions.Length - 1)
//                            selectedOption++;
//                        break;
//                    case ConsoleKey.Enter:
//                        switch (selectedOption)
//                        {
//                            case 0:
//                                Display.ViewProjects(context);
//                                Console.ReadKey();
//                                break;
//                            case 1:
//                                Display.ViewEmployees(context);
//                                Console.ReadKey();
//                                break;
//                            case 2:
//                                Display.ViewTasks(context);
//                                Console.ReadKey();
//                                break;
//                            case 3:
//                                running = false;
//                                break;
//                        }
//                        break;
//                }
//            } while (running);
//        }
//    }
//}