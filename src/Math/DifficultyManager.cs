namespace TurboMathRally.Math
{
    /// <summary>
    /// Manages difficulty progression and scaling for rally series
    /// </summary>
    public static class DifficultyManager
    {
        /// <summary>
        /// Get difficulty level based on rally series
        /// </summary>
        public static DifficultyLevel GetDifficultyForSeries(int seriesNumber)
        {
            return seriesNumber switch
            {
                1 => DifficultyLevel.Rookie,    // Rookie Rally (Ages 5-7)
                2 => DifficultyLevel.Junior,    // Junior Championship (Ages 7-9)
                3 => DifficultyLevel.Pro,       // Pro Circuit (Ages 9-12)
                _ => DifficultyLevel.Rookie     // Default to easiest
            };
        }
        
        /// <summary>
        /// Get recommended age range for difficulty level
        /// </summary>
        public static string GetAgeRange(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Rookie => "Ages 5-7",
                DifficultyLevel.Junior => "Ages 7-9",
                DifficultyLevel.Pro => "Ages 9-12",
                _ => "All ages"
            };
        }
        
        /// <summary>
        /// Get series name for difficulty level
        /// </summary>
        public static string GetSeriesName(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Rookie => "Rookie Rally",
                DifficultyLevel.Junior => "Junior Championship",
                DifficultyLevel.Pro => "Pro Circuit",
                _ => "Unknown Series"
            };
        }
        
        /// <summary>
        /// Get description of what this difficulty level includes
        /// </summary>
        public static string GetDifficultyDescription(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Rookie => 
                    "Single digit addition and subtraction, simple counting challenges",
                DifficultyLevel.Junior => 
                    "Double digit problems without carrying/borrowing, times tables 1-5, simple division",
                DifficultyLevel.Pro => 
                    "Complex operations with carrying/borrowing, full times tables up to 12, division with remainders",
                _ => "Unknown difficulty"
            };
        }
        
        /// <summary>
        /// Check if a math operation is appropriate for the difficulty level
        /// </summary>
        public static bool IsOperationAvailable(MathOperation operation, DifficultyLevel difficulty)
        {
            return (operation, difficulty) switch
            {
                (MathOperation.Addition, _) => true,      // Addition available at all levels
                (MathOperation.Subtraction, _) => true,   // Subtraction available at all levels
                (MathOperation.Multiplication, DifficultyLevel.Rookie) => false, // No multiplication for youngest
                (MathOperation.Multiplication, _) => true, // Available for Junior and Pro
                (MathOperation.Division, DifficultyLevel.Rookie) => false,       // No division for youngest
                (MathOperation.Division, _) => true,       // Available for Junior and Pro
                _ => false
            };
        }
        
        /// <summary>
        /// Get available operations for a difficulty level
        /// </summary>
        public static MathOperation[] GetAvailableOperations(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Rookie => new[] { MathOperation.Addition, MathOperation.Subtraction },
                DifficultyLevel.Junior => new[] { MathOperation.Addition, MathOperation.Subtraction, 
                                                   MathOperation.Multiplication, MathOperation.Division },
                DifficultyLevel.Pro => new[] { MathOperation.Addition, MathOperation.Subtraction, 
                                                MathOperation.Multiplication, MathOperation.Division },
                _ => new[] { MathOperation.Addition, MathOperation.Subtraction }
            };
        }
        
        /// <summary>
        /// Get the number of problems typically in a stage for this difficulty
        /// </summary>
        public static int GetProblemsPerStage(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Rookie => 8,   // 8-12 problems for youngest
                DifficultyLevel.Junior => 12,  // 12-15 problems for middle
                DifficultyLevel.Pro => 15,     // 15-20 problems for oldest
                _ => 10
            };
        }
        
        /// <summary>
        /// Get minimum accuracy required to pass a stage
        /// </summary>
        public static double GetMinimumAccuracy(DifficultyLevel difficulty)
        {
            return difficulty switch
            {
                DifficultyLevel.Rookie => 0.70, // 70% accuracy
                DifficultyLevel.Junior => 0.75, // 75% accuracy
                DifficultyLevel.Pro => 0.80,    // 80% accuracy
                _ => 0.70
            };
        }
    }
}
