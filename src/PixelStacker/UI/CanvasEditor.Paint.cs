using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class CanvasEditor
    {
        private ThreadSafe<bool> IsPainting = new ThreadSafe<bool>(false);

        [System.Diagnostics.DebuggerStepThrough]
        private void timerPaint_Tick(object sender, EventArgs e)
        {
            if (this.RepaintRequested)
            {
                if (!IsPainting.Value)
                {
                    IsPainting.Value = true;
                    // Force repaint
                    Refresh();
                    this.RepaintRequested = false;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            if (this.DesignMode)
            {
                using Brush bgBrush = new TextureBrush(Resources.UIResources.bg_imagepanel);
                g.FillRectangle(bgBrush, 0, 0, this.Width, this.Height);
                return;
            }

            base.OnPaint(e);

            var painter = this.Painter;
            if (painter == null) return;

            var pz = this.PanZoomSettings;
            if (pz == null) return;

            var canvas = this.Canvas;
            if (canvas == null) return;

            if (pz.zoomLevel < 1.0D)
            {
                g.InterpolationMode = InterpolationMode.Low;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.SmoothingMode = SmoothingMode.HighSpeed;
            }
            else
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }

            painter.RenderToView(g, this.Size, pz);
            DrawWorldEditOrigin(g);
            DrawGridLines(g);
            DrawBorder(g);
            IsPainting.Value = false;
        }

        private void DrawBorder(Graphics g)
        {
        }

        private void DrawWorldEditOrigin(Graphics g)
        {
        }

        protected void DrawGridLines(Graphics g)
        {

        }
    }
}
