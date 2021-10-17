using System.ComponentModel;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    [ToolboxItemFilter("PixelStacker.UI.CanvasEditor", ToolboxItemFilterType.Require)]
    public partial class CanvasEditor : UserControl
    {
        public CanvasEditor()
        {
            InitializeComponent();
            this.BackgroundImage = Resources.UIResources.bg_imagepanel;
            this.DoubleBuffered = true;
        }
    }
}
