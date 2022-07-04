using PixelStacker.UI.Controls.MaterialPicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PixelStacker.UI.Controls
{
    //Declare a class that inherits from ToolStripControlHost.
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripMaterialPicker : ToolStripControlHost
    {
        public PickerPanel Picker => this.Control as PickerPanel;
        public ToolStripMaterialPicker() : base(CreateControlInstance()) {
        }

        public ToolStripMaterialPicker(PickerPanel c) : base(c)
        {
        }

        private static PickerPanel CreateControlInstance()
        {
            PickerPanel mtb = new PickerPanel();
            mtb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            mtb.MinimumSize = new System.Drawing.Size(40, 40);
            return mtb;
        }
    }
}
