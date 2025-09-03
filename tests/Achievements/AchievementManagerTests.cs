namespace TurboMathRally.Tests.Achievements
{
    /// <summary>
    /// Tests for the AchievementManager class
    /// </summary>
    public class AchievementManagerTests
    {
        private readonly AchievementManager _achievementManager;
        private readonly Mock<ProfileManager> _mockProfileManager;
        private readonly UserProfile _testProfile;

        public AchievementManagerTests()
        {
            _mockProfileManager = new Mock<ProfileManager>();
            _testProfile = new UserProfile
            {
                Name = "Test User",
                AchievementData = new AchievementData()
            };
            _mockProfileManager.Setup(p => p.CurrentProfile).Returns(_testProfile);
            _achievementManager = new AchievementManager(_mockProfileManager.Object);
        }

        [Fact]
        public void Constructor_InitializesAllAchievements()
        {
            // Act
            var manager = new AchievementManager();

            // Assert
            manager.AllAchievements.Should().NotBeEmpty();
            manager.AllAchievements.Should().HaveCountGreaterThan(10); // We have many achievements
        }

        [Fact]
        public void GetAchievementsByType_ReturnsCorrectAchievements()
        {
            // Act
            var accuracyAchievements = _achievementManager.GetAchievementsByType(AchievementType.Accuracy);
            var streakAchievements = _achievementManager.GetAchievementsByType(AchievementType.Streak);

            // Assert
            accuracyAchievements.Should().NotBeEmpty();
            accuracyAchievements.Should().AllSatisfy(a => a.Type.Should().Be(AchievementType.Accuracy));
            
            streakAchievements.Should().NotBeEmpty();
            streakAchievements.Should().AllSatisfy(a => a.Type.Should().Be(AchievementType.Streak));
        }

        [Fact]
        public void UnlockedAchievements_ReturnsOnlyUnlockedAchievements()
        {
            // Arrange
            var firstAchievement = _achievementManager.AllAchievements.First();
            firstAchievement.Unlock();

            // Act
            var unlockedAchievements = _achievementManager.UnlockedAchievements;

            // Assert
            unlockedAchievements.Should().HaveCount(1);
            unlockedAchievements.Should().Contain(firstAchievement);
        }

        [Fact]
        public void LockedAchievements_ReturnsOnlyLockedAchievements()
        {
            // Arrange
            var firstAchievement = _achievementManager.AllAchievements.First();
            var totalCount = _achievementManager.AllAchievements.Count;
            firstAchievement.Unlock();

            // Act
            var lockedAchievements = _achievementManager.LockedAchievements;

            // Assert
            lockedAchievements.Should().HaveCount(totalCount - 1);
            lockedAchievements.Should().NotContain(firstAchievement);
        }

        [Fact]
        public void CheckAchievements_FirstPerfectAccuracy_UnlocksAchievement()
        {
            // Arrange
            var gameConfig = new GameConfiguration();
            var stats = new GameStatistics
            {
                TotalQuestions = 5,
                CorrectAnswers = 5,
                AccuracyPercentage = 100.0
            };

            // Act
            _achievementManager.CheckAchievements(stats, gameConfig);

            // Assert
            var firstPerfectAchievement = _achievementManager.AllAchievements
                .First(a => a.Id == "first_100");
            firstPerfectAchievement.IsUnlocked.Should().BeTrue();
        }

        [Fact]
        public void CheckAchievements_StreakAchievement_UnlocksWhenStreakReached()
        {
            // Arrange
            var gameConfig = new GameConfiguration();
            var stats = new GameStatistics
            {
                BestStreak = 5
            };

            // Act
            _achievementManager.CheckAchievements(stats, gameConfig);

            // Assert
            var streakAchievement = _achievementManager.AllAchievements
                .First(a => a.Id == "streak_5");
            streakAchievement.IsUnlocked.Should().BeTrue();
        }

        [Fact]
        public void CheckAchievements_SeriesCompletion_UnlocksCorrectSeriesAchievement()
        {
            // Arrange
            var gameConfig = new GameConfiguration();
            gameConfig.SetDifficulty(DifficultyLevel.Junior);
            var stats = new GameStatistics
            {
                StagesCompleted = 1
            };

            // Act
            _achievementManager.CheckAchievements(stats, gameConfig);

            // Assert
            var juniorChampion = _achievementManager.AllAchievements
                .First(a => a.Id == "junior_champion");
            juniorChampion.IsUnlocked.Should().BeTrue();
        }

        [Fact]
        public void CheckSessionAchievements_OnlyChecksRelevantAchievements()
        {
            // Arrange
            var gameConfig = new GameConfiguration();
            var sessionStats = new GameStatistics
            {
                BestStreak = 3 // Not enough for streak achievement
            };
            var combinedStats = new GameStatistics
            {
                BestStreak = 5 // Enough for streak achievement
            };

            // Act
            _achievementManager.CheckSessionAchievements(sessionStats, combinedStats, gameConfig);

            // Assert
            var streakAchievement = _achievementManager.AllAchievements
                .First(a => a.Id == "streak_5");
            streakAchievement.IsUnlocked.Should().BeTrue();
        }

        [Fact]
        public void UpdateProfileManager_ReloadsAchievementProgress()
        {
            // Arrange
            var newProfile = new UserProfile
            {
                Name = "New User",
                AchievementData = new AchievementData()
            };
            newProfile.AchievementData.UnlockedAchievements["first_100"] = true;
            
            var newProfileManager = new Mock<ProfileManager>();
            newProfileManager.Setup(p => p.CurrentProfile).Returns(newProfile);

            // Act
            _achievementManager.UpdateProfileManager(newProfileManager.Object);

            // Assert
            var firstPerfectAchievement = _achievementManager.AllAchievements
                .First(a => a.Id == "first_100");
            firstPerfectAchievement.IsUnlocked.Should().BeTrue();
        }

        [Fact]
        public void SaveProgress_UpdatesProfileData()
        {
            // Arrange
            var achievement = _achievementManager.AllAchievements.First();
            achievement.Unlock();

            // Act
            _achievementManager.SaveProgress();

            // Assert
            _testProfile.AchievementData.UnlockedAchievements.Should()
                .ContainKey(achievement.Id)
                .WhoseValue.Should().BeTrue();
            _testProfile.AchievementData.TotalPoints.Should().Be(achievement.Points);
        }

        [Fact]
        public void AchievementUnlocked_Event_FiresWhenAchievementUnlocked()
        {
            // Arrange
            Achievement? unlockedAchievement = null;
            _achievementManager.AchievementUnlocked += (sender, achievement) => 
            {
                unlockedAchievement = achievement;
            };

            var gameConfig = new GameConfiguration();
            var stats = new GameStatistics
            {
                TotalQuestions = 5,
                CorrectAnswers = 5,
                AccuracyPercentage = 100.0
            };

            // Act
            _achievementManager.CheckAchievements(stats, gameConfig);

            // Assert
            unlockedAchievement.Should().NotBeNull();
            unlockedAchievement!.Id.Should().Be("first_100");
        }

        [Theory]
        [InlineData("first_100", AchievementType.Accuracy)]
        [InlineData("streak_5", AchievementType.Streak)]
        [InlineData("junior_champion", AchievementType.Series)]
        [InlineData("speed_demon", AchievementType.Speed)]
        public void GetAchievementById_ReturnsCorrectAchievement(string achievementId, AchievementType expectedType)
        {
            // Act
            var achievement = _achievementManager.GetAchievementById(achievementId);

            // Assert
            achievement.Should().NotBeNull();
            achievement!.Id.Should().Be(achievementId);
            achievement.Type.Should().Be(expectedType);
        }

        [Fact]
        public void GetAchievementById_InvalidId_ReturnsNull()
        {
            // Act
            var achievement = _achievementManager.GetAchievementById("nonexistent_achievement");

            // Assert
            achievement.Should().BeNull();
        }

        [Fact]
        public void GetTotalPoints_SumsUnlockedAchievementPoints()
        {
            // Arrange
            var achievement1 = _achievementManager.AllAchievements.First();
            var achievement2 = _achievementManager.AllAchievements.Skip(1).First();
            
            achievement1.Points = 10;
            achievement2.Points = 15;
            
            achievement1.Unlock();
            achievement2.Unlock();

            // Act
            var totalPoints = _achievementManager.GetTotalPoints();

            // Assert
            totalPoints.Should().Be(25);
        }

        [Fact]
        public void GetCompletionPercentage_CalculatesCorrectly()
        {
            // Arrange
            var totalAchievements = _achievementManager.AllAchievements.Count;
            var achievement = _achievementManager.AllAchievements.First();
            achievement.Unlock();

            // Act
            var completionPercentage = _achievementManager.GetCompletionPercentage();

            // Assert
            var expectedPercentage = (1.0 / totalAchievements) * 100;
            completionPercentage.Should().BeApproximately(expectedPercentage, 0.1);
        }
    }
}
