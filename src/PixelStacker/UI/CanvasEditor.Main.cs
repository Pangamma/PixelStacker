using PixelStacker.IO.Config;
using PixelStacker.Logic;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
        private PanZoomSettings PanZoomSettings { get; set; }



        public async Task SetCanvas(CancellationToken? worker, RenderedCanvas canvas, PanZoomSettings pz)
        {
            int? textureSizeOut = RenderCanvasEngine.CalculateTextureSize(canvas.Width, canvas.Height, 2);
            if (textureSizeOut == null)
            {
                ProgressX.Report(100, Resources.Text.Error_ImageTooLarge);
                return;
            }

            int textureSize = textureSizeOut.Value;
            // possible to use faster math?


            ProgressX.Report(0, "Rendering block plan to viewing window.");
            var painter = await RenderedCanvasPainter.Create(worker, canvas);
            this.Painter = painter;

            this.RepaintRequested = true;
            // DO not set these until ready
            this.Canvas = canvas;
            this.PanZoomSettings = pz;
        }
    }
}
