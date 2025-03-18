using SkiaSharp;
using System;

namespace PixelStacker.Logic.Extensions
{
    public static class PointExtensions
    {
        public static SKPoint ToSKPoint(this System.Drawing.Point a)
        {
            return new SKPoint(a.X, a.Y);
        }

        public static System.Drawing.Point ToPoint(this SKPoint a)
        {
            return new System.Drawing.Point((int)a.X, (int)a.Y);
        }

        public static SkiaSharp.SKRect ToRectangle(this System.Drawing.Point a, System.Drawing.Point b)
        {
            return ToRectangle(a.X, a.Y, b.X, b.Y);
        }

        public static SkiaSharp.SKRect ToRectangle(this SKPoint a, System.Drawing.Point b)
        {
            return ToRectangle(a.X, a.Y, b.X, b.Y);
        }

        public static SkiaSharp.SKRect ToRectangle(this System.Drawing.Point a, SKPoint b)
        {
            return ToRectangle(a.X, a.Y, b.X, b.Y);
        }

        public static SkiaSharp.SKRect ToRectangle(float x1, float y1, float x2, float y2)
        {
            return new SkiaSharp.SKRect()
            {
                Location = new SKPoint(Math.Min(x1, x2), Math.Min(y1, y2)),
                Size = new SKSize(Math.Abs(x2 - x1), Math.Abs(y2 - y1))
            };
        }

        public static SkiaSharp.SKRect ToRectangle(this SKPoint a, SKPoint b)
        {
            return ToRectangle(a.X, a.Y, b.X, b.Y);
        }
    }
}
