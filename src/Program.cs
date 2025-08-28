using System.Reflection;
using TurboMathRally.Core;

// Turbo Math Rally - A math learning game with rally racing theme
// Version: 0.1.0-alpha

try
{
    // Display version info
    Console.WriteLine("🏎️ Turbo Math Rally 🏎️");
    Console.WriteLine($"Version: {GetVersion()}");
    Console.WriteLine("A rally racing math game for ages 5-12");
    Console.WriteLine();
    
    // Start the game
    var game = new Game();
    game.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Fatal error: {ex.Message}");
    Console.WriteLine("Press Enter to exit...");
    Console.ReadLine(); // Changed from ReadKey for better compatibility
}

static string GetVersion()
{
    var version = Assembly.GetExecutingAssembly().GetName().Version;
    var informationalVersion = Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    
    return informationalVersion ?? version?.ToString() ?? "Unknown";
}
