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

namespace PixelStacker.Logic.IO.Formatters
{
    public class PngFormatter : IExportFormatter
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

        public Task ExportAsync(string filePath, PixelStackerProjectData canvas, CancellationToken? worker)
        {
            worker ??= CancellationToken.None;
            int? textureSizee = CalculateTextureSize(canvas.Width, canvas.Height);
            if (textureSizee == null) return Task.CompletedTask;
            int texSize = textureSizee.Value;

            int H = canvas.Height * textureSizee.Value;
            int W = canvas.Width * textureSizee.Value;
            using var outputBitmap = new Bitmap(canvas.Width * texSize, canvas.Height * texSize, PixelFormat.Format32bppArgb);

            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                var cd = canvas.CanvasData;

                var aBM = new AsyncBitmapWrapper(outputBitmap);
                //for (int y = 0; y < canvas.Height; y++)
                //{
                Parallel.For(0, canvas.Height, new ParallelOptions()
                {
                    CancellationToken = worker.Value,
                    MaxDegreeOfParallelism = Math.Max(Environment.ProcessorCount / 2, 1)
                }, (int y) =>
                {
                    var bmProxy = aBM.ToBitmap();
                    using Graphics gImg = Graphics.FromImage(bmProxy);
                    gImg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    gImg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    gImg.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                    for (int x = 0; x < canvas.Width; x++)
                    {
                        var mc = cd[x, y];
                        Bitmap bmTileToPaint = mc.GetImage(cd.IsSideView);
                        lock (bmTileToPaint)
                        {
                            gImg.DrawImage(bmTileToPaint, x * texSize, y * texSize, texSize, texSize);
                        }
                    }
                //}
                });

            outputBitmap.Save(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Task.CompletedTask;
        }
    }
}
