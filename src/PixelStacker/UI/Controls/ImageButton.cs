using PixelStacker.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public enum ImageButtonPushState
    {
        Normal,
        Pressed,
        Hover,
        Disabled
    }

    [ToolboxItemFilter("PixelStacker.UI.Controls.ImageButton", ToolboxItemFilterType.Require)]
    public partial class ImageButton : UserControl
    {
        public ImageButtonPushState PushState { get; set; } = ImageButtonPushState.Normal;
        public bool IsChecked { get; set; } = false;

        public void SetTooltip(string tipMsg, string tipTitle = null)
        {
            this.toolTip1.ToolTipTitle = tipTitle;
            this.toolTip1.SetToolTip(this, tipMsg);
        }

        private Bitmap _image;
        public Bitmap Image { 
            get => this._image; 
            set {
                this._image.DisposeSafely();
                this._image = value;
                this.Invalidate();
            }
        }

        public ImageButton()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.AntiAlias; 
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            using Brush bgBrush = new TextureBrush(Resources.UIResources.bg_imagepanel);
            g.FillRectangle(bgBrush, 0, 0, this.Width, this.Height);
            //base.OnPaint(e);

            if (this.Image != null)
            {
                g.DrawImage(this.Image, 0, 0, this.Width, this.Height);
            }

            using Pen pen = new Pen(Color.Black, 2);
            g.DrawRectangle(pen, 1, 1, this.Width - 2, this.Height - 2);
            if (PushState == ImageButtonPushState.Hover)
            {
                using Brush pPen = new SolidBrush(Color.FromArgb(127, 255, 255, 255));
                g.FillRectangle(pPen, this.ClientRectangle);
            }
        }

        private void ImageButton_MouseHover(object sender, EventArgs e) => this.PushState = ImageButtonPushState.Hover;

        private void ImageButton_MouseLeave(object sender, EventArgs e) => this.PushState = ImageButtonPushState.Normal;

        private void ImageButton_MouseDown(object sender, MouseEventArgs e) => this.PushState = ImageButtonPushState.Pressed;

        private void ImageButton_MouseUp(object sender, MouseEventArgs e) => this.PushState = ImageButtonPushState.Normal;
    }
}
