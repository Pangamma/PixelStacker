using PixelStacker.Logic.CanvasEditor;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using SkiaSharp;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Formatters
{
    public class PngFormatter : IExportImageFormatter
    {

        private int CalcMaxTileSize(int w, int h, int defaultTextureSize)
        {
            int calculatedTextureSize = defaultTextureSize;

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
            => await this.ExportAsync(canvas, new CanvasViewerSettings().ToReadonlyClone(), worker);

        public async Task<byte[]> ExportAsync(PixelStackerProjectData canvas, IReadonlyCanvasViewerSettings srs, CancellationToken? worker = null)
        {
            var painter = await RenderedCanvasPainter.Create(worker, new RenderedCanvas()
            {
                CanvasData = canvas.CanvasData,
                IsSideView = canvas.IsSideView,
                MaterialPalette = canvas.MaterialPalette,
                PreprocessedImage = canvas.PreprocessedImage,
                WorldEditOrigin = canvas.WorldEditOrigin,
                IsCustomized = false
            }, srs, 1);

            int tileSize = CalcMaxTileSize(canvas.Width, canvas.Height, srs.TextureSize);
            int vWidth = tileSize * canvas.Width;
            int vHeight = tileSize * canvas.Height;

            var pz = PanZoomSettings.CalculateDefaultPanZoomSettings(canvas.Width, canvas.Height, vWidth, vHeight);
            using var bm = new SKBitmap(new SKImageInfo(vWidth, vHeight, SKColorType.Rgba8888, SKAlphaType.Premul));
            var skCanvas = new SKCanvas(bm);
            painter.PaintSurface(skCanvas, new SKSize(vWidth, vHeight), pz, new CanvasViewerSettings()
            {
                IsShowBorder = false,
            });

            using var ms = new MemoryStream();
            bool isSuccessfulEncode = bm.Encode(ms, SKEncodedImageFormat.Png, 100);
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }
    }
}
