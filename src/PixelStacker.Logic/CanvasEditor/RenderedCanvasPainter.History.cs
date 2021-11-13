using PixelStacker.Extensions;
using PixelStacker.Logic.CanvasEditor.History;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.CanvasEditor
{
    public partial class RenderedCanvasPainter
    {
        public EditHistory History { get; }
        public BufferedHistory HistoryBuffer { get; set; } = new BufferedHistory();

        public async Task DoRenderFromHistoryBuffer()
        {
            List<RenderRecord> records = HistoryBuffer.ToRenderInstructions(true);
            await DoProcessRenderRecords(records);
        }

        public Task DoProcessRenderRecords(List<RenderRecord> records)
        {
            if (records.Count == 0) return Task.CompletedTask;
            //var uniqueChunkIndexes = records.SelectMany(x => x.ChangedPixels).Distinct();
            var chunkIndexes = records.SelectMany(x => x.ChangedPixels.Select(cp => new
            {
                PaletteID = x.PaletteID,
                X = cp.X,
                Y = cp.Y
            })).GroupBy(cp => new PxPoint(GetChunkIndexX(cp.X), GetChunkIndexY(cp.Y)));

            // Layer 0
            // Iterate over chunks
            bool isv = Data.IsSideView;
            foreach (var changeGroup in chunkIndexes)
            {
                // Get a lock on the current chunk's image and make a copy
                PxPoint chunkIndex = changeGroup.Key;
                SKBitmap bmCopied = null;
                lock (this.Padlocks[0][chunkIndex.X, chunkIndex.Y])
                {
                    bmCopied = Bitmaps[0][chunkIndex.X, chunkIndex.Y].Copy();
                }

                int offsetX = chunkIndex.X * BlocksPerChunk;
                int offsetY = chunkIndex.Y * BlocksPerChunk;
                // Modify the copied chunk
                using SKCanvas skCanvas = new SKCanvas(bmCopied);
                using SKPaint paint = new SKPaint() { BlendMode = SKBlendMode.Src };
                foreach (var pxToModify in changeGroup)
                {
                    MaterialCombination mc = Data.MaterialPalette[pxToModify.PaletteID];
                    int ix = Constants.TextureSize * (pxToModify.X - offsetX);
                    int iy = Constants.TextureSize * (pxToModify.Y - offsetY);
                    skCanvas.DrawBitmap(mc.GetImage(isv), ix, iy, paint);
                }

                lock (this.Padlocks[0][chunkIndex.X, chunkIndex.Y])
                {
                    var tmp = Bitmaps[0][chunkIndex.X, chunkIndex.Y];
                    Bitmaps[0][chunkIndex.X, chunkIndex.Y] = bmCopied;
                    tmp.DisposeSafely();
                }
            }


            #region OTHER LAYERS
            {
                for (int l = 1; l < Bitmaps.Count; l++)
                {
                    SKBitmap[,] bmSet = Bitmaps[l];
                    int scaleDivide = (int)Math.Pow(2, l);
                    int numChunksWide = bmSet.GetLength(0);
                    int numChunksHigh = bmSet.GetLength(1);
                    int srcPixelsPerChunk = BlocksPerChunk * scaleDivide;
                    int dstPixelsPerChunk = Constants.TextureSize * srcPixelsPerChunk / scaleDivide;

                    for (int x = 0; x < bmSet.GetLength(0); x++)
                    {
                        for (int y = 0; y < bmSet.GetLength(1); y++)
                        {
                            SKSize dstSize;
                            lock (Padlocks[l][x, y])
                            {
                                dstSize = bmSet[x, y].Info.Size;
                            }


                            var bm = new SKBitmap((int)dstSize.Width, (int)dstSize.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
                            using SKCanvas g = new SKCanvas(bm);

                            // tiles within the chunk. We iterate over the main src image to get our content for our chunk data.
                            for (int xWithinDownsizedChunk = 0; xWithinDownsizedChunk < scaleDivide; xWithinDownsizedChunk++)
                            {
                                for (int yWithinDownsizedChunk = 0; yWithinDownsizedChunk < scaleDivide; yWithinDownsizedChunk++)
                                {
                                    int xIndexOIfL0Chunk = xWithinDownsizedChunk + scaleDivide * x;
                                    int yIndexOfL0Chunk = yWithinDownsizedChunk + scaleDivide * y;
                                    if (xIndexOIfL0Chunk > Bitmaps[0].GetLength(0) - 1 || yIndexOfL0Chunk > Bitmaps[0].GetLength(1) - 1)
                                        continue;

                                    lock (Padlocks[l][x, y])
                                    {
                                        var bmToPaint = Bitmaps[0][xIndexOIfL0Chunk, yIndexOfL0Chunk];
                                        var rect = new SKRect()
                                        {
                                            Location = new SKPoint((float)(xWithinDownsizedChunk * dstPixelsPerChunk / scaleDivide),
                                                (float)(yWithinDownsizedChunk * dstPixelsPerChunk / scaleDivide)),
                                            Size = new SKSize(dstPixelsPerChunk / scaleDivide, dstPixelsPerChunk / scaleDivide)
                                        };

                                        g.DrawBitmap(
                                        bmToPaint,
                                        rect);
                                    }
                                }
                            }

                            lock (Padlocks[l][x, y])
                            {
                                Bitmaps[l][x, y].DisposeSafely();
                                Bitmaps[l][x, y] = bm;
                            }
                        }
                    }
                }
            }
            #endregion OTHER LAYERS


            return Task.CompletedTask;
        }

        //public bool IsUndoEnabled { get => this.IsAnyChangePossible && this.History.HistoryPast.Any(); }
        //public bool IsRedoEnabled { get => this.IsAnyChangePossible && this.History.HistoryFuture.Any(); }

    }
}
