using TurboMathRally.Utils;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Handles menu navigation and user interface
    /// </summary>
    public class MenuSystem
    {
        /// <summary>
        /// Display the main menu and get user selection
        /// </summary>
        public GameState DisplayMainMenu()
        {
            ConsoleHelper.DisplayHeader("TURBO MATH RALLY - MAIN MENU");
            
            ConsoleHelper.DisplayMenuOption(1, "🏁 Start Racing");
            ConsoleHelper.DisplayMenuOption(2, "⚙️  Settings");
            ConsoleHelper.DisplayMenuOption(3, "📊 Parent Dashboard");
            ConsoleHelper.DisplayMenuOption(4, "ℹ️  About");
            ConsoleHelper.DisplayMenuOption(5, "🚪 Exit");
            
            Console.WriteLine();
            string input = ConsoleHelper.GetUserInput("Select an option (1-5)");
            
            return input switch
            {
                "1" => GameState.ModeSelection,
                "2" => DisplaySettingsMenu(),
                "3" => GameState.ParentDashboard,
                "4" => DisplayAbout(),
                "5" => GameState.Exit,
                _ => HandleInvalidInput("Invalid selection. Please choose 1-5.")
            };
        }
        
        /// <summary>
        /// Display mode selection menu (Parent/Kid)
        /// </summary>
        public GameState DisplayModeSelection()
        {
            ConsoleHelper.DisplayHeader("SELECT PLAYER MODE");
            
            ConsoleHelper.DisplayMenuOption(1, "🧒 Kid Mode (Fun interface with encouragement)");
            ConsoleHelper.DisplayMenuOption(2, "👨‍👩‍👧‍👦 Parent Mode (Analytics and detailed progress)");
            ConsoleHelper.DisplayMenuOption(3, "🔙 Back to Main Menu");
            
            Console.WriteLine();
            string input = ConsoleHelper.GetUserInput("Select mode (1-3)");
            
            return input switch
            {
                "1" => GameState.MathSelection, // Kid mode -> Math selection
                "2" => GameState.ParentDashboard, // Parent mode -> Dashboard
                "3" => GameState.Menu,
                _ => HandleInvalidInput("Invalid selection. Please choose 1-3.")
            };
        }
        
        /// <summary>
        /// Display math type selection
        /// </summary>
        public GameState DisplayMathSelection()
        {
            ConsoleHelper.DisplayHeader("CHOOSE YOUR MATH CHALLENGE");
            
            ConsoleHelper.DisplayMenuOption(1, "➕ Addition Only");
            ConsoleHelper.DisplayMenuOption(2, "➖ Subtraction Only");
            ConsoleHelper.DisplayMenuOption(3, "✖️  Multiplication Only");
            ConsoleHelper.DisplayMenuOption(4, "➗ Division Only");
            ConsoleHelper.DisplayMenuOption(5, "🎲 Mixed Problems (All operations)");
            ConsoleHelper.DisplayMenuOption(6, "🔙 Back");
            
            Console.WriteLine();
            string input = ConsoleHelper.GetUserInput("Select math type (1-6)");
            
            return input switch
            {
                "1" or "2" or "3" or "4" or "5" => GameState.SeriesSelection,
                "6" => GameState.ModeSelection,
                _ => HandleInvalidInput("Invalid selection. Please choose 1-6.")
            };
        }
        
        /// <summary>
        /// Display rally series selection
        /// </summary>
        public GameState DisplaySeriesSelection()
        {
            ConsoleHelper.DisplayHeader("SELECT RALLY SERIES");
            
            ConsoleHelper.DisplayMenuOption(1, "🌲 Rookie Rally (Ages 5-7) - Forest, Park, Beach");
            ConsoleHelper.DisplayMenuOption(2, "🏔️  Junior Championship (Ages 7-9) - Mountain, Desert, City, Snow");
            ConsoleHelper.DisplayMenuOption(3, "🏆 Pro Circuit (Ages 9-12) - Extreme challenges!");
            ConsoleHelper.DisplayMenuOption(4, "🔙 Back");
            
            Console.WriteLine();
            string input = ConsoleHelper.GetUserInput("Select series (1-4)");
            
            return input switch
            {
                "1" or "2" or "3" => GameState.Playing,
                "4" => GameState.MathSelection,
                _ => HandleInvalidInput("Invalid selection. Please choose 1-4.")
            };
        }
        
        /// <summary>
        /// Display settings menu
        /// </summary>
        private GameState DisplaySettingsMenu()
        {
            ConsoleHelper.DisplayHeader("SETTINGS");
            
            Console.WriteLine("⚙️  Settings coming in future update!");
            Console.WriteLine();
            ConsoleHelper.DisplayMenuOption(1, "🔙 Back to Main Menu");
            
            Console.WriteLine();
            ConsoleHelper.GetUserInput("Press Enter to continue");
            
            return GameState.Menu;
        }
        
        /// <summary>
        /// Display about information
        /// </summary>
        private GameState DisplayAbout()
        {
            ConsoleHelper.DisplayHeader("ABOUT TURBO MATH RALLY");
            
            Console.WriteLine("🏎️ Turbo Math Rally v0.1.0-alpha");
            Console.WriteLine();
            Console.WriteLine("A rally racing math game designed for ages 5-12.");
            Console.WriteLine("Solve math problems to advance through exciting rally stages!");
            Console.WriteLine();
            Console.WriteLine("Features:");
            Console.WriteLine("• Addition, Subtraction, Multiplication, Division");
            Console.WriteLine("• Age-appropriate difficulty scaling");
            Console.WriteLine("• Rally racing theme with car breakdowns");
            Console.WriteLine("• Parent and Kid modes");
            Console.WriteLine("• Progress tracking and analytics");
            Console.WriteLine();
            Console.WriteLine("Created by George Aaron Kibbie © 2025");
            
            ConsoleHelper.WaitForKeyPress();
            return GameState.Menu;
        }
        
        /// <summary>
        /// Handle invalid input and return to current menu
        /// </summary>
        private GameState HandleInvalidInput(string message)
        {
            Console.WriteLine();
            ConsoleHelper.DisplayError(message);
            ConsoleHelper.WaitForKeyPress();
            return GameState.Menu; // Return to main menu on invalid input
        }
    }
}
