using PixelStacker.EditorTools;
using PixelStacker.Extensions;
using PixelStacker.Logic.CanvasEditor;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        private object Padlock = new { };
        public RenderedCanvasPainter Painter;

        public RenderedCanvas Canvas { get; private set; }
        public PanZoomSettings PanZoomSettings { get; set; }


        public MainForm MainForm { get; set; }

        public Options Options { get; set; }

        public CanvasEditor()
        {
            InitializeComponent();
            tsCanvasTools.Renderer = new CustomToolStripButtonRenderer();
            this.BackgroundImage = Resources.UIResources.bg_imagepanel;
            this.DoubleBuffered = true;
            this.PanZoomTool = new PanZoomTool(this);
            this.CurrentTool = new PanZoomTool(this);
            this.ApplyLocalization(CultureInfo.CurrentUICulture);

            this.Disposed += CanvasEditor_Disposed;
            AppEvents.OnPrimaryColorChange += this.AppEvents_OnPrimaryColorChange;
        }

        private void CanvasEditor_Disposed(object sender, System.EventArgs e)
        {
            AppEvents.OnPrimaryColorChange -= this.AppEvents_OnPrimaryColorChange;
        }

        private void AppEvents_OnPrimaryColorChange(object sender, OptionsChangeEvent<MaterialCombination> e)
        {
            MaterialCombination mcAfter = Options.Tools.PrimaryColor;
            var img = mcAfter.GetImage(Options.IsSideView);
            btnMaterialCombination.Image = img.SKBitmapToBitmap();
            btnMaterialCombination.ToolTipText = mcAfter.Top.Label + ", " + mcAfter.Bottom.Label;
        }

        public async Task SetCanvas(CancellationToken? worker, RenderedCanvas canvas, PanZoomSettings pz, SpecialCanvasRenderSettings vs)
        {
            this.BackgroundImage = UIResources.bg_imagepanel;

            pz ??= PanZoomSettings.CalculateDefaultPanZoomSettings(canvas.Width, canvas.Height, this.Width, this.Height);
            // possible to use faster math?

            ProgressX.Report(0, "Rendering block plan to viewing window.");
            var painter = await RenderedCanvasPainter.Create(worker, canvas, vs);
            this.Painter = painter;

            this.RepaintRequested = true;
            // DO not set these until ready
            this.Canvas = canvas;
            this.PanZoomSettings = pz;
        }

        private PanZoomSettings CalculateInitialPanZoomSettings(int bmWidth, int bmHeight)
        {
            var settings = new PanZoomSettings()
            {
                initialImageX = 0,
                initialImageY = 0,
                imageX = 0,
                imageY = 0,
                zoomLevel = 0,
                maxZoomLevel = Constants.MAX_ZOOM,
                minZoomLevel = Constants.MIN_ZOOM
            };

                double wRatio = (double)Width / bmWidth;
                double hRatio = (double)Height / bmHeight;
                if (hRatio < wRatio)
                {
                    settings.zoomLevel = hRatio;
                    settings.imageX = (Width - (int)(bmWidth * hRatio)) / 2;
                }
                else
                {
                    settings.zoomLevel = wRatio;
                    settings.imageY = (Height - (int)(bmHeight * wRatio)) / 2;
                }

                int numICareAbout = Math.Max(bmWidth, bmHeight);
                settings.minZoomLevel = (100.0D / numICareAbout);
                if (settings.minZoomLevel > 1.0D)
                {
                    settings.minZoomLevel = 1.0D;
                }

            return settings;
        }
    }
}
