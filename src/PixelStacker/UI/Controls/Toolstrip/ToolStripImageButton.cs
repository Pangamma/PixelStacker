using PixelStacker.UI.Controls.MaterialPicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PixelStacker.UI.Controls.Toolstrip
{
    /// <summary>
    /// This class will show up in the toolbox when selecting to add a new item
    /// to toolstrips and status bars. Works great. Basically provides a 
    /// ToolStripImageButtonControl as the actual component.
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripImageButton : ToolStripControlHost
    {
        private ToolStripImageButtonControl Component;

        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        [Category("PixelStacker")]
        [Description("Set the image or icon to display")]
        public Bitmap ButtonImage
        {
            get => this.Component.Image;
            set => this.Component.Image = value;
        }

        [Category("PixelStacker")]
        public Color Color { get; set; }
        //public ToolStripImageButtonControl Picker => this.Control as ToolStripImageButtonControl;
        public ToolStripImageButton() : base(CreateControlInstance())
        {
        }

        public ToolStripImageButton(ToolStripImageButtonControl c) : base(c)
        {
            this.Component = this.Control as ToolStripImageButtonControl;
        }

        private static ToolStripImageButtonControl CreateControlInstance()
        {
            ToolStripImageButtonControl mtb = new ToolStripImageButtonControl();
            return mtb;
        }
    }
}
