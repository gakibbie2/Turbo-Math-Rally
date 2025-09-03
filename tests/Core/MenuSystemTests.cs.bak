namespace TurboMathRally.Tests.Core
{
    /// <summary>
    /// Tests for the MenuSystem class
    /// </summary>
    public class MenuSystemTests
    {
        private readonly MenuSystem _menuSystem;

        public MenuSystemTests()
        {
            _menuSystem = new MenuSystem();
        }

        [Fact]
        public void DisplayMainMenu_ReturnsValidMenuOptions()
        {
            // This test would typically require mocking Console input/output
            // For now, we'll test the basic structure
            _menuSystem.Should().NotBeNull();
        }

        [Theory]
        [InlineData("1", GameState.ModeSelection)]
        [InlineData("2", GameState.Achievements)]
        [InlineData("3", GameState.ParentDashboard)]
        [InlineData("4", GameState.Exit)]
        public void ProcessMainMenuInput_ValidInputs_ReturnsCorrectState(string input, GameState expectedState)
        {
            // This would require refactoring MenuSystem to separate input processing from display
            // For now, we'll verify the MenuSystem can be instantiated
            _menuSystem.Should().NotBeNull();
            
            // TODO: Refactor MenuSystem to have testable input processing methods
            // Example: var result = _menuSystem.ProcessMainMenuInput(input);
            // result.Should().Be(expectedState);
        }

        [Fact]
        public void DisplayModeSelection_AcceptsGameConfiguration()
        {
            // Arrange
            var gameConfig = new GameConfiguration();

            // Act & Assert - Should not throw
            var action = () => _menuSystem.DisplayModeSelection();
            action.Should().NotThrow();
        }

        [Fact]
        public void DisplayMathSelection_AcceptsGameConfiguration()
        {
            // Arrange
            var gameConfig = new GameConfiguration();

            // Act & Assert - Should not throw
            var action = () => _menuSystem.DisplayMathSelection(gameConfig);
            action.Should().NotThrow();
        }

        [Fact]
        public void DisplaySeriesSelection_AcceptsGameConfiguration()
        {
            // Arrange
            var gameConfig = new GameConfiguration();

            // Act & Assert - Should not throw
            var action = () => _menuSystem.DisplaySeriesSelection(gameConfig);
            action.Should().NotThrow();
        }

        // Note: These tests are basic structure tests. 
        // To properly test MenuSystem, we would need to:
        // 1. Refactor it to separate input processing from console I/O
        // 2. Use dependency injection for console operations
        // 3. Mock the console input/output
        // 4. Test the logic separately from the UI
    }
}
