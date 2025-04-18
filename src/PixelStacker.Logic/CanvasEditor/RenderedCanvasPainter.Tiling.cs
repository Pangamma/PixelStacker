using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.CanvasEditor
{
    public partial class RenderedCanvasPainter
    {

        /// Should contain: 
        /// 0 = 1/1 size, when viewing at zoom(tex)+ to zoom(tex * 0.75) 
        /// 1 = 1/2 size
        /// 2 = 1/4 size
        /// 3 = 1/8 size
        private List<SKImage[,]> Tiles { get; }
        private List<object[,]> Padlocks { get; }

        private static int GetChunkIndexX(int srcX, int BlocksPerChunk) => srcX / BlocksPerChunk;
        private static int GetChunkIndexY(int srcY, int BlocksPerChunk) => srcY / BlocksPerChunk;

        /// <summary>
        /// Initialize the bitmaps by rendering a canvas into image tiles.
        /// </summary>
        /// <returns></returns>
        private static async Task<List<SKImage[,]>> RenderIntoTilesAsync(CancellationToken? worker, RenderedCanvas data, IReadonlyCanvasViewerSettings srs, int maxLayers)
        {
            worker ??= CancellationToken.None;
            var sizes = CalculateChunkSizes(new SKSize(data.Width, data.Height), srs, maxLayers);
            worker.SafeThrowIfCancellationRequested();
            var images = new List<SKImage[,]>();

            int chunksFinishedSoFar = 0;
            int totalChunksToRender = 0;
            foreach (var size in sizes)
            {
                totalChunksToRender += size.Length;
                images.Add(new SKImage[size.GetLength(0), size.GetLength(1)]);
            }

            await Task.Delay(1);

            #region LAYER 0
            {
                SKSize[,] sizeSet = sizes[0];
                int scaleDivide = 1;
                int numChunksWide = sizeSet.GetLength(0);
                int numChunksHigh = sizeSet.GetLength(1);
                int srcPixelsPerChunk = srs.BlocksPerChunk * scaleDivide;
                int dstPixelsPerChunk = srs.TextureSize * srcPixelsPerChunk / scaleDivide;


                int iTask = 0;
                Action[] L0Tasks = new Action[sizes[0].Length];
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
                            Size = new SKSize((float)Math.Floor(tileSize.Width * scaleDivide / srs.TextureSize)
                            , (float)Math.Floor(tileSize.Height * scaleDivide / srs.TextureSize))
                        };

                        SKRect dstRect = new SKRect()
                        {
                            Location = new SKPoint(cWf * dstPixelsPerChunk, cHf * dstPixelsPerChunk),
                            Size = new SKSize(tileSize.Width, tileSize.Height)
                        };

                        L0Tasks[iTask++] = () =>
                        {
                            var bmToAdd = CreateLayer0Image(worker ?? CancellationToken.None, data, srs, srcRect, dstRect);
                            images[0][cWf, cHf] = bmToAdd;
                            Task.Delay(1).Wait();
                            int nVal = Interlocked.Increment(ref chunksFinishedSoFar);
                            ProgressX.Report(100 * nVal / totalChunksToRender);
                        };
                    }
                }

                Parallel.Invoke(new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Constants.MAX_THREADS_FOR_UI,
                    CancellationToken = worker.Value
                }, L0Tasks);
            }
            #endregion LAYER 0


            // OTHER LAYERS 2.0

            #region OTHER LAYERS
            {
                var paint = new SKPaint()
                {
                    BlendMode = SKBlendMode.Src,
                    IsAntialias = false
                };

                SKSamplingOptions samplingOptions = Constants.SAMPLE_OPTS_HIGH;
                float pixelsPerHalfChunk = srs.TextureSize * srs.BlocksPerChunk / 2;

                for (int l = 1; l < sizes.Count; l++)
                {
                    //int iTask = 0;
                    SKSize[,] sizeSet = sizes[l];
                    int scaleDivide = (int)Math.Pow(2, l);
                    int numChunksWide = sizeSet.GetLength(0);
                    int numChunksHigh = sizeSet.GetLength(1);
                    int srcPixelsPerChunk = srs.BlocksPerChunk * scaleDivide;
                    int dstPixelsPerChunk = srs.TextureSize * srcPixelsPerChunk / scaleDivide;
                    int ssWidth = sizeSet.GetLength(0);
                    int ssHeight = sizeSet.GetLength(1);
                    var upperLayer = images[l - 1];
                    int numTaskskAtLayer = ssWidth * ssHeight;
                    //;
                    for (int xi = 0; xi < ssWidth; xi++)
                    {
                        int x = xi;
                        await Parallel.ForAsync(0, ssHeight, new ParallelOptions()
                        {
                            MaxDegreeOfParallelism = Constants.MAX_THREADS_FOR_UI,
                            CancellationToken = worker.Value
                        },
                        async (y, cancelToken) =>
                        {
                            worker.SafeThrowIfCancellationRequested();
                            SKSize dstSize = sizeSet[x, y];
                            using var bm = new SKBitmap((int)dstSize.Width, (int)dstSize.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
                            using SKCanvas g = new SKCanvas(bm);

                            int xUpper = x * 2;
                            int yUpper = y * 2;

                            // TL
                            //if (upperLayer[x * 2, y * 2])
                            {
                                SKImage bmToCopy = upperLayer[xUpper, yUpper];
                                var rect = new SKRect()
                                {
                                    Location = new SKPoint(0, 0),
                                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                }; // 3ms
                                g.DrawImage(bmToCopy, rect, samplingOptions, paint); // 6ms
                            }

                            // TR
                            if (upperLayer.GetLength(0) > xUpper + 1
                                    && upperLayer.GetLength(1) > yUpper
                                    //    && upperLayer[xUpper + 1, yUpper]
                                    )
                            {
                                SKImage bmToCopy = images[l - 1][xUpper + 1, yUpper];
                                var rect = new SKRect()
                                {
                                    Location = new SKPoint(pixelsPerHalfChunk, 0),
                                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                };

                                g.DrawImage(bmToCopy, rect, samplingOptions, paint); // 6ms
                            }

                            // BL
                            if (upperLayer.GetLength(0) > xUpper
                                    && upperLayer.GetLength(1) > yUpper + 1
                                    //    && upperLayer[xUpper, yUpper + 1]
                                    )
                            {
                                SKImage bmToCopy = images[l - 1][xUpper, yUpper + 1];
                                var rect = new SKRect()
                                {
                                    Location = new SKPoint(0, pixelsPerHalfChunk),
                                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                };

                                g.DrawImage(bmToCopy, rect, samplingOptions, paint); // 6ms
                            }

                            // BR
                            if (upperLayer.GetLength(0) > xUpper + 1
                                    && upperLayer.GetLength(1) > yUpper + 1
                                    //&& upperLayer[xUpper + 1, yUpper + 1]
                                    )
                            {
                                SKImage bmToCopy = images[l - 1][xUpper + 1, yUpper + 1];
                                var rect = new SKRect()
                                {
                                    Location = new SKPoint(pixelsPerHalfChunk, pixelsPerHalfChunk),
                                    Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                };

                                g.DrawImage(bmToCopy, rect, samplingOptions, paint); // 6ms
                            }

                            images[l][x, y] = SKImage.FromBitmap(bm);
                            await Task.Delay(1); // Add a small delay to allow the ProgressX.Report call to update the UI.
                            int nVal = Interlocked.Increment(ref chunksFinishedSoFar);
                            ProgressX.Report(100 * nVal / totalChunksToRender);
                        });
                    }
                }
            }
            #endregion OTHER LAYERS
            return images;
        }

        /// <summary>
        /// When given sizes and a scale, returns an array of the chunk sizes, where the each "chunk size"
        /// represents the size of the bitmap tile to be actually rendered.
        /// </summary>
        /// <param name="srcImageSize">The TOTAL size of the data to render. (width tiles x height tiles)</param>
        /// <param name="scaleDivide">1x = 40 blocks per chunk. 2x = 80 blocks per chunk. And so on. 
        /// But each block will be rendered at half scale. Make sense? Basically this value is used
        /// for down-sizing.</param>
        /// <returns></returns>
        private static SKSize[,] CalculateChunkSizesForLayer(SKSize srcImageSize, int scaleDivide, IReadonlyCanvasViewerSettings srs)
        {
            int srcW = (int)srcImageSize.Width;
            int srcH = (int)srcImageSize.Height;
            int srcPixelsPerChunk = srs.BlocksPerChunk * scaleDivide;
            int dstPixelsPerChunk = srs.TextureSize * srcPixelsPerChunk / scaleDivide; // 16 * (RenderedCanvasPainter.BlocksPerChunk * N) / N = 6RenderedCanvasPainter.BlocksPerChunk
            int numChunksWide = (int)srcW / srcPixelsPerChunk + (srcW % srcPixelsPerChunk == 0 ? 0 : 1);
            int numChunksHigh = (int)srcH / srcPixelsPerChunk + (srcH % srcPixelsPerChunk == 0 ? 0 : 1);
            var sizeSet = new SKSize[numChunksWide, numChunksHigh];

            // MAX PERFECT WIDTH - ACTUAL WIDTH = difference
            int deltaX = numChunksWide * dstPixelsPerChunk - srs.TextureSize * srcW / scaleDivide;
            int deltaY = numChunksHigh * dstPixelsPerChunk - srs.TextureSize * srcH / scaleDivide;
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

        /// <summary>
        /// Calculates a list of arrays that each contain chunk size definitions. Layer 0 would be a 1:1 render.
        /// Layer 1 would be a half scale rendering. 
        /// </summary>
        /// <param name="data">totalSourcePixelsSize</param>
        /// <param name="srs"></param>
        /// <param name="maxLayers"></param>
        /// <returns></returns>
        private static List<SKSize[,]> CalculateChunkSizes(SKSize data, IReadonlyCanvasViewerSettings srs, int maxLayers)
        {
            int scaleDivide = 1;
            List<SKSize[,]> sizesList = new List<SKSize[,]>();
            SKSize[,] curSizeSet;

            bool a = false;
            bool b = false;
            bool c = false;
            // JUST the pixels in a dest chunk no matter what scale it is at.
            int pixelsPerChunkTile = srs.BlocksPerChunk * srs.TextureSize;

            // We run into integer overflows when doing pixelsPerChunk^2. So we use algebra to say
            // W*H*PPC*PPC > MAX_AREA is the same as W*W*PPC > MAX_AREA / PPC
            int MAX_AREA_B4_SPLIT_ADJUSTED = Constants.BIG_IMG_MAX_AREA_B4_SPLIT / pixelsPerChunkTile;
            do
            {
                curSizeSet = CalculateChunkSizesForLayer(new SKSize(data.Width, data.Height), scaleDivide, srs);
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

        private static SKImage CreateLayer0Image(CancellationToken worker, RenderedCanvas data, IReadonlyCanvasViewerSettings srs, SKRect srcTile, SKRect dstTile)
        {
            using var bm = new SKBitmap((int)dstTile.Width, (int)dstTile.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            int scaleDivide = (int)(dstTile.Width / srcTile.Width);

            int srcWidth = (int)srcTile.Width;
            int srcHeight = (int)srcTile.Height;
            using var canvas = new SKCanvas(bm);

            SKSamplingOptions samplingOptions = Constants.SAMPLE_OPTS_NONE;

            var tilesInFrame = data.CanvasData.GetEnumerator(srcTile).ToList();

            // A C T U A L _ T I L E S
            var groupsOfMaterials = tilesInFrame.GroupBy(cd => cd.PaletteID).ToList();

            Parallel.ForEach(groupsOfMaterials, new ParallelOptions()
            {
                MaxDegreeOfParallelism = Constants.MAX_THREADS_FOR_UI,
                CancellationToken = worker,
            },
            (matGroup, cancelToken) =>
            {
                int paletteID = matGroup.Key;
                var mc = data.MaterialPalette[paletteID];

                var tileRects = matGroup.Select(cd =>
                {
                    var loc = srcTile.Location;
                    float x = (cd.X - loc.X) * srs.TextureSize;
                    float y = (cd.Y - loc.Y) * srs.TextureSize;
                    SKRect tileRect = new SKRect(x, y, x + srs.TextureSize, y + srs.TextureSize);
                    return tileRect;
                });

                //MaterialCombination.PaintOntoCanvas(canvas, tileRects, mc, data.IsSideView, srs, false);
                MaterialCombinationHelper.PaintOntoCanvas(canvas, tileRects, mc, data.IsSideView, srs, false);
            });

            // Shadow solids
            if (srs.IsShadowRenderingEnabled)
            {
                var shadowGroup = tilesInFrame.Where(cd => data.MaterialPalette[cd.PaletteID].GetShadowHeight(srs) == MaterialHeight.L1_SOLID).ToList();
                using SKPath maskPath = new SKPath();
                foreach (var cd in shadowGroup)
                {
                    var loc = srcTile.Location;
                    float x = (cd.X - loc.X) * srs.TextureSize;
                    float y = (cd.Y - loc.Y) * srs.TextureSize;
                    SKRect tileRect = new SKRect(x, y, x + srs.TextureSize, y + srs.TextureSize);
                    maskPath.AddRect(tileRect);
                }

                canvas.Save();
                // Clip the canvas to the path.
                canvas.ClipPath(maskPath);
                // Create a paint object with a repeating shader using the tile image.
                using var paint = new SKPaint() { BlendMode = SKBlendMode.SrcOver /*, FilterQuality = SKFilterQuality.None*/ };
                // SKShaderTileMode.Repeat tells SkiaSharp to repeat the tile in both directions.

                // Draw over the entire canvas. Only the areas within the clip (maskPath) will show the tile.
                SKRect fullRect = new SKRect(0, 0, bm.Width, bm.Height);  //new SKRect(0, 0, canvas.LocalClipBounds.Width, canvas.LocalClipBounds.Height);

                using var paintShade = new SKPaint()
                {
                    Color = new SKColor(127, 127, 127, 40),
                    BlendMode = SKBlendMode.SrcOver,
                    IsAntialias = true,
                    IsStroke = false,
                };

                canvas.DrawRect(fullRect, paintShade);
                // Restore the canvas state.
                canvas.Restore();
            }

            // Shadow edges
            if (srs.IsShadowRenderingEnabled)
            {
                var shadesInFrame = CalculateShadowMapForSubSectionOfCanvas(data.CanvasData, data.MaterialPalette, srcTile, srs);

                int LEFT = (int)srcTile.Left;
                int TOP = (int)srcTile.Top;
                var groupsOfShadows = shadesInFrame.GroupBy(tile => tile.Data).ToList();

                Parallel.ForEach(groupsOfShadows, new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Constants.MAX_THREADS_FOR_UI,
                    CancellationToken = worker
                },
                (shadowGroup, cancelToken) =>
                {
                    if (shadowGroup.Key == ShadeFrom.EMPTY)
                        return;

                    using var paint = new SKPaint() { BlendMode = SKBlendMode.SrcOver };
                    var sFrom = shadowGroup.Key;
                    var shadeImg = ShadowHelper.GetSpriteIndividual(srs.TextureSize, sFrom);

                    foreach (var cd in shadowGroup)
                    {
                        var loc = srcTile.Location;
                        float x = (cd.X) * srs.TextureSize;
                        float y = (cd.Y) * srs.TextureSize;
                        SKRect tileRect = new SKRect(x, y, x + srs.TextureSize, y + srs.TextureSize);
                        canvas.DrawImage(shadeImg, tileRect, Constants.SAMPLE_OPTS_HIGH, paint);
                    }
                });
            }

            return SKImage.FromBitmap(bm);
        }
    }
}
