using Microsoft.AspNetCore.Http;
using SkiaSharp;

namespace PixelStacker.Web.Net.Utility
{
    public static class FileExtensions
    {
        public static SKBitmap ToSKBitmap(this IFormFile file)
        {
            var stream = file.OpenReadStream();
            var bm = SKBitmap.Decode(stream);
            return bm;
        }
    }
}
