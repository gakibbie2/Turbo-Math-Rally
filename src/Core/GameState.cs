namespace TurboMathRally.Core
{
    /// <summary>
    /// Represents the current state of the game
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Main menu - user can select options and navigate
        /// </summary>
        Menu,
        
        /// <summary>
        /// Math type selection - choose which operations to practice
        /// </summary>
        MathSelection,
        
        /// <summary>
        /// Mode selection - Parent or Kid mode
        /// </summary>
        ModeSelection,
        
        /// <summary>
        /// Rally series selection - choose difficulty level
        /// </summary>
        SeriesSelection,
        
        /// <summary>
        /// Actively playing a rally stage
        /// </summary>
        Playing,
        
        /// <summary>
        /// Car breakdown - solving story problem for repair
        /// </summary>
        CarRepair,
        
        /// <summary>
        /// Stage completed successfully
        /// </summary>
        StageComplete,
        
        /// <summary>
        /// Game over - failed stage or quit
        /// </summary>
        GameOver,
        
        /// <summary>
        /// Parent mode analytics dashboard
        /// </summary>
        ParentDashboard,
        
        /// <summary>
        /// Exit the game
        /// </summary>
        Exit
    }
}
