using System.Text.Json;



class Program
{
    static void Main()
    {
        Console.CursorVisible = false;

        UserProfile user = LoadProfile();
        List<TaskItem> tasks = LoadTasks();

        Console.Clear();
        DisplayDashboard(user, tasks);

        while (true)
        {
            Console.SetCursorPosition(0, 6);

            Console.Write(
                $"{DateTime.Now:HH:mm:ss}");

            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.A:
                        AddTask(tasks);
                        Console.Clear();
                        break;

                    case ConsoleKey.R:
                        RemoveTask(tasks);
                        Console.Clear();
                        break;

                    case ConsoleKey.C:
                        CompleteTask(tasks);
                        Console.Clear();
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }

            Thread.Sleep(1000);
        }
    }


// Load User Profile
    static UserProfile LoadProfile()
    {
        if (File.Exists("user.json"))
        {
            string json = File.ReadAllText("user.json");

            UserProfile? user =
                JsonSerializer.Deserialize<UserProfile>(json);

            if (user != null)
                return user;
        }

        Console.Clear();

        Console.Write("Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Gender: ");
        string gender = Console.ReadLine() ?? "";

        Console.Write("DOB (yyyy-MM-dd): ");

        DateTime dob;

        while (!DateTime.TryParse(Console.ReadLine(), out dob))
        {
            Console.Write("Invalid date. Try again: ");
        }

        UserProfile newUser = new UserProfile
        {
            Name = name,
            Gender = gender,
            DateOfBirth = dob
        };

        string profileJson =
            JsonSerializer.Serialize(
                newUser,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText("user.json", profileJson);

        return newUser;
    }


// Load Tasks
    static List<TaskItem> LoadTasks()
    {
        if (!File.Exists("tasks.json"))
            return new List<TaskItem>();

        string json = File.ReadAllText("tasks.json");

        return JsonSerializer.Deserialize<List<TaskItem>>(json)
               ?? new List<TaskItem>();
    }


// Save Tasks
    static void SaveTasks(List<TaskItem> tasks)
    {
        string json =
            JsonSerializer.Serialize(
                tasks,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText("tasks.json", json);
    }


// Add Tasks
    static void AddTask(List<TaskItem> tasks)
    {
        Console.Clear();
        Console.CursorVisible = true;

        Console.Write("Task Title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Due Date (yyyy-MM-dd HH:mm): ");

        DateTime dueDate;

        while (!DateTime.TryParse(Console.ReadLine(), out dueDate))
        {
            Console.Write("Invalid date. Try again: ");
        }

        tasks.Add(new TaskItem
        {
            Title = title,
            DueDate = dueDate,
            Completed = false
        });

        SaveTasks(tasks);

        Console.CursorVisible = false;
    }


// Remove Tasks
    static void RemoveTask(List<TaskItem> tasks)
    {
        if (tasks.Count == 0)
            return;

        Console.Clear();
        Console.CursorVisible = true;

        Console.WriteLine("Tasks:");

        for (int i = 0; i < tasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tasks[i].Title}");
        }

        Console.Write("\nRemove task #: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            if (choice >= 1 && choice <= tasks.Count)
            {
                tasks.RemoveAt(choice - 1);
                SaveTasks(tasks);
            }
        }

        Console.CursorVisible = false;
    }


// Complete Tasks
    static void CompleteTask(List<TaskItem> tasks)
    {
        if (tasks.Count == 0)
            return;

        Console.Clear();
        Console.CursorVisible = true;

        Console.WriteLine("Tasks:");

        for (int i = 0; i < tasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tasks[i].Title}");
        }

        Console.Write("\nComplete task #: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            if (choice >= 1 && choice <= tasks.Count)
            {
                tasks[choice - 1].Completed = true;
                SaveTasks(tasks);
            }
        }

        Console.CursorVisible = false;
    }


// A little happy surprise!
    static bool IsBirthday(DateTime dob)
    {
        DateTime today = DateTime.Today;

        return today.Month == dob.Month &&
               today.Day == dob.Day;
    }


// Get age
    static int GetAge(DateTime dob)
    {
        int age = DateTime.Today.Year - dob.Year;

        if (dob.Date > DateTime.Today.AddYears(-age))
        {
            age--;
        }

        return age;
    }


// Greeting functions
    static string GetGreeting()
    {
        int hour = DateTime.Now.Hour;

        if (hour < 12)
            return "Good Morning";

        if (hour < 18)
            return "Good Afternoon";

        return "Good Evening";
    }


// Display Dashboard Loop
    static void DisplayDashboard(
        UserProfile user,
        List<TaskItem> tasks)
    {
        DateTime now = DateTime.Now;

        Console.WriteLine("====================================================");
        Console.WriteLine($"{GetGreeting()}, {user.Name}");

        if (IsBirthday(user.DateOfBirth))
        {
            Console.WriteLine("🎉 HAPPY BIRTHDAY! 🎂");
        }
        else
        {
            Console.WriteLine();
        }

        Console.WriteLine("====================================================");

        Console.WriteLine($"{now:dddd}");
        Console.WriteLine($"{now:dd MMMM yyyy}");
        Console.WriteLine($"{now:HH:mm:ss}");

        Console.WriteLine(
            $"Timezone: {TimeZoneInfo.Local.DisplayName}");

        Console.WriteLine();

        Console.WriteLine("PROFILE");
        Console.WriteLine("----------------------------------------");

        Console.WriteLine($"Name   : {user.Name}");
        Console.WriteLine($"Gender : {user.Gender}");
        Console.WriteLine($"DOB    : {user.DateOfBirth:dd MMMM yyyy}");
        Console.WriteLine($"Age    : {GetAge(user.DateOfBirth)}");

        Console.WriteLine();

        Console.WriteLine("TASKS");
        Console.WriteLine("----------------------------------------");

        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks.");
        }
        else
        {
            foreach (TaskItem task in tasks.OrderBy(t => t.DueDate))
            {
                string status =
                    task.Completed
                    ? "[DONE]"
                    : task.IsOverdue
                        ? "[OVERDUE]"
                        : task.IsDueSoon
                            ? "[SOON]"
                            : "[ACTIVE]";

                Console.WriteLine(
                    $"{status} {task.Title}");

                Console.WriteLine(
                    $"       Due: {task.DueDate:g}");
            }
        }

        Console.WriteLine();

        TaskItem? overdue =
            tasks.FirstOrDefault(t => t.IsOverdue);

        TaskItem? dueSoon =
            tasks.FirstOrDefault(t => t.IsDueSoon);

        if (overdue != null)
        {
            Console.WriteLine(
                $"⚠ OVERDUE: {overdue.Title}");
        }
        else if (dueSoon != null)
        {
            TimeSpan remaining =
                dueSoon.DueDate - DateTime.Now;

            Console.WriteLine(
                $"🔔 Due in {(int)remaining.TotalMinutes} minutes: {dueSoon.Title}");
        }

        Console.WriteLine();
        Console.WriteLine("A = Add Task");
        Console.WriteLine("R = Remove Task");
        Console.WriteLine("C = Complete Task");
        Console.WriteLine("ESC = Exit");
    }
}