using FluentAssertions;
using TurboMathRally.Core;
using TurboMathRally.Math;

namespace TurboMathRally.Tests.Integration
{
    /// <summary>
    /// Integration tests for complete game workflows
    /// </summary>
    public class GameWorkflowIntegrationTests
    {
        [Fact]
        public async Task ProfileManager_CreateAndLoadProfile_WorksCorrectly()
        {
            // Arrange
            var profileManager = new ProfileManager();
            const string testProfileName = "TestPlayer";

            // Act
            var success = await profileManager.CreateNewProfileAsync(testProfileName);
            var loadedProfile = profileManager.CurrentProfile;

            // Assert
            success.Should().BeTrue();
            loadedProfile.Should().NotBeNull();
            loadedProfile!.PlayerName.Should().Be(testProfileName);
        }

        [Fact]
        public void AchievementManager_WithValidProfile_InitializesCorrectly()
        {
            // Arrange
            var profileManager = new ProfileManager();
            profileManager.CreateNewProfileAsync("TestPlayer").Wait();

            // Act
            var achievementManager = profileManager.AchievementManager;

            // Assert
            achievementManager.Should().NotBeNull();
            achievementManager.AllAchievements.Should().NotBeEmpty();
            achievementManager.AllAchievements.Count.Should().BeGreaterThan(20); // We have 30+ achievements
        }

        [Fact]
        public void GameSession_BasicWorkflow_CompletesSuccessfully()
        {
            // Arrange
            var profileManager = new ProfileManager();
            profileManager.CreateNewProfileAsync("TestPlayer").Wait();
            var gameConfig = new GameConfiguration
            {
                SelectedMathType = MathOperation.Addition,
                SelectedDifficulty = DifficultyLevel.Junior,
                SelectedPlayerMode = PlayerMode.Kid
            };

            // Act
            var gameSession = new GameSession();
            gameSession.SetProfileManager(profileManager);
            var initialStats = gameSession.GetStatistics(gameConfig);

            // Assert
            gameSession.Should().NotBeNull();
            gameSession.AchievementManager.Should().NotBeNull();
            initialStats.Should().NotBeNull();
            initialStats.TotalQuestions.Should().Be(0);
            initialStats.CorrectAnswers.Should().Be(0);
        }

        [Fact]
        public void MathProblemGeneration_AllOperations_WorksCorrectly()
        {
            // Arrange
            var operations = new[] { MathOperation.Addition, MathOperation.Subtraction, 
                                   MathOperation.Multiplication, MathOperation.Division };
            var generator = new ProblemGenerator();

            foreach (var operation in operations)
            {
                // Act
                var problem = generator.GenerateProblem(operation, DifficultyLevel.Junior);

                // Assert
                problem.Should().NotBeNull();
                problem.Operation.Should().Be(operation);
                problem.Answer.Should().BeGreaterThan(0);
                problem.Operand1.Should().BeGreaterThan(0);
                problem.Operand2.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public void DifficultyManager_NumberRanges_AreAppropriate()
        {
            // Test that difficulty levels produce appropriate number ranges
            var generator = new ProblemGenerator();
            
            // Test Junior level (should have smaller numbers)
            var juniorProblem = generator.GenerateProblem(MathOperation.Addition, DifficultyLevel.Junior);
            var proProblem = generator.GenerateProblem(MathOperation.Addition, DifficultyLevel.Pro);

            // Assert
            juniorProblem.Operand1.Should().BeLessThan(100); // Junior should be smaller
            juniorProblem.Operand2.Should().BeLessThan(100);
            
            // Pro can have larger numbers, but let's not assume specific ranges
            proProblem.Operand1.Should().BeGreaterThan(0);
            proProblem.Operand2.Should().BeGreaterThan(0);
        }
    }
}
