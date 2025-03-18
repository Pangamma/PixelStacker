using SkiaSharp;
using System;

namespace PixelStacker.Logic.IO.Config
{
    public class PanZoomSettings
    {
        /// <summary>
        /// Used when measuring drag distances. Starts as equal to imageX 
        /// or imageY, and is then compared to "current drag point" with a
        /// UI element of some kind.
        /// </summary>
        public int initialImageX = 0;

        /// <summary>
        /// Used when measuring drag distances. Starts as equal to imageX 
        /// or imageY, and is then compared to "current drag point" with a
        /// UI element of some kind.
        /// </summary>
        public int initialImageY = 0;

        public int imageY = 0;
        public int imageX = 0;

        /// <summary>
        /// Current zoom level. Acts as a multiplier for tile size.
        /// </summary>
        public double zoomLevel = 0;

        /// <summary>
        /// Prevent zooming IN further than this
        /// </summary>
        public double maxZoomLevel = 100.0D;

        /// <summary>
        /// Prevent zooming further away than this
        /// </summary>
        public double minZoomLevel = 0.0D;

        public PanZoomSettings Clone()
        {
            return new PanZoomSettings()
            {
                initialImageX = this.initialImageX,
                initialImageY = this.initialImageY,
                imageX = this.imageX,
                imageY = this.imageY,
                zoomLevel = this.zoomLevel,
                maxZoomLevel = this.maxZoomLevel,
                minZoomLevel = this.minZoomLevel
            };
        }

        public PanZoomSettings() { }

        public override string ToString()
        {
            return $"zoom={this.zoomLevel}, imageY={imageY}, imageX={imageX}";
        }

        /// <summary>
        /// This method is supposed to correctly update a pan and zoom config to a new set of viewing windows and image sizes.
        /// I am not sure if it actually works though. Only considers the size of the srcImage, does not consider the size of
        /// the panel. More work is required if panel size needs to be considered.
        /// </summary>
        /// <param name="prevSrcWidth"></param>
        /// <param name="nextSrcWidth"></param>
        /// <returns></returns>
        public PanZoomSettings TranslateForNewSize(double prevSrcWidth, double nextSrcWidth)
        {
            /** 
            double prevViewerWidth, double prevViewerHeight,
            double nextViewerWidth, double nextViewerHeight,
            double prevSrcWidth, double prevSrcHeight,
            double nextSrcWidth, double nextSrcHeight
            */

            var pz = this.Clone();
            var factor = (float)prevSrcWidth / nextSrcWidth;
            pz.zoomLevel = this.zoomLevel * factor;

            // I think this logic is correct as well.
            var imagePoint = new SKPoint(0, 0);
            var panelPoint = new SKPoint(
                (int)Math.Round(imagePoint.X * this.zoomLevel + this.imageX),
                (int)Math.Round(imagePoint.Y * this.zoomLevel + this.imageY));
            pz.imageX = (int)(panelPoint.X - imagePoint.X * pz.zoomLevel);
            pz.imageY = (int)(panelPoint.Y - imagePoint.Y * pz.zoomLevel);

            return pz;
        }


        /// <summary>
        /// Centers the image so that its corners are as expanded as possible while still being within the bounds of the viewing window.
        /// </summary>
        /// <param name="srcWidth"></param>
        /// <param name="srcHeight"></param>
        /// <param name="viewerWidth"></param>
        /// <param name="viewerHeight"></param>
        /// <returns></returns>
        public static PanZoomSettings CalculateDefaultPanZoomSettings(int srcWidth, int srcHeight, int viewerWidth, int viewerHeight)
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

            double wRatio = (double)viewerWidth / srcWidth;
            double hRatio = (double)viewerHeight / srcHeight;
            if (hRatio < wRatio)
            {
                // Expand up and down. It is tall image.
                settings.zoomLevel = hRatio;
                settings.imageX = (viewerWidth - (int)(srcWidth * settings.zoomLevel)) / 2;
                settings.imageY = (viewerHeight - (int)(srcHeight * settings.zoomLevel));
            }
            else
            {
                // Expand side to side. It is wide image.
                settings.zoomLevel = wRatio;
                settings.imageY = (viewerHeight - (int)(srcHeight * settings.zoomLevel)) / 2;
                settings.imageX = (viewerWidth - (int)(srcWidth * settings.zoomLevel));
            }

            int numICareAbout = Math.Max(srcWidth, srcHeight);
            settings.minZoomLevel = (100.0D / numICareAbout);
            if (settings.minZoomLevel > 1.0D)
            {
                settings.minZoomLevel = 1.0D;
            }

            return settings;
        }
    }
}
