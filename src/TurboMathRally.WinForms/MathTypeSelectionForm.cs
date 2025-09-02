using TurboMathRally.Core;
using TurboMathRally.Math;

namespace TurboMathRally.WinForms
{
    /// <summary>
    /// Form for selecting math operation type with kid-friendly interface
    /// </summary>
    public partial class MathTypeSelectionForm : Form
    {
        private readonly GameConfiguration _gameConfig;
        
        public MathTypeSelectionForm(GameConfiguration gameConfig)
        {
            _gameConfig = gameConfig;
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 700);
            this.Text = "🎯 Choose Your Math Challenge";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.LightSkyBlue;
            
            // Header
            var headerLabel = new Label
            {
                Text = "🎯 CHOOSE YOUR MATH CHALLENGE",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Size = new Size(800, 50),
                Location = new Point(50, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            var subtitleLabel = new Label
            {
                Text = "Select the type of math problems you want to practice:",
                Font = new Font("Arial", 12, FontStyle.Regular),
                ForeColor = Color.DarkGreen,
                Size = new Size(800, 30),
                Location = new Point(50, 80),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Addition button
            var additionButton = CreateMathButton(
                "➕ Addition Only",
                "Perfect for beginners (3 + 5 = ?)",
                "👶 Best for ages 5-7 | 🎯 Focus: Number sense & counting",
                new Point(50, 140),
                Color.LightGreen,
                () => SelectMathType(MathOperation.Addition, "Addition Only", false)
            );
            
            // Subtraction button  
            var subtractionButton = CreateMathButton(
                "➖ Subtraction Only",
                "Building on addition skills (8 - 3 = ?)",
                "🧒 Best for ages 6-8 | 🎯 Focus: Reverse thinking & logic",
                new Point(450, 140),
                Color.Orange,
                () => SelectMathType(MathOperation.Subtraction, "Subtraction Only", false)
            );
            
            // Multiplication button
            var multiplicationButton = CreateMathButton(
                "✖️ Multiplication Only", 
                "Times tables mastery (4 × 6 = ?)",
                "👦 Best for ages 7-10 | 🎯 Focus: Pattern recognition",
                new Point(50, 320),
                Color.Gold,
                () => SelectMathType(MathOperation.Multiplication, "Multiplication Only", false)
            );
            
            // Division button
            var divisionButton = CreateMathButton(
                "➗ Division Only",
                "Advanced problem solving (24 ÷ 6 = ?)", 
                "👧 Best for ages 8-12 | 🎯 Focus: Logical reasoning",
                new Point(450, 320),
                Color.Plum,
                () => SelectMathType(MathOperation.Division, "Division Only", false)
            );
            
            // Mixed mode button
            var mixedButton = CreateMathButton(
                "🎲 Mixed Problems",
                "All operations for variety and challenge",
                "🏆 Best for ages 9+ | 🎯 Focus: Comprehensive math skills",
                new Point(250, 500),
                Color.RoyalBlue,
                () => SelectMathType(MathOperation.Addition, "Mixed Problems", true)
            );
            
            // Back button
            var backButton = new Button
            {
                Text = "🔙 Back",
                Font = new Font("Arial", 12, FontStyle.Regular),
                Size = new Size(120, 40),
                Location = new Point(50, 620),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            
            // Add all controls
            this.Controls.AddRange(new Control[] {
                headerLabel, subtitleLabel,
                additionButton, subtractionButton, multiplicationButton, divisionButton, mixedButton,
                backButton
            });
            
            this.ResumeLayout(false);
        }
        
        private Panel CreateMathButton(string title, string description, string ageInfo, Point location, Color backColor, Action onClick)
        {
            var panel = new Panel
            {
                Size = new Size(380, 150),
                Location = location,
                BackColor = backColor,
                BorderStyle = BorderStyle.FixedSingle
            };
            
            var titleLabel = new Label
            {
                Text = title,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Size = new Size(360, 30),
                Location = new Point(10, 10),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            
            var descLabel = new Label
            {
                Text = description,
                Font = new Font("Arial", 11, FontStyle.Regular),
                ForeColor = Color.White,
                Size = new Size(360, 40),
                Location = new Point(10, 45),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            
            var ageLabel = new Label
            {
                Text = ageInfo,
                Font = new Font("Arial", 9, FontStyle.Regular),
                ForeColor = Color.White,
                Size = new Size(360, 30),
                Location = new Point(10, 90),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            
            // Make the entire panel clickable instead of having a separate button
            panel.Click += (sender, e) => onClick();
            panel.Cursor = Cursors.Hand;
            
            // Add hover effects
            panel.MouseEnter += (sender, e) =>
            {
                panel.BackColor = Color.FromArgb(
                    System.Math.Min(255, backColor.R + 30),
                    System.Math.Min(255, backColor.G + 30),
                    System.Math.Min(255, backColor.B + 30)
                );
            };
            
            panel.MouseLeave += (sender, e) =>
            {
                panel.BackColor = backColor;
            };
            
            panel.Controls.AddRange(new Control[] { titleLabel, descLabel, ageLabel });
            this.Controls.Add(panel);
            
            return panel; // Return the panel instead of a button
        }
        
        private void SelectMathType(MathOperation operation, string typeName, bool isMixed)
        {
            _gameConfig.SelectedMathType = operation;
            _gameConfig.SelectedMathTypeName = typeName;
            _gameConfig.IsMixedMode = isMixed;
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
