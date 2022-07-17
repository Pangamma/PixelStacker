using PixelStacker.Extensions;
using PixelStacker.Logic.CanvasEditor.History;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelStacker.Logic.CanvasEditor
{
    public partial class RenderedCanvasPainter
    {
        #region SHADOWS
        private static bool IsShadedBy(MaterialCombination mcTarget, int x2, int y2, CanvasData data, MaterialPalette palette)
        {
            var mcCaster = data.IsInRange(x2, y2) ? data[x2, y2] : palette[Constants.MaterialCombinationIDForAir];
            bool isShaded = (int)mcTarget.GetShadowHeight() < (int)mcCaster.GetShadowHeight();
            return isShaded;
        }

        private static bool TryPaintShadowTile(int x, int y, CanvasData data, MaterialPalette palette, SKCanvas skCanvas, SKPaint paintShade, int ix, int iy)
        {
            ShadeFrom sFrom = ShadeFrom.EMPTY;
            var mc = data.IsInRange(x, y) ? data[x, y] : palette[Constants.MaterialCombinationIDForAir];
            bool isBlockTop = IsShadedBy(mc, x, y - 1, data, palette);
            bool isBlockLeft = IsShadedBy(mc, x - 1, y, data, palette);
            bool isBlockRight = IsShadedBy(mc, x + 1, y, data, palette);
            bool isBlockBottom = IsShadedBy(mc, x, y + 1, data, palette);
            bool isBlockTopLeft = IsShadedBy(mc, x - 1, y - 1, data, palette);
            bool isBlockTopRight = IsShadedBy(mc, x + 1, y - 1, data, palette);
            bool isBlockBottomLeft = IsShadedBy(mc, x - 1, y + 1, data, palette);
            bool isBlockBottomRight = IsShadedBy(mc, x + 1, y + 1, data, palette);

            if (isBlockTop) sFrom |= ShadeFrom.T;
            if (isBlockLeft) sFrom |= ShadeFrom.L;
            if (isBlockRight) sFrom |= ShadeFrom.R;
            if (isBlockBottom) sFrom |= ShadeFrom.B;
            if (isBlockTopLeft) sFrom |= ShadeFrom.TL;
            if (isBlockTopRight) sFrom |= ShadeFrom.TR;
            if (isBlockBottomLeft) sFrom |= ShadeFrom.BL;
            if (isBlockBottomRight) sFrom |= ShadeFrom.BR;

            if (mc.GetShadowHeight() == MaterialHeight.L1_SOLID)
            {
                skCanvas.DrawRect(ix, iy, Constants.TextureSize, Constants.TextureSize, paintShade);
            }

            var shadeImg = ShadowHelper.GetSpriteIndividual(Constants.TextureSize, sFrom);
            lock (shadeImg)
            {
                skCanvas.DrawBitmap(shadeImg, new SKRect(ix, iy, ix + Constants.TextureSize, iy + Constants.TextureSize));
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
            if (!this.SpecialRenderSettings.EnableShadows)
                return;

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

            //var uniqueChunkIndexes = records.SelectMany(x => x.ChangedPixels).Distinct();
            var chunkIndexes = toShadeMap
                .Where(cp => cp.Key.X > -1 && cp.Key.X < Data.Width - 1 && cp.Key.Y > -1 && cp.Key.Y < Data.Height - 1)
                .GroupBy(cp => new PxPoint(GetChunkIndexX(cp.Key.X), GetChunkIndexY(cp.Key.Y)));

            var chunksThatNeedReRendering = GetChunksThatNeedReRendering(chunkIndexes.Select(x => x.Key));
            using SKPaint paint = new SKPaint() { BlendMode = SKBlendMode.Src, FilterQuality = SKFilterQuality.None };

            using var paintShade = new SKPaint()
            {
                Color = new SKColor(127, 127, 127, 40),
                BlendMode = SKBlendMode.SrcOver,
                IsAntialias = true,
                IsStroke = false,
                FilterQuality = SKFilterQuality.High
            };

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
                bool isSolid = this.SpecialRenderSettings.IsSolidColors;
                var paintSolid = new SKPaint()
                {
                    BlendMode = SKBlendMode.Src,
                    FilterQuality = SKFilterQuality.High,
                    IsAntialias = false,
                    IsStroke = false, // FILL
                };

                foreach (var pxToMaybeRerenderTexture in changeGroup)
                {
                    var pxToModify = pxToMaybeRerenderTexture.Key;
                    bool isRerenderNeeded = !pxToMaybeRerenderTexture.Value;
                    //MaterialCombination mc = Data.MaterialPalette[pxToModify.PaletteID];
                    int ix = Constants.TextureSize * (pxToModify.X - offsetX);
                    int iy = Constants.TextureSize * (pxToModify.Y - offsetY);

                    if (isRerenderNeeded)
                    {
                        var mc = Data.CanvasData[pxToModify.X, pxToModify.Y];

                        if (isSolid)
                        {
                            paintSolid.Color = mc.GetAverageColor(isv, this.SpecialRenderSettings);
                            skCanvas.DrawRect(new SKRect() { Location = new SKPoint(ix, iy), Size = new SKSize(Constants.TextureSize, Constants.TextureSize) }, paintSolid);
                        }
                        else
                        {
                            skCanvas.DrawBitmap(mc.GetImage(isv, this.SpecialRenderSettings),
                                new SKRect() { Location = new SKPoint(ix, iy), Size = new SKSize(Constants.TextureSize, Constants.TextureSize) },
                                paint);
                        }
                    }

                    if (this.SpecialRenderSettings.EnableShadows)
                    {
                        TryPaintShadowTile(pxToModify.X, pxToModify.Y,
                        Data.CanvasData,
                        Data.MaterialPalette,
                        skCanvas,
                        paintShade,
                        ix,
                        iy);
                    }
                }

                lock (this.Padlocks[0][chunkIndex.X, chunkIndex.Y])
                {
                    var tmp = Bitmaps[0][chunkIndex.X, chunkIndex.Y];
                    Bitmaps[0][chunkIndex.X, chunkIndex.Y] = bmCopied;
                    tmp.DisposeSafely();
                }
            }

            // OTHER LAYERS 2.0
            {
                float pixelsPerHalfChunk = Constants.TextureSize * BlocksPerChunk / 2;
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
                                bmToEdit = Bitmaps[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer].Copy();
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
                                    SKBitmap bmToCopy = Bitmaps[layerIndexToRender - 1][xUpper, yUpper];
                                    var rect = new SKRect()
                                    {
                                        Location = new SKPoint(0, 0),
                                        Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                    };

                                    g.DrawBitmap(bmToCopy, rect, paint);
                                }
                            }

                            // TR
                            if (upperLayer.GetLength(0) > xUpper + 1
                                && upperLayer.GetLength(1) > yUpper
                                && upperLayer[xUpper + 1, yUpper])
                            {
                                lock (Padlocks[layerIndexToRender - 1][xUpper + 1, yUpper])
                                {
                                    SKBitmap bmToCopy = Bitmaps[layerIndexToRender - 1][xUpper + 1, yUpper];
                                    var rect = new SKRect()
                                    {
                                        Location = new SKPoint(pixelsPerHalfChunk, 0),
                                        Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                    };

                                    g.DrawBitmap(bmToCopy, rect, paint);
                                }
                            }

                            // BL
                            if (upperLayer.GetLength(0) > xUpper
                                && upperLayer.GetLength(1) > yUpper + 1
                                && upperLayer[xUpper, yUpper + 1])
                            {
                                lock (Padlocks[layerIndexToRender - 1][xUpper, yUpper + 1])
                                {
                                    SKBitmap bmToCopy = Bitmaps[layerIndexToRender - 1][xUpper, yUpper + 1];
                                    var rect = new SKRect()
                                    {
                                        Location = new SKPoint(0, pixelsPerHalfChunk),
                                        Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                    };

                                    g.DrawBitmap(bmToCopy, rect, paint);
                                }
                            }

                            // BR
                            if (upperLayer.GetLength(0) > xUpper + 1
                                && upperLayer.GetLength(1) > yUpper + 1
                                && upperLayer[xUpper + 1, yUpper + 1])
                            {
                                lock (Padlocks[layerIndexToRender - 1][xUpper + 1, yUpper + 1])
                                {
                                    SKBitmap bmToCopy = Bitmaps[layerIndexToRender - 1][xUpper + 1, yUpper + 1];
                                    var rect = new SKRect()
                                    {
                                        Location = new SKPoint(pixelsPerHalfChunk, pixelsPerHalfChunk),
                                        Size = new SKSize(bmToCopy.Width / 2, bmToCopy.Height / 2)
                                    };

                                    g.DrawBitmap(bmToCopy, rect, paint);
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
                                var tmp = Bitmaps[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer];
                                Bitmaps[layerIndexToRender][xIndexCurrentLayer, yIndexCurrentLayer] = bmToEdit;
                                tmp.DisposeSafely();
                            }
                        }
                    }
                }
            }
        }
    }
}
