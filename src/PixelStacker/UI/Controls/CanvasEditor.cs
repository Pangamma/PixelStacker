using PixelStacker.EditorTools;
using PixelStacker.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.UI.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    [ToolboxItemFilter("PixelStacker.UI.CanvasEditor", ToolboxItemFilterType.Require)]
    public partial class CanvasEditor : UserControl
    {
        private Options _opts = null;
        public Options Options
        {
            get => _opts; set
            {
                _opts = value;
                this.tbxBrushWidth.Text = _opts?.Tools?.BrushWidth.ToString();
            }
        }

        public CanvasEditor(Options opts)
        {
            this.Options = opts;
            InitializeComponent();
            this.BackgroundImage = Resources.UIResources.bg_imagepanel;
            this.DoubleBuffered = true;
            this.PanZoomTool = new PanZoomTool(this);
            this.CurrentTool = new PanZoomTool(this);
            this.btnMaterialCombination.Image = Resources.Textures.stone.SKBitmapToBitmap();
        }

        public CanvasEditor()
        {
            InitializeComponent();
            this.BackgroundImage = Resources.UIResources.bg_imagepanel;
            this.DoubleBuffered = true;
            this.PanZoomTool = new PanZoomTool(this);
            this.CurrentTool = new PanZoomTool(this);
            this.btnMaterialCombination.Image = Resources.Textures.stone.SKBitmapToBitmap();
        }
    }
}
