using PixelStacker.Logic.Engine.Quantizer.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Model
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

        public bool IsValid(QuantizerAlgorithmOptions opts, bool fixIfPossible = true)
        {
            if (!QuantizerAlgorithm.Values.Contains(this.Algorithm))
                return false;

            if (!opts.MaxParallelProcessesList.Contains(this.MaxParallelProcesses))
            {
                if (!fixIfPossible) return false;
                this.MaxParallelProcesses = 1;
            }

            if (null != this.DitherAlgorithm && !opts.DithererList.ContainsKey(this.DitherAlgorithm))
            {
                if (!fixIfPossible) return false;
                this.DitherAlgorithm = "No dithering";
            }

            if (!opts.ColorCacheList.ContainsKey(this.ColorCache))
            {
                if (!fixIfPossible) return false;
                this.ColorCache = opts.ColorCacheList.Keys.First();
            }

            if (!opts.MaxColorCountsList.Contains(this.MaxColorCount))
            {
                if (!fixIfPossible) return false;
                this.MaxColorCount = 256;
            }

            return true;
        }
    }
}
