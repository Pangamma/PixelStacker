using System;
using System.Linq;

namespace PixelStacker.Core.Model.Config
{
    public class QuantizerSettings
    {
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// Color cache algorithm
        /// </summary>
        public string ColorCache { get; set; }

        /// <summary>
        /// Max number of colors.
        /// </summary>
        public int MaxColorCount { get; set; }

        /// <summary>
        /// The quantizer algorithm to use.
        /// </summary>
        public string Algorithm { get; set; }

        /// <summary>
        /// Dithering method to use. (If any)
        /// </summary>
        public string DitherAlgorithm { get; set; }

        /// <summary>
        /// Acceptable values: 
        /// "1", 2, 4, 8, 16, 32, 64
        /// Number of parallel processes allowed to be used by the quantizer.
        /// </summary>
        public int MaxParallelProcesses { get; set; } = 1;

        //public bool IsValid(QuantizerAlgorithmOptions opts, bool fixIfPossible = true)
        //{
        //    if (!QuantizerAlgorithm.Values.Contains(Algorithm))
        //        return false;

        //    if (!opts.MaxParallelProcessesList.Contains(MaxParallelProcesses))
        //    {
        //        if (!fixIfPossible) return false;
        //        MaxParallelProcesses = 1;
        //    }

        //    if (null != DitherAlgorithm && !opts.DithererList.ContainsKey(DitherAlgorithm))
        //    {
        //        if (!fixIfPossible) return false;
        //        DitherAlgorithm = "No dithering";
        //    }

        //    if (ColorCache != null && !opts.ColorCacheList.ContainsKey(ColorCache))
        //    {
        //        if (!fixIfPossible) return false;
        //        ColorCache = opts.ColorCacheList.Keys.First();
        //    }

        //    if (!opts.MaxColorCountsList.Contains(MaxColorCount))
        //    {
        //        if (!fixIfPossible) return false;
        //        MaxColorCount = 256;
        //    }

        //    return true;
        //}
    }
}
