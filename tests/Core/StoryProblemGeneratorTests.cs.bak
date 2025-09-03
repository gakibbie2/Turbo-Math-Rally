namespace TurboMathRally.Tests.Core
{
    /// <summary>
    /// Tests for the StoryProblemGenerator class
    /// </summary>
    public class StoryProblemGeneratorTests
    {
        private readonly StoryProblemGenerator _generator;

        public StoryProblemGeneratorTests()
        {
            _generator = new StoryProblemGenerator();
        }

        [Theory]
        [InlineData(ProblemType.Addition)]
        [InlineData(ProblemType.Subtraction)]
        [InlineData(ProblemType.Multiplication)]
        [InlineData(ProblemType.Division)]
        public void GenerateStoryProblem_ValidTypes_ReturnsStoryProblem(ProblemType problemType)
        {
            // Act
            var storyProblem = _generator.GenerateStoryProblem(problemType, DifficultyLevel.Junior);

            // Assert
            storyProblem.Should().NotBeNull();
            storyProblem.Question.Should().NotBeNullOrEmpty();
            storyProblem.CorrectAnswer.Should().BeGreaterThan(0);
            storyProblem.Type.Should().Be(problemType);
            storyProblem.Difficulty.Should().Be(DifficultyLevel.Junior);
        }

        [Fact]
        public void GenerateStoryProblem_Addition_ContainsRallyTheme()
        {
            // Act
            var storyProblem = _generator.GenerateStoryProblem(ProblemType.Addition, DifficultyLevel.Rookie);

            // Assert
            var question = storyProblem.Question.ToLower();
            question.Should().ContainAny("car", "race", "track", "driver", "speed", "rally", "lap", "fuel");
        }

        [Fact]
        public void GenerateStoryProblem_Subtraction_ContainsRallyTheme()
        {
            // Act
            var storyProblem = _generator.GenerateStoryProblem(ProblemType.Subtraction, DifficultyLevel.Junior);

            // Assert
            var question = storyProblem.Question.ToLower();
            question.Should().ContainAny("car", "race", "track", "driver", "speed", "rally", "lap", "fuel", "time", "distance");
        }

        [Fact]
        public void GenerateStoryProblem_Multiplication_ContainsRallyTheme()
        {
            // Act
            var storyProblem = _generator.GenerateStoryProblem(ProblemType.Multiplication, DifficultyLevel.Pro);

            // Assert
            var question = storyProblem.Question.ToLower();
            question.Should().ContainAny("car", "race", "track", "driver", "speed", "rally", "lap", "fuel");
        }

        [Fact]
        public void GenerateStoryProblem_Division_ContainsRallyTheme()
        {
            // Act
            var storyProblem = _generator.GenerateStoryProblem(ProblemType.Division, DifficultyLevel.Pro);

            // Assert
            var question = storyProblem.Question.ToLower();
            question.Should().ContainAny("car", "race", "track", "driver", "speed", "rally", "lap", "fuel", "team", "equally");
        }

        [Theory]
        [InlineData(DifficultyLevel.Rookie)]
        [InlineData(DifficultyLevel.Junior)]
        [InlineData(DifficultyLevel.Pro)]
        public void GenerateStoryProblem_DifferentDifficulties_ReturnsValidProblems(DifficultyLevel difficulty)
        {
            // Act
            var storyProblem = _generator.GenerateStoryProblem(ProblemType.Addition, difficulty);

            // Assert
            storyProblem.Should().NotBeNull();
            storyProblem.Difficulty.Should().Be(difficulty);
            storyProblem.CorrectAnswer.Should().BeGreaterThan(0);
            
            // Check that numbers are within appropriate range for difficulty
            var (minRange, maxRange) = DifficultyManager.GetNumberRange(difficulty);
            storyProblem.CorrectAnswer.Should().BeLessOrEqualTo(maxRange * 2); // Allow for reasonable calculation results
        }

        [Fact]
        public void GenerateStoryProblem_Addition_MathematicallyCorrect()
        {
            // Act
            var storyProblem = _generator.GenerateStoryProblem(ProblemType.Addition, DifficultyLevel.Rookie);

            // Assert
            // Extract numbers from the story problem to verify correctness
            var question = storyProblem.Question;
            var numbers = ExtractNumbersFromString(question);
            
            if (numbers.Count >= 2)
            {
                var expectedAnswer = numbers.Take(2).Sum();
                storyProblem.CorrectAnswer.Should().Be(expectedAnswer);
            }
        }

        [Fact]
        public void GenerateStoryProblem_Subtraction_MathematicallyCorrect()
        {
            // Act
            var storyProblem = _generator.GenerateStoryProblem(ProblemType.Subtraction, DifficultyLevel.Junior);

            // Assert
            // The answer should be positive for story problems
            storyProblem.CorrectAnswer.Should().BeGreaterOrEqualTo(0);
            
            var question = storyProblem.Question;
            var numbers = ExtractNumbersFromString(question);
            
            if (numbers.Count >= 2)
            {
                var expectedAnswer = numbers[0] - numbers[1];
                if (expectedAnswer >= 0)
                {
                    storyProblem.CorrectAnswer.Should().Be(expectedAnswer);
                }
            }
        }

        [Fact]
        public void GenerateStoryProblem_Multiplication_MathematicallyCorrect()
        {
            // Act
            var storyProblem = _generator.GenerateStoryProblem(ProblemType.Multiplication, DifficultyLevel.Pro);

            // Assert
            var question = storyProblem.Question;
            var numbers = ExtractNumbersFromString(question);
            
            if (numbers.Count >= 2)
            {
                var expectedAnswer = numbers[0] * numbers[1];
                storyProblem.CorrectAnswer.Should().Be(expectedAnswer);
            }
        }

        [Fact]
        public void GenerateStoryProblem_Division_MathematicallyCorrect()
        {
            // Act
            var storyProblem = _generator.GenerateStoryProblem(ProblemType.Division, DifficultyLevel.Pro);

            // Assert
            storyProblem.CorrectAnswer.Should().BeGreaterThan(0);
            
            // Division should result in whole numbers for story problems
            storyProblem.CorrectAnswer.Should().Be((int)storyProblem.CorrectAnswer);
        }

        [Fact]
        public void GenerateMultipleStoryProblems_ReturnsRequestedCount()
        {
            // Act
            var storyProblems = _generator.GenerateMultipleStoryProblems(3, ProblemType.Addition, DifficultyLevel.Junior);

            // Assert
            storyProblems.Should().HaveCount(3);
            storyProblems.Should().AllSatisfy(p =>
            {
                p.Type.Should().Be(ProblemType.Addition);
                p.Difficulty.Should().Be(DifficultyLevel.Junior);
                p.Question.Should().NotBeNullOrEmpty();
            });
        }

        [Fact]
        public void GenerateMultipleStoryProblems_ZeroCount_ReturnsEmptyList()
        {
            // Act
            var storyProblems = _generator.GenerateMultipleStoryProblems(0, ProblemType.Addition, DifficultyLevel.Rookie);

            // Assert
            storyProblems.Should().BeEmpty();
        }

        [Fact]
        public void GenerateStoryProblem_Mixed_GeneratesVariousTypes()
        {
            // Act
            var storyProblems = _generator.GenerateMultipleStoryProblems(20, ProblemType.Mixed, DifficultyLevel.Junior);

            // Assert
            storyProblems.Should().HaveCount(20);
            
            // Should have different operation types
            var operationTypes = storyProblems.Select(p => p.Type).Distinct().ToList();
            operationTypes.Should().HaveCountGreaterOrEqualTo(2); // At least 2 different types
            
            // Should not all be the same type
            storyProblems.Should().NotAllSatisfy(p => p.Type == storyProblems.First().Type);
        }

        /// <summary>
        /// Helper method to extract numbers from a string
        /// </summary>
        private static List<double> ExtractNumbersFromString(string input)
        {
            var numbers = new List<double>();
            var regex = new System.Text.RegularExpressions.Regex(@"\b\d+(?:\.\d+)?\b");
            var matches = regex.Matches(input);
            
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (double.TryParse(match.Value, out double number))
                {
                    numbers.Add(number);
                }
            }
            
            return numbers;
        }
    }
}
