using TurboMathRally.Core;
using TurboMathRally.Math;

namespace TurboMathRally
{
    /// <summary>
    /// Settings form for managing user preferences
    /// </summary>
    public partial class SettingsForm : Form
    {
        private readonly UserSettings _settings;
        private readonly ProfileManager? _profileManager;
        
        // UI Controls - initialized in InitializeComponent
        private TrackBar _soundVolumeTrackBar = null!;
        private TrackBar _musicVolumeTrackBar = null!;
        private CheckBox _showHintsCheckBox = null!;
        private CheckBox _showAchievementNotificationsCheckBox = null!;
        private ComboBox _preferredDifficultyComboBox = null!;
        private ComboBox _preferredMathTypeComboBox = null!;
        private CheckBox _preferMixedModeCheckBox = null!;
        private ComboBox _themeComboBox = null!;
        private CheckBox _autoSaveCheckBox = null!;
        private CheckBox _showDetailedStatsCheckBox = null!;
        private TextBox _playerNameTextBox = null!;
        
        public SettingsForm()
        {
            _profileManager = Program.ProfileManager;
            _settings = _profileManager?.CurrentProfile?.Settings ?? new UserSettings();
            
            InitializeComponent();
            LoadCurrentSettings();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(600, 700);
            this.Text = "⚙️ Settings";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.LightGray;
            
            int yPos = 20;
            
            // Header
            var headerLabel = new Label
            {
                Text = "⚙️ TURBO MATH RALLY SETTINGS",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Size = new Size(560, 30),
                Location = new Point(20, yPos),
                TextAlign = ContentAlignment.MiddleCenter
            };
            yPos += 50;
            
            // Player Name
            var playerNameLabel = new Label
            {
                Text = "👤 Player Name:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Size = new Size(120, 25),
                Location = new Point(20, yPos)
            };
            
            _playerNameTextBox = new TextBox
            {
                Size = new Size(200, 25),
                Location = new Point(150, yPos),
                Font = new Font("Arial", 10)
            };
            yPos += 40;
            
            // Audio Settings Section
            var audioSectionLabel = new Label
            {
                Text = "🎵 AUDIO SETTINGS",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Size = new Size(560, 25),
                Location = new Point(20, yPos)
            };
            yPos += 35;
            
            // Sound Volume
            var soundVolumeLabel = new Label
            {
                Text = "🔊 Sound Effects Volume:",
                Size = new Size(180, 25),
                Location = new Point(20, yPos)
            };
            
            _soundVolumeTrackBar = new TrackBar
            {
                Size = new Size(200, 45),
                Location = new Point(200, yPos - 10),
                Minimum = 0,
                Maximum = 100,
                TickFrequency = 10,
                LargeChange = 10,
                SmallChange = 5
            };
            yPos += 40;
            
            // Music Volume
            var musicVolumeLabel = new Label
            {
                Text = "🎶 Background Music Volume:",
                Size = new Size(180, 25),
                Location = new Point(20, yPos)
            };
            
            _musicVolumeTrackBar = new TrackBar
            {
                Size = new Size(200, 45),
                Location = new Point(200, yPos - 10),
                Minimum = 0,
                Maximum = 100,
                TickFrequency = 10,
                LargeChange = 10,
                SmallChange = 5
            };
            yPos += 50;
            
            // Gameplay Settings Section
            var gameplaySectionLabel = new Label
            {
                Text = "🎮 GAMEPLAY SETTINGS",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Size = new Size(560, 25),
                Location = new Point(20, yPos)
            };
            yPos += 35;
            
            // Show Hints
            _showHintsCheckBox = new CheckBox
            {
                Text = "💡 Show hints for wrong answers",
                Size = new Size(300, 25),
                Location = new Point(20, yPos)
            };
            yPos += 30;
            
            // Show Achievement Notifications
            _showAchievementNotificationsCheckBox = new CheckBox
            {
                Text = "🏆 Show achievement notifications",
                Size = new Size(300, 25),
                Location = new Point(20, yPos)
            };
            yPos += 30;
            
            // Show Detailed Stats
            _showDetailedStatsCheckBox = new CheckBox
            {
                Text = "📊 Show detailed statistics",
                Size = new Size(300, 25),
                Location = new Point(20, yPos)
            };
            yPos += 40;
            
            // Preferred Difficulty
            var difficultyLabel = new Label
            {
                Text = "🎯 Preferred Difficulty:",
                Size = new Size(150, 25),
                Location = new Point(20, yPos)
            };
            
            _preferredDifficultyComboBox = new ComboBox
            {
                Size = new Size(150, 25),
                Location = new Point(170, yPos),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _preferredDifficultyComboBox.Items.AddRange(new[] { "Rookie", "Junior", "Pro" });
            yPos += 35;
            
            // Preferred Math Type
            var mathTypeLabel = new Label
            {
                Text = "🧮 Preferred Math Type:",
                Size = new Size(150, 25),
                Location = new Point(20, yPos)
            };
            
            _preferredMathTypeComboBox = new ComboBox
            {
                Size = new Size(150, 25),
                Location = new Point(170, yPos),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _preferredMathTypeComboBox.Items.AddRange(new[] { "Addition", "Subtraction", "Multiplication", "Division" });
            yPos += 35;
            
            // Prefer Mixed Mode
            _preferMixedModeCheckBox = new CheckBox
            {
                Text = "🎲 Prefer mixed math operations",
                Size = new Size(300, 25),
                Location = new Point(20, yPos)
            };
            yPos += 40;
            
            // Visual Settings Section
            var visualSectionLabel = new Label
            {
                Text = "🎨 VISUAL SETTINGS",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Size = new Size(560, 25),
                Location = new Point(20, yPos)
            };
            yPos += 35;
            
            // Theme
            var themeLabel = new Label
            {
                Text = "🎨 Theme:",
                Size = new Size(80, 25),
                Location = new Point(20, yPos)
            };
            
            _themeComboBox = new ComboBox
            {
                Size = new Size(120, 25),
                Location = new Point(100, yPos),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _themeComboBox.Items.AddRange(new[] { "Rally", "Light", "Dark" });
            yPos += 40;
            
            // System Settings Section
            var systemSectionLabel = new Label
            {
                Text = "💾 SYSTEM SETTINGS",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Size = new Size(560, 25),
                Location = new Point(20, yPos)
            };
            yPos += 35;
            
            // Auto Save
            _autoSaveCheckBox = new CheckBox
            {
                Text = "💾 Automatically save progress",
                Size = new Size(300, 25),
                Location = new Point(20, yPos)
            };
            yPos += 50;
            
            // Buttons
            var saveButton = new Button
            {
                Text = "💾 Save Settings",
                Size = new Size(120, 40),
                Location = new Point(200, yPos),
                BackColor = Color.LimeGreen,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            saveButton.Click += SaveButton_Click;
            
            var cancelButton = new Button
            {
                Text = "❌ Cancel",
                Size = new Size(100, 40),
                Location = new Point(340, yPos),
                BackColor = Color.Crimson,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            
            var resetButton = new Button
            {
                Text = "🔄 Reset to Defaults",
                Size = new Size(140, 40),
                Location = new Point(50, yPos),
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            resetButton.Click += ResetButton_Click;
            
            // Add all controls
            this.Controls.AddRange(new Control[] {
                headerLabel, playerNameLabel, _playerNameTextBox,
                audioSectionLabel, soundVolumeLabel, _soundVolumeTrackBar,
                musicVolumeLabel, _musicVolumeTrackBar,
                gameplaySectionLabel, _showHintsCheckBox, _showAchievementNotificationsCheckBox,
                _showDetailedStatsCheckBox, difficultyLabel, _preferredDifficultyComboBox,
                mathTypeLabel, _preferredMathTypeComboBox, _preferMixedModeCheckBox,
                visualSectionLabel, themeLabel, _themeComboBox,
                systemSectionLabel, _autoSaveCheckBox,
                saveButton, cancelButton, resetButton
            });
            
            this.ResumeLayout(false);
        }
        
        private void LoadCurrentSettings()
        {
            _playerNameTextBox.Text = _profileManager?.CurrentProfile?.PlayerName ?? "Young Racer";
            _soundVolumeTrackBar.Value = (int)(_settings.SoundVolume * 100);
            _musicVolumeTrackBar.Value = (int)(_settings.MusicVolume * 100);
            _showHintsCheckBox.Checked = _settings.ShowHints;
            _showAchievementNotificationsCheckBox.Checked = _settings.ShowAchievementNotifications;
            _preferredDifficultyComboBox.SelectedItem = _settings.PreferredDifficulty.ToString();
            _preferredMathTypeComboBox.SelectedItem = _settings.PreferredMathType.ToString();
            _preferMixedModeCheckBox.Checked = _settings.PreferMixedMode;
            _themeComboBox.SelectedItem = _settings.Theme;
            _autoSaveCheckBox.Checked = _settings.AutoSave;
            _showDetailedStatsCheckBox.Checked = _settings.ShowDetailedStats;
        }
        
        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            try
            {
                // Update settings from form
                _settings.SoundVolume = _soundVolumeTrackBar.Value / 100f;
                _settings.MusicVolume = _musicVolumeTrackBar.Value / 100f;
                _settings.ShowHints = _showHintsCheckBox.Checked;
                _settings.ShowAchievementNotifications = _showAchievementNotificationsCheckBox.Checked;
                
                if (Enum.TryParse<DifficultyLevel>(_preferredDifficultyComboBox.SelectedItem?.ToString(), out var difficulty))
                    _settings.PreferredDifficulty = difficulty;
                
                if (Enum.TryParse<MathOperation>(_preferredMathTypeComboBox.SelectedItem?.ToString(), out var mathType))
                    _settings.PreferredMathType = mathType;
                
                _settings.PreferMixedMode = _preferMixedModeCheckBox.Checked;
                _settings.Theme = _themeComboBox.SelectedItem?.ToString() ?? "Rally";
                _settings.AutoSave = _autoSaveCheckBox.Checked;
                _settings.ShowDetailedStats = _showDetailedStatsCheckBox.Checked;
                
                // Update player name if changed
                if (_profileManager?.CurrentProfile != null)
                {
                    _profileManager.CurrentProfile.PlayerName = _playerNameTextBox.Text.Trim();
                    await _profileManager.UpdateSettingsAsync(_settings);
                }
                
                MessageBox.Show("Settings saved successfully!", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ResetButton_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Reset all settings to defaults?", "Reset Settings", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var defaultSettings = new UserSettings();
                _soundVolumeTrackBar.Value = (int)(defaultSettings.SoundVolume * 100);
                _musicVolumeTrackBar.Value = (int)(defaultSettings.MusicVolume * 100);
                _showHintsCheckBox.Checked = defaultSettings.ShowHints;
                _showAchievementNotificationsCheckBox.Checked = defaultSettings.ShowAchievementNotifications;
                _preferredDifficultyComboBox.SelectedItem = defaultSettings.PreferredDifficulty.ToString();
                _preferredMathTypeComboBox.SelectedItem = defaultSettings.PreferredMathType.ToString();
                _preferMixedModeCheckBox.Checked = defaultSettings.PreferMixedMode;
                _themeComboBox.SelectedItem = defaultSettings.Theme;
                _autoSaveCheckBox.Checked = defaultSettings.AutoSave;
                _showDetailedStatsCheckBox.Checked = defaultSettings.ShowDetailedStats;
            }
        }
    }
}
