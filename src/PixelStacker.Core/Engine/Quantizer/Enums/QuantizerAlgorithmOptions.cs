using PixelStacker.Core.Collections;
using PixelStacker.Core.Engine.Quantizer.ColorCaches;
using PixelStacker.Core.Engine.Quantizer.ColorCaches.EuclideanDistance;
using PixelStacker.Core.Engine.Quantizer.Ditherers;
using PixelStacker.Core.Engine.Quantizer.ColorCaches.Common;
using PixelStacker.Core.Engine.Quantizer.Quantizers;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Core.Engine.Quantizer.Enums
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
