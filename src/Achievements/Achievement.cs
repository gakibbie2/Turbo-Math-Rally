using TurboMathRally.Math;

namespace TurboMathRally.Core.Achievements
{
    /// <summary>
    /// Defines the different types of achievements available
    /// </summary>
    public enum AchievementType
    {
        Accuracy,      // Based on accuracy percentage
        Streak,        // Based on consecutive correct answers
        Speed,         // Based on response time
        Endurance,     // Based on total questions answered
        Series,        // Based on completing rally series
        Mastery,       // Based on mastering specific math operations
        Comeback,      // Based on recovering from mistakes
        Consistency    // Based on maintaining performance
    }

    /// <summary>
    /// Defines the rarity/difficulty of achievements
    /// </summary>
    public enum AchievementRarity
    {
        Common,        // Easy to achieve (Bronze-level)
        Uncommon,      // Moderate effort (Silver-level) 
        Rare,          // Significant effort (Gold-level)
        Epic,          // Major milestone (Platinum-level)
        Legendary      // Ultimate achievement (Diamond-level)
    }

    /// <summary>
    /// Represents a single achievement that can be unlocked
    /// </summary>
    public class Achievement
    {
        /// <summary>
        /// Unique identifier for the achievement
        /// </summary>
        public string Id { get; set; } = string.Empty;
        
        /// <summary>
        /// Display name of the achievement
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// Detailed description of what the player accomplished
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Rally-themed emoji icon for the achievement
        /// </summary>
        public string Icon { get; set; } = "🏆";
        
        /// <summary>
        /// Type category of this achievement
        /// </summary>
        public AchievementType Type { get; set; }
        
        /// <summary>
        /// Rarity level of this achievement
        /// </summary>
        public AchievementRarity Rarity { get; set; }
        
        /// <summary>
        /// Points awarded for unlocking this achievement
        /// </summary>
        public int Points { get; set; }
        
        /// <summary>
        /// Whether this achievement has been unlocked
        /// </summary>
        public bool IsUnlocked { get; set; }
        
        /// <summary>
        /// When this achievement was unlocked (null if not unlocked)
        /// </summary>
        public DateTime? UnlockedDate { get; set; }
        
        /// <summary>
        /// Current progress toward unlocking this achievement (0.0 to 1.0)
        /// </summary>
        public double Progress { get; set; }
        
        /// <summary>
        /// Optional target value for progress tracking
        /// </summary>
        public int TargetValue { get; set; }
        
        /// <summary>
        /// Optional current value for progress tracking
        /// </summary>
        public int CurrentValue { get; set; }
        
        /// <summary>
        /// Rally series this achievement is specific to (null for general achievements)
        /// </summary>
        public DifficultyLevel? RequiredSeries { get; set; }
        
        /// <summary>
        /// Math operation this achievement is specific to (null for general achievements)
        /// </summary>
        public MathOperation? RequiredOperation { get; set; }
        
        /// <summary>
        /// Whether this achievement should be hidden until unlocked
        /// </summary>
        public bool IsSecret { get; set; }

        /// <summary>
        /// Create a new achievement
        /// </summary>
        public Achievement()
        {
            Progress = 0.0;
            IsUnlocked = false;
        }

        /// <summary>
        /// Mark this achievement as unlocked
        /// </summary>
        public void Unlock()
        {
            if (!IsUnlocked)
            {
                IsUnlocked = true;
                UnlockedDate = DateTime.Now;
                Progress = 1.0;
                CurrentValue = TargetValue;
            }
        }

        /// <summary>
        /// Update progress toward unlocking this achievement
        /// </summary>
        /// <param name="newValue">New current value</param>
        public void UpdateProgress(int newValue)
        {
            if (IsUnlocked) return;
            
            CurrentValue = newValue;
            
            if (TargetValue > 0)
            {
                Progress = System.Math.Min(1.0, (double)CurrentValue / TargetValue);
                
                if (Progress >= 1.0)
                {
                    Unlock();
                }
            }
        }

        /// <summary>
        /// Get formatted progress string for display
        /// </summary>
        /// <returns>Progress string like "7/10" or "100%"</returns>
        public string GetProgressString()
        {
            if (IsUnlocked)
                return "UNLOCKED";
                
            if (TargetValue > 0)
                return $"{CurrentValue}/{TargetValue}";
                
            return $"{Progress * 100:F0}%";
        }

        /// <summary>
        /// Get color coding based on rarity
        /// </summary>
        /// <returns>Console color for this achievement</returns>
        public ConsoleColor GetRarityColor()
        {
            return Rarity switch
            {
                AchievementRarity.Common => ConsoleColor.Gray,
                AchievementRarity.Uncommon => ConsoleColor.Green,
                AchievementRarity.Rare => ConsoleColor.Blue,
                AchievementRarity.Epic => ConsoleColor.Magenta,
                AchievementRarity.Legendary => ConsoleColor.Yellow,
                _ => ConsoleColor.White
            };
        }

        /// <summary>
        /// Get descriptive rarity name
        /// </summary>
        /// <returns>Rarity name for display</returns>
        public string GetRarityName()
        {
            return Rarity switch
            {
                AchievementRarity.Common => "Bronze",
                AchievementRarity.Uncommon => "Silver", 
                AchievementRarity.Rare => "Gold",
                AchievementRarity.Epic => "Platinum",
                AchievementRarity.Legendary => "Diamond",
                _ => "Unknown"
            };
        }
    }
}
