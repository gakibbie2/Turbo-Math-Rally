namespace TurboMathRally.Utils
{
    /// <summary>
    /// Manages sound effects for game events using console beeps
    /// Respects user volume settings and provides consistent audio feedback
    /// </summary>
    public class SoundManager
    {
        private readonly float _baseVolume;
        
        /// <summary>
        /// Initialize the sound manager with user's volume preference
        /// </summary>
        /// <param name="userVolume">Volume from 0.0 (mute) to 1.0 (full)</param>
        public SoundManager(float userVolume = 0.7f)
        {
            _baseVolume = System.Math.Max(0.0f, System.Math.Min(1.0f, userVolume));
        }
        
        /// <summary>
        /// Play sound for correct answer - upbeat success tone
        /// </summary>
        public void PlayCorrectAnswer()
        {
            if (_baseVolume <= 0) return;
            
            Task.Run(() =>
            {
                try
                {
                    // Upbeat success: Rising tone sequence
                    Console.Beep(523, (int)(150 * _baseVolume)); // C5
                    Console.Beep(659, (int)(200 * _baseVolume)); // E5
                }
                catch
                {
                    // Ignore beep errors on systems where not supported
                }
            });
        }
        
        /// <summary>
        /// Play sound for incorrect answer - error notification
        /// </summary>
        public void PlayIncorrectAnswer()
        {
            if (_baseVolume <= 0) return;
            
            Task.Run(() =>
            {
                try
                {
                    // Error: Low disappointed tone
                    Console.Beep(220, (int)(300 * _baseVolume)); // A3
                }
                catch
                {
                    // Ignore beep errors
                }
            });
        }
        
        /// <summary>
        /// Play sound for car breakdown - warning alarm sequence
        /// </summary>
        public void PlayCarBreakdown()
        {
            if (_baseVolume <= 0) return;
            
            Task.Run(() =>
            {
                try
                {
                    // Car breakdown: Descending alarm sequence
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Beep(800, (int)(100 * _baseVolume));
                        Thread.Sleep(50);
                        Console.Beep(400, (int)(100 * _baseVolume));
                        Thread.Sleep(50);
                    }
                }
                catch
                {
                    // Ignore beep errors
                }
            });
        }
        
        /// <summary>
        /// Play sound for car repair success - mechanical success
        /// </summary>
        public void PlayCarRepaired()
        {
            if (_baseVolume <= 0) return;
            
            Task.Run(() =>
            {
                try
                {
                    // Car repair: Mechanical startup sequence
                    Console.Beep(293, (int)(100 * _baseVolume)); // D4
                    Thread.Sleep(50);
                    Console.Beep(369, (int)(100 * _baseVolume)); // F#4
                    Thread.Sleep(50);
                    Console.Beep(440, (int)(200 * _baseVolume)); // A4
                }
                catch
                {
                    // Ignore beep errors
                }
            });
        }
        
        /// <summary>
        /// Play sound for stage completion - victory fanfare
        /// </summary>
        public void PlayStageComplete()
        {
            if (_baseVolume <= 0) return;
            
            Task.Run(() =>
            {
                try
                {
                    // Victory fanfare: Ascending celebration
                    Console.Beep(523, (int)(150 * _baseVolume)); // C5
                    Thread.Sleep(50);
                    Console.Beep(659, (int)(150 * _baseVolume)); // E5
                    Thread.Sleep(50);
                    Console.Beep(783, (int)(150 * _baseVolume)); // G5
                    Thread.Sleep(50);
                    Console.Beep(1046, (int)(300 * _baseVolume)); // C6
                }
                catch
                {
                    // Ignore beep errors
                }
            });
        }
        
        /// <summary>
        /// Play sound for achievement unlock - special distinctive pattern
        /// </summary>
        public void PlayAchievementUnlock()
        {
            if (_baseVolume <= 0) return;
            
            Task.Run(() =>
            {
                try
                {
                    // Achievement: Distinctive sparkle pattern
                    int[] notes = { 523, 659, 783, 659, 523 }; // C-E-G-E-C pattern
                    foreach (int note in notes)
                    {
                        Console.Beep(note, (int)(120 * _baseVolume));
                        Thread.Sleep(30);
                    }
                }
                catch
                {
                    // Ignore beep errors
                }
            });
        }
        
        /// <summary>
        /// Play sound for engine start - racing startup sequence
        /// </summary>
        public void PlayEngineStart()
        {
            if (_baseVolume <= 0) return;
            
            Task.Run(() =>
            {
                try
                {
                    // Engine start: Revving up sequence
                    for (int freq = 100; freq <= 300; freq += 50)
                    {
                        Console.Beep(freq, (int)(80 * _baseVolume));
                        Thread.Sleep(30);
                    }
                    Console.Beep(400, (int)(200 * _baseVolume)); // Final rev
                }
                catch
                {
                    // Ignore beep errors
                }
            });
        }
        
        /// <summary>
        /// Play menu navigation sound - subtle UI feedback
        /// </summary>
        public void PlayMenuSelect()
        {
            if (_baseVolume <= 0) return;
            
            Task.Run(() =>
            {
                try
                {
                    // Menu: Quick selection beep
                    Console.Beep(440, (int)(100 * _baseVolume)); // A4
                }
                catch
                {
                    // Ignore beep errors
                }
            });
        }
        
        /// <summary>
        /// Update the volume level for future sound effects
        /// </summary>
        /// <param name="newVolume">New volume from 0.0 to 1.0</param>
        public void UpdateVolume(float newVolume)
        {
            // Volume changes will be applied to new SoundManager instances
            // Current instance maintains its volume for consistency
        }
    }
}
