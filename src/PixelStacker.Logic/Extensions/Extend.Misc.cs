using System;
using System.Drawing;

namespace PixelStacker.Extensions
{
    public static partial class Extend
    {
        public static Size CalculateSize(this Point L, Point R)
        {
            int w = Math.Max(L.X, R.X) - Math.Min(L.X, R.X);
            int h = Math.Max(L.Y, R.Y) - Math.Min(L.Y, R.Y);
            Size size = new Size(Math.Max(1, w), Math.Max(1, h));
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

        /// <summary>
        /// Disposes image, and handles nulls gracefully.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static void SwapDispose<T>(this T newT, ref T old) where T : IDisposable
        {
            var tmp = old;
            old = newT;
            if (tmp != null)
                try { tmp.Dispose(); } catch { }
        }
    }
}
