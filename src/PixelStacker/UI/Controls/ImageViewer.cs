using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Resources.Themes;
using PixelStacker.UI.Helpers;
using SkiaSharp;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.WF.Components
{
    [ToolboxItemFilter("PixelStacker.WF.Components.ImageViewer", ToolboxItemFilterType.Require)]
    public partial class ImageViewer : UserControl
    {
        private Point InitialDragPoint;
        private object Padlock = new { };
        private bool IsDragging = false;
        private bool _WasDragged = false;
        private bool IsRepaintRequested
        {
            get
            {
                lock (Padlock)
                {
                    return _WasDragged;
                }
            }
            set
            {
                lock (Padlock)
                {
                    _WasDragged = value;
                }
            }
        }

        public PanZoomSettings PanZoomSettings { get; set; }

        public ImageViewer()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            repaintTimer.Interval = Constants.DisplayRefreshIntervalMs;
            this.PanZoomSettings = CalculateInitialPanZoomSettings(null);

            ThemeManager.OnThemeChange += this.OnThemeChange;
            this.OnThemeChange(null, ThemeManager.Theme);
        }

        private void OnThemeChange(object sender, ThemeChangeEventArgs e)
        {
            this.BackgroundImage = ThemeHelper.bg_imagepanel;
            var skbg = this.SKBackgroundImage;
            this.SKBackgroundImage = ThemeHelper.bg_imagepanel.BitmapToSKBitmap();
            skbg.DisposeSafely();
        }

        public void SetImage(SKBitmap src, PanZoomSettings pz = null)
        {
            Image.DisposeSafely();
            Image = null;
            Image = src.Copy();
            this.PanZoomSettings = pz != null ? pz : CalculateInitialPanZoomSettings(Image);
            this.IsRepaintRequested = true;
        }

        private PanZoomSettings CalculateInitialPanZoomSettings(SKBitmap src)
        {
            var settings = new PanZoomSettings()
            {
                initialImageX = 0,
                initialImageY = 0,
                imageX = 0,
                imageY = 0,
                zoomLevel = 0,
                maxZoomLevel = Constants.MAX_ZOOM,
                minZoomLevel = Constants.MIN_ZOOM
            };

            if (src != null)
            {
                lock (src)
                {
                    settings = PanZoomSettings.CalculateDefaultPanZoomSettings(src.Width, src.Height, Width, Height);
                }
            }

            return settings;
        }

        [Category("ImageViewer")]
        [Browsable(true)]
        private SKBitmap Image { get; set; }
        private SKBitmap SKBackgroundImage { get; set; }

        private void skCanvas_PaintSurface(object sender, UI.Controls.GenericSKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            var canvas = surface.Canvas;
            var bgImg = this.SKBackgroundImage;
            SKShader bgShader = SKShader.CreateBitmap(bgImg, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);
            using (SKPaint paint = new SKPaint())
            {
                paint.Shader = bgShader;
                paint.IsDither = true;
                canvas.DrawRect(e.Rect, paint);
                canvas.DrawBitmap(bgImg, 0, 0);
            }

            // Render the image they are looking at.
            var pz = this.PanZoomSettings;
            var img = this.Image;
            if (img != null && pz != null)
            {
                SKPoint pStart = GetPointOnImage(new SKPoint(0, 0), EstimateProp.Floor);
                SKPoint fStart = GetPointOnPanel(pStart);

                SKPoint pEnd = GetPointOnImage(new SKPoint(this.Width, this.Height), EstimateProp.Ceil);
                SKPoint fEnd = GetPointOnPanel(pEnd);

                SKRect rectSRC = pStart.ToRectangle(pEnd);
                SKRect rectDST = fStart.ToRectangle(fEnd);

                lock (img)
                {
                    canvas.DrawBitmap(bitmap: img,
                        source: rectSRC,
                        dest: rectDST);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                this.PaintDesignerView(e);
                return;
            }

            base.OnPaint(e);
        }

        private enum EstimateProp
        {
            Floor, Ceil, Round
        }

        private SKPoint GetPointOnImage(SKPoint pointOnPanel, EstimateProp prop)
        {
            int x, y;

            if (prop == EstimateProp.Ceil)
            {
                x = (int)Math.Ceiling((pointOnPanel.X - this.PanZoomSettings.imageX) / this.PanZoomSettings.zoomLevel);
                y = (int)Math.Ceiling((pointOnPanel.Y - this.PanZoomSettings.imageY) / this.PanZoomSettings.zoomLevel);
            }
            else if (prop == EstimateProp.Floor)
            {
                x = (int)Math.Floor((pointOnPanel.X - this.PanZoomSettings.imageX) / this.PanZoomSettings.zoomLevel);
                y = (int)Math.Floor((pointOnPanel.Y - this.PanZoomSettings.imageY) / this.PanZoomSettings.zoomLevel);
            }
            else
            {
                x = (int)Math.Round((pointOnPanel.X - this.PanZoomSettings.imageX) / this.PanZoomSettings.zoomLevel);
                y = (int)Math.Round((pointOnPanel.Y - this.PanZoomSettings.imageY) / this.PanZoomSettings.zoomLevel);
            }

            return new SKPoint(x, y);
        }

        public SKPoint GetPointOnPanel(SKPoint pointOnImage)
        {
            var pz = this.PanZoomSettings;
            if (pz == null)
            {
#if FAIL_FAST
                throw new ArgumentNullException("PanZoomSettings are not set. So weird!");
#else
                return new SKPoint(0, 0);
#endif
            }

            var pointOnPanel = new SKPoint((int)Math.Round(pointOnImage.X * pz.zoomLevel + pz.imageX), (int)Math.Round(pointOnImage.Y * pz.zoomLevel + pz.imageY));

            return pointOnPanel;
        }


        private void restrictZoom()
        {
            this.PanZoomSettings.zoomLevel = (this.PanZoomSettings.zoomLevel < this.PanZoomSettings.minZoomLevel ? this.PanZoomSettings.minZoomLevel : this.PanZoomSettings.zoomLevel > this.PanZoomSettings.maxZoomLevel ? this.PanZoomSettings.maxZoomLevel : this.PanZoomSettings.zoomLevel);
        }

        #region Mouse Events
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta != 0)
            {
                SKPoint panelPoint = new SKPoint(e.Location.X, e.Location.Y);
                SKPoint imagePoint = this.GetPointOnImage(panelPoint, EstimateProp.Round);
                if (e.Delta < 0)
                {
                    this.PanZoomSettings.zoomLevel *= 0.8;
                }
                else
                {
                    this.PanZoomSettings.zoomLevel *= 1.25;
                }

                this.restrictZoom();
                this.PanZoomSettings.imageX = ((int)Math.Round(panelPoint.X - imagePoint.X * this.PanZoomSettings.zoomLevel));
                this.PanZoomSettings.imageY = ((int)Math.Round(panelPoint.Y - imagePoint.Y * this.PanZoomSettings.zoomLevel));
                this.IsRepaintRequested = true;
            }
        }

        private void ImageViewer_MouseDown(object sender, MouseEventArgs e)
        {
            this.InitialDragPoint = e.Location;
            this.PanZoomSettings.initialImageX = this.PanZoomSettings.imageX;
            this.PanZoomSettings.initialImageY = this.PanZoomSettings.imageY;
            this.IsDragging = true;
        }

        private void ImageViewer_MouseUp(object sender, MouseEventArgs e)
        {
            this.IsDragging = false;
        }

        private void ImageViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                Point point = e.Location;
                this.PanZoomSettings.imageX = this.PanZoomSettings.initialImageX - (this.InitialDragPoint.X - point.X);
                this.PanZoomSettings.imageY = this.PanZoomSettings.initialImageY - (this.InitialDragPoint.Y - point.Y);
                this.IsRepaintRequested = true;
            }
        }

        #endregion

        [System.Diagnostics.DebuggerStepThrough]
        private void repaintTimer_Tick(object sender, EventArgs e)
        {
            if (IsRepaintRequested)
            {
                // repaint self, and child controls. Including the skcontrol
                this.Refresh();
                IsRepaintRequested = false;
            }
        }
    }
}
