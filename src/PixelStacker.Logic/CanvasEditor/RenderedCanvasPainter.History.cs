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

        public async Task DoRenderFromHistoryBuffer()
        {
            List<RenderRecord> records = new List<RenderRecord>();
            while (History.BufferedRenderQueue.TryDequeue(out RenderRecord toAdd))
            {
                records.Add(toAdd);
            }

            await DoProcessRenderRecords(records);
        }

        public async Task DoProcessRenderRecords(List<RenderRecord> records)
        {
            //var uniqueChunkIndexes = records.SelectMany(x => x.ChangedPixels).Distinct();
            var chunkIndexes = records.SelectMany(x => x.ChangedPixels.Select(cp => new { 
                PaletteID = x.PaletteID,
                X = cp.X,
                Y = cp.Y
            })).GroupBy(cp => new PxPoint(GetChunkIndexX(cp.X), GetChunkIndexY(cp.Y)));

            // Iterate over chunks
            bool isv = Data.IsSideView;
            foreach(var changeGroup in chunkIndexes)
            {
                // Get a lock on the current chunk's image and make a copy
                PxPoint chunkIndex = changeGroup.Key;
                SKBitmap bmCopied = null;
                lock(this.Padlocks[0][chunkIndex.X, chunkIndex.Y])
                {
                    bmCopied = Bitmaps[0][chunkIndex.X, chunkIndex.Y].Copy();
                }

                int offsetX = chunkIndex.X * BlocksPerChunk;
                int offsetY = chunkIndex.Y * BlocksPerChunk;
                // Modify the copied chunk
                using SKCanvas skCanvas = new SKCanvas(bmCopied);
                using SKPaint paint = new SKPaint() { BlendMode = SKBlendMode.Src};
                foreach(var pxToModify in changeGroup)
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

            //foreach (var record in records)
            //{
            //    MaterialCombination mc = this.Data.MaterialPalette[record.PaletteID];

            //    foreach(PxPoint loc in record.ChangedPixels)
            //    {
            //        // re-paint on the base image tiles then have it propogate.








            //        //#region LAYER 0
            //        //{
            //        //    SKSize[,] sizeSet = sizes[0];
            //        //    int scaleDivide = 1;
            //        //    int numChunksWide = sizeSet.GetLength(0);
            //        //    int numChunksHigh = sizeSet.GetLength(1);
            //        //    int srcPixelsPerChunk = BlocksPerChunk * scaleDivide;
            //        //    int dstPixelsPerChunk = Constants.TextureSize * srcPixelsPerChunk / scaleDivide;
            //        //    int iTask = 0;
            //        //    Task[] L0Tasks = new Task[sizes[0].Length];
            //        //    for (int cW = 0; cW < numChunksWide; cW++)
            //        //    {
            //        //        for (int cH = 0; cH < numChunksHigh; cH++)
            //        //        {
            //        //            int cWf = cW;
            //        //            int cHf = cH;
            //        //            SKSize tileSize = sizeSet[cW, cH];
            //        //            SKRect srcRect = new SKRect()
            //        //            {
            //        //                Location = new SKPoint(cWf * srcPixelsPerChunk, cHf * srcPixelsPerChunk),
            //        //                Size = new SKSize((float)Math.Floor(tileSize.Width * scaleDivide / Constants.TextureSize)
            //        //                , (float)Math.Floor(tileSize.Height * scaleDivide / Constants.TextureSize))
            //        //            };
            //        //            SKRect dstRect = new SKRect()
            //        //            {
            //        //                Location = new SKPoint(cWf * dstPixelsPerChunk, cHf * dstPixelsPerChunk),
            //        //                Size = new SKSize(tileSize.Width, tileSize.Height)
            //        //            };

            //        //            L0Tasks[iTask++] = Task.Run(() =>
            //        //            {
            //        //                var bmToAdd = CreateLayer0Image(data, srcRect, dstRect);
            //        //                bitmaps[0][cWf, cHf] = bmToAdd;
            //        //                int nVal = Interlocked.Increment(ref chunksFinishedSoFar);
            //        //                ProgressX.Report(100 * nVal / totalChunksToRender);
            //        //            }, worker.Value);
            //        //        }
            //        //    }

            //        //    await Task.WhenAll(L0Tasks);
            //        //}
            //        //#endregion LAYER 0
















            //    }
            //}
        }

        //public bool IsUndoEnabled { get => this.IsAnyChangePossible && this.History.HistoryPast.Any(); }
        //public bool IsRedoEnabled { get => this.IsAnyChangePossible && this.History.HistoryFuture.Any(); }

    }
}
