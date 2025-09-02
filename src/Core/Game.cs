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
        private readonly CarBreakdownSystem _carBreakdownSystem;
        private readonly StoryProblemGenerator _storyProblemGenerator;
        private bool _isRunning;
        private bool _isContinuingRace = false;  // Track if we're continuing after repair
        private int _currentQuestionNumber = 1;  // Track current question in race
        private int _totalQuestionsThisRace = 0; // Track total questions including story problems
        private DateTime _raceStartTime;         // Track race start time for performance metrics
        
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
            _carBreakdownSystem = new CarBreakdownSystem();
            _storyProblemGenerator = new StoryProblemGenerator();
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
            
            // Reset validator stats and car condition for this race (only if starting fresh)
            if (!_isContinuingRace)
            {
                _answerValidator.ResetStats();
                _carBreakdownSystem.Reset();
                _currentQuestionNumber = 1;
                _totalQuestionsThisRace = 0;
                _raceStartTime = DateTime.Now; // Start timing the race
                Console.WriteLine("🎯 Starting fresh race statistics...");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("🔧 Continuing race after successful repair!");
                Console.WriteLine($"📊 Resuming from question {_currentQuestionNumber}");
                Console.WriteLine("📈 Your previous statistics are maintained.");
                Console.WriteLine();
                _isContinuingRace = false; // Reset the flag
            }
            
            // Generate problems for this stage (varies by difficulty)
            int questionsPerStage = _gameConfig.SelectedDifficulty switch
            {
                DifficultyLevel.Rookie => 25,    // Ages 5-7: Shorter stages
                DifficultyLevel.Junior => 35,    // Ages 7-9: Medium stages  
                DifficultyLevel.Pro => 50,       // Ages 9-12: Long stages
                _ => 25
            };
            
            for (int i = _currentQuestionNumber; i <= questionsPerStage; i++)
            {
                // Clear screen for clean presentation
                ConsoleHelper.ClearScreen();
                
                Console.WriteLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine($"🏁 {_gameConfig.SelectedSeriesName} - {_gameConfig.SelectedMathTypeName}");
                Console.WriteLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();
                
                // Display enhanced progress bar with live stats and car status
                ConsoleHelper.DisplayRaceProgressBar(i, questionsPerStage, $"{_gameConfig.SelectedSeriesName} Rally", 
                    _answerValidator.AccuracyPercentage, _answerValidator.CurrentStreak, _answerValidator.BestStreak,
                    _carBreakdownSystem.GetCompactCarStatus());
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
                    
                    // Add strike to car breakdown system
                    StrikeResult strikeResult = _carBreakdownSystem.AddStrike();
                    
                    if (!result.IsValid)
                    {
                        Console.WriteLine("🐌 Invalid input caused mechanical problems!");
                    }
                    else
                    {
                        Console.WriteLine("🐌 Wrong answer damaged your car!");
                    }
                    
                    Console.WriteLine();
                    
                    // Show strike warning
                    switch (strikeResult.WarningLevel)
                    {
                        case WarningLevel.Light:
                            ConsoleHelper.DisplayWarning(strikeResult.Message);
                            Console.WriteLine(strikeResult.Description);
                            break;
                        case WarningLevel.Moderate:
                            ConsoleHelper.DisplayError(strikeResult.Message);
                            Console.WriteLine(strikeResult.Description);
                            break;
                        case WarningLevel.Breakdown:
                            Console.WriteLine();
                            ConsoleHelper.DisplayError(strikeResult.Message);
                            Console.WriteLine(strikeResult.Description);
                            
                            // Save current position before going to repair
                            _currentQuestionNumber = i + 1; // Continue from next question after repair
                            
                            ConsoleHelper.WaitForKeyPress();
                            return GameState.CarRepair;
                    }
                }
                
                // Brief pause for readability before next question (only if not the last question)
                if (i < questionsPerStage)
                {
                    Console.WriteLine("⏱️ Next question in 1 second...");
                    Thread.Sleep(1000); // Brief pause for smooth transition
                }
            }
            
            // Stage completed successfully!
            Console.WriteLine();
            ConsoleHelper.DisplayRaceProgressBar(questionsPerStage, questionsPerStage, $"{_gameConfig.SelectedSeriesName} Rally");
            Console.WriteLine();
            ConsoleHelper.DisplaySuccess("🏆 STAGE COMPLETED! 🏆");
            Console.WriteLine();
            
            // Calculate race time and display comprehensive stats
            DateTime raceEndTime = DateTime.Now;
            int totalRaceTimeSeconds = (int)(raceEndTime - _raceStartTime).TotalSeconds;
            
            // Display enhanced statistics using our new method
            ConsoleHelper.DisplayRaceStats(
                _answerValidator.TotalQuestions, 
                _answerValidator.AccuracyPercentage, 
                totalRaceTimeSeconds
            );
            
            // Additional detailed stats
            Console.WriteLine();
            Console.WriteLine("🔥 ADDITIONAL RACE DETAILS:");
            Console.WriteLine($"   🎯 Best Streak: {_answerValidator.BestStreak} correct in a row");
            Console.WriteLine($"   🏁 Questions per minute: {(_answerValidator.TotalQuestions / System.Math.Max(1, totalRaceTimeSeconds / 60.0)):F1}");
            Console.WriteLine($"   ⏱️ Average time per question: {(totalRaceTimeSeconds / System.Math.Max(1, _answerValidator.TotalQuestions)):F1} seconds");
            
            if (_totalQuestionsThisRace > questionsPerStage)
            {
                Console.WriteLine($"   🔧 Repair problems solved: {_totalQuestionsThisRace - questionsPerStage}");
                Console.WriteLine("   💪 Great recovery from breakdowns!");
            }
            
            ConsoleHelper.WaitForKeyPress();
            
            return GameState.StageComplete;
        }
        
        /// <summary>
        /// Handle car repair state - solve a story problem to fix the car
        /// </summary>
        private GameState HandleCarRepair()
        {
            ConsoleHelper.DisplayHeader("🔧 EMERGENCY CAR REPAIR");
            
            Console.WriteLine("� Your rally car has broken down on the side of the track!");
            Console.WriteLine("🚗 A friendly mechanic offers to help, but you need to solve their repair problem!");
            Console.WriteLine();
            
            // Display car breakdown status
            _carBreakdownSystem.DisplayCarStatus();
            Console.WriteLine();
            
            // Generate a story problem for repair
            Console.WriteLine("🔧 REPAIR STORY PROBLEM:");
            Console.WriteLine();
            
            // Use the story problem generator
            StoryProblem storyProblem = _storyProblemGenerator.GenerateRepairStoryProblem(
                _gameConfig.SelectedDifficulty, 
                _gameConfig.SelectedMathType, 
                _gameConfig.IsMixedMode);
            
            Console.WriteLine($"📖 {storyProblem.StoryText}");
            Console.WriteLine();
            Console.WriteLine($"� Context: {storyProblem.Context}");
            Console.WriteLine();
            
            // Get user input for repair
            string userInput = ConsoleHelper.GetUserInput("Your answer");
            
            // Validate repair answer
            bool isCorrect = false;
            if (int.TryParse(userInput.Trim(), out int userAnswer))
            {
                isCorrect = userAnswer == storyProblem.Answer;
            }
            
            // Count this story problem in our total questions
            _totalQuestionsThisRace++;
            
            Console.WriteLine();
            
            if (isCorrect)
            {
                ConsoleHelper.DisplaySuccess("🎉 Perfect! The mechanic fixes your car!");
                Console.WriteLine("🔧 Your car is running like new again!");
                Console.WriteLine("🚗💨 Time to get back on the rally track!");
                Console.WriteLine();
                Console.WriteLine(_carBreakdownSystem.GetEncouragingMessage());
                
                // Reset car condition and set continuation flag
                _carBreakdownSystem.Reset();
                _isContinuingRace = true;  // Flag that we're continuing the race
                
                ConsoleHelper.WaitForKeyPress("Press Enter to continue racing...");
                return GameState.Playing;
            }
            else
            {
                ConsoleHelper.DisplayError("❌ That's not quite right...");
                Console.WriteLine($"💡 The correct answer was: {storyProblem.Answer}");
                Console.WriteLine();
                Console.WriteLine("🚨 The mechanic shakes their head sadly...");
                Console.WriteLine("💔 Your car couldn't be repaired this time.");
                Console.WriteLine("🏁 Your rally ends here, but every great driver faces setbacks!");
                
                ConsoleHelper.WaitForKeyPress();
                return GameState.GameOver;
            }
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
