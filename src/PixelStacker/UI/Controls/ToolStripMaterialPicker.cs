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
        public MaterialPicker Picker => this.Control as MaterialPicker;
        public ToolStripMaterialPicker() : base(CreateControlInstance()) {
        }

        public ToolStripMaterialPicker(MaterialPicker c) : base(c)
        {
        }

        private static MaterialPicker CreateControlInstance()
        {
            MaterialPicker mtb = new MaterialPicker();
            mtb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            mtb.MinimumSize = new System.Drawing.Size(100, 200);
            return mtb;
        }
    }
}
