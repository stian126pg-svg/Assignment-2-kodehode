using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

class Program
{
    private static readonly Dictionary<string, string> Greetings = new()
    {
        { "Morning", "Good Morning" },
        { "Afternoon", "Good Afternoon" },
        { "Evening", "Good Evening" }
    };

    static void Main()
    {
        UserProfile user = LoadOrCreateProfile();

        Console.CursorVisible = false;

        while (true)
        {
            DisplayDashboard(user);
            Thread.Sleep(1000);
        }
    }

    static UserProfile LoadOrCreateProfile()
    {
        const string fileName = "user.json";

        if (File.Exists(fileName))
        {
            string json = File.ReadAllText(fileName);

            UserProfile? loadedUser =
                JsonSerializer.Deserialize<UserProfile>(json);

            if (loadedUser != null)
                return loadedUser;
        }

        UserProfile newUser = CreateProfile();
        SaveProfile(newUser);

        return newUser;
    }

    static UserProfile CreateProfile()
    {
        Console.Clear();

        Console.Write("Enter your name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter your gender: ");
        string gender = Console.ReadLine() ?? "";

        Console.Write("Enter your date of birth (yyyy-MM-dd): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "");

        return new UserProfile
        {
            Name = name,
            Gender = gender,
            DateOfBirth = dob
        };
    }

    static void SaveProfile(UserProfile user)
    {
        string json = JsonSerializer.Serialize(
            user,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText("user.json", json);
    }

    static string GetGreeting()
    {
        int hour = DateTime.Now.Hour;

        string period = hour switch
        {
            < 12 => "Morning",
            < 18 => "Afternoon",
            _ => "Evening"
        };

        return Greetings[period];
    }

    static bool IsBirthday(DateTime birthday)
    {
        DateTime today = DateTime.Today;

        return birthday.Month == today.Month &&
               birthday.Day == today.Day;
    }

    static void DisplayDashboard(UserProfile user)
    {
        DateTime now = DateTime.Now;

        Console.SetCursorPosition(0, 0);

        Console.WriteLine("====================================================");
        Console.WriteLine($"{GetGreeting()}, {user.Name}!");

        if (IsBirthday(user.DateOfBirth))
        {
            Console.WriteLine("🎉 HAPPY BIRTHDAY! 🎂");
        }
        else
        {
            Console.WriteLine();
        }

        Console.WriteLine("====================================================");
        
        Console.WriteLine(
            $"{now:dddd} | Week {ISOWeek.GetWeekOfYear(now)} | {now:MMMM dd yyyy} | {now:HH:mm:ss}");

        Console.WriteLine(
            $"Timezone: {TimeZoneInfo.Local.DisplayName}");

        Console.WriteLine();

        Console.WriteLine("Profile");
        Console.WriteLine("--------------------------------------------");
        Console.WriteLine($"Name   : {user.Name}");
        Console.WriteLine($"Gender : {user.Gender}");
        Console.WriteLine($"DOB    : {user.DateOfBirth:dd MMMM yyyy}");

        Console.WriteLine();

        Console.WriteLine("Tasks");
        Console.WriteLine("--------------------------------------------");
        Console.WriteLine("• Meeting at 14:00");
        Console.WriteLine("• Buy groceries");
        Console.WriteLine("• Pay electricity bill");

        Console.WriteLine();
        Console.WriteLine("Press CTRL+C to exit.");
    }
}