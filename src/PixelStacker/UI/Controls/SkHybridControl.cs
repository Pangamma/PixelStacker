using PixelStacker.IO;
using PixelStacker.UI.External;
using SkiaSharp;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public partial class SkHybridControl : UserControl
    {
#if DEBUG
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
            if (this.DesignMode) return;
            Control control = null;
            if (IsGpuAvailable)
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
        }

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
                return;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KonamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OnPaintSurfaceInner(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (this.PaintSurface != null)
                this.PaintSurface(this, new GenericSKPaintSurfaceEventArgs()
                {
                    Surface = e.Surface,
                    Rect = e.BackendRenderTarget.Rect
                });
        }

        private void OnPaintSurfaceInner(object sender, SKPaintSurfaceEventArgs e)
        {
            if (this.PaintSurface != null)
                this.PaintSurface(this, new GenericSKPaintSurfaceEventArgs()
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
    }

    public class GenericSKPaintSurfaceEventArgs : EventArgs
    {
        public SKSurface Surface { get; set; }
        public SKRectI Rect { get; internal set; }
        public GenericSKPaintSurfaceEventArgs() { }
    }
}
