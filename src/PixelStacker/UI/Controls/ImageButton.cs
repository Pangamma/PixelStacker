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

        private bool _isChecked = false;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    this.Invalidate();
                }
            }
        }

        private Bitmap _image;
        public SkiaSharp.SKBitmap Image
        {
            set
            {
                this._image.DisposeSafely();
                this._image = null;
                this._image = value?.SKBitmapToBitmap();
                this.Invalidate();
            }
        }

        public ImageButton()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
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

            if (this._image != null)
            {
                g.DrawImage(this._image, 0, 0, this.Width, this.Height);
            }

            using Pen pen = new Pen(Color.Black, 2);
            g.DrawRectangle(pen, 1, 1, this.Width - 2, this.Height - 2);

            if (this._isChecked)
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.DrawImage(Resources.UIResources.selected_frame_128, 0, 0, this.Width, this.Height);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            }

            if (PushState == ImageButtonPushState.Hover)
            {
                using Brush pPen = new SolidBrush(Color.FromArgb(16, 0, 0, 0));
                g.FillRectangle(pPen, 3, 3, this.Width - 6, this.Height - 6);
            }
        }

        private void ImageButton_MouseEnter(object sender, EventArgs e)
        {
            this.PushState = ImageButtonPushState.Hover;
            this.Invalidate();
        }

        private void ImageButton_MouseLeave(object sender, EventArgs e)
        {
            this.PushState = ImageButtonPushState.Normal;
            this.Invalidate();
        }

        private void ImageButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.PushState = ImageButtonPushState.Pressed;
            this.Invalidate();
        }

        private void ImageButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.PushState = ImageButtonPushState.Normal;
            this.Invalidate();
        }
    }
}
