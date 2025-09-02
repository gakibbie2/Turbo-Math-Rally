using TurboMathRally.Core;
using TurboMathRally.Math;

namespace TurboMathRally.WinForms
{
    /// <summary>
    /// Form for selecting rally series difficulty with visual previews
    /// </summary>
    public partial class SeriesSelectionForm : Form
    {
        private readonly GameConfiguration _gameConfig;
        
        public SeriesSelectionForm(GameConfiguration gameConfig)
        {
            _gameConfig = gameConfig;
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 650);
            this.Text = "🏁 Select Rally Series";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.LightSkyBlue;
            
            // Header
            var headerLabel = new Label
            {
                Text = "🏁 SELECT RALLY SERIES",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Size = new Size(800, 50),
                Location = new Point(50, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            var subtitleLabel = new Label
            {
                Text = "Choose your difficulty level - each series has different challenges:",
                Font = new Font("Arial", 12, FontStyle.Regular),
                ForeColor = Color.DarkGreen,
                Size = new Size(800, 30),
                Location = new Point(50, 80),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Rookie Rally
            CreateSeriesButton(
                "🌲 Rookie Rally",
                "Ages 5-7",
                "📊 25 questions | 🔢 Small numbers (1-10) | ⏱️ No time pressure",
                "🌳 Tracks: Forest Trail, Sunny Park, Sandy Beach",
                "💡 Perfect for: First-time racers, building confidence",
                new Point(50, 140),
                Color.ForestGreen,
                () => SelectSeries(DifficultyLevel.Rookie, "Rookie Rally")
            );
            
            // Junior Championship  
            CreateSeriesButton(
                "🏔️ Junior Championship",
                "Ages 7-9", 
                "📊 35 questions | 🔢 Medium numbers (1-50) | ⏱️ Moderate pace",
                "🏔️ Tracks: Mountain Pass, Desert Dunes, City Streets, Snow Rally",
                "💡 Perfect for: Developing skills, consistent practice",
                new Point(50, 300),
                Color.Orange,
                () => SelectSeries(DifficultyLevel.Junior, "Junior Championship")
            );
            
            // Pro Circuit
            CreateSeriesButton(
                "🏆 Pro Circuit",
                "Ages 9-12",
                "📊 50 questions | 🔢 Challenging numbers (1-100) | ⏱️ Racing pace", 
                "🏆 Tracks: Extreme terrain, advanced rally courses",
                "💡 Perfect for: Math champions, advanced learners",
                new Point(50, 460),
                Color.Purple,
                () => SelectSeries(DifficultyLevel.Pro, "Pro Circuit")
            );
            
            // Pro tip
            var tipLabel = new Label
            {
                Text = "💡 Pro tip: Start with Rookie Rally if you're new to math racing!",
                Font = new Font("Arial", 11, FontStyle.Italic),
                ForeColor = Color.DarkBlue,
                Size = new Size(600, 25),
                Location = new Point(150, 590),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Back button
            var backButton = new Button
            {
                Text = "🔙 Back",
                Font = new Font("Arial", 12, FontStyle.Regular),
                Size = new Size(120, 40),
                Location = new Point(750, 590),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            
            // Add all controls
            this.Controls.AddRange(new Control[] {
                headerLabel, subtitleLabel, tipLabel, backButton
            });
            
            this.ResumeLayout(false);
        }
        
        private void CreateSeriesButton(string title, string ageRange, string stats, string tracks, string perfect, Point location, Color backColor, Action onClick)
        {
            var panel = new Panel
            {
                Size = new Size(800, 130),
                Location = location,
                BackColor = backColor,
                BorderStyle = BorderStyle.FixedSingle
            };
            
            var titleLabel = new Label
            {
                Text = $"{title} ({ageRange})",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(400, 30),
                Location = new Point(15, 10),
                BackColor = Color.Transparent
            };
            
            var statsLabel = new Label
            {
                Text = stats,
                Font = new Font("Arial", 10, FontStyle.Regular),
                ForeColor = Color.White,
                Size = new Size(500, 20),
                Location = new Point(15, 45),
                BackColor = Color.Transparent
            };
            
            var tracksLabel = new Label
            {
                Text = tracks,
                Font = new Font("Arial", 10, FontStyle.Regular),
                ForeColor = Color.White,
                Size = new Size(500, 20),
                Location = new Point(15, 65),
                BackColor = Color.Transparent
            };
            
            var perfectLabel = new Label
            {
                Text = perfect,
                Font = new Font("Arial", 10, FontStyle.Regular),
                ForeColor = Color.White,
                Size = new Size(500, 20),
                Location = new Point(15, 85),
                BackColor = Color.Transparent
            };
            
            var selectButton = new Button
            {
                Text = "🏁 START RACING",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(180, 50),
                Location = new Point(600, 40),
                BackColor = Color.White,
                ForeColor = backColor,
                FlatStyle = FlatStyle.Flat
            };
            
            selectButton.Click += (sender, e) => onClick();
            
            panel.Controls.AddRange(new Control[] { titleLabel, statsLabel, tracksLabel, perfectLabel, selectButton });
            this.Controls.Add(panel);
        }
        
        private void SelectSeries(DifficultyLevel difficulty, string seriesName)
        {
            _gameConfig.SelectedDifficulty = difficulty;
            _gameConfig.SelectedSeriesName = seriesName;
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
