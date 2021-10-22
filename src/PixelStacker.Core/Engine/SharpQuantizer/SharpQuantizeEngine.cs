using PixelStacker.Core.Engine.Quantizer.Enums;
using PixelStacker.Core.IO.Config;
using PixelStacker.Core.Model.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Core.Engine.SharpQuantizer
{
    public class SharpQuantizeEngine
    {
        private static Configuration Config = new Configuration(
                new PngConfigurationModule(),
                new JpegConfigurationModule(),
                new GifConfigurationModule(),
                new BmpConfigurationModule(),
                new TgaConfigurationModule(),
                new GifConfigurationModule());

        public static string[] GetQuantizerAlgorithms() => QuantizerAlgorithm.Values;

        private static IQuantizer GetQuantizerByAlgorithmName(string algo) 
        { 
            if (string.IsNullOrEmpty(algo))
                throw new ArgumentNullException(nameof(algo));

            //var werner = new WernerPaletteQuantizer();
            //var webSafe = new WebSafePaletteQuantizer();
            //var octree = new OctreeQuantizer();
            //var wu = new WuQuantizer();
            switch (algo)
            {
                //case QuantizerAlgorithm.HslDistinctSelection:
                //    return new DistinctSelectionQuantizer();
                //case QuantizerAlgorithm.MedianCut:
                //    return new MedianCutQuantizer();
                //case QuantizerAlgorithm.Neural:
                //    return new NeuralColorQuantizer();
                case QuantizerAlgorithm.Octree:
                    return new OctreeQuantizer();
                //case QuantizerAlgorithm.OptimalPalette:
                //    return new OptimalPaletteQuantizer();
                //case QuantizerAlgorithm.Popularity:
                //    return new PopularityQuantizer();
                //case QuantizerAlgorithm.UniformQuantizer:
                //    return new UniformQuantizer();
                case QuantizerAlgorithm.WuColor:
                    return new WuQuantizer();
                default:
                    throw new ArgumentOutOfRangeException(nameof(algo), algo, "Algorithm must be from supported list of values: [" + string.Join(", ", QuantizerAlgorithm.Values) + "]");
            }
        }

        public static QuantizerAlgorithmOptions GetQuantizerAlgorithmOptions(string algo)
        {
            var opts = new QuantizerAlgorithmOptions();
            //IColorQuantizer q = GetQuantizerByAlgorithmName(algo);
            //if (q is BaseColorCacheQuantizer)
            //{
            //    opts.ColorCacheList = new OrderedDictionary<string, IColorCache>
            //    {
            //        { "Euclidean distance", new EuclideanDistanceColorCache() },
            //        { "Locality-sensitive hash", new LshColorCache () },
            //        { "Octree search", new OctreeColorCache() }
            //    };
            //}

            //if (q is not WuColorQuantizer)
            //{
            //    opts.DithererList = new OrderedDictionary<string, IColorDitherer>
            //    {
            //        { "No dithering", null },
            //        { "Bayer dithering (4x4)", new BayerDitherer4() },
            //        { "Bayer dithering (8x8)", new BayerDitherer8() },
            //        { "Clustered dot (4x4)", new ClusteredDotDitherer() },
            //        { "Dot halftoning (8x8)", new DotHalfToneDitherer() },
            //        { "--[ Error diffusion ]--", null },
            //        { "Fan dithering (7x3)", new FanDitherer() },
            //        { "Shiau dithering (5x3)", new ShiauDitherer() },
            //        { "Sierra dithering (5x3)", new SierraDitherer() },
            //        { "Stucki dithering (5x5)", new StuckiDitherer() },
            //        { "Burkes dithering (5x3)", new BurkesDitherer() },
            //        { "Atkinson dithering (5x5)", new AtkinsonDithering() },
            //        { "Two-row Sierra dithering (5x3)", new TwoRowSierraDitherer() },
            //        { "Floyd–Steinberg dithering (3x3)", new FloydSteinbergDitherer() },
            //        { "Jarvis-Judice-Ninke dithering (5x5)", new JarvisJudiceNinkeDitherer() },
            //    };
            //}

            //if (q.AllowParallel)
            //{
            //    opts.MaxParallelProcessesList = new List<int>()
            //    {
            //        1, 2, 4, 8, 16, 32, 64
            //    };
            //}

            //if (!(q is UniformQuantizer || q is NeuralColorQuantizer || q is OptimalPaletteQuantizer))
            //{
            //    opts.MaxColorCountsList = new List<int>()
            //    {
            //        2, 4, 8, 16, 32, 64, 128, 256
            //    };
            //}

            return opts;
        }

        public static PxBitmap RenderImage(CancellationToken? _worker, PxBitmap sourceImage, QuantizerSettings settings, QuantizerAlgorithmOptions opts = null)
        {
            var sharpImage = sourceImage.ToSharpImage();
            return PxBitmap.FromSharpImage(sharpImage);

            ///// prepares quantizer
            //opts ??= GetQuantizerAlgorithmOptions(settings.Algorithm);
            //if (!settings.IsValid(opts, !Constants.IsDevMode))
            //    throw new Exception("Invalid settings. Verify all settings are correct before moving forward!");

            //// tries to retrieve an image based on HSB quantization
            //var activeQuantizer = GetQuantizerByAlgorithmName(settings.Algorithm);

            //using (var quantizer = activeQuantizer.CreatePixelSpecificQuantizer<Rgba32>(Config, new QuantizerOptions() { 
            //    MaxColors = settings.MaxColorCount,
            //}))
            //{
            //    //quantizer.QuantizeFrame(sourceImage);
            //}
            ////if (activeQuantizer is BaseColorCacheQuantizer)
            ////{
            ////    var colorCacheProvider = opts.ColorCacheList[settings.ColorCache];
            ////    ((BaseColorCacheQuantizer)activeQuantizer).ChangeCacheProvider(colorCacheProvider);
            ////}

            ////var activeDitherer = opts.DithererList[settings.DitherAlgorithm];
            ////int parallelTaskCount = activeQuantizer.AllowParallel ? settings.MaxParallelProcesses : 1;
            ////int colorCount = settings.MaxColorCount;
            //return null;

        }
            public void QuantizeImage(PxBitmap image)
        {

            var data = image.ToBytes();
            var img = Image.Load(data);
            var encoder = new PngEncoder()
            {
                BitDepth = PngBitDepth.Bit16,
                TransparentColorMode = PngTransparentColorMode.Clear,
                ColorType = PngColorType.RgbWithAlpha,
                InterlaceMethod = PngInterlaceMode.None,
            };
        }
    }
}
