using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.IO.Config
{
    public class PanZoomSettings
    { 
        public int initialImageX = 0;
        public int initialImageY = 0;
        public int imageY = 0;
        public int imageX = 0;
        public double zoomLevel = 0;
        public double maxZoomLevel = 100.0D;
        public double minZoomLevel = 0.0D;
    }
}
