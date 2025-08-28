namespace TurboMathRally.Math
{
    /// <summary>
    /// Generates math problems based on difficulty level and operation type
    /// </summary>
    public class ProblemGenerator
    {
        private readonly Random _random;
        
        public ProblemGenerator()
        {
            _random = new Random();
        }
        
        /// <summary>
        /// Generate a random math problem for the specified operation and difficulty
        /// </summary>
        public MathProblem GenerateProblem(MathOperation operation, DifficultyLevel difficulty)
        {
            return operation switch
            {
                MathOperation.Addition => GenerateAddition(difficulty),
                MathOperation.Subtraction => GenerateSubtraction(difficulty),
                MathOperation.Multiplication => GenerateMultiplication(difficulty),
                MathOperation.Division => GenerateDivision(difficulty),
                _ => throw new ArgumentException("Unknown math operation")
            };
        }
        
        /// <summary>
        /// Generate a random problem from any operation for the difficulty level
        /// </summary>
        public MathProblem GenerateRandomProblem(DifficultyLevel difficulty)
        {
            var operations = Enum.GetValues<MathOperation>();
            var randomOperation = operations[_random.Next(operations.Length)];
            return GenerateProblem(randomOperation, difficulty);
        }
        
        /// <summary>
        /// Generate an addition problem
        /// </summary>
        private MathProblem GenerateAddition(DifficultyLevel difficulty)
        {
            int operand1, operand2;
            
            switch (difficulty)
            {
                case DifficultyLevel.Rookie: // Ages 5-7: Single digit
                    operand1 = _random.Next(1, 10); // 1-9
                    operand2 = _random.Next(1, 10); // 1-9
                    break;
                    
                case DifficultyLevel.Junior: // Ages 7-9: Double digit, no carrying
                    operand1 = GenerateNoCarryAddend();
                    operand2 = GenerateNoCarryAddend(operand1);
                    break;
                    
                case DifficultyLevel.Pro: // Ages 9-12: Any double digit
                    operand1 = _random.Next(10, 100); // 10-99
                    operand2 = _random.Next(1, 50);   // Keep sum reasonable
                    break;
                    
                default:
                    throw new ArgumentException("Invalid difficulty level");
            }
            
            return new MathProblem(MathOperation.Addition, operand1, operand2, difficulty);
        }
        
        /// <summary>
        /// Generate a subtraction problem (ensures no negative results)
        /// </summary>
        private MathProblem GenerateSubtraction(DifficultyLevel difficulty)
        {
            int operand1, operand2;
            
            switch (difficulty)
            {
                case DifficultyLevel.Rookie: // Ages 5-7: Single digit
                    operand1 = _random.Next(2, 10); // 2-9 (larger number)
                    operand2 = _random.Next(1, operand1); // 1 to operand1-1
                    break;
                    
                case DifficultyLevel.Junior: // Ages 7-9: Double digit, no borrowing
                    operand1 = GenerateNoBorrowMinuend();
                    operand2 = GenerateNoBorrowSubtrahend(operand1);
                    break;
                    
                case DifficultyLevel.Pro: // Ages 9-12: Any subtraction
                    operand1 = _random.Next(20, 100); // 20-99
                    operand2 = _random.Next(1, operand1); // Ensure positive result
                    break;
                    
                default:
                    throw new ArgumentException("Invalid difficulty level");
            }
            
            return new MathProblem(MathOperation.Subtraction, operand1, operand2, difficulty);
        }
        
        /// <summary>
        /// Generate a multiplication problem
        /// </summary>
        private MathProblem GenerateMultiplication(DifficultyLevel difficulty)
        {
            int operand1, operand2;
            
            switch (difficulty)
            {
                case DifficultyLevel.Rookie: // Ages 5-7: Simple times tables
                    operand1 = _random.Next(1, 6); // 1-5
                    operand2 = _random.Next(1, 6); // 1-5
                    break;
                    
                case DifficultyLevel.Junior: // Ages 7-9: Times tables 1-5
                    operand1 = _random.Next(1, 6); // 1-5
                    operand2 = _random.Next(1, 11); // 1-10
                    break;
                    
                case DifficultyLevel.Pro: // Ages 9-12: Full times tables
                    operand1 = _random.Next(1, 13); // 1-12
                    operand2 = _random.Next(1, 13); // 1-12
                    break;
                    
                default:
                    throw new ArgumentException("Invalid difficulty level");
            }
            
            return new MathProblem(MathOperation.Multiplication, operand1, operand2, difficulty);
        }
        
        /// <summary>
        /// Generate a division problem
        /// </summary>
        private MathProblem GenerateDivision(DifficultyLevel difficulty)
        {
            int operand2, answer;
            
            switch (difficulty)
            {
                case DifficultyLevel.Rookie: // Ages 5-7: Simple division
                    operand2 = _random.Next(2, 6); // 2-5 (divisor)
                    answer = _random.Next(1, 6);   // 1-5 (quotient)
                    break;
                    
                case DifficultyLevel.Junior: // Ages 7-9: Simple division
                    operand2 = _random.Next(2, 6); // 2-5 (divisor)
                    answer = _random.Next(1, 11);  // 1-10 (quotient)
                    break;
                    
                case DifficultyLevel.Pro: // Ages 9-12: Division with potential remainders
                    operand2 = _random.Next(2, 13); // 2-12 (divisor)
                    answer = _random.Next(1, 15);   // 1-14 (quotient)
                    
                    // 30% chance for remainder in Pro level
                    if (_random.Next(1, 101) <= 30)
                    {
                        int remainder = _random.Next(1, operand2);
                        int dividend = (answer * operand2) + remainder;
                        // For now, we'll still use exact division to keep it simple
                        // Remainder division can be added in a future enhancement
                    }
                    break;
                    
                default:
                    throw new ArgumentException("Invalid difficulty level");
            }
            
            int operand1 = answer * operand2; // Calculate dividend to ensure exact division
            
            return new MathProblem(MathOperation.Division, operand1, operand2, difficulty);
        }
        
        /// <summary>
        /// Generate a number for addition that won't require carrying
        /// </summary>
        private int GenerateNoCarryAddend()
        {
            int tens = _random.Next(1, 10) * 10; // 10, 20, 30, etc.
            int ones = _random.Next(1, 6);       // 1-5 (keep ones digit low)
            return tens + ones; // e.g., 23, 34, 45
        }
        
        /// <summary>
        /// Generate second addend that won't cause carrying with first addend
        /// </summary>
        private int GenerateNoCarryAddend(int firstAddend)
        {
            int firstOnes = firstAddend % 10;
            int maxOnes = 9 - firstOnes; // Ensure ones don't exceed 9
            int ones = _random.Next(1, System.Math.Max(2, maxOnes + 1));
            int tens = _random.Next(0, 5) * 10; // 0, 10, 20, etc.
            return tens + ones;
        }
        
        /// <summary>
        /// Generate a minuend (number being subtracted from) for no-borrowing subtraction
        /// </summary>
        private int GenerateNoBorrowMinuend()
        {
            int tens = _random.Next(2, 10) * 10; // 20, 30, 40, etc.
            int ones = _random.Next(5, 10);      // 5-9 (ensure ones digit is large)
            return tens + ones; // e.g., 25, 37, 48
        }
        
        /// <summary>
        /// Generate subtrahend that won't require borrowing
        /// </summary>
        private int GenerateNoBorrowSubtrahend(int minuend)
        {
            int minuendOnes = minuend % 10;
            int minuendTens = minuend / 10;
            
            int ones = _random.Next(1, minuendOnes + 1); // Ones digit <= minuend ones
            int tens = _random.Next(1, minuendTens) * 10; // Tens digit < minuend tens
            
            return tens + ones;
        }
    }
}
