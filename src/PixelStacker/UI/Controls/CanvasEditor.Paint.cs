using PixelStacker.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class CanvasEditor
    {
        private bool IsPainting = false;

        [System.Diagnostics.DebuggerStepThrough]
        private void timerPaint_Tick(object sender, EventArgs e)
        {
            if (this.RepaintRequested)
            {
                if (!IsPainting)
                {
                    IsPainting = true;
                    // Force repaint
                    //Refresh();
                    skiaControl.Refresh();
                    this.RepaintRequested = false;
                }
            }
        }

        private void skiaControl_PaintSurface(object sender, Controls.GenericSKPaintSurfaceEventArgs e)
        {
            IsPainting = true;
            SKSurface surface = e.Surface;
            var g = surface.Canvas;
            var bgImg = UIResources.bg_imagepanel.BitmapToSKBitmap();
            SKShader bgShader = SKShader.CreateBitmap(bgImg, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);
            using (SKPaint paint = new SKPaint())
            {
                paint.Shader = bgShader;
                paint.FilterQuality = SKFilterQuality.High;
                paint.IsDither = true;
                g.DrawRect(e.Rect, paint);
                g.DrawBitmap(bgImg, 0, 0);
            }

            // Render the image they are looking at.
            var painter = this.Painter;
            if (painter == null) return;

            var pz = this.PanZoomSettings;
            if (pz == null) return;

            var canvas = this.Canvas;
            if (canvas == null) return;

            painter.RenderToView(g, new SKSize(this.Width, this.Height), pz);

            var options = this.Options;
            if (options == null || options.ViewerSettings == null) return;
            CanvasViewerSettings vs = options.ViewerSettings;

            if (vs.IsShowBorder) DrawBorder(g, pz);

            DrawWorldEditOrigin(g, pz, canvas.WorldEditOrigin);
            if (vs.IsShowGrid) DrawGridLines(g, canvas, vs, pz);

            IsPainting = false;
        }

        [Obsolete("FIX IT")]
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                Graphics g = e.Graphics;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                using Brush bgBrush = new TextureBrush(Resources.UIResources.bg_imagepanel);
                g.FillRectangle(bgBrush, 0, 0, this.Width, this.Height);
                base.OnPaint(e);
                return;
            }

            //base.OnPaint(e);

            //var painter = this.Painter;
            //if (painter == null) return;

            //var pz = this.PanZoomSettings;
            //if (pz == null) return;

            //var canvas = this.Canvas;
            //if (canvas == null) return;

            //if (pz.zoomLevel < 1.0D)
            //{
            //    g.InterpolationMode = InterpolationMode.Low;
            //    g.CompositingQuality = CompositingQuality.HighSpeed;
            //    g.SmoothingMode = SmoothingMode.HighSpeed;
            //}
            //else
            //{
            //    g.InterpolationMode = InterpolationMode.NearestNeighbor;
            //    g.CompositingQuality = CompositingQuality.HighSpeed;
            //    g.SmoothingMode = SmoothingMode.AntiAlias;
            //}

            //painter.RenderToView(g, this.Size, pz);
        }

        private void DrawBorder(SKCanvas g, PanZoomSettings pz)
        {
            Point pp2 = GetPointOnPanel(new Point(0, 0));
            float strokeWidth = (float)Math.Max(1, pz.zoomLevel);
            float strokeOffset = strokeWidth / 2;
            using SKPaint penWE = new SKPaint()
            {
                Color = new SKColor(0, 0, 0, 127),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth, // Apparently this will be equal to the size of a block.
            };
            //penWE.Alignment = PenAlignment.Inset;
            g.DrawRect(
                pp2.X+strokeOffset, pp2.Y + strokeOffset,
                (int)(this.Canvas.Width * pz.zoomLevel) - strokeWidth, (int)(this.Canvas.Height * pz.zoomLevel) - strokeWidth, penWE);
        }

        private void DrawWorldEditOrigin(SKCanvas g, PanZoomSettings pz, SkiaSharp.SKPoint weOrigin)
        {
            //var zoom = pz.zoomLevel;
            //using (Brush brush = new SolidBrush(Color.Red))
            //{
            //    Point wePoint = GetPointOnPanel(new Point((int)weOrigin.X, (int)weOrigin.Y));
            //    g.FillRect(brush, wePoint.X, wePoint.Y, (int)zoom + 1, (int)zoom + 1);
            //}
        }

        protected void DrawGridLines(SKCanvas g, RenderedCanvas canvas, CanvasViewerSettings vs, PanZoomSettings pz)
        {
            //if (pz.zoomLevel >= 0.5D)
            //{
            //    DrawGridMask(g, pz, vs.GridSize, vs.GridColor);
            //    drawGrid(g, canvas, pz, vs.GridSize, vs.GridColor);
            //    if (pz.zoomLevel > 5) drawGrid(g, canvas, pz, 1, Color.FromArgb(40, vs.GridColor));
            //}
        }

        private void DrawGridMask(SKCanvas g, PanZoomSettings pz, int gridSize, Color c)
        {
            //if (this.gridMaskClip == null) return;
            //Point tL = new Point(this.gridMaskClip.Value.Left, this.gridMaskClip.Value.Top);
            //Point bR = new Point(this.gridMaskClip.Value.Right, this.gridMaskClip.Value.Bottom);
            //tL = this.GetPointOnPanel(tL);
            //bR = this.GetPointOnPanel(bR);
            //g.ExcludeClip(new Rectangle(tL, tL.CalculateSize(bR)));
            //using (Pen p = new Pen(Color.FromArgb(128, c)))
            //{
            //    g.FillRectangle(p.Brush, 0, 0, this.Width, this.Height);
            //}
            //g.ResetClip();
        }

        private void drawGrid(Graphics g, RenderedCanvas canvas, PanZoomSettings pz, int gridSize, Color c)
        {
            static int getRoundedZoomDistance(int x, int deltaX, double zoom) => (int)Math.Round(x + deltaX * zoom);
            static int getRoundedZoomX(int val, int blockSize, double zoom, int imageX) => (int)Math.Floor(imageX + val * blockSize * zoom);
            static int getRoundedZoomY(int val, int blockSize, double zoom, int imageY) => (int)Math.Floor(imageY + val * blockSize * zoom);
            double zoom = pz.zoomLevel;
            int numHorizBlocks = (canvas.Width / gridSize);
            int numVertBlocks = (canvas.Height / gridSize);
            using Pen p = new Pen(c, gridSize == 1 ? 2 : GetGridWidth(zoom));

            g.DrawLine(p, pz.imageX, pz.imageY, pz.imageX, getRoundedZoomDistance(pz.imageY, canvas.Height, zoom));
            g.DrawLine(p, pz.imageX, getRoundedZoomDistance(pz.imageY, canvas.Height, zoom), getRoundedZoomDistance(pz.imageX, canvas.Width, zoom), getRoundedZoomDistance(pz.imageY, canvas.Height, zoom));
            g.DrawLine(p, getRoundedZoomDistance(pz.imageX, canvas.Width, zoom), getRoundedZoomDistance(pz.imageY, canvas.Height, zoom), getRoundedZoomDistance(pz.imageX, canvas.Width, zoom), pz.imageY);
            g.DrawLine(p, getRoundedZoomDistance(pz.imageX, canvas.Width, zoom), pz.imageY, pz.imageX, pz.imageY);
            for (int x = 0; x <= numHorizBlocks; x++)
            {
                g.DrawLine(p, getRoundedZoomX(x, gridSize, zoom, pz.imageX), pz.imageY, getRoundedZoomX(x, gridSize, zoom, pz.imageX), getRoundedZoomDistance(pz.imageY, canvas.Height, zoom));
            }
            for (int y = 0; y <= numVertBlocks; y++)
            {
                g.DrawLine(p, pz.imageX, getRoundedZoomY(y, gridSize, zoom, pz.imageY), getRoundedZoomDistance(pz.imageX, canvas.Width, zoom), getRoundedZoomY(y, gridSize, zoom, pz.imageY));
            }
        }


        private static int GetGridWidth(double zoom)
        {
            if (zoom > 70) return 8;
            if (zoom > 60) return 7;
            if (zoom > 50) return 6;
            if (zoom > 35) return 5;
            if (zoom > 25) return 4;
            if (zoom > 15) return 3;
            if (zoom > 8) return 2;
            if (zoom >= 0) return 1;
            return 1;
        }
    }
}
