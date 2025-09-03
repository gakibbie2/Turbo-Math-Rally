using System.Text.Json.Serialization;
using TurboMathRally.Math;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Represents a user profile with all persistent data
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// User's display name
        /// </summary>
        public string PlayerName { get; set; } = "Young Racer";
        
        /// <summary>
        /// When this profile was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Last time this profile was used
        /// </summary>
        public DateTime LastPlayedDate { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Profile data version for migration
        /// </summary>
        public string Version { get; set; } = "1.0";
        
        /// <summary>
        /// User's preferred settings
        /// </summary>
        public UserSettings Settings { get; set; } = new UserSettings();
        
        /// <summary>
        /// Achievement progress and unlocks
        /// </summary>
        public AchievementProgress AchievementData { get; set; } = new AchievementProgress();
        
        /// <summary>
        /// Overall game statistics
        /// </summary>
        public GameStatistics OverallStats { get; set; } = new GameStatistics();
        
        /// <summary>
        /// Rally series progress
        /// </summary>
        public RallyProgress RallyData { get; set; } = new RallyProgress();
        
        /// <summary>
        /// Session history for analytics
        /// </summary>
        public List<SessionRecord> SessionHistory { get; set; } = new List<SessionRecord>();
    }
    
    /// <summary>
    /// User preferences and settings
    /// </summary>
    public class UserSettings
    {
        /// <summary>
        /// Sound effects volume (0.0 to 1.0)
        /// </summary>
        public float SoundVolume { get; set; } = 0.7f;
        
        /// <summary>
        /// Background music volume (0.0 to 1.0)
        /// </summary>
        public float MusicVolume { get; set; } = 0.5f;
        
        /// <summary>
        /// Whether to show hints for wrong answers
        /// </summary>
        public bool ShowHints { get; set; } = true;
        
        /// <summary>
        /// Whether to show achievement notifications
        /// </summary>
        public bool ShowAchievementNotifications { get; set; } = true;
        
        /// <summary>
        /// Preferred difficulty level
        /// </summary>
        public DifficultyLevel PreferredDifficulty { get; set; } = DifficultyLevel.Junior;
        
        /// <summary>
        /// Preferred math operation type
        /// </summary>
        public MathOperation PreferredMathType { get; set; } = MathOperation.Addition;
        
        /// <summary>
        /// Whether mixed mode is preferred
        /// </summary>
        public bool PreferMixedMode { get; set; } = false;
        
        /// <summary>
        /// Theme preference (Light, Dark, Rally)
        /// </summary>
        public string Theme { get; set; } = "Rally";
        
        /// <summary>
        /// Whether to auto-save progress
        /// </summary>
        public bool AutoSave { get; set; } = true;
        
        /// <summary>
        /// Whether to show detailed statistics
        /// </summary>
        public bool ShowDetailedStats { get; set; } = true;
    }
    
    /// <summary>
    /// Achievement progress tracking
    /// </summary>
    public class AchievementProgress
    {
        /// <summary>
        /// Dictionary of achievement IDs to their unlock status
        /// </summary>
        public Dictionary<string, bool> UnlockedAchievements { get; set; } = new Dictionary<string, bool>();
        
        /// <summary>
        /// Dictionary of achievement IDs to their current progress values
        /// </summary>
        public Dictionary<string, int> ProgressValues { get; set; } = new Dictionary<string, int>();
        
        /// <summary>
        /// Total achievement points earned
        /// </summary>
        public int TotalPoints { get; set; } = 0;
        
        /// <summary>
        /// Date when each achievement was unlocked
        /// </summary>
        public Dictionary<string, DateTime> UnlockDates { get; set; } = new Dictionary<string, DateTime>();
    }
    
    /// <summary>
    /// Rally series progression data
    /// </summary>
    public class RallyProgress
    {
        /// <summary>
        /// Highest difficulty level completed
        /// </summary>
        public DifficultyLevel HighestCompletedDifficulty { get; set; } = DifficultyLevel.Rookie;
        
        /// <summary>
        /// Number of stages completed per difficulty
        /// </summary>
        public Dictionary<DifficultyLevel, int> StagesCompleted { get; set; } = new Dictionary<DifficultyLevel, int>();
        
        /// <summary>
        /// Best accuracy per difficulty level
        /// </summary>
        public Dictionary<DifficultyLevel, double> BestAccuracy { get; set; } = new Dictionary<DifficultyLevel, double>();
        
        /// <summary>
        /// Best completion time per difficulty level (in seconds)
        /// </summary>
        public Dictionary<DifficultyLevel, int> BestCompletionTime { get; set; } = new Dictionary<DifficultyLevel, int>();
        
        /// <summary>
        /// Total rally stages completed across all difficulties
        /// </summary>
        public int TotalStagesCompleted { get; set; } = 0;
    }
    
    /// <summary>
    /// Record of a single game session
    /// </summary>
    public class SessionRecord
    {
        /// <summary>
        /// When this session started
        /// </summary>
        public DateTime SessionStart { get; set; }
        
        /// <summary>
        /// How long the session lasted
        /// </summary>
        public TimeSpan Duration { get; set; }
        
        /// <summary>
        /// Difficulty level played
        /// </summary>
        public DifficultyLevel Difficulty { get; set; }
        
        /// <summary>
        /// Math operation type practiced
        /// </summary>
        public MathOperation MathType { get; set; }
        
        /// <summary>
        /// Whether mixed mode was used
        /// </summary>
        public bool WasMixedMode { get; set; }
        
        /// <summary>
        /// Questions answered in this session
        /// </summary>
        public int QuestionsAnswered { get; set; }
        
        /// <summary>
        /// Correct answers in this session
        /// </summary>
        public int CorrectAnswers { get; set; }
        
        /// <summary>
        /// Best streak achieved in this session
        /// </summary>
        public int BestStreak { get; set; }
        
        /// <summary>
        /// Average response time in seconds
        /// </summary>
        public double AverageResponseTime { get; set; }
        
        /// <summary>
        /// Stages completed in this session
        /// </summary>
        public int StagesCompleted { get; set; }
        
        /// <summary>
        /// Achievements unlocked in this session
        /// </summary>
        public List<string> AchievementsUnlocked { get; set; } = new List<string>();
    }
    
    /// <summary>
    /// Overall game statistics across all sessions
    /// </summary>
    public class GameStatistics
    {
        /// <summary>
        /// Total questions answered across all sessions
        /// </summary>
        public int TotalQuestions { get; set; } = 0;
        
        /// <summary>
        /// Total correct answers across all sessions
        /// </summary>
        public int CorrectAnswers { get; set; } = 0;
        
        /// <summary>
        /// Overall accuracy percentage
        /// </summary>
        public double AccuracyPercentage => TotalQuestions == 0 ? 0 : (double)CorrectAnswers / TotalQuestions * 100;
        
        /// <summary>
        /// Best streak ever achieved
        /// </summary>
        public int BestStreak { get; set; } = 0;
        
        /// <summary>
        /// Current streak (resets between sessions)
        /// </summary>
        public int CurrentStreak { get; set; } = 0;
        
        /// <summary>
        /// Average response time across all sessions
        /// </summary>
        public double AverageResponseTime { get; set; } = 0;
        
        /// <summary>
        /// Total stages completed
        /// </summary>
        public int StagesCompleted { get; set; } = 0;
        
        /// <summary>
        /// Number of comebacks achieved
        /// </summary>
        public int ComebacksAchieved { get; set; } = 0;
        
        /// <summary>
        /// Total play time across all sessions
        /// </summary>
        public TimeSpan TotalPlayTime { get; set; } = TimeSpan.Zero;
        
        /// <summary>
        /// Number of sessions played
        /// </summary>
        public int SessionsPlayed { get; set; } = 0;
        
        /// <summary>
        /// Favorite math operation (most practiced)
        /// </summary>
        public MathOperation FavoriteMathOperation { get; set; } = MathOperation.Addition;
        
        /// <summary>
        /// Favorite difficulty level (most played)
        /// </summary>
        public DifficultyLevel FavoriteDifficulty { get; set; } = DifficultyLevel.Junior;
    }
}
