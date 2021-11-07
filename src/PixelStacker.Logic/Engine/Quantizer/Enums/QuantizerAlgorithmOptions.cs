using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Engine.Quantizer.ColorCaches;
using PixelStacker.Logic.Engine.Quantizer.ColorCaches.EuclideanDistance;
using PixelStacker.Logic.Engine.Quantizer.Ditherers;
using SixLabors.ImageSharp.Processing.Processors.Dithering;
using System.Collections.Generic;

namespace PixelStacker.Logic.Engine.Quantizer.Enums
{
    public class QuantizerAlgorithmOptions
    {
        public OrderedDictionary<string, IDither> DithererList = new OrderedDictionary<string, IDither>() { { "No dithering", null } };
        public List<int> MaxColorCountsList = new List<int>() { 256 };

        /// <summary>
        /// The max number of processes allowed.
        /// </summary>
        public List<int> MaxParallelProcessesList = new List<int>() { 1 };
    }
}
