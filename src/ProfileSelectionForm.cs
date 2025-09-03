using TurboMathRally.Core;

namespace TurboMathRally
{
    /// <summary>
    /// Form for selecting or creating player profiles
    /// </summary>
    public partial class ProfileSelectionForm : Form
    {
        private ListBox profileListBox = null!;
        private TextBox newProfileTextBox = null!;
        private Button selectButton = null!;
        private Button createButton = null!;
        private Button deleteButton = null!;
        private Label instructionLabel = null!;
        
        /// <summary>
        /// Selected profile manager instance
        /// </summary>
        public ProfileManager? SelectedProfileManager { get; private set; }

        public ProfileSelectionForm()
        {
            InitializeComponent();
            LoadAvailableProfiles();
        }

        private void InitializeComponent()
        {
            this.Text = "Turbo Math Rally - Select Profile";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Instruction label
            instructionLabel = new Label
            {
                Text = "Select an existing profile or create a new one:",
                Location = new Point(20, 20),
                Size = new Size(460, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            this.Controls.Add(instructionLabel);

            // Profile list
            profileListBox = new ListBox
            {
                Location = new Point(20, 60),
                Size = new Size(300, 200),
                Font = new Font("Segoe UI", 10F)
            };
            profileListBox.SelectedIndexChanged += ProfileListBox_SelectedIndexChanged;
            this.Controls.Add(profileListBox);

            // Select button
            selectButton = new Button
            {
                Text = "Select Profile",
                Location = new Point(340, 60),
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 9F),
                Enabled = false
            };
            selectButton.Click += SelectButton_Click;
            this.Controls.Add(selectButton);

            // Delete button
            deleteButton = new Button
            {
                Text = "Delete Profile",
                Location = new Point(340, 105),
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 9F),
                Enabled = false,
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White
            };
            deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);

            // New profile section
            var newProfileLabel = new Label
            {
                Text = "Or create a new profile:",
                Location = new Point(20, 280),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            this.Controls.Add(newProfileLabel);

            newProfileTextBox = new TextBox
            {
                Location = new Point(20, 310),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Enter player name..."
            };
            newProfileTextBox.TextChanged += NewProfileTextBox_TextChanged;
            newProfileTextBox.KeyPress += NewProfileTextBox_KeyPress;
            this.Controls.Add(newProfileTextBox);

            createButton = new Button
            {
                Text = "Create Profile",
                Location = new Point(340, 308),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 9F),
                Enabled = false,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White
            };
            createButton.Click += CreateButton_Click;
            this.Controls.Add(createButton);
        }

        private void LoadAvailableProfiles()
        {
            try
            {
                var tempProfileManager = new ProfileManager();
                var profiles = tempProfileManager.GetAvailableProfiles();
                
                profileListBox.Items.Clear();
                foreach (var profile in profiles)
                {
                    // Remove .json extension and make it more readable
                    var displayName = Path.GetFileNameWithoutExtension(profile);
                    if (displayName == "default_profile")
                        displayName = "Default Player";
                    else
                        displayName = displayName.Replace('_', ' ');
                    
                    profileListBox.Items.Add(displayName);
                }

                if (profileListBox.Items.Count == 0)
                {
                    profileListBox.Items.Add("No profiles found - Create a new one below");
                    profileListBox.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading profiles: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProfileListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            bool hasSelection = profileListBox.SelectedIndex >= 0 && 
                               profileListBox.Enabled && 
                               profileListBox.SelectedItem?.ToString() != "No profiles found - Create a new one below";
            
            selectButton.Enabled = hasSelection;
            deleteButton.Enabled = hasSelection;
        }

        private void NewProfileTextBox_TextChanged(object? sender, EventArgs e)
        {
            createButton.Enabled = !string.IsNullOrWhiteSpace(newProfileTextBox.Text);
        }

        private void NewProfileTextBox_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && createButton.Enabled)
            {
                CreateButton_Click(sender, e);
            }
        }

        private async void SelectButton_Click(object? sender, EventArgs e)
        {
            if (profileListBox.SelectedItem == null) return;

            try
            {
                string selectedProfile = profileListBox.SelectedItem.ToString()!;
                
                // Convert display name back to filename
                string filename;
                if (selectedProfile == "Default Player")
                    filename = "default_profile.json";
                else
                    filename = selectedProfile.Replace(' ', '_').ToLowerInvariant() + ".json";

                SelectedProfileManager = new ProfileManager();
                await SelectedProfileManager.LoadProfileAsync(filename);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading profile: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CreateButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(newProfileTextBox.Text)) return;

            try
            {
                string playerName = newProfileTextBox.Text.Trim();
                
                // Validate player name
                if (playerName.Length > 20)
                {
                    MessageBox.Show("Player name must be 20 characters or less.", "Invalid Name", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SelectedProfileManager = new ProfileManager();
                await SelectedProfileManager.CreateNewProfileAsync(playerName);

                MessageBox.Show($"Profile '{playerName}' created successfully!", "Profile Created", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating profile: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteButton_Click(object? sender, EventArgs e)
        {
            if (profileListBox.SelectedItem == null) return;

            string selectedProfile = profileListBox.SelectedItem.ToString()!;
            
            var result = MessageBox.Show(
                $"Are you sure you want to delete the profile '{selectedProfile}'?\n\nThis action cannot be undone.",
                "Confirm Delete", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Convert display name back to filename
                    string filename;
                    if (selectedProfile == "Default Player")
                        filename = "default_profile.json";
                    else
                        filename = selectedProfile.Replace(' ', '_').ToLowerInvariant() + ".json";

                    var tempProfileManager = new ProfileManager();
                    tempProfileManager.DeleteProfileAsync(filename);

                    MessageBox.Show("Profile deleted successfully.", "Profile Deleted", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reload the profile list
                    LoadAvailableProfiles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting profile: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
