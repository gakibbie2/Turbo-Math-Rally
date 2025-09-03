using TurboMathRally.Core;
using TurboMathRally.Utils;
using System.IO;
using System.Text;
using System.Linq;

namespace TurboMathRally
{
    /// <summary>
    /// Main menu form for Turbo Math Rally Windows Forms version
    /// </summary>
    public partial class MainMenuForm : Form
    {
        private readonly GameConfiguration _gameConfig;
        private Label? _playerNameLabel;
        
        public MainMenuForm()
        {
            _gameConfig = new GameConfiguration();
            InitializeComponent();
            SetupRallyTheme();
            UpdatePlayerInfo();
        }
        
        private void InitializeComponent()
        {
            // Form setup
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 650);
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
            
            // Race car image
            var raceCarPictureBox = new PictureBox
            {
                Size = new Size(100, 60),
                Location = new Point(350, 155),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            
            // Try to load race car image from assets
            var carImage = AssetManager.GetRaceCarImage("red", 1);
            if (carImage != null)
            {
                raceCarPictureBox.Image = carImage;
            }
            else
            {
                // Fallback to text if image not found
                raceCarPictureBox.Visible = false;
            }
            
            // Start Racing button
            var startRacingButton = new Button
            {
                Text = "🏁 Start Racing",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Size = new Size(300, 60),
                Location = new Point(250, 230),
                BackColor = Color.LimeGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            startRacingButton.Click += StartRacingButton_Click;
            
            // Achievements button
            var achievementsButton = new Button
            {
                Text = "🏆 Achievements",
                Font = new Font("Arial", 14, FontStyle.Regular),
                Size = new Size(250, 50),
                Location = new Point(275, 320),
                BackColor = Color.Gold,
                ForeColor = Color.DarkBlue,
                FlatStyle = FlatStyle.Flat
            };
            achievementsButton.Click += AchievementsButton_Click;
            
            // Settings button
            var settingsButton = new Button
            {
                Text = "⚙️ Settings",
                Font = new Font("Arial", 14, FontStyle.Regular),
                Size = new Size(200, 50),
                Location = new Point(300, 390),
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
                Location = new Point(325, 430),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            exitButton.Click += ExitButton_Click;
            
            // Add controls to form
            this.Controls.Add(titleLabel);
            this.Controls.Add(subtitleLabel);
            this.Controls.Add(raceCarPictureBox);
            this.Controls.Add(startRacingButton);
            this.Controls.Add(achievementsButton);
            this.Controls.Add(settingsButton);
            this.Controls.Add(exitButton);
            
            // Add player info label
            _playerNameLabel = new Label
            {
                Text = "Welcome, Player!",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            this.Controls.Add(_playerNameLabel);
            
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
        
        private void AchievementsButton_Click(object? sender, EventArgs e)
        {
            // Use the global profile manager's achievement manager
            var achievementManager = Program.ProfileManager?.AchievementManager;
            if (achievementManager == null)
            {
                MessageBox.Show("Profile system not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Create an achievement display form
            var achievementForm = new Form
            {
                Text = "🏆 Achievement Gallery",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.LightBlue,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            
            // Create a rich text box to display achievements
            var achievementTextBox = new RichTextBox
            {
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Size = new Size(760, 500),
                Location = new Point(20, 20),
                BackColor = Color.White,
                Font = new Font("Courier New", 10)
            };
            
            // Build achievement display text directly
            var achievementText = new System.Text.StringBuilder();
            achievementText.AppendLine("🏆 TURBO MATH RALLY - ACHIEVEMENT GALLERY 🏆");
            achievementText.AppendLine("===========================================\n");
            
            // Get achievements by category
            var categories = new[]
            {
                TurboMathRally.Core.Achievements.AchievementType.Accuracy,
                TurboMathRally.Core.Achievements.AchievementType.Streak,
                TurboMathRally.Core.Achievements.AchievementType.Speed,
                TurboMathRally.Core.Achievements.AchievementType.Endurance,
                TurboMathRally.Core.Achievements.AchievementType.Series,
                TurboMathRally.Core.Achievements.AchievementType.Mastery,
                TurboMathRally.Core.Achievements.AchievementType.Comeback,
                TurboMathRally.Core.Achievements.AchievementType.Consistency
            };
            
            foreach (var category in categories)
            {
                var categoryAchievements = achievementManager.GetAchievementsByType(category);
                if (categoryAchievements.Any())
                {
                    achievementText.AppendLine($"📂 {category} Achievements:");
                    achievementText.AppendLine(new string('-', 40));
                    
                    foreach (var achievement in categoryAchievements)
                    {
                        var status = achievement.IsUnlocked ? "✅ UNLOCKED" : "🔒 Not Unlocked";
                        var rarity = achievement.GetRarityName();
                        achievementText.AppendLine($"{status} [{rarity}] {achievement.Title}");
                        achievementText.AppendLine($"    📝 {achievement.Description}");
                        if (!achievement.IsUnlocked && achievement.TargetValue > 1)
                        {
                            achievementText.AppendLine($"    📊 Progress: {achievement.CurrentValue}/{achievement.TargetValue}");
                        }
                        achievementText.AppendLine();
                    }
                    achievementText.AppendLine();
                }
            }
            
            achievementTextBox.Text = achievementText.ToString();
            
            // Add close button
            var closeButton = new Button
            {
                Text = "Close",
                Size = new Size(100, 30),
                Location = new Point(350, 530),
                BackColor = Color.LightGray
            };
            closeButton.Click += (s, ev) => achievementForm.Close();
            
            achievementForm.Controls.Add(achievementTextBox);
            achievementForm.Controls.Add(closeButton);
            
            achievementForm.ShowDialog();
        }
        
        private void SettingsButton_Click(object? sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            this.Hide();
            
            var result = settingsForm.ShowDialog();
            
            // Update player info in case it changed
            if (result == DialogResult.OK)
            {
                UpdatePlayerInfo();
            }
            
            this.Show();
        }
        
        private void ExitButton_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit Game", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        
        private void UpdatePlayerInfo()
        {
            if (_playerNameLabel != null && Program.ProfileManager?.CurrentProfile != null && Program.ProfileManager.AchievementManager != null)
            {
                var profile = Program.ProfileManager.CurrentProfile;
                var achievementCount = Program.ProfileManager.AchievementManager.UnlockedAchievements.Count;
                var totalAchievements = Program.ProfileManager.AchievementManager.AllAchievements.Count;
                
                _playerNameLabel.Text = $"Welcome, {profile.PlayerName}! ({achievementCount}/{totalAchievements} achievements)";
            }
        }
    }
}
