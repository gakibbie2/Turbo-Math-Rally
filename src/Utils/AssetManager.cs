using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace TurboMathRally.Utils
{
    /// <summary>
    /// Manages visual assets including images and ASCII art for the Turbo Math Rally game
    /// </summary>
    public static class AssetManager
    {
        private static readonly string AssetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
        
        #region Image Asset Loading
        
        /// <summary>
        /// Loads an image asset from the Assets folder, returns null if not found
        /// </summary>
        public static Image? LoadImage(string relativePath)
        {
            try
            {
                string fullPath = Path.Combine(AssetsPath, relativePath);
                if (File.Exists(fullPath))
                {
                    return Image.FromFile(fullPath);
                }
            }
            catch (Exception)
            {
                // Silently fail and return null for fallback handling
            }
            return null;
        }

        /// <summary>
        /// Gets a race car image - tries to load from assets, falls back to ASCII
        /// </summary>
        public static Image? GetRaceCarImage(string color = "red", int carNumber = 1)
        {
            string imagePath = $"racing-pack/PNG/Cars/car_{color}_{carNumber}.png";
            return LoadImage(imagePath);
        }

        /// <summary>
        /// Gets a small race car image for progress indicators
        /// </summary>
        public static Image? GetSmallRaceCarImage(string color = "red", int carNumber = 1)
        {
            string imagePath = $"racing-pack/PNG/Cars/car_{color}_small_{carNumber}.png";
            return LoadImage(imagePath);
        }

        /// <summary>
        /// Gets a UI button image
        /// </summary>
        public static Image? GetUIButtonImage(string color = "Blue", string buttonType = "button_square_header")
        {
            string imagePath = $"kenney_ui-pack/PNG/{color}/{buttonType}.png";
            return LoadImage(imagePath);
        }

        /// <summary>
        /// Gets a generic item/icon image (tools, trophies, etc.)
        /// </summary>
        public static Image? GetGenericItemImage(int itemNumber)
        {
            string imagePath = $"kenney_genericItems_updatedCross/PNG/Colored/genericItem_color_{itemNumber:D3}.png";
            return LoadImage(imagePath);
        }

        #endregion

        #region ASCII Art Fallbacks
        
        /// <summary>
        /// ASCII art race car for fallback when images aren't available
        /// </summary>
        public static string GetRaceCarASCII()
        {
            return @"
    ______
   //  ||\ \
  //__||_\_\
 |_    ____|
   |__|  o|
";
        }

        /// <summary>
        /// ASCII art broken car for repair screens
        /// </summary>
        public static string GetBrokenCarASCII()
        {
            return @"
    ______
   //  ||\ \
  //__||_\_\
 |_   💥___|
   |__|  x|
";
        }

        /// <summary>
        /// ASCII art wrench tool
        /// </summary>
        public static string GetWrenchASCII()
        {
            return @"
     🔧
    /  \
   |    |
   |    |
    \  /
     \/
";
        }

        /// <summary>
        /// ASCII art trophy
        /// </summary>
        public static string GetTrophyASCII()
        {
            return @"
    🏆
   /   \
  |  1  |
  |_____|
   |   |
   |___|
";
        }

        /// <summary>
        /// Enhanced progress bar with racing theme
        /// </summary>
        public static string GetProgressBarASCII(int current, int total, int width = 20)
        {
            double percentage = (double)current / total;
            int filled = (int)(percentage * width);
            int empty = width - filled;

            StringBuilder bar = new StringBuilder();
            bar.Append("🏁"); // Start flag
            
            // Filled portion
            for (int i = 0; i < filled; i++)
            {
                bar.Append("━");
            }
            
            // Car position
            if (current < total && filled < width)
            {
                bar.Append("🚗");
                empty--;
            }
            
            // Empty portion
            for (int i = 0; i < empty; i++)
            {
                bar.Append("░");
            }
            
            bar.Append("🏆"); // Finish trophy
            
            return bar.ToString();
        }

        /// <summary>
        /// Car health indicator
        /// </summary>
        public static string GetCarHealthASCII(int health)
        {
            return health switch
            {
                3 => "🚗💚💚💚", // Perfect condition
                2 => "🚗💛💛⚫", // Good condition
                1 => "🚗❤️⚫⚫",  // Needs repair
                0 => "🚗💥⚫⚫",  // Broken down
                _ => "🚗❓❓❓"   // Unknown
            };
        }

        /// <summary>
        /// Answer feedback graphics
        /// </summary>
        public static string GetAnswerFeedbackASCII(bool correct)
        {
            return correct 
                ? @"
  ✅ CORRECT!
  🎉 Great job!
  🏁➤ Keep racing!"
                : @"
  ❌ INCORRECT
  🤔 Try again!
  🏎️➤ Keep going!";
        }

        #endregion

        #region Asset Discovery

        /// <summary>
        /// Gets the available car colors from the racing pack
        /// </summary>
        public static string[] GetAvailableCarColors()
        {
            return new[] { "red", "blue", "green", "yellow", "black" };
        }

        /// <summary>
        /// Gets the number of available car types per color (1-5)
        /// </summary>
        public static int GetMaxCarNumber()
        {
            return 5;
        }

        /// <summary>
        /// Checks if assets are properly loaded
        /// </summary>
        public static bool AreAssetsAvailable()
        {
            return Directory.Exists(AssetsPath);
        }

        #endregion
    }
}
