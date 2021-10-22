using PixelStacker.Logic.Engine;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class RenderedImagePanel : UserControl
    {
        private Point initialDragPoint;
        private object Padlock = new { };
        private bool IsDragging = false;
        private bool _WasDragged = false;
        public RenderedCanvas Canvas { get; private set; }
        private PanZoomSettings PanZoomSettings { get; set; }

        public RenderedImagePanel()
        {
            this.BackgroundImage = Resources.UIResources.bg_imagepanel;
            InitializeComponent();
        }

        public async Task SetRenderedImage(RenderedCanvas canvas, PanZoomSettings zoom = null)
        {
            this.BackgroundImage = Resources.UIResources.bg_imagepanel;
            int? textureSizeOut = RenderCanvasEngine.CalculateTextureSize(canvas.Width, canvas.Height, 2);
            if (textureSizeOut == null)
            {
                ProgressX.Report(100, Resources.Text.Error_ImageTooLarge);
                return;
            }
            
            int textureSize = textureSizeOut.Value;
            // possible to use faster math?

            // DO not set these until ready
            this.Canvas = canvas;
            this.PanZoomSettings = zoom;
        }
    }
}
