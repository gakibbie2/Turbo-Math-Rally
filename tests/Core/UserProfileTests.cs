using FluentAssertions;
using TurboMathRally.Core;
using TurboMathRally.Math;

namespace TurboMathRally.Tests.Core
{
    public class UserProfileTests
    {
        [Fact]
        public void UserProfile_DefaultConstruction_HasValidDefaults()
        {
            // Act
            var profile = new UserProfile();
            
            // Assert
            profile.Should().NotBeNull();
            profile.PlayerName.Should().Be("Young Racer");
            profile.Settings.Should().NotBeNull();
            profile.AchievementData.Should().NotBeNull();
            profile.OverallStats.Should().NotBeNull();
            profile.RallyData.Should().NotBeNull();
            profile.SessionHistory.Should().NotBeNull();
            profile.Version.Should().Be("1.0");
        }
        
        [Fact]
        public void UserProfile_Settings_HasValidDefaults()
        {
            // Act
            var profile = new UserProfile();
            
            // Assert
            var settings = profile.Settings;
            settings.SoundVolume.Should().Be(0.7f);
            settings.MusicVolume.Should().Be(0.5f);
            settings.ShowHints.Should().BeTrue();
            settings.ShowAchievementNotifications.Should().BeTrue();
            settings.PreferredDifficulty.Should().Be(DifficultyLevel.Junior);
            settings.PreferredMathType.Should().Be(MathOperation.Addition);
            settings.Theme.Should().Be("Rally");
        }
        
        [Fact]
        public void UserProfile_CreatedDate_IsReasonable()
        {
            // Act
            var profile = new UserProfile();
            
            // Assert
            profile.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
            profile.LastPlayedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
        }
        
        [Fact]
        public void UserProfile_CanModifyPlayerName()
        {
            // Arrange
            var profile = new UserProfile();
            
            // Act
            profile.PlayerName = "Test Racer";
            
            // Assert
            profile.PlayerName.Should().Be("Test Racer");
        }
    }
}
