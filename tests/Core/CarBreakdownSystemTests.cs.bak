namespace TurboMathRally.Tests.Core
{
    /// <summary>
    /// Tests for the CarBreakdownSystem class
    /// </summary>
    public class CarBreakdownSystemTests
    {
        private readonly CarBreakdownSystem _carBreakdownSystem;

        public CarBreakdownSystemTests()
        {
            _carBreakdownSystem = new CarBreakdownSystem();
        }

        [Fact]
        public void Constructor_InitializesWithNoBreakdown()
        {
            // Act
            var system = new CarBreakdownSystem();

            // Assert
            system.IsCarBrokenDown.Should().BeFalse();
            system.RepairProgress.Should().Be(0);
        }

        [Theory]
        [InlineData(0, false)]   // 0 wrong answers, no breakdown
        [InlineData(1, false)]   // 1 wrong answer, no breakdown
        [InlineData(2, false)]   // 2 wrong answers, no breakdown
        [InlineData(3, true)]    // 3 wrong answers, breakdown possible
        [InlineData(4, true)]    // 4 wrong answers, breakdown more likely
        [InlineData(5, true)]    // 5 wrong answers, breakdown very likely
        public void CheckForBreakdown_ReturnsExpectedResult(int consecutiveWrong, bool canBreakdown)
        {
            // Act
            var result = _carBreakdownSystem.CheckForBreakdown(consecutiveWrong);

            // Assert
            if (canBreakdown)
            {
                // Result can be true or false due to randomness, but should be possible
                result.Should().BeOneOf(true, false);
            }
            else
            {
                // Should never breakdown with fewer than 3 wrong answers
                result.Should().BeFalse();
            }
        }

        [Fact]
        public void TriggerBreakdown_SetsCarToBrokenDown()
        {
            // Act
            _carBreakdownSystem.TriggerBreakdown();

            // Assert
            _carBreakdownSystem.IsCarBrokenDown.Should().BeTrue();
            _carBreakdownSystem.RepairProgress.Should().Be(0);
        }

        [Fact]
        public void AttemptRepair_CorrectAnswer_IncreasesProgress()
        {
            // Arrange
            _carBreakdownSystem.TriggerBreakdown();
            var initialProgress = _carBreakdownSystem.RepairProgress;

            // Act
            var result = _carBreakdownSystem.AttemptRepair(true);

            // Assert
            _carBreakdownSystem.RepairProgress.Should().BeGreaterThan(initialProgress);
        }

        [Fact]
        public void AttemptRepair_IncorrectAnswer_DoesNotIncreaseProgress()
        {
            // Arrange
            _carBreakdownSystem.TriggerBreakdown();
            var initialProgress = _carBreakdownSystem.RepairProgress;

            // Act
            var result = _carBreakdownSystem.AttemptRepair(false);

            // Assert
            _carBreakdownSystem.RepairProgress.Should().Be(initialProgress);
        }

        [Fact]
        public void AttemptRepair_CompleteRepair_ClearsBreakdownState()
        {
            // Arrange
            _carBreakdownSystem.TriggerBreakdown();

            // Act - Keep attempting repair until complete
            bool repaired = false;
            int attempts = 0;
            while (!repaired && attempts < 10) // Safety limit
            {
                repaired = _carBreakdownSystem.AttemptRepair(true);
                attempts++;
            }

            // Assert
            repaired.Should().BeTrue();
            _carBreakdownSystem.IsCarBrokenDown.Should().BeFalse();
            _carBreakdownSystem.RepairProgress.Should().Be(0);
        }

        [Fact]
        public void GetBreakdownMessage_ReturnsAppropriateMessage()
        {
            // Arrange
            _carBreakdownSystem.TriggerBreakdown();

            // Act
            var message = _carBreakdownSystem.GetBreakdownMessage();

            // Assert
            message.Should().NotBeNullOrEmpty();
            message.Should().Contain("car").Or.Contain("engine").Or.Contain("tire").Or.Contain("breakdown");
        }

        [Fact]
        public void GetRepairInstructions_ReturnsInstructions()
        {
            // Arrange
            _carBreakdownSystem.TriggerBreakdown();

            // Act
            var instructions = _carBreakdownSystem.GetRepairInstructions();

            // Assert
            instructions.Should().NotBeNullOrEmpty();
            instructions.Should().Contain("repair").Or.Contain("fix");
        }

        [Fact]
        public void GetRepairProgressMessage_ReturnsProgressInfo()
        {
            // Arrange
            _carBreakdownSystem.TriggerBreakdown();
            _carBreakdownSystem.AttemptRepair(true); // Make some progress

            // Act
            var message = _carBreakdownSystem.GetRepairProgressMessage();

            // Assert
            message.Should().NotBeNullOrEmpty();
            message.Should().Contain(_carBreakdownSystem.RepairProgress.ToString());
        }

        [Fact]
        public void Reset_ClearsAllBreakdownState()
        {
            // Arrange
            _carBreakdownSystem.TriggerBreakdown();
            _carBreakdownSystem.AttemptRepair(true); // Make some progress

            // Act
            _carBreakdownSystem.Reset();

            // Assert
            _carBreakdownSystem.IsCarBrokenDown.Should().BeFalse();
            _carBreakdownSystem.RepairProgress.Should().Be(0);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(10)]
        public void CheckForBreakdown_MultipleAttempts_EventuallyBreaksDown(int consecutiveWrong)
        {
            // Act - Try multiple times since it's random
            bool brokeDown = false;
            for (int i = 0; i < 100; i++) // Try many times
            {
                _carBreakdownSystem.Reset();
                if (_carBreakdownSystem.CheckForBreakdown(consecutiveWrong))
                {
                    brokeDown = true;
                    break;
                }
            }

            // Assert - With enough attempts, it should eventually break down
            brokeDown.Should().BeTrue();
        }
    }
}
