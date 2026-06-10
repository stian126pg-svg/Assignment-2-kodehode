// Date/Time Console / Simple Tasklist/Appointment booker


// User Profile Class
public class UserProfile
{
    public string Name { get; set; } = "";
    public string Gender { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
}

// Greeting Lookup Table
Dictionary<string, string> greetings = new()
{
    { "Morning", "Good Morning" },
    { "Afternoon", "Good Afternoon"},
    { "Evening", "Good Evening" }
};

string GetTimePeriod ()
{
    int hour = DateTime.Now.Hour;

    if (hour < 12)
        return "Morning";

    if (hour < 18)
        return "Afternoon";

    return "Evening";
}

string period = GetTimePeriod();

Console.WriteLine($"{greetings[period]}, {user.Name}!");