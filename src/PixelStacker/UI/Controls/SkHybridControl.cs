using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public partial class SkHybridControl : UserControl
    {
        public static bool IsGpuAvailable => true;
        private SKGLControl glCanvas;
        private SKControl Canvas;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler<GenericSKPaintSurfaceEventArgs> PaintSurface;

        public SkHybridControl()
        {
            InitializeComponent();
            Control control;
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
                this.Controls.Add(control);
            }
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

        private void SkHybridControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        private void SkHybridControl_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        private void SkHybridControl_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }
    }

    public class GenericSKPaintSurfaceEventArgs: EventArgs
    {
        public SKSurface Surface { get; set; }
        public SKRectI Rect { get; internal set; }

        public GenericSKPaintSurfaceEventArgs() { }
    }
}
