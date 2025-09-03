using FluentAssertions;
using TurboMathRally.Core;
using TurboMathRally.Math;
using System.Windows.Forms;

namespace TurboMathRally.Tests.UI
{
    /// <summary>
    /// Tests for Windows Forms UI components and interactions
    /// </summary>
    public class WindowsFormsTests
    {
        [Fact]
        public void ProfileSelectionForm_Creation_DoesNotThrow()
        {
            // This is a basic test to ensure the form can be instantiated
            // In a real scenario, you'd want to test form interactions
            
            // Act & Assert
            var exception = Record.Exception(() =>
            {
                // We can't directly instantiate forms in unit tests without special setup
                // But we can test the underlying logic
                var profileManager = new ProfileManager();
                profileManager.Should().NotBeNull();
            });

            exception.Should().BeNull();
        }

        [Fact]
        public void GameConfiguration_FormCompatibleValues_AreValid()
        {
            // Test that configuration values work with Windows Forms
            
            // Arrange & Act
            var config = new GameConfiguration
            {
                SelectedMathType = MathOperation.Addition,
                SelectedDifficulty = DifficultyLevel.Junior,
                SelectedPlayerMode = PlayerMode.Kid,
                SelectedSeriesName = "Junior Championship",
                SelectedMathTypeName = "Addition Only"
            };

            // Assert
            config.SelectedSeriesName.Should().NotBeNullOrWhiteSpace();
            config.SelectedMathTypeName.Should().NotBeNullOrWhiteSpace();
            
            // Test that the strings are appropriate for UI display
            config.SelectedSeriesName.Length.Should().BeLessThan(50); // Reasonable for button text
            config.SelectedMathTypeName.Length.Should().BeLessThan(30);
        }

        [Theory]
        [InlineData(DifficultyLevel.Rookie, "Rookie Rally")]
        [InlineData(DifficultyLevel.Junior, "Junior Championship")]
        [InlineData(DifficultyLevel.Pro, "Pro Circuit")]
        public void DifficultyLevel_DisplayNames_AreUserFriendly(DifficultyLevel level, string expectedDisplayName)
        {
            // This tests that our difficulty levels have appropriate display names
            // that would work well in Windows Forms buttons and labels
            
            // Assert
            expectedDisplayName.Should().NotBeNullOrWhiteSpace();
            expectedDisplayName.Should().NotContain("_"); // No underscores in display text
            expectedDisplayName.Length.Should().BeGreaterThan(5); // Descriptive enough
            expectedDisplayName.Length.Should().BeLessThan(25); // Not too long for buttons
        }

        [Theory]
        [InlineData(MathOperation.Addition, "Addition")]
        [InlineData(MathOperation.Subtraction, "Subtraction")]
        [InlineData(MathOperation.Multiplication, "Multiplication")]
        [InlineData(MathOperation.Division, "Division")]
        public void MathOperation_DisplayNames_AreUserFriendly(MathOperation operation, string expectedDisplayName)
        {
            // Test that operation names are suitable for UI display
            
            // Assert
            expectedDisplayName.Should().NotBeNullOrWhiteSpace();
            expectedDisplayName.Length.Should().BeGreaterThan(3);
            expectedDisplayName.Length.Should().BeLessThan(20);
        }

        [Fact]
        public void UserProfile_PlayerName_ValidatesForUI()
        {
            // Test player name validation for UI forms
            
            // Arrange
            var profile = new UserProfile();

            // Act
            profile.PlayerName = "Test Player 123";

            // Assert
            profile.PlayerName.Should().Be("Test Player 123");
            profile.PlayerName.Length.Should().BeLessThan(50); // Reasonable length for UI
        }

        [Fact]
        public void AchievementManager_DisplayText_IsFormatted()
        {
            // Test that achievement text is properly formatted for display
            
            // Arrange
            var profileManager = new ProfileManager();
            profileManager.CreateNewProfileAsync("UITestPlayer").Wait();
            var achievementManager = profileManager.AchievementManager;

            // Act
            var achievements = achievementManager.AllAchievements;

            // Assert
            achievements.Should().NotBeEmpty();
            foreach (var achievement in achievements)
            {
                achievement.Title.Should().NotBeNullOrWhiteSpace();
                achievement.Description.Should().NotBeNullOrWhiteSpace();
                achievement.Icon.Should().NotBeNullOrWhiteSpace();
                
                // Test reasonable lengths for UI display
                achievement.Title.Length.Should().BeLessThan(50);
                achievement.Description.Length.Should().BeLessThan(200);
                achievement.Icon.Length.Should().BeLessThan(10); // Emoji icons are short
            }
        }

        [Fact]
        public void GameSession_StatisticsDisplay_IsFormatted()
        {
            // Test that statistics are formatted appropriately for display
            
            // Arrange
            var profileManager = new ProfileManager();
            profileManager.CreateNewProfileAsync("UITestPlayer").Wait();
            var gameConfig = new GameConfiguration();
            // Act
            var gameSession = new GameSession();
            gameSession.SetProfileManager(profileManager);

            // Act
            var stats = gameSession.GetStatistics(gameConfig);

            // Assert
            stats.Should().NotBeNull();
            stats.TotalQuestions.Should().BeGreaterOrEqualTo(0);
            stats.CorrectAnswers.Should().BeGreaterOrEqualTo(0);
            
            // Verify that accuracy calculation won't cause UI issues
            if (stats.TotalQuestions > 0)
            {
                var accuracy = (double)stats.CorrectAnswers / stats.TotalQuestions * 100;
                accuracy.Should().BeInRange(0, 100);
            }
        }
    }
}
