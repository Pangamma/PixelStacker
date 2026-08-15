using PixelStacker.Logic.Collections;
using SixLabors.ImageSharp.Processing.Processors.Dithering;
using System.Collections.Generic;

namespace PixelStacker.Logic.Engine.Quantizer.Enums
{
    public class QuantizerAlgorithmOptions
    {
        public PixelStacker.Logic.Collections.OrderedDictionary<string, IDither> DithererList = new PixelStacker.Logic.Collections.OrderedDictionary<string, IDither>() { { "No dithering", null } };
        public List<int> MaxColorCountsList = new List<int>() { 256 };

        /// <summary>
        /// The max number of processes allowed.
        /// </summary>
        public List<int> MaxParallelProcessesList = new List<int>() { 1 };
    }
}
