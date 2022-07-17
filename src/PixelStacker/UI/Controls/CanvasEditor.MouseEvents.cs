using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        public bool RepaintRequested = false;

        private void restrictZoom()
        {
            this.PanZoomSettings.zoomLevel = (this.PanZoomSettings.zoomLevel < this.PanZoomSettings.minZoomLevel ? this.PanZoomSettings.minZoomLevel : this.PanZoomSettings.zoomLevel > this.PanZoomSettings.maxZoomLevel ? this.PanZoomSettings.maxZoomLevel : this.PanZoomSettings.zoomLevel);
        }

        public static Point GetPointOnImage(Point pointOnPanel, PanZoomSettings pz, EstimateProp prop)
        {
            if (prop == EstimateProp.Ceil)
            {
                return new Point((int)Math.Ceiling((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Ceiling((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
            }
            if (prop == EstimateProp.Floor)
            {
                return new Point((int)Math.Floor((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Floor((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
            }
            return new Point((int)Math.Round((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Round((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
        }

        public static Point GetPointOnPanel(Point pointOnImage, PanZoomSettings pz)
        {
            if (pz == null)
            {
#if FAIL_FAST
                throw new ArgumentNullException("PanZoomSettings are not set. So weird!");
#else
                        return new Point(0, 0);
#endif
            }

            return new Point((int)Math.Round(pointOnImage.X * pz.zoomLevel + pz.imageX), (int)Math.Round(pointOnImage.Y * pz.zoomLevel + pz.imageY));
        }

        #region Mouse Events

        private void ImagePanel_DoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void ImagePanel_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                PanZoomTool.OnClick(e);
                return;
            }

            CurrentTool?.OnClick(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta != 0)
            {
                Point panelPoint = e.Location;
                Point imagePoint = GetPointOnImage(panelPoint, this.PanZoomSettings, EstimateProp.Round);
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
                this.RepaintRequested = true;
            }
        }

        private void ImagePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                this.PanZoomTool.OnMouseDown(e);
                return;
            }

            this.CurrentTool?.OnMouseDown(e);
        }

        private void ImagePanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                this.PanZoomTool.OnMouseUp(e);
                return;
            }

            this.CurrentTool?.OnMouseUp(e);
        }

        private Point previousCursorPosition = new Point(0, 0);
        private Point previousPointOnImage = new Point(0, 0);

        private void ImagePanel_MouseMove(object sender, MouseEventArgs e)
        {
            var pt = GetPointOnImage(e.Location, this.PanZoomSettings, EstimateProp.Floor);
            var cd = this.Canvas?.CanvasData;

            if (previousPointOnImage != pt)
            {
                MaterialCombination mc = null;
                if (cd != null)
                {
                    if (cd.IsInRange(pt.X, pt.Y))
                    {
                        mc = cd[pt.X, pt.Y];
                    }
                }

                mc ??= MaterialPalette.FromResx()[Constants.MaterialCombinationIDForAir];

                string hoverText = $"X: {pt.X},".PadRight(8)
                    + $"Y: {pt.Y},".PadRight(8)
                    + $"Top: {mc.Top.Label}, Bottom: {mc.Bottom.Label}";

                if (lblHoverInfo.Text != hoverText)
                    lblHoverInfo.Text = hoverText;
                previousPointOnImage = pt;
                this.RepaintRequested = true;
            }

            previousCursorPosition = e.Location;
            if (e.Button == MouseButtons.Middle)
            {
                this.PanZoomTool.OnMouseMove(e);
                return;
            }

            this.CurrentTool?.OnMouseMove(e);
        }
        #endregion

    }
}
