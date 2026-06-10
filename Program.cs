using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

UserProfile user;


// Load existing profile or create one
if (File.Exists("user.json"))
{
    string json = File.ReadAllText("user.json");
    user = JsonSerializer.Deserialize<UserProfile>(json)!;
}
else
{
    user = CreateProfile();
    SaveProfile(user);
}


// Main Dashboard Loop
while (true)
{
    DisplayDashboard(user);

    Thread.Sleep(1000);
}

static UserProfile CreateProfile()
{
    Console.Write("Enter your name: ");
    string name = Console.ReadLine() ?? "";

    Console.Write("Enter your gender: ");
    string gender = Console.ReadLine() ?? "";

    Console.Write("Enter your date of birth: (yyyy-MM-dd): ");
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

static bool IsBirthday(DateTime birthday)
{
    DateTime today = DateTime.Today;

    return birthday.Month == today.Month &&
           birthday.Day == today.Day;
}

static string GetGreeting()
{
    Directory<string, string> greetings = new()
    {
        { "Morning", "Good Morning" },
        { "Afternoon", "Good Afternoon" },
        { "Evening", "Good Evening" }
    };

    int hour = DateTime.Now.Hour;

    string period = hour switch
    {
        < 12 => "Morning",
        < 18 => "Afternoon",
        _ => "Evening"
    };

    return greetings[period];
}


