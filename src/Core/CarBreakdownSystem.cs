using TurboMathRally.Utils;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Manages the car breakdown system with progressive warnings and repair mechanics
    /// </summary>
    public class CarBreakdownSystem
    {
        private int _strikeCount;
        private const int MaxStrikes = 3;
        
        /// <summary>
        /// Current number of strikes
        /// </summary>
        public int StrikeCount => _strikeCount;
        
        /// <summary>
        /// Maximum strikes before breakdown
        /// </summary>
        public int MaxStrikesAllowed => MaxStrikes;
        
        /// <summary>
        /// Whether the car is broken down
        /// </summary>
        public bool IsBrokenDown => _strikeCount >= MaxStrikes;
        
        /// <summary>
        /// Initialize a new car breakdown system
        /// </summary>
        public CarBreakdownSystem()
        {
            Reset();
        }
        
        /// <summary>
        /// Add a strike for a wrong answer and return warning message
        /// </summary>
        /// <returns>Warning details</returns>
        public StrikeResult AddStrike()
        {
            _strikeCount++;
            
            return _strikeCount switch
            {
                1 => new StrikeResult
                {
                    StrikeCount = _strikeCount,
                    IsBrokenDown = false,
                    WarningLevel = WarningLevel.Light,
                    Message = "⚠️ Strike 1: Your engine is making strange noises...",
                    Description = "🔧 One more wrong answer and you'll need to check under the hood!"
                },
                2 => new StrikeResult
                {
                    StrikeCount = _strikeCount,
                    IsBrokenDown = false,
                    WarningLevel = WarningLevel.Moderate,
                    Message = "⚠️⚠️ Strike 2: Your car is smoking and slowing down!",
                    Description = "🚨 Danger! One more mistake and your car will break down completely!"
                },
                >= 3 => new StrikeResult
                {
                    StrikeCount = _strikeCount,
                    IsBrokenDown = true,
                    WarningLevel = WarningLevel.Breakdown,
                    Message = "💥 BREAKDOWN! Your car has stopped working!",
                    Description = "🔧 You must solve a repair story problem to get back on track!"
                },
                _ => new StrikeResult() // Shouldn't happen
            };
        }
        
        /// <summary>
        /// Reset the strike counter after successful repair
        /// </summary>
        public void Reset()
        {
            _strikeCount = 0;
        }
        
        /// <summary>
        /// Display the current car status
        /// </summary>
        public void DisplayCarStatus()
        {
            Console.WriteLine();
            Console.WriteLine("🚗 CAR STATUS:");
            
            // Show visual car health
            string carHealth = _strikeCount switch
            {
                0 => "🚗💨 Perfect condition - running smoothly!",
                1 => "🚗⚠️  Minor issues - engine sounds rough",
                2 => "🚗💨❌ Major problems - smoking and slow!",
                >= 3 => "🚗💥 BROKEN DOWN - not moving!",
                _ => "🚗"
            };
            
            Console.WriteLine($"   {carHealth}");
            Console.WriteLine($"   🔧 Strikes: {_strikeCount}/{MaxStrikes}");
            
            // Show strike indicator
            string strikeIndicator = "";
            for (int i = 0; i < MaxStrikes; i++)
            {
                if (i < _strikeCount)
                    strikeIndicator += "❌ ";
                else
                    strikeIndicator += "⭕ ";
            }
            Console.WriteLine($"   {strikeIndicator}");
        }
        
        /// <summary>
        /// Generate car-themed encouraging message
        /// </summary>
        public string GetEncouragingMessage()
        {
            string[] messages = _strikeCount switch
            {
                0 => new[]
                {
                    "🏁 Your car is purring like a well-tuned engine!",
                    "🚀 Smooth driving - you're in the lead!",
                    "⚡ Perfect performance - keep up the great work!"
                },
                1 => new[]
                {
                    "🔧 A quick tune-up and you'll be back to racing speed!",
                    "🚗 Don't worry - even race cars need maintenance!",
                    "💪 You can fix this - focus and get back on track!"
                },
                2 => new[]
                {
                    "🚨 Time to be extra careful - your car needs you!",
                    "🔧 One careful answer and you can avoid the pit stop!",
                    "🏁 Rally drivers face challenges - you've got this!"
                },
                _ => new[]
                {
                    "🔧 Even the best drivers need repairs sometimes!",
                    "🚗 Time to show your mechanical skills!",
                    "💪 Fix it up and get back to racing!"
                }
            };
            
            Random random = new Random();
            return messages[random.Next(messages.Length)];
        }
    }
    
    /// <summary>
    /// Result of adding a strike to the car breakdown system
    /// </summary>
    public class StrikeResult
    {
        /// <summary>
        /// Current strike count
        /// </summary>
        public int StrikeCount { get; set; }
        
        /// <summary>
        /// Whether the car is broken down
        /// </summary>
        public bool IsBrokenDown { get; set; }
        
        /// <summary>
        /// Warning level for this strike
        /// </summary>
        public WarningLevel WarningLevel { get; set; }
        
        /// <summary>
        /// Warning message to display
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// Additional description
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// Warning levels for strikes
    /// </summary>
    public enum WarningLevel
    {
        Light,
        Moderate,
        Breakdown
    }
}
