using TurboMathRally.Core;

namespace TurboMathRally.WinForms
{
    /// <summary>
    /// Main menu form for Turbo Math Rally Windows Forms version
    /// </summary>
    public partial class MainMenuForm : Form
    {
        private readonly GameConfiguration _gameConfig;
        
        public MainMenuForm()
        {
            _gameConfig = new GameConfiguration();
            InitializeComponent();
            SetupRallyTheme();
        }
        
        private void InitializeComponent()
        {
            // Form setup
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 600);
            this.Text = "🏎️ Turbo Math Rally - Main Menu";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            
            // Title label
            var titleLabel = new Label
            {
                Text = "🏎️ TURBO MATH RALLY",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Size = new Size(700, 60),
                Location = new Point(50, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Subtitle label
            var subtitleLabel = new Label
            {
                Text = "Rev up your math skills in this exciting rally adventure!",
                Font = new Font("Arial", 12, FontStyle.Regular),
                ForeColor = Color.DarkGreen,
                Size = new Size(700, 30),
                Location = new Point(50, 120),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Start Racing button
            var startRacingButton = new Button
            {
                Text = "🏁 Start Racing",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Size = new Size(300, 60),
                Location = new Point(250, 200),
                BackColor = Color.LimeGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            startRacingButton.Click += StartRacingButton_Click;
            
            // Settings button
            var settingsButton = new Button
            {
                Text = "⚙️ Settings",
                Font = new Font("Arial", 14, FontStyle.Regular),
                Size = new Size(200, 50),
                Location = new Point(300, 290),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            settingsButton.Click += SettingsButton_Click;
            
            // Exit button
            var exitButton = new Button
            {
                Text = "🚪 Exit",
                Font = new Font("Arial", 14, FontStyle.Regular),
                Size = new Size(150, 40),
                Location = new Point(325, 370),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            exitButton.Click += ExitButton_Click;
            
            // Add controls to form
            this.Controls.Add(titleLabel);
            this.Controls.Add(subtitleLabel);
            this.Controls.Add(startRacingButton);
            this.Controls.Add(settingsButton);
            this.Controls.Add(exitButton);
            
            this.ResumeLayout(false);
        }
        
        private void SetupRallyTheme()
        {
            // Set rally-themed background color
            this.BackColor = Color.LightSkyBlue;
        }
        
        private void StartRacingButton_Click(object? sender, EventArgs e)
        {
            // Open math type selection form
            var mathTypeForm = new MathTypeSelectionForm(_gameConfig);
            this.Hide();
            
            if (mathTypeForm.ShowDialog() == DialogResult.OK)
            {
                // Continue to series selection or directly to game
                var seriesForm = new SeriesSelectionForm(_gameConfig);
                if (seriesForm.ShowDialog() == DialogResult.OK)
                {
                    // Start the game
                    var gameForm = new GameForm(_gameConfig);
                    gameForm.ShowDialog();
                }
            }
            
            // Return to main menu
            this.Show();
        }
        
        private void SettingsButton_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Settings coming soon!", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void ExitButton_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit Game", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
