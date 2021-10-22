using System;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public class CustomPanel : Panel
    {
        public CustomPanel() : base()
        {
            DoubleBuffered = true;
        }

        public Func<Message, Keys, bool> OnCommandKey { get; set; } = null;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (OnCommandKey != null)
            {
                return OnCommandKey(msg, keyData);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            return AutoScrollPosition;
        }
    }
}
