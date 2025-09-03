using FluentAssertions;
using TurboMathRally.Math;

namespace TurboMathRally.Tests.Math
{
    public class MathProblemTests
    {
        [Fact]
        public void MathProblem_ValidInputs_CreatesCorrectProblem()
        {
            // Arrange & Act
            var problem = new MathProblem(MathOperation.Addition, 5, 3, DifficultyLevel.Junior);
            
            // Assert
            problem.Should().NotBeNull();
            problem.Answer.Should().Be(8);
            problem.Operation.Should().Be(MathOperation.Addition);
            problem.Operand1.Should().Be(5);
            problem.Operand2.Should().Be(3);
            problem.Difficulty.Should().Be(DifficultyLevel.Junior);
        }
        
        [Theory]
        [InlineData(MathOperation.Addition, 10, 5, 15)]
        [InlineData(MathOperation.Subtraction, 8, 3, 5)]
        [InlineData(MathOperation.Multiplication, 4, 5, 20)]
        [InlineData(MathOperation.Division, 15, 3, 5)]
        public void MathProblem_VariousOperations_CalculatesCorrectAnswer(
            MathOperation operation, int operand1, int operand2, int expectedAnswer)
        {
            // Arrange & Act
            var problem = new MathProblem(operation, operand1, operand2, DifficultyLevel.Pro);
            
            // Assert
            problem.Answer.Should().Be(expectedAnswer);
        }
        
        [Theory]
        [InlineData(MathOperation.Addition, 10, 4, 14, true)]
        [InlineData(MathOperation.Subtraction, 8, 3, 4, false)]
        public void MathProblem_IsCorrect_ValidatesCorrectly(
            MathOperation operation, int operand1, int operand2, int userAnswer, bool expected)
        {
            // Arrange
            var problem = new MathProblem(operation, operand1, operand2, DifficultyLevel.Junior);
            
            // Act
            var result = problem.IsCorrect(userAnswer);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Theory]
        [InlineData(MathOperation.Addition, 5, 3, "5 + 3 = ?")]
        [InlineData(MathOperation.Subtraction, 10, 4, "10 - 4 = ?")]
        [InlineData(MathOperation.Multiplication, 6, 7, "6 × 7 = ?")]
        [InlineData(MathOperation.Division, 12, 3, "12 ÷ 3 = ?")]
        public void MathProblem_Question_FormatsCorrectly(MathOperation operation, int operand1, int operand2, string expectedQuestion)
        {
            // Arrange & Act
            var problem = new MathProblem(operation, operand1, operand2, DifficultyLevel.Rookie);
            
            // Assert
            problem.Question.Should().Be(expectedQuestion);
        }
    }
}
