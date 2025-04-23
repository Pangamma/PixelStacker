using PixelStacker.Extensions;
using PixelStacker.IO;
using PixelStacker.Resources.Themes;
using PixelStacker.UI.External;
using PixelStacker.UI.Helpers;
using SkiaSharp;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public partial class SkHybridControl : UserControl
    {
#if USE_GPU
        public static bool IsGpuAvailable => true;
#else
        public static bool IsGpuAvailable => false;
#endif
        private SKGLControl glCanvas;
        private SKControl Canvas;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler<GenericSKPaintSurfaceEventArgs> PaintSurface;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public bool BoxShadowOnEdges { get; set; } = false;

        public SkHybridControl()
        {
            InitializeComponent();
            Control control = null;
            if (IsGpuAvailable && !this.DesignMode)
            {
                this.glCanvas = new SKGLControl();
                this.glCanvas.PaintSurface += this.OnPaintSurfaceInner;
                control = this.glCanvas;
            }
            else
            {
                this.Canvas = new SKControl();
                this.Canvas.PaintSurface += this.OnPaintSurfaceInner;
                control = this.Canvas;
            }
            {
                // common
                control.Dock = DockStyle.Fill;
                control.MouseDown += this.SkHybridControl_MouseDown;
                control.MouseMove += this.SkHybridControl_MouseMove;
                control.MouseUp += this.SkHybridControl_MouseUp;
                control.MouseDoubleClick += this.SkHybridControl_MouseDoubleClick;
                control.MouseClick += this.SkHybridControl_MouseClick;
                control.MouseWheel += this.SkHybridControl_MouseWheel;

                this.Controls.Add(control);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                this.PaintDesignerView(e);
                return;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KonamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OnPaintSurfaceGeneric(object sender, GenericSKPaintSurfaceEventArgs e)
        {
            if (this.PaintSurface != null)
            {
                this.PaintSurface(sender, e);
#if DEBUG_GPU
                double fps = Math.Floor(Logic.Utilities.RateLimit.GetHitsPerSecond(90, 3000));
                e.Surface.Canvas.DrawText($"{fps} FPS", new SKPoint(8, 20), new SKPaint() { Color = new SKColor(255, 0, 0), TextSize = 16 });
#endif

                if (this.BoxShadowOnEdges)
                {
                    if (BoxShadow == null)
                    {
                        BoxShadow = SkHybridControl.CalculateBoxShadow(e.Rect);
                    }

                    SKRect srcRect = new SKRect(0, 0, BoxShadow.Width, BoxShadow.Height);
                    SKRect dstRect = new SKRect(0, 0, Width, Height);
                    e.Surface.Canvas.DrawImage(BoxShadow, srcRect, dstRect, new SKPaint()
                    {
                        BlendMode = SKBlendMode.SrcOver
                    });
                }
            }
        }

        private void OnPaintSurfaceInner(object sender, SKPaintGLSurfaceEventArgs e)
        {
            this.OnPaintSurfaceGeneric(this, new GenericSKPaintSurfaceEventArgs()
            {
                Surface = e.Surface,
                Rect = e.BackendRenderTarget.Rect
            });
        }

        private void OnPaintSurfaceInner(object sender, SKPaintSurfaceEventArgs e)
        {
            this.OnPaintSurfaceGeneric(this, new GenericSKPaintSurfaceEventArgs()
            {
                Surface = e.Surface,
                Rect = e.Info.Rect,
            });
        }

        private void SkHybridControl_MouseWheel(object sender, MouseEventArgs e) => this.OnMouseWheel(e);
        private void SkHybridControl_MouseDoubleClick(object sender, MouseEventArgs e) => this.OnMouseDoubleClick(e);
        private void SkHybridControl_MouseDown(object sender, MouseEventArgs e) => this.OnMouseDown(e);
        private void SkHybridControl_MouseMove(object sender, MouseEventArgs e) => this.OnMouseMove(e);
        private void SkHybridControl_MouseUp(object sender, MouseEventArgs e) => this.OnMouseUp(e);
        private void SkHybridControl_MouseClick(object sender, MouseEventArgs e) => this.OnMouseClick(e);
        private void SkHybridControl_Resize(object sender, EventArgs e)
        {
            var tmp = BoxShadow;
            BoxShadow = null;
            tmp.DisposeSafely();

            if (IsGpuAvailable && !this.DesignMode)
            {
                this.glCanvas?.Invalidate();
            }
            else
            {
                this.Canvas?.Invalidate();
            }
        }

        private SKImage BoxShadow { get; set; }
        public static SKImage CalculateBoxShadow(SKRect rect)
        {
            using SKBitmap bm = new SKBitmap((int)rect.Width, (int)rect.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using var canvas = new SKCanvas(bm);
            DrawBoxWithInnerShadows(canvas, rect);
            canvas.Save();
            return SKImage.FromBitmap(bm);
        }

        public static void DrawBoxWithInnerShadows(SKCanvas canvas, SKRect rect)
        {
            // Draw each inset shadow layer, as per your CSS parameters.
            //    (The layering remains the same; we just handle each shadow in a simpler way.)
            // box-shadow:  inset 0 6.4px 14.4px 0 rgba(0, 0, 0, 0.132),
            //              inset 0 1.2px 3.6px 0 rgba(0, 0, 0, 0.216),
            //              inset 0 .3px .9px 0 rgba(0, 0, 0, 0.316);
            DrawInsetShadow(canvas, rect, offsetY: 6.4f, blur: 14.4f, opacity: 0.432f);
            DrawInsetShadow(canvas, rect, offsetY: 1.2f, blur: 3.6f, opacity: 0.616f);
            DrawInsetShadow(canvas, rect, offsetY: 0.3f, blur: 0.9f, opacity: 0.816f);
        }

        /// <summary>
        /// Draws one inset shadow layer by creating a temporary layer (SaveLayer), 
        /// filling it with a blurred color, then cutting out the center with DstOut.
        /// This avoids building a path and uses simpler compositing operations.
        /// </summary>
        private static void DrawInsetShadow(SKCanvas canvas, SKRect rect, float offsetY, float blur, float opacity)
        {
            // Convert opacity 0..1 => 0..255
            byte alpha = (byte)(opacity * 255);

            // Expand to ensure enough space for blur and offset.
            float expand = blur + Math.Abs(offsetY);
            var outerRect = new SKRect(
                rect.Left - expand,
                rect.Top - expand,
                rect.Right + expand,
                rect.Bottom + expand
            );

            // We only want the shadow to appear inside the base rectangle.
            canvas.Save();
            canvas.ClipRect(rect);

            // Translate in the opposite direction of the offset so the shadow 
            // “pulls” inward, as CSS inset shadows do.
            canvas.Translate(0, -offsetY);

            // 1. Begin a new layer with the shadow color and blur.
            using var shadowPaint = new SKPaint
            {
                IsAntialias = true,
                BlendMode = SKBlendMode.SrcOver,
                Color = new SKColor(0, 0, 0, alpha),
                ImageFilter = SKImageFilter.CreateBlur(blur, blur)
            };
            // SaveLayer creates a temporary layer that we can blend back onto the canvas.
            canvas.SaveLayer(shadowPaint);

            // 2. Fill the entire expanded rectangle with our blurred paint.
            canvas.DrawRect(outerRect, shadowPaint);

            // 3. “Cut out” the center of that layer using DstOut blend mode,
            //    so only the ring (the edges) remain.
            using var cutPaint = new SKPaint { BlendMode = SKBlendMode.DstOut };
            canvas.DrawRect(rect, cutPaint);

            // 4. End the layer.
            canvas.Restore();

            // 5. Restore the original clip and translation.
            canvas.Restore();
        }
    }

    public class GenericSKPaintSurfaceEventArgs : EventArgs
    {
        public SKSurface Surface { get; set; }
        public SKRectI Rect { get; internal set; }
        public GenericSKPaintSurfaceEventArgs() { }
    }
}
