using TurboMathRally.Utils;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Main game class that manages the game loop and state transitions
    /// </summary>
    public class Game
    {
        private GameState _currentState;
        private readonly MenuSystem _menuSystem;
        private bool _isRunning;
        
        /// <summary>
        /// Initialize a new game instance
        /// </summary>
        public Game()
        {
            _currentState = GameState.Menu;
            _menuSystem = new MenuSystem();
            _isRunning = true;
        }
        
        /// <summary>
        /// Start the main game loop
        /// </summary>
        public void Run()
        {
            DisplayWelcome();
            
            while (_isRunning)
            {
                try
                {
                    _currentState = _currentState switch
                    {
                        GameState.Menu => _menuSystem.DisplayMainMenu(),
                        GameState.ModeSelection => _menuSystem.DisplayModeSelection(),
                        GameState.MathSelection => _menuSystem.DisplayMathSelection(),
                        GameState.SeriesSelection => _menuSystem.DisplaySeriesSelection(),
                        GameState.Playing => HandlePlaying(),
                        GameState.CarRepair => HandleCarRepair(),
                        GameState.StageComplete => HandleStageComplete(),
                        GameState.GameOver => HandleGameOver(),
                        GameState.ParentDashboard => HandleParentDashboard(),
                        GameState.Exit => HandleExit(),
                        _ => GameState.Menu
                    };
                }
                catch (Exception ex)
                {
                    HandleError(ex);
                }
            }
        }
        
        /// <summary>
        /// Display welcome message
        /// </summary>
        private void DisplayWelcome()
        {
            ConsoleHelper.DisplayHeader("WELCOME TO TURBO MATH RALLY!");
            
            Console.WriteLine("🏎️ Rev up your math skills in this exciting rally adventure!");
            Console.WriteLine();
            Console.WriteLine("Race through challenging courses by solving math problems.");
            Console.WriteLine("The faster and more accurate you are, the better you'll perform!");
            Console.WriteLine();
            ConsoleHelper.DisplaySuccess("Get ready to put the pedal to the metal! 🏁");
            
            ConsoleHelper.WaitForKeyPress();
        }
        
        /// <summary>
        /// Handle the playing state (placeholder for now)
        /// </summary>
        private GameState HandlePlaying()
        {
            ConsoleHelper.DisplayHeader("RALLY STAGE IN PROGRESS");
            
            Console.WriteLine("🏁 Racing mechanics will be implemented in the next work item!");
            Console.WriteLine();
            Console.WriteLine("For now, let's simulate a successful stage completion...");
            
            ConsoleHelper.WaitForKeyPress();
            return GameState.StageComplete;
        }
        
        /// <summary>
        /// Handle car repair state (placeholder for now)
        /// </summary>
        private GameState HandleCarRepair()
        {
            ConsoleHelper.DisplayHeader("CAR REPAIR NEEDED");
            
            Console.WriteLine("🔧 Your car broke down! Time for a repair story problem.");
            Console.WriteLine();
            Console.WriteLine("Car repair mechanics will be implemented soon...");
            
            ConsoleHelper.WaitForKeyPress();
            return GameState.Playing;
        }
        
        /// <summary>
        /// Handle stage complete state
        /// </summary>
        private GameState HandleStageComplete()
        {
            ConsoleHelper.DisplayHeader("STAGE COMPLETE!");
            
            ConsoleHelper.DisplaySuccess("🏆 Congratulations! You completed the rally stage!");
            Console.WriteLine();
            Console.WriteLine("🏎️ Great driving and excellent math skills!");
            Console.WriteLine("⏱️  Time bonus points will be calculated in future updates.");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(1, "🏁 Race Another Stage");
            ConsoleHelper.DisplayMenuOption(2, "🏠 Return to Main Menu");
            
            Console.WriteLine();
            string input = ConsoleHelper.GetUserInput("What would you like to do? (1-2)");
            
            return input switch
            {
                "1" => GameState.SeriesSelection,
                "2" => GameState.Menu,
                _ => GameState.Menu
            };
        }
        
        /// <summary>
        /// Handle game over state
        /// </summary>
        private GameState HandleGameOver()
        {
            ConsoleHelper.DisplayHeader("GAME OVER");
            
            Console.WriteLine("😔 Better luck next time, racer!");
            Console.WriteLine();
            Console.WriteLine("🎯 Remember: Practice makes perfect!");
            Console.WriteLine("🏎️ Every great racer started as a beginner.");
            Console.WriteLine();
            
            ConsoleHelper.DisplayMenuOption(1, "🔄 Try Again");
            ConsoleHelper.DisplayMenuOption(2, "🏠 Return to Main Menu");
            
            Console.WriteLine();
            string input = ConsoleHelper.GetUserInput("What would you like to do? (1-2)");
            
            return input switch
            {
                "1" => GameState.SeriesSelection,
                "2" => GameState.Menu,
                _ => GameState.Menu
            };
        }
        
        /// <summary>
        /// Handle parent dashboard state (placeholder for now)
        /// </summary>
        private GameState HandleParentDashboard()
        {
            ConsoleHelper.DisplayHeader("PARENT DASHBOARD");
            
            Console.WriteLine("📊 Parent analytics dashboard coming soon!");
            Console.WriteLine();
            Console.WriteLine("This will include:");
            Console.WriteLine("• Detailed performance statistics");
            Console.WriteLine("• Areas needing improvement");
            Console.WriteLine("• Time spent on each math operation");
            Console.WriteLine("• Progress tracking over time");
            
            ConsoleHelper.WaitForKeyPress();
            return GameState.Menu;
        }
        
        /// <summary>
        /// Handle exit state
        /// </summary>
        private GameState HandleExit()
        {
            ConsoleHelper.ClearScreen();
            ConsoleHelper.DisplayHeader("THANKS FOR PLAYING!");
            
            Console.WriteLine("🏎️ Thanks for racing with Turbo Math Rally!");
            Console.WriteLine("🧮 Keep practicing those math skills!");
            Console.WriteLine("🏁 See you on the track next time!");
            Console.WriteLine();
            ConsoleHelper.DisplaySuccess("Drive safe! 🚗💨");
            
            _isRunning = false;
            return GameState.Exit;
        }
        
        /// <summary>
        /// Handle unexpected errors gracefully
        /// </summary>
        private void HandleError(Exception ex)
        {
            ConsoleHelper.DisplayError($"An unexpected error occurred: {ex.Message}");
            ConsoleHelper.DisplayWarning("Returning to main menu...");
            ConsoleHelper.WaitForKeyPress();
            _currentState = GameState.Menu;
        }
    }
}
