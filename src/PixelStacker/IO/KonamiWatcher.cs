using System;
using System.Windows.Forms;

namespace PixelStacker.IO
{
    public static class KonamiWatcher
    {
        public static Action OnCodeEntry { get; set; } = () => { };


        private static int konamiIndex = 0;
        private static Keys[] konamiSequence = new Keys[] {
            Keys.Up, Keys.Up, Keys.Down, Keys.Down,
            Keys.Left, Keys.Right, Keys.Left, Keys.Right,
            Keys.B, Keys.A
        };

        private static DateTime LastEntry = DateTime.MinValue;
        public static void ProcessKey(Keys keyData)
        {
            // Duplicate input detection
            if (DateTime.UtcNow - LastEntry < TimeSpan.FromMilliseconds(20)) return;
            LastEntry = DateTime.UtcNow;

            if (konamiSequence[konamiIndex] == keyData)
            {
                konamiIndex++;
                if (keyData == Keys.A)
                {
                    konamiIndex = 0;
                    KonamiWatcher.OnCodeEntry();
                }
            }
            else
            {
                konamiIndex = 0;
            }
        }
    }
}
