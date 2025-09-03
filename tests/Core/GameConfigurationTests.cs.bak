namespace TurboMathRally.Tests.Core
{
    /// <summary>
    /// Tests for the GameConfiguration class
    /// </summary>
    public class GameConfigurationTests
    {
        [Fact]
        public void Constructor_InitializesWithDefaults()
        {
            // Act
            var config = new GameConfiguration();

            // Assert
            config.SelectedDifficulty.Should().Be(DifficultyLevel.Rookie);
            config.SelectedMathType.Should().Be(ProblemType.Addition); // Assuming Addition is default
            config.QuestionsPerStage.Should().Be(5);
            config.TimeLimit.Should().Be(15.0);
            config.ShowHints.Should().BeTrue();
            config.SoundEnabled.Should().BeTrue();
            config.MusicVolume.Should().Be(0.5);
            config.Theme.Should().Be("Rally");
            config.AutoSave.Should().BeTrue();
        }

        [Theory]
        [InlineData(DifficultyLevel.Rookie)]
        [InlineData(DifficultyLevel.Junior)]
        [InlineData(DifficultyLevel.Pro)]
        public void SetDifficulty_UpdatesRelatedProperties(DifficultyLevel difficulty)
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SetDifficulty(difficulty);

            // Assert
            config.SelectedDifficulty.Should().Be(difficulty);
            config.QuestionsPerStage.Should().Be(DifficultyManager.GetQuestionsPerStage(difficulty));
            config.TimeLimit.Should().Be(DifficultyManager.GetTimeLimit(difficulty));
        }

        [Theory]
        [InlineData(ProblemType.Addition)]
        [InlineData(ProblemType.Subtraction)]
        [InlineData(ProblemType.Multiplication)]
        [InlineData(ProblemType.Division)]
        public void SetMathType_UpdatesMathType(ProblemType mathType)
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SetMathType(mathType);

            // Assert
            config.SelectedMathType.Should().Be(mathType);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetShowHints_UpdatesShowHints(bool showHints)
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SetShowHints(showHints);

            // Assert
            config.ShowHints.Should().Be(showHints);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetSoundEnabled_UpdatesSoundEnabled(bool soundEnabled)
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SetSoundEnabled(soundEnabled);

            // Assert
            config.SoundEnabled.Should().Be(soundEnabled);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.5)]
        [InlineData(1.0)]
        public void SetMusicVolume_ValidValues_UpdatesVolume(double volume)
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SetMusicVolume(volume);

            // Assert
            config.MusicVolume.Should().Be(volume);
        }

        [Theory]
        [InlineData(-0.1)]
        [InlineData(1.1)]
        [InlineData(2.0)]
        public void SetMusicVolume_InvalidValues_ClampsToValidRange(double volume)
        {
            // Arrange
            var config = new GameConfiguration();
            var originalVolume = config.MusicVolume;

            // Act
            config.SetMusicVolume(volume);

            // Assert
            config.MusicVolume.Should().BeInRange(0.0, 1.0);
        }

        [Theory]
        [InlineData("Rally")]
        [InlineData("Speed")]
        [InlineData("Classic")]
        public void SetTheme_UpdatesTheme(string theme)
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SetTheme(theme);

            // Assert
            config.Theme.Should().Be(theme);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetAutoSave_UpdatesAutoSave(bool autoSave)
        {
            // Arrange
            var config = new GameConfiguration();

            // Act
            config.SetAutoSave(autoSave);

            // Assert
            config.AutoSave.Should().Be(autoSave);
        }

        [Fact]
        public void GetAllowedOperations_ReturnsCorrectOperationsForDifficulty()
        {
            // Arrange
            var config = new GameConfiguration();
            config.SetDifficulty(DifficultyLevel.Junior);

            // Act
            var operations = config.GetAllowedOperations();

            // Assert
            var expected = DifficultyManager.GetAllowedProblemTypes(DifficultyLevel.Junior);
            operations.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void IsOperationAllowed_ValidOperation_ReturnsTrue()
        {
            // Arrange
            var config = new GameConfiguration();
            config.SetDifficulty(DifficultyLevel.Junior);

            // Act & Assert
            config.IsOperationAllowed(ProblemType.Addition).Should().BeTrue();
            config.IsOperationAllowed(ProblemType.Subtraction).Should().BeTrue();
            config.IsOperationAllowed(ProblemType.Multiplication).Should().BeTrue();
        }

        [Fact]
        public void IsOperationAllowed_InvalidOperation_ReturnsFalse()
        {
            // Arrange
            var config = new GameConfiguration();
            config.SetDifficulty(DifficultyLevel.Rookie); // Only allows Addition and Subtraction

            // Act & Assert
            config.IsOperationAllowed(ProblemType.Division).Should().BeFalse();
        }

        [Fact]
        public void GetPointsForCorrectAnswer_ReturnsBasePointsForDifficulty()
        {
            // Arrange
            var config = new GameConfiguration();
            config.SetDifficulty(DifficultyLevel.Pro);

            // Act
            var points = config.GetPointsForCorrectAnswer();

            // Assert
            points.Should().Be(DifficultyManager.GetBasePoints(DifficultyLevel.Pro));
        }
    }
}
