using System.Reflection;

// Turbo Math Rally - A math learning game with rally racing theme
// Version: 0.1.0-alpha

Console.WriteLine("🏎️ Turbo Math Rally 🏎️");
Console.WriteLine($"Version: {GetVersion()}");
Console.WriteLine("A rally racing math game for ages 5-12");
Console.WriteLine();
Console.WriteLine("Hello, World! (Basic setup complete)");

static string GetVersion()
{
    var version = Assembly.GetExecutingAssembly().GetName().Version;
    var informationalVersion = Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    
    return informationalVersion ?? version?.ToString() ?? "Unknown";
}
