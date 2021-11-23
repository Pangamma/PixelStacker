using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.CanvasEditor
{
    public partial class RenderedCanvasPainter
    {

        public const int BlocksPerChunk = 38;

        /// Should contain: 
        /// 0 = 1/1 size, when viewing at zoom(tex)+ to zoom(tex * 0.75) 
        /// 1 = 1/2 size
        /// 2 = 1/4 size
        /// 3 = 1/8 size
        private List<SKBitmap[,]> Bitmaps { get; }
        private List<object[,]> Padlocks { get; }

        private static int GetChunkIndexX(int srcX) => srcX / BlocksPerChunk;
        private static int GetChunkIndexY(int srcY) => srcY / BlocksPerChunk;

        /// <summary>
        /// Initialize the bitmaps by rendering a canvas into image tiles.
        /// </summary>
        /// <returns></returns>
        private static async Task<List<SKBitmap[,]>> RenderIntoTilesAsync(CancellationToken? worker, RenderedCanvas data, SpecialCanvasRenderSettings srs, int maxLayers)
        {
            worker ??= CancellationToken.None;

            var sizes = CalculateChunkSizes(data,srs, maxLayers);
            worker.SafeThrowIfCancellationRequested();
            var bitmaps = new List<SKBitmap[,]>();

            int chunksFinishedSoFar = 0;
            int totalChunksToRender = 0;
            foreach (var size in sizes)
            {
                totalChunksToRender += size.Length;
                bitmaps.Add(new SKBitmap[size.GetLength(0), size.GetLength(1)]);
            }

            #region LAYER 0
            {
                SKSize[,] sizeSet = sizes[0];
                int scaleDivide = 1;
                int numChunksWide = sizeSet.GetLength(0);
                int numChunksHigh = sizeSet.GetLength(1);
                int srcPixelsPerChunk = BlocksPerChunk * scaleDivide;
                int dstPixelsPerChunk = Constants.TextureSize * srcPixelsPerChunk / scaleDivide;
                int iTask = 0;
                Task[] L0Tasks = new Task[sizes[0].Length];
                for (int cW = 0; cW < numChunksWide; cW++)
                {
                    for (int cH = 0; cH < numChunksHigh; cH++)
                    {
                        int cWf = cW;
                        int cHf = cH;
                        SKSize tileSize = sizeSet[cW, cH];
                        SKRect srcRect = new SKRect()
                        {
                            Location = new SKPoint(cWf * srcPixelsPerChunk, cHf * srcPixelsPerChunk),
                            Size = new SKSize((float)Math.Floor(tileSize.Width * scaleDivide / Constants.TextureSize)
                            , (float)Math.Floor(tileSize.Height * scaleDivide / Constants.TextureSize))
                        };
                        SKRect dstRect = new SKRect()
                        {
                            Location = new SKPoint(cWf * dstPixelsPerChunk, cHf * dstPixelsPerChunk),
                            Size = new SKSize(tileSize.Width, tileSize.Height)
                        };

                        L0Tasks[iTask++] = Task.Run(() =>
                        {
                            var bmToAdd = CreateLayer0Image(data, srs, srcRect, dstRect);
                            bitmaps[0][cWf, cHf] = bmToAdd;
                            int nVal = Interlocked.Increment(ref chunksFinishedSoFar);
                            ProgressX.Report(100 * nVal / totalChunksToRender);
                        }, worker.Value);
                    }
                }

                await Task.WhenAll(L0Tasks);
            }
            #endregion LAYER 0



            // OTHER LAYERS 2.0
            {
                //float pixelsPerHalfChunk = Constants.TextureSize * BlocksPerChunk / 2;
                //for (int layerIndexToRender = 1; layerIndexToRender < sizes.Count; layerIndexToRender++)
                //{
                //    int scaleDivide = (int)Math.Pow(2, layerIndexToRender);
                //    var curLayer = bitmaps[layerIndexToRender];
                //    var upperLayer = chunksThatNeedReRendering[layerIndexToRender - 1];
                //    for (int xIndexCurrentLayer = 0; xIndexCurrentLayer < curLayer.GetLength(0); xIndexCurrentLayer++)
                //    {
                //        for (int yIndexCurrentLayer = 0; yIndexCurrentLayer < curLayer.GetLength(1); yIndexCurrentLayer++)
                //        {
                //            // No need to re-render this chunk?
                //            if (!curLayer[xIndexCurrentLayer, yIndexCurrentLayer]) continue;

                //            SKBitmap bmToEdit = null;
                //            lock (Padlocks[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer])
                //            {
                //                bmToEdit = Bitmaps[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer].Copy();
                //            }

                //            using SKCanvas g = new SKCanvas(bmToEdit);
                //            //g.DrawRect(0, 0, bmToEdit.Width, bmToEdit.Height, new SKPaint() { Color = new SKColor(255, 0, 0, 255) });

                //            int xUpper = xIndexCurrentLayer * 2;
                //            int yUpper = yIndexCurrentLayer * 2;

                //            // TL
                //            if (upperLayer[xIndexCurrentLayer * 2, yIndexCurrentLayer * 2])
                //            {
                //                SKBitmap bmToCopy = Bitmaps[layerIndexToRender - 1][xUpper, yUpper];
                //                var rect = new SKRect()
                //                {
                //                    Location = new SKPoint(0, 0),
                //                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                //                };

                //                g.DrawBitmap(bmToCopy, rect, paint);
                //            }

                //            // TR
                //            if (upperLayer.GetLength(0) > xUpper + 1
                //                && upperLayer.GetLength(1) > yUpper
                //                && upperLayer[xUpper + 1, yUpper])
                //            {
                //                SKBitmap bmToCopy = Bitmaps[layerIndexToRender - 1][xUpper + 1, yUpper];
                //                var rect = new SKRect()
                //                {
                //                    Location = new SKPoint(pixelsPerHalfChunk, 0),
                //                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                //                };

                //                g.DrawBitmap(bmToCopy, rect, paint);
                //            }

                //            // BL
                //            if (upperLayer.GetLength(0) > xUpper
                //                && upperLayer.GetLength(1) > yUpper + 1
                //                && upperLayer[xUpper, yUpper + 1])
                //            {
                //                SKBitmap bmToCopy = Bitmaps[layerIndexToRender - 1][xUpper, yUpper + 1];
                //                var rect = new SKRect()
                //                {
                //                    Location = new SKPoint(0, pixelsPerHalfChunk),
                //                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                //                };

                //                g.DrawBitmap(bmToCopy, rect, paint);
                //            }

                //            // BR
                //            if (upperLayer.GetLength(0) > xUpper + 1
                //                && upperLayer.GetLength(1) > yUpper + 1
                //                && upperLayer[xUpper + 1, yUpper + 1])
                //            {
                //                SKBitmap bmToCopy = Bitmaps[layerIndexToRender - 1][xUpper + 1, yUpper + 1];
                //                var rect = new SKRect()
                //                {
                //                    Location = new SKPoint(pixelsPerHalfChunk, pixelsPerHalfChunk),
                //                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                //                };

                //                g.DrawBitmap(bmToCopy, rect, paint);
                //            }
                //            //// Let's talk it out.
                //            //// L0 is your base set of chunks. 
                //            //// L1 has half as many chunks as L0.
                //            //// given L1[x,y] which x,y coordinates would be from L0[x,y]?
                //            //DrawTileOnLargerTile(layerIndexToRender, upperLayer, g, xUpper, yUpper, 0F, 0F, paint);
                //            //DrawTileOnLargerTile(layerIndexToRender, upperLayer, g, xu, yu + 1, 0F, pixelsPerHalfChunk, paint);
                //            //DrawTileOnLargerTile(layerIndexToRender, upperLayer, g, xu + 1, yu, pixelsPerHalfChunk, 0F, paint);
                //            //DrawTileOnLargerTile(layerIndexToRender, upperLayer, g, xu + 1, yu + 1, pixelsPerHalfChunk, pixelsPerHalfChunk, paint);

                //            lock (Padlocks[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer])
                //            {
                //                var tmp = Bitmaps[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer];
                //                Bitmaps[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer] = bmToEdit;
                //                tmp.DisposeSafely();
                //            }
                //        }
                //    }
                }
                
                #region OTHER LAYERS
                {

                var paint = new SKPaint()
                {
                    BlendMode = SKBlendMode.Src,
                    FilterQuality = SKFilterQuality.High,
                    IsAntialias = false
                };

                float pixelsPerHalfChunk = Constants.TextureSize * BlocksPerChunk / 2;
                
                for (int l = 1; l < sizes.Count; l++)
                {
                    SKSize[,] sizeSet = sizes[l];
                    int scaleDivide = (int)Math.Pow(2, l);
                    int numChunksWide = sizeSet.GetLength(0);
                    int numChunksHigh = sizeSet.GetLength(1);
                    int srcPixelsPerChunk = BlocksPerChunk * scaleDivide;
                    int dstPixelsPerChunk = Constants.TextureSize * srcPixelsPerChunk / scaleDivide;
                    int ssWidth = sizeSet.GetLength(0);
                    int ssHeight = sizeSet.GetLength(1);
                    var upperLayer = bitmaps[l - 1];
                    for (int x = 0; x < ssWidth; x++)
                    {
                        for (int y = 0; y < ssHeight; y++)
                        {
                            SKSize dstSize = sizeSet[x, y];

                            var bm = new SKBitmap((int)dstSize.Width, (int)dstSize.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
                            using SKCanvas g = new SKCanvas(bm);

                            int xUpper = x * 2;
                            int yUpper = y * 2;

                            // TL
                            //if (upperLayer[x * 2, y * 2])
                            {
                                SKBitmap bmToCopy = upperLayer[xUpper, yUpper];
                                var rect = new SKRect()
                                {
                                    Location = new SKPoint(0, 0),
                                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                };

                                g.DrawBitmap(bmToCopy, rect, paint);
                            }

                            // TR
                            if (upperLayer.GetLength(0) > xUpper + 1
                                && upperLayer.GetLength(1) > yUpper
                            //    && upperLayer[xUpper + 1, yUpper]
                                )
                            {
                                SKBitmap bmToCopy = bitmaps[l - 1][xUpper + 1, yUpper];
                                var rect = new SKRect()
                                {
                                    Location = new SKPoint(pixelsPerHalfChunk, 0),
                                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                };

                                g.DrawBitmap(bmToCopy, rect, paint);
                            }

                            // BL
                            if (upperLayer.GetLength(0) > xUpper
                                && upperLayer.GetLength(1) > yUpper + 1
                            //    && upperLayer[xUpper, yUpper + 1]
                                )
                            {
                                SKBitmap bmToCopy = bitmaps[l - 1][xUpper, yUpper + 1];
                                var rect = new SKRect()
                                {
                                    Location = new SKPoint(0, pixelsPerHalfChunk),
                                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                };

                                g.DrawBitmap(bmToCopy, rect, paint);
                            }

                            // BR
                            if (upperLayer.GetLength(0) > xUpper + 1
                                && upperLayer.GetLength(1) > yUpper + 1
                                //&& upperLayer[xUpper + 1, yUpper + 1]
                                )
                            {
                                SKBitmap bmToCopy = bitmaps[l - 1][xUpper + 1, yUpper + 1];
                                var rect = new SKRect()
                                {
                                    Location = new SKPoint(pixelsPerHalfChunk, pixelsPerHalfChunk),
                                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                };

                                g.DrawBitmap(bmToCopy, rect, paint);
                            }


                            ////g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            ////g.InterpolationMode = InterpolationMode.NearestNeighbor;
                            ////g.SmoothingMode = SmoothingMode.None;
                            ////g.CompositingMode = CompositingMode.SourceOver;

                            //// tiles within the chunk. We iterate over the main src image to get our content for our chunk data.
                            //for (int xWithinDownsizedChunk = 0; xWithinDownsizedChunk < scaleDivide; xWithinDownsizedChunk++)
                            //{
                            //    for (int yWithinDownsizedChunk = 0; yWithinDownsizedChunk < scaleDivide; yWithinDownsizedChunk++)
                            //    {
                            //        int xIndexOIfL0Chunk = xWithinDownsizedChunk + scaleDivide * x;
                            //        int yIndexOfL0Chunk = yWithinDownsizedChunk + scaleDivide * y;
                            //        if (xIndexOIfL0Chunk > bitmaps[0].GetLength(0) - 1 || yIndexOfL0Chunk > bitmaps[0].GetLength(1) - 1)
                            //            continue;

                            //        var bmToPaint = bitmaps[0][xIndexOIfL0Chunk, yIndexOfL0Chunk];
                            //        var rect = new SKRect()
                            //        {
                            //            Location = new SKPoint((float)(xWithinDownsizedChunk * dstPixelsPerChunk / scaleDivide),
                            //                (float)(yWithinDownsizedChunk * dstPixelsPerChunk / scaleDivide)),
                            //            Size = new SKSize(dstPixelsPerChunk / scaleDivide, dstPixelsPerChunk / scaleDivide)
                            //        };

                            //        g.DrawBitmap(
                            //            bmToPaint,
                            //            rect);
                            //    }
                            //}





                            bitmaps[l][x, y] = bm;
                            ProgressX.Report(100 * ++chunksFinishedSoFar / totalChunksToRender);
                        }
                    }
                }
            }
            #endregion OTHER LAYERS

            return bitmaps;
        }

        private static SKSize[,] CalculateChunkSizesForLayer(SKSize srcImageSize, int scaleDivide)
        {
            int srcW = (int)srcImageSize.Width;
            int srcH = (int)srcImageSize.Height;
            int srcPixelsPerChunk = BlocksPerChunk * scaleDivide;
            int dstPixelsPerChunk = Constants.TextureSize * srcPixelsPerChunk / scaleDivide; // 16 * (RenderedCanvasPainter.BlocksPerChunk * N) / N = 6RenderedCanvasPainter.BlocksPerChunk
            int numChunksWide = (int)srcW / srcPixelsPerChunk + (srcW % srcPixelsPerChunk == 0 ? 0 : 1);
            int numChunksHigh = (int)srcH / srcPixelsPerChunk + (srcH % srcPixelsPerChunk == 0 ? 0 : 1);
            var sizeSet = new SKSize[numChunksWide, numChunksHigh];

            // MAX PERFECT WIDTH - ACTUAL WIDTH = difference
            int deltaX = numChunksWide * dstPixelsPerChunk - Constants.TextureSize * srcW / scaleDivide;
            int deltaY = numChunksHigh * dstPixelsPerChunk - Constants.TextureSize * srcH / scaleDivide;
            for (int x = 0; x < numChunksWide; x++)
            {
                int dstWidthOfChunk = x < numChunksWide - 1
                    ? dstPixelsPerChunk // Very simple. We know if it isn't on the tail we can assume a standard full width.
                    : dstPixelsPerChunk - deltaX;
                for (int y = 0; y < numChunksHigh; y++)
                {
                    int dstHeightOfChunk = y < numChunksHigh - 1
                        ? dstPixelsPerChunk // Very simple. We know if it isn't on the tail we can assume a standard full width.
                        : dstPixelsPerChunk - deltaY;

                    sizeSet[x, y] = new SKSize(width: dstWidthOfChunk, height: dstHeightOfChunk);
                }
            }
            return sizeSet;
        }

        private static List<SKSize[,]> CalculateChunkSizes(RenderedCanvas data, SpecialCanvasRenderSettings srs, int maxLayers)
        {
            int scaleDivide = 1;
            List<SKSize[,]> sizesList = new List<SKSize[,]>();
            SKSize[,] curSizeSet;

            bool a = false;
            bool b = false;
            bool c = false;
            // JUST the pixels in a dest chunk no matter what scale it is at.
            int pixelsPerChunkTile = BlocksPerChunk * Constants.TextureSize;

            // We run into integer overflows when doing pixelsPerChunk^2. So we use algebra to say
            // W*H*PPC*PPC > MAX_AREA is the same as W*W*PPC > MAX_AREA / PPC
            int MAX_AREA_B4_SPLIT_ADJUSTED = Constants.BIG_IMG_MAX_AREA_B4_SPLIT / pixelsPerChunkTile;
            do
            {
                curSizeSet = CalculateChunkSizesForLayer(new SKSize(data.Width, data.Height), scaleDivide);
                sizesList.Add(curSizeSet);
                scaleDivide *= 2;
                maxLayers--;

                // Do not split if one dimension is unable to be split further.
                a = curSizeSet.GetLength(0) > 2 && curSizeSet.GetLength(1) > 2;

                long bb = (curSizeSet.GetLength(0) * curSizeSet.GetLength(1)) * pixelsPerChunkTile;
                b = MAX_AREA_B4_SPLIT_ADJUSTED < bb;

                // Do not go on forever
                c = maxLayers > 0;
            } while (a && b && c);

            return sizesList;
        }

        private static SKBitmap CreateLayer0Image(RenderedCanvas data, SpecialCanvasRenderSettings srs, SKRect srcTile, SKRect dstTile)
        {
            var bm = new SKBitmap((int)dstTile.Width, (int)dstTile.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            int scaleDivide = (int)(dstTile.Width / srcTile.Width);

            int srcWidth = (int)srcTile.Width;
            int srcHeight = (int)srcTile.Height;
            var canvas = new SKCanvas(bm);
            var paint = new SKPaint()
            {
                BlendMode = SKBlendMode.Src,
                FilterQuality = SKFilterQuality.High,
                IsAntialias = false
            };


            if (srs.IsSolidColors)
            {
                Parallel.For(0, srcHeight, (y) =>
                {
                    var paintSolid = new SKPaint()
                    {
                        BlendMode = SKBlendMode.Src,
                        FilterQuality = SKFilterQuality.High,
                        IsAntialias = false,
                        IsStroke = false, // FILL
                    };

                    for (int x = 0; x < srcWidth; x++)
                    {
                        var loc = srcTile.Location;
                        var mc = data.CanvasData[(int)loc.X + x, (int)loc.Y + y];

                        SKColor toPaint = mc.GetAverageColor(data.IsSideView, srs);

                        paintSolid.Color = toPaint;
                        canvas.DrawRect(new SKRect() { 
                            Location = new SKPoint(x * Constants.TextureSize, y * Constants.TextureSize),
                            Size = new SKSize(Constants.TextureSize, Constants.TextureSize) 
                       }, paintSolid); 
                    }
                });
            }
            else
            {
                Parallel.For(0, srcHeight, (y) =>
                {
                    for (int x = 0; x < srcWidth; x++)
                    {
                        var loc = srcTile.Location;
                        var mc = data.CanvasData[(int)loc.X + x, (int)loc.Y + y];

                        SKBitmap toPaint;
                        if (srs.ZLayerFilter == 0) toPaint = mc.Bottom.GetImage(data.IsSideView);
                        else if (srs.ZLayerFilter == 1) toPaint = mc.Top.GetImage(data.IsSideView);
                        else toPaint = mc.GetImage(data.IsSideView);

                        canvas.DrawBitmap(toPaint, new SKPoint(x * Constants.TextureSize, y * Constants.TextureSize), paint);
                    }
                });
            }

            return bm;
        }
    }
}
