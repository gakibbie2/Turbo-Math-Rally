namespace TurboMathRally.Tests.Achievements
{
    /// <summary>
    /// Tests for the Achievement class
    /// </summary>
    public class AchievementTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            var achievement = new Achievement
            {
                Id = "test_achievement",
                Title = "Test Achievement",
                Description = "A test achievement",
                Icon = "🏆",
                Type = AchievementType.Accuracy,
                Rarity = AchievementRarity.Common,
                Points = 10,
                TargetValue = 5
            };

            // Assert
            achievement.Id.Should().Be("test_achievement");
            achievement.Title.Should().Be("Test Achievement");
            achievement.Description.Should().Be("A test achievement");
            achievement.Icon.Should().Be("🏆");
            achievement.Type.Should().Be(AchievementType.Accuracy);
            achievement.Rarity.Should().Be(AchievementRarity.Common);
            achievement.Points.Should().Be(10);
            achievement.TargetValue.Should().Be(5);
            achievement.IsUnlocked.Should().BeFalse(); // Default value
            achievement.UnlockedDate.Should().BeNull(); // Default value
            achievement.Progress.Should().Be(0.0); // Default value
            achievement.CurrentValue.Should().Be(0); // Default value
        }

        [Theory]
        [InlineData(AchievementRarity.Common, "Bronze")]
        [InlineData(AchievementRarity.Uncommon, "Silver")]
        [InlineData(AchievementRarity.Rare, "Gold")]
        [InlineData(AchievementRarity.Epic, "Platinum")]
        [InlineData(AchievementRarity.Legendary, "Diamond")]
        public void GetRarityName_ReturnsCorrectName(AchievementRarity rarity, string expectedName)
        {
            // Arrange
            var achievement = new Achievement { Rarity = rarity };

            // Act
            var rarityName = achievement.GetRarityName();

            // Assert
            rarityName.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(AchievementRarity.Common, "🥉")]
        [InlineData(AchievementRarity.Uncommon, "🥈")]
        [InlineData(AchievementRarity.Rare, "🥇")]
        [InlineData(AchievementRarity.Epic, "💎")]
        [InlineData(AchievementRarity.Legendary, "👑")]
        public void GetRarityIcon_ReturnsCorrectIcon(AchievementRarity rarity, string expectedIcon)
        {
            // Arrange
            var achievement = new Achievement { Rarity = rarity };

            // Act
            var rarityIcon = achievement.GetRarityIcon();

            // Assert
            rarityIcon.Should().Be(expectedIcon);
        }

        [Fact]
        public void Unlock_SetsUnlockedStateAndDate()
        {
            // Arrange
            var achievement = new Achievement();
            var beforeUnlock = DateTime.Now;

            // Act
            achievement.Unlock();

            // Assert
            achievement.IsUnlocked.Should().BeTrue();
            achievement.UnlockedDate.Should().NotBeNull();
            achievement.UnlockedDate.Should().BeOnOrAfter(beforeUnlock);
            achievement.UnlockedDate.Should().BeOnOrBefore(DateTime.Now);
        }

        [Theory]
        [InlineData(0, 10, 0.0)]
        [InlineData(5, 10, 0.5)]
        [InlineData(10, 10, 1.0)]
        [InlineData(15, 10, 1.0)] // Should cap at 1.0
        public void UpdateProgress_CalculatesProgressCorrectly(int currentValue, int targetValue, double expectedProgress)
        {
            // Arrange
            var achievement = new Achievement
            {
                TargetValue = targetValue
            };

            // Act
            achievement.UpdateProgress(currentValue);

            // Assert
            achievement.CurrentValue.Should().Be(currentValue);
            achievement.Progress.Should().BeApproximately(expectedProgress, 0.001);
        }

        [Fact]
        public void UpdateProgress_ZeroTargetValue_SetsProgressToZero()
        {
            // Arrange
            var achievement = new Achievement
            {
                TargetValue = 0
            };

            // Act
            achievement.UpdateProgress(5);

            // Assert
            achievement.CurrentValue.Should().Be(5);
            achievement.Progress.Should().Be(0.0);
        }

        [Fact]
        public void UpdateProgress_CompletesAchievement_UnlocksAchievement()
        {
            // Arrange
            var achievement = new Achievement
            {
                TargetValue = 5
            };

            // Act
            achievement.UpdateProgress(5);

            // Assert
            achievement.IsUnlocked.Should().BeTrue();
            achievement.UnlockedDate.Should().NotBeNull();
            achievement.Progress.Should().Be(1.0);
        }

        [Fact]
        public void IsProgressComplete_ReturnsCorrectValue()
        {
            // Arrange
            var achievement = new Achievement
            {
                TargetValue = 10
            };

            // Act & Assert
            achievement.UpdateProgress(5);
            achievement.IsProgressComplete().Should().BeFalse();

            achievement.UpdateProgress(10);
            achievement.IsProgressComplete().Should().BeTrue();

            achievement.UpdateProgress(15);
            achievement.IsProgressComplete().Should().BeTrue();
        }

        [Fact]
        public void GetDisplayText_FormatsCorrectly()
        {
            // Arrange
            var achievement = new Achievement
            {
                Title = "Test Achievement",
                Description = "A test achievement",
                Rarity = AchievementRarity.Rare,
                Points = 25,
                IsUnlocked = true
            };

            // Act
            var displayText = achievement.GetDisplayText();

            // Assert
            displayText.Should().Contain("Test Achievement");
            displayText.Should().Contain("A test achievement");
            displayText.Should().Contain("Gold");
            displayText.Should().Contain("25");
            displayText.Should().Contain("UNLOCKED");
        }

        [Fact]
        public void GetDisplayText_LockedAchievement_ShowsProgress()
        {
            // Arrange
            var achievement = new Achievement
            {
                Title = "Test Achievement",
                Description = "A test achievement",
                Rarity = AchievementRarity.Common,
                Points = 10,
                CurrentValue = 3,
                TargetValue = 5,
                IsUnlocked = false
            };
            achievement.UpdateProgress(3);

            // Act
            var displayText = achievement.GetDisplayText();

            // Assert
            displayText.Should().Contain("3/5");
            displayText.Should().Contain("60%");
        }
    }
}
