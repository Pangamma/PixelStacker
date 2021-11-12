using PixelStacker.EditorTools;
using PixelStacker.Logic.IO.Config;
using PixelStacker.UI.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    [ToolboxItemFilter("PixelStacker.UI.CanvasEditor", ToolboxItemFilterType.Require)]
    public partial class CanvasEditor : UserControl
    {
        public Options Options { get; set; }
        public CanvasEditor()
        {
            InitializeComponent();
            this.BackgroundImage = Resources.UIResources.bg_imagepanel;
            this.DoubleBuffered = true;
            this.PanZoomTool = new PanZoomTool(this);
            this.CurrentTool = new PanZoomTool(this);
        }
    }
}
