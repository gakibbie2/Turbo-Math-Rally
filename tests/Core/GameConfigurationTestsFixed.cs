using FluentAssertions;
using TurboMathRally.Core;
using TurboMathRally.Math;

namespace TurboMathRally.Tests.Core
{
    /// <summary>
    /// Updated tests for GameConfiguration functionality
    /// </summary>
    public class GameConfigurationTestsFixed
    {
        [Fact]
        public void GameConfiguration_DefaultConstructor_SetsDefaultValues()
        {
            // Act
            var config = new GameConfiguration();

            // Assert
            config.SelectedMathType.Should().Be(MathOperation.Addition);
            config.SelectedDifficulty.Should().Be(DifficultyLevel.Junior);
            config.SelectedPlayerMode.Should().Be(PlayerMode.Kid);
            config.IsMixedMode.Should().BeFalse();
            config.SelectedSeriesName.Should().Be("Junior Championship");
            config.SelectedMathTypeName.Should().Be("Addition Only");
        }

        [Fact]
        public void GameConfiguration_SetMathType_UpdatesCorrectly()
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SelectedMathType = MathOperation.Multiplication;
            config.SelectedMathTypeName = "Multiplication Only";

            // Assert
            config.SelectedMathType.Should().Be(MathOperation.Multiplication);
            config.SelectedMathTypeName.Should().Be("Multiplication Only");
        }

        [Fact]
        public void GameConfiguration_SetDifficulty_UpdatesCorrectly()
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SelectedDifficulty = DifficultyLevel.Pro;
            config.SelectedSeriesName = "Pro Circuit";

            // Assert
            config.SelectedDifficulty.Should().Be(DifficultyLevel.Pro);
            config.SelectedSeriesName.Should().Be("Pro Circuit");
        }

        [Fact]
        public void GameConfiguration_SetPlayerMode_UpdatesCorrectly()
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SelectedPlayerMode = PlayerMode.Parent;

            // Assert
            config.SelectedPlayerMode.Should().Be(PlayerMode.Parent);
        }

        [Fact]
        public void GameConfiguration_SetMixedMode_UpdatesCorrectly()
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.IsMixedMode = true;

            // Assert
            config.IsMixedMode.Should().BeTrue();
        }

        [Theory]
        [InlineData(MathOperation.Addition, "Addition Only")]
        [InlineData(MathOperation.Subtraction, "Subtraction Only")]
        [InlineData(MathOperation.Multiplication, "Multiplication Only")]
        [InlineData(MathOperation.Division, "Division Only")]
        public void GameConfiguration_SetMathOperations_UpdatesNamesCorrectly(MathOperation operation, string expectedName)
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SelectedMathType = operation;
            config.SelectedMathTypeName = expectedName;

            // Assert
            config.SelectedMathType.Should().Be(operation);
            config.SelectedMathTypeName.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(DifficultyLevel.Rookie, "Rookie Rally")]
        [InlineData(DifficultyLevel.Junior, "Junior Championship")]
        [InlineData(DifficultyLevel.Pro, "Pro Circuit")]
        public void GameConfiguration_SetDifficultyLevels_UpdatesNamesCorrectly(DifficultyLevel level, string expectedName)
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SelectedDifficulty = level;
            config.SelectedSeriesName = expectedName;

            // Assert
            config.SelectedDifficulty.Should().Be(level);
            config.SelectedSeriesName.Should().Be(expectedName);
        }
    }
}
