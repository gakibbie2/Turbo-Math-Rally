using TurboMathRally.Utils;
using TurboMathRally.Math;

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
            return DisplayMathSelection(null);
        }
        
        /// <summary>
        /// Display math type selection and update game configuration
        /// </summary>
        public GameState DisplayMathSelection(GameConfiguration? gameConfig)
        {
            ConsoleHelper.DisplayHeader("CHOOSE YOUR MATH CHALLENGE");
            
            Console.WriteLine("🎯 Select the type of math problems you want to practice:");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(1, "➕ Addition Only - Perfect for beginners (3 + 5 = ?)");
            Console.WriteLine("    👶 Best for ages 5-7 | 🎯 Focus: Number sense & counting");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(2, "➖ Subtraction Only - Building on addition skills (8 - 3 = ?)");
            Console.WriteLine("    🧒 Best for ages 6-8 | 🎯 Focus: Reverse thinking & logic");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(3, "✖️  Multiplication Only - Times tables mastery (4 × 6 = ?)");
            Console.WriteLine("    👦 Best for ages 7-10 | 🎯 Focus: Pattern recognition & memory");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(4, "➗ Division Only - Advanced problem solving (24 ÷ 6 = ?)");
            Console.WriteLine("    👧 Best for ages 8-12 | 🎯 Focus: Logical reasoning & facts");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(5, "🎲 Mixed Problems - All operations for variety");
            Console.WriteLine("    🏆 Best for ages 9+ | 🎯 Focus: Comprehensive math skills");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(6, "🔙 Back");
            
            Console.WriteLine();
            string input = ConsoleHelper.GetUserInput("Select math type (1-6)");
            
            // Update game configuration if provided
            if (gameConfig != null)
            {
                switch (input)
                {
                    case "1":
                        gameConfig.SelectedMathType = MathOperation.Addition;
                        gameConfig.SelectedMathTypeName = "Addition Only";
                        gameConfig.IsMixedMode = false;
                        break;
                    case "2":
                        gameConfig.SelectedMathType = MathOperation.Subtraction;
                        gameConfig.SelectedMathTypeName = "Subtraction Only";
                        gameConfig.IsMixedMode = false;
                        break;
                    case "3":
                        gameConfig.SelectedMathType = MathOperation.Multiplication;
                        gameConfig.SelectedMathTypeName = "Multiplication Only";
                        gameConfig.IsMixedMode = false;
                        break;
                    case "4":
                        gameConfig.SelectedMathType = MathOperation.Division;
                        gameConfig.SelectedMathTypeName = "Division Only";
                        gameConfig.IsMixedMode = false;
                        break;
                    case "5":
                        gameConfig.SelectedMathType = MathOperation.Addition; // Default for mixed mode
                        gameConfig.SelectedMathTypeName = "Mixed Problems";
                        gameConfig.IsMixedMode = true;
                        break;
                }
            }
            
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
            return DisplaySeriesSelection(null);
        }
        
        /// <summary>
        /// Display rally series selection and update game configuration
        /// </summary>
        public GameState DisplaySeriesSelection(GameConfiguration? gameConfig)
        {
            ConsoleHelper.DisplayHeader("SELECT RALLY SERIES");
            
            Console.WriteLine("🏁 Choose your difficulty level - each series has different challenges:");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(1, "🌲 Rookie Rally (Ages 5-7)");
            Console.WriteLine("    📊 25 questions | 🔢 Small numbers (1-10) | ⏱️ No time pressure");
            Console.WriteLine("    🌳 Tracks: Forest Trail, Sunny Park, Sandy Beach");
            Console.WriteLine("    💡 Perfect for: First-time racers, building confidence");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(2, "🏔️  Junior Championship (Ages 7-9)");
            Console.WriteLine("    📊 35 questions | 🔢 Medium numbers (1-50) | ⏱️ Moderate pace");
            Console.WriteLine("    🏔️ Tracks: Mountain Pass, Desert Dunes, City Streets, Snow Rally");
            Console.WriteLine("    💡 Perfect for: Developing skills, consistent practice");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(3, "🏆 Pro Circuit (Ages 9-12)");
            Console.WriteLine("    📊 50 questions | 🔢 Challenging numbers (1-100) | ⏱️ Racing pace");
            Console.WriteLine("    🏆 Tracks: Extreme terrain, advanced rally courses");
            Console.WriteLine("    💡 Perfect for: Math champions, advanced learners");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(4, "🔙 Back");
            
            Console.WriteLine();
            Console.WriteLine("💡 Pro tip: Start with Rookie Rally if you're new to math racing!");
            Console.WriteLine();
            
            string input = ConsoleHelper.GetUserInput("Select series (1-4)");
            
            // Update game configuration if provided
            if (gameConfig != null)
            {
                switch (input)
                {
                    case "1":
                        gameConfig.SelectedDifficulty = DifficultyLevel.Rookie;
                        gameConfig.SelectedSeriesName = "Rookie Rally";
                        break;
                    case "2":
                        gameConfig.SelectedDifficulty = DifficultyLevel.Junior;
                        gameConfig.SelectedSeriesName = "Junior Championship";
                        break;
                    case "3":
                        gameConfig.SelectedDifficulty = DifficultyLevel.Pro;
                        gameConfig.SelectedSeriesName = "Pro Circuit";
                        break;
                }
            }
            
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
            
            ConsoleHelper.DisplayMenuOption(1, "🧮 Test Math Engine");
            ConsoleHelper.DisplayMenuOption(2, "⚙️  Other Settings (Coming Soon)");
            ConsoleHelper.DisplayMenuOption(3, "🔙 Back to Main Menu");
            
            Console.WriteLine();
            string input = ConsoleHelper.GetUserInput("Select option (1-3)");
            
            return input switch
            {
                "1" => TestMathEngine(),
                "2" => DisplayComingSoon("Settings"),
                "3" => GameState.Menu,
                _ => HandleInvalidInput("Invalid selection. Please choose 1-3.")
            };
        }
        
        /// <summary>
        /// Test the math engine and return to settings
        /// </summary>
        private GameState TestMathEngine()
        {
            MathTester.TestMathEngine();
            return GameState.Menu; // Return to main menu after test
        }
        
        /// <summary>
        /// Display a coming soon message
        /// </summary>
        private GameState DisplayComingSoon(string feature)
        {
            ConsoleHelper.DisplayHeader($"{feature.ToUpper()} - COMING SOON");
            
            Console.WriteLine($"🚧 {feature} will be available in a future update!");
            Console.WriteLine();
            Console.WriteLine("Coming soon:");
            Console.WriteLine("• Sound effects toggle");
            Console.WriteLine("• Difficulty adjustment");
            Console.WriteLine("• Color theme selection");
            Console.WriteLine("• And more!");
            
            ConsoleHelper.WaitForKeyPress();
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
