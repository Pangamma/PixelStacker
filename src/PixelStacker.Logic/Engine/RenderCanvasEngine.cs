using PixelStacker.Extensions;
using PixelStacker.IO.Config;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using SimplePaletteQuantizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
            if (F > 1)
            {
                ProgressX.Report(10, $"Pre-processing image. Flattening into RGB buckets of size {F}");
                resized.ToEditStream(worker, (x, y, c) => c.Normalize(F));
            }

            worker.SafeThrowIfCancellationRequested();

            if (settings.QuantizerSettings?.IsEnabled == true)
            {
                ProgressX.Report(10, $"Pre-processing image. Quantizing.");
                var quantized = QuantizerEngine.RenderImage(worker, resized, settings.QuantizerSettings);
                resized.DisposeSafely();
                return Task.FromResult(quantized);
            }

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

            preprocessedImage.ToViewStream(worker, (x, y, c) => {
                canvas.CanvasData[x, y] = mapper.FindBestMatch(c);
            });

            return Task.FromResult(canvas);
        }
    }
}
