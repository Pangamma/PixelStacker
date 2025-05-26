using PixelStacker.Extensions;
using PixelStacker.Logic.Model;
using SkiaSharp;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public class PngPreviewFormatter : IExportFormatter
    {
        public Task<byte[]> ExportAsync(PixelStackerProjectData canvas, CancellationToken? worker = null)
        {
            using SKBitmap bm = new SKBitmap(canvas.Width, canvas.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            bool isv = canvas.IsSideView;
            SKBitmap bmOutput = bm.ToEditStream(worker, (x, y, c) => canvas.CanvasData[x, y].GetAverageColor(isv));

            using var ms = new MemoryStream();
            bool isSuccessfulEncode = bmOutput.Encode(ms, SKEncodedImageFormat.Png, 100);
            ms.Seek(0, SeekOrigin.Begin);
            return Task.FromResult(ms.ToArray());
        }
    }
}
