using System;

namespace TurboMathRally.Utils
{
    /// <summary>
    /// Utility class for console display management and formatting
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Clear the console and reset cursor position
        /// </summary>
        public static void ClearScreen()
        {
            Console.Clear();
        }
        
        /// <summary>
        /// Write a line with specified color
        /// </summary>
        public static void WriteLineColored(string text, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = originalColor;
        }
        
        /// <summary>
        /// Write text with specified color (no newline)
        /// </summary>
        public static void WriteColored(string text, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = originalColor;
        }
        
        /// <summary>
        /// Display a header with decorative border
        /// </summary>
        public static void DisplayHeader(string title)
        {
            ClearScreen();
            WriteLineColored("=".PadRight(50, '='), ConsoleColor.Cyan);
            WriteLineColored($"🏎️  {title.ToUpper()}", ConsoleColor.Yellow);
            WriteLineColored("=".PadRight(50, '='), ConsoleColor.Cyan);
            Console.WriteLine();
        }
        
        /// <summary>
        /// Display a menu option with number
        /// </summary>
        public static void DisplayMenuOption(int number, string option)
        {
            WriteColored($"  {number}. ", ConsoleColor.Green);
            Console.WriteLine(option);
        }
        
        /// <summary>
        /// Get user input with prompt
        /// </summary>
        public static string GetUserInput(string prompt)
        {
            WriteColored($"{prompt}: ", ConsoleColor.White);
            return Console.ReadLine() ?? string.Empty;
        }
        
        /// <summary>
        /// Wait for user to press Enter
        /// </summary>
        public static void WaitForKeyPress(string message = "Press Enter to continue...")
        {
            Console.WriteLine();
            WriteLineColored(message, ConsoleColor.Gray);
            Console.ReadLine(); // Changed from ReadKey to ReadLine for better compatibility
        }
        
        /// <summary>
        /// Display an error message
        /// </summary>
        public static void DisplayError(string message)
        {
            WriteLineColored($"❌ Error: {message}", ConsoleColor.Red);
        }
        
        /// <summary>
        /// Display a success message
        /// </summary>
        public static void DisplaySuccess(string message)
        {
            WriteLineColored($"✅ {message}", ConsoleColor.Green);
        }
        
        /// <summary>
        /// Display a warning message
        /// </summary>
        public static void DisplayWarning(string message)
        {
            WriteLineColored($"⚠️  {message}", ConsoleColor.Yellow);
        }
    }
}
