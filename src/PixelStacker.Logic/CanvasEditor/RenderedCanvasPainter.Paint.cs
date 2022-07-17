using PixelStacker.Logic.Extensions;
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
        public void PaintSurface(SKCanvas g, SKSize parentControlSize, PanZoomSettings pz, CanvasViewerSettings vs)
        {
            PaintTilesToView(g, parentControlSize, pz, this.Bitmaps, this.Padlocks);
            if (vs.IsShowGrid) DrawGridLines(g, Data, vs, pz);
            if (vs.IsShowBorder) DrawBorder(g, pz, new SKSize(Data.Width, Data.Height));
            if (Data.WorldEditOrigin != null) DrawWorldEditOrigin(g, pz, Data.WorldEditOrigin);
        }

        protected static void DrawGridLines(SKCanvas g, RenderedCanvas canvas, CanvasViewerSettings vs, PanZoomSettings pz)
        {
            if (pz.zoomLevel >= 0.5D)
            {
                DrawGridMask(g, pz, vs.GridSize, vs.GridColor);
                DrawGrid(g, canvas, pz, vs.GridSize, vs.GridColor.WithAlpha(255));
                if (pz.zoomLevel > 5) DrawGrid(g, canvas, pz, 1, vs.GridColor.WithAlpha(40));
            }
        }

        private static void DrawGridMask(SKCanvas g, PanZoomSettings pz, int gridSize, SKColor c)
        {
            //if (this.gridMaskClip == null) return;
            //Point tL = new Point(this.gridMaskClip.Value.Left, this.gridMaskClip.Value.Top);
            //Point bR = new Point(this.gridMaskClip.Value.Right, this.gridMaskClip.Value.Bottom);
            //tL = this.GetPointOnPanel(tL);
            //bR = this.GetPointOnPanel(bR);
            //g.ExcludeClip(new Rectangle(tL, tL.CalculateSize(bR)));
            //using (Pen p = new Pen(Color.FromArgb(128, c)))
            //{
            //    g.FillRectangle(p.Brush, 0, 0, this.Width, this.Height);
            //}
            //g.ResetClip();
        }

        private static void DrawGrid(SKCanvas g, RenderedCanvas canvas, PanZoomSettings pz, int gridSize, SKColor c)
        {
            static int getRoundedZoomDistance(int x, int deltaX, double zoom) => (int)Math.Round(x + deltaX * zoom);
            static int getRoundedZoomX(int val, int blockSize, double zoom, int imageX) => (int)Math.Floor(imageX + val * blockSize * zoom);
            static int getRoundedZoomY(int val, int blockSize, double zoom, int imageY) => (int)Math.Floor(imageY + val * blockSize * zoom);
            double zoom = pz.zoomLevel;
            int numHorizBlocks = (canvas.Width / gridSize);
            int numVertBlocks = (canvas.Height / gridSize);
            using SKPaint p = new SKPaint()
            {
                Color = c,
                StrokeWidth = gridSize == 1 ? 2 : GetGridWidth(zoom),
                Style = SKPaintStyle.StrokeAndFill,
            };

            g.DrawLine(pz.imageX, pz.imageY, pz.imageX, getRoundedZoomDistance(pz.imageY, canvas.Height, zoom), p);
            g.DrawLine(pz.imageX, getRoundedZoomDistance(pz.imageY, canvas.Height, zoom), getRoundedZoomDistance(pz.imageX, canvas.Width, zoom), getRoundedZoomDistance(pz.imageY, canvas.Height, zoom), p);
            g.DrawLine(getRoundedZoomDistance(pz.imageX, canvas.Width, zoom), getRoundedZoomDistance(pz.imageY, canvas.Height, zoom), getRoundedZoomDistance(pz.imageX, canvas.Width, zoom), pz.imageY, p);
            g.DrawLine(getRoundedZoomDistance(pz.imageX, canvas.Width, zoom), pz.imageY, pz.imageX, pz.imageY, p);
            for (int x = 0; x <= numHorizBlocks; x++)
            {
                g.DrawLine(getRoundedZoomX(x, gridSize, zoom, pz.imageX), pz.imageY, getRoundedZoomX(x, gridSize, zoom, pz.imageX), getRoundedZoomDistance(pz.imageY, canvas.Height, zoom), p);
            }
            for (int y = 0; y <= numVertBlocks; y++)
            {
                g.DrawLine(pz.imageX, getRoundedZoomY(y, gridSize, zoom, pz.imageY), getRoundedZoomDistance(pz.imageX, canvas.Width, zoom), getRoundedZoomY(y, gridSize, zoom, pz.imageY), p);
            }
        }


        private static int GetGridWidth(double zoom)
        {
            if (zoom > 70) return 8;
            if (zoom > 60) return 7;
            if (zoom > 50) return 6;
            if (zoom > 35) return 5;
            if (zoom > 25) return 4;
            if (zoom > 15) return 3;
            if (zoom > 8) return 2;
            if (zoom >= 0) return 1;
            return 1;
        }

        #region WE Origin
        private static void DrawWorldEditOrigin(SKCanvas g, PanZoomSettings pz, PxPoint weOrigin)
        {
            var zoom = pz.zoomLevel;
            var color = new SKColor(255, 0, 0);
            using var paint = new SKPaint()
            {
                Color = color,
                IsAntialias = false
            };

            SKPoint wePoint = GetPointOnPanel(new SKPoint((int)weOrigin.X, (int)weOrigin.Y), pz);
            g.DrawRect(wePoint.X, wePoint.Y, (float)zoom + 1, (float)zoom + 1, paint);
        }
        #endregion WE Origin


        #region Border
        private static void DrawBorder(SKCanvas g, PanZoomSettings pz, SKSize canvasDimensions)
        {
            SKPoint pp2 = GetPointOnPanel(new SKPoint(0, 0), pz);
            float strokeWidth = (float)Math.Max(1, pz.zoomLevel);
            float strokeOffset = strokeWidth / 2;
            using SKPaint penWE = new SKPaint()
            {
                Color = new SKColor(0, 0, 0, 127),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth, // Apparently this will be equal to the size of a block.
            };
            g.DrawRect(
                pp2.X + strokeOffset, pp2.Y + strokeOffset,
                (int)(canvasDimensions.Width * pz.zoomLevel) - strokeWidth, (int)(canvasDimensions.Height * pz.zoomLevel) - strokeWidth, penWE);
        }
        #endregion Border

        /// <summary>
        /// Paint the rendered canvas onto the SKCanvas view.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="parentControlSize"></param>
        /// <param name="pz"></param>
        private static void PaintTilesToView(SKCanvas g, SKSize parentControlSize, PanZoomSettings pz, List<SKBitmap[,]> bitmaps, List<object[,]> padlocks)
        {
            #region SET GRAPHICS SETTINGS
            //if (pz.zoomLevel < 1.0D)
            //{
            //    g.InterpolationMode = InterpolationMode.Low;
            //    g.CompositingQuality = CompositingQuality.HighSpeed;
            //    g.SmoothingMode = SmoothingMode.HighSpeed;
            //    g.PixelOffsetMode = PixelOffsetMode.Half;
            //}
            //else
            //{
            //    g.InterpolationMode = InterpolationMode.NearestNeighbor;
            //    g.CompositingQuality = CompositingQuality.HighSpeed;
            //    g.SmoothingMode = SmoothingMode.AntiAlias;
            //    g.PixelOffsetMode = PixelOffsetMode.Half;
            //}
            #endregion SET GRAPHICS SETTINGS

            #region GET BITMAP SET
            if (bitmaps == null || bitmaps.Count == 0 || padlocks == null || padlocks.Count == 0)
            {
#if FAIL_FAST
                throw new Exception("BAD STATE. RenderToView is called before view is ready.");
#else
                return;
#endif
            }

            SKBitmap[,] toUse = bitmaps[0];
            object[,] lockSetToUse = padlocks[0];
            int divideAmount = 1;
            int i = 1;
            while (pz.zoomLevel <= 10.0D / divideAmount / Constants.SMALL_IMAGE_DIVIDE_SIZE && i < bitmaps.Count)
            {
                toUse = bitmaps[i];
                lockSetToUse = padlocks[i];
                divideAmount *= Constants.SMALL_IMAGE_DIVIDE_SIZE;
                i++;
            }
            #endregion GET BITMAP SET

            // The count of ORIGINAL SOURCE pixels in a FULL chunk.
            int srcPixelsPerChunk = BlocksPerChunk * divideAmount;
            SKPoint srcLocationOfPanelTL = GetPointOnImage(new SKPoint(0, 0), pz, EstimateProp.Floor);
            SKPoint srcLocationOfPanelBR = GetPointOnImage(new SKPoint(parentControlSize.Width, parentControlSize.Height), pz, EstimateProp.Floor);

            // Figure out min and max chunk indexes for faster iteration.
            int minXIndex = (int)Math.Floor(srcLocationOfPanelTL.X / srcPixelsPerChunk);
            int minYIndex = (int)Math.Floor(srcLocationOfPanelTL.Y / srcPixelsPerChunk); 
            int maxXIndex = (int)Math.Ceiling(srcLocationOfPanelBR.X / srcPixelsPerChunk); 
            int maxYIndex = (int)Math.Ceiling(srcLocationOfPanelBR.Y / srcPixelsPerChunk); 

            // Prevent out of bounds exceptions and clip it to only what is actually visible and renderable.
            int maxX = toUse.GetLength(0) - 1;
            int maxY = toUse.GetLength(1) - 1;
            minXIndex = Math.Clamp(minXIndex, 0, maxX);
            minYIndex = Math.Clamp(minYIndex, 0, maxY);
            maxXIndex = Math.Clamp(maxXIndex, 0, maxX);
            maxYIndex = Math.Clamp(maxYIndex, 0, maxY);
#if DEBUG_GPU
            using SKPaint chunkLines = new SKPaint() { Color = new SKColor(255, 255, 0), IsStroke = true, StrokeWidth = 2, BlendMode = SKBlendMode.Src, PathEffect = SKPathEffect.CreateDash(new float[] { 4, 4 }, 0) };
#endif
            for (int xChunk = minXIndex; xChunk <= maxXIndex; xChunk++)
            {
                for (int yChunk = minYIndex; yChunk <= maxYIndex; yChunk++)
                {
                    lock (lockSetToUse[xChunk, yChunk])
                    {
                        var bmToPaint = toUse[xChunk, yChunk];
                        SKPoint pnlStart = GetPointOnPanel(new SKPoint(x: xChunk * srcPixelsPerChunk, y: yChunk * srcPixelsPerChunk), pz);
                        SKPoint pnlEnd = GetPointOnPanel(new SKPoint(
                            x: (xChunk * srcPixelsPerChunk) + (bmToPaint.Width * divideAmount / Constants.TextureSize),
                            y: (yChunk * srcPixelsPerChunk) + (bmToPaint.Height * divideAmount / Constants.TextureSize)),
                            pz);
                        SKRect rectDST = pnlStart.ToRectangle(pnlEnd);
                        SKRect rectSRC = PointExtensions.ToRectangle(0, 0, bmToPaint.Width, bmToPaint.Height); // left, top, right, bottom
                                                                                                               //g.DrawImage(image: bmToPaint, source: rectSRC, dest: rectDST);

                        g.DrawBitmap(bitmap: bmToPaint,
                        source: rectSRC,
                        dest: rectDST);
#if DEBUG_GPU
                        g.DrawRect(rectDST, chunkLines);
#endif
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
        private static SKPoint GetPointOnImage(SKPoint pointOnPanel, PanZoomSettings pz, EstimateProp prop)
        {
            if (prop == EstimateProp.Ceil)
            {
                return new SKPoint((int)Math.Ceiling((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Ceiling((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
            }
            if (prop == EstimateProp.Floor)
            {
                return new SKPoint((int)Math.Floor((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Floor((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
            }
            return new SKPoint((int)Math.Round((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Round((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
        }

        /// <summary>
        /// (imgX * zoom) + offsetX
        /// </summary>
        /// <param name="pointOnImage"></param>
        /// <param name="pz"></param>
        /// <returns></returns>
        private static SKPoint GetPointOnPanel(SKPoint pointOnImage, PanZoomSettings pz)
        {
            if (pz == null)
            {
#if FAIL_FAST
                throw new ArgumentNullException("PanZoomSettings are not set. So weird!");
#else
                return new SKPoint(0, 0);
#endif
            }

            return new SKPoint((int)Math.Round(pointOnImage.X * pz.zoomLevel + pz.imageX), (int)Math.Round(pointOnImage.Y * pz.zoomLevel + pz.imageY));
        }
    }
}
