using SkiaSharp;
using System;

namespace PixelStacker.Extensions
{
    public static partial class Extend
    {
        public static SKSize CalculateSize(this SKPoint L, SKPoint R)
        {
            float w = Math.Max(L.X, R.X) - Math.Min(L.X, R.X);
            float h = Math.Max(L.Y, R.Y) - Math.Min(L.Y, R.Y);
            SKSize size = new SKSize(Math.Max(1, w), Math.Max(1, h));
            return size;
        }

        /// <summary>
        /// Disposes image, and handles nulls gracefully.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static void DisposeSafely(this IDisposable src)
        {
            if (src == null) return;
            try { src.Dispose(); } catch { }
        }
    }
}
