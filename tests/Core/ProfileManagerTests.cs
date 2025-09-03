namespace TurboMathRally.Tests.Core
{
    /// <summary>
    /// Tests for the ProfileManager class
    /// </summary>
    public class ProfileManagerTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly ProfileManager _profileManager;

        public ProfileManagerTests()
        {
            // Create a temporary directory for test files
            _testDirectory = Path.Combine(Path.GetTempPath(), "TurboMathRallyTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectory);
            
            _profileManager = new ProfileManager();
            
            // Use reflection to set the test directory
            var field = typeof(ProfileManager).GetField("_savesDirectory", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(_profileManager, _testDirectory);
        }

        public void Dispose()
        {
            // Clean up test directory
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }

        [Fact]
        public async Task LoadDefaultProfileAsync_NoExistingProfile_CreatesNewProfile()
        {
            // Act
            var profile = await _profileManager.LoadDefaultProfileAsync();

            // Assert
            profile.Should().NotBeNull();
            profile.Name.Should().Be("Player");
            profile.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            profile.LastPlayedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            profile.TotalPlayTime.Should().Be(TimeSpan.Zero);
            profile.AchievementData.Should().NotBeNull();
            _profileManager.CurrentProfile.Should().Be(profile);
        }

        [Fact]
        public async Task LoadDefaultProfileAsync_ExistingProfile_LoadsProfile()
        {
            // Arrange
            var originalProfile = new UserProfile
            {
                Name = "Existing Player",
                CreatedDate = DateTime.Now.AddDays(-5),
                TotalPlayTime = TimeSpan.FromHours(2),
                AchievementData = new AchievementData()
            };

            var profilePath = Path.Combine(_testDirectory, "default_profile.json");
            var json = JsonSerializer.Serialize(originalProfile, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(profilePath, json);

            // Act
            var loadedProfile = await _profileManager.LoadDefaultProfileAsync();

            // Assert
            loadedProfile.Should().NotBeNull();
            loadedProfile.Name.Should().Be("Existing Player");
            loadedProfile.CreatedDate.Should().Be(originalProfile.CreatedDate);
            loadedProfile.TotalPlayTime.Should().Be(TimeSpan.FromHours(2));
            loadedProfile.LastPlayedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task CreateNewProfileAsync_CreatesProfileWithCorrectName()
        {
            // Act
            var profile = await _profileManager.CreateNewProfileAsync("TestPlayer");

            // Assert
            profile.Should().NotBeNull();
            profile.Name.Should().Be("TestPlayer");
            profile.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            profile.LastPlayedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            profile.TotalPlayTime.Should().Be(TimeSpan.Zero);
            profile.AchievementData.Should().NotBeNull();
            _profileManager.CurrentProfile.Should().Be(profile);
        }

        [Fact]
        public async Task CreateNewProfileAsync_SavesProfileToFile()
        {
            // Act
            await _profileManager.CreateNewProfileAsync("TestPlayer");

            // Assert
            var profilePath = Path.Combine(_testDirectory, "testplayer.json");
            File.Exists(profilePath).Should().BeTrue();
            
            var json = await File.ReadAllTextAsync(profilePath);
            var savedProfile = JsonSerializer.Deserialize<UserProfile>(json, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
            
            savedProfile.Should().NotBeNull();
            savedProfile!.Name.Should().Be("TestPlayer");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task CreateNewProfileAsync_EmptyName_ThrowsException(string? name)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _profileManager.CreateNewProfileAsync(name!));
        }

        [Fact]
        public async Task LoadProfileAsync_ValidProfile_LoadsCorrectly()
        {
            // Arrange
            var testProfile = new UserProfile
            {
                Name = "LoadTest Player",
                CreatedDate = DateTime.Now.AddDays(-3),
                TotalPlayTime = TimeSpan.FromHours(1.5),
                AchievementData = new AchievementData()
            };
            testProfile.AchievementData.UnlockedAchievements["test_achievement"] = true;

            var profilePath = Path.Combine(_testDirectory, "loadtest.json");
            var json = JsonSerializer.Serialize(testProfile, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await File.WriteAllTextAsync(profilePath, json);

            // Act
            var loadedProfile = await _profileManager.LoadProfileAsync("loadtest.json");

            // Assert
            loadedProfile.Should().NotBeNull();
            loadedProfile.Name.Should().Be("LoadTest Player");
            loadedProfile.TotalPlayTime.Should().Be(TimeSpan.FromHours(1.5));
            loadedProfile.AchievementData.UnlockedAchievements.Should().ContainKey("test_achievement");
            _profileManager.CurrentProfile.Should().Be(loadedProfile);
        }

        [Fact]
        public async Task LoadProfileAsync_NonexistentFile_ThrowsFileNotFoundException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => 
                _profileManager.LoadProfileAsync("nonexistent.json"));
        }

        [Fact]
        public async Task SaveCurrentProfileAsync_SavesProfileData()
        {
            // Arrange
            await _profileManager.CreateNewProfileAsync("SaveTest");
            var profile = _profileManager.CurrentProfile!;
            profile.TotalPlayTime = TimeSpan.FromHours(3);
            profile.AchievementData.UnlockedAchievements["new_achievement"] = true;

            // Act
            await _profileManager.SaveCurrentProfileAsync();

            // Assert
            var profilePath = Path.Combine(_testDirectory, "savetest.json");
            File.Exists(profilePath).Should().BeTrue();
            
            var json = await File.ReadAllTextAsync(profilePath);
            var savedProfile = JsonSerializer.Deserialize<UserProfile>(json, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
            
            savedProfile.Should().NotBeNull();
            savedProfile!.TotalPlayTime.Should().Be(TimeSpan.FromHours(3));
            savedProfile.AchievementData.UnlockedAchievements.Should().ContainKey("new_achievement");
        }

        [Fact]
        public async Task SaveCurrentProfileAsync_NoCurrentProfile_ThrowsInvalidOperationException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _profileManager.SaveCurrentProfileAsync());
        }

        [Fact]
        public void GetAllProfileFiles_ReturnsAllJsonFiles()
        {
            // Arrange
            var profile1Path = Path.Combine(_testDirectory, "player1.json");
            var profile2Path = Path.Combine(_testDirectory, "player2.json");
            var nonJsonPath = Path.Combine(_testDirectory, "notaprofile.txt");
            
            File.WriteAllText(profile1Path, "{}");
            File.WriteAllText(profile2Path, "{}");
            File.WriteAllText(nonJsonPath, "test");

            // Act
            var profileFiles = _profileManager.GetAllProfileFiles();

            // Assert
            profileFiles.Should().HaveCount(2);
            profileFiles.Should().Contain("player1.json");
            profileFiles.Should().Contain("player2.json");
            profileFiles.Should().NotContain("notaprofile.txt");
        }

        [Fact]
        public void DeleteProfileAsync_ValidFile_DeletesProfile()
        {
            // Arrange
            var profilePath = Path.Combine(_testDirectory, "todelete.json");
            File.WriteAllText(profilePath, "{}");

            // Act
            _profileManager.DeleteProfileAsync("todelete.json");

            // Assert
            File.Exists(profilePath).Should().BeFalse();
        }

        [Fact]
        public void DeleteProfileAsync_NonexistentFile_ThrowsFileNotFoundException()
        {
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => 
                _profileManager.DeleteProfileAsync("nonexistent.json"));
        }

        [Fact]
        public async Task ProfileChanged_Event_FiresWhenProfileLoaded()
        {
            // Arrange
            UserProfile? changedProfile = null;
            _profileManager.ProfileChanged += (sender, profile) => 
            {
                changedProfile = profile;
            };

            // Act
            await _profileManager.CreateNewProfileAsync("EventTest");

            // Assert
            changedProfile.Should().NotBeNull();
            changedProfile!.Name.Should().Be("EventTest");
        }

        [Fact]
        public async Task ProfileSaved_Event_FiresWhenProfileSaved()
        {
            // Arrange
            string? savedFileName = null;
            _profileManager.ProfileSaved += (sender, fileName) => 
            {
                savedFileName = fileName;
            };

            await _profileManager.CreateNewProfileAsync("SaveEventTest");

            // Act
            await _profileManager.SaveCurrentProfileAsync();

            // Assert
            savedFileName.Should().NotBeNull();
            savedFileName.Should().EndWith(".json");
        }

        [Fact]
        public void UpdatePlayTime_UpdatesTotalPlayTime()
        {
            // Arrange
            var profile = new UserProfile
            {
                Name = "TimeTest",
                TotalPlayTime = TimeSpan.FromHours(1),
                AchievementData = new AchievementData()
            };
            
            // Use reflection to set current profile
            var property = typeof(ProfileManager).GetProperty("CurrentProfile");
            property?.SetValue(_profileManager, profile);

            var additionalTime = TimeSpan.FromMinutes(30);

            // Act
            _profileManager.UpdatePlayTime(additionalTime);

            // Assert
            profile.TotalPlayTime.Should().Be(TimeSpan.FromHours(1.5));
        }

        [Fact]
        public void GetDisplayName_ReturnsCorrectDisplayName()
        {
            // Act & Assert
            _profileManager.GetDisplayName("default_profile.json").Should().Be("Default Player");
            _profileManager.GetDisplayName("john_doe.json").Should().Be("John Doe");
            _profileManager.GetDisplayName("testplayer.json").Should().Be("Testplayer");
        }
    }
}
