using PixelStacker.Logic.IO.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls.MaterialPicker
{
    public partial class MaterialPickerTile : UserControl
    {
        public string TileID { get; set; }
        public Bitmap Image { get; set; } = null;
        public bool IsChecked { get; set; }

        public MaterialPickerTile()
        {
            InitializeComponent();
        }

        [Category("Appearance")]
        public event EventHandler<MaterialPickerTileClickEventArgs> ClickTile;

        private void MaterialPickerTile_Click(object sender, EventArgs e)
        {
            this.ClickTile?.Invoke(this, new MaterialPickerTileClickEventArgs()
            {
                MouseButton = Control.MouseButtons,
                Tile = this,
                TileID = this.TileID
            });
        }

        private bool isHovered = false;
        private bool isFocused = false;

        private void MaterialPickerTile_MouseEnter(object sender, EventArgs e)
        {
            this.isHovered = true;
            this.Refresh();
        }

        private void MaterialPickerTile_MouseLeave(object sender, EventArgs e)
        {
            this.isHovered = false;
            this.Refresh();
        }

        // on focus
        private void MaterialPickerTile_Enter(object sender, EventArgs e)
        {
            this.isHovered = true;
            this.isFocused = true;
            this.Refresh();
        }

        private void MaterialPickerTile_Leave(object sender, EventArgs e)
        {
            this.isHovered = false;
            this.isFocused = false;
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            Graphics g = args.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            using Pen penShadow = new Pen(Color.FromArgb(50, 50, 50, 100))
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Solid,
                Width = 2
            };

            if (this.Image != null)
            {
                g.DrawImage(this.Image, 0, 0, this.Width, this.Height);
            }

            g.DrawRectangle(penShadow, 0, 0, this.Width, this.Height);

            if (this.isHovered)
            {
                using Brush brHover = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
                g.FillRectangle(brHover, 0, 0, this.Width, this.Height);
            }

            if (this.IsChecked)
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.DrawImage(Resources.UIResources.selected_frame_128, 0, 0, this.Width, this.Height);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            }

            if (this.isFocused)
            {
                using Pen penFocus = new Pen(Color.FromArgb(255, 5, 105, 210))
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Solid,
                    Width = 2
                };

                g.DrawRectangle(penFocus, 0, 0, this.Width, this.Height);
            }
        }
    }
}
