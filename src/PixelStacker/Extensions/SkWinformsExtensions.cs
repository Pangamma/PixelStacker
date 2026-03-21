using SkiaSharp;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace PixelStacker.Extensions
{
    public static class SkWinformsExtensions
    {
        /// <summary>
        /// Tries to parse #FF00AA hex format to a SKColor
        /// </summary>
        /// <param name="needle"></param>
        /// <returns></returns>
        public static SKColor? ToSKColor(this string needle)
        {
            if (needle.StartsWith("#"))
            {
                try
                {
                    int R, G, B;
                    string needleTrim = needle.Trim();

                    if (needleTrim.Length == 7)
                    {
                        R = Convert.ToByte(needleTrim.Substring(1, 2), 16);
                        G = Convert.ToByte(needleTrim.Substring(3, 2), 16);
                        B = Convert.ToByte(needleTrim.Substring(5, 2), 16);
                    }
                    else if (needleTrim.Length == 4)
                    {
                        R = Convert.ToByte(needleTrim.Substring(1, 1) + needleTrim.Substring(1, 1), 16);
                        G = Convert.ToByte(needleTrim.Substring(2, 1) + needleTrim.Substring(2, 1), 16);
                        B = Convert.ToByte(needleTrim.Substring(3, 1) + needleTrim.Substring(3, 1), 16);
                    }
                    else
                    {
                        return null;
                    }

                    SKColor cNeedle = new SKColor((byte)R, (byte)G, (byte)B, (byte)255);
                    return cNeedle;
                }
                catch (Exception) { }
            }

            return null;
        }

        public static SKBitmap BitmapToSKBitmap(this Bitmap bitmap)
        {
            var skInfo = new SKImageInfo(bitmap.Width, bitmap.Height, SKColorType.Bgra8888, SKAlphaType.Premul);
            var skiaBitmap = new SKBitmap(skInfo);

            var bmpData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            try
            {
                // Copy into SKBitmap's own allocated pixel buffer (not a pointer alias).
                // SetPixels() stores a raw pointer — using it with locked memory causes
                // an AccessViolationException after UnlockBits frees the GDI+ memory.
                int byteCount = skiaBitmap.RowBytes * skiaBitmap.Height;
                byte[] pixels = new byte[byteCount];
                Marshal.Copy(bmpData.Scan0, pixels, 0, byteCount);
                Marshal.Copy(pixels, 0, skiaBitmap.GetPixels(), byteCount);
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }

            return skiaBitmap;
        }

        public static Bitmap SKBitmapToBitmap(this SKBitmap bitmap)
        {
            // Ensure the bitmap is in a compatible format for direct copy
            SKBitmap source = bitmap;
            bool needsDispose = false;
            if (bitmap.ColorType != SKColorType.Bgra8888)
            {
                source = bitmap.Copy(SKColorType.Bgra8888);
                needsDispose = true;
            }

            var result = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
            var bmpData = result.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);
            try
            {
                int byteCount = source.RowBytes * source.Height;
                byte[] pixels = new byte[byteCount];
                Marshal.Copy(source.GetPixels(), pixels, 0, byteCount); // SKBitmap → managed byte[]
                Marshal.Copy(pixels, 0, bmpData.Scan0, byteCount);
            }
            finally
            {
                result.UnlockBits(bmpData);
                if (needsDispose) source.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Copies the original image into a NEW image.
        /// If you are done with your original image, you should
        /// dispose it yourself.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static Bitmap Resize(this Bitmap bitmap, int w, int h)
        {
            Bitmap bm = new Bitmap(w, h);
            using Graphics g = Graphics.FromImage(bm);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(bitmap, 0, 0, w, h);
            return bm;
        }
    }
}
