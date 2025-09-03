using TurboMathRally.Core.Achievements;
using TurboMathRally.Math;
using System.Windows.Forms;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Tracks statistics and progress for a gaming session
    /// </summary>
    public class GameSession
    {
        private readonly List<double> _responseTimes;
        private readonly List<bool> _answerHistory;
        private int _currentStreak;
        private int _bestStreak;
        private int _stagesCompleted;
        private int _comebacksAchieved;
        private DateTime? _sessionStart;
        private DateTime? _lastIncorrectTime;

        /// <summary>
        /// Total number of questions answered this session
        /// </summary>
        public int TotalQuestions => _answerHistory.Count;

        /// <summary>
        /// Number of correct answers this session
        /// </summary>
        public int CorrectAnswers => _answerHistory.Count(x => x);

        /// <summary>
        /// Current accuracy percentage
        /// </summary>
        public double AccuracyPercentage => TotalQuestions == 0 ? 0 : (double)CorrectAnswers / TotalQuestions * 100;

        /// <summary>
        /// Current streak of correct answers
        /// </summary>
        public int CurrentStreak => _currentStreak;

        /// <summary>
        /// Best streak achieved this session
        /// </summary>
        public int BestStreak => _bestStreak;

        /// <summary>
        /// Number of rally stages completed this session
        /// </summary>
        public int StagesCompleted => _stagesCompleted;

        /// <summary>
        /// Number of comebacks achieved (recovering from 3+ wrong answers)
        /// </summary>
        public int ComebacksAchieved => _comebacksAchieved;

        /// <summary>
        /// Average response time in seconds
        /// </summary>
        public double AverageResponseTime => _responseTimes.Count == 0 ? 0 : _responseTimes.Average();

        /// <summary>
        /// Total time spent in this session
        /// </summary>
        public TimeSpan TotalPlayTime => DateTime.Now - (_sessionStart ?? DateTime.Now);

        /// <summary>
        /// Achievement manager for this session
        /// </summary>
        public AchievementManager AchievementManager { get; private set; }

        /// <summary>
        /// Profile manager for persistent data
        /// </summary>
        public ProfileManager? ProfileManager { get; private set; }

        /// <summary>
        /// Initialize a new game session
        /// </summary>
        public GameSession()
        {
            _responseTimes = new List<double>();
            _answerHistory = new List<bool>();
            _currentStreak = 0;
            _bestStreak = 0;
            _stagesCompleted = 0;
            _comebacksAchieved = 0;
            _sessionStart = DateTime.Now;
            AchievementManager = new AchievementManager();
            
            // Subscribe to achievement unlocks for notifications
            AchievementManager.AchievementUnlocked += OnAchievementUnlocked;
        }

        /// <summary>
        /// Initialize a new game session with profile manager
        /// </summary>
        public GameSession(ProfileManager profileManager) : this()
        {
            ProfileManager = profileManager;
            AchievementManager = new AchievementManager(profileManager);
            
            // Subscribe to achievement unlocks for notifications
            AchievementManager.AchievementUnlocked += OnAchievementUnlocked;
        }

        /// <summary>
        /// Update the profile manager and reload achievements
        /// </summary>
        /// <param name="profileManager">The new profile manager to use</param>
        public void UpdateProfileManager(ProfileManager profileManager)
        {
            ProfileManager = profileManager;
            AchievementManager.UpdateProfileManager(profileManager);
        }

        /// <summary>
        /// Record an answer and update statistics
        /// </summary>
        /// <param name="isCorrect">Whether the answer was correct</param>
        /// <param name="responseTime">Time taken to answer in seconds</param>
        public void RecordAnswer(bool isCorrect, double responseTime)
        {
            _answerHistory.Add(isCorrect);
            _responseTimes.Add(System.Math.Max(0.1, responseTime)); // Minimum 0.1s to avoid division by zero

            if (isCorrect)
            {
                _currentStreak++;
                if (_currentStreak > _bestStreak)
                {
                    _bestStreak = _currentStreak;
                }

                // Check for comeback after being wrong
                if (_lastIncorrectTime.HasValue && 
                    GetRecentWrongCount() >= 3 && 
                    _currentStreak >= 3)
                {
                    _comebacksAchieved++;
                    _lastIncorrectTime = null; // Reset comeback tracking
                }
            }
            else
            {
                _currentStreak = 0;
                _lastIncorrectTime = DateTime.Now;
            }

            // Check achievements after each answer
            CheckAchievements();
        }

        /// <summary>
        /// Record that a stage has been completed
        /// </summary>
        public void RecordStageCompletion()
        {
            _stagesCompleted++;
            CheckAchievements();
        }

        /// <summary>
        /// Reset session statistics (new game)
        /// </summary>
        public void ResetSession()
        {
            _responseTimes.Clear();
            _answerHistory.Clear();
            _currentStreak = 0;
            _bestStreak = 0;
            _stagesCompleted = 0;
            _comebacksAchieved = 0;
            _sessionStart = DateTime.Now;
            _lastIncorrectTime = null;
        }

        /// <summary>
        /// Get statistics object for achievement checking (session only)
        /// </summary>
        /// <param name="gameConfig">Current game configuration</param>
        /// <returns>GameStatistics object with session data only</returns>
        public Achievements.GameStatistics GetSessionStatistics(GameConfiguration gameConfig)
        {
            return new Achievements.GameStatistics
            {
                AccuracyPercentage = AccuracyPercentage,
                TotalQuestions = TotalQuestions,
                CorrectAnswers = CorrectAnswers,
                BestStreak = BestStreak,
                CurrentStreak = CurrentStreak,
                AverageResponseTime = AverageResponseTime,
                StagesCompleted = StagesCompleted,
                ComebacksAchieved = ComebacksAchieved,
                TotalPlayTime = TotalPlayTime
            };
        }

        /// <summary>
        /// Get statistics object for achievement checking
        /// </summary>
        /// <param name="gameConfig">Current game configuration</param>
        /// <returns>GameStatistics object</returns>
        public Achievements.GameStatistics GetStatistics(GameConfiguration gameConfig)
        {
            // For achievement checking, we need to use combined (profile + session) statistics
            if (ProfileManager?.CurrentProfile?.OverallStats != null)
            {
                var profileStats = ProfileManager.CurrentProfile.OverallStats;
                return new Achievements.GameStatistics
                {
                    AccuracyPercentage = AccuracyPercentage, // Use current session accuracy
                    TotalQuestions = profileStats.TotalQuestions + TotalQuestions,
                    CorrectAnswers = profileStats.CorrectAnswers + CorrectAnswers,
                    BestStreak = System.Math.Max(profileStats.BestStreak, BestStreak),
                    CurrentStreak = CurrentStreak,
                    AverageResponseTime = _responseTimes.Count > 0 ? AverageResponseTime : profileStats.AverageResponseTime,
                    StagesCompleted = profileStats.StagesCompleted + StagesCompleted,
                    ComebacksAchieved = profileStats.ComebacksAchieved + ComebacksAchieved,
                    TotalPlayTime = profileStats.TotalPlayTime.Add(TotalPlayTime)
                };
            }
            else
            {
                // Fallback to session-only statistics if no profile
                return new Achievements.GameStatistics
                {
                    AccuracyPercentage = AccuracyPercentage,
                    TotalQuestions = TotalQuestions,
                    CorrectAnswers = CorrectAnswers,
                    BestStreak = BestStreak,
                    CurrentStreak = CurrentStreak,
                    AverageResponseTime = AverageResponseTime,
                    StagesCompleted = StagesCompleted,
                    ComebacksAchieved = ComebacksAchieved,
                    TotalPlayTime = TotalPlayTime
                };
            }
        }

        /// <summary>
        /// Check for achievement unlocks
        /// </summary>
        /// <param name="gameConfig">Current game configuration</param>
        public void CheckAchievements(GameConfiguration? gameConfig = null)
        {
            // Use a default config if none provided
            gameConfig ??= new GameConfiguration();
            
            var sessionStats = GetSessionStatistics(gameConfig);
            var combinedStats = GetStatistics(gameConfig);
            AchievementManager.CheckSessionAchievements(sessionStats, combinedStats, gameConfig);
        }

        /// <summary>
        /// Display any recent achievement unlocks
        /// </summary>
        public void DisplayRecentAchievements()
        {
            var recentUnlocks = AchievementManager.RecentUnlocks;
            
            if (recentUnlocks.Count > 0)
            {
                var achievementText = new System.Text.StringBuilder();
                achievementText.AppendLine("🏆━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                achievementText.AppendLine("🎉  ACHIEVEMENT UNLOCKED!");
                achievementText.AppendLine("🏆━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                achievementText.AppendLine();

                foreach (var achievement in recentUnlocks)
                {
                    achievementText.AppendLine($"🎖️  {achievement.Icon} {achievement.Title}");
                    achievementText.AppendLine($"    {achievement.Description}");
                    achievementText.AppendLine($"    +{achievement.Points} Achievement Points!");
                    achievementText.AppendLine();
                }

                // Show achievements in a message box
                MessageBox.Show(achievementText.ToString(), "New Achievements!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Clear the recent unlocks after displaying
                AchievementManager.ClearRecentUnlocks();
            }
        }

        /// <summary>
        /// Save current session data to profile
        /// </summary>
        public async Task SaveSessionAsync(GameConfiguration gameConfig)
        {
            if (ProfileManager == null)
                return;

            var sessionRecord = new SessionRecord
            {
                SessionStart = _sessionStart ?? DateTime.Now,
                Duration = TotalPlayTime,
                Difficulty = gameConfig.SelectedDifficulty,
                MathType = gameConfig.SelectedMathType,
                WasMixedMode = gameConfig.IsMixedMode,
                QuestionsAnswered = TotalQuestions,
                CorrectAnswers = CorrectAnswers,
                BestStreak = BestStreak,
                AverageResponseTime = AverageResponseTime,
                StagesCompleted = StagesCompleted,
                AchievementsUnlocked = AchievementManager.RecentUnlocks.Select(a => a.Id).ToList()
            };

            try
            {
                await ProfileManager.RecordSessionAsync(sessionRecord);
                
                // Update overall game statistics with Core.GameStatistics  
                if (ProfileManager.CurrentProfile != null)
                {
                    var coreStats = new GameStatistics
                    {
                        TotalQuestions = ProfileManager.CurrentProfile.OverallStats.TotalQuestions + TotalQuestions,
                        CorrectAnswers = ProfileManager.CurrentProfile.OverallStats.CorrectAnswers + CorrectAnswers,
                        BestStreak = System.Math.Max(ProfileManager.CurrentProfile.OverallStats.BestStreak, BestStreak),
                        CurrentStreak = CurrentStreak,
                        AverageResponseTime = (ProfileManager.CurrentProfile.OverallStats.AverageResponseTime + AverageResponseTime) / 2,
                        StagesCompleted = ProfileManager.CurrentProfile.OverallStats.StagesCompleted + StagesCompleted,
                        ComebacksAchieved = ProfileManager.CurrentProfile.OverallStats.ComebacksAchieved + ComebacksAchieved,
                        TotalPlayTime = ProfileManager.CurrentProfile.OverallStats.TotalPlayTime.Add(TotalPlayTime)
                    };
                    await ProfileManager.UpdateGameStatisticsAsync(coreStats);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving session: {ex.Message}");
            }
        }

        /// <summary>
        /// Get count of wrong answers in recent history (last 10 answers)
        /// </summary>
        private int GetRecentWrongCount()
        {
            var recentAnswers = _answerHistory.TakeLast(10).ToList();
            return recentAnswers.Count(x => !x);
        }

        /// <summary>
        /// Handle achievement unlock notifications
        /// </summary>
        private void OnAchievementUnlocked(object? sender, Achievement achievement)
        {
            // This can be used for immediate notifications or logging
            // For now, we'll rely on DisplayRecentAchievements() for showing unlocks
        }

        /// <summary>
        /// Get formatted session summary for display
        /// </summary>
        public void DisplaySessionSummary()
        {
            Console.WriteLine("📊 SESSION SUMMARY");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine($"🎯 Questions Answered: {TotalQuestions}");
            Console.WriteLine($"✅ Correct: {CorrectAnswers}");
            Console.WriteLine($"📈 Accuracy: {AccuracyPercentage:F1}%");
            Console.WriteLine($"🔥 Best Streak: {BestStreak}");
            Console.WriteLine($"⚡ Avg Response Time: {AverageResponseTime:F1}s");
            Console.WriteLine($"🏁 Stages Completed: {StagesCompleted}");
            Console.WriteLine($"💪 Comebacks: {ComebacksAchieved}");
            Console.WriteLine($"⏱️ Play Time: {TotalPlayTime:hh\\:mm\\:ss}");
            Console.WriteLine($"🏆 Achievement Points: {AchievementManager.TotalPoints}");
            Console.WriteLine($"🎖️ Achievements: {AchievementManager.UnlockedAchievements.Count}/{AchievementManager.AllAchievements.Count}");
        }
    }
}
