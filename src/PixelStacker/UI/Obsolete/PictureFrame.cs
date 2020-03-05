using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PixelStacker.Logic;
using PixelStacker.Logic.Extensions;

namespace PixelStacker.UI
{
    public partial class PictureFrame : UserControl
    {
        public Color BorderColor { get; set; } = Color.Black;
        public int BorderWidth { get; set; } = 4;
        private Bitmap Image { get; set; } = null;

        public PictureFrame()
        {
            InitializeComponent();
        }

        public void SetImage(Bitmap img)
        {
            this.Image = img.To32bppBitmap(this.Width - this.BorderWidth * 2, this.Height - this.BorderWidth * 2);
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            {
                Graphics g = e.Graphics;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                if (this.Image != null)
                {
                    g.DrawImage(this.Image, this.BorderWidth, this.BorderWidth, this.Width - (2 * this.BorderWidth), this.Height - (2 * this.BorderWidth));
                }

                if (this.BorderWidth > 0)
                {
                    using (Brush b = new SolidBrush(this.BorderColor))
                    {
                        g.FillRectangle(b, 0, 0, this.Width, this.BorderWidth); // top
                        g.FillRectangle(b, 0, 0, this.BorderWidth, this.Height); // left
                        g.FillRectangle(b, this.Width - this.BorderWidth, 0, this.BorderWidth, this.Height); // right
                        g.FillRectangle(b, 0, this.Height - this.BorderWidth, this.Width, this.BorderWidth); // bottom
                    }
                }

                //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                //if (this.image != null)
                //{
                //    using (var padlock = AsyncDuplicateLock.Get.Lock(this.image))
                //    {

                //        double origW = this.image.Width;
                //        double origH = this.image.Height;
                //        int w = (int) (origW * MainForm.PanZoomSettings.zoomLevel);
                //        int h = (int) (origH * MainForm.PanZoomSettings.zoomLevel);

                //        g.DrawImage(this.image, MainForm.PanZoomSettings.imageX, MainForm.PanZoomSettings.imageY, w + 1, h + 1);
                //    }
                //}
            }
        }

    }
}
