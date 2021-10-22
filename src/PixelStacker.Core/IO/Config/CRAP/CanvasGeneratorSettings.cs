using PixelStacker.Core.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Core.IO.Config.NEW
{
    public class CanvasGeneratorSettings
    {
        public int? MaxWidth { get; set; }
        public int? MaxHeight { get; set; }

        /// <summary>
        /// R/5, G/5, B/5
        /// The value all RGB values should be divided by to achieve awesome truncating.
        /// Basically, 251 would become 250. This is used by the Color cache size
        /// settings dropdown in the pre-render/quantizer options menu.
        /// Valid values: 1, 5, 15, 17, 51
        /// </summary>
        public int RgbBucketSize { get; set; } = 1;

        public QuantizerSettings QuantizerSettings { get; set; } = new QuantizerSettings()
        {
            IsEnabled = false,
            //Algorithm = QuantizerAlgorithm.HslDistinctSelection,
            ColorCache = "Octree search",
            MaxColorCount = 256,
            DitherAlgorithm = "No dithering",
            MaxParallelProcesses = 1
        };
    }
}
