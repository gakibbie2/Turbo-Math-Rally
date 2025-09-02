namespace TurboMathRally.Math
{
    /// <summary>
    /// Types of math operations
    /// </summary>
    public enum MathOperation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }
    
    /// <summary>
    /// Difficulty levels corresponding to age groups and rally series
    /// </summary>
    public enum DifficultyLevel
    {
        /// <summary>
        /// Ages 5-7: Rookie Rally - Basic single digit operations
        /// </summary>
        Rookie = 1,
        
        /// <summary>
        /// Ages 7-9: Junior Championship - Double digit without carrying, simple times tables
        /// </summary>
        Junior = 2,
        
        /// <summary>
        /// Ages 9-12: Pro Circuit - Complex operations with carrying, full times tables
        /// </summary>
        Pro = 3
    }
    
    /// <summary>
    /// Represents a math problem with its question, answer, and metadata
    /// </summary>
    public class MathProblem
    {
        /// <summary>
        /// The math operation type
        /// </summary>
        public MathOperation Operation { get; set; }
        
        /// <summary>
        /// First operand (left side of operation)
        /// </summary>
        public int Operand1 { get; set; }
        
        /// <summary>
        /// Second operand (right side of operation)
        /// </summary>
        public int Operand2 { get; set; }
        
        /// <summary>
        /// The correct answer
        /// </summary>
        public int Answer { get; set; }
        
        /// <summary>
        /// Difficulty level of this problem
        /// </summary>
        public DifficultyLevel Difficulty { get; set; }
        
        /// <summary>
        /// Format the problem as a string for display
        /// </summary>
        public string Question => Operation switch
        {
            MathOperation.Addition => $"{Operand1} + {Operand2} = ?",
            MathOperation.Subtraction => $"{Operand1} - {Operand2} = ?",
            MathOperation.Multiplication => $"{Operand1} × {Operand2} = ?",
            MathOperation.Division => $"{Operand1} ÷ {Operand2} = ?",
            _ => "Unknown operation"
        };
        
        /// <summary>
        /// Get the operation symbol
        /// </summary>
        public string OperationSymbol => Operation switch
        {
            MathOperation.Addition => "+",
            MathOperation.Subtraction => "-",
            MathOperation.Multiplication => "×",
            MathOperation.Division => "÷",
            _ => "?"
        };
        
        /// <summary>
        /// Check if a given answer is correct
        /// </summary>
        public bool IsCorrect(int userAnswer) => userAnswer == Answer;
        
        /// <summary>
        /// Create a new math problem
        /// </summary>
        public MathProblem(MathOperation operation, int operand1, int operand2, DifficultyLevel difficulty)
        {
            Operation = operation;
            Operand1 = operand1;
            Operand2 = operand2;
            Difficulty = difficulty;
            
            // Calculate the answer based on operation
            Answer = operation switch
            {
                MathOperation.Addition => operand1 + operand2,
                MathOperation.Subtraction => operand1 - operand2,
                MathOperation.Multiplication => operand1 * operand2,
                MathOperation.Division => operand1 / operand2,
                _ => throw new ArgumentException("Unknown operation")
            };
        }
        
        /// <summary>
        /// String representation of the problem
        /// </summary>
        public override string ToString() => $"{Question} (Answer: {Answer})";
    }
}
