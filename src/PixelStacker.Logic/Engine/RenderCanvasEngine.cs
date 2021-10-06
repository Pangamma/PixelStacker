using PixelStacker.Extensions;
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
        /// <param name="src"></param>
        /// <param name="settings"></param>
        /// <exception cref="OperationCanceledException"></exception>
        /// <returns></returns>
        public async Task<Bitmap> PreprocessImageAsync(CancellationToken? worker, Bitmap src, CanvasPreprocessorSettings settings)
        {
            // Resize based on max size
            ProgressX.Report(5, "Pre-processing image. Resizing.");
            int mH = Math.Min(settings.MaxHeight ?? src.Height, src.Height);
            int mW = Math.Min(settings.MaxWidth ?? src.Width, src.Width);
            int H = mW < mH ? (mW * src.Height / src.Width) : mH;
            int W = mW < mH ? mH : (mH * src.Width / src.Height);
            var resized = new Bitmap(src, W, H);

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
                return quantized;
            }

            return resized;
        }

        public async Task<RenderedCanvas> RenderCanvasAsync(Bitmap preprocessedImage)
        {
            return null;
        }
    }
}
