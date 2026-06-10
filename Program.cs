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