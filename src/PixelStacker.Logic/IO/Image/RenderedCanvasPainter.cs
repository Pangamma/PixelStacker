using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Image
{
    /// <summary>
    /// Stitch a series of 6RenderedCanvasPainter.BlocksPerChunkx6RenderedCanvasPainter.BlocksPerChunk bitmaps together to make a giant bitmap tile set that renders quickly.
    /// </summary>
    public class RenderedCanvasPainter : IDisposable
    {
        public const int BlocksPerChunk = 40;
        public RenderedCanvas Data { get; }

        /// Should contain: 
        /// 0 = 1/1 size, when viewing at zoom(tex)+ to zoom(tex * 0.75) 
        /// 1 = 1/2 size
        /// 2 = 1/4 size
        /// 3 = 1/8 size
        public List<Bitmap[,]> Bitmaps { get; }

        public RenderedCanvasPainter(RenderedCanvas data)
        {
            Data = data;
            Bitmaps = new List<Bitmap[,]>();
        }

        private static Size[,] CalculateChunkSizesForLayer(Size srcImageSize, int scaleDivide)
        {
            int srcPixelsPerChunk = BlocksPerChunk * scaleDivide;
            int dstPixelsPerChunk = Constants.TextureSize * srcPixelsPerChunk / scaleDivide; // 16 * (RenderedCanvasPainter.BlocksPerChunk * N) / N = 6RenderedCanvasPainter.BlocksPerChunk
            int numChunksWide = srcImageSize.Width / srcPixelsPerChunk + (srcImageSize.Width % srcPixelsPerChunk == 0 ? 0 : 1);
            int numChunksHigh = srcImageSize.Height / srcPixelsPerChunk + (srcImageSize.Height % srcPixelsPerChunk == 0 ? 0 : 1);
            var sizeSet = new Size[numChunksWide, numChunksHigh];

            // MAX PERFECT WIDTH - ACTUAL WIDTH = difference
            int deltaX = numChunksWide * dstPixelsPerChunk - Constants.TextureSize * srcImageSize.Width / scaleDivide;
            int deltaY = numChunksHigh * dstPixelsPerChunk - Constants.TextureSize * srcImageSize.Height / scaleDivide;
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

                    sizeSet[x, y] = new Size(width: dstWidthOfChunk, height: dstHeightOfChunk);
                }
            }
            return sizeSet;
        }

        private static List<Size[,]> CalculateChunkSizes(RenderedCanvas data)
        {
            int maxLayers = 5;
            int scaleDivide = 1;
            List<Size[,]> sizesList = new List<Size[,]>();
            Size[,] curSizeSet;
            do
            {
                curSizeSet = CalculateChunkSizesForLayer(new Size(data.Width, data.Height), scaleDivide);
                sizesList.Add(curSizeSet);
                scaleDivide *= 2;
                maxLayers--;
            } while (
            // Do not split if one dimension is unable to be split further.
            curSizeSet.GetLength(0) > 2 && curSizeSet.GetLength(1) > 2

            // Do not go on forever
            && maxLayers > 0
            );

            return sizesList;
        }

        private static Bitmap RenderLayer0Image(RenderedCanvas data, Rectangle srcTile, Rectangle dstTile)
        {
            var bm = new Bitmap(dstTile.Width, dstTile.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var bmc = new AsyncBitmapWrapper(bm);
            int scaleDivide = dstTile.Width / srcTile.Width;

            bool isv = data.CanvasData.IsSideView;
            int srcWidth = srcTile.Width;
            int srcHeight = srcTile.Height;
            Parallel.For(0, srcHeight, (y) =>
            {
                using Graphics g = Graphics.FromImage(bmc.ToBitmap());
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.SmoothingMode = SmoothingMode.None;
                g.PixelOffsetMode = PixelOffsetMode.Half;

                for (int x = 0; x < srcWidth; x++)
                {
                    var mc = data.CanvasData[srcTile.X + x, srcTile.Y + y];
                    var toPaint = mc.GetImage(isv);
                    lock (toPaint)
                    {
                        g.DrawImage(toPaint, x * Constants.TextureSize, y * Constants.TextureSize);
                    }
                }
            });

            return bm;
        }

        public static async Task<RenderedCanvasPainter> Create(CancellationToken? worker, RenderedCanvas data)
        {
            worker ??= CancellationToken.None;

            var canvas = new RenderedCanvasPainter(data);
            var sizes = CalculateChunkSizes(data);
            worker.SafeThrowIfCancellationRequested();

            int chunksFinishedSoFar = 0;
            int totalChunksToRender = 0;
            foreach (var size in sizes)
            {
                totalChunksToRender += size.Length;
                canvas.Bitmaps.Add(new Bitmap[size.GetLength(0), size.GetLength(1)]);
            }



            #region LAYER 0
            {
                Size[,] sizeSet = sizes[0];
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
                        Size tileSize = sizeSet[cW, cH];
                        Rectangle dstRect = new Rectangle(
                            x: cW * dstPixelsPerChunk,
                            y: cH * dstPixelsPerChunk,
                            width: tileSize.Width,
                            height: tileSize.Height);
                        Rectangle srcRect = new Rectangle(
                            x: cW * srcPixelsPerChunk,
                            y: cH * srcPixelsPerChunk,
                            width: tileSize.Width * scaleDivide / Constants.TextureSize,
                            height: tileSize.Height * scaleDivide / Constants.TextureSize);

                        int cWf = cW; int cHf = cH;
                        L0Tasks[iTask++] = Task.Run(() =>
                        {
                            var bmToAdd = RenderLayer0Image(data, srcRect, dstRect);
                            canvas.Bitmaps[0][cWf, cHf] = bmToAdd;
                            int nVal = Interlocked.Increment(ref chunksFinishedSoFar);
                            ProgressX.Report(100 * nVal / totalChunksToRender);
                        }, worker.Value);
                    }
                }

                await Task.WhenAll(L0Tasks);
            }
            #endregion LAYER 0

            #region OTHER LAYERS
            {
                for (int l = 1; l < sizes.Count; l++)
                {
                    Size[,] sizeSet = sizes[l];
                    int scaleDivide = (int)Math.Pow(2, l);
                    int numChunksWide = sizeSet.GetLength(0);
                    int numChunksHigh = sizeSet.GetLength(1);
                    int srcPixelsPerChunk = BlocksPerChunk * scaleDivide;
                    int dstPixelsPerChunk = Constants.TextureSize * srcPixelsPerChunk / scaleDivide;

                    for (int x = 0; x < sizeSet.GetLength(0); x++)
                    {
                        for (int y = 0; y < sizeSet.GetLength(1); y++)
                        {
                            Size dstSize = sizeSet[x, y];
                            Rectangle dstRect = new Rectangle(x, y, dstSize.Width, dstSize.Height);
                            var bm = new Bitmap(dstSize.Width, dstSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            using Graphics g = Graphics.FromImage(bm);
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.InterpolationMode = InterpolationMode.NearestNeighbor;
                            g.SmoothingMode = SmoothingMode.None;
                            g.CompositingMode = CompositingMode.SourceOver;

                            // tiles within the chunk. We iterate over the main src image to get our content for our chunk data.
                            for (int xWithinDownsizedChunk = 0; xWithinDownsizedChunk < scaleDivide; xWithinDownsizedChunk++)
                            {
                                for (int yWithinDownsizedChunk = 0; yWithinDownsizedChunk < scaleDivide; yWithinDownsizedChunk++)
                                {
                                    int xIndexOIfL0Chunk = xWithinDownsizedChunk + scaleDivide * x;
                                    int yIndexOfL0Chunk = yWithinDownsizedChunk + scaleDivide * y;
                                    if (xIndexOIfL0Chunk > canvas.Bitmaps[0].GetLength(0) - 1 || yIndexOfL0Chunk > canvas.Bitmaps[0].GetLength(1) - 1)
                                        continue;

                                    var bmToPaint = canvas.Bitmaps[0][xIndexOIfL0Chunk, yIndexOfL0Chunk];
                                    lock (bmToPaint)
                                    {
                                        g.DrawImage(
                                            bmToPaint,
                                            xWithinDownsizedChunk * dstPixelsPerChunk / scaleDivide,
                                            yWithinDownsizedChunk * dstPixelsPerChunk / scaleDivide,
                                            dstPixelsPerChunk / scaleDivide,
                                            dstPixelsPerChunk / scaleDivide);
                                    }
                                }
                            }

                            canvas.Bitmaps[l][x, y] = bm;
                            ProgressX.Report(100 * ++chunksFinishedSoFar / totalChunksToRender);
                        }
                    }
                }
            }
            #endregion OTHER LAYERS

            return canvas;
        }

        public void RenderToView(Graphics g, Size parentControlSize, PanZoomSettings pz)
        {
            #region SET GRAPHICS SETTINGS
            if (pz.zoomLevel < 1.0D)
            {
                g.InterpolationMode = InterpolationMode.Low;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.PixelOffsetMode = PixelOffsetMode.Half;
            }
            else
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.Half;
            }
            #endregion SET GRAPHICS SETTINGS

            #region GET BITMAP SET
            List<Bitmap[,]> bitmaps = Bitmaps;
            if (bitmaps == null || bitmaps.Count == 0)
            {
#if !RELEASE
                throw new Exception("BAD STATE. RenderToView is called before view is ready.");
#else
                return;
#endif
            }

            Bitmap[,] toUse = bitmaps[0];
            int divideAmount = 1;
            int i = 1;
            while (pz.zoomLevel <= 10.0D / divideAmount / Constants.SMALL_IMAGE_DIVIDE_SIZE && i < bitmaps.Count)
            {
                toUse = bitmaps[i];
                divideAmount *= Constants.SMALL_IMAGE_DIVIDE_SIZE;
                i++;
            }
            #endregion GET BITMAP SET

            int CalculatedTextureSize = Constants.TextureSize;
            // SRC will be... 
            //Point pStart = getPointOnImage(new Point(0, 0), pz, EstimateProp.Floor);
            //Point fStart = getPointOnPanel(pStart, pz);
            //pStart.X *= CalculatedTextureSize; pStart.X /= divideAmount;
            //pStart.Y *= CalculatedTextureSize; pStart.Y /= divideAmount;

            //Point pEnd = getPointOnImage(new Point(parentControlSize.Width, parentControlSize.Height), pz, EstimateProp.Ceil);
            //Point fEnd = getPointOnPanel(pEnd, pz);
            //pEnd.X *= CalculatedTextureSize; pEnd.X /= divideAmount;
            //pEnd.Y *= CalculatedTextureSize; pEnd.Y /= divideAmount;

            //// SRC = Rectangular section of the raw original input image.
            //// DST = Rectangular section that may as well just be the dimensions of the panel to draw on. (Probably.)
            //Rectangle rectSRC = new Rectangle(pStart, pStart.CalculateSize(pEnd));
            //Rectangle rectDST = new Rectangle(fStart, fStart.CalculateSize(fEnd));

            // The count of ORIGINAL SOURCE pixels in a FULL chunk.
            int srcPixelsPerChunk = BlocksPerChunk * divideAmount;
            Point srcLocationOfPanelTL = GetPointOnImage(new Point(0, 0), pz, EstimateProp.Floor);
            // The offset in FULL pixels
            int xOffset = srcLocationOfPanelTL.X * CalculatedTextureSize / divideAmount;
            int yOffset = srcLocationOfPanelTL.Y * CalculatedTextureSize / divideAmount;


            Point srcLocationOfPanelBR = GetPointOnImage(new Point(parentControlSize.Width, parentControlSize.Height), pz, EstimateProp.Floor);

            // Figure out min and max chunk indexes for faster iteration.
            int minXIndex = 0; //Math.Max(0, rectSRC.X / srcPixelsPerChunk);
            int minYIndex = 0; //Math.Max(0, rectSRC.Y / srcPixelsPerChunk);
            int maxXIndex = toUse.GetLength(0) - 1; //  (int)Math.Min(toUse.GetLength(0) - 1, Math.Max(0, Math.Ceiling((double)rectSRC.X + rectSRC.Width) / srcPixelsPerChunk));
            int maxYIndex = toUse.GetLength(1) - 1; //  (int)Math.Min(toUse.GetLength(1) - 1, Math.Max(0, Math.Ceiling((double)rectSRC.Y + rectSRC.Height) / srcPixelsPerChunk));
            for (int xChunk = minXIndex; xChunk <= maxXIndex; xChunk++)
            {
                for (int yChunk = minYIndex; yChunk <= maxYIndex; yChunk++)
                {
                    var bmToPaint = toUse[xChunk, yChunk];

                    lock (bmToPaint)
                    {
                        Point pnlStart = GetPointOnPanel(new Point(xChunk * srcPixelsPerChunk, y: yChunk * srcPixelsPerChunk), pz);
                        Point pnlEnd = GetPointOnPanel(new Point(
                            x: xChunk * srcPixelsPerChunk + bmToPaint.Width * divideAmount / Constants.TextureSize
                            , y: yChunk * srcPixelsPerChunk + bmToPaint.Height * divideAmount / Constants.TextureSize
                            ), pz);

                        //// SRC will be... 
                        //Point pStart = getPointOnImage(new Point(0, 0), pz, EstimateProp.Floor);
                        //Point fStart = getPointOnPanel(pStart, pz);
                        //pStart.X *= CalculatedTextureSize; pStart.X /= divideAmount;
                        //pStart.Y *= CalculatedTextureSize; pStart.Y /= divideAmount;

                        //Point pEnd = getPointOnImage(new Point(parentControlSize.Width, parentControlSize.Height), pz, EstimateProp.Ceil);
                        //Point fEnd = getPointOnPanel(pEnd, pz);
                        //pEnd.X *= CalculatedTextureSize; pEnd.X /= divideAmount;
                        //pEnd.Y *= CalculatedTextureSize; pEnd.Y /= divideAmount;

                        // SRC = Rectangular section of the raw original input image.
                        // DST = Rectangular section that may as well just be the dimensions of the panel to draw on. (Probably.)
                        //Rectangle rectSRC = new Rectangle(pStart, pStart.CalculateSize(pEnd));
                        Rectangle rectDST = new Rectangle(pnlStart, pnlStart.CalculateSize(pnlEnd));

                        // TODO: Make it so it leaves out the parts that are cut off outside of the panel area.
                        Rectangle rectSRC = new Rectangle(0, 0, bmToPaint.Width, bmToPaint.Height);

                        g.DrawImage(image: bmToPaint,
                        srcRect: rectSRC,
                        destRect: rectDST,
                        srcUnit: GraphicsUnit.Pixel);
                    }
                }
            }
        }

        /// <summary>
        /// = (panelX - offsetX) / zoomLevel
        /// </summary>
        /// <param name="pointOnPanel"></param>
        /// <param name="pz"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static Point GetPointOnImage(Point pointOnPanel, PanZoomSettings pz, EstimateProp prop)
        {
            if (prop == EstimateProp.Ceil)
            {
                return new Point((int)Math.Ceiling((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Ceiling((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
            }
            if (prop == EstimateProp.Floor)
            {
                return new Point((int)Math.Floor((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Floor((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
            }
            return new Point((int)Math.Round((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Round((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
        }

        /// <summary>
        /// (imgX * zoom) + offsetX
        /// </summary>
        /// <param name="pointOnImage"></param>
        /// <param name="pz"></param>
        /// <returns></returns>
        private static Point GetPointOnPanel(Point pointOnImage, PanZoomSettings pz)
        {
            if (pz == null)
            {
#if !RELEASE
                throw new ArgumentNullException("PanZoomSettings are not set. So weird!");
#else
                return new Point(0, 0);
#endif
            }

            return new Point((int)Math.Round(pointOnImage.X * pz.zoomLevel + pz.imageX), (int)Math.Round(pointOnImage.Y * pz.zoomLevel + pz.imageY));
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
