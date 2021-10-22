using System;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.Components
{
    public class CustomTableLayoutPanel : TableLayoutPanel
    {
        public CustomTableLayoutPanel(): base()
        {
            this.DoubleBuffered = true;
        }

        public Func<Message, Keys, bool> OnCommandKey { get; set; } = null;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.OnCommandKey != null)
            {
                return this.OnCommandKey(msg, keyData);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }
    }
}
