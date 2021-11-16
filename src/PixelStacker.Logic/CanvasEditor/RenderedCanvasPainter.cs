using PixelStacker.Extensions;
using PixelStacker.Logic.CanvasEditor.History;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.CanvasEditor
{
    /// <summary>
    /// Stitch a series of 6RenderedCanvasPainter.BlocksPerChunkx6RenderedCanvasPainter.BlocksPerChunk bitmaps together to make a giant bitmap tile set that renders quickly.
    /// </summary>
    public partial class RenderedCanvasPainter : IDisposable
    {
        public RenderedCanvas Data { get; }

        public RenderedCanvasPainter(RenderedCanvas data)
        {
            Data = data;
            Bitmaps = new List<SKBitmap[,]>();
            Padlocks = new List<object[,]>();
            History = new EditHistory(Data);
        }

        public static async Task<RenderedCanvasPainter> Create(CancellationToken? worker, RenderedCanvas data, int maxLayers = 10)
        {
            worker ??= CancellationToken.None;
            var canvas = new RenderedCanvasPainter(data);
            worker.SafeThrowIfCancellationRequested();
            var bms = await RenderIntoTilesAsync(worker, data, maxLayers);

            var padlocks = new List<object[,]>();
            for (int i = 0; i < bms.Count; i++)
            {
                var bmLayer = bms[i];
                var layer = new object[bmLayer.GetLength(0), bmLayer.GetLength(1)];
                padlocks.Add(layer);
                for (int x = 0; x < layer.GetLength(0); x++)
                    for (int y = 0; y < layer.GetLength(1); y++)
                        layer[x, y] = new object {};
            }

            canvas.Padlocks.Clear();
            canvas.Padlocks.AddRange(padlocks);
            canvas.Bitmaps.Clear();
            canvas.Bitmaps.AddRange(bms);
            
            return canvas;
        }

        #region DISPOSE
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var bms in Bitmaps)
                    {
                        foreach (var bm in bms)
                        {
                            bm.DisposeSafely();
                        }
                    }

                    Bitmaps.Clear();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion DISPOSE
    }
}
