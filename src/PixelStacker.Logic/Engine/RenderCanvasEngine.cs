using PixelStacker.Extensions;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using PixelStacker.Logic.Engine.Quantizer;

namespace PixelStacker.Logic.Engine
{

    /// <summary>
    /// 1. Preprocess the image
    ///     a. Resize
    ///     b. Flatten colorspace
    ///     c. Quantize
    /// 2. Convert to "RenderedCanvas" or blueprint format.
    /// 3. Render the blueprint to screen so it can be viewed.
    /// </summary>
    public class RenderCanvasEngine
    {
        /// <summary>
        /// If NULL, it means image is too large.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="safetyMultiplier"></param>
        /// <returns></returns>
        public static int? CalculateTextureSize(int width, int height, int safetyMultiplier = 2)
        {
            int calculatedTextureSize = Constants.TextureSize;
            int bytesInSrcImage = (width * height * 32 / 8); // Still need to multiply by texture size (4 bytes per pixel / 8 bits per byte = 4 bytes)

            bool isSuccess = false;

            do
            {
                if (width * calculatedTextureSize >= 30000 || height * calculatedTextureSize >= 30000)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 1);
                    continue;
                }

                int totalPixels = (width + 1) * height * calculatedTextureSize * calculatedTextureSize * 4;
                if (totalPixels >= int.MaxValue || totalPixels < 0)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 1);
                    continue;
                }

                try
                {
                    int numMegaBytes = bytesInSrcImage // pixels in base image * bytes per pixel
                        * calculatedTextureSize * calculatedTextureSize // size of texture tile squared 
                        / 1024 / 1024       // convert to MB
                        * safetyMultiplier  // Multiply by safety buffer to plan for a bunch of these layers.
                        ;

                    if (numMegaBytes > 0)
                    {
                        using (var memoryCheck = new System.Runtime.MemoryFailPoint(numMegaBytes))
                        {
                        }
                    }

                    isSuccess = true;
                }
                catch (InsufficientMemoryException)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
                }
            } while (isSuccess == false && calculatedTextureSize > 1);

            if (!isSuccess) return null;
            else return calculatedTextureSize;
        }

        [Obsolete("TODO: How to get nearest neighboor sampling selected?", false)]
        /// <summary>
        /// Processes an image prior to be matched up with existing material combinations.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="LIM"></param>
        /// <param name="settings"></param>
        /// <exception cref="OperationCanceledException"></exception>
        /// <returns></returns>
        public Task<SkiaSharp.SKBitmap> PreprocessImageAsync(CancellationToken? worker, SkiaSharp.SKBitmap LIM, CanvasPreprocessorSettings settings)
        {
            var dims = LIM.Info.Rect;
            // Resize based on max size
            ProgressX.Report(5, "Pre-processing image. Resizing.");
            int mH = Math.Min(settings.MaxHeight ?? dims.Height, dims.Height);
            int mW = Math.Min(settings.MaxWidth ?? dims.Width, dims.Width);
            int H = (mW < mH) ? (mW * dims.Height / dims.Width) : mH;
            int W = (mW < mH) ? mW : (mH * dims.Width / dims.Height);
            // TODO: How to get "nearest neighboor" sampling selected?
            var resized = LIM.Resize(new SkiaSharp.SKImageInfo(W, H, SkiaSharp.SKColorType.Rgba8888), SkiaSharp.SKFilterQuality.Low);

            // Color bucket normalization
            int F = settings.RgbBucketSize;
            ProgressX.Report(25, $"Pre-processing image. Flattening into RGB buckets of size {F}");
            if (F > 1)
            {
                var tmp = resized.ToEditStream(worker, (x, y, c) => c.Normalize(F));
                resized.DisposeSafely();
                resized = tmp;
            }

            worker.SafeThrowIfCancellationRequested();
            if (settings.QuantizerSettings?.IsEnabled == true)
            {
                ProgressX.Report(50, $"Pre-processing image. Quantizing.");
                var quantized = QuantizerEngine.RenderImage(worker, resized, settings.QuantizerSettings);
                //var quantized = this.QuantizeImage(worker, resized, settings);

                resized.DisposeSafely();
                ProgressX.Report(100, "Finished pre-processing the image.");
                return Task.FromResult(quantized);
            }

            ProgressX.Report(100, "Finished pre-processing the image.");
            return Task.FromResult(resized);
        }


        /// <summary>
        /// Processes an image prior to be matched up with existing material combinations.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="LIM"></param>
        /// <param name="settings"></param>
        /// <exception cref="OperationCanceledException"></exception>
        /// <returns></returns>
        public SKBitmap QuantizeImage(CancellationToken? worker, SKBitmap origImage, CanvasPreprocessorSettings settings)
        {
            Configuration config = Configuration.Default;
            var gOpts = config.GetGraphicsOptions();
            gOpts.Antialias = false;
            config.SetGraphicsOptions(gOpts);

            var frameQuantizer = KnownQuantizers.Octree.CreatePixelSpecificQuantizer<Rgba32>(config, new QuantizerOptions
            {
                Dither = KnownDitherings.Bayer8x8,
                MaxColors = settings.QuantizerSettings.MaxColorCount
            });

            // Paint onto the "actual" image.
            using var actualImage = new Image<Rgba32>(config, origImage.Width, origImage.Height, Color.Transparent);
            Dictionary<Rgba32, MaterialCombination> map = new Dictionary<Rgba32, MaterialCombination>();
            origImage.ToViewStream(null, (x, y, c) => {
                var ic = new Rgba32((byte)c.Red, (byte)c.Green, (byte)c.Blue, (byte)c.Alpha);
                actualImage[x, y] = ic;
            });

            // Quantize the image
            ImageFrame<Rgba32> frame = actualImage.Frames.RootFrame;
            using IndexedImageFrame<Rgba32> result = frameQuantizer.BuildPaletteAndQuantizeFrame(frame, frame.Bounds());

            // Copy quantized info back to result image
            var srcImagePixels = origImage.Pixels;
            var outImage = new SKBitmap(origImage.Width, origImage.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var outImagePixels = outImage.Pixels;

            int w = origImage.Width;
            int h = origImage.Height;

            var paletteSpan = result.Palette.Span;
            int paletteMaxI = paletteSpan.Length - 1;

            // Compare quantized data to original data and make changes as appropriate
            for (int y = 0; y < h; y++)
            {
                Span<Rgba32> row = actualImage.GetPixelRowSpan(y);
                ReadOnlySpan<byte> quantizedPixelSpan = result.GetPixelRowSpan(y);

                for (int x = 0; x < w; x++)
                {
                    int i = x + y * w;
                    Rgba32 winningC = paletteSpan[Math.Min(paletteMaxI, quantizedPixelSpan[x])];
                    outImagePixels[i] = new SKColor(winningC.R, winningC.G, winningC.B, winningC.A);
                    row[x] = winningC;
                }

                worker?.SafeThrowIfCancellationRequested();
                if (worker != null) ProgressX.Report(100 * y / h);
            }

            // Do we really need this?
            outImage.Pixels = outImagePixels;
            return outImage;
        }

        /// <summary>
        /// Processes an image prior to be matched up with existing material combinations.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="LIM"></param>
        /// <param name="settings"></param>
        /// <exception cref="OperationCanceledException"></exception>
        /// <returns></returns>
        public Task<RenderedCanvas> PostprocessImageAsync(CancellationToken? worker, RenderedCanvas canvas, IColorMapper cMapper, CanvasPreprocessorSettings settings)
        {
            Configuration config = Configuration.Default;
            var gOpts = config.GetGraphicsOptions();
            gOpts.Antialias = false;
            config.SetGraphicsOptions(gOpts);

            var quantizer = new WuQuantizer(new QuantizerOptions
            {
                Dither = null,
                MaxColors = settings.QuantizerSettings.MaxColorCount
            });

            using IQuantizer<Rgba32> frameQuantizer = quantizer.CreatePixelSpecificQuantizer<Rgba32>(config);
            using var actualImage = new Image<Rgba32>(config, canvas.Width, canvas.Height, Color.Transparent);

            Dictionary<Rgba32, MaterialCombination> map = new Dictionary<Rgba32, MaterialCombination>();

            // Paint onto the "actual" image.
            for (int y = 0; y < actualImage.Height; y++)
            {
                for (int x = 0; x < actualImage.Width; x++)
                {
                    var mc = canvas.CanvasData[x, y];
                    var c = mc.GetAverageColor(canvas.IsSideView);
                    var ic = new Rgba32((byte)c.Red, (byte)c.Green, (byte)c.Blue, (byte)c.Alpha);
                    map.TryAdd(ic, mc);
                    if (!map.TryGetValue(ic, out MaterialCombination mc2))
                    {
                        Console.WriteLine("Bruh.");
                    }
                    actualImage[x, y] = ic;
                }
            }

            // Quantize the image
            ImageFrame<Rgba32> frame = actualImage.Frames.RootFrame;
            using IndexedImageFrame<Rgba32> result = frameQuantizer.BuildPaletteAndQuantizeFrame(frame, frame.Bounds());
            var paletteSpan = result.Palette.Span;
            int paletteMaxI = paletteSpan.Length - 1;

            // Compare quantized data to original data and make changes as appropriate
            for (int y = 0; y < canvas.Height; y++)
            {
                Span<Rgba32> row = actualImage.GetPixelRowSpan(y);
                ReadOnlySpan<byte> quantizedPixelSpan = result.GetPixelRowSpan(y);

                for (int x = 0; x < actualImage.Width; x++)
                {
                    Rgba32 winningC = paletteSpan[Math.Min(paletteMaxI, quantizedPixelSpan[x])];
                    row[x] = winningC;

                    if (!map.TryGetValue(winningC, out MaterialCombination winningMC))
                    {
                        winningMC = cMapper.FindBestMatch(new SKColor(winningC.R, winningC.G, winningC.B, winningC.A));
                    }

                    canvas.CanvasData[x, y] = winningMC;
                }
            }

            return Task.FromResult(canvas);
        }

        /// <summary>
        /// </summary>
        /// <param name="worker">A canceller token</param>
        /// <param name="preprocessedImage">Image to be used as the reference image later on.</param>
        /// <param name="mapper">Should be pre-loaded with the enabled material combos.</param>
        /// <param name="palette">The master lookup table for color palettes</param>
        /// <returns></returns>
        public Task<RenderedCanvas> RenderCanvasAsync(
            CancellationToken? worker,
            SKBitmap preprocessedImage,
            IColorMapper mapper,
            MaterialPalette palette
            )
        {
            preprocessedImage = preprocessedImage.Copy();
            RenderedCanvas canvas = new RenderedCanvas()
            {
                WorldEditOrigin = new PxPoint(0, preprocessedImage.Height - 1),
                IsCustomized = false,
                PreprocessedImage = preprocessedImage,
                MaterialPalette = palette,
                CanvasData = new CanvasData(palette, new int[preprocessedImage.Width, preprocessedImage.Height])
            };
            ProgressX.Report(0, Resources.Text.RenderEngine_ConvertingToBlocks);
            preprocessedImage.ToViewStream(worker, (x, y, c) =>
            {
                var mcId = mapper.FindBestMatch(c);
                canvas.CanvasData[x, y] = mcId;
            });

            return Task.FromResult(canvas);
        }
    }
}
