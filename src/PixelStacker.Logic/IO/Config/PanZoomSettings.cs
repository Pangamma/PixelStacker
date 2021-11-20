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

        public PanZoomSettings TranslateForNewSize(int srcWidth, int srcHeight, int viewerWidth, int viewerHeight)
        {
            var settings = this.Clone();
            // TODO the imageX and imageY and zoomLevel
            settings.minZoomLevel = Math.Min(1.0D, (100.0D / Math.Max(srcWidth, srcHeight)));

            return settings;
        }

        /// <summary>
        /// TAKEN FROM... VIEWER PANEL
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
                settings.zoomLevel = hRatio;
                settings.imageX = (viewerWidth - (int)(srcWidth * hRatio)) / 2;
            }
            else
            {
                settings.zoomLevel = wRatio;
                settings.imageY = (viewerHeight - (int)(srcHeight * wRatio)) / 2;
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
