using PixelStacker.Logic.Great;
using PixelStacker.PreRender.Extensions;
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
        CompactGraph,
        CompactGrid,
        DetailedGrid,
    }

    class ColorPaletteFormatter
    {
        public static void writeBlueprint(string filePath, BlueprintPA blueprint, ColorPaletteStyle style)
        {
            if (ColorPaletteStyle.CompactBrick == style)
            {
                renderCompactBrick(filePath, blueprint);
            }
            else if (ColorPaletteStyle.CompactGraph == style)
            {
                renderCompactGraph(filePath, blueprint);
            }
            else if (ColorPaletteStyle.DetailedGrid == style)
            {
                renderDetailedGrid(filePath, blueprint, true);
            }
            else if (ColorPaletteStyle.CompactGrid == style)
            {
                renderDetailedGrid(filePath, blueprint, false);
            }
        }


        #region render 
        private static void renderCompactBrick(string filePath, BlueprintPA blueprint)
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

                    bm.Save(filePath, ImageFormat.Png);
                    return;
                }
            }
        }

        private static void renderCompactGraph(string filePath, BlueprintPA blueprint)
        {

            #region Settings
            int blockWidth = 1;
            int numHueFragments = 18; // How many buckets should we split out color wheel into?
            #endregion

            HashSet<Color> colors = GetColorsList(blueprint);
            var grayscale = colors.Where(x => x.GetSaturation() <= 0.20 || x.GetBrightness() <= 0.15 || x.GetBrightness() >= 0.85)
                .OrderBy(x => x.GetBrightness());
            var grayscaleDark = grayscale.Where(x => x.GetBrightness() < 0.50).ToList();
            var grayscaleLight = grayscale.Where(x => x.GetBrightness() >= 0.50).ToList();
            var saturated = colors.Except(grayscale).ToList();
            var allColorBuckets = new List<IGrouping<int, Color>>();
            allColorBuckets.Add(new Grouping<int, Color>(-2, grayscaleLight));
            allColorBuckets.Add(new Grouping<int, Color>(-1, grayscaleDark));
            allColorBuckets.AddRange(saturated.GroupBy(x => ((int)Math.Round(x.GetHue())) / numHueFragments).OrderBy(x => x.Key).ToList());


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

                    bm.Save(filePath, ImageFormat.Png);
                    return;
                }
            }
        }

        private static void renderDetailedGrid(string filePath, BlueprintPA blueprint, bool isDetailed)
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
                                            var imgGlass = glasses[z-1].getImage(isSide);
                                            g.DrawImage(imgGlass, xOffset + (z * blockWidth), yOffset + blockWidth, blockWidth, blockWidth);
                                        }
                                        else
                                        {
                                            b.Color = glasses[z-1].getAverageColor(isSide);
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

                bm.Save(filePath, ImageFormat.Png);
                return;
            }
        }

        private static void renderDetailedGrid2(string filePath, BlueprintPA blueprint)
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














                //for (int y = 0; y < numGroups; y++)
                //{
                //    int columnOff
                //    var materialGroup = materialGroups[y].ToList();
                //    for (int mi = 0; mi < materialGroup.Count; mi++)
                //    {
                //        var material = materialGroup[mi];
                //        var img = material.getImage(isSide);
                //        for (int x = 0; x < numBlocksPerRow; x++)
                //        {


                //            int xImg = (x + gapOnBorder) * blockWidth;
                //            int yImg = (mi + gapOnBorder) * blockWidth;
                //            g.DrawImage(img, xImg, yImg, blockWidth, blockWidth);
                //        }
                //    }
                //}




                //using (SolidBrush brush = new SolidBrush(Color.Transparent))
                //{
                //    g.FillRectangle(brush, 0, 0, wGraph, hGraph);

                //    int xi = 0;
                //    foreach (var bucket in allColorBuckets)
                //    {
                //        var sortedBucket = bucket.OrderBy(x => x.GetBrightness());
                //        for (int yi = 0; yi < sortedBucket.Count(); yi++)
                //        {
                //            brush.Color = sortedBucket.ElementAt(yi);
                //            g.FillRectangle(brush, xi * blockWidth, yi * blockWidth, blockWidth, blockWidth);
                //        }

                //        xi++;
                //    }
                //}

                bm.Save(filePath, ImageFormat.Png);
                return;
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
