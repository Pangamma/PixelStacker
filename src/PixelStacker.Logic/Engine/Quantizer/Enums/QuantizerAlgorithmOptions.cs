using SimplePaletteQuantizer.ColorCaches;
using SimplePaletteQuantizer.ColorCaches.Common;
using SimplePaletteQuantizer.ColorCaches.EuclideanDistance;
using SimplePaletteQuantizer.Ditherers;
using SimplePaletteQuantizer.Quantizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Engine.Quantizer.Enums
{
    public class QuantizerAlgorithmOptions
    {
        public OrderedDictionary<string, IColorDitherer> DithererList = new OrderedDictionary<string, IColorDitherer>() { { "No dithering", null } };
        public OrderedDictionary<string, IColorCache> ColorCacheList = new OrderedDictionary<string, IColorCache>() { { "Euclidean distance", new EuclideanDistanceColorCache() } };
        public List<int> MaxColorCountsList = new List<int>() { 256 };

        /// <summary>
        /// The max number of processes allowed.
        /// </summary>
        public List<int> MaxParallelProcessesList = new List<int>() { 1 };
    }
}
