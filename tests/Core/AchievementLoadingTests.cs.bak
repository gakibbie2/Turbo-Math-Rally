using FluentAssertions;
using TurboMathRally.Core;
using TurboMathRally.Core.Achievements;

namespace TurboMathRally.Tests.Core
{
    public class AchievementLoadingTests
    {
        [Fact]
        public void AchievementManager_AfterProfileSwitch_LoadsCorrectAchievements()
        {
            // Arrange
            var profileManager = new ProfileManager();
            var profile1 = new UserProfile { PlayerName = "TestPlayer1" };
            var profile2 = new UserProfile { PlayerName = "TestPlayer2" };
            
            // Unlock an achievement in profile1
            profile1.AchievementData.UnlockedAchievements["getting_started"] = true;
            
            // Act - Switch to profile1
            profileManager.GetType().GetProperty("CurrentProfile")?.SetValue(profileManager, profile1);
            profileManager.AchievementManager.UpdateProfileManager(profileManager);
            var unlockedCount1 = profileManager.AchievementManager.UnlockedAchievements.Count;
            
            // Switch to profile2 (no achievements)  
            profileManager.GetType().GetProperty("CurrentProfile")?.SetValue(profileManager, profile2);
            profileManager.AchievementManager.UpdateProfileManager(profileManager);
            var unlockedCount2 = profileManager.AchievementManager.UnlockedAchievements.Count;
            
            // Switch back to profile1
            profileManager.GetType().GetProperty("CurrentProfile")?.SetValue(profileManager, profile1);
            profileManager.AchievementManager.UpdateProfileManager(profileManager);
            var unlockedCount3 = profileManager.AchievementManager.UnlockedAchievements.Count;
            
            // Assert
            unlockedCount1.Should().BeGreaterThan(0, "Profile1 should have unlocked achievements");
            unlockedCount2.Should().Be(0, "Profile2 should have no unlocked achievements");
            unlockedCount3.Should().Be(unlockedCount1, "Profile1 achievements should be restored when switching back");
        }
        
        [Fact]
        public void AchievementManager_UpdateProfileManager_ReloadsAchievementData()
        {
            // Arrange
            var profileManager = new ProfileManager();
            var profile = new UserProfile { PlayerName = "TestPlayer" };
            
            // Initially no achievements
            profileManager.GetType().GetProperty("CurrentProfile")?.SetValue(profileManager, profile);
            profileManager.AchievementManager.UpdateProfileManager(profileManager);
            var initialCount = profileManager.AchievementManager.UnlockedAchievements.Count;
            
            // Unlock an achievement directly in the profile using a valid achievement ID
            profile.AchievementData.UnlockedAchievements["getting_started"] = true;
            
            // Act - Update the achievement manager
            profileManager.AchievementManager.UpdateProfileManager(profileManager);
            var updatedCount = profileManager.AchievementManager.UnlockedAchievements.Count;
            
            // Assert
            initialCount.Should().Be(0, "Initially should have no achievements");
            updatedCount.Should().BeGreaterThan(initialCount, "Should reflect the newly unlocked achievement");
        }
    }
}
