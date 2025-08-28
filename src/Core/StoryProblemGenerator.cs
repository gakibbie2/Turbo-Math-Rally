using TurboMathRally.Math;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Generates car-themed story problems for repair scenarios
    /// </summary>
    public class StoryProblemGenerator
    {
        private readonly Random _random;
        
        /// <summary>
        /// Initialize the story problem generator
        /// </summary>
        public StoryProblemGenerator()
        {
            _random = new Random();
        }
        
        /// <summary>
        /// Generate a car-themed story problem for repairs
        /// </summary>
        /// <param name="difficulty">Difficulty level to match</param>
        /// <returns>A story problem with context and answer</returns>
        public StoryProblem GenerateRepairStoryProblem(DifficultyLevel difficulty)
        {
            // Get problem templates for different operations
            var additionTemplates = GetAdditionTemplates();
            var subtractionTemplates = GetSubtractionTemplates();
            var multiplicationTemplates = GetMultiplicationTemplates();
            var divisionTemplates = GetDivisionTemplates();
            
            // Combine all templates
            var allTemplates = additionTemplates
                .Concat(subtractionTemplates)
                .Concat(multiplicationTemplates)
                .Concat(divisionTemplates)
                .ToArray();
            
            // Select random template
            var template = allTemplates[_random.Next(allTemplates.Length)];
            
            // Generate numbers based on difficulty
            var (num1, num2) = GenerateNumbersForDifficulty(difficulty, template.Operation);
            
            // Create the story problem
            return new StoryProblem
            {
                StoryText = string.Format(template.Template, num1, num2),
                Operation = template.Operation,
                Number1 = num1,
                Number2 = num2,
                Answer = CalculateAnswer(template.Operation, num1, num2),
                Context = template.Context,
                Difficulty = difficulty
            };
        }
        
        /// <summary>
        /// Get addition story templates
        /// </summary>
        private List<StoryTemplate> GetAdditionTemplates()
        {
            return new List<StoryTemplate>
            {
                new StoryTemplate
                {
                    Template = "Your mechanic gives you {0} wrenches and then hands you {1} more. How many wrenches do you have in total?",
                    Operation = MathOperation.Addition,
                    Context = "Getting tools from the mechanic"
                },
                new StoryTemplate
                {
                    Template = "You find {0} spare bolts in your toolbox and {1} more bolts on the workbench. How many bolts do you have altogether?",
                    Operation = MathOperation.Addition,
                    Context = "Collecting repair parts"
                },
                new StoryTemplate
                {
                    Template = "The parts store gives you {0} spark plugs and you buy {1} additional ones. How many spark plugs do you have now?",
                    Operation = MathOperation.Addition,
                    Context = "Buying car parts"
                },
                new StoryTemplate
                {
                    Template = "You worked on your car for {0} minutes in the morning and {1} minutes in the afternoon. How many minutes did you work in total?",
                    Operation = MathOperation.Addition,
                    Context = "Tracking repair time"
                }
            };
        }
        
        /// <summary>
        /// Get subtraction story templates
        /// </summary>
        private List<StoryTemplate> GetSubtractionTemplates()
        {
            return new List<StoryTemplate>
            {
                new StoryTemplate
                {
                    Template = "Your car's gas tank holds {0} gallons. You've already used {1} gallons during the rally. How many gallons are left?",
                    Operation = MathOperation.Subtraction,
                    Context = "Fuel management"
                },
                new StoryTemplate
                {
                    Template = "You started with {0} screws for your repair but dropped {1} of them. How many screws do you have left?",
                    Operation = MathOperation.Subtraction,
                    Context = "Lost repair parts"
                },
                new StoryTemplate
                {
                    Template = "The repair should take {0} minutes total. You've already spent {1} minutes working. How many more minutes do you need?",
                    Operation = MathOperation.Subtraction,
                    Context = "Remaining repair time"
                },
                new StoryTemplate
                {
                    Template = "Your car needs {0} new parts, but the mechanic only has {1} in stock. How many parts are still needed?",
                    Operation = MathOperation.Subtraction,
                    Context = "Parts shortage"
                }
            };
        }
        
        /// <summary>
        /// Get multiplication story templates
        /// </summary>
        private List<StoryTemplate> GetMultiplicationTemplates()
        {
            return new List<StoryTemplate>
            {
                new StoryTemplate
                {
                    Template = "Each wheel on your car needs {0} bolts. Your car has {1} wheels. How many bolts do you need in total?",
                    Operation = MathOperation.Multiplication,
                    Context = "Calculating total parts needed"
                },
                new StoryTemplate
                {
                    Template = "Each spark plug costs ${0}. You need to buy {1} spark plugs. How much will you spend in total?",
                    Operation = MathOperation.Multiplication,
                    Context = "Calculating repair costs"
                },
                new StoryTemplate
                {
                    Template = "Your car has {0} cylinders and each cylinder needs {1} new seals. How many seals do you need altogether?",
                    Operation = MathOperation.Multiplication,
                    Context = "Engine repair calculations"
                },
                new StoryTemplate
                {
                    Template = "You need to tighten {0} bolts and each bolt requires {1} full turns. How many turns will you make in total?",
                    Operation = MathOperation.Multiplication,
                    Context = "Repair procedure counting"
                }
            };
        }
        
        /// <summary>
        /// Get division story templates
        /// </summary>
        private List<StoryTemplate> GetDivisionTemplates()
        {
            return new List<StoryTemplate>
            {
                new StoryTemplate
                {
                    Template = "You have {0} screws to divide equally among {1} wheels. How many screws does each wheel get?",
                    Operation = MathOperation.Division,
                    Context = "Equal distribution of parts"
                },
                new StoryTemplate
                {
                    Template = "The mechanic has {0} minutes to fix {1} cars. How many minutes can be spent on each car?",
                    Operation = MathOperation.Division,
                    Context = "Time allocation"
                },
                new StoryTemplate
                {
                    Template = "You bought {0} spark plugs for ${1} each. If you split the cost equally, how much did each spark plug cost?",
                    Operation = MathOperation.Division,
                    Context = "Cost per item calculation"
                },
                new StoryTemplate
                {
                    Template = "The parts store has {0} oil filters to pack into {1} boxes equally. How many filters go in each box?",
                    Operation = MathOperation.Division,
                    Context = "Packaging calculations"
                }
            };
        }
        
        /// <summary>
        /// Generate appropriate numbers for the given difficulty and operation
        /// </summary>
        private (int num1, int num2) GenerateNumbersForDifficulty(DifficultyLevel difficulty, MathOperation operation)
        {
            return difficulty switch
            {
                DifficultyLevel.Rookie => GenerateRookieNumbers(operation),
                DifficultyLevel.Junior => GenerateJuniorNumbers(operation),
                DifficultyLevel.Pro => GenerateProNumbers(operation),
                _ => GenerateRookieNumbers(operation)
            };
        }
        
        /// <summary>
        /// Generate numbers for rookie difficulty (ages 5-7)
        /// </summary>
        private (int num1, int num2) GenerateRookieNumbers(MathOperation operation)
        {
            switch (operation)
            {
                case MathOperation.Addition:
                    return (_random.Next(1, 8), _random.Next(1, 8));
                case MathOperation.Subtraction:
                    int larger = _random.Next(3, 10);
                    int smaller = _random.Next(1, larger);
                    return (larger, smaller);
                case MathOperation.Multiplication:
                    return (_random.Next(1, 6), _random.Next(1, 6));
                case MathOperation.Division:
                    int divisor = _random.Next(1, 6);
                    int result = _random.Next(1, 6);
                    return (divisor * result, divisor);
                default:
                    return (2, 3);
            }
        }
        
        /// <summary>
        /// Generate numbers for junior difficulty (ages 7-9)
        /// </summary>
        private (int num1, int num2) GenerateJuniorNumbers(MathOperation operation)
        {
            switch (operation)
            {
                case MathOperation.Addition:
                    return (_random.Next(10, 50), _random.Next(5, 30));
                case MathOperation.Subtraction:
                    int larger = _random.Next(20, 80);
                    int smaller = _random.Next(5, larger - 5);
                    return (larger, smaller);
                case MathOperation.Multiplication:
                    return (_random.Next(2, 12), _random.Next(2, 8));
                case MathOperation.Division:
                    int divisor = _random.Next(2, 10);
                    int result = _random.Next(2, 15);
                    return (divisor * result, divisor);
                default:
                    return (12, 4);
            }
        }
        
        /// <summary>
        /// Generate numbers for pro difficulty (ages 9-12)
        /// </summary>
        private (int num1, int num2) GenerateProNumbers(MathOperation operation)
        {
            switch (operation)
            {
                case MathOperation.Addition:
                    return (_random.Next(50, 200), _random.Next(25, 150));
                case MathOperation.Subtraction:
                    int larger = _random.Next(100, 500);
                    int smaller = _random.Next(25, larger - 10);
                    return (larger, smaller);
                case MathOperation.Multiplication:
                    return (_random.Next(5, 25), _random.Next(3, 15));
                case MathOperation.Division:
                    int divisor = _random.Next(3, 20);
                    int result = _random.Next(5, 30);
                    return (divisor * result, divisor);
                default:
                    return (144, 12);
            }
        }
        
        /// <summary>
        /// Calculate the answer for the given operation and numbers
        /// </summary>
        private int CalculateAnswer(MathOperation operation, int num1, int num2)
        {
            return operation switch
            {
                MathOperation.Addition => num1 + num2,
                MathOperation.Subtraction => num1 - num2,
                MathOperation.Multiplication => num1 * num2,
                MathOperation.Division => num1 / num2,
                _ => 0
            };
        }
    }
    
    /// <summary>
    /// Template for generating story problems
    /// </summary>
    public class StoryTemplate
    {
        /// <summary>
        /// Story template with {0} and {1} placeholders for numbers
        /// </summary>
        public string Template { get; set; } = string.Empty;
        
        /// <summary>
        /// Math operation this template represents
        /// </summary>
        public MathOperation Operation { get; set; }
        
        /// <summary>
        /// Context description for this story
        /// </summary>
        public string Context { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// Complete story problem with context and solution
    /// </summary>
    public class StoryProblem
    {
        /// <summary>
        /// The story problem text
        /// </summary>
        public string StoryText { get; set; } = string.Empty;
        
        /// <summary>
        /// Math operation used
        /// </summary>
        public MathOperation Operation { get; set; }
        
        /// <summary>
        /// First number in the problem
        /// </summary>
        public int Number1 { get; set; }
        
        /// <summary>
        /// Second number in the problem
        /// </summary>
        public int Number2 { get; set; }
        
        /// <summary>
        /// Correct answer
        /// </summary>
        public int Answer { get; set; }
        
        /// <summary>
        /// Story context description
        /// </summary>
        public string Context { get; set; } = string.Empty;
        
        /// <summary>
        /// Difficulty level
        /// </summary>
        public DifficultyLevel Difficulty { get; set; }
    }
}
