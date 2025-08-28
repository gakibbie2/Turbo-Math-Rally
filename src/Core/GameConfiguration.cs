using TurboMathRally.Math;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Stores the current game configuration selected by the player
    /// </summary>
    public class GameConfiguration
    {
        /// <summary>
        /// Selected math operation type
        /// </summary>
        public MathOperation SelectedMathType { get; set; } = MathOperation.Addition;
        
        /// <summary>
        /// Selected difficulty level based on rally series
        /// </summary>
        public DifficultyLevel SelectedDifficulty { get; set; } = DifficultyLevel.Junior;
        
        /// <summary>
        /// Selected player mode (Kid vs Parent)
        /// </summary>
        public PlayerMode SelectedPlayerMode { get; set; } = PlayerMode.Kid;
        
        /// <summary>
        /// Whether mixed math operations are enabled
        /// </summary>
        public bool IsMixedMode { get; set; } = false;
        
        /// <summary>
        /// The selected rally series name for display
        /// </summary>
        public string SelectedSeriesName { get; set; } = "Junior Championship";
        
        /// <summary>
        /// The selected math type name for display
        /// </summary>
        public string SelectedMathTypeName { get; set; } = "Addition Only";
    }
    
    /// <summary>
    /// Player mode selection
    /// </summary>
    public enum PlayerMode
    {
        Kid,
        Parent
    }
}
