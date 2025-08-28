using TurboMathRally.Utils;
using TurboMathRally.Math;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Main game class that manages the game loop and state transitions
    /// </summary>
    public class Game
    {
        private GameState _currentState;
        private readonly MenuSystem _menuSystem;
        private readonly AnswerValidator _answerValidator;
        private readonly ProblemGenerator _problemGenerator;
        private readonly GameConfiguration _gameConfig;
        private bool _isRunning;
        
        /// <summary>
        /// Initialize a new game instance
        /// </summary>
        public Game()
        {
            _currentState = GameState.Menu;
            _gameConfig = new GameConfiguration();
            _menuSystem = new MenuSystem();
            _answerValidator = new AnswerValidator();
            _problemGenerator = new ProblemGenerator();
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
                        GameState.MathSelection => _menuSystem.DisplayMathSelection(_gameConfig),
                        GameState.SeriesSelection => _menuSystem.DisplaySeriesSelection(_gameConfig),
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
        /// Handle the playing state - actual racing gameplay
        /// </summary>
        private GameState HandlePlaying()
        {
            ConsoleHelper.DisplayHeader("RALLY STAGE IN PROGRESS");
            
            Console.WriteLine("🏁 Time to race! Solve math problems to speed through the stage!");
            Console.WriteLine("🚗 Answer correctly to keep your speed up!");
            Console.WriteLine("❌ Wrong answers will slow you down!");
            Console.WriteLine();
            
            // Reset validator stats for this race
            _answerValidator.ResetStats();
            
            // Generate problems for this stage (varies by difficulty)
            int questionsPerStage = _gameConfig.SelectedDifficulty switch
            {
                DifficultyLevel.Rookie => 25,    // Ages 5-7: Shorter stages
                DifficultyLevel.Junior => 35,    // Ages 7-9: Medium stages  
                DifficultyLevel.Pro => 50,       // Ages 9-12: Long stages
                _ => 25
            };
            
            int mistakes = 0;
            const int maxMistakes = 3;
            
            for (int i = 1; i <= questionsPerStage; i++)
            {
                Console.WriteLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine($"🏎️  Question {i}/{questionsPerStage} | Stage Progress: {(double)i / questionsPerStage * 100:F0}%");
                Console.WriteLine($"🏁 {_gameConfig.SelectedSeriesName} - {_gameConfig.SelectedMathTypeName}");
                Console.WriteLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();
                
                // Generate a problem (handle mixed mode)
                MathProblem problem;
                if (_gameConfig.IsMixedMode)
                {
                    // For mixed mode, randomly select an operation
                    Random random = new Random();
                    MathOperation[] operations = { MathOperation.Addition, MathOperation.Subtraction, MathOperation.Multiplication, MathOperation.Division };
                    MathOperation randomOperation = operations[random.Next(operations.Length)];
                    problem = _problemGenerator.GenerateProblem(randomOperation, _gameConfig.SelectedDifficulty);
                }
                else
                {
                    problem = _problemGenerator.GenerateProblem(_gameConfig.SelectedMathType, _gameConfig.SelectedDifficulty);
                }
                
                // Display the problem
                Console.WriteLine($"🧮 Solve this problem:");
                Console.WriteLine();
                ConsoleHelper.WriteColored($"     {problem.Question}", ConsoleColor.Yellow);
                Console.WriteLine();
                Console.WriteLine();
                
                // Get user input
                string userInput = ConsoleHelper.GetUserInput("Your answer");
                
                // Validate the answer
                ValidationResult result = _answerValidator.ValidateAnswer(problem, userInput);
                
                // Display feedback
                Console.WriteLine();
                if (result.IsCorrect)
                {
                    ConsoleHelper.DisplaySuccess(result.Message);
                    Console.WriteLine("🚀 Your car speeds ahead!");
                }
                else
                {
                    ConsoleHelper.DisplayError(result.Message);
                    Console.WriteLine($"💡 The correct answer was: {result.CorrectAnswer}");
                    mistakes++;
                    
                    if (!result.IsValid)
                    {
                        Console.WriteLine("🐌 Invalid input slowed you down!");
                    }
                    else
                    {
                        Console.WriteLine("🐌 Your car loses speed!");
                    }
                    
                    // Check if too many mistakes (car breakdown)
                    if (mistakes >= maxMistakes)
                    {
                        Console.WriteLine();
                        ConsoleHelper.DisplayError($"💥 Oh no! Your car broke down after {maxMistakes} mistakes!");
                        Console.WriteLine("🔧 Time for emergency repairs!");
                        ConsoleHelper.WaitForKeyPress();
                        return GameState.CarRepair;
                    }
                }
                
                // Show progress
                Console.WriteLine();
                Console.WriteLine($"🎯 Current accuracy: {result.AccuracyPercentage:F1}%");
                Console.WriteLine($"❌ Mistakes: {mistakes}/{maxMistakes}");
                
                // Show milestone encouragement
                double progress = (double)i / questionsPerStage;
                if (i == (int)(questionsPerStage * 0.25))
                    Console.WriteLine("🚗 25% complete - You're cruising!");
                else if (i == (int)(questionsPerStage * 0.50))
                    Console.WriteLine("🏁 Halfway there - Keep up the great pace!");
                else if (i == (int)(questionsPerStage * 0.75))
                    Console.WriteLine("🚀 75% complete - Final stretch ahead!");
                
                if (i < questionsPerStage)
                {
                    ConsoleHelper.WaitForKeyPress("Press Enter for the next problem...");
                    Console.WriteLine();
                }
            }
            
            // Stage completed successfully!
            Console.WriteLine();
            Console.WriteLine("🏁 Stage completed!");
            _answerValidator.DisplayStats();
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
