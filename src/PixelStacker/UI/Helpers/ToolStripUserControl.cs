using PixelStacker.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PixelStacker.UI.Helpers
{
    //Declare a class that inherits from ToolStripControlHost.
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripUserControl : ToolStripControlHost
    {
        public ToolStripUserControl() : base(CreateControlInstance()) { }
        public ToolStripUserControl(Control c) : base(c)
        {
        }

        private static Control CreateControlInstance()
        {
            MaskedTextBox mtb = new MaskedTextBox();
            mtb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            mtb.MinimumSize = new System.Drawing.Size(100, 16);
            mtb.PasswordChar = '*';
            return mtb;
        }
    }
}