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

namespace PixelStacker.UI
{
    public partial class MaterialSelectTile : UserControl
    {
        public MaterialSelectTile()
        {
            InitializeComponent();
            this.TabStop = true;
        }

        public Material Material { get; set; } = null;

        protected override void OnPaint(PaintEventArgs args)
        {
            Graphics g = args.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            bool isv = Options.Get.IsSideView;
            //Brush brHover = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
            Brush brHover = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
            Pen penFocus = new Pen(Color.FromArgb(255, 5, 105, 210))
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Solid,
                Width = 2
            };

            if (this.Material != null)
            {
                g.DrawImage(this.Material.getImage(isv), 0, 0, this.Width, this.Height);

                if (this.isHovered)
                {
                    g.FillRectangle(brHover, 0, 0, this.Width, this.Height);
                }

                if (this.Material.IsEnabled)
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                    g.DrawImage(Resources.UIResources.selected_frame_128, 0, 0, this.Width, this.Height);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                }

                if (this.isFocused)
                {
                    g.DrawRectangle(penFocus, 0, 0, this.Width, this.Height);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Space)
            {
                this.InvokeOnClick(this, new MouseEventArgs(MouseButtons.Left, 1, this.Location.X + 5, this.Location.Y + 5, 0));
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MaterialSelectTile_Click(object sender, EventArgs e)
        {
            if (this.Material != null)
            {
                this.Material.IsEnabled = !this.Material.IsEnabled;
                this.Refresh();
            }
        }

        private bool isHovered = false;
        private bool isFocused = false;

        private void MaterialSelectTile_MouseEnter(object sender, EventArgs e)
        {
            this.isHovered = true;
            this.Refresh();
        }

        private void MaterialSelectTile_MouseLeave(object sender, EventArgs e)
        {
            this.isHovered = false;
            this.Refresh();
        }

        // on focus
        private void MaterialSelectTile_Enter(object sender, EventArgs e)
        {
            this.isHovered = true;
            this.isFocused = true;
            this.Refresh();
        }

        private void MaterialSelectTile_Leave(object sender, EventArgs e)
        {
            this.isHovered = false;
            this.isFocused = false;
            this.Refresh();
        }
    }
}
