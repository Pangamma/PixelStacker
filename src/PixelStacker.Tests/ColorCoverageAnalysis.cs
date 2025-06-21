using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Logic.Model;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PixelStacker.Tests
{
    public class ColorNode
    {
        public MaterialCombination mc;

        public int MaterialPaletteID { get; set; }
        public SKColor Color { get; internal set; }
        public double Hue { get; internal set; }
        public float Brightness { get; internal set; }
        public float Saturation { get; internal set; }

        public double TargetX { get; set; }
        public double TargetY { get; set; }
        public bool IsUsed { get; set; } = false;
    }

    public class SimpleGrouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public TKey Key { get; }
        private IEnumerable<TElement> Elements { get; }

        public SimpleGrouping(TKey key, IEnumerable<TElement> elements)
        {
            Key = key;
            Elements = elements;
        }

        public IEnumerator<TElement> GetEnumerator() => Elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [TestClass]
    public class ColorCoverageAnalysis
    {
        private (int x, int y) FindNearestFreeSpot(Dictionary<(int x, int y), ColorNode> grid, int startX, int startY, ColorNode node)
        {
            int radius = 0;
            int xRadius = radius;
            int maxYRadius = 25;
            while (true)
            {
                for (int dx = -xRadius; dx <= xRadius; dx++)
                {
                    for (int dy = -Math.Min(radius, maxYRadius); dy <= Math.Min(radius, maxYRadius); dy++)
                    {
                        int x = startX + dx;
                        int y = startY + dy;
                        if (!grid.ContainsKey((x, y)))
                        {
                            return (x, y);
                        }
                        else
                        {
                            var existing = grid[(x, y)];
                            var existingSat = existing.Saturation;
                            var newSat = node.Saturation;
                            var diff = newSat - existingSat;
                            if (newSat - existingSat > 10)
                            {
                                return (x, y);
                            }
                        }
                    }
                }
                radius++;
                if (radius > 5)
                {
                    throw new IndexOutOfRangeException("");
                }
            }
        }
        
        public void CollapseToVerticalCenter(int[,] blocksMap)
        {
            int width = blocksMap.GetLength(0);
            int height = blocksMap.GetLength(1);

            // Collect all non-zero entries by their X column
            List<int>[] columns = new List<int>[width];
            for (int x = 0; x < width; x++)
            {
                columns[x] = new List<int>();
                for (int y = 0; y < height; y++)
                {
                    if (blocksMap[x, y] != 0)
                    {
                        columns[x].Add(blocksMap[x, y]);
                        blocksMap[x, y] = 0; // Clear the original
                    }
                }
            }

            // Now reassign them centered vertically
            for (int x = 0; x < width; x++)
            {
                var column = columns[x];
                int count = column.Count;
                if (count == 0) continue;

                int startY = (height - count) / 2;
                for (int i = 0; i < count; i++)
                {
                    blocksMap[x, startY + i] = column[i];
                }
            }
        }
        
        public static void CollapseToTop(int[,] blocksMap)
        {
            int width = blocksMap.GetLength(0);
            int height = blocksMap.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                int insertY = 0; // Always insert at the topmost available spot
                for (int y = 0; y < height; y++)
                {
                    if (blocksMap[x, y] != 0)
                    {
                        if (insertY != y)
                        {
                            blocksMap[x, insertY] = blocksMap[x, y];
                            blocksMap[x, y] = 0;
                        }
                        insertY++;
                    }
                }
            }
        }

        [TestMethod]
        public async Task Show_CoverageChart()
        {
            #region SETUP
            var palette = MaterialPalette.FromResx();
            var opts = new Options(new MemoryOptionsProvider());
            opts.IsSideView = false;

            foreach (var item in Materials.List.Where(m => !m.IsAdvanced))
            {
                item.IsEnabledF(opts, true);
            }

            opts.IsAdvancedModeEnabled = false;
            var combos = palette.ToValidCombinationList(opts);

            var colorNodesByHue = combos.Select(x =>
            {
                return new ColorNode
                {
                    MaterialPaletteID = palette[x],
                    Color = x.GetAverageColor(opts.IsSideView),
                    Hue = x.GetAverageColor(opts.IsSideView).GetHue(),
                    Brightness = x.GetAverageColor(opts.IsSideView).GetBrightness(),
                    Saturation = x.GetAverageColor(opts.IsSideView).GetSaturation(),
                    mc = x
                };
            })
            .OrderBy(x => x.Hue)
            .ToList()
            ;

            var sameColorNodesByBrightness = colorNodesByHue.OrderByDescending(x => x.Brightness).ToList();

            #endregion SETUP

            // 1. Normalize hue and brightness
            //int estimatedWidth = 100;   // You can tweak
            //int estimatedHeight = 250;  // You can tweak
            int estimatedWidth = 55;   // You can tweak
            int estimatedHeight = 35;  // You can tweak

            double hueNormalizationConstant = ((double)estimatedWidth)/colorNodesByHue.Count;
            double brightnessNormalizationConstant = ((double)estimatedHeight)/sameColorNodesByBrightness.Count;
            // Assume sorted by hue already.
            for (int i = 0; i < colorNodesByHue.Count; i++)
            {
                var node = colorNodesByHue[i];
                node.TargetX = (int)(i * hueNormalizationConstant);
                //node.TargetY = (int)((1.0f - node.Brightness) * estimatedHeight); // Top = bright, Bottom = dark
            }

            for (int i = 0; i < sameColorNodesByBrightness.Count; i++)
            {
                var node = sameColorNodesByBrightness[i];
                //node.TargetY = (int)((1.0f - node.Brightness) * estimatedHeight); // Top = bright, Bottom = dark
                node.TargetY = (int)(i * brightnessNormalizationConstant); // Top = bright, Bottom = dark
            }

            // 2. Sort by TargetY, TargetX
            List<IGrouping<(double TargetX, double TargetY), ColorNode>> colorGroups = colorNodesByHue
                .OrderByDescending(n => n.Saturation) // Prefer saturated colors earlier
                .ThenBy(n => n.mc.IsMultiLayer)
                .ThenBy(n => n.TargetX)
                .ThenBy(n => n.TargetY)
                .GroupBy(n => (n.TargetX, n.TargetY))
                //.OrderByDescending(n => n.Count())
                .ToList();

            // 3. Placement
            var grid = new Dictionary<(int x, int y), ColorNode>();
            List<IGrouping<(double TargetX, double TargetY), ColorNode>> colorGroupsRemaining = new List<IGrouping<(double TargetX, double TargetY), ColorNode>>();
            foreach (var nodeGroup in colorGroups)
            {
                int freeX = (int)nodeGroup.Key.TargetX;
                int freeY = (int)nodeGroup.Key.TargetY;
                grid[(freeX, freeY)] = nodeGroup.OrderByDescending(x => x.Saturation).FirstOrDefault();

                if (nodeGroup.Count() > 1)
                {
                    colorGroupsRemaining.Add(new SimpleGrouping<(double x, double y), ColorNode>((freeX, freeY), nodeGroup.OrderByDescending(x => x.Saturation).Skip(1).ToList()));
                }
            }

            // 4. Catch the stragglers.
            for (int i = 0; i < 4; i++)
            {
                if (colorGroupsRemaining.Any())
                {
                    HashSet<(double x, double y)> noMoreSpaces = new HashSet<(double x, double y)>();
                    foreach (var nodeGroup in colorGroupsRemaining.OrderByDescending(c => c.First().Saturation))
                    {
                        int targetX = (int)nodeGroup.Key.TargetX;
                        int targetY = (int)nodeGroup.Key.TargetY;
                        var nodeToUse = nodeGroup.OrderByDescending(x => x.Saturation).FirstOrDefault();
                        try
                        {
                            var (freeX, freeY) = FindNearestFreeSpot(grid, targetX, targetY, nodeToUse);
                            grid[(freeX, freeY)] = nodeToUse;
                        }
                        catch (IndexOutOfRangeException) {
                            noMoreSpaces.Add((targetX, targetY));
                        }
                    }

                    colorGroupsRemaining = colorGroupsRemaining
                        .Where(x => !noMoreSpaces.Contains(x.Key))
                        .Select(nodeGroup => {
                            var grouping = new SimpleGrouping<(double x, double y), ColorNode>(nodeGroup.Key, nodeGroup.Skip(1));
                            return (IGrouping<(double x, double y), ColorNode>)grouping;
                        })
                        .Where(x => x.Count() > 0)
                        .ToList();
                }
            }

            // 5. Put it into a blocksMap.
            int minX = grid.Keys.Min(p => p.x);
            int maxX = grid.Keys.Max(p => p.x);
            int minY = grid.Keys.Min(p => p.y);
            int maxY = grid.Keys.Max(p => p.y);

            int Width = maxX - minX + 1;
            int Height = maxY - minY + 1;
            var blocksMap = new int[Width, Height];
            foreach (var kvp in grid)
            {
                var coords = kvp.Key;
                int x = coords.x + (0 - minX);
                int y = coords.y + (0 - minY);
                blocksMap[x, y] = kvp.Value.MaterialPaletteID;
            }

            // 6. Collapse it vertically.
            // (Thanks, Chat GPT.)
            //CollapseToVerticalCenter(blocksMap);
            //CollapseToTop(blocksMap);

            #region FINISH
            var data = new CanvasData(palette, blocksMap);

            var pxdat = new PixelStackerProjectData()
            {
                CanvasData = data,
                IsSideView = opts.IsSideView,
                MaterialPalette = palette,
                PreprocessedImage = null,
                WorldEditOrigin = null
            };

            var srs = new CanvasViewerSettings()
            {
                IsShadowRenderingEnabled = false,
                TextureSize = 16,
                IsSolidColors = false,
            };

            IExportFormatter exporter = ExportFormat.Png.GetFormatter();
            if (exporter is IExportImageFormatter)
            {
                var imgExporter = (IExportImageFormatter)exporter;
                byte[] bs = await imgExporter.ExportAsync(pxdat, srs, null);
                await File.WriteAllBytesAsync("output.sm.png", bs);
            } 
            else
            {
                byte[] bs = await exporter.ExportAsync(pxdat, null);
                await File.WriteAllBytesAsync("output.sm.png", bs);
            }
            #endregion FINISH
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1-x2) * (x1-x2) + (y1-y2) * (y1-y2));
        }

        /// <summary>
        ///  TODO: MAke it so sorting happens not only by distance (brightness x hue) but also by saturation. 
        ///  In the middle, saturation should be favored more than brightness. 
        /// </summary>
        /// <param name="allNodes"></param>
        /// <param name="grid"></param>
        /// <param name="gridX"></param>
        /// <param name="gridY"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private bool SetBestMatch(List<ColorNode> allNodes, Dictionary<(int x, int y), ColorNode> grid, int gridX, int gridY, int radius)
        {
            if (grid.ContainsKey((gridX, gridY)))
            {
                return true;
            }

            if (allNodes.Count == 0)
            {
                return false;
            }

            var filtered = allNodes
                .Where(x => !x.IsUsed)
                .Where(n =>
                {
                    var dist = Distance(gridX, gridY, n.TargetX, n.TargetY);
                    return dist <= radius+2;
                })
                .OrderByDescending(n => n.Saturation)
                .ToList()
                ;

            if (filtered.Any())
            {
                var node = filtered.FirstOrDefault();
                node.IsUsed = true;
                grid[(gridX, gridY)] = node;
                return true;
            }

            return false;
        }


        /// <summary>
        /// For each location, see if there is an exact match with 0 radius.
        /// Then, for each unfilled location, see again if there is a match with 1 radius.
        /// Keep going until radius reaches a certain amount.
        /// As you expand outwards from the center, you should prefer blocks most similar to newer location.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Show_CoverageChart_LocationFirst()
        {
            int estimatedWidth = 55;   // You can tweak
            int estimatedHeight = 35;  // You can tweak

            #region SETUP
            var palette = MaterialPalette.FromResx();
            var opts = new Options(new MemoryOptionsProvider());
            opts.IsSideView = false;

            foreach (var item in Materials.List.Where(m => !m.IsAdvanced))
            {
                item.IsEnabledF(opts, true);
            }

            opts.IsAdvancedModeEnabled = false;
            var combos = palette.ToValidCombinationList(opts);

            var colorNodesByHue = combos.Select(x =>
            {
                return new ColorNode
                {
                    MaterialPaletteID = palette[x],
                    Color = x.GetAverageColor(opts.IsSideView),
                    Hue = x.GetAverageColor(opts.IsSideView).GetHue(),
                    Brightness = x.GetAverageColor(opts.IsSideView).GetBrightness(),
                    Saturation = x.GetAverageColor(opts.IsSideView).GetSaturation(),
                    IsUsed = false,
                    mc = x
                };
            })
            .OrderBy(x => x.Hue)
            .ToList()
            ;

            var sameColorNodesByBrightness = colorNodesByHue.OrderByDescending(x => x.Brightness).ToList();
            double hueNormalizationConstant = ((double)estimatedWidth)/colorNodesByHue.Count;
            double brightnessNormalizationConstant = ((double)estimatedHeight)/sameColorNodesByBrightness.Count;

            #endregion SETUP

            // 1. Normalize hue and brightness
            // Assume sorted by hue already.
            for (int i = 0; i < colorNodesByHue.Count; i++)
            {
                var node = colorNodesByHue[i];
                node.TargetX = (int)(i * hueNormalizationConstant);
                //node.TargetY = (int)((1.0f - node.Brightness) * estimatedHeight); // Top = bright, Bottom = dark
            }

            for (int i = 0; i < sameColorNodesByBrightness.Count; i++)
            {
                var node = sameColorNodesByBrightness[i];
                //node.TargetY = (int)((1.0f - node.Brightness) * estimatedHeight); // Top = bright, Bottom = dark
                node.TargetY = (int)(i * brightnessNormalizationConstant); // Top = bright, Bottom = dark
            }


            //int estimatedWidth = 100;   // You can tweak
            //int estimatedHeight = 250;  // You can tweak
            int MAX_RADIUS = 4;
            var grid = new Dictionary<(int x, int y), ColorNode>();

            double maxBrightness = sameColorNodesByBrightness.Max(x => x.Brightness);
            double minBrightness = sameColorNodesByBrightness.Min(x => x.Brightness);
            double rangeBrightness = maxBrightness - minBrightness;

            for (int r = 0; r < MAX_RADIUS; r++)
            {
                bool everythingHasValue = true;
                for (int y = 0; y < estimatedHeight; y++)
                {
                    for (int x = 0; x < estimatedWidth; x++)
                    {
                        everythingHasValue &= SetBestMatch(colorNodesByHue, grid, x, y, r);
                    }
                }

                if (everythingHasValue)
                {
                    break;
                }

                //colorNodesByHue = colorNodesByHue.Where(n => !n.IsUsed).ToList();
                if (colorNodesByHue.Count(n => !n.IsUsed) == 0)
                {
                    break;
                }
            }

            var avgBri = colorNodesByHue.Average(x => x.Brightness);
            var avgSat = colorNodesByHue.Average(x => x.Saturation);


            // 5. Put it into a blocksMap.
            int minX = grid.Keys.Min(p => p.x);
            int maxX = grid.Keys.Max(p => p.x);
            int minY = grid.Keys.Min(p => p.y);
            int maxY = grid.Keys.Max(p => p.y);

            int Width = maxX - minX + 1;
            int Height = maxY - minY + 1;
            var blocksMap = new int[Width, Height];
            foreach (var kvp in grid)
            {
                var coords = kvp.Key;
                int x = coords.x + (0 - minX);
                int y = coords.y + (0 - minY);
                blocksMap[x, y] = kvp.Value.MaterialPaletteID;
            }

            // 6. Collapse it vertically.
            // (Thanks, Chat GPT.)
            //CollapseToVerticalCenter(blocksMap);
            //CollapseToTop(blocksMap);

            #region FINISH
            var data = new CanvasData(palette, blocksMap);

            var pxdat = new PixelStackerProjectData()
            {
                CanvasData = data,
                IsSideView = opts.IsSideView,
                MaterialPalette = palette,
                PreprocessedImage = null,
                WorldEditOrigin = null
            };

            var srs = new CanvasViewerSettings()
            {
                IsShadowRenderingEnabled = false,
                TextureSize = 16,
                IsSolidColors = false,
            };

            IExportFormatter exporter = ExportFormat.Png.GetFormatter();
            if (exporter is IExportImageFormatter)
            {
                var imgExporter = (IExportImageFormatter)exporter;
                byte[] bs = await imgExporter.ExportAsync(pxdat, srs, null);
                await File.WriteAllBytesAsync("output.sm.png", bs);
            }
            else
            {
                byte[] bs = await exporter.ExportAsync(pxdat, null);
                await File.WriteAllBytesAsync("output.sm.png", bs);
            }
            #endregion FINISH
        }


        [TestMethod]
        public async Task Show_ColorsPerHueBucket()
        {
            #region SETUP
            var palette = MaterialPalette.FromResx();
            var opts = new Options(new MemoryOptionsProvider());
            opts.IsSideView = false;

            foreach (var item in Materials.List.Where(m => !m.IsAdvanced))
            {
                item.IsEnabledF(opts, true);
            }

            opts.IsAdvancedModeEnabled = false;
            var combos = palette.ToValidCombinationList(opts);
            #endregion SETUP

            var colorNodes = combos.Select(x =>
            {
                return new ColorNode
                {
                    MaterialPaletteID = palette[x],
                    Color = x.GetAverageColor(opts.IsSideView),
                    Hue = x.GetAverageColor(opts.IsSideView).GetHue(),
                    Brightness = x.GetAverageColor(opts.IsSideView).GetBrightness(),
                    Saturation = x.GetAverageColor(opts.IsSideView).GetSaturation(),
                    mc = x
                };
            }).OrderByColor(x => x.Color);

            // 1. Normalize hue and brightness
            //int estimatedWidth = 100;   // You can tweak
            //int estimatedHeight = 250;  // You can tweak
            int estimatedWidth = 360;   // You can tweak
            int estimatedHeight = 40;  // You can tweak
            foreach (var node in colorNodes)
            {
                double maxHue = 360-45;
                double effectiveHue;
                if (node.Hue < 90) effectiveHue = node.Hue;
                else if (node.Hue < 180) effectiveHue = 90.0f + (node.Hue - 90) / 2;
                else effectiveHue = (node.Hue - 45);

                node.TargetX = (int)((effectiveHue / maxHue) * estimatedWidth);
                node.TargetY = (int)((1.0f - node.Brightness) * estimatedHeight); // Top = bright, Bottom = dark
            }

            var grid = new Dictionary<(int x, int y), ColorNode>();
            var hueGroups = colorNodes.GroupBy(c => { return ((int)c.Hue) / 5; }).ToList();
            for (int x = 0; x < hueGroups.Count; x++)
            {
                var group = hueGroups[x];
                for (int y = 0; y < group.Count(); y++)
                {
                    grid[(x, y)] = group.FirstOrDefault();
                }
            }

            // 5. Put it into a blocksMap.
            int minX = grid.Keys.Min(p => p.x);
            int maxX = grid.Keys.Max(p => p.x);
            int minY = grid.Keys.Min(p => p.y);
            int maxY = grid.Keys.Max(p => p.y);

            int Width = maxX - minX + 1;
            int Height = maxY - minY + 1;
            var blocksMap = new int[Width, Height];
            foreach (var kvp in grid)
            {
                var coords = kvp.Key;
                int x = coords.x + (0 - minX);
                int y = coords.y + (0 - minY);
                blocksMap[x, y] = kvp.Value.MaterialPaletteID;
            }

            // 6. Collapse it vertically.
            // (Thanks, Chat GPT.)
            //CollapseToVerticalCenter(blocksMap);
            CollapseToTop(blocksMap);

            #region FINISH
            var data = new CanvasData(palette, blocksMap);

            var pxdat = new PixelStackerProjectData()
            {
                CanvasData = data,
                IsSideView = opts.IsSideView,
                MaterialPalette = palette,
                PreprocessedImage = null,
                WorldEditOrigin = null
            };

            var srs = new CanvasViewerSettings()
            {
                IsShadowRenderingEnabled = false,
                TextureSize = 16,
                IsSolidColors = false,
            };

            IExportFormatter exporter = ExportFormat.Png.GetFormatter();
            if (exporter is IExportImageFormatter)
            {
                var imgExporter = (IExportImageFormatter)exporter;
                byte[] bs = await imgExporter.ExportAsync(pxdat, srs, null);
                await File.WriteAllBytesAsync("output.sm.png", bs);
            }
            else
            {
                byte[] bs = await exporter.ExportAsync(pxdat, null);
                await File.WriteAllBytesAsync("output.sm.png", bs);
            }
            #endregion FINISH
        }



        [TestMethod]
        public async Task Show_Hsl_Brick()
        {
            int width = 550;
            int height = 350;
            SKBitmap bm = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using SKCanvas canvas = new SKCanvas(bm);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float hue = x * 360.0f / width;
                    float lightness = 100 - y * 100.0f / height;
                    float sat = 100;
                    var c = SKColor.FromHsl(hue, sat, lightness);
                    var satPrime = c.GetSaturation();
                    var briPrime = c.GetBrightness();
                    bm.SetPixel(x, y, c);
                }
            }

            canvas.Save();
            using var ms = new MemoryStream();
            bool isSuccessfulEncode = bm.Encode(ms, SKEncodedImageFormat.Jpeg, 90);
            ms.Seek(0, SeekOrigin.Begin);
            var bs = ms.ToArray();
            await File.WriteAllBytesAsync("hsl_brick.png", bs);
        }


        [TestMethod]
        public async Task Show_sl_Brick()
        {
            int width = 550;
            int height = 350;
            SKBitmap bm = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using SKCanvas canvas = new SKCanvas(bm);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float hue = 0; //  x * 360.0f / width;
                    float lightness = 100 - y * 100.0f / height;
                    float sat = 100 - x * 100.0f / width;
                    var c = SKColor.FromHsl(hue, sat, lightness);
                    var satPrime = c.GetSaturation();
                    var briPrime = c.GetBrightness();
                    bm.SetPixel(x, y, c);
                }
            }

            canvas.Save();
            using var ms = new MemoryStream();
            bool isSuccessfulEncode = bm.Encode(ms, SKEncodedImageFormat.Jpeg, 90);
            ms.Seek(0, SeekOrigin.Begin);
            var bs = ms.ToArray();
            await File.WriteAllBytesAsync("sl_brick.png", bs);
        }
    }
}
