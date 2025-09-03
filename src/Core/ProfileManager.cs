using System.Text.Json;
using System.Text.Json.Serialization;
using TurboMathRally.Math;
using TurboMathRally.Core.Achievements;

namespace TurboMathRally.Core
{
    /// <summary>
    /// Manages user profile persistence, loading, and saving
    /// </summary>
    public class ProfileManager
    {
        private static readonly string _savesDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TurboMathRally",
            "Saves"
        );
        
        private static readonly string _defaultProfilePath = Path.Combine(_savesDirectory, "default_profile.json");
        private static readonly string _settingsPath = Path.Combine(_savesDirectory, "app_settings.json");
        
        private static string GetSavesDirectory()
        {
            // This method is no longer used but kept for compatibility
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var srcDir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(baseDir)));
            return Path.Combine(srcDir ?? baseDir, "Saves");
        }
        
        private readonly JsonSerializerOptions _jsonOptions;
        
        /// <summary>
        /// Current active user profile
        /// </summary>
        public UserProfile? CurrentProfile { get; private set; }
        
        /// <summary>
        /// Path to the currently loaded profile file
        /// </summary>
        private string? CurrentProfilePath { get; set; }
        
        /// <summary>
        /// Achievement manager for tracking achievements
        /// </summary>
        public AchievementManager AchievementManager { get; private set; }
        
        /// <summary>
        /// Event fired when profile is loaded or changed
        /// </summary>
        public event EventHandler<UserProfile>? ProfileChanged;
        
        /// <summary>
        /// Event fired when profile save occurs
        /// </summary>
        public event EventHandler<string>? ProfileSaved;
        
        /// <summary>
        /// Initialize the profile manager
        /// </summary>
        public ProfileManager()
        {
            AchievementManager = new AchievementManager(this);
            
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
            
            EnsureDirectoryExists();
        }
        
        /// <summary>
        /// Load the default profile or create a new one
        /// </summary>
        public async Task<UserProfile> LoadDefaultProfileAsync()
        {
            try
            {
                if (File.Exists(_defaultProfilePath))
                {
                    var json = await File.ReadAllTextAsync(_defaultProfilePath);
                    var profile = JsonSerializer.Deserialize<UserProfile>(json, _jsonOptions);
                    
                    if (profile != null)
                    {
                        // Update last played date
                        profile.LastPlayedDate = DateTime.Now;
                        CurrentProfile = profile;
                        
                        // Update the achievement manager with the new profile
                        AchievementManager.UpdateProfileManager(this);
                        
                        ProfileChanged?.Invoke(this, profile);
                        return profile;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but continue with new profile creation
                Console.WriteLine($"Error loading profile: {ex.Message}");
            }
            
            // Create new profile if loading failed or file doesn't exist
            return await CreateNewProfileAsync("Young Racer");
        }
        
        /// <summary>
        /// Save the current profile to disk
        /// </summary>
        public async Task SaveCurrentProfileAsync()
        {
            if (CurrentProfile == null)
                throw new InvalidOperationException("No profile is currently loaded");
            
            try
            {
                CurrentProfile.LastPlayedDate = DateTime.Now;
                var json = JsonSerializer.Serialize(CurrentProfile, _jsonOptions);
                
                // Use current profile path if available, otherwise use default
                string savePath = CurrentProfilePath ?? _defaultProfilePath;
                await File.WriteAllTextAsync(savePath, json);
                ProfileSaved?.Invoke(this, savePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save profile: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// Update achievement progress in the current profile
        /// </summary>
        public async Task UpdateAchievementProgressAsync(string achievementId, bool isUnlocked, int currentProgress = 0)
        {
            if (CurrentProfile == null)
                throw new InvalidOperationException("No profile is currently loaded");
            
            CurrentProfile.AchievementData.UnlockedAchievements[achievementId] = isUnlocked;
            CurrentProfile.AchievementData.ProgressValues[achievementId] = currentProgress;
            
            if (isUnlocked && !CurrentProfile.AchievementData.UnlockDates.ContainsKey(achievementId))
            {
                CurrentProfile.AchievementData.UnlockDates[achievementId] = DateTime.Now;
            }
            
            await SaveCurrentProfileAsync();
        }
        
        /// <summary>
        /// Update overall game statistics
        /// </summary>
        public async Task UpdateGameStatisticsAsync(GameStatistics stats)
        {
            if (CurrentProfile == null)
                throw new InvalidOperationException("No profile is currently loaded");
            
            CurrentProfile.OverallStats = stats;
            CurrentProfile.OverallStats.SessionsPlayed++;
            await SaveCurrentProfileAsync();
        }
        
        /// <summary>
        /// Record a completed game session
        /// </summary>
        public async Task RecordSessionAsync(SessionRecord session)
        {
            if (CurrentProfile == null)
                throw new InvalidOperationException("No profile is currently loaded");
            
            CurrentProfile.SessionHistory.Add(session);
            
            // Update overall statistics
            CurrentProfile.OverallStats.TotalQuestions += session.QuestionsAnswered;
            CurrentProfile.OverallStats.CorrectAnswers += session.CorrectAnswers;
            CurrentProfile.OverallStats.StagesCompleted += session.StagesCompleted;
            CurrentProfile.OverallStats.TotalPlayTime = CurrentProfile.OverallStats.TotalPlayTime.Add(session.Duration);
            
            if (session.BestStreak > CurrentProfile.OverallStats.BestStreak)
            {
                CurrentProfile.OverallStats.BestStreak = session.BestStreak;
            }
            
            // Update favorite math operation and difficulty based on most recent preferences
            CurrentProfile.OverallStats.FavoriteMathOperation = session.MathType;
            CurrentProfile.OverallStats.FavoriteDifficulty = session.Difficulty;
            
            // Calculate new average response time
            var totalSessions = CurrentProfile.SessionHistory.Count;
            if (totalSessions > 0)
            {
                var totalResponseTime = CurrentProfile.SessionHistory.Sum(s => s.AverageResponseTime * s.QuestionsAnswered);
                var totalQuestions = CurrentProfile.SessionHistory.Sum(s => s.QuestionsAnswered);
                CurrentProfile.OverallStats.AverageResponseTime = totalQuestions > 0 ? totalResponseTime / totalQuestions : 0;
            }
            
            // Keep only last 100 sessions to prevent file bloat
            if (CurrentProfile.SessionHistory.Count > 100)
            {
                CurrentProfile.SessionHistory = CurrentProfile.SessionHistory
                    .OrderByDescending(s => s.SessionStart)
                    .Take(100)
                    .OrderBy(s => s.SessionStart)
                    .ToList();
            }
            
            await SaveCurrentProfileAsync();
        }
        
        /// <summary>
        /// Update rally progress
        /// </summary>
        public async Task UpdateRallyProgressAsync(DifficultyLevel difficulty, bool stageCompleted, double accuracy, int completionTimeSeconds)
        {
            if (CurrentProfile == null)
                throw new InvalidOperationException("No profile is currently loaded");
            
            var rallyData = CurrentProfile.RallyData;
            
            if (stageCompleted)
            {
                // Update stages completed for this difficulty
                if (!rallyData.StagesCompleted.ContainsKey(difficulty))
                    rallyData.StagesCompleted[difficulty] = 0;
                rallyData.StagesCompleted[difficulty]++;
                rallyData.TotalStagesCompleted++;
                
                // Update highest completed difficulty
                if (difficulty > rallyData.HighestCompletedDifficulty)
                {
                    rallyData.HighestCompletedDifficulty = difficulty;
                }
            }
            
            // Update best accuracy for this difficulty
            if (!rallyData.BestAccuracy.ContainsKey(difficulty) || accuracy > rallyData.BestAccuracy[difficulty])
            {
                rallyData.BestAccuracy[difficulty] = accuracy;
            }
            
            // Update best completion time for this difficulty
            if (!rallyData.BestCompletionTime.ContainsKey(difficulty) || 
                (completionTimeSeconds > 0 && (rallyData.BestCompletionTime[difficulty] == 0 || completionTimeSeconds < rallyData.BestCompletionTime[difficulty])))
            {
                rallyData.BestCompletionTime[difficulty] = completionTimeSeconds;
            }
            
            await SaveCurrentProfileAsync();
        }
        
        /// <summary>
        /// Update user settings
        /// </summary>
        public async Task UpdateSettingsAsync(UserSettings settings)
        {
            if (CurrentProfile == null)
                throw new InvalidOperationException("No profile is currently loaded");
            
            CurrentProfile.Settings = settings;
            await SaveCurrentProfileAsync();
        }
        
        /// <summary>
        /// Get all available profiles (for future multi-profile support)
        /// </summary>
        public List<string> GetAvailableProfiles()
        {
            var profiles = new List<string>();
            
            if (!Directory.Exists(_savesDirectory))
                return profiles;
            
            var profileFiles = Directory.GetFiles(_savesDirectory, "*.json")
                .Where(f => !f.EndsWith("app_settings.json"));
            
            foreach (var file in profileFiles)
            {
                try
                {
                    var filename = Path.GetFileName(file);
                    profiles.Add(filename);
                }
                catch
                {
                    // Skip corrupted profile files
                }
            }
            
            return profiles;
        }

        /// <summary>
        /// Load a specific profile by filename
        /// </summary>
        public async Task<UserProfile> LoadProfileAsync(string filename)
        {
            var profilePath = Path.Combine(_savesDirectory, filename);
            
            if (!File.Exists(profilePath))
                throw new FileNotFoundException($"Profile file not found: {filename}");

            try
            {
                var json = await File.ReadAllTextAsync(profilePath);
                var profile = JsonSerializer.Deserialize<UserProfile>(json, _jsonOptions);
                
                if (profile != null)
                {
                    // Update last played date
                    profile.LastPlayedDate = DateTime.Now;
                    CurrentProfile = profile;
                    CurrentProfilePath = profilePath;
                    
                    // Update the achievement manager with the new profile
                    AchievementManager.UpdateProfileManager(this);
                    
                    ProfileChanged?.Invoke(this, profile);
                    return profile;
                }
                else
                {
                    throw new InvalidOperationException("Failed to deserialize profile data");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error loading profile: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Delete a profile by filename
        /// </summary>
        public void DeleteProfileAsync(string filename)
        {
            var profilePath = Path.Combine(_savesDirectory, filename);
            
            if (!File.Exists(profilePath))
                throw new FileNotFoundException($"Profile file not found: {filename}");

            try
            {
                File.Delete(profilePath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting profile: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Create a new user profile with custom filename
        /// </summary>
        public async Task<UserProfile> CreateNewProfileAsync(string playerName)
        {
            var profile = new UserProfile
            {
                PlayerName = playerName,
                CreatedDate = DateTime.Now,
                LastPlayedDate = DateTime.Now
            };
            
            CurrentProfile = profile;
            
            // Update the achievement manager with the new profile
            AchievementManager.UpdateProfileManager(this);
            
            // Generate filename from player name
            string filename = GenerateFilename(playerName);
            var profilePath = Path.Combine(_savesDirectory, filename);
            CurrentProfilePath = profilePath;
            
            // Save the new profile
            var json = JsonSerializer.Serialize(profile, _jsonOptions);
            await File.WriteAllTextAsync(profilePath, json);
            
            ProfileChanged?.Invoke(this, profile);
            ProfileSaved?.Invoke(this, profilePath);
            
            return profile;
        }

        /// <summary>
        /// Generate a safe filename from player name
        /// </summary>
        private string GenerateFilename(string playerName)
        {
            // Convert to lowercase and replace invalid characters
            var filename = playerName.ToLowerInvariant()
                .Replace(' ', '_')
                .Replace('-', '_');
            
            // Remove invalid characters
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }
            
            // Ensure it doesn't conflict with existing files
            string baseName = filename;
            int counter = 1;
            
            while (File.Exists(Path.Combine(_savesDirectory, $"{filename}.json")))
            {
                filename = $"{baseName}_{counter}";
                counter++;
            }
            
            return $"{filename}.json";
        }
        
        /// <summary>
        /// Export profile data as JSON for backup
        /// </summary>
        public string ExportProfile()
        {
            if (CurrentProfile == null)
                throw new InvalidOperationException("No profile is currently loaded");
            
            return JsonSerializer.Serialize(CurrentProfile, _jsonOptions);
        }
        
        /// <summary>
        /// Import profile data from JSON backup
        /// </summary>
        public async Task<UserProfile> ImportProfileAsync(string jsonData)
        {
            try
            {
                var profile = JsonSerializer.Deserialize<UserProfile>(jsonData, _jsonOptions);
                if (profile == null)
                    throw new InvalidOperationException("Invalid profile data");
                
                CurrentProfile = profile;
                
                // Update the achievement manager with the new profile
                AchievementManager.UpdateProfileManager(this);
                
                await SaveCurrentProfileAsync();
                ProfileChanged?.Invoke(this, profile);
                return profile;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to import profile: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// Reset current profile to defaults (keep name and creation date)
        /// </summary>
        public async Task ResetProfileAsync()
        {
            if (CurrentProfile == null)
                throw new InvalidOperationException("No profile is currently loaded");
            
            var playerName = CurrentProfile.PlayerName;
            var createdDate = CurrentProfile.CreatedDate;
            
            CurrentProfile = new UserProfile
            {
                PlayerName = playerName,
                CreatedDate = createdDate,
                LastPlayedDate = DateTime.Now
            };
            
            // Update the achievement manager with the reset profile
            AchievementManager.UpdateProfileManager(this);
            
            await SaveCurrentProfileAsync();
            ProfileChanged?.Invoke(this, CurrentProfile);
        }
        
        /// <summary>
        /// Get profile storage directory path
        /// </summary>
        public string GetProfileDirectory() => _savesDirectory;
        
        /// <summary>
        /// Ensure the profile directory exists
        /// </summary>
        private void EnsureDirectoryExists()
        {
            try
            {
                Directory.CreateDirectory(_savesDirectory);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create profile directory: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// Check if profile data needs migration to newer version
        /// </summary>
        private bool NeedsProfileMigration(UserProfile profile)
        {
            // Simple version comparison - in future, implement more sophisticated migration
            return profile.Version != "1.0";
        }
        
        /// <summary>
        /// Migrate profile to current version (placeholder for future use)
        /// </summary>
        private UserProfile MigrateProfile(UserProfile oldProfile)
        {
            // Future migration logic would go here
            oldProfile.Version = "1.0";
            return oldProfile;
        }
    }
}
