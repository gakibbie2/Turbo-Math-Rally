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
        
        /// <summary>
        /// Display a rally-themed progress bar
        /// </summary>
        public static void DisplayRaceProgressBar(int currentQuestion, int totalQuestions, string stageName = "Rally Stage")
        {
            double progressPercent = (double)currentQuestion / totalQuestions;
            int barWidth = 40; // Width of the progress bar
            int filledWidth = (int)(progressPercent * barWidth);
            
            // Ensure we don't exceed the bar width and handle edge cases
            filledWidth = System.Math.Min(filledWidth, barWidth);
            int emptyWidth = System.Math.Max(0, barWidth - filledWidth - 1); // Reserve 1 space for car emoji
            
            // Create the progress bar visual
            string progressBar;
            if (progressPercent >= 1.0)
            {
                // At 100%, show car at the finish line
                progressBar = "🏁" + new string('━', barWidth - 1) + "🚗🏆";
            }
            else
            {
                // Normal progress with car in the middle
                progressBar = "🏁" + new string('━', filledWidth) + "🚗" + new string('░', emptyWidth) + "🏆";
            }
            
            // Display the progress information
            WriteColored($"📊 {stageName} Progress: ", ConsoleColor.Cyan);
            WriteColored($"{currentQuestion}/{totalQuestions}", ConsoleColor.White);
            WriteColored($" ({progressPercent * 100:F0}%)", ConsoleColor.Yellow);
            Console.WriteLine();
            
            // Display the visual progress bar
            WriteColored(progressBar, ConsoleColor.Green);
            Console.WriteLine();
            
            // Add milestone indicators
            if (progressPercent >= 0.25 && progressPercent < 0.5)
                WriteLineColored("🚩 Quarter checkpoint passed!", ConsoleColor.Yellow);
            else if (progressPercent >= 0.5 && progressPercent < 0.75)
                WriteLineColored("🚩 Halfway checkpoint - Great pace!", ConsoleColor.Yellow);
            else if (progressPercent >= 0.75 && progressPercent < 1.0)
                WriteLineColored("🚩 Final quarter - Push to the finish!", ConsoleColor.Yellow);
            else if (progressPercent >= 1.0)
                WriteLineColored("🏆 FINISH LINE REACHED! 🏆", ConsoleColor.Green);
        }
        
        /// <summary>
        /// Display race statistics summary
        /// </summary>
        public static void DisplayRaceStats(int questionsAnswered, double accuracy, int totalTime = 0)
        {
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            WriteLineColored("🏁 RACE STATISTICS", ConsoleColor.Cyan);
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            
            WriteColored("📊 Questions Completed: ", ConsoleColor.White);
            WriteLineColored(questionsAnswered.ToString(), ConsoleColor.Yellow);
            
            WriteColored("🎯 Accuracy Rate: ", ConsoleColor.White);
            ConsoleColor accuracyColor = accuracy >= 80 ? ConsoleColor.Green : 
                                       accuracy >= 60 ? ConsoleColor.Yellow : ConsoleColor.Red;
            WriteLineColored($"{accuracy:F1}%", accuracyColor);
            
            if (totalTime > 0)
            {
                WriteColored("⏱️ Total Time: ", ConsoleColor.White);
                WriteLineColored($"{totalTime} seconds", ConsoleColor.Yellow);
            }
            
            // Performance rating
            string rating = accuracy >= 90 ? "🏆 CHAMPION RACER!" :
                           accuracy >= 80 ? "🥇 EXPERT DRIVER!" :
                           accuracy >= 70 ? "🥈 SKILLED RACER!" :
                           accuracy >= 60 ? "🥉 IMPROVING DRIVER!" :
                           "🚗 ROOKIE RACER - Keep practicing!";
            
            Console.WriteLine();
            WriteLineColored(rating, ConsoleColor.Green);
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }
    }
}
