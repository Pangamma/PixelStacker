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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PixelStacker.UI
{
    public partial class ImagePanel : UserControl, IDisposable
    {
        private Bitmap image;
        private Point initialDragPoint;
        private bool IsDragging = false;

        public ImagePanel()
        {
            InitializeComponent();
        }

        public void SetImage(Bitmap src)
        {
            bool preserveZoom = MainForm.PanZoomSettings != null;
            //(this.image != null && this.image.Width == src.Width && this.image.Height == src.Height);

            this.image.DisposeSafely();
            this.image = src.To32bppBitmap();

            if (!preserveZoom)
            {
                var settings = new PanZoomSettings()
                {
                    initialImageX = 0,
                    initialImageY = 0,
                    imageX = 0,
                    imageY = 0,
                    zoomLevel = 0,
                    maxZoomLevel = 100.0D,
                    minZoomLevel = 0.0D,
                };

                using (var padlock = AsyncDuplicateLock.Get.Lock(this.image))
                {
                    double wRatio = (double)Width / src.Width;
                    double hRatio = (double)Height / src.Height;
                    if (hRatio < wRatio)
                    {
                        settings.zoomLevel = hRatio;
                        settings.imageX = (Width - (int)(src.Width * hRatio)) / 2;
                    }
                    else
                    {
                        settings.zoomLevel = wRatio;
                        settings.imageY = (Height - (int)(src.Height * wRatio)) / 2;
                    }
                    int numICareAbout = Math.Max(image.Width, image.Height);
                    settings.minZoomLevel = (100.0D / numICareAbout);
                    if (settings.minZoomLevel > 1.0D)
                    {
                        settings.minZoomLevel = 1.0D;
                    }
                }

                MainForm.PanZoomSettings = settings;
            }
            
            Refresh();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            {
                Graphics g = e.Graphics;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                if (this.image != null)
                {
                    using (var padlock = AsyncDuplicateLock.Get.Lock(this.image))
                    {

                        double origW = this.image.Width;
                        double origH = this.image.Height;
                        int w = (int)(origW * MainForm.PanZoomSettings.zoomLevel);
                        int h = (int)(origH * MainForm.PanZoomSettings.zoomLevel);
                        
                        g.DrawImage(this.image, MainForm.PanZoomSettings.imageX, MainForm.PanZoomSettings.imageY, w+1, h+1);
                    }
                }
            }
        }

        #region Mouse Events
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta != 0)
            {
                Point panelPoint = e.Location;
                Point imagePoint = this.getPointOnImage(panelPoint, EstimateProp.Round);
                if (e.Delta < 0)
                {
                    MainForm.PanZoomSettings.zoomLevel *= 0.8;
                }
                else
                {
                    MainForm.PanZoomSettings.zoomLevel *= 1.2;
                }
                this.restrictZoom();
                MainForm.PanZoomSettings.imageX = ((int)Math.Round(panelPoint.X - imagePoint.X * MainForm.PanZoomSettings.zoomLevel));
                MainForm.PanZoomSettings.imageY = ((int)Math.Round(panelPoint.Y - imagePoint.Y * MainForm.PanZoomSettings.zoomLevel));
                this.Refresh();
            }
        }

        private void ImagePanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.initialDragPoint = e.Location;
            MainForm.PanZoomSettings.initialImageX = MainForm.PanZoomSettings.imageX;
            MainForm.PanZoomSettings.initialImageY = MainForm.PanZoomSettings.imageY;
            this.Cursor = new Cursor(Resources.cursor_handclosed.GetHicon());
            this.IsDragging = true;
        }

        private void ImagePanel_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            this.IsDragging = false;
        }

        private void ImagePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                Point point = e.Location;
                MainForm.PanZoomSettings.imageX = MainForm.PanZoomSettings.initialImageX - (this.initialDragPoint.X - point.X);
                MainForm.PanZoomSettings.imageY = MainForm.PanZoomSettings.initialImageY - (this.initialDragPoint.Y - point.Y);
                Refresh();
            }
        }
        #endregion 

        private Point getPointOnImage(Point pointOnPanel, EstimateProp prop)
        {
            if (prop == EstimateProp.Ceil)
            {
                return new Point((int)Math.Ceiling((pointOnPanel.X - MainForm.PanZoomSettings.imageX) / MainForm.PanZoomSettings.zoomLevel), (int)Math.Ceiling((pointOnPanel.Y - MainForm.PanZoomSettings.imageY) / MainForm.PanZoomSettings.zoomLevel));
            }
            if (prop == EstimateProp.Floor)
            {
                return new Point((int)Math.Floor((pointOnPanel.X - MainForm.PanZoomSettings.imageX) / MainForm.PanZoomSettings.zoomLevel), (int)Math.Floor((pointOnPanel.Y - MainForm.PanZoomSettings.imageY) / MainForm.PanZoomSettings.zoomLevel));
            }
            return new Point((int)Math.Round((pointOnPanel.X - MainForm.PanZoomSettings.imageX) / MainForm.PanZoomSettings.zoomLevel), (int)Math.Round((pointOnPanel.Y - MainForm.PanZoomSettings.imageY) / MainForm.PanZoomSettings.zoomLevel));
        }

        private Point getPointOnPanel(Point pointOnImage)
        {
            return new Point((int)Math.Round(pointOnImage.X * MainForm.PanZoomSettings.zoomLevel + MainForm.PanZoomSettings.imageX), (int)Math.Round(pointOnImage.Y * MainForm.PanZoomSettings.zoomLevel + MainForm.PanZoomSettings.imageY));
        }

        private void restrictZoom()
        {
            MainForm.PanZoomSettings.zoomLevel = (MainForm.PanZoomSettings.zoomLevel < MainForm.PanZoomSettings.minZoomLevel ? MainForm.PanZoomSettings.minZoomLevel : MainForm.PanZoomSettings.zoomLevel > MainForm.PanZoomSettings.maxZoomLevel ? MainForm.PanZoomSettings.maxZoomLevel : MainForm.PanZoomSettings.zoomLevel);
        }

        public Bitmap getImage()
        {
            return this.image;
        }

        private Point getShowingStart()
        {
            Point showingStart = getPointOnImage(new Point(0, 0), EstimateProp.Floor);
            showingStart.X = (showingStart.X < 0 ? 0 : showingStart.X);
            showingStart.Y = (showingStart.Y < 0 ? 0 : showingStart.Y);
            return showingStart;
        }

        private Point getShowingEnd(double origW, double origH)
        {
            Point showingEnd = getPointOnImage(new Point(Width, Height), EstimateProp.Ceil);
            showingEnd.X = (showingEnd.X > origW ? (int)origW : showingEnd.X);
            showingEnd.Y = (showingEnd.Y > origH ? (int)origH : showingEnd.Y);
            return showingEnd;
        }


        private int getRoundedZoomDistance(int x, int deltaX)
        {
            return (int)Math.Round(x + deltaX * MainForm.PanZoomSettings.zoomLevel);
        }

        private int getRoundedZoomX(int val, int blockSize)
        {
            return (int)Math.Floor(MainForm.PanZoomSettings.imageX + val * blockSize * MainForm.PanZoomSettings.zoomLevel);
        }

        private int getRoundedZoomY(int val, int blockSize)
        {
            return (int)Math.Floor(MainForm.PanZoomSettings.imageY + val * blockSize * MainForm.PanZoomSettings.zoomLevel);
        }

        private enum EstimateProp
        {
            Floor, Ceil, Round
        }
    }

}
