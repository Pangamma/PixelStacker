using PixelStacker.Extensions;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Engine.Quantizer.ColorCaches;
using PixelStacker.Logic.Engine.Quantizer.ColorCaches.EuclideanDistance;
using PixelStacker.Logic.Engine.Quantizer.ColorCaches.LocalitySensitiveHash;
using PixelStacker.Logic.Engine.Quantizer.ColorCaches.Octree;
using PixelStacker.Logic.Engine.Quantizer.Ditherers;
using PixelStacker.Logic.Engine.Quantizer.Ditherers.ErrorDiffusion;
using PixelStacker.Logic.Engine.Quantizer.Ditherers.Ordered;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.Engine.Quantizer.Quantizers;
using PixelStacker.Logic.Engine.Quantizer.Quantizers.NeuQuant;
using PixelStacker.Logic.Engine.Quantizer.Quantizers.OptimalPalette;
using PixelStacker.Logic.Engine.Quantizer.Quantizers.Uniform;
using PixelStacker.Logic.Engine.Quantizer.Quantizers.XiaolinWu;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Dithering;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PixelStacker.Logic.Engine.Quantizer
{
    public static class QuantizerEngine
    {
        public static string[] GetQuantizerAlgorithms() => QuantizerAlgorithm.Values;

        private static IQuantizer GetQuantizerByAlgorithmName(string algo)
        {
            if (string.IsNullOrEmpty(algo))
                throw new ArgumentNullException(nameof(algo));

            switch (algo)
            {
                case nameof(KnownQuantizers.Werner): return KnownQuantizers.Werner;
                case nameof(KnownQuantizers.Wu): return KnownQuantizers.Wu;
                case nameof(KnownQuantizers.Octree): return KnownQuantizers.Octree;
                case nameof(KnownQuantizers.WebSafe): return KnownQuantizers.WebSafe;
                default:
                    return null;
                    //throw new ArgumentOutOfRangeException(nameof(algo), algo, "Algorithm must be from supported list of values: [" + string.Join(", ", QuantizerAlgorithm.Values) + "]");
            }
        }

        public static QuantizerAlgorithmOptions GetQuantizerAlgorithmOptions(string algo)
        {
            var opts = new QuantizerAlgorithmOptions();
            IQuantizer q = GetQuantizerByAlgorithmName(algo);

            opts.DithererList = new OrderedDictionary<string, IDither>
            {
                { "No dithering", null },
                { "Atkinson", KnownDitherings.Atkinson },
                { "Bayer (2x2)", KnownDitherings.Bayer2x2 },
                { "Bayer (4x4)", KnownDitherings.Bayer4x4 },
                { "Bayer (8x8)", KnownDitherings.Bayer8x8 },
                { "Burks", KnownDitherings.Burks},
                { "Floyd-Steinberg", KnownDitherings.FloydSteinberg },
                { "Jarvis-Judice-Ninke", KnownDitherings.JarvisJudiceNinke },
                { "Ordered (3x3)", KnownDitherings.Ordered3x3 },
                { "Sierra 2", KnownDitherings.Sierra2 },
                { "Sierra 3", KnownDitherings.Sierra3 },
                { "Sierra Lite", KnownDitherings.SierraLite },
                { "Stevenson Arce", KnownDitherings.StevensonArce },
                { "Stucki", KnownDitherings.Stucki }
            };

            opts.MaxParallelProcessesList = new List<int>()
            {
                1, 2, 4, 8, 16, 32, 64
            };

            if ((q is WuQuantizer || q is OctreeQuantizer))
            {
                opts.MaxColorCountsList = new List<int>()
                {
                    2, 4, 8, 16, 32, 64, 128, 256
                };
            }

            return opts;
        }



        /// <summary>
        /// Processes an image prior to be matched up with existing material combinations.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="LIM"></param>
        /// <param name="settings"></param>
        /// <exception cref="OperationCanceledException"></exception>
        /// <returns></returns>
        private static void TransferImage(IndexedImageFrame<Rgba32> result, SKBitmap outImage)
        {
            // Copy quantized info back to result image
            var outImagePixels = outImage.Pixels;

            int w = outImage.Width;
            int h = outImage.Height;

            var paletteSpan = result.Palette.Span;
            int paletteMaxI = paletteSpan.Length - 1;

            // Compare quantized data to original data and make changes as appropriate
            for (int y = 0; y < h; y++)
            {
                ReadOnlySpan<byte> quantizedPixelSpan = result.GetPixelRowSpan(y);

                for (int x = 0; x < w; x++)
                {
                    int i = x + y * w;
                    Rgba32 winningC = paletteSpan[Math.Min(paletteMaxI, quantizedPixelSpan[x])];
                    outImagePixels[i] = new SKColor(winningC.R, winningC.G, winningC.B, winningC.A);
                }
            }

            // Do we really need this?
            outImage.Pixels = outImagePixels;
        }


        [Obsolete("Fix this so we use skia images")]
        /// <summary>
        ///  SOURCE IMAGE SHOULD BE 32bbpARGB
        /// </summary>
        /// <param name="sourceImage"></param>
        /// <exception cref="OperationCanceledException"></exception>
        /// <returns></returns>
        public static SkiaSharp.SKBitmap RenderImage(CancellationToken? _worker, SkiaSharp.SKBitmap sourceImage, QuantizerSettings settings, QuantizerAlgorithmOptions opts = null)
        {
            int w = sourceImage.Width;
            int h = sourceImage.Height;

            // prepares quantizer
            opts ??= GetQuantizerAlgorithmOptions(settings.Algorithm);
            if (!settings.IsValid(opts, !Constants.IsDevMode))
                throw new Exception("Invalid settings. Verify all settings are correct before moving forward!");

            if (!settings.IsEnabled) return sourceImage.Copy();

            // tries to retrieve an image based on HSB quantization
            var activeQuantizer = GetQuantizerByAlgorithmName(settings.Algorithm);
            if (activeQuantizer == null) return sourceImage.Copy();

            IDither activeDitherer = null;
            opts.DithererList.TryGetValue(settings.DitherAlgorithm, out activeDitherer);

            int parallelTaskCount = settings.MaxParallelProcesses; //activeQuantizer.AllowParallel ? settings.MaxParallelProcesses : 1;
            int colorCount = settings.MaxColorCount;

            try
            {
                Configuration config = Configuration.Default;
                var gOpts = config.GetGraphicsOptions();
                gOpts.Antialias = false;
                config.SetGraphicsOptions(gOpts);
                config.MaxDegreeOfParallelism = parallelTaskCount;

                var frameQuantizer = activeQuantizer.CreatePixelSpecificQuantizer<Rgba32>(config, new QuantizerOptions
                {
                    Dither = activeDitherer,
                    MaxColors = colorCount
                });


                // Paint onto the "actual" image.
                using var actualImage = new Image<Rgba32>(config, w, h, Color.Transparent);
                sourceImage.ToViewStream(null, (x, y, c) =>
                {
                    var ic = new Rgba32((byte)c.Red, (byte)c.Green, (byte)c.Blue, (byte)c.Alpha);
                    actualImage[x, y] = ic;
                });

                // Quantize the image
                ImageFrame<Rgba32> frame = actualImage.Frames.RootFrame;
                using IndexedImageFrame<Rgba32> result = frameQuantizer.BuildPaletteAndQuantizeFrame(frame, frame.Bounds());

                var outImage = new SKBitmap(w, h, SKColorType.Rgba8888, SKAlphaType.Premul);
                TransferImage(result, outImage);
                return outImage;

                //// For some reason the super quick algo failed. Need to fail over to this super safe one.
                //using (Image targetImage = ImageBuffer.QuantizeImage(sourceImage, activeQuantizer, activeDitherer, colorCount, parallelTaskCount))
                //{
                //    //using 
                //    Bitmap formattedBM = targetImage.To32bppBitmap();
                //    //var returnVal = sourceImage.ToMergeStream(formattedBM, _worker, (x, y, o, n) =>
                //    //{
                //    //    if (o.A < 32) return Color.Transparent;
                //    //    else return n;
                //    //});

                //    //return returnVal;
                //    return formattedBM;
                //}
            }
            catch (Exception)
            {
                // Throw THIS type if cancellation caused the issue
                _worker.SafeThrowIfCancellationRequested();
                throw; // Throw whatever type was already there if it is something else.
            }
        }
    }
}
