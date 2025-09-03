namespace TurboMathRally.Tests.Core
{
    /// <summary>
    /// Tests for the GameSession class
    /// </summary>
    public class GameSessionTests
    {
        private readonly GameSession _gameSession;
        private readonly Mock<ProfileManager> _mockProfileManager;

        public GameSessionTests()
        {
            _mockProfileManager = new Mock<ProfileManager>();
            _gameSession = new GameSession(_mockProfileManager.Object);
        }

        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange & Act
            var session = new GameSession();

            // Assert
            session.TotalQuestions.Should().Be(0);
            session.CorrectAnswers.Should().Be(0);
            session.AccuracyPercentage.Should().Be(0);
            session.CurrentStreak.Should().Be(0);
            session.BestStreak.Should().Be(0);
            session.StagesCompleted.Should().Be(0);
            session.ComebacksAchieved.Should().Be(0);
            session.AchievementManager.Should().NotBeNull();
        }

        [Fact]
        public void RecordAnswer_CorrectAnswer_UpdatesStatistics()
        {
            // Act
            _gameSession.RecordAnswer(true, 2.5);

            // Assert
            _gameSession.TotalQuestions.Should().Be(1);
            _gameSession.CorrectAnswers.Should().Be(1);
            _gameSession.AccuracyPercentage.Should().Be(100.0);
            _gameSession.CurrentStreak.Should().Be(1);
            _gameSession.BestStreak.Should().Be(1);
        }

        [Fact]
        public void RecordAnswer_IncorrectAnswer_UpdatesStatistics()
        {
            // Act
            _gameSession.RecordAnswer(false, 3.0);

            // Assert
            _gameSession.TotalQuestions.Should().Be(1);
            _gameSession.CorrectAnswers.Should().Be(0);
            _gameSession.AccuracyPercentage.Should().Be(0.0);
            _gameSession.CurrentStreak.Should().Be(0);
            _gameSession.BestStreak.Should().Be(0);
        }

        [Fact]
        public void RecordAnswer_MixedAnswers_CalculatesAccuracyCorrectly()
        {
            // Act
            _gameSession.RecordAnswer(true, 2.0);   // 100%
            _gameSession.RecordAnswer(false, 3.0);  // 50%
            _gameSession.RecordAnswer(true, 1.5);   // 66.67%
            _gameSession.RecordAnswer(true, 2.2);   // 75%

            // Assert
            _gameSession.TotalQuestions.Should().Be(4);
            _gameSession.CorrectAnswers.Should().Be(3);
            _gameSession.AccuracyPercentage.Should().BeApproximately(75.0, 0.1);
        }

        [Fact]
        public void RecordAnswer_StreakBuilding_UpdatesBestStreak()
        {
            // Act
            _gameSession.RecordAnswer(true, 1.0);   // Streak: 1, Best: 1
            _gameSession.RecordAnswer(true, 1.0);   // Streak: 2, Best: 2
            _gameSession.RecordAnswer(true, 1.0);   // Streak: 3, Best: 3
            _gameSession.RecordAnswer(false, 1.0);  // Streak: 0, Best: 3
            _gameSession.RecordAnswer(true, 1.0);   // Streak: 1, Best: 3
            _gameSession.RecordAnswer(true, 1.0);   // Streak: 2, Best: 3

            // Assert
            _gameSession.CurrentStreak.Should().Be(2);
            _gameSession.BestStreak.Should().Be(3);
        }

        [Fact]
        public void RecordStageCompletion_IncrementsStagesCompleted()
        {
            // Act
            _gameSession.RecordStageCompletion();
            _gameSession.RecordStageCompletion();

            // Assert
            _gameSession.StagesCompleted.Should().Be(2);
        }

        [Fact]
        public void AverageResponseTime_CalculatesCorrectly()
        {
            // Act
            _gameSession.RecordAnswer(true, 2.0);
            _gameSession.RecordAnswer(true, 4.0);
            _gameSession.RecordAnswer(true, 3.0);

            // Assert
            _gameSession.AverageResponseTime.Should().BeApproximately(3.0, 0.1);
        }

        [Fact]
        public void AverageResponseTime_NoAnswers_ReturnsZero()
        {
            // Assert
            _gameSession.AverageResponseTime.Should().Be(0.0);
        }

        [Fact]
        public void RecordAnswer_VeryFastResponse_ClampedToMinimum()
        {
            // Act
            _gameSession.RecordAnswer(true, 0.05); // Below minimum

            // Assert
            _gameSession.AverageResponseTime.Should().BeGreaterOrEqualTo(0.1);
        }

        [Fact]
        public void GetStatistics_ReturnsCorrectStatistics()
        {
            // Arrange
            _gameSession.RecordAnswer(true, 2.0);
            _gameSession.RecordAnswer(false, 3.0);
            _gameSession.RecordAnswer(true, 1.5);
            _gameSession.RecordStageCompletion();

            // Act
            var stats = _gameSession.GetStatistics();

            // Assert
            stats.TotalQuestions.Should().Be(3);
            stats.CorrectAnswers.Should().Be(2);
            stats.AccuracyPercentage.Should().BeApproximately(66.67, 0.1);
            stats.BestStreak.Should().Be(1);
            stats.StagesCompleted.Should().Be(1);
            stats.AverageResponseTime.Should().BeApproximately(2.17, 0.1);
        }

        [Fact]
        public void UpdateProfileManager_UpdatesProfileManagerAndReloadsAchievements()
        {
            // Arrange
            var newProfileManager = new Mock<ProfileManager>();
            
            // Act
            _gameSession.UpdateProfileManager(newProfileManager.Object);

            // Assert
            _gameSession.ProfileManager.Should().Be(newProfileManager.Object);
        }

        [Fact]
        public void Comeback_ThreeWrongThenThreeRight_RecordsComeback()
        {
            // Act - Record 3 wrong answers
            _gameSession.RecordAnswer(false, 2.0);
            _gameSession.RecordAnswer(false, 2.0);
            _gameSession.RecordAnswer(false, 2.0);
            
            // Record 3 correct answers (should trigger comeback)
            _gameSession.RecordAnswer(true, 2.0);
            _gameSession.RecordAnswer(true, 2.0);
            _gameSession.RecordAnswer(true, 2.0);

            // Assert
            _gameSession.ComebacksAchieved.Should().Be(1);
            _gameSession.CurrentStreak.Should().Be(3);
        }

        [Fact]
        public void TotalPlayTime_ReturnsTimeSpanSinceSessionStart()
        {
            // Arrange
            var session = new GameSession();
            Thread.Sleep(10); // Small delay

            // Act
            var playTime = session.TotalPlayTime;

            // Assert
            playTime.Should().BeGreaterThan(TimeSpan.Zero);
            playTime.Should().BeLessThan(TimeSpan.FromSeconds(1)); // Should be very small
        }
    }
}
