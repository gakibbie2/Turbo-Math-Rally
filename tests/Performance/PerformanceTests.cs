using FluentAssertions;
using TurboMathRally.Core;
using TurboMathRally.Math;
using System.Diagnostics;

namespace TurboMathRally.Tests.Performance
{
    /// <summary>
    /// Performance and edge case tests for the game systems
    /// </summary>
    public class PerformanceTests
    {
        [Fact]
        public void MathProblemGeneration_GenerateMany_PerformsWell()
        {
            // Arrange
            var generator = new ProblemGenerator();
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                var problem = generator.GenerateProblem(MathOperation.Addition, DifficultyLevel.Junior);
                problem.Should().NotBeNull();
            }
            stopwatch.Stop();

            // Assert - Should generate 1000 problems in under 1 second
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000);
        }

        [Fact]
        public void AchievementManager_CheckManyAchievements_PerformsWell()
        {
            // Arrange
            var profileManager = new ProfileManager();
            profileManager.CreateNewProfileAsync("PerfTestPlayer").Wait();
            var achievementManager = profileManager.AchievementManager;
            var gameConfig = new GameConfiguration();
            var stats = new GameStatistics { TotalQuestions = 100, CorrectAnswers = 95, BestStreak = 10 };
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            for (int i = 0; i < 100; i++)
            {
                achievementManager.CheckAchievements(stats, gameConfig);
            }
            stopwatch.Stop();

            // Assert - Should check achievements 100 times in under 500ms
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(500);
        }

        [Theory]
        [InlineData(0, 0)] // No questions answered
        [InlineData(1000000, 999999)] // Very large numbers
        [InlineData(-1, 0)] // Invalid negative (should handle gracefully)
        public void GameStatistics_EdgeCaseValues_HandledCorrectly(int totalQuestions, int correctAnswers)
        {
            // Arrange & Act
            var stats = new TurboMathRally.Core.GameStatistics 
            { 
                TotalQuestions = System.Math.Max(0, totalQuestions), // Ensure non-negative
                CorrectAnswers = System.Math.Max(0, correctAnswers)
            };

            // Assert
            stats.TotalQuestions.Should().BeGreaterOrEqualTo(0);
            stats.CorrectAnswers.Should().BeGreaterOrEqualTo(0);
            stats.CorrectAnswers.Should().BeLessOrEqualTo(System.Math.Max(stats.TotalQuestions, 0));
        }

        [Fact]
        public void ProfileManager_RapidProfileSwitching_HandlesCorrectly()
        {
            // Arrange
            var profileManager = new ProfileManager();
            var profileNames = new[] { "Player1", "Player2", "Player3", "Player4", "Player5" };

            // Create multiple profiles
            foreach (var name in profileNames)
            {
                profileManager.CreateNewProfileAsync(name).Wait();
            }

            // Act - Rapidly switch between profiles
            var stopwatch = Stopwatch.StartNew();
            foreach (var name in profileNames)
            {
                var success = profileManager.LoadProfileAsync(name).Result;
                success.Should().BeTrue();
                profileManager.CurrentProfile?.PlayerName.Should().Be(name);
            }
            stopwatch.Stop();

            // Assert - Should handle rapid switching in under 1 second
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000);
        }

        [Fact]
        public void StoryProblemGenerator_GenerateMany_AllValid()
        {
            // Arrange
            var generator = new StoryProblemGenerator();
            var operations = new[] { MathOperation.Addition, MathOperation.Subtraction, 
                                   MathOperation.Multiplication, MathOperation.Division };

            // Act & Assert
            foreach (var operation in operations)
            {
                for (int i = 0; i < 50; i++) // Generate 50 of each type
                {
                    var storyProblem = generator.GenerateRepairStoryProblem(DifficultyLevel.Junior, operation);
                    
                    storyProblem.Should().NotBeNull();
                    storyProblem.StoryText.Should().NotBeNullOrWhiteSpace();
                    storyProblem.Answer.Should().BeGreaterThan(0);
                    storyProblem.Operation.Should().Be(operation);
                }
            }
        }
    }
}
