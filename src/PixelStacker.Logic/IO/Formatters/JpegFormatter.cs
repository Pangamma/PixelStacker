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

namespace PixelStacker.Logic.IO.Formatters
{
    public class JpegFormatter : IExportFormatter
    {
        // Calculate texture size so we can handle large images.
        public static int? CalculateTextureSize(int width, int height)
        {
            int safetyMultiplier = 1; // Want to be able to store N of these things in memory
            int calculatedTextureSize = Constants.TextureSize;

            // Still need to multiply by texture size (4 bytes per pixel / 8 bits per byte = 4 bytes)
            int bytesInSrcImage = (width * height * 32 / 8);

            bool isSuccess = false;

            do
            {
                if (width * calculatedTextureSize >= 30000 || height * calculatedTextureSize >= 30000)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
                    continue;
                }

                int totalPixels = (width + 1) * height * calculatedTextureSize * calculatedTextureSize * 4;
                if (totalPixels >= int.MaxValue || totalPixels < 0)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
                    continue;
                }

                try
                {
                    int numMegaBytes = bytesInSrcImage // pixels in base image * bytes per pixel
                        * calculatedTextureSize * calculatedTextureSize // size of texture tile squared 
                        / 1024 / 1024       // convert to MB
                        * safetyMultiplier  // Multiply by safety buffer to plan for a bunch of these layers.
                        ;

                    if (numMegaBytes > 0)
                        using (var memoryCheck = new System.Runtime.MemoryFailPoint(numMegaBytes)) { }

                    isSuccess = true;
                }
                catch (InsufficientMemoryException)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
                }
            } while (isSuccess == false && calculatedTextureSize > 1);

            if (!isSuccess)
            {
                ProgressX.Report(100, Text.Error_ImageTooLarge);
                return null;
            }

            return calculatedTextureSize;
        }

        public async Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            byte[] data = await this.ExportAsync(canvas, worker);
            await File.WriteAllBytesAsync(filePath, data);
        }

        private int CalcMaxTileSize(int w, int h)
        {
            int calculatedTextureSize = Constants.TextureSize;

            do
            {
                if (w * calculatedTextureSize >= 30000 || h * calculatedTextureSize >= 30000)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
                    continue;
                }

                int totalPixels = (w + 1) * h * calculatedTextureSize * calculatedTextureSize * 4;
                if (totalPixels >= int.MaxValue || totalPixels < 0)
                {
                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
                    continue;
                }

                break;
            }
            while (calculatedTextureSize > 1);

            return Math.Max(1, calculatedTextureSize);
        }

        public async Task<byte[]> ExportAsync(PixelStackerProjectData canvas, CancellationToken? worker = null)
        {
            var painter = await RenderedCanvasPainter.Create(worker, new RenderedCanvas()
            {
                CanvasData = canvas.CanvasData,
                IsSideView = canvas.IsSideView,
                MaterialPalette = canvas.MaterialPalette,
                PreprocessedImage = canvas.PreprocessedImage,
                WorldEditOrigin = canvas.WorldEditOrigin,
                IsCustomized = false
            }, 1);

            int tileSize = CalcMaxTileSize(canvas.Width, canvas.Height);
            int vWidth = tileSize * canvas.Width;
            int vHeight = tileSize * canvas.Height;
            
            var pz = PanZoomSettings.CalculateDefaultPanZoomSettings(canvas.Width, canvas.Height, vWidth, vHeight);
            using var bm = new SKBitmap(new SKImageInfo(vWidth, vHeight, SKColorType.Rgba8888, SKAlphaType.Premul));
            var skCanvas = new SKCanvas(bm);
            painter.PaintSurface(skCanvas, new SKSize(vWidth, vHeight), pz, new CanvasViewerSettings() {
                IsShowBorder = true,
            });

            using var ms = new MemoryStream();
            
            bool isSuccessfulEncode = bm.Encode(ms, SKEncodedImageFormat.Jpeg, 90);
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }
    }
}
