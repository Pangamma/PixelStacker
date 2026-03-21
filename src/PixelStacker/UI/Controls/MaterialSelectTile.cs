using PixelStacker.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.WF.Components
{
    public partial class MaterialSelectTile : UserControl
    {
        private Material _material;
        private Bitmap _cachedBitmap;
        private bool _cachedIsSideView;

        public Material Material
        {
            get => _material;
            set
            {
                if (_material == value) return;
                _material = value;
                _cachedBitmap?.Dispose();
                _cachedBitmap = null;
            }
        }

        public Options Opts { get; set; } = null;

        private Bitmap GetCachedBitmap(bool isv)
        {
            if (_cachedBitmap == null || _cachedIsSideView != isv)
            {
                _cachedBitmap?.Dispose();
                _cachedBitmap = _material.GetImage(isv).SKBitmapToBitmap();
                _cachedIsSideView = isv;
            }
            return _cachedBitmap;
        }

        public MaterialSelectTile()
        {
            InitializeComponent();
            this.TabStop = true;
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            Graphics g = args.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            bool isv = Opts?.IsSideView ?? false;
            //Brush brHover = new SolidBrush(Color.FromArgb(100, 0, 0, 0));

            using Pen penShadow = new Pen(Color.FromArgb(50, 50, 50, 100))
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Solid,
                Width = 2
            };

            if (this.Material != null)
            {
                var img = GetCachedBitmap(isv);
                g.DrawImage(img, 0, 0, this.Width, this.Height);
                g.DrawRectangle(penShadow, 0, 0, this.Width, this.Height);

                if (this.isHovered)
                {
                    using Brush brHover = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
                    g.FillRectangle(brHover, 0, 0, this.Width, this.Height);
                }

                if (this.Material.IsEnabledF(this.Opts))
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
                this.Material.IsEnabledF(this.Opts, !this.Material.IsEnabledF(this.Opts));
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
