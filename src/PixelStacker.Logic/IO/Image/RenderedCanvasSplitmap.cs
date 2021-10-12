using PixelStacker.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    /// <summary>
    /// Stitch a series of 640x640 bitmaps together to make a giant bitmap tile set that renders quickly.
    /// </summary>
    public class RenderedCanvasSplitmap
    {
        public int Width { get; }
        public int Height { get; }
        public RenderedCanvas Data { get; }

        /// Should contain: 
        /// 0 = 1/1 size, when viewing at zoom(tex)+ to zoom(tex * 0.75) 
        /// 1 = 1/2 size
        /// 2 = 1/4 size
        /// 3 = 1/8 size
        public List<Bitmap[,]> Bitmaps { get; }

        public RenderedCanvasSplitmap(RenderedCanvas data)
        {
            this.Width = data.Width;
            this.Height = data.Height;
            this.Data = data;
            this.Bitmaps = new List<Bitmap[,]>();
        }

        public static async Task<RenderedCanvasSplitmap> Create(CancellationToken? worker, RenderedCanvas data)
        {
            worker ??= CancellationToken.None;
            int srcPixelsPerChunk = 40;
            int dstPixelsPerChunk = Constants.TextureSize * srcPixelsPerChunk; // 16 * 40 = 640
            int numChunksWide = (data.Width / srcPixelsPerChunk) + (data.Width % srcPixelsPerChunk == 0 ? 0 : 1);
            int numChunksHigh = (data.Height / srcPixelsPerChunk) + (data.Height % srcPixelsPerChunk == 0 ? 0 : 1);
            int totalChunks = numChunksHigh * numChunksWide // Full size
                + (numChunksHigh * numChunksWide) / 4    // 1/2 size
                + (numChunksHigh * numChunksWide) / 16   // 1/4th size
                + (numChunksHigh * numChunksWide) / 256  // 1/8th size
                ;
            int numChunksSoFar = 0;


            var canvas = new RenderedCanvasSplitmap(data);
            canvas.Bitmaps.Add(new Bitmap[numChunksWide, numChunksHigh]);
            canvas.Bitmaps.Add(new Bitmap[numChunksWide, numChunksHigh]);
            canvas.Bitmaps.Add(new Bitmap[numChunksWide, numChunksHigh]);

            Task[] tasks = new Task[numChunksHigh * numChunksWide];
            int i = 0;
            for (int cW = 0; cW < numChunksWide; cW++)
            {
                for (int cH = 0; cH < numChunksHigh; cH++)
                {
                    int ww = cW; // Make local variables
                    int hh = cH;
                    tasks[i++] = Task.Run(() => {
                        worker.Value.ThrowIfCancellationRequested();
                        RenderToBitmap(ww, hh, srcPixelsPerChunk, data, canvas);
                        int nVal = Interlocked.Increment(ref numChunksSoFar);
                        ProgressX.Report(100 * nVal / totalChunks);
                    }, worker.Value);
                }
            }

            await Task.WhenAll(tasks);

            return canvas;
        }

        private static void RenderToBitmap(int cW, int cH, int pxPerChunk, RenderedCanvas data, RenderedCanvasSplitmap splitmap)
        {
            int chunkSize = Constants.TextureSize * pxPerChunk;
            int bmW = Math.Min(data.Width, (cW + 1) * pxPerChunk) // max pixel index
                - (cW * pxPerChunk); // 0 offset
            int bmH = Math.Min(data.Height, (cH + 1) * pxPerChunk) // max pixel index
                - (cH * pxPerChunk); // 0 offset
            if (bmH == 0 || bmW == 0)
            {
                // This is very bad...
                return;
            }
            Bitmap bm = new Bitmap(bmW * Constants.TextureSize, bmH*Constants.TextureSize, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            splitmap.Bitmaps[0][cW, cH] = bm;
            int xOffset = cW * pxPerChunk;
            int yOffset = cH * pxPerChunk;
            bool isv = data.CanvasData.IsSideView;

            using (Graphics g = Graphics.FromImage(bm))
            {
                for (int x = 0; x < bmW; x++)
                {
                    for (int y = 0; y < bmH; y++)
                    {
                        var mc = data.CanvasData[x + xOffset, y + yOffset];
                        var imgTile = mc.GetImage(isv);
                        lock (imgTile)
                        {
                            g.DrawImageUnscaled(imgTile, x * Constants.TextureSize, y * Constants.TextureSize);
                        }
                    }
                }
            }

            double scale = 1;
            for (int i = 1; i < splitmap.Bitmaps.Count; i++)
            {
                scale /= 2;

                Bitmap bmSmall = new Bitmap((int)(bmW * scale), (int) (bmH * scale), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bmSmall))
                {
                    lock (bm)
                    {
                        g.DrawImage(bm, 0, 0, bmSmall.Width, bmSmall.Height);
                    splitmap.Bitmaps[i][cW, cH] = bmSmall;
                    }
                }
            }
        }

        public void Render(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        }
    }
}
