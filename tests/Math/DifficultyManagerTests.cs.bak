using FluentAssertions;
using TurboMathRally.Math;

namespace TurboMathRally.Tests.Math
{
    public class DifficultyManagerTests
    {
        [Theory]
        [InlineData(1, DifficultyLevel.Rookie)]
        [InlineData(2, DifficultyLevel.Junior)]
        [InlineData(3, DifficultyLevel.Pro)]
        [InlineData(4, DifficultyLevel.Rookie)] // defaults to rookie
        public void GetDifficultyForSeries_ValidSeries_ReturnsCorrectLevel(int seriesNumber, DifficultyLevel expected)
        {
            // Act
            var result = DifficultyManager.GetDifficultyForSeries(seriesNumber);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Theory]
        [InlineData(DifficultyLevel.Rookie, "Ages 5-7")]
        [InlineData(DifficultyLevel.Junior, "Ages 7-9")]
        [InlineData(DifficultyLevel.Pro, "Ages 9-12")]
        public void GetAgeRange_ValidDifficulty_ReturnsCorrectRange(DifficultyLevel difficulty, string expectedRange)
        {
            // Act
            var result = DifficultyManager.GetAgeRange(difficulty);
            
            // Assert
            result.Should().Be(expectedRange);
        }
        
        [Theory]
        [InlineData(DifficultyLevel.Rookie, "Rookie Rally")]
        [InlineData(DifficultyLevel.Junior, "Junior Championship")]
        [InlineData(DifficultyLevel.Pro, "Pro Circuit")]
        public void GetSeriesName_ValidDifficulty_ReturnsCorrectName(DifficultyLevel difficulty, string expectedName)
        {
            // Act
            var result = DifficultyManager.GetSeriesName(difficulty);
            
            // Assert
            result.Should().Be(expectedName);
        }
        
        [Theory]
        [InlineData(MathOperation.Addition, DifficultyLevel.Rookie, true)]
        [InlineData(MathOperation.Subtraction, DifficultyLevel.Rookie, true)]
        [InlineData(MathOperation.Multiplication, DifficultyLevel.Rookie, false)]
        [InlineData(MathOperation.Division, DifficultyLevel.Rookie, false)]
        [InlineData(MathOperation.Multiplication, DifficultyLevel.Junior, true)]
        [InlineData(MathOperation.Division, DifficultyLevel.Pro, true)]
        public void IsOperationAvailable_VariousCombinations_ReturnsCorrectAvailability(
            MathOperation operation, DifficultyLevel difficulty, bool expected)
        {
            // Act
            var result = DifficultyManager.IsOperationAvailable(operation, difficulty);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Theory]
        [InlineData(DifficultyLevel.Rookie, 8)]
        [InlineData(DifficultyLevel.Junior, 12)]
        [InlineData(DifficultyLevel.Pro, 15)]
        public void GetProblemsPerStage_ValidDifficulty_ReturnsCorrectCount(DifficultyLevel difficulty, int expected)
        {
            // Act
            var result = DifficultyManager.GetProblemsPerStage(difficulty);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Theory]
        [InlineData(DifficultyLevel.Rookie, 0.70)]
        [InlineData(DifficultyLevel.Junior, 0.75)]
        [InlineData(DifficultyLevel.Pro, 0.80)]
        public void GetMinimumAccuracy_ValidDifficulty_ReturnsCorrectAccuracy(DifficultyLevel difficulty, double expected)
        {
            // Act
            var result = DifficultyManager.GetMinimumAccuracy(difficulty);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Fact]
        public void GetAvailableOperations_RookieLevel_ReturnsBasicOperations()
        {
            // Act
            var operations = DifficultyManager.GetAvailableOperations(DifficultyLevel.Rookie);
            
            // Assert
            operations.Should().BeEquivalentTo(new[] { MathOperation.Addition, MathOperation.Subtraction });
        }
        
        [Fact]
        public void GetAvailableOperations_JuniorLevel_ReturnsAllOperations()
        {
            // Act
            var operations = DifficultyManager.GetAvailableOperations(DifficultyLevel.Junior);
            
            // Assert
            operations.Should().BeEquivalentTo(new[] { MathOperation.Addition, MathOperation.Subtraction, 
                MathOperation.Multiplication, MathOperation.Division });
        }
    }
}
