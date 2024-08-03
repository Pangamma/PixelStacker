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

                this.Controls.Add(control);
            }

            ThemeManager.OnThemeChange += this.OnThemeChange;
            this.OnThemeChange(this, ThemeManager.Theme);
        }

        private void OnThemeChange(object sender, ThemeChangeEventArgs e)
        {
            this.BackgroundImage = ThemeHelper.bg_imagepanel;
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

        private void SkHybridControl_MouseDoubleClick(object sender, MouseEventArgs e) => this.OnMouseDoubleClick(e);
        private void SkHybridControl_MouseDown(object sender, MouseEventArgs e) => this.OnMouseDown(e);
        private void SkHybridControl_MouseMove(object sender, MouseEventArgs e) => this.OnMouseMove(e);
        private void SkHybridControl_MouseUp(object sender, MouseEventArgs e) => this.OnMouseUp(e);
        private void SkHybridControl_MouseClick(object sender, MouseEventArgs e) => this.OnMouseClick(e);
        private void SkHybridControl_Resize(object sender, EventArgs e)
        {
            if (IsGpuAvailable && !this.DesignMode)
            {
                this.glCanvas?.Invalidate();
            }
            else
            {
                this.Canvas?.Invalidate();
            }
        }
    }

    public class GenericSKPaintSurfaceEventArgs : EventArgs
    {
        public SKSurface Surface { get; set; }
        public SKRectI Rect { get; internal set; }
        public GenericSKPaintSurfaceEventArgs() { }
    }
}
