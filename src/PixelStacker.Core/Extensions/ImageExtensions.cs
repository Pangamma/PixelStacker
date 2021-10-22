using PixelStacker.Core.Model.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PixelStacker.Core.Extensions
{
    public static class ImageExtensions
    {
        public static PxBitmap ToPxBitmap(this byte[] bitmapData)
        {
            Image<Rgba32> data = Image.Load(bitmapData);
            PxBitmap pixels = new PxBitmap(new Model.Drawing.PxColor[data.Width, data.Height]);

            for (int x = 0; x < data.Width; x++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    var pixel = data[x, y];
                    pixels[x, y] = Model.Drawing.PxColor.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
                }
            }

            return pixels;
        }
    }
}
