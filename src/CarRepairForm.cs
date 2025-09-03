using TurboMathRally.Core;
using TurboMathRally.Math;
using TurboMathRally.Utils;

namespace TurboMathRally
{
    /// <summary>
    /// Form for handling car repair story problems when car breaks down
    /// </summary>
    public partial class CarRepairForm : Form
    {
        private readonly GameConfiguration _gameConfig;
        private readonly CarBreakdownSystem _carBreakdownSystem;
        private readonly StoryProblemGenerator _storyProblemGenerator;
        private readonly AnswerValidator _answerValidator;
        private readonly SoundManager _soundManager;
        
        private StoryProblem _currentStoryProblem = null!;
        private int _repairAttempts = 0;
        private const int MAX_REPAIR_ATTEMPTS = 3;
        
        // UI Controls - initialized in InitializeComponent
        private Label _headerLabel = null!;
        private Label _carStatusLabel = null!;
        private Label _storyLabel = null!;
        private Label _contextLabel = null!;
        private Label _questionLabel = null!;
        private TextBox _answerTextBox = null!;
        private Label _instructionLabel = null!;
        private Label _feedbackLabel = null!;
        private Button _tryAgainButton = null!;
        private Button _giveUpButton = null!;
        
        public CarRepairForm(GameConfiguration gameConfig, CarBreakdownSystem carBreakdownSystem, StoryProblemGenerator storyProblemGenerator)
        {
            _gameConfig = gameConfig;
            _carBreakdownSystem = carBreakdownSystem;
            _storyProblemGenerator = storyProblemGenerator;
            _answerValidator = new AnswerValidator();
            
            // Initialize sound manager with user's volume preference
            if (Program.ProfileManager?.CurrentProfile != null)
            {
                _soundManager = new SoundManager(Program.ProfileManager.CurrentProfile.Settings.SoundVolume);
            }
            else
            {
                _soundManager = new SoundManager(0.7f);
            }
            
            InitializeComponent();
            StartRepair();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 650);
            this.Text = "🔧 Emergency Car Repair";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(255, 248, 220); // Cornsilk background for repair shop feel
            
            // Prevent unwanted Enter key processing
            this.KeyPreview = true;
            this.KeyDown += CarRepairForm_KeyDown;
            
            // Header
            _headerLabel = new Label
            {
                Text = "🔧 EMERGENCY CAR REPAIR 🔧",
                Font = new Font("Arial", 18, FontStyle.Bold),
                Size = new Size(860, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(178, 34, 34),
                ForeColor = Color.White
            };
            
            // Car status
            _carStatusLabel = new Label
            {
                Text = "🚗 Your rally car has broken down!",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(860, 30),
                Location = new Point(20, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(178, 34, 34)
            };
            
            // Story section
            _storyLabel = new Label
            {
                Text = "A friendly mechanic offers to help...",
                Font = new Font("Arial", 11),
                Size = new Size(860, 60),
                Location = new Point(20, 120),
                TextAlign = ContentAlignment.TopLeft,
                ForeColor = Color.FromArgb(25, 25, 112)
            };
            
            _contextLabel = new Label
            {
                Text = "",
                Font = new Font("Arial", 10, FontStyle.Italic),
                Size = new Size(860, 40),
                Location = new Point(20, 190),
                TextAlign = ContentAlignment.TopLeft,
                ForeColor = Color.FromArgb(139, 69, 19)
            };
            
            // Question section
            _questionLabel = new Label
            {
                Text = "",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Size = new Size(860, 60),
                Location = new Point(20, 240),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(25, 25, 112),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            
            // Answer text box
            _answerTextBox = new TextBox
            {
                Font = new Font("Arial", 18, FontStyle.Bold),
                Size = new Size(180, 40),
                Location = new Point(360, 320),
                TextAlign = HorizontalAlignment.Center,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(25, 25, 112),
                BorderStyle = BorderStyle.FixedSingle
            };
            _answerTextBox.KeyDown += AnswerTextBox_KeyDown;
            
            // Instruction label
            _instructionLabel = new Label
            {
                Text = "⌨️ Type your answer and press ENTER to fix the car! ⚡",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(860, 30),
                Location = new Point(20, 380),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(34, 139, 34),
                BackColor = Color.Transparent
            };
            
            // Feedback
            _feedbackLabel = new Label
            {
                Text = "",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(860, 80),
                Location = new Point(20, 420),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            
            // Control buttons
            _tryAgainButton = new Button
            {
                Text = "Try New Repair",
                Size = new Size(150, 40),
                Location = new Point(500, 550),
                Font = new Font("Arial", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(34, 139, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Visible = false,
                TabStop = false // Prevent focus via Tab/Enter
            };
            _tryAgainButton.Click += TryAgainButton_Click;
            
            _giveUpButton = new Button
            {
                Text = "Abandon Race",
                Size = new Size(150, 40),
                Location = new Point(670, 550),
                Font = new Font("Arial", 11),
                BackColor = Color.FromArgb(178, 34, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel,
                TabStop = false // Prevent focus via Tab/Enter
            };
            
            // Add all controls
            this.Controls.Add(_headerLabel);
            this.Controls.Add(_carStatusLabel);
            this.Controls.Add(_storyLabel);
            this.Controls.Add(_contextLabel);
            this.Controls.Add(_questionLabel);
            this.Controls.Add(_answerTextBox);
            this.Controls.Add(_instructionLabel);
            this.Controls.Add(_feedbackLabel);
            this.Controls.Add(_tryAgainButton);
            this.Controls.Add(_giveUpButton);
            
            this.ResumeLayout(false);
        }
        
        private void CarRepairForm_KeyDown(object? sender, KeyEventArgs e)
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
        
        private void StartRepair()
        {
            _repairAttempts++;
            
            // Update car status display
            _carStatusLabel.Text = $"🚗 {_carBreakdownSystem.GetCompactCarStatus()}";
            
            // Generate a story problem for repair
            _currentStoryProblem = _storyProblemGenerator.GenerateRepairStoryProblem(
                _gameConfig.SelectedDifficulty,
                _gameConfig.SelectedMathType,
                _gameConfig.IsMixedMode);
            
            // Update UI
            _storyLabel.Text = $"📖 {_currentStoryProblem.StoryText}";
            _contextLabel.Text = $"🔧 Context: {_currentStoryProblem.Context}";
            _questionLabel.Text = $"{_currentStoryProblem.Number1} {GetOperationSymbol(_currentStoryProblem.Operation)} {_currentStoryProblem.Number2} = ?";
            
            // Clear and focus the text box
            _answerTextBox.Clear();
            _answerTextBox.Focus();
            _answerTextBox.BackColor = Color.White;
            _answerTextBox.Enabled = true;
            
            // Hide feedback and try again button
            _feedbackLabel.Visible = false;
            _tryAgainButton.Visible = false;
        }
        
        private string GetOperationSymbol(MathOperation operation)
        {
            return operation switch
            {
                MathOperation.Addition => "+",
                MathOperation.Subtraction => "-",
                MathOperation.Multiplication => "×",
                MathOperation.Division => "÷",
                _ => "+"
            };
        }
        
        private void AnswerTextBox_KeyDown(object? sender, KeyEventArgs e)
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
            
            // Create a temporary MathProblem for validation
            var tempMathProblem = new MathProblem(
                _currentStoryProblem.Operation,
                _currentStoryProblem.Number1,
                _currentStoryProblem.Number2,
                _currentStoryProblem.Difficulty);
            
            // Validate the answer
            ValidationResult result = _answerValidator.ValidateAnswer(tempMathProblem, userInput);
            
            // Show feedback
            _feedbackLabel.Visible = true;
            
            if (result.IsCorrect)
            {
                _feedbackLabel.Text = "🎉 Perfect! The mechanic fixes your car! 🔧✨";
                _feedbackLabel.ForeColor = Color.FromArgb(34, 139, 34);
                _answerTextBox.BackColor = Color.FromArgb(144, 238, 144);
                
                // Play repair success sound
                _soundManager.PlayCarRepaired();
                
                // Repair the car
                _carBreakdownSystem.Reset();
                
                // Show success and close after delay
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 2000;
                timer.Tick += (s, e) =>
                {
                    timer.Stop();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };
                timer.Start();
            }
            else
            {
                _feedbackLabel.Text = $"❌ That's not right! The correct answer was {result.CorrectAnswer}";
                _feedbackLabel.ForeColor = Color.FromArgb(178, 34, 34);
                _answerTextBox.BackColor = Color.FromArgb(255, 182, 193);
                
                if (_repairAttempts < MAX_REPAIR_ATTEMPTS)
                {
                    _feedbackLabel.Text += $"\n🔧 Don't give up! Try another repair problem. ({MAX_REPAIR_ATTEMPTS - _repairAttempts} attempts left)";
                    _tryAgainButton.Visible = true;
                }
                else
                {
                    _feedbackLabel.Text += "\n💥 Your car can't be repaired! The race is over.";
                    
                    // Auto-close after showing final message
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Interval = 3000;
                    timer.Tick += (s, e) =>
                    {
                        timer.Stop();
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    };
                    timer.Start();
                }
            }
        }
        
        private void TryAgainButton_Click(object? sender, EventArgs e)
        {
            if (_repairAttempts < MAX_REPAIR_ATTEMPTS)
            {
                StartRepair();
            }
        }
    }
}