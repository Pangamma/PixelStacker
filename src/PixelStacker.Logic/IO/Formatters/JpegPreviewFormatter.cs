using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Image;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;
using PixelStacker.Extensions;

namespace PixelStacker.Logic.IO.Formatters
{
    public class JpegPreviewFormatter : IExportFormatter
    {
        public async Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            byte[] data = await this.ExportAsync(canvas, worker);
            await File.WriteAllBytesAsync(filePath, data);
        }

        public async Task<byte[]> ExportAsync(PixelStackerProjectData canvas, CancellationToken? worker = null)
        {
            using SKBitmap bm = new SKBitmap(canvas.Width, canvas.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            bool isv = canvas.IsSideView;
            SKBitmap bmOutput = bm.ToEditStream(worker, (x, y, c) => canvas.CanvasData[x, y].GetAverageColor(isv));

            using var ms = new MemoryStream();
            bool isSuccessfulEncode = bmOutput.Encode(ms, SKEncodedImageFormat.Jpeg, 90);
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }
    }
}
