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

            return Task.CompletedTask;
        }

        //public bool IsUndoEnabled { get => this.IsAnyChangePossible && this.History.HistoryPast.Any(); }
        //public bool IsRedoEnabled { get => this.IsAnyChangePossible && this.History.HistoryFuture.Any(); }

    }
}
