using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Keys.B, Keys.A, Keys.Enter
        };

        public void ProcessKey(Keys keyData)
        {
            if (konamiSequence[konamiIndex] == keyData)
            {
                konamiIndex++;
                if (keyData == Keys.Enter)
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
