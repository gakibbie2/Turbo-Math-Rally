using TurboMathRally.Math;
using TurboMathRally.Utils;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Handles math problem testing and demonstration
    /// </summary>
    public static class MathTester
    {
        /// <summary>
        /// Test the math problem generator by creating sample problems
        /// </summary>
        public static void TestMathEngine()
        {
            ConsoleHelper.DisplayHeader("MATH ENGINE TEST");
            
            var generator = new ProblemGenerator();
            
            // Test each difficulty level
            foreach (DifficultyLevel difficulty in Enum.GetValues<DifficultyLevel>())
            {
                ConsoleHelper.WriteLineColored($"\n🎯 Testing {DifficultyManager.GetSeriesName(difficulty)} ({DifficultyManager.GetAgeRange(difficulty)}):", ConsoleColor.Cyan);
                Console.WriteLine($"   {DifficultyManager.GetDifficultyDescription(difficulty)}");
                Console.WriteLine();
                
                var availableOps = DifficultyManager.GetAvailableOperations(difficulty);
                
                // Test each available operation
                foreach (var operation in availableOps)
                {
                    ConsoleHelper.WriteColored($"  {GetOperationIcon(operation)} ", ConsoleColor.Yellow);
                    
                    try
                    {
                        // Generate 3 sample problems for each operation
                        for (int i = 0; i < 3; i++)
                        {
                            var problem = generator.GenerateProblem(operation, difficulty);
                            Console.Write($"{problem.Question.Replace(" = ?", "")} = {problem.Answer}");
                            if (i < 2) Console.Write(", ");
                        }
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        ConsoleHelper.DisplayError($"Error generating {operation}: {ex.Message}");
                    }
                }
                
                // Test mixed problems
                ConsoleHelper.WriteColored("  🎲 Mixed: ", ConsoleColor.Yellow);
                try
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var problem = generator.GenerateRandomProblem(difficulty);
                        Console.Write($"{problem.Question.Replace(" = ?", "")} = {problem.Answer}");
                        if (i < 2) Console.Write(", ");
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayError($"Error generating mixed problems: {ex.Message}");
                }
            }
            
            ConsoleHelper.DisplaySuccess("\n✅ Math engine test completed!");
            ConsoleHelper.WaitForKeyPress("Press Enter to return to main menu...");
        }
        
        /// <summary>
        /// Get icon for math operation
        /// </summary>
        private static string GetOperationIcon(MathOperation operation)
        {
            return operation switch
            {
                MathOperation.Addition => "➕ Addition:",
                MathOperation.Subtraction => "➖ Subtraction:",
                MathOperation.Multiplication => "✖️  Multiplication:",
                MathOperation.Division => "➗ Division:",
                _ => "? Unknown:"
            };
        }
    }
}
