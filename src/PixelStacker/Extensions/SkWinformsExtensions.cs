using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Extensions
{
    public static class SkWinformsExtensions
    {
        public static SKBitmap BitmapToSKBitmap(this Bitmap bitmap)
        {
            var info = new SKImageInfo(bitmap.Width, bitmap.Height);
            var skiaBitmap = new SKBitmap(info);

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    skiaBitmap.SetPixel(i, j, new SKColor(color.R, color.G, color.B, color.A));
                }
            }

            return skiaBitmap;
        }
    }
}
