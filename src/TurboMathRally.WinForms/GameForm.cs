using TurboMathRally.Core;
using TurboMathRally.Math;
using TurboMathRally.Utils;

namespace TurboMathRally.WinForms
{
    /// <summary>
    /// Main game form where the racing and math questions happen
    /// </summary>
    public partial class GameForm : Form
    {
        private readonly GameConfiguration _gameConfig;
        private readonly AnswerValidator _answerValidator;
        private readonly ProblemGenerator _problemGenerator;
        private readonly CarBreakdownSystem _carBreakdownSystem;
        private readonly StoryProblemGenerator _storyProblemGenerator;
        
        // Game state
        private int _currentQuestionNumber = 1;
        private int _questionsPerStage;
        private MathProblem _currentProblem;
        private DateTime _raceStartTime;
        private bool _gameCompleted = false;
        
        // UI Controls
        private Label _headerLabel;
        private Label _progressLabel;
        private ProgressBar _progressBar;
        private Label _statsLabel;
        private Label _carStatusLabel;
        private Label _questionLabel;
        private TextBox _answerTextBox;
        private Label _instructionLabel;
        private Label _feedbackLabel;
        private Button _exitButton;
        
        public GameForm(GameConfiguration gameConfig)
        {
            _gameConfig = gameConfig;
            _answerValidator = new AnswerValidator();
            _problemGenerator = new ProblemGenerator();
            _carBreakdownSystem = new CarBreakdownSystem();
            _storyProblemGenerator = new StoryProblemGenerator();
            
            // Set questions per stage based on difficulty
            _questionsPerStage = _gameConfig.SelectedDifficulty switch
            {
                DifficultyLevel.Rookie => 25,    // Ages 5-7: Shorter stages
                DifficultyLevel.Junior => 35,    // Ages 7-9: Medium stages  
                DifficultyLevel.Pro => 50,       // Ages 9-12: Long stages
                _ => 25
            };
            
            _raceStartTime = DateTime.Now;
            
            InitializeComponent();
            StartNextQuestion();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 700);
            this.Text = $"Turbo Math Rally - {_gameConfig.SelectedSeriesName} Rally";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255); // Light blue background
            
            // Prevent form from processing Enter key (which could trigger default buttons)
            this.KeyPreview = true;
            this.KeyDown += GameForm_KeyDown;
            
            // Header
            _headerLabel = new Label
            {
                Text = $"🏁 {_gameConfig.SelectedSeriesName} - {_gameConfig.SelectedMathTypeName}",
                Font = new Font("Arial", 18, FontStyle.Bold),
                Size = new Size(960, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White
            };
            
            // Progress section
            _progressLabel = new Label
            {
                Text = "Question 1 of " + _questionsPerStage,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(200, 25),
                Location = new Point(20, 80),
                ForeColor = Color.FromArgb(25, 25, 112)
            };
            
            _progressBar = new ProgressBar
            {
                Size = new Size(760, 25),
                Location = new Point(220, 80),
                Style = ProgressBarStyle.Continuous,
                ForeColor = Color.FromArgb(34, 139, 34)
            };
            
            // Stats section
            _statsLabel = new Label
            {
                Text = "Accuracy: 0% | Streak: 0 | Best: 0",
                Font = new Font("Arial", 10),
                Size = new Size(300, 20),
                Location = new Point(20, 115),
                ForeColor = Color.FromArgb(139, 69, 19)
            };
            
            _carStatusLabel = new Label
            {
                Text = "🚗 Car Status: Perfect",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(300, 20),
                Location = new Point(680, 115),
                ForeColor = Color.FromArgb(34, 139, 34)
            };
            
            // Keyboard instruction
            _instructionLabel = new Label
            {
                Text = "⌨️ Type your answer and press ENTER to submit! ⚡",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(600, 30),
                Location = new Point(200, 140),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(34, 139, 34)
            };
            
            // Question section
            _questionLabel = new Label
            {
                Text = "Loading question...",
                Font = new Font("Arial", 28, FontStyle.Bold),
                Size = new Size(960, 100),
                Location = new Point(20, 180),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(25, 25, 112),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            
            // Answer text box
            _answerTextBox = new TextBox
            {
                Font = new Font("Arial", 24, FontStyle.Bold),
                Size = new Size(200, 50),
                Location = new Point(400, 310),
                TextAlign = HorizontalAlignment.Center,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(25, 25, 112),
                BorderStyle = BorderStyle.FixedSingle
            };
            _answerTextBox.KeyDown += AnswerTextBox_KeyDown;
            
            // Feedback section
            _feedbackLabel = new Label
            {
                Text = "",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Size = new Size(960, 80),
                Location = new Point(20, 380),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            
            _exitButton = new Button
            {
                Text = "Exit Race",
                Size = new Size(120, 40),
                Location = new Point(440, 480),
                Font = new Font("Arial", 12),
                BackColor = Color.FromArgb(178, 34, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel,
                TabStop = false // Prevent this button from receiving focus via Tab/Enter
            };
            
            // Add all controls
            this.Controls.Add(_headerLabel);
            this.Controls.Add(_progressLabel);
            this.Controls.Add(_progressBar);
            this.Controls.Add(_statsLabel);
            this.Controls.Add(_carStatusLabel);
            this.Controls.Add(_instructionLabel);
            this.Controls.Add(_questionLabel);
            this.Controls.Add(_answerTextBox);
            this.Controls.Add(_feedbackLabel);
            this.Controls.Add(_exitButton);
            
            this.ResumeLayout(false);
        }
        
        private void StartNextQuestion()
        {
            if (_currentQuestionNumber > _questionsPerStage)
            {
                CompleteRace();
                return;
            }
            
            // Generate a problem (handle mixed mode)
            if (_gameConfig.IsMixedMode)
            {
                // For mixed mode, randomly select an operation
                Random random = new Random();
                MathOperation[] operations = { MathOperation.Addition, MathOperation.Subtraction, MathOperation.Multiplication, MathOperation.Division };
                MathOperation randomOperation = operations[random.Next(operations.Length)];
                _currentProblem = _problemGenerator.GenerateProblem(randomOperation, _gameConfig.SelectedDifficulty);
            }
            else
            {
                _currentProblem = _problemGenerator.GenerateProblem(_gameConfig.SelectedMathType, _gameConfig.SelectedDifficulty);
            }
            
            UpdateUI();
            
            // Clear and focus the text box
            _answerTextBox.Clear();
            _answerTextBox.Focus();
            
            // Hide feedback
            _feedbackLabel.Visible = false;
        }
        
        private void UpdateUI()
        {
            // Update progress
            _progressLabel.Text = $"Question {_currentQuestionNumber} of {_questionsPerStage}";
            _progressBar.Maximum = _questionsPerStage;
            _progressBar.Value = _currentQuestionNumber - 1;
            
            // Update stats
            _statsLabel.Text = $"Accuracy: {_answerValidator.AccuracyPercentage:F0}% | Streak: {_answerValidator.CurrentStreak} | Best: {_answerValidator.BestStreak}";
            
            // Update car status
            var carStatus = _carBreakdownSystem.GetCompactCarStatus();
            _carStatusLabel.Text = $"🚗 {carStatus}";
            
            // Update car status color based on condition
            if (carStatus.Contains("Perfect"))
                _carStatusLabel.ForeColor = Color.FromArgb(34, 139, 34);
            else if (carStatus.Contains("Minor") || carStatus.Contains("Light"))
                _carStatusLabel.ForeColor = Color.FromArgb(255, 165, 0);
            else if (carStatus.Contains("Moderate"))
                _carStatusLabel.ForeColor = Color.FromArgb(255, 69, 0);
            else
                _carStatusLabel.ForeColor = Color.FromArgb(178, 34, 34);
            
            // Update question
            _questionLabel.Text = _currentProblem.Question;
        }
        
        
        private void AnswerTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true; // Prevent the "ding" sound and further processing
                e.SuppressKeyPress = true; // Completely suppress the key press
                ProcessAnswer();
            }
        }
        
        private void ProcessAnswer()
        {
            string userInput = _answerTextBox.Text.Trim();
            
            if (string.IsNullOrEmpty(userInput))
            {
                return; // Don't process empty input
            }
            
            // Disable the text box temporarily
            _answerTextBox.Enabled = false;
            
            // Validate the answer
            ValidationResult result = _answerValidator.ValidateAnswer(_currentProblem, userInput);
            
            // Show feedback
            _feedbackLabel.Visible = true;
            
            if (result.IsCorrect)
            {
                _feedbackLabel.Text = "🎉 Correct! Your car speeds ahead! 🚀";
                _feedbackLabel.ForeColor = Color.FromArgb(34, 139, 34);
                _answerTextBox.BackColor = Color.FromArgb(144, 238, 144);
            }
            else
            {
                _feedbackLabel.Text = $"❌ Wrong! The correct answer was {result.CorrectAnswer}";
                _feedbackLabel.ForeColor = Color.FromArgb(178, 34, 34);
                _answerTextBox.BackColor = Color.FromArgb(255, 182, 193);
                
                // Add strike to car breakdown system
                StrikeResult strikeResult = _carBreakdownSystem.AddStrike();
                
                if (strikeResult.WarningLevel == WarningLevel.Breakdown)
                {
                    // Car breakdown - show repair form
                    ShowCarRepair();
                    return;
                }
                else if (strikeResult.WarningLevel == WarningLevel.Light || strikeResult.WarningLevel == WarningLevel.Moderate)
                {
                    _feedbackLabel.Text += $"\n🔧 {strikeResult.Message}";
                }
            }
            
            // Update stats display
            UpdateUI();
            
            // Auto-advance to next question after a short delay
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1500; // 1.5 second delay
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                _currentQuestionNumber++;
                _answerTextBox.BackColor = Color.White; // Reset color
                _answerTextBox.Enabled = true;
                StartNextQuestion();
                _answerTextBox.Focus(); // Ensure focus returns to text box
            };
            timer.Start();
        }
        
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Only allow Enter key to be processed by the answer text box
            if (e.KeyCode == Keys.Enter)
            {
                // If the answer text box is enabled and has focus, let it handle the Enter
                if (_answerTextBox.Enabled && _answerTextBox.Focused)
                {
                    // Let the text box handle it
                    return;
                }
                else
                {
                    // Suppress Enter key for everything else to prevent unwanted button clicks
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }
        
        private void ShowCarRepair()
        {
            // Create and show car repair dialog
            using (var repairForm = new CarRepairForm(_gameConfig, _carBreakdownSystem, _storyProblemGenerator))
            {
                if (repairForm.ShowDialog() == DialogResult.OK)
                {
                    // Repair successful, continue race
                    _feedbackLabel.Text = "🔧 Repair completed! Your car is ready to race again! 🚗💨";
                    _feedbackLabel.ForeColor = Color.FromArgb(34, 139, 34);
                    _feedbackLabel.Visible = true;
                    UpdateUI(); // Refresh car status
                    
                    // Auto-continue after repair
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Interval = 2000;
                    timer.Tick += (s, e) =>
                    {
                        timer.Stop();
                        _answerTextBox.BackColor = Color.White;
                        _answerTextBox.Enabled = true;
                        _currentQuestionNumber++;
                        StartNextQuestion();
                    };
                    timer.Start();
                }
                else
                {
                    // Repair failed or cancelled, end race
                    MessageBox.Show("Race ended due to car breakdown.", "Race Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }
        
        private void CompleteRace()
        {
            _gameCompleted = true;
            
            // Calculate race time
            DateTime raceEndTime = DateTime.Now;
            int totalRaceTimeSeconds = (int)(raceEndTime - _raceStartTime).TotalSeconds;
            
            // Show completion message
            string completionMessage = $"🏆 RACE COMPLETED! 🏆\n\n" +
                $"📊 Final Stats:\n" +
                $"Questions Answered: {_answerValidator.TotalQuestions}\n" +
                $"Accuracy: {_answerValidator.AccuracyPercentage:F1}%\n" +
                $"Best Streak: {_answerValidator.BestStreak}\n" +
                $"Race Time: {totalRaceTimeSeconds / 60}:{totalRaceTimeSeconds % 60:D2}\n" +
                $"Questions per minute: {(_answerValidator.TotalQuestions / System.Math.Max(1, totalRaceTimeSeconds / 60.0)):F1}";
            
            MessageBox.Show(completionMessage, "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
