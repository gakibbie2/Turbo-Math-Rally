using FluentAssertions;
using TurboMathRally.Core;
using TurboMathRally.Math;

namespace TurboMathRally.Tests.Core
{
    /// <summary>
    /// Updated tests for UserProfile functionality
    /// </summary>
    public class UserProfileTestsFixed
    {
        [Fact]
        public void UserProfile_DefaultConstructor_SetsDefaultValues()
        {
            // Act
            var profile = new UserProfile();

            // Assert
            profile.PlayerName.Should().Be("Young Racer");
            profile.Settings.Should().NotBeNull();
            profile.AchievementData.Should().NotBeNull();
            profile.OverallStats.Should().NotBeNull();
            profile.RallyData.Should().NotBeNull();
            profile.Version.Should().Be("1.0");
            profile.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void UserProfile_SetPlayerName_UpdatesCorrectly()
        {
            // Arrange
            var profile = new UserProfile();
            const string testName = "Test Player";

            // Act
            profile.PlayerName = testName;

            // Assert
            profile.PlayerName.Should().Be(testName);
        }

        [Fact]
        public void UserProfile_Settings_CanBeModified()
        {
            // Arrange
            var profile = new UserProfile();
            
            // Act
            profile.Settings.PreferredDifficulty = DifficultyLevel.Pro;

            // Assert
            profile.Settings.PreferredDifficulty.Should().Be(DifficultyLevel.Pro);
        }

        [Fact]
        public void UserProfile_AchievementData_InitializesWithDefaults()
        {
            // Arrange & Act
            var profile = new UserProfile();

            // Assert
            profile.AchievementData.Should().NotBeNull();
            profile.AchievementData.UnlockedAchievements.Should().NotBeNull();
            profile.AchievementData.ProgressValues.Should().NotBeNull();
        }

        [Fact]
        public void UserProfile_OverallStats_InitializesWithZeros()
        {
            // Arrange & Act
            var profile = new UserProfile();

            // Assert
            profile.OverallStats.Should().NotBeNull();
            profile.OverallStats.TotalQuestions.Should().Be(0);
            profile.OverallStats.CorrectAnswers.Should().Be(0);
        }
    }
}
