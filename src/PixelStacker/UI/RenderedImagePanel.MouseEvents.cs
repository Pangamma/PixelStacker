using System;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class RenderedImagePanel
    {
        private bool WasDragged
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

        private enum EstimateProp
        {
            Floor, Ceil, Round
        }
        private void restrictZoom()
        {
            this.PanZoomSettings.zoomLevel = (this.PanZoomSettings.zoomLevel < this.PanZoomSettings.minZoomLevel ? this.PanZoomSettings.minZoomLevel : this.PanZoomSettings.zoomLevel > this.PanZoomSettings.maxZoomLevel ? this.PanZoomSettings.maxZoomLevel : this.PanZoomSettings.zoomLevel);
        }

        private Point GetPointOnImage(Point pointOnPanel, EstimateProp prop)
        {
            if (prop == EstimateProp.Ceil)
            {
                return new Point((int)Math.Ceiling((pointOnPanel.X - this.PanZoomSettings.imageX) / this.PanZoomSettings.zoomLevel), (int)Math.Ceiling((pointOnPanel.Y - this.PanZoomSettings.imageY) / this.PanZoomSettings.zoomLevel));
            }
            if (prop == EstimateProp.Floor)
            {
                return new Point((int)Math.Floor((pointOnPanel.X - this.PanZoomSettings.imageX) / this.PanZoomSettings.zoomLevel), (int)Math.Floor((pointOnPanel.Y - this.PanZoomSettings.imageY) / this.PanZoomSettings.zoomLevel));
            }
            return new Point((int)Math.Round((pointOnPanel.X - this.PanZoomSettings.imageX) / this.PanZoomSettings.zoomLevel), (int)Math.Round((pointOnPanel.Y - this.PanZoomSettings.imageY) / this.PanZoomSettings.zoomLevel));
        }

        public Point GetPointOnPanel(Point pointOnImage)
        {
            var pz = this.PanZoomSettings;
            if (pz == null)
            {
#if DEBUG
                throw new ArgumentNullException("PanZoomSettings are not set. So weird!");
#else
                return new Point(0, 0);
#endif
            }

            return new Point((int)Math.Round(pointOnImage.X * pz.zoomLevel + pz.imageX), (int)Math.Round(pointOnImage.Y * pz.zoomLevel + pz.imageY));
        }

        #region Mouse Events
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta != 0)
            {
                Point panelPoint = e.Location;
                Point imagePoint = this.GetPointOnImage(panelPoint, EstimateProp.Round);
                if (e.Delta < 0)
                {
                    this.PanZoomSettings.zoomLevel *= 0.65;
                }
                else
                {
                    this.PanZoomSettings.zoomLevel *= 1.50;
                }
                this.restrictZoom();
                this.PanZoomSettings.imageX = ((int)Math.Round(panelPoint.X - imagePoint.X * this.PanZoomSettings.zoomLevel));
                this.PanZoomSettings.imageY = ((int)Math.Round(panelPoint.Y - imagePoint.Y * this.PanZoomSettings.zoomLevel));
                this.WasDragged = true;
            }
        }

        private void ImagePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.initialDragPoint = e.Location;
                this.PanZoomSettings.initialImageX = this.PanZoomSettings.imageX;
                this.PanZoomSettings.initialImageY = this.PanZoomSettings.imageY;
                this.Cursor = new Cursor(Resources.UIResources.cursor_handclosed.GetHicon());
                this.IsDragging = true;
            }
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
                //EditorPanel.this.fitToSize = false;
                Point point = e.Location;
                this.PanZoomSettings.imageX = this.PanZoomSettings.initialImageX - (this.initialDragPoint.X - point.X);
                this.PanZoomSettings.imageY = this.PanZoomSettings.initialImageY - (this.initialDragPoint.Y - point.Y);
                this.WasDragged = true;
            }
        }
        #endregion 

    }
}
