namespace TurboMathRally.Tests.Math
{
    /// <summary>
    /// Basic integration tests to verify core math functionality
    /// </summary>
    public class BasicMathTests
    {
        [Fact]
        public void MathProblem_Creation_Works()
        {
            // Arrange & Act
            var problem = new MathProblem();
            problem.Operand1 = 5;
            problem.Operand2 = 3;
            problem.Operation = MathOperation.Addition;
            problem.Difficulty = DifficultyLevel.Rookie;

            // Assert
            problem.Should().NotBeNull();
            problem.Operand1.Should().Be(5);
            problem.Operand2.Should().Be(3);
            problem.Operation.Should().Be(MathOperation.Addition);
            problem.Difficulty.Should().Be(DifficultyLevel.Rookie);
        }

        [Theory]
        [InlineData(5, 3, MathOperation.Addition, 8)]
        [InlineData(10, 4, MathOperation.Subtraction, 6)]
        [InlineData(7, 2, MathOperation.Multiplication, 14)]
        [InlineData(15, 3, MathOperation.Division, 5)]
        public void MathProblem_CalculateAnswer_ReturnsCorrectResult(int operand1, int operand2, MathOperation operation, double expectedAnswer)
        {
            // Arrange
            var problem = new MathProblem();
            problem.Operand1 = operand1;
            problem.Operand2 = operand2;
            problem.Operation = operation;

            // Act
            var answer = problem.CalculateAnswer();

            // Assert
            answer.Should().Be(expectedAnswer);
        }

        [Fact]
        public void DifficultyLevel_HasExpectedValues()
        {
            // Act & Assert
            Enum.IsDefined(typeof(DifficultyLevel), DifficultyLevel.Rookie).Should().BeTrue();
            Enum.IsDefined(typeof(DifficultyLevel), DifficultyLevel.Junior).Should().BeTrue();
            Enum.IsDefined(typeof(DifficultyLevel), DifficultyLevel.Pro).Should().BeTrue();
        }

        [Fact]
        public void MathOperation_HasExpectedValues()
        {
            // Act & Assert
            Enum.IsDefined(typeof(MathOperation), MathOperation.Addition).Should().BeTrue();
            Enum.IsDefined(typeof(MathOperation), MathOperation.Subtraction).Should().BeTrue();
            Enum.IsDefined(typeof(MathOperation), MathOperation.Multiplication).Should().BeTrue();
            Enum.IsDefined(typeof(MathOperation), MathOperation.Division).Should().BeTrue();
        }
    }
}
