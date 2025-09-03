using Xunit;
using FluentAssertions;
using TurboMathRally.Core;
using TurboMathRally.Math;
using TurboMathRally.Core.Achievements;

namespace TurboMathRally.Tests
{
    /// <summary>
    /// Comprehensive test suite for the current TurboMathRally implementation
    /// Tests core functionality, Windows Forms integration, and achievements
    /// </summary>
    public class ComprehensiveIntegrationTests
    {
        [Fact]
        public void UserProfile_DefaultInitialization_HasCorrectDefaults()
        {
            // Arrange & Act
            var profile = new UserProfile();

            // Assert
            profile.PlayerName.Should().Be("Young Racer");
            profile.Settings.Should().NotBeNull();
            profile.AchievementData.Should().NotBeNull();
            profile.OverallStats.Should().NotBeNull();
            profile.RallyData.Should().NotBeNull();
            profile.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));
            profile.Version.Should().Be("1.0");
        }

        [Fact]
        public void GameConfiguration_DefaultSettings_AreCorrect()
        {
            // Arrange & Act
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
        public void MathProblem_Creation_CalculatesCorrectAnswer()
        {
            // Arrange & Act
            var additionProblem = new MathProblem(MathOperation.Addition, 5, 3, DifficultyLevel.Junior);
            var subtractionProblem = new MathProblem(MathOperation.Subtraction, 10, 4, DifficultyLevel.Junior);
            var multiplicationProblem = new MathProblem(MathOperation.Multiplication, 6, 7, DifficultyLevel.Pro);
            var divisionProblem = new MathProblem(MathOperation.Division, 12, 3, DifficultyLevel.Pro);

            // Assert
            additionProblem.Answer.Should().Be(8);
            additionProblem.Question.Should().Be("5 + 3 = ?");
            additionProblem.IsCorrect(8).Should().BeTrue();
            additionProblem.IsCorrect(7).Should().BeFalse();

            subtractionProblem.Answer.Should().Be(6);
            multiplicationProblem.Answer.Should().Be(42);
            divisionProblem.Answer.Should().Be(4);
        }

        [Fact]
        public void GameSession_TrackingAnswers_UpdatesStatisticsCorrectly()
        {
            // Arrange
            var session = new GameSession();

            // Act - Simulate answering questions
            session.RecordAnswer(true, 1.5);  // Correct
            session.RecordAnswer(true, 2.0);  // Correct
            session.RecordAnswer(false, 3.0); // Incorrect
            session.RecordAnswer(true, 1.2);  // Correct

            // Assert
            session.TotalQuestions.Should().Be(4);
            session.CorrectAnswers.Should().Be(3);
            session.AccuracyPercentage.Should().Be(75.0);
            session.CurrentStreak.Should().Be(1); // Reset after wrong answer
            session.BestStreak.Should().Be(2);   // Best streak was 2
        }

        [Fact]
        public void CarBreakdownSystem_ProgressiveStrikes_WorksCorrectly()
        {
            // Arrange
            var carSystem = new CarBreakdownSystem();

            // Act & Assert - First strike
            var firstStrike = carSystem.AddStrike();
            firstStrike.StrikeCount.Should().Be(1);
            firstStrike.IsBrokenDown.Should().BeFalse();
            carSystem.IsBrokenDown.Should().BeFalse();

            // Second strike
            var secondStrike = carSystem.AddStrike();
            secondStrike.StrikeCount.Should().Be(2);
            secondStrike.IsBrokenDown.Should().BeFalse();

            // Third strike - breakdown
            var thirdStrike = carSystem.AddStrike();
            thirdStrike.StrikeCount.Should().Be(3);
            thirdStrike.IsBrokenDown.Should().BeTrue();
            carSystem.IsBrokenDown.Should().BeTrue();
        }

        [Fact]
        public void ProblemGenerator_GeneratesValidProblems()
        {
            // Arrange
            var generator = new ProblemGenerator();

            // Act
            var problems = new List<MathProblem>();
            for (int i = 0; i < 10; i++)
            {
                var problem = generator.GenerateProblem(MathOperation.Addition, DifficultyLevel.Junior);
                problems.Add(problem);
            }

            // Assert
            problems.Should().HaveCount(10);
            problems.Should().OnlyContain(p => p.Operation == MathOperation.Addition);
            problems.Should().OnlyContain(p => p.Difficulty == DifficultyLevel.Junior);
            problems.Should().OnlyContain(p => p.Answer > 0);
            
            // Verify answers are calculated correctly
            foreach (var problem in problems)
            {
                problem.Answer.Should().Be(problem.Operand1 + problem.Operand2);
            }
        }

        [Fact]
        public void AchievementManager_InitializesCorrectly()
        {
            // Arrange & Act
            var achievementManager = new AchievementManager();

            // Assert
            achievementManager.Should().NotBeNull();
            achievementManager.AllAchievements.Should().NotBeEmpty();
            // AchievementManager should load achievements from initialization
        }

        [Fact]
        public async Task ProfileManager_LoadDefaultProfile_WorksCorrectly()
        {
            // Arrange
            var profileManager = new ProfileManager();

            try
            {
                // Act - Load default profile (creates one if doesn't exist)
                var profile = await profileManager.LoadDefaultProfileAsync();

                // Assert
                profile.Should().NotBeNull();
                profile.PlayerName.Should().NotBeNullOrEmpty();
                profile.Settings.Should().NotBeNull();
                profile.AchievementData.Should().NotBeNull();
                profile.OverallStats.Should().NotBeNull();

                // Profile should be accessible
                profileManager.CurrentProfile.Should().NotBeNull();
                profileManager.CurrentProfile.Should().Be(profile);
            }
            finally
            {
                // Cleanup is automatic - default profile will be reused
            }
        }

        [Fact]
        public void DifficultyLevel_HasCorrectOrder()
        {
            // Assert - Verify enum ordering
            ((int)DifficultyLevel.Rookie).Should().Be(1);
            ((int)DifficultyLevel.Junior).Should().Be(2);
            ((int)DifficultyLevel.Pro).Should().Be(3);
        }

        [Fact]
        public void StoryProblem_CreatesValidNarrative()
        {
            // Arrange
            var generator = new StoryProblemGenerator();

            // Act
            var storyProblem = generator.GenerateRepairStoryProblem(DifficultyLevel.Junior, MathOperation.Addition);

            // Assert
            storyProblem.Should().NotBeNull();
            storyProblem.StoryText.Should().NotBeNullOrEmpty();
            storyProblem.Answer.Should().BeGreaterThan(0);
            storyProblem.Operation.Should().Be(MathOperation.Addition);
            storyProblem.Difficulty.Should().Be(DifficultyLevel.Junior);
        }

        [Theory]
        [InlineData(MathOperation.Addition, "+")]
        [InlineData(MathOperation.Subtraction, "-")]
        [InlineData(MathOperation.Multiplication, "×")]
        [InlineData(MathOperation.Division, "÷")]
        public void MathProblem_OperationSymbols_AreCorrect(MathOperation operation, string expectedSymbol)
        {
            // Arrange & Act
            var problem = new MathProblem(operation, 10, 5, DifficultyLevel.Junior);

            // Assert
            problem.OperationSymbol.Should().Be(expectedSymbol);
        }

        [Fact]
        public void GameSession_Performance_CanHandleManyOperations()
        {
            // Arrange
            var session = new GameSession();
            var random = new Random(42); // Fixed seed for reproducible tests

            // Act - Record many answers quickly
            var startTime = DateTime.UtcNow;
            for (int i = 0; i < 1000; i++)
            {
                session.RecordAnswer(random.Next(2) == 0, random.NextDouble() * 5);
            }
            var endTime = DateTime.UtcNow;

            // Assert
            session.TotalQuestions.Should().Be(1000);
            (endTime - startTime).TotalSeconds.Should().BeLessThan(1.0); // Should complete in under 1 second
        }

        [Fact]
        public void Achievement_SystemIntegration_WorksWithGameplay()
        {
            // Arrange
            var profileManager = new ProfileManager();
            var achievementManager = new AchievementManager(profileManager);
            var config = new GameConfiguration();

            // Act - Simulate some gameplay statistics using the achievement system's GameStatistics
            var stats = new TurboMathRally.Core.Achievements.GameStatistics
            {
                CorrectAnswers = 50,
                TotalQuestions = 60,
                AccuracyPercentage = 83.3,
                BestStreak = 15,
                CurrentStreak = 8,
                StagesCompleted = 3
            };

            // Simulate updating achievements
            achievementManager.CheckAchievements(stats, config);

            // Assert - Achievement system should process without errors
            achievementManager.Should().NotBeNull();
            achievementManager.AllAchievements.Should().NotBeEmpty();
        }
    }
}
