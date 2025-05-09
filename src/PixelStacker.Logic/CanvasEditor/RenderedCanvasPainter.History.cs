﻿using PixelStacker.Extensions;
using PixelStacker.Logic.CanvasEditor.History;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelStacker.Logic.CanvasEditor
{
    public partial class RenderedCanvasPainter
    {
        public SuperHistory History { get; }

        private List<bool[,]> GetChunksThatNeedReRendering(IEnumerable<PxPoint> chunks)
        {
            var output = new List<bool[,]>();
            for (int i = 0; i < Padlocks.Count; i++)
            {
                output.Add(new bool[Padlocks[i].GetLength(0), Padlocks[i].GetLength(1)]);
            }

            foreach (var chunk in chunks)
            {
                output[0][chunk.X, chunk.Y] = true;
            }

            for (int i = 1; i < output.Count; i++)
            {
                var curLayer = output[i];
                var upperLayer = output[i - 1];

                for (int x = 0; x < upperLayer.GetLength(0); x++)
                {
                    for (int y = 0; y < upperLayer.GetLength(1); y++)
                    {
                        curLayer[x / 2, y / 2] |= upperLayer[x, y];
                    }
                }
            }

            return output;
        }


        /// <summary>
        /// You will have to apply shadows as well here.
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public Task DoProcessRenderRecords(List<RenderRecord> records)
        {
            if (records.Count == 0) return Task.CompletedTask;
            int bpc = this.SpecialRenderSettings.BlocksPerChunk;
            int texSize = this.SpecialRenderSettings.TextureSize;
            //var uniqueChunkIndexes = records.SelectMany(x => x.ChangedPixels).Distinct();
            var chunkIndexes = records.SelectMany(x => x.ChangedPixels.Select(cp => new
            {
                PaletteID = x.PaletteID,
                X = cp.X,
                Y = cp.Y
            })).GroupBy(cp => new PxPoint(GetChunkIndexX(cp.X, bpc), GetChunkIndexY(cp.Y, bpc)));

            var chunksThatNeedReRendering = GetChunksThatNeedReRendering(chunkIndexes.Select(x => x.Key));
            using SKPaint paint = new SKPaint() { BlendMode = SKBlendMode.Src };

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
                    bmCopied = SKBitmap.FromImage(Tiles[0][chunkIndex.X, chunkIndex.Y]);
                }

                int offsetX = chunkIndex.X * bpc;
                int offsetY = chunkIndex.Y * bpc;
                // Modify the copied chunk
                using SKCanvas skCanvas = new SKCanvas(bmCopied);

                var matGroups = changeGroup.GroupBy(chunk => chunk.PaletteID);
                using var primer = new SKPaint() { BlendMode = SKBlendMode.Src, Color = new SKColor(0, 0, 0, 0) };
                foreach (var matGroup in matGroups)
                {
                    var tileRects = matGroup.Select(pxToModify =>
                    {
                        int ix = texSize * (pxToModify.X - offsetX);
                        int iy = texSize * (pxToModify.Y - offsetY);
                        var rect = new SKRect(ix, iy, ix + texSize, iy + texSize);
                        return rect;
                    });

                    // Initialize by throwing out old paint.
                    foreach (var tileRect in tileRects)
                    {
                        skCanvas.DrawRect(tileRect, primer);
                    }

                    MaterialCombination mc = Data.MaterialPalette[matGroup.Key];
                    MaterialCombinationHelper.PaintOntoCanvas(skCanvas, tileRects, mc, isv, this.SpecialRenderSettings, false);
                    //MaterialCombination.PaintOntoCanvas(skCanvas, tileRects, mc, isv, this.SpecialRenderSettings, true);
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
                float pixelsPerHalfChunk = texSize * bpc / 2;
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
                                    SKImage bmToCopy = Tiles[layerIndexToRender - 1][xUpper, yUpper];
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
                                    SKImage bmToCopy = Tiles[layerIndexToRender - 1][xUpper + 1, yUpper];
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
                                    SKImage bmToCopy = Tiles[layerIndexToRender - 1][xUpper, yUpper + 1];
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
                                    SKImage bmToCopy = Tiles[layerIndexToRender - 1][xUpper + 1, yUpper + 1];
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

                return Task.CompletedTask;
            }
        }

    }
}
