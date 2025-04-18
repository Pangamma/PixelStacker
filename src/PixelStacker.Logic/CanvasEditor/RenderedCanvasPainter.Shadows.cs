using PixelStacker.Extensions;
using PixelStacker.Logic.CanvasEditor.History;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PixelStacker.Logic.CanvasEditor
{
    public partial class RenderedCanvasPainter
    {
        #region SHADOWS
        /// <summary>
        /// Where X is the location, relative to the TopLeft position of the passed in rect object,
        /// and Y is also relative to that same location. To optimize, be sure to not re-paint any
        /// blank empty spaces.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="palette"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static IEnumerable<XYData<ShadeFrom>> CalculateShadowMapForSubSectionOfCanvas(CanvasData data, MaterialPalette palette, SKRect rect, IReadonlyCanvasViewerSettings srs)
        {
            if (rect.Top < 0) throw new ArgumentOutOfRangeException(nameof(rect), $"{nameof(rect)}.Top < 0");
            if (rect.Left < 0) throw new ArgumentOutOfRangeException(nameof(rect), $"{nameof(rect)}.Left < 0");
            if (rect.Left + rect.Width > data.Width) throw new ArgumentOutOfRangeException(nameof(rect), $"{nameof(rect)}.Right > {nameof(data)}.Width - 1");
            if (rect.Top + rect.Height > data.Height) throw new ArgumentOutOfRangeException(nameof(rect), $"{nameof(rect)}.Bottom > {nameof(data)}.Height - 1");

            int rectW = (int)rect.Width;
            int rectH = (int)rect.Height;
            int airHeight = (int)MaterialHeight.L0_EMPTY;

            // Allocate the shadow heights array with an extra border and fill it with airHeight,
            // but only if airHeight is not 0 which would be the default fill value anyways.
            int[,] shadowHeights = new int[rectW + 2, rectH + 2];
            if (airHeight != 0)
            {
                for (int y = 0; y < rectH + 2; y++)
                    for (int x = 0; x < rectW + 2; x++)
                        shadowHeights[x, y] = airHeight;
            }

            // Compute an extended region (the target rect plus a border) and ensure it doesn't exceed canvas bounds.
            SKRect extendedRect = new SKRect(
                Math.Max(rect.Left - 1, 0),
                Math.Max(rect.Top - 1, 0),
                Math.Min(rect.Right + 1, data.Width),
                Math.Min(rect.Bottom + 1, data.Height)
            );

            // Fill in the shadow heights from the canvas.
            // Map each tile from canvas coordinates into the shadowHeights array.
            foreach (var tile in data.GetEnumerator(extendedRect))
            {
                int xIndex = (int)(tile.X - rect.Left) + 1;
                int yIndex = (int)(tile.Y - rect.Top) + 1;
                var mc = palette[tile.PaletteID];
                shadowHeights[xIndex, yIndex] = (int)mc.GetShadowHeight(srs);
            }

            // Process each tile in the target rectangle.
            // Note: the target cells map to indices [1, rectW] and [1, rectH] in the shadowHeights array.
            for (int yi = 1; yi <= rectH; yi++)
            {
                for (int xi = 1; xi <= rectW; xi++)
                {
                    int currentHeight = shadowHeights[xi, yi];
                    ShadeFrom sFrom = ShadeFrom.EMPTY;

                    // With the border pre-filled, we can directly compare neighbors.
                    if (shadowHeights[xi, yi - 1] > currentHeight) sFrom |= ShadeFrom.T;   // Top
                    if (shadowHeights[xi - 1, yi] > currentHeight) sFrom |= ShadeFrom.L;   // Left
                    if (shadowHeights[xi + 1, yi] > currentHeight) sFrom |= ShadeFrom.R;   // Right
                    if (shadowHeights[xi, yi + 1] > currentHeight) sFrom |= ShadeFrom.B;   // Bottom
                    if (shadowHeights[xi - 1, yi - 1] > currentHeight) sFrom |= ShadeFrom.TL; // Top-left
                    if (shadowHeights[xi + 1, yi - 1] > currentHeight) sFrom |= ShadeFrom.TR; // Top-right
                    if (shadowHeights[xi - 1, yi + 1] > currentHeight) sFrom |= ShadeFrom.BL; // Bottom-left
                    if (shadowHeights[xi + 1, yi + 1] > currentHeight) sFrom |= ShadeFrom.BR; // Bottom-right

                    yield return new XYData<ShadeFrom>(xi - 1, yi - 1, sFrom);
                }
            }
        }

        private static bool IsShadedBy(MaterialCombination mcTarget, int x2, int y2, CanvasData data, MaterialPalette palette, IReadonlyCanvasViewerSettings srs)
        {
            var mcCaster = data.IsInRange(x2, y2) ? data[x2, y2] : palette[Constants.MaterialCombinationIDForAir];
            bool isShaded = (int)mcTarget.GetShadowHeight(srs) < (int)mcCaster.GetShadowHeight(srs);
            return isShaded;
        }

        private static ShadeFrom GetTileShadowType(int x, int y, CanvasData data, MaterialPalette palette, IReadonlyCanvasViewerSettings srs)
        {
            ShadeFrom sFrom = ShadeFrom.EMPTY;
            var mc = data.IsInRange(x, y) ? data[x, y] : throw new Exception("Out of range");  // palette[Constants.MaterialCombinationIDForAir];
            bool isBlockTop = IsShadedBy(mc, x, y - 1, data, palette, srs);
            bool isBlockLeft = IsShadedBy(mc, x - 1, y, data, palette, srs);
            bool isBlockRight = IsShadedBy(mc, x + 1, y, data, palette, srs);
            bool isBlockBottom = IsShadedBy(mc, x, y + 1, data, palette, srs);
            bool isBlockTopLeft = IsShadedBy(mc, x - 1, y - 1, data, palette, srs);
            bool isBlockTopRight = IsShadedBy(mc, x + 1, y - 1, data, palette, srs);
            bool isBlockBottomLeft = IsShadedBy(mc, x - 1, y + 1, data, palette, srs);
            bool isBlockBottomRight = IsShadedBy(mc, x + 1, y + 1, data, palette, srs);

            if (isBlockTop) sFrom |= ShadeFrom.T;
            if (isBlockLeft) sFrom |= ShadeFrom.L;
            if (isBlockRight) sFrom |= ShadeFrom.R;
            if (isBlockBottom) sFrom |= ShadeFrom.B;
            if (isBlockTopLeft) sFrom |= ShadeFrom.TL;
            if (isBlockTopRight) sFrom |= ShadeFrom.TR;
            if (isBlockBottomLeft) sFrom |= ShadeFrom.BL;
            if (isBlockBottomRight) sFrom |= ShadeFrom.BR;

            return sFrom;

        }

        private static bool TryPaintShadowTile(int x, int y, CanvasData data, IReadonlyCanvasViewerSettings srs, MaterialPalette palette, SKCanvas skCanvas, SKPaint paintShade, int ix, int iy, int textureSize)
        {
            ShadeFrom sFrom = ShadeFrom.EMPTY;
            var mc = data.IsInRange(x, y) ? data[x, y] : palette[Constants.MaterialCombinationIDForAir];
            bool isBlockTop = IsShadedBy(mc, x, y - 1, data, palette, srs);
            bool isBlockLeft = IsShadedBy(mc, x - 1, y, data, palette, srs);
            bool isBlockRight = IsShadedBy(mc, x + 1, y, data, palette, srs);
            bool isBlockBottom = IsShadedBy(mc, x, y + 1, data, palette, srs);
            bool isBlockTopLeft = IsShadedBy(mc, x - 1, y - 1, data, palette, srs);
            bool isBlockTopRight = IsShadedBy(mc, x + 1, y - 1, data, palette, srs);
            bool isBlockBottomLeft = IsShadedBy(mc, x - 1, y + 1, data, palette, srs);
            bool isBlockBottomRight = IsShadedBy(mc, x + 1, y + 1, data, palette, srs);

            if (isBlockTop) sFrom |= ShadeFrom.T;
            if (isBlockLeft) sFrom |= ShadeFrom.L;
            if (isBlockRight) sFrom |= ShadeFrom.R;
            if (isBlockBottom) sFrom |= ShadeFrom.B;
            if (isBlockTopLeft) sFrom |= ShadeFrom.TL;
            if (isBlockTopRight) sFrom |= ShadeFrom.TR;
            if (isBlockBottomLeft) sFrom |= ShadeFrom.BL;
            if (isBlockBottomRight) sFrom |= ShadeFrom.BR;

            if (mc.GetShadowHeight(srs) == MaterialHeight.L1_SOLID)
            {
                skCanvas.DrawRect(ix, iy, textureSize, textureSize, paintShade);
            }

            var shadeImg = ShadowHelper.GetSpriteIndividual(textureSize, sFrom);
            lock (shadeImg)
            {
                skCanvas.DrawImage(shadeImg, new SKRect(ix, iy, ix + textureSize, iy + textureSize), Constants.SAMPLE_OPTS_HIGH);
            }

            return false;
        }

        #endregion SHADOWS
        /// <summary>
        /// HistoryRecord.ToRenderRecords() should be called for this.
        /// Basically pass in a list of points that were modified, and
        /// then this method will expand those out to include points
        /// AROUND the modified points. It will then paint on the shadow
        /// effects.
        /// </summary>
        /// <param name="records"></param>
        public void DoApplyShadowsForRenderRecords(List<RenderRecord> records)
        {
            if (!this.SpecialRenderSettings.IsShadowRenderingEnabled)
                return;
            int bpc = this.SpecialRenderSettings.BlocksPerChunk;
            int textureSize = this.SpecialRenderSettings.TextureSize;
            Dictionary<PxPoint, bool> toShadeMap = new Dictionary<PxPoint, bool>();
            foreach (var r in records)
            {
                foreach (var rr in r.ChangedPixels)
                {
                    // TRUE means it was in the original list. False means it is just something to try updating.
                    toShadeMap[rr] = true;
                    toShadeMap.TryAdd(new PxPoint(rr.X + 1, rr.Y - 1), false);
                    toShadeMap.TryAdd(new PxPoint(rr.X + 1, rr.Y), false);
                    toShadeMap.TryAdd(new PxPoint(rr.X + 1, rr.Y + 1), false);

                    toShadeMap.TryAdd(new PxPoint(rr.X - 1, rr.Y - 1), false);
                    toShadeMap.TryAdd(new PxPoint(rr.X - 1, rr.Y), false);
                    toShadeMap.TryAdd(new PxPoint(rr.X - 1, rr.Y + 1), false);

                    toShadeMap.TryAdd(new PxPoint(rr.X, rr.Y + 1), false);
                    toShadeMap.TryAdd(new PxPoint(rr.X, rr.Y - 1), false);
                }
            }


            var chunkIndexes = toShadeMap
                .Where(cp => cp.Key.X > -1 && cp.Key.X < Data.Width - 1 && cp.Key.Y > -1 && cp.Key.Y < Data.Height - 1)
                .GroupBy(cp => new PxPoint(GetChunkIndexX(cp.Key.X, bpc), GetChunkIndexY(cp.Key.Y, bpc)));

            var chunksThatNeedReRendering = GetChunksThatNeedReRendering(chunkIndexes.Select(x => x.Key));
            using SKPaint paint = new SKPaint() { BlendMode = SKBlendMode.Src };

            using var paintShade = new SKPaint()
            {
                Color = new SKColor(127, 127, 127, 40),
                BlendMode = SKBlendMode.SrcOver,
                IsAntialias = true,
                IsStroke = false
            };

            // Layer 0
            // Iterate over chunks
            bool isv = Data.IsSideView;

            foreach (var chunk in chunkIndexes)
            {
                // Get a lock on the current chunk's image and make a copy
                PxPoint chunkIndex = chunk.Key;
                SKBitmap bmCopied = null;
                lock (this.Padlocks[0][chunkIndex.X, chunkIndex.Y])
                {
                    bmCopied = SKBitmap.FromImage(Tiles[0][chunkIndex.X, chunkIndex.Y]);
                }

                int offsetX = chunkIndex.X * bpc;
                int offsetY = chunkIndex.Y * bpc;
                // Modify the copied chunk
                using SKCanvas skCanvas = new SKCanvas(bmCopied);
                bool isSolid = this.SpecialRenderSettings.IsSolidColors;
                int numToChange = chunk.Count(x => !x.Value);
                //Debug.WriteLine($"Need to change {numToChange} tiles.");
                var groupsOfChange = chunk.GroupBy(pixels =>
                {
                    var pxToModify = pixels.Key;
                    int ix = textureSize * (pxToModify.X - offsetX);
                    int iy = textureSize * (pxToModify.Y - offsetY);
                    var mcPaletteID = Data.CanvasData.GetDirectly(pxToModify.X, pxToModify.Y);
                    return mcPaletteID;
                });

                foreach (var pxToMaybeRerenderTexture in chunk)
                {
                    var pxToModify = pxToMaybeRerenderTexture.Key;
                    bool isRerenderNeeded = !pxToMaybeRerenderTexture.Value;
                    //MaterialCombination mc = Data.MaterialPalette[pxToModify.PaletteID];
                    int ix = textureSize * (pxToModify.X - offsetX);
                    int iy = textureSize * (pxToModify.Y - offsetY);

                    if (isRerenderNeeded)
                    {
                        var mc = Data.CanvasData[pxToModify.X, pxToModify.Y];
                        var rect = new SKRect() { Location = new SKPoint(ix, iy), Size = new SKSize(textureSize, textureSize) };
                        MaterialCombinationHelper.PaintOntoCanvas(skCanvas, new List<SKRect>() { rect }, mc, isv, this.SpecialRenderSettings, true);
                    }

                    if (this.SpecialRenderSettings.IsShadowRenderingEnabled)
                    {
                        TryPaintShadowTile(pxToModify.X, pxToModify.Y,
                        Data.CanvasData,
                        this.SpecialRenderSettings,
                        Data.MaterialPalette,
                        skCanvas,
                        paintShade,
                        ix,
                        iy,
                        textureSize);
                    }
                }

                lock (this.Padlocks[0][chunkIndex.X, chunkIndex.Y])
                {
                    var tmp = Tiles[0][chunkIndex.X, chunkIndex.Y];
                    Tiles[0][chunkIndex.X, chunkIndex.Y] = SKImage.FromBitmap(bmCopied);
                    tmp.DisposeSafely();
                }
            }

            // OTHER LAYERS 2.0
            {
                float pixelsPerHalfChunk = textureSize * bpc / 2;
                for (int layerIndexToRender = 1; layerIndexToRender < chunksThatNeedReRendering.Count; layerIndexToRender++)
                {
                    int scaleDivide = (int)Math.Pow(2, layerIndexToRender);
                    var curLayer = chunksThatNeedReRendering[layerIndexToRender];
                    var upperLayer = chunksThatNeedReRendering[layerIndexToRender - 1];
                    for (int xIndexCurrentLayer = 0; xIndexCurrentLayer < curLayer.GetLength(0); xIndexCurrentLayer++)
                    {
                        for (int yIndexCurrentLayer = 0; yIndexCurrentLayer < curLayer.GetLength(1); yIndexCurrentLayer++)
                        {
                            // No need to re-render this chunk?
                            if (!curLayer[xIndexCurrentLayer, yIndexCurrentLayer]) continue;

                            SKBitmap bmToEdit = null;
                            lock (Padlocks[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer])
                            {
                                bmToEdit = SKBitmap.FromImage(Tiles[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer]);
                            }

                            using SKCanvas g = new SKCanvas(bmToEdit);
                            //g.DrawRect(0, 0, bmToEdit.Width, bmToEdit.Height, new SKPaint() { Color = new SKColor(255, 0, 0, 255) });

                            int xUpper = xIndexCurrentLayer * 2;
                            int yUpper = yIndexCurrentLayer * 2;

                            // TL
                            if (upperLayer[xIndexCurrentLayer * 2, yIndexCurrentLayer * 2])
                            {
                                lock (Padlocks[layerIndexToRender - 1][xUpper, yUpper])
                                {
                                    var bmToCopy = Tiles[layerIndexToRender - 1][xUpper, yUpper];
                                    var rect = new SKRect()
                                    {
                                        Location = new SKPoint(0, 0),
                                        Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                    };

                                    g.DrawImage(bmToCopy, rect, Constants.SAMPLE_OPTS_NONE, paint);
                                }
                            }

                            // TR
                            if (upperLayer.GetLength(0) > xUpper + 1
                                && upperLayer.GetLength(1) > yUpper
                                && upperLayer[xUpper + 1, yUpper])
                            {
                                lock (Padlocks[layerIndexToRender - 1][xUpper + 1, yUpper])
                                {
                                    var bmToCopy = Tiles[layerIndexToRender - 1][xUpper + 1, yUpper];
                                    var rect = new SKRect()
                                    {
                                        Location = new SKPoint(pixelsPerHalfChunk, 0),
                                        Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                    };

                                    g.DrawImage(bmToCopy, rect, Constants.SAMPLE_OPTS_NONE, paint);
                                }
                            }

                            // BL
                            if (upperLayer.GetLength(0) > xUpper
                                && upperLayer.GetLength(1) > yUpper + 1
                                && upperLayer[xUpper, yUpper + 1])
                            {
                                lock (Padlocks[layerIndexToRender - 1][xUpper, yUpper + 1])
                                {
                                    var bmToCopy = Tiles[layerIndexToRender - 1][xUpper, yUpper + 1];
                                    var rect = new SKRect()
                                    {
                                        Location = new SKPoint(0, pixelsPerHalfChunk),
                                        Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                    };

                                    g.DrawImage(bmToCopy, rect, Constants.SAMPLE_OPTS_NONE, paint);
                                }
                            }

                            // BR
                            if (upperLayer.GetLength(0) > xUpper + 1
                                && upperLayer.GetLength(1) > yUpper + 1
                                && upperLayer[xUpper + 1, yUpper + 1])
                            {
                                lock (Padlocks[layerIndexToRender - 1][xUpper + 1, yUpper + 1])
                                {
                                    var bmToCopy = Tiles[layerIndexToRender - 1][xUpper + 1, yUpper + 1];
                                    var rect = new SKRect()
                                    {
                                        Location = new SKPoint(pixelsPerHalfChunk, pixelsPerHalfChunk),
                                        Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                    };

                                    g.DrawImage(bmToCopy, rect, Constants.SAMPLE_OPTS_NONE, paint);
                                }
                            }
                            //// Let's talk it out.
                            //// L0 is your base set of chunks. 
                            //// L1 has half as many chunks as L0.
                            //// given L1[x,y] which x,y coordinates would be from L0[x,y]?
                            //DrawTileOnLargerTile(layerIndexToRender, upperLayer, g, xUpper, yUpper, 0F, 0F, paint);
                            //DrawTileOnLargerTile(layerIndexToRender, upperLayer, g, xu, yu + 1, 0F, pixelsPerHalfChunk, paint);
                            //DrawTileOnLargerTile(layerIndexToRender, upperLayer, g, xu + 1, yu, pixelsPerHalfChunk, 0F, paint);
                            //DrawTileOnLargerTile(layerIndexToRender, upperLayer, g, xu + 1, yu + 1, pixelsPerHalfChunk, pixelsPerHalfChunk, paint);

                            lock (Padlocks[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer])
                            {
                                var tmp = Tiles[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer];
                                Tiles[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer] = SKImage.FromBitmap(bmToEdit);
                                tmp.DisposeSafely();
                            }
                        }
                    }
                }
            }
        }
    }
}
