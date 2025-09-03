namespace TurboMathRally.Tests.Integration
{
    /// <summary>
    /// Integration tests that test multiple components working together
    /// </summary>
    public class GameFlowIntegrationTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly ProfileManager _profileManager;
        private readonly GameSession _gameSession;
        private readonly ProblemGenerator _problemGenerator;
        private readonly AnswerValidator _answerValidator;

        public GameFlowIntegrationTests()
        {
            // Setup test directory
            _testDirectory = Path.Combine(Path.GetTempPath(), "TurboMathRallyIntegrationTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectory);

            // Initialize components
            _profileManager = new ProfileManager();
            
            // Use reflection to set test directory
            var field = typeof(ProfileManager).GetField("_savesDirectory", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(_profileManager, _testDirectory);

            _gameSession = new GameSession(_profileManager);
            _problemGenerator = new ProblemGenerator();
            _answerValidator = new AnswerValidator();
        }

        public void Dispose()
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }

        [Fact]
        public async Task CompleteGameFlow_CreateProfile_PlayGame_SaveProgress()
        {
            // Arrange - Create a profile
            var profile = await _profileManager.CreateNewProfileAsync("IntegrationTest");
            profile.Should().NotBeNull();

            // Act - Play through a mini game session
            var gameConfig = new GameConfiguration();
            gameConfig.SetDifficulty(DifficultyLevel.Rookie);

            // Generate some problems and "answer" them
            var problems = _problemGenerator.GenerateMultiple(5, DifficultyLevel.Rookie, ProblemType.Addition);
            
            foreach (var problem in problems)
            {
                // Simulate answering correctly
                var result = _answerValidator.ValidateAnswer(problem.CorrectAnswer.ToString(), problem);
                _gameSession.RecordAnswer(result.IsCorrect, 2.0); // 2 second response time
            }

            // Complete a stage
            _gameSession.RecordStageCompletion();

            // Check achievements
            var stats = _gameSession.GetStatistics();
            _gameSession.AchievementManager.CheckAchievements(stats, gameConfig);

            // Save the profile
            await _profileManager.SaveCurrentProfileAsync();

            // Assert - Verify everything was recorded correctly
            var savedProfile = _profileManager.CurrentProfile!;
            savedProfile.Name.Should().Be("IntegrationTest");
            
            stats.TotalQuestions.Should().Be(5);
            stats.CorrectAnswers.Should().Be(5);
            stats.AccuracyPercentage.Should().Be(100.0);
            stats.StagesCompleted.Should().Be(1);

            // Should have unlocked the "First Perfect" achievement
            var firstPerfectAchievement = _gameSession.AchievementManager.GetAchievementById("first_100");
            firstPerfectAchievement.Should().NotBeNull();
            firstPerfectAchievement!.IsUnlocked.Should().BeTrue();

            // Verify profile was saved to file
            var profileFiles = _profileManager.GetAllProfileFiles();
            profileFiles.Should().Contain("integrationtest.json");
        }

        [Fact]
        public async Task AchievementSystem_Integration_UnlocksCorrectAchievements()
        {
            // Arrange
            await _profileManager.CreateNewProfileAsync("AchievementTest");
            var gameConfig = new GameConfiguration();

            // Act - Simulate various game scenarios to unlock different achievements

            // 1. Perfect accuracy achievement
            for (int i = 0; i < 5; i++)
            {
                _gameSession.RecordAnswer(true, 1.5);
            }

            // 2. Streak achievement
            for (int i = 0; i < 3; i++) // Build up to 8 total correct (5+3)
            {
                _gameSession.RecordAnswer(true, 1.0);
            }

            // 3. Complete a stage
            _gameSession.RecordStageCompletion();

            // 4. Check achievements
            var stats = _gameSession.GetStatistics();
            _gameSession.AchievementManager.CheckAchievements(stats, gameConfig);

            // Assert
            stats.AccuracyPercentage.Should().Be(100.0);
            stats.BestStreak.Should().Be(8);
            stats.StagesCompleted.Should().Be(1);

            var achievements = _gameSession.AchievementManager.UnlockedAchievements;
            achievements.Should().Contain(a => a.Id == "first_100"); // Perfect accuracy
            achievements.Should().Contain(a => a.Id == "streak_5");  // 5-streak
            achievements.Should().Contain(a => a.Id == "rookie_graduate"); // Stage completion
        }

        [Fact]
        public async Task ProfilePersistence_Integration_LoadsSavedData()
        {
            // Arrange - Create and save a profile with progress
            var originalProfile = await _profileManager.CreateNewProfileAsync("PersistenceTest");
            
            // Make some progress
            _gameSession.RecordAnswer(true, 2.0);
            _gameSession.RecordAnswer(true, 1.8);
            _gameSession.RecordStageCompletion();
            
            var gameConfig = new GameConfiguration();
            var stats = _gameSession.GetStatistics();
            _gameSession.AchievementManager.CheckAchievements(stats, gameConfig);
            
            await _profileManager.SaveCurrentProfileAsync();

            // Act - Create a new profile manager and load the same profile
            var newProfileManager = new ProfileManager();
            var field = typeof(ProfileManager).GetField("_savesDirectory", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(newProfileManager, _testDirectory);

            var loadedProfile = await newProfileManager.LoadProfileAsync("persistencetest.json");

            // Assert
            loadedProfile.Should().NotBeNull();
            loadedProfile.Name.Should().Be("PersistenceTest");
            
            // Verify achievement data was persisted
            loadedProfile.AchievementData.Should().NotBeNull();
            loadedProfile.AchievementData.UnlockedAchievements.Should().ContainKey("rookie_graduate");
            loadedProfile.AchievementData.UnlockedAchievements["rookie_graduate"].Should().BeTrue();
        }

        [Fact]
        public void ProblemGeneration_Integration_GeneratesValidProblems()
        {
            // Arrange
            var gameConfig = new GameConfiguration();
            gameConfig.SetDifficulty(DifficultyLevel.Junior);
            gameConfig.SetMathType(ProblemType.Mixed);

            // Act - Generate a set of problems
            var problems = _problemGenerator.GenerateMixed(10, DifficultyLevel.Junior);

            // Assert
            problems.Should().HaveCount(10);
            problems.Should().AllSatisfy(problem =>
            {
                problem.Question.Should().NotBeNullOrEmpty();
                problem.CorrectAnswer.Should().BeGreaterThan(0);
                problem.Difficulty.Should().Be(DifficultyLevel.Junior);
                
                // Verify we can validate answers for each problem
                var correctResult = _answerValidator.ValidateAnswer(problem.CorrectAnswer.ToString(), problem);
                correctResult.IsValid.Should().BeTrue();
                correctResult.IsCorrect.Should().BeTrue();
                
                var incorrectResult = _answerValidator.ValidateAnswer("999999", problem);
                incorrectResult.IsValid.Should().BeTrue();
                incorrectResult.IsCorrect.Should().BeFalse();
            });

            // Should have multiple operation types
            var operationTypes = problems.Select(p => p.Type).Distinct().ToList();
            operationTypes.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public async Task CarBreakdown_Integration_WorksWithGameSession()
        {
            // Arrange
            await _profileManager.CreateNewProfileAsync("BreakdownTest");
            var carBreakdownSystem = new CarBreakdownSystem();
            
            // Act - Simulate multiple wrong answers to trigger breakdown
            for (int i = 0; i < 5; i++)
            {
                _gameSession.RecordAnswer(false, 3.0);
            }

            // Check if breakdown should occur
            var shouldBreakdown = carBreakdownSystem.CheckForBreakdown(5);
            
            if (shouldBreakdown)
            {
                carBreakdownSystem.TriggerBreakdown();
                
                // Simulate repair attempts
                bool repaired = false;
                int repairAttempts = 0;
                while (!repaired && repairAttempts < 10)
                {
                    repaired = carBreakdownSystem.AttemptRepair(true);
                    repairAttempts++;
                }

                // Assert
                repaired.Should().BeTrue();
                carBreakdownSystem.IsCarBrokenDown.Should().BeFalse();
            }

            // The test should complete without exceptions regardless of breakdown
            _gameSession.GetStatistics().TotalQuestions.Should().Be(5);
        }
    }
}
