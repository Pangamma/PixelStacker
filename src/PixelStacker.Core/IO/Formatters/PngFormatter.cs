//using Newtonsoft.Json;
//using PixelStacker.Core;
//using PixelStacker.Core.IO.Formatters;
//using PixelStacker.Core.Model;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using PixelStacker.Core.Model.Drawing;
//using System.IO;
//using System.IO.Compression;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using PixelStacker.Core.Utilities;
//using SixLabors.ImageSharp.Processing;

//namespace PixelStacker.Logic.IO.Formatters
//{
//    public class PngFormatter : IExportFormatter
//    {
//        // Calculate texture size so we can handle large images.
//        public static int? CalculateTextureSize(int width, int height)
//        {
//            int safetyMultiplier = 1; // Want to be able to store N of these things in memory
//            int calculatedTextureSize = Constants.TextureSize;

//            // Still need to multiply by texture size (4 bytes per pixel / 8 bits per byte = 4 bytes)
//            int bytesInSrcImage = (width * height * 32 / 8);

//            bool isSuccess = false;

//            do
//            {
//                if (width * calculatedTextureSize >= 30000 || height * calculatedTextureSize >= 30000)
//                {
//                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
//                    continue;
//                }

//                int totalPixels = (width + 1) * height * calculatedTextureSize * calculatedTextureSize * 4;
//                if (totalPixels >= int.MaxValue || totalPixels < 0)
//                {
//                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
//                    continue;
//                }

//                try
//                {
//                    int numMegaBytes = bytesInSrcImage // pixels in base image * bytes per pixel
//                        * calculatedTextureSize * calculatedTextureSize // size of texture tile squared 
//                        / 1024 / 1024       // convert to MB
//                        * safetyMultiplier  // Multiply by safety buffer to plan for a bunch of these layers.
//                        ;

//                    if (numMegaBytes > 0)
//                        using (var memoryCheck = new System.Runtime.MemoryFailPoint(numMegaBytes)) { }

//                    isSuccess = true;
//                }
//                catch (InsufficientMemoryException)
//                {
//                    calculatedTextureSize = Math.Max(1, calculatedTextureSize - 2);
//                }
//            } while (isSuccess == false && calculatedTextureSize > 1);

//            if (!isSuccess)
//            {
//                ProgressX.Report(100, "Text.Error_ImageTooLarge");
//                return null;
//            }

//            return calculatedTextureSize;
//        }

//        public Task ExportAsync(string filePath, RenderedCanvas canvas, CancellationToken? worker)
//        {
//            worker ??= CancellationToken.None;
//            int? textureSizee = CalculateTextureSize(canvas.Width, canvas.Height);
//            if (textureSizee == null) return Task.CompletedTask;
//            int texSize = textureSizee.Value;

//            int H = canvas.Height * textureSizee.Value;
//            int W = canvas.Width * textureSizee.Value;
//            using var bm = new Image<Rgba32>(canvas.Width * texSize, canvas.Height * texSize);

//            try
//            {
//                if (File.Exists(filePath))
//                    File.Delete(filePath);

//                var cd = canvas.CanvasData;

//                bm.Mutate(ctx =>
//                {
//                    ctx.BackgroundColor(Color.Transparent);

//                    for (int y = 0; y < canvas.Height; y++)
//                    {
//                        //using Graphics gImg = Graphics.FromImage(bmProxy);
//                        //gImg.InterpolationMode = PixelStacker.Core.Model.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
//                        //gImg.SmoothingMode = PixelStacker.Core.Model.Drawing.Drawing2D.SmoothingMode.None;
//                        //gImg.PixelOffsetMode = PixelStacker.Core.Model.Drawing.Drawing2D.PixelOffsetMode.Half;

//                        for (int x = 0; x < canvas.Width; x++)
//                        {
//                            var mc = cd[x, y];
//                            PxBitmap bmTileToPaint = mc.GetImage(cd.IsSideView);
//                            lock (bmTileToPaint)
//                            {
//                                ctx.DrawImage(bmTileToPaint.ToSharpImage(), new SixLabors.ImageSharp.Point(x * texSize, y * texSize));
//                                //gImg.DrawImage(bmTileToPaint, x * texSize, y * texSize, texSize, texSize);
//                            }
//                        }
//                    };


//                    //int lineCount = 1000;
//                    //for (int i = 0; i < lineCount; i++)
//                    //{
//                    //    // create an array of two points to make the straight line
//                    //    var points = new SixLabors.Primitives.PointF[2];
//                    //    points[0] = new SixLabors.Primitives.PointF(
//                    //        x: (float)(rand.NextDouble() * pictureBox1.Width),
//                    //        y: (float)(rand.NextDouble() * pictureBox1.Height));
//                    //    points[1] = new SixLabors.Primitives.PointF(
//                    //        x: (float)(rand.NextDouble() * pictureBox1.Width),
//                    //        y: (float)(rand.NextDouble() * pictureBox1.Height));

//                    //    // create a pen unique to this line
//                    //    var lineColor = SixLabors.ImageSharp.Color.FromRgba(
//                    //        r: (byte)rand.Next(255),
//                    //        g: (byte)rand.Next(255),
//                    //        b: (byte)rand.Next(255),
//                    //        a: (byte)rand.Next(255));
//                    //    float lineWidth = rand.Next(1, 10);
//                    //    var linePen = new SixLabors.ImageSharp.Processing.Pen(lineColor, lineWidth);

//                    //    // draw the line
//                    //    ctx.DrawLines(linePen, points);
//                    //}

//                });

//                //    for (int y = 0; y < canvas.Height; y++)
//                //{
//                //    //using Graphics gImg = Graphics.FromImage(bmProxy);
//                //    //gImg.InterpolationMode = PixelStacker.Core.Model.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
//                //    //gImg.SmoothingMode = PixelStacker.Core.Model.Drawing.Drawing2D.SmoothingMode.None;
//                //    //gImg.PixelOffsetMode = PixelStacker.Core.Model.Drawing.Drawing2D.PixelOffsetMode.Half;

//                //    for (int x = 0; x < canvas.Width; x++)
//                //    {
//                //        var mc = cd[x, y];
//                //        PxBitmap bmTileToPaint = mc.GetImage(cd.IsSideView);
//                //        bm.
//                //        lock (bmTileToPaint)
//                //        {
//                //            gImg.DrawImage(bmTileToPaint, x * texSize, y * texSize, texSize, texSize);
//                //        }
//                //    }
//                //};

//                bm.Save(filePath);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex);
//            }

//            return Task.CompletedTask;
//        }
//    }
//}
