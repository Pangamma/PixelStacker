using PixelStacker.Resources;
using PixelStacker.Resources.Localization;
using SimplePaletteQuantizer.Quantizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Engine.Quantizer.Enums
{
    public class QuantizerAlgorithm
    {
        public const string HslDistinctSelection = "HSL distinct selection";
        public const string UniformQuantizer = "Uniform quantization";
        public const string Popularity = "Popularity algorithm";
        public const string MedianCut = "Median cut algorithm";
        public const string WuColor = "Wu's color quantizer";
        public const string Octree = "Octree quantization";
        public const string Neural = "Neural quantizer";
        public const string OptimalPalette = "Optimal palette";

        public static readonly string[] Values = new string[] {
            HslDistinctSelection, UniformQuantizer, Popularity, MedianCut, WuColor, Octree, Neural, OptimalPalette
        };
    }
}
