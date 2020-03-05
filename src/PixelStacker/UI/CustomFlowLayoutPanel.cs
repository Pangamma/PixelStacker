using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public class CustomFlowLayoutPanel : FlowLayoutPanel
    {
        public CustomFlowLayoutPanel(): base()
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
