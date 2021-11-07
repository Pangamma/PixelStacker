using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Image;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.UI
{
    public partial class CanvasEditor
    {
        private object Padlock = new { };
        private Point initialDragPoint;
        private bool IsDragging = false;
        private RenderedCanvasPainter Painter;

        public RenderedCanvas Canvas { get; private set; }
        public PanZoomSettings PanZoomSettings { get; set; }



        public async Task SetCanvas(CancellationToken? worker, RenderedCanvas canvas, PanZoomSettings pz)
        {
            this.BackgroundImage = UIResources.bg_imagepanel;
            //int? textureSizeOut = RenderCanvasEngine.CalculateTextureSize(canvas.Width, canvas.Height, 2);
            //if (textureSizeOut == null)
            //{
            //    ProgressX.Report(100, Resources.Text.Error_ImageTooLarge);
            //    return;
            //}
            //int textureSize = textureSizeOut.Value;

            pz ??= CalculateInitialPanZoomSettings(canvas.Width, canvas.Height);
            // possible to use faster math?


            ProgressX.Report(0, "Rendering block plan to viewing window.");
            var painter = await RenderedCanvasPainter.Create(worker, canvas);
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
