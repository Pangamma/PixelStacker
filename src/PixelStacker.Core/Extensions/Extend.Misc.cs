namespace PixelStacker.Core.Extensions
{
    public static partial class Extend
    {
        //public static PxSize CalculateSize(this PxPoint L, PxPoint R)
        //{
        //    int w = Math.Max(L.X, R.X) - Math.Min(L.X, R.X);
        //    int h = Math.Max(L.Y, R.Y) - Math.Min(L.Y, R.Y);
        //    PxSize size = new PxSize(Math.Max(1, w), Math.Max(1, h));
        //    return size;
        //}

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
