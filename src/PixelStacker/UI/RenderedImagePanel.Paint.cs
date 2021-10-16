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
    public partial class RenderedImagePanel
    {
        private void timerPaint_Tick(object sender, EventArgs e)
        {
            if (this.WasDragged)
            {
                // Force repaint
                Refresh();
                this.WasDragged = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            if (this.DesignMode && this.BackgroundImage != null)
            {
                using Brush bgBrush = new TextureBrush(this.BackgroundImage);
                g.FillRectangle(bgBrush, 0, 0, this.Width, this.Height);
                return;
            }

            var pz = this.PanZoomSettings;

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

            DrawWorldEditOrigin(g);
            DrawGridLines(g);
            DrawBorder(g);

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
