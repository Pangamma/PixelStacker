using PixelStacker.Extensions;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine.Quantizer;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

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

        /// <summary>
        /// Processes an image prior to be matched up with existing material combinations.
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="LIM"></param>
        /// <param name="settings"></param>
        /// <exception cref="OperationCanceledException"></exception>
        /// <returns></returns>
        public Task<Bitmap> PreprocessImageAsync(CancellationToken? worker, Bitmap LIM, CanvasPreprocessorSettings settings)
        {
            // Resize based on max size
            ProgressX.Report(5, "Pre-processing image. Resizing.");
            int mH = Math.Min(settings.MaxHeight ?? LIM.Height, LIM.Height);
            int mW = Math.Min(settings.MaxWidth ?? LIM.Width, LIM.Width);
            int H = (mW < mH) ? (mW * LIM.Height / LIM.Width) : mH;
            int W = (mW < mH) ? mW : (mH * LIM.Width / LIM.Height);
            var resized = new Bitmap(LIM, W, H);

            // Color bucket normalization
            int F = settings.RgbBucketSize;
            ProgressX.Report(25, $"Pre-processing image. Flattening into RGB buckets of size {F}");
            if (F > 1)
            {
                resized.ToEditStream(worker, (x, y, c) => c.Normalize(F));
            }

            worker.SafeThrowIfCancellationRequested();

            if (settings.QuantizerSettings?.IsEnabled == true)
            {
                ProgressX.Report(50, $"Pre-processing image. Quantizing.");
                var quantized = QuantizerEngine.RenderImage(worker, resized, settings.QuantizerSettings);
                resized.DisposeSafely();
                ProgressX.Report(100, "Finished pre-processing the image.");
                return Task.FromResult(quantized);
            }

            ProgressX.Report(100, "Finished pre-processing the image.");
            return Task.FromResult(resized);
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
            ref Bitmap preprocessedImage,
            IColorMapper mapper,
            MaterialPalette palette
            )
        {
            RenderedCanvas canvas = new RenderedCanvas()
            {
                WorldEditOrigin = new Point(0, preprocessedImage.Height - 1),
                IsCustomized = false,
                PreprocessedImage = preprocessedImage,
                MaterialPalette = palette,
                CanvasData = new CanvasData(palette, new int[preprocessedImage.Width, preprocessedImage.Height])
            };
            ProgressX.Report(0, Resources.Text.RenderEngine_ConvertingToBlocks);
            preprocessedImage.ToViewStream(worker, (x, y, c) => {
                canvas.CanvasData[x, y] = mapper.FindBestMatch(c);
            });

            return Task.FromResult(canvas);
        }
    }
}
