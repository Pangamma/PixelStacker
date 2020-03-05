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
using PixelStacker.Properties;
using static System.Windows.Forms.ListViewItem;
using System.Drawing.Drawing2D;

namespace PixelStacker.UI
{
    public partial class FastPixelBox : UserControl
    {
        public string Category { get; set; }
        public SmoothingMode SmoothingMode { get; set; } = SmoothingMode.AntiAlias;
        public InterpolationMode InterpolationMode { get; set; } = InterpolationMode.NearestNeighbor;
        public Image Image { get; set; }

        public FastPixelBox()
        {
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            args.Graphics.InterpolationMode = InterpolationMode;
            args.Graphics.SmoothingMode = SmoothingMode;
            if (this.Image != null)
            {
                args.Graphics.DrawImage(this.Image, 0, 0, this.Size.Width, this.Size.Height);
            }

            //base.OnPaint(args);
        }
    }
}
