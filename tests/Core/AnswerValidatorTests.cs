using FluentAssertions;
using TurboMathRally.Core;
using TurboMathRally.Math;

namespace TurboMathRally.Tests.Core
{
    public class AnswerValidatorTests
    {
        [Fact]
        public void AnswerValidator_ValidInput_ValidatesCorrectly()
        {
            // Arrange
            var validator = new AnswerValidator();
            var problem = new MathProblem(MathOperation.Addition, 5, 3, DifficultyLevel.Junior);
            
            // Act
            var result = validator.ValidateAnswer(problem, "8");
            
            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.IsCorrect.Should().BeTrue();
            result.UserAnswer.Should().Be("8");
            result.CorrectAnswer.Should().Be(8);
        }
        
        [Fact]
        public void AnswerValidator_InvalidInput_ReturnsInvalid()
        {
            // Arrange
            var validator = new AnswerValidator();
            var problem = new MathProblem(MathOperation.Addition, 5, 3, DifficultyLevel.Junior);
            
            // Act
            var result = validator.ValidateAnswer(problem, "abc");
            
            // Assert
            result.IsValid.Should().BeFalse();
            result.IsCorrect.Should().BeFalse();
            result.Message.Should().Contain("Invalid input");
        }
        
        [Fact]
        public void AnswerValidator_TracksAccuracy()
        {
            // Arrange
            var validator = new AnswerValidator();
            var problem1 = new MathProblem(MathOperation.Addition, 2, 2, DifficultyLevel.Rookie);
            var problem2 = new MathProblem(MathOperation.Addition, 3, 3, DifficultyLevel.Rookie);
            
            // Act - one correct, one wrong
            validator.ValidateAnswer(problem1, "4");  // correct
            validator.ValidateAnswer(problem2, "5");  // wrong (should be 6)
            
            // Assert
            validator.AccuracyPercentage.Should().Be(50.0); // 1 correct out of 2
            validator.CorrectAnswers.Should().Be(1);
            validator.TotalQuestions.Should().Be(2);
        }
        
        [Fact]
        public void AnswerValidator_TracksStreak()
        {
            // Arrange
            var validator = new AnswerValidator();
            var problem1 = new MathProblem(MathOperation.Addition, 1, 1, DifficultyLevel.Rookie);
            var problem2 = new MathProblem(MathOperation.Addition, 2, 2, DifficultyLevel.Rookie);
            var problem3 = new MathProblem(MathOperation.Addition, 3, 3, DifficultyLevel.Rookie);
            
            // Act - build a streak
            validator.ValidateAnswer(problem1, "2");  // correct
            validator.ValidateAnswer(problem2, "4");  // correct
            validator.ValidateAnswer(problem3, "5");  // wrong
            
            // Assert
            validator.BestStreak.Should().Be(2);
            validator.CurrentStreak.Should().Be(0); // reset after wrong answer
        }
    }
}
