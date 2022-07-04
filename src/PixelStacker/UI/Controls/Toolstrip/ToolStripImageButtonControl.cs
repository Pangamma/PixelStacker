using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;

namespace PixelStacker.UI.Controls.Toolstrip
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public partial class ToolStripImageButtonControl : UserControl
    {
        public ToolStripImageButtonControl()
        {
            InitializeComponent();
        }

        [Category("PixelStacker")]
        public Bitmap Image { get; set; } = null;

        protected override void OnPaint(PaintEventArgs e)
        {
            using Pen p = new Pen(Color.Black, 2);
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            if (this.Image != null)
            {
                g.DrawImage(this.Image, 0, 0, this.Width, this.Height);
            }

            g.DrawRectangle(p, 0, 0, this.Width - 2, this.Height - 2);
        }
    }
}
