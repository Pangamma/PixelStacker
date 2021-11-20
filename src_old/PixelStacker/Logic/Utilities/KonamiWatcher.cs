using System;
using System.Windows.Forms;

namespace PixelStacker.Logic.Utilities
{
    public class KonamiWatcher
    {
        public Action OnCodeEntry { get; private set; }

        public KonamiWatcher(Action onCodeEntry)
        {
            this.OnCodeEntry = onCodeEntry;
        }

        private int konamiIndex = 0;
        private Keys[] konamiSequence = new Keys[] {
            Keys.Up, Keys.Up, Keys.Down, Keys.Down,
            Keys.Left, Keys.Right, Keys.Left, Keys.Right,
            Keys.B, Keys.A
        };

        private DateTime LastEntry = DateTime.MinValue;
        public void ProcessKey(Keys keyData)
        {
            if (DateTime.UtcNow - LastEntry < TimeSpan.FromMilliseconds(20)) return;
            LastEntry = DateTime.UtcNow;

            if (konamiSequence[konamiIndex] == keyData)
            {
                konamiIndex++;
                if (keyData == Keys.A)
                {
                    konamiIndex = 0;
                    this.OnCodeEntry();
                }
            }
            else
            {
                konamiIndex = 0;
            }
        }
    }
}
