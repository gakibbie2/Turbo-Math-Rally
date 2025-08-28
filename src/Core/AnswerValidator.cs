using TurboMathRally.Math;
using TurboMathRally.Utils;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Handles answer validation, input sanitization, and accuracy tracking
    /// </summary>
    public class AnswerValidator
    {
        private int _correctAnswers;
        private int _totalQuestions;
        private int _currentStreak;
        private int _bestStreak;
        
        /// <summary>
        /// Get the current accuracy percentage
        /// </summary>
        public double AccuracyPercentage => _totalQuestions == 0 ? 0 : (double)_correctAnswers / _totalQuestions * 100;
        
        /// <summary>
        /// Get the number of correct answers
        /// </summary>
        public int CorrectAnswers => _correctAnswers;
        
        /// <summary>
        /// Get the total number of questions asked
        /// </summary>
        public int TotalQuestions => _totalQuestions;
        
        /// <summary>
        /// Get the current streak of correct answers
        /// </summary>
        public int CurrentStreak => _currentStreak;
        
        /// <summary>
        /// Get the best streak achieved
        /// </summary>
        public int BestStreak => _bestStreak;
        
        /// <summary>
        /// Initialize a new answer validator
        /// </summary>
        public AnswerValidator()
        {
            ResetStats();
        }
        
        /// <summary>
        /// Reset all statistics
        /// </summary>
        public void ResetStats()
        {
            _correctAnswers = 0;
            _totalQuestions = 0;
            _currentStreak = 0;
            _bestStreak = 0;
        }
        
        /// <summary>
        /// Validate a user's answer against the correct answer
        /// </summary>
        /// <param name="problem">The math problem</param>
        /// <param name="userInput">The user's input</param>
        /// <returns>ValidationResult with details</returns>
        public ValidationResult ValidateAnswer(MathProblem problem, string userInput)
        {
            _totalQuestions++;
            
            // Sanitize input
            string cleanInput = SanitizeInput(userInput);
            
            // Try to parse the input
            if (!int.TryParse(cleanInput, out int userAnswer))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    IsCorrect = false,
                    UserAnswer = userInput,
                    CorrectAnswer = problem.Answer,
                    Message = "Invalid input! Please enter a whole number.",
                    AccuracyPercentage = AccuracyPercentage
                };
            }
            
            // Check if answer is correct
            bool isCorrect = userAnswer == problem.Answer;
            
            if (isCorrect)
            {
                _correctAnswers++;
                _currentStreak++;
                if (_currentStreak > _bestStreak)
                {
                    _bestStreak = _currentStreak;
                }
            }
            else
            {
                _currentStreak = 0;
            }
            
            return new ValidationResult
            {
                IsValid = true,
                IsCorrect = isCorrect,
                UserAnswer = userAnswer.ToString(),
                CorrectAnswer = problem.Answer,
                Message = GenerateFeedbackMessage(isCorrect, _currentStreak),
                AccuracyPercentage = AccuracyPercentage
            };
        }
        
        /// <summary>
        /// Sanitize user input by trimming whitespace and removing extra characters
        /// </summary>
        /// <param name="input">Raw user input</param>
        /// <returns>Cleaned input string</returns>
        private string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            
            // Remove all whitespace and non-digit characters except minus sign for negative numbers
            return input.Trim().Replace(" ", "").Replace(",", "");
        }
        
        /// <summary>
        /// Generate encouraging feedback messages based on correctness and streak
        /// </summary>
        /// <param name="isCorrect">Whether the answer was correct</param>
        /// <param name="streak">Current streak count</param>
        /// <returns>Feedback message</returns>
        private string GenerateFeedbackMessage(bool isCorrect, int streak)
        {
            if (isCorrect)
            {
                return streak switch
                {
                    1 => "🎉 Correct! Great job!",
                    2 => "🔥 Two in a row! You're on fire!",
                    3 => "⚡ Triple correct! Amazing streak!",
                    4 => "🚀 Four correct! You're flying!",
                    5 => "🏆 FIVE in a row! Incredible!",
                    >= 6 => $"🎯 {streak} correct answers in a row! You're a math champion!",
                    _ => "✅ Correct!"
                };
            }
            else
            {
                string[] encouragingMessages = {
                    "❌ Not quite right, but keep trying! You've got this!",
                    "🤔 Close! Take your time and try again!",
                    "💪 Don't give up! Every mistake helps you learn!",
                    "🎯 Almost there! Check your calculation again!",
                    "🌟 Keep going! You're learning with every attempt!"
                };
                
                Random random = new Random();
                return encouragingMessages[random.Next(encouragingMessages.Length)];
            }
        }
        
        /// <summary>
        /// Display current statistics
        /// </summary>
        public void DisplayStats()
        {
            Console.WriteLine();
            ConsoleHelper.DisplayHeader("RACE STATISTICS");
            
            Console.WriteLine($"📊 Questions Answered: {_totalQuestions}");
            Console.WriteLine($"✅ Correct Answers: {_correctAnswers}");
            Console.WriteLine($"🎯 Accuracy: {AccuracyPercentage:F1}%");
            Console.WriteLine($"🔥 Current Streak: {_currentStreak}");
            Console.WriteLine($"🏆 Best Streak: {_bestStreak}");
            
            if (AccuracyPercentage >= 90)
                ConsoleHelper.DisplaySuccess("🏁 Excellent driving! You're ready for the pro circuit!");
            else if (AccuracyPercentage >= 75)
                ConsoleHelper.DisplaySuccess("🚗 Great job! You're becoming a skilled rally driver!");
            else if (AccuracyPercentage >= 50)
                Console.WriteLine("🔧 Good effort! A little more practice and you'll be racing like a pro!");
            else
                Console.WriteLine("🛠️ Keep practicing! Every great driver started where you are now!");
        }
    }
    
    /// <summary>
    /// Result of answer validation
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Whether the input was valid (parseable as a number)
        /// </summary>
        public bool IsValid { get; set; }
        
        /// <summary>
        /// Whether the answer was correct
        /// </summary>
        public bool IsCorrect { get; set; }
        
        /// <summary>
        /// The user's answer (cleaned)
        /// </summary>
        public string UserAnswer { get; set; } = string.Empty;
        
        /// <summary>
        /// The correct answer
        /// </summary>
        public int CorrectAnswer { get; set; }
        
        /// <summary>
        /// Feedback message for the user
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// Current accuracy percentage
        /// </summary>
        public double AccuracyPercentage { get; set; }
    }
}
