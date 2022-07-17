using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PixelStacker.Extensions
{
    /// <summary>
    /// The utility extender class.
    /// </summary>
    public static partial class Extend
    {
        /// <summary>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="normalize">If colors should be put into their buckets of 5 or 15 or or whatever.</param>
        /// <returns></returns>
        public static List<SKColor> GetColorsInImage(this SKBitmap src, int rgbBucketSize = 1)
        {
            List<SKColor> cs = new List<SKColor>();
            src.ToViewStream(null, (int x, int y, SKColor c) =>
            {
                cs.Add(rgbBucketSize == 1 ? c : c.Normalize(rgbBucketSize));
            });

            return cs;
        }

        /// <summary>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="normalize">If colors should be put into their buckets of 5 or 15 or or whatever.</param>
        /// <returns></returns>
        public static SKColor GetAverageColor(this SKBitmap src, int rgbBucketSize = 1)
        {
            long r = 0;
            long g = 0;
            long b = 0;
            long a = 0;
            long total = src.Width * src.Height;

            src.ToViewStream(null, (int x, int y, SKColor c) =>
            {
                Interlocked.Add(ref r, c.Red);
                Interlocked.Add(ref g, c.Green);
                Interlocked.Add(ref b, c.Blue);
                Interlocked.Add(ref a, c.Alpha);
            });

            r /= total;
            g /= total;
            b /= total;
            a /= total;

            if (a > 128)
            {
                a = 255;
            }

            SKColor rt = new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
            return rgbBucketSize == 1 ? rt : rt.Normalize(rgbBucketSize);
        }

        /// <summary>
        /// Returns a NEW image.
        /// Image MUST be 32bppARGB
        /// (int x, int y, Color cOrig, cDest) => { return newColorDest; }
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static SKBitmap ToMergeStream(this SKBitmap origImage, SKBitmap dstImage, CancellationToken? worker, Func<int, int, SKColor, SKColor, SKColor> callback)
        {
            if (origImage.ColorType != SKColorType.Rgba8888)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            if (dstImage.ColorType != SKColorType.Rgba8888)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            var srcImagePixels = origImage.Pixels;
            var dstImagePixels = dstImage.Pixels;

            if (srcImagePixels.Length != dstImagePixels.Length)
            {
                throw new ArgumentException("Image sizes must be the same.");
            }

            var outImage = new SKBitmap(origImage.Width, origImage.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var outImagePixels = outImage.Pixels;

            int w = origImage.Width;
            int h = origImage.Height;
            for (int i = 0; i < srcImagePixels.Length; ++i)
            {
                int x = i % w;
                int y = i / w;

                outImagePixels[i] = callback(x, y, srcImagePixels[i], dstImagePixels[i]);
                if (x == 0)
                {
                    worker?.SafeThrowIfCancellationRequested();
                    if (worker != null) ProgressX.Report(100 * y / h);
                }
            }

            // Do we really need this?
            outImage.Pixels = outImagePixels;
            return outImage;
        }

        /// <summary>
        /// Returns a NEW image.
        /// Image MUST be 32bppARGB
        /// (int x, int y, Color cOrig, cDest) => { return newColorDest; }
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static SKBitmap ToEditStream(this SKBitmap origImage, CancellationToken? worker, Func<int, int, SKColor, SKColor> callback)
        {
            if (origImage.ColorType != SKColorType.Rgba8888)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            var srcImagePixels = origImage.Pixels;

            var outImage = new SKBitmap(origImage.Width, origImage.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var outImagePixels = outImage.Pixels;

            int w = origImage.Width;
            int h = origImage.Height;
            for (int i = 0; i < srcImagePixels.Length; ++i)
            {
                int x = i % w;
                int y = i / w;

                outImagePixels[i] = callback(x, y, srcImagePixels[i]);
                if (x == 0)
                {
                    worker?.SafeThrowIfCancellationRequested();
                    if (worker != null) ProgressX.Report(100 * y / h);
                }
            }

            // Do we really need this?
            outImage.Pixels = outImagePixels;
            return outImage;
        }

        /// <summary>
        /// Image MUST be 32bppARGB
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static void ToViewStream(this SKBitmap origImage, CancellationToken? worker, Action<int, int, SKColor> callback)
        {

            if (origImage.ColorType != SKColorType.Rgba8888)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            SKColor[] bitmapData = origImage.Pixels;

            int oH = origImage.Height;
            int oW = origImage.Width;
            int oL = bitmapData.Length;

            for (int i = 0; i < oL; i ++)
            {
                //Get the color of a pixel
                // On a little-endian machine, the byte order is bb gg rr aa
                SKColor color = bitmapData[i];

                callback(i % oW, i / oW, color);
                if (worker != null)
                {
                    ProgressX.Report(100 * (i / oW) / oH);
                    worker?.SafeThrowIfCancellationRequested();
                }
            }

            return;
        }
    }
}