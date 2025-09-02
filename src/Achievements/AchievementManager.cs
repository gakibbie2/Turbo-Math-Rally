using TurboMathRally.Math;
using TurboMathRally.Utils;

namespace TurboMathRally.Core.Achievements
{
    /// <summary>
    /// Manages achievement unlocking, progress tracking, and display
    /// </summary>
    public class AchievementManager
    {
        private readonly Dictionary<string, Achievement> _achievements;
        private readonly List<Achievement> _recentUnlocks;

        /// <summary>
        /// Event fired when an achievement is unlocked
        /// </summary>
        public event EventHandler<Achievement>? AchievementUnlocked;

        /// <summary>
        /// Get all available achievements
        /// </summary>
        public IReadOnlyList<Achievement> AllAchievements => _achievements.Values.ToList();

        /// <summary>
        /// Get all unlocked achievements
        /// </summary>
        public IReadOnlyList<Achievement> UnlockedAchievements => 
            _achievements.Values.Where(a => a.IsUnlocked).ToList();

        /// <summary>
        /// Get recently unlocked achievements (since last session display)
        /// </summary>
        public IReadOnlyList<Achievement> RecentUnlocks => _recentUnlocks;

        /// <summary>
        /// Get total achievement points earned
        /// </summary>
        public int TotalPoints => UnlockedAchievements.Sum(a => a.Points);

        /// <summary>
        /// Get achievement completion percentage
        /// </summary>
        public double CompletionPercentage => 
            _achievements.Count == 0 ? 0 : (double)UnlockedAchievements.Count / _achievements.Count * 100;

        /// <summary>
        /// Initialize the achievement manager with all available achievements
        /// </summary>
        public AchievementManager()
        {
            _achievements = new Dictionary<string, Achievement>();
            _recentUnlocks = new List<Achievement>();
            
            InitializeAchievements();
        }

        /// <summary>
        /// Check for achievement unlocks based on current game statistics
        /// </summary>
        /// <param name="stats">Current game statistics</param>
        /// <param name="gameConfig">Current game configuration</param>
        public void CheckAchievements(GameStatistics stats, GameConfiguration gameConfig)
        {
            // Check all achievements for potential unlocks
            foreach (var achievement in _achievements.Values.Where(a => !a.IsUnlocked))
            {
                CheckIndividualAchievement(achievement, stats, gameConfig);
            }
        }

        /// <summary>
        /// Check a specific achievement for unlock conditions
        /// </summary>
        /// <param name="achievement">Achievement to check</param>
        /// <param name="stats">Current game statistics</param>
        /// <param name="gameConfig">Current game configuration</param>
        private void CheckIndividualAchievement(Achievement achievement, GameStatistics stats, GameConfiguration gameConfig)
        {
            bool shouldUnlock = achievement.Id switch
            {
                // Accuracy Achievements
                "first_100" => stats.AccuracyPercentage >= 100 && stats.TotalQuestions >= 5,
                "accuracy_90" => stats.AccuracyPercentage >= 90 && stats.TotalQuestions >= 20,
                "accuracy_95" => stats.AccuracyPercentage >= 95 && stats.TotalQuestions >= 50,
                "perfectionist" => stats.AccuracyPercentage >= 100 && stats.TotalQuestions >= 100,

                // Streak Achievements
                "streak_5" => stats.BestStreak >= 5,
                "streak_10" => stats.BestStreak >= 10,
                "streak_25" => stats.BestStreak >= 25,
                "unstoppable" => stats.BestStreak >= 50,

                // Speed Achievements (assuming we track average response time)
                "speedster" => stats.AverageResponseTime > 0 && stats.AverageResponseTime <= 3.0 && stats.TotalQuestions >= 10,
                "lightning_fast" => stats.AverageResponseTime > 0 && stats.AverageResponseTime <= 2.0 && stats.TotalQuestions >= 25,
                "flash" => stats.AverageResponseTime > 0 && stats.AverageResponseTime <= 1.5 && stats.TotalQuestions >= 50,

                // Endurance Achievements
                "getting_started" => stats.TotalQuestions >= 10,
                "dedicated" => stats.TotalQuestions >= 100,
                "marathon_runner" => stats.TotalQuestions >= 500,
                "champion" => stats.TotalQuestions >= 1000,

                // Series Completion (based on difficulty level)
                "rookie_graduate" => gameConfig.SelectedDifficulty == DifficultyLevel.Rookie && stats.StagesCompleted >= 1,
                "junior_champion" => gameConfig.SelectedDifficulty == DifficultyLevel.Junior && stats.StagesCompleted >= 1,
                "pro_racer" => gameConfig.SelectedDifficulty == DifficultyLevel.Pro && stats.StagesCompleted >= 1,

                // Comeback Achievements
                "comeback_kid" => stats.ComebacksAchieved >= 1,
                "resilient" => stats.ComebacksAchieved >= 5,

                _ => false
            };

            if (shouldUnlock)
            {
                UnlockAchievement(achievement.Id);
            }
            else
            {
                // Update progress for achievements that aren't unlocked yet
                UpdateAchievementProgress(achievement, stats, gameConfig);
            }
        }

        /// <summary>
        /// Update progress for an achievement that hasn't been unlocked
        /// </summary>
        private void UpdateAchievementProgress(Achievement achievement, GameStatistics stats, GameConfiguration gameConfig)
        {
            var newValue = achievement.Id switch
            {
                "first_100" => stats.TotalQuestions,
                "accuracy_90" => stats.TotalQuestions,
                "accuracy_95" => stats.TotalQuestions,
                "perfectionist" => stats.TotalQuestions,
                "streak_5" => stats.BestStreak,
                "streak_10" => stats.BestStreak,
                "streak_25" => stats.BestStreak,
                "unstoppable" => stats.BestStreak,
                "getting_started" => stats.TotalQuestions,
                "dedicated" => stats.TotalQuestions,
                "marathon_runner" => stats.TotalQuestions,
                "champion" => stats.TotalQuestions,
                "comeback_kid" => stats.ComebacksAchieved,
                "resilient" => stats.ComebacksAchieved,
                _ => 0
            };

            achievement.UpdateProgress(newValue);
        }

        /// <summary>
        /// Unlock a specific achievement by ID
        /// </summary>
        /// <param name="achievementId">ID of achievement to unlock</param>
        /// <returns>True if achievement was unlocked, false if already unlocked or not found</returns>
        public bool UnlockAchievement(string achievementId)
        {
            if (_achievements.TryGetValue(achievementId, out var achievement) && !achievement.IsUnlocked)
            {
                achievement.Unlock();
                _recentUnlocks.Add(achievement);
                
                // Fire event for UI notifications
                AchievementUnlocked?.Invoke(this, achievement);
                
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Clear recent unlocks (call after displaying them to user)
        /// </summary>
        public void ClearRecentUnlocks()
        {
            _recentUnlocks.Clear();
        }

        /// <summary>
        /// Get achievements by type
        /// </summary>
        /// <param name="type">Achievement type to filter by</param>
        /// <returns>List of achievements of the specified type</returns>
        public List<Achievement> GetAchievementsByType(AchievementType type)
        {
            return _achievements.Values.Where(a => a.Type == type).ToList();
        }

        /// <summary>
        /// Get achievements by rarity
        /// </summary>
        /// <param name="rarity">Achievement rarity to filter by</param>
        /// <returns>List of achievements of the specified rarity</returns>
        public List<Achievement> GetAchievementsByRarity(AchievementRarity rarity)
        {
            return _achievements.Values.Where(a => a.Rarity == rarity).ToList();
        }

        /// <summary>
        /// Display achievements to console in organized format
        /// </summary>
        public void DisplayAchievements()
        {
            ConsoleHelper.ClearScreen();
            
            Console.WriteLine("🏆━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("🎖️  ACHIEVEMENT GALLERY - TURBO MATH RALLY");
            Console.WriteLine("🏆━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();

            // Display completion stats
            ConsoleHelper.WriteColored("📊 Progress: ", ConsoleColor.Cyan);
            ConsoleHelper.WriteColored($"{UnlockedAchievements.Count}/{AllAchievements.Count}", ConsoleColor.White);
            ConsoleHelper.WriteColored($" ({CompletionPercentage:F1}%)", ConsoleColor.Yellow);
            Console.WriteLine();

            ConsoleHelper.WriteColored("⭐ Total Points: ", ConsoleColor.Cyan);
            ConsoleHelper.WriteColored($"{TotalPoints}", ConsoleColor.Yellow);
            Console.WriteLine();
            Console.WriteLine();

            // Group achievements by type for better organization
            var achievementGroups = _achievements.Values.GroupBy(a => a.Type);

            foreach (var group in achievementGroups.OrderBy(g => g.Key))
            {
                DisplayAchievementGroup(group.Key, group.ToList());
                Console.WriteLine();
            }

            Console.WriteLine("🏁 Press any key to return to menu...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Display a group of achievements by type
        /// </summary>
        private void DisplayAchievementGroup(AchievementType type, List<Achievement> achievements)
        {
            string groupTitle = type switch
            {
                AchievementType.Accuracy => "🎯 ACCURACY MASTERS",
                AchievementType.Streak => "🔥 STREAK LEGENDS", 
                AchievementType.Speed => "⚡ SPEED DEMONS",
                AchievementType.Endurance => "💪 ENDURANCE CHAMPIONS",
                AchievementType.Series => "🏁 RALLY GRADUATES",
                AchievementType.Mastery => "🧠 MATH MASTERS",
                AchievementType.Comeback => "💥 COMEBACK HEROES",
                AchievementType.Consistency => "📈 CONSISTENCY KINGS",
                _ => "🏆 MISCELLANEOUS"
            };

            ConsoleHelper.WriteLineColored(groupTitle, ConsoleColor.Magenta);
            Console.WriteLine(new string('─', 50));

            foreach (var achievement in achievements.OrderBy(a => a.Rarity).ThenBy(a => a.Title))
            {
                DisplaySingleAchievement(achievement);
            }
        }

        /// <summary>
        /// Display a single achievement with proper formatting
        /// </summary>
        private void DisplaySingleAchievement(Achievement achievement)
        {
            var color = achievement.IsUnlocked ? achievement.GetRarityColor() : ConsoleColor.DarkGray;
            var statusIcon = achievement.IsUnlocked ? "✅" : "🔒";
            
            ConsoleHelper.WriteColored($"{statusIcon} {achievement.Icon} ", ConsoleColor.White);
            ConsoleHelper.WriteColored($"{achievement.Title}", color);
            ConsoleHelper.WriteColored($" ({achievement.GetRarityName()})", ConsoleColor.Gray);
            
            if (achievement.IsUnlocked)
            {
                ConsoleHelper.WriteColored($" +{achievement.Points} pts", ConsoleColor.Yellow);
            }
            
            Console.WriteLine();
            
            ConsoleHelper.WriteColored($"   {achievement.Description}", ConsoleColor.Gray);
            
            if (!achievement.IsUnlocked && achievement.Progress > 0)
            {
                ConsoleHelper.WriteColored($" [{achievement.GetProgressString()}]", ConsoleColor.DarkYellow);
            }
            
            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Initialize all available achievements
        /// </summary>
        private void InitializeAchievements()
        {
            // Accuracy Achievements
            AddAchievement("first_100", "First Perfect!", "Get your first 100% accuracy", "🎯", 
                AchievementType.Accuracy, AchievementRarity.Common, 25, targetValue: 5);

            AddAchievement("accuracy_90", "Sharp Shooter", "Maintain 90%+ accuracy for 20 questions", "🎯", 
                AchievementType.Accuracy, AchievementRarity.Uncommon, 50, targetValue: 20);

            AddAchievement("accuracy_95", "Master Marksman", "Maintain 95%+ accuracy for 50 questions", "🏹", 
                AchievementType.Accuracy, AchievementRarity.Rare, 100, targetValue: 50);

            AddAchievement("perfectionist", "The Perfectionist", "Maintain 100% accuracy for 100 questions", "💎", 
                AchievementType.Accuracy, AchievementRarity.Legendary, 500, targetValue: 100);

            // Streak Achievements
            AddAchievement("streak_5", "Hot Streak", "Answer 5 questions correctly in a row", "🔥", 
                AchievementType.Streak, AchievementRarity.Common, 25, targetValue: 5);

            AddAchievement("streak_10", "On Fire!", "Answer 10 questions correctly in a row", "🔥", 
                AchievementType.Streak, AchievementRarity.Uncommon, 75, targetValue: 10);

            AddAchievement("streak_25", "Unstoppable", "Answer 25 questions correctly in a row", "🚀", 
                AchievementType.Streak, AchievementRarity.Rare, 200, targetValue: 25);

            AddAchievement("unstoppable", "Legendary Streak", "Answer 50 questions correctly in a row", "⚡", 
                AchievementType.Streak, AchievementRarity.Legendary, 750, targetValue: 50);

            // Speed Achievements  
            AddAchievement("speedster", "Speedster", "Average under 3 seconds per answer (10+ questions)", "💨", 
                AchievementType.Speed, AchievementRarity.Uncommon, 60, targetValue: 10);

            AddAchievement("lightning_fast", "Lightning Fast", "Average under 2 seconds per answer (25+ questions)", "⚡", 
                AchievementType.Speed, AchievementRarity.Rare, 150, targetValue: 25);

            AddAchievement("flash", "The Flash", "Average under 1.5 seconds per answer (50+ questions)", "💫", 
                AchievementType.Speed, AchievementRarity.Epic, 400, targetValue: 50);

            // Endurance Achievements
            AddAchievement("getting_started", "Getting Started", "Answer your first 10 questions", "🌱", 
                AchievementType.Endurance, AchievementRarity.Common, 10, targetValue: 10);

            AddAchievement("dedicated", "Dedicated Racer", "Answer 100 total questions", "🏃", 
                AchievementType.Endurance, AchievementRarity.Uncommon, 100, targetValue: 100);

            AddAchievement("marathon_runner", "Marathon Runner", "Answer 500 total questions", "🏃‍♂️", 
                AchievementType.Endurance, AchievementRarity.Rare, 300, targetValue: 500);

            AddAchievement("champion", "Ultimate Champion", "Answer 1000 total questions", "👑", 
                AchievementType.Endurance, AchievementRarity.Epic, 1000, targetValue: 1000);

            // Series Completion
            AddAchievement("rookie_graduate", "Rookie Graduate", "Complete your first Rookie Rally stage", "🎓", 
                AchievementType.Series, AchievementRarity.Common, 50, requiredSeries: DifficultyLevel.Rookie);

            AddAchievement("junior_champion", "Junior Champion", "Complete your first Junior Championship stage", "🏆", 
                AchievementType.Series, AchievementRarity.Uncommon, 100, requiredSeries: DifficultyLevel.Junior);

            AddAchievement("pro_racer", "Pro Racer", "Complete your first Pro Circuit stage", "🏁", 
                AchievementType.Series, AchievementRarity.Rare, 200, requiredSeries: DifficultyLevel.Pro);

            // Comeback Achievements
            AddAchievement("comeback_kid", "Comeback Kid", "Turn a losing streak into a winning one", "💪", 
                AchievementType.Comeback, AchievementRarity.Uncommon, 75, targetValue: 1);

            AddAchievement("resilient", "Resilient Racer", "Make 5 successful comebacks", "🛡️", 
                AchievementType.Comeback, AchievementRarity.Rare, 250, targetValue: 5);
        }

        /// <summary>
        /// Helper method to add an achievement to the collection
        /// </summary>
        private void AddAchievement(string id, string title, string description, string icon,
            AchievementType type, AchievementRarity rarity, int points, int targetValue = 0,
            DifficultyLevel? requiredSeries = null, MathOperation? requiredOperation = null, bool isSecret = false)
        {
            var achievement = new Achievement
            {
                Id = id,
                Title = title,
                Description = description,
                Icon = icon,
                Type = type,
                Rarity = rarity,
                Points = points,
                TargetValue = targetValue,
                RequiredSeries = requiredSeries,
                RequiredOperation = requiredOperation,
                IsSecret = isSecret
            };

            _achievements[id] = achievement;
        }
    }

    /// <summary>
    /// Consolidated game statistics for achievement checking
    /// </summary>
    public class GameStatistics
    {
        public double AccuracyPercentage { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int BestStreak { get; set; }
        public int CurrentStreak { get; set; }
        public double AverageResponseTime { get; set; }
        public int StagesCompleted { get; set; }
        public int ComebacksAchieved { get; set; }
        public TimeSpan TotalPlayTime { get; set; }
    }
}
