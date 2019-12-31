using PixelStacker.Logic.Great;
using PixelStacker.Logic.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace PixelStacker.Logic
{
    public enum ColorPaletteStyle

    {
        CompactBrick,
        CompactSquare,
        CompactGraph,
        CompactGrid,
        DetailedGrid,
    }

    class ColorPaletteFormatter
    {
        public static void WriteBlueprint(string filePath, BlueprintPA blueprint, ColorPaletteStyle style)
        {
            if (ColorPaletteStyle.CompactBrick == style)
            {
                using (var img = RenderCompactBrick(blueprint))
                {
                    img.Save(filePath);
                }
            }
            else if (ColorPaletteStyle.CompactGraph == style)
            {
                using (var img = RenderCompactGraph(blueprint, false))
                {
                    img.Save(filePath);
                }
            }
            else if (ColorPaletteStyle.CompactSquare == style)
            {
                using (var img = RenderCompactGraph(blueprint, true))
                {
                    img.Save(filePath);
                }
            }
            else if (ColorPaletteStyle.DetailedGrid == style)
            {
                using (var img = RenderDetailedGrid(blueprint, true))
                {
                    img.Save(filePath);
                }
            }
            else if (ColorPaletteStyle.CompactGrid == style)
            {
                using (var img = RenderDetailedGrid(blueprint, false))
                {
                    img.Save(filePath);
                }
            }
        }


        private static List<IGrouping<int, Color>> SplitColors(HashSet<Color> colors)
        {
            var bucketGroups = new List<IGrouping<int, Color>>();
            var buckets = new List<List<Color>>();

            var grayscale = colors
                .Where(x => x.GetSaturation() <= 0.20 || x.GetBrightness() <= 0.15 || x.GetBrightness() >= 0.85)
                .OrderBy(x => x.GetBrightness());
            //var grayscaleDark = grayscale.Where(x => x.GetBrightness() < 0.50).OrderBy(x => x.GetBrightness()).ToList();
            //var grayscaleLight = grayscale.Where(x => x.GetBrightness() >= 0.50).OrderBy(x => x.GetBrightness()).ToList();
            var saturated = colors.Except(grayscale).OrderBy(x => x.GetHue()).ToList();
            var sortedColors = grayscale.Concat(saturated).ToList();

            #region Handle sorted colors
            {
                int numHueFragments = (int)Math.Pow(sortedColors.Count, (0.5));
                int avgHueBucketSize = (int)Math.Ceiling(((double)sortedColors.Count) / numHueFragments);
                for (int i = 0; i < sortedColors.Count; i += avgHueBucketSize)
                {
                    var colorsForThisBucket = sortedColors
                        .Skip(i)
                        .Take(avgHueBucketSize)
                        .OrderBy(x => x.GetBrightness())
                        .ToList();

                    buckets.Add(colorsForThisBucket);
                }
            }
            #endregion

            #region Handle grayscale
            //{
            //    var bucket = new List<Color>();

            //    foreach (Color c in grayscale)
            //    {
            //        bucket.Add(c);
            //        if (bucket.Count >= 10)
            //        {
            //            buckets.Add(bucket);
            //            bucket = new List<Color>();
            //        }
            //    }

            //    if (bucket.Count > 0)
            //    {
            //        buckets.Add(bucket);
            //        bucket = new List<Color>();
            //    }

            //}
            #endregion

            #region HUE
            //int numHueFragments = (int)Math.Min(18, Math.Sqrt(saturated.Count));
            //int avgHueBucketSize = (int)Math.Ceiling(((double)saturated.Count) / numHueFragments);
            //for (int i = 0; i < saturated.Count; i += avgHueBucketSize)
            //{
            //    var colorsForThisBucket = saturated
            //        .Skip(i)
            //        .Take(avgHueBucketSize)
            //        .OrderBy(x => x.GetBrightness())
            //        .ToList();
            //    buckets.Add(colorsForThisBucket);
            //}
            #endregion

            #region NORMALIZE
            {
                //int bucketSize = (int) Math.Ceiling(((double)saturated.Count) / numHueFragments);

                //double numGroups = (buckets.Count() + groupedByHue.Count());
                //int avgGroupSize = (int)(buckets.Sum(x => x.Count) + groupedByHue.Sum(x => x.Count())) / (buckets.Count() + groupedByHue.Count());

                //int maxDeviationFromAverage = avgGroupSize * 2 / 5;
                //double maxGroupSize = Math.Max(buckets.Max(x => x.Count), groupedByHue.Max(x => x.Count()));
                //double minGroupSize = Math.Min(buckets.Min(x => x.Count), groupedByHue.Min(x => x.Count()));

                //var modifiedGroups = new List<List<Color>>();
                //for (int i = 0; i < buckets.Count; i++)
                //{
                //    var bucket = buckets[i];

                //    if (bucket.Count > avgGroupSize + maxDeviationFromAverage)
                //    {
                //        var curBucket = new List<Color>(bucket.OrderBy(x => x.GetHue()).Take(avgGroupSize + maxDeviationFromAverage));
                //        var nextBucket = new List<Color>(bucket.OrderBy(x => x.GetHue()).Skip(avgGroupSize + maxDeviationFromAverage));
                //        modifiedGroups.Add(curBucket);

                //        // If we have any carry-over digits...
                //        if (nextBucket.Any())
                //        {
                //            if (i + 1 < buckets.Count)
                //            {
                //                // Add them to the next bucket if possible
                //                buckets[i + 1].AddRange(nextBucket);
                //            }
                //            else
                //            {
                //                // Append to END of list since this would be last bucket.
                //                // Might end up with a super big list. Maybe. 
                //                modifiedGroups.Add(nextBucket);
                //            }
                //        }
                //    }
                //    else
                //    if (bucket.Count < avgGroupSize - maxDeviationFromAverage)
                //    {
                //        if (i + 1 < buckets.Count)
                //        {
                //            // Add them to the next bucket if possible
                //            buckets[i + 1].AddRange(bucket);
                //        }
                //        else
                //        {
                //            // Append to END of list since this would be last bucket.
                //            // Might end up with a super big list. Maybe. 
                //            modifiedGroups.Add(bucket);
                //        }
                //    }
                //    else
                //    {
                //        modifiedGroups.Add(bucket);
                //    }
                //}

                //int nn = 0;
                //bucketGroups.Clear();
                //bucketGroups.AddRange(modifiedGroups.Select(x => new Grouping<int, Color>(nn++, x)));
            }
            #endregion
            int n = 0;
            bucketGroups.AddRange(buckets.Select(x => new Grouping<int, Color>(n++, x)));
            return bucketGroups;
        }

        #region Render 
        public static Bitmap RenderCompactBrick(BlueprintPA blueprint)
        {
            int blockWidth = 1;
            var colorsInOrder = GetColorsList(blueprint).OrderByColor(c => c).ToList();

            const int MAX_BRICK_H = 9;
            int wBrick = (int)(Math.Ceiling((double)colorsInOrder.Count() / MAX_BRICK_H)) * blockWidth;
            int hBrick = MAX_BRICK_H * blockWidth;


            using (Bitmap bm = new Bitmap(wBrick, hBrick, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.SmoothingMode = SmoothingMode.None;
                    g.PixelOffsetMode = PixelOffsetMode.None;

                    using (SolidBrush brush = new SolidBrush(Color.Transparent))
                    {
                        g.FillRectangle(brush, 0, 0, wBrick, hBrick);
                        int xBrick = 0;
                        int yBrick = 0;

                        int incrementAmount = 1;
                        foreach (Color c in colorsInOrder)
                        {
                            brush.Color = c;
                            g.FillRectangle(brush, xBrick * blockWidth, yBrick * blockWidth, blockWidth, blockWidth);
                            yBrick += incrementAmount;
                            if (yBrick > MAX_BRICK_H - 1)
                            {
                                yBrick = MAX_BRICK_H - 1;
                                incrementAmount = -1;
                                xBrick++;
                            }
                            else if (yBrick < 0)
                            {
                                yBrick = 0;
                                incrementAmount = 1;
                                xBrick++;
                            }
                        }

                    }

                    return bm.To32bppBitmap();
                }
            }
        }

        public static Bitmap RenderCompactGraph(BlueprintPA blueprint, bool isNormalized)
        {
            #region Settings
            int blockWidth = 1;
            #endregion

            HashSet<Color> colors = GetColorsList(blueprint);
            var grayscale = colors.Where(x => x.GetSaturation() <= 0.20 || x.GetBrightness() <= 0.15 || x.GetBrightness() >= 0.85)
                .OrderBy(x => x.GetBrightness());
            var grayscaleDark = grayscale.Where(x => x.GetBrightness() < 0.50).ToList();
            var grayscaleLight = grayscale.Where(x => x.GetBrightness() >= 0.50).ToList();
            var saturated = colors.Except(grayscale).ToList();
            var allColorBuckets = new List<IGrouping<int, Color>>();

            if (isNormalized)
            {
                allColorBuckets.AddRange(SplitColors(colors));
            }
            else
            {
                const int MIN_COLORS_IN_BUCKET = 5;

                var outputColorBuckets = new List<List<Color>>();
                var inputColorBuckets = new List<List<Color>>(); 
                inputColorBuckets.Add(grayscaleLight);
                inputColorBuckets.Add(grayscaleDark);
                inputColorBuckets.AddRange(saturated.GroupBy(x => ((int)Math.Round(x.GetHue())) / 9).OrderBy(x => x.Key).Select(x => x.ToList()).ToList());

                var carry = new List<Color>();
                for (int iBucket = 0; iBucket < inputColorBuckets.Count; iBucket++)
                {
                    var cBucket = inputColorBuckets[iBucket];
                    cBucket.AddRange(carry);
                    carry.Clear();

                    if (cBucket.Count() < MIN_COLORS_IN_BUCKET)
                    {
                        if (iBucket < inputColorBuckets.Count - 1)
                        {
                            carry.AddRange(cBucket);
                        }
                        else
                        {
                            outputColorBuckets.Add(cBucket);
                        }
                    }
                    else
                    {
                        outputColorBuckets.Add(cBucket);
                    }
                }

                int nBucket = 0;
                allColorBuckets.AddRange(outputColorBuckets.Select(xBucket => new Grouping<int, Color>(nBucket++, xBucket)));

            }

            int wGraph = allColorBuckets.Count * blockWidth;
            int hGraph = allColorBuckets.Count == 0 ? blockWidth : allColorBuckets.Where(x => x.Any()).Max(x => x.Count()) * blockWidth;

            using (Bitmap bm = new Bitmap(wGraph, hGraph, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.SmoothingMode = SmoothingMode.None;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

                    using (SolidBrush brush = new SolidBrush(Color.Transparent))
                    {
                        g.FillRectangle(brush, 0, 0, wGraph, hGraph);

                        int xi = 0;
                        foreach (var bucket in allColorBuckets)
                        {
                            var sortedBucket = bucket.OrderBy(x => x.GetBrightness());
                            for (int yi = 0; yi < sortedBucket.Count(); yi++)
                            {
                                brush.Color = sortedBucket.ElementAt(yi);
                                g.FillRectangle(brush, xi * blockWidth, yi * blockWidth, blockWidth, blockWidth);
                            }

                            xi++;
                        }
                    }

                    return bm.To32bppBitmap();
                }
            }
        }

        public static Bitmap RenderDetailedGrid(BlueprintPA blueprint, bool isDetailed)
        {
            #region Settings
            bool isImageMode = isDetailed;
            int gapBetweenColumns = 1;
            int gapBetweenRows = 1;
            int gapOnBorder = 1;
            bool isSide = Options.Get.IsSideView;
            #endregion

            #region GROUPINGS OF MATERIALS 
            List<Material> glasses = Materials.List.Where(m => m.Category == "Glass")
                .OrderByColor(m => m.getAverageColor(isSide)).ToList();

            List<IGrouping<string, Material>> materialGroups = Materials.List
                .Where(m => m.Category != "Glass" && m.Label != "Air")
                .GroupBy(m => m.Category)
                .OrderByDescending(m => m.Count())
                .ToList();

            List<List<Material>> leftSide = new List<List<Material>>();
            List<List<Material>> rightSide = new List<List<Material>>();

            for (int i = 0; i < materialGroups.Count; i++)
            {
                if (i % 2 == 0)
                {
                    leftSide.Add(materialGroups[i].OrderByColor(m => m.getAverageColor(isSide)).ToList());
                }
                else
                {
                    rightSide.Add(materialGroups[i].OrderByColor(m => m.getAverageColor(isSide)).ToList());
                }
            }
            #endregion

            #region GROUPINGS OF SIZES
            int blockWidth = isImageMode ? 16 : 1;
            int totalWidth = blockWidth * (
                +(gapBetweenColumns)
                + (gapOnBorder * 2) // 1 border each side
                + (17 * 2)); // two palettes

            int totalHeight = blockWidth * Math.Max(
                leftSide.Sum(mg => mg.Count() + 1) /* materials + glass index */
                + (gapOnBorder * 2) // top/bottom borders
                + (leftSide.Count - 1), // gaps between rows

                rightSide.Sum(mg => mg.Count() + 1) /* materials + glass index*/
                + (gapOnBorder * 2) // top/bottom borders
                + (leftSide.Count - 1) // gaps between rows
                );
            #endregion

            int xOffset = blockWidth;
            int yOffset = 0;
            using (Bitmap bm = new Bitmap(totalWidth, totalHeight, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.SmoothingMode = SmoothingMode.None;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

                    using (SolidBrush b = new SolidBrush(Color.Black))
                    {
                        foreach (var left in rightSide)
                        {
                            for (int mi = -1; mi < left.Count; mi++)
                            {
                                for (int z = 0; z < 17; z++)
                                {
                                    if (mi != -1)
                                    {
                                        Material mat = left[mi];
                                        if (isImageMode)
                                        {
                                            var img = mat.getImage(isSide);
                                            g.DrawImage(img, xOffset + (z * blockWidth), yOffset + blockWidth, blockWidth, blockWidth);
                                        }
                                        else
                                        {
                                            b.Color = mat.getAverageColor(isSide);
                                            g.FillRectangle(b, xOffset + (z * blockWidth), yOffset + blockWidth, blockWidth, blockWidth);
                                        }
                                    }

                                    if (z != 0) // < 17
                                    {
                                        if (isImageMode)
                                        {
                                            var imgGlass = glasses[z - 1].getImage(isSide);
                                            g.DrawImage(imgGlass, xOffset + (z * blockWidth), yOffset + blockWidth, blockWidth, blockWidth);
                                        }
                                        else
                                        {
                                            b.Color = glasses[z - 1].getAverageColor(isSide);
                                            g.FillRectangle(b, xOffset + (z * blockWidth), yOffset + blockWidth, blockWidth, blockWidth);
                                        }
                                    }
                                }

                                yOffset += blockWidth;
                            }

                            yOffset += gapBetweenRows * blockWidth;// GAP between the sections
                        }

                        xOffset += (17 + gapBetweenColumns) * blockWidth;
                        yOffset = 0;
                        foreach (var left in leftSide)
                        {
                            for (int mi = -1; mi < left.Count; mi++)
                            {
                                for (int z = 0; z < 17; z++)
                                {
                                    if (mi != -1)
                                    {
                                        Material mat = left[mi];
                                        if (isImageMode)
                                        {
                                            var img = mat.getImage(isSide);
                                            g.DrawImage(img, xOffset + (z * blockWidth), yOffset + blockWidth, blockWidth, blockWidth);
                                        }
                                        else
                                        {
                                            b.Color = mat.getAverageColor(isSide);
                                            g.FillRectangle(b, xOffset + (z * blockWidth), yOffset + blockWidth, blockWidth, blockWidth);
                                        }
                                    }

                                    if (z != 0) // < 17
                                    {
                                        if (isImageMode)
                                        {
                                            var imgGlass = glasses[z - 1].getImage(isSide);
                                            g.DrawImage(imgGlass, xOffset + (z * blockWidth), yOffset + blockWidth, blockWidth, blockWidth);
                                        }
                                        else
                                        {
                                            b.Color = glasses[z - 1].getAverageColor(isSide);
                                            g.FillRectangle(b, xOffset + (z * blockWidth), yOffset + blockWidth, blockWidth, blockWidth);
                                        }
                                    }
                                }

                                yOffset += blockWidth;
                            }

                            yOffset += gapBetweenRows * blockWidth;// GAP between the sections
                        }
                    }
                }

                return bm.To32bppBitmap();
            }
        }

        public static Bitmap RenderDetailedGrid2(BlueprintPA blueprint)
        {
            #region Settings
            int blockWidth = 16;
            int gapBetweenColumns = 2;
            int gapBetweenRows = 2;
            int gapOnBorder = 1;
            int numColumns = 2;
            int numRows = 7;
            bool isSide = Options.Get.IsSideView;
            #endregion
            List<Material> glasses = Materials.List.Where(m => m.Category == "Glass").ToList();
            List<IGrouping<string, Material>> materialGroups = Materials.List.Where(m => m.Category != "Glass" && m.Label != "Air").GroupBy(m => m.Category).ToList();


            int numGroups = materialGroups.Count;
            int numBlocksPerRow = 1 + glasses.Count; // 16 glass + 1 regular
            int numBlocksPerColumn = 1 + materialGroups.Max(mg => mg.Count()); // 16 glass + 1 regular

            int wGrid = blockWidth * (
                (2 * gapOnBorder)
                + (numColumns * numBlocksPerRow)
                + ((numColumns - 1) * gapBetweenRows)
                );

            int hGrid = blockWidth * (
                (2 * gapOnBorder)
                + (numRows * numBlocksPerColumn)
                + ((numRows - 1) * gapBetweenColumns)
                );


            // total groups CURRENTLY is... 14, plus glass.

            using (Bitmap bm = new Bitmap(wGrid, hGrid, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.SmoothingMode = SmoothingMode.None;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

                    int row = 0; int col = 0;
                    foreach (var materialGroup in materialGroups)
                    {
                        int materialIndex = 0;
                        foreach (var material in materialGroup)
                        {
                            int xOffsetMaterials = blockWidth * (1 + gapOnBorder
                            + (col * gapBetweenColumns)
                            + (col * numBlocksPerColumn)
                            );

                            int yOffsetMaterials = blockWidth * (1 + gapOnBorder
                            + (row * gapBetweenRows)
                            + (row * numBlocksPerRow)
                            + (materialIndex++) // should go all the way up to 17 or whatever it is
                            );

                            var img = material.getImage(isSide);
                            for (int glassIndex = 0; glassIndex <= glasses.Count; glassIndex++)
                            {
                                int xOffsetMaterialsWithGlassIndex = xOffsetMaterials + (glassIndex * blockWidth) - blockWidth;
                                g.DrawImage(img, xOffsetMaterialsWithGlassIndex, yOffsetMaterials, blockWidth, blockWidth);
                            }
                        }

                        for (int glassIndex = 0; glassIndex < glasses.Count; glassIndex++)
                        {
                            int xOffsetGlass = blockWidth * (1 + gapOnBorder
                            + (col * gapBetweenColumns)
                            + (col * numBlocksPerColumn)
                            + (glassIndex)
                            );

                            var imgGlass = glasses[glassIndex].getImage(isSide);

                            for (int materialItemIndex = 0; materialItemIndex <= materialGroup.Count(); materialItemIndex++)
                            {
                                int yOffsetGlass = blockWidth * (gapOnBorder
                                    + (row * gapBetweenRows)
                                    + (row * numBlocksPerRow)
                                    + (materialItemIndex) // should go all the way up to 17 or whatever it is
                                    );

                                g.DrawImage(imgGlass,
                                xOffsetGlass,
                                yOffsetGlass,
                                blockWidth, blockWidth
                                );
                            }
                        }

                        col++;
                        if (col >= numColumns)
                        {
                            col = 0;
                            row++;
                        }
                    }
                }

                return bm.To32bppBitmap();
            }
        }

        #endregion

        private static HashSet<Color> GetColorsList(BlueprintPA blueprint)
        {
            HashSet<Color> colors = new HashSet<Color>();
            for (int x = 0; x < blueprint.BlocksMap.GetLength(0); x++)
            {
                for (int y = 0; y < blueprint.BlocksMap.GetLength(1); y++)
                {
                    var row = Color.FromArgb(blueprint.BlocksMap[x, y]);
                    if (!colors.Contains(row))
                    {
                        colors.Add(row);
                    }
                }
            }

            return colors;
        }
    }
}
