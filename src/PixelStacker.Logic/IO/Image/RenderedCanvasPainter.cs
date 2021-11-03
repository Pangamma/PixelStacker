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

namespace PixelStacker.Logic.IO.Image
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
        }

        public static async Task<RenderedCanvasPainter> Create(CancellationToken? worker, RenderedCanvas data, int maxLayers = 5)
        {
            worker ??= CancellationToken.None;
            var canvas = new RenderedCanvasPainter(data);
            worker.SafeThrowIfCancellationRequested();
            var bms = await RenderIntoTilesAsync(worker, data, maxLayers);
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
