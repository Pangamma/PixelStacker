using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Engine.Quantizer.ColorCaches;
using PixelStacker.Logic.Engine.Quantizer.ColorCaches.EuclideanDistance;
using PixelStacker.Logic.Engine.Quantizer.Ditherers;
using System.Collections.Generic;

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
