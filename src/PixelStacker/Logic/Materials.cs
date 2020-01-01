using PixelStacker.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PixelStacker.Logic.Extensions;
using System.Threading;
using PixelStacker.Logic.Great;

namespace PixelStacker.Logic
{
    public class Materials
    {
        public static Material Air { get; set; }
        public static Dictionary<Color, Material[]> ColorMap = new Dictionary<Color, Material[]>();
        // Maps best matches to the set of quantized colors available.
        // [Src color] --> [ColorMap.Key]
        public static BestMatchCacheMap BestMatchCache = new BestMatchCacheMap().Load();

        // Color blending is just a linear interpolation per channel, right?
        // So the math is pretty simple. If you have RGBA1 over RGB2, the 
        // effective visual result RGB3 will be:
        // r3 = r2 + (r1 - r2) * a1
        // g3 = g2 + (g1 - g2) * a1
        // b3 = b2 + (b1 - b2) * a1
        private static Color OverlayColor(Color RGBA1_Top, Color RGBA2_Bottom)
        {
            double alpha = Convert.ToDouble(RGBA1_Top.A) / 255;
            int R = (int)((RGBA1_Top.R * alpha) + (RGBA2_Bottom.R * (1.0 - alpha)));
            int G = (int)((RGBA1_Top.G * alpha) + (RGBA2_Bottom.G * (1.0 - alpha)));
            int B = (int)((RGBA1_Top.B * alpha) + (RGBA2_Bottom.B * (1.0 - alpha)));
            return Color.FromArgb(255, R, G, B);
        }

        public static void CompileColorMap(CancellationToken worker, bool isClearBestMatchCache)
        {
            int n = 0;
            int maxN = Materials.List.Where(x => x.IsEnabled).Count();
            if (Options.Get.IsMultiLayer) maxN *= 16;
            Dictionary<Color, List<Material[]>> collisions = new Dictionary<Color, List<Material[]>>();

            bool isSide = Options.Get.IsSideView;
            bool isMultiLayer = Options.Get.IsMultiLayer;
            if (isClearBestMatchCache) { BestMatchCache.Clear(); }
            TaskManager.SafeReport(0, "Compiling Color Map based on selected materials.");
            Air = Materials.List.FirstOrDefault(m => m.Label == "Air");

            ColorMap.Clear();
            var categoriesSelected = Materials.List.Where(m2 => m2.IsEnabled && m2.Category != "Air").Select(m2 => m2.Category).Distinct();
            if (categoriesSelected.Count() == 1 && categoriesSelected.FirstOrDefault() == "Glass")
            {
                foreach (Material m in Materials.List.Where(m2 => m2.IsEnabled && m2.Category == "Glass" && m2.Category != "Air"))
                {
                    Color cAvg = m.getAverageColor(isSide);
                    ColorMap[cAvg] = new Material[1] { m };
                    if (n++ % 30 == 0)
                    {
                        TaskManager.SafeReport(100 * n / maxN, "Compiling color map...");
                        if (worker.SafeIsCancellationRequested())
                        {
                            ColorMap.Clear();
                            if (isClearBestMatchCache) { BestMatchCache.Clear(); }
                            worker.SafeThrowIfCancellationRequested();
                        }
                    }
                }
            }
            else
            {
                foreach (Material m in Materials.List.Where(m2 => m2.IsEnabled && m2.Category != "Glass" && m2.Category != "Air"))
                {
                    Color cAvg = m.getAverageColor(isSide);
                    ColorMap[cAvg] = new Material[1] { m };
                    if (n++ % 30 == 0)
                    {
                        TaskManager.SafeReport(100 * n / maxN, "Compiling color map...");
                        if (worker.SafeIsCancellationRequested())
                        {
                            ColorMap.Clear();
                            if (isClearBestMatchCache) { BestMatchCache.Clear(); }
                            worker.SafeThrowIfCancellationRequested();
                        }
                    }
                }
            }

            if (isMultiLayer)
            {
                Dictionary<Color, Material[]> toAdd = new Dictionary<Color, Material[]>();
                List<Material> glasses = Materials.List.Where(m2 => m2.Category == "Glass" && m2.IsEnabled).ToList();
                foreach (Material[] mArr in ColorMap.Values.Where(cm => cm.Length == 1))
                {
                    foreach (Material glassM in glasses)
                    {
                        Color combinedColor = OverlayColor(glassM.getAverageColor(isSide), mArr[0].getAverageColor(isSide));

                        Material[] matMap = new Material[mArr.Length + 1];
                        for (int i = 0; i < mArr.Length; i++)
                        {
                            matMap[i] = mArr[i];
                        }
                        matMap[matMap.Length - 1] = glassM;

                        // Prefer single layer versions of same color.
                        if (!ColorMap.ContainsKey(combinedColor))
                        {
                            toAdd[combinedColor] = matMap;
                        }

                        if (n++ % 30 == 0)
                        {
                            TaskManager.SafeReport(100 * n / maxN, "Compiling color map...");
                            if (worker.SafeIsCancellationRequested())
                            {
                                ColorMap.Clear();
                                if (isClearBestMatchCache) { BestMatchCache.Clear(); }
                                worker.SafeThrowIfCancellationRequested();
                            }
                        }
                    }
                }

                foreach (Color c in toAdd.Keys)
                {
                    if (!ColorMap.ContainsKey(c))
                    {
                        ColorMap[c] = toAdd[c];
                    }
                }

                toAdd.Clear();
            }

            ColorMap[Air.getAverageColor(isSide)] = new Material[1] { Air };
            ColorMap[Color.FromArgb(0, 255, 255, 255)] = new Material[1] { Air };
            TaskManager.SafeReport(100, "Color map finished compiling");
        }

        public static Color? FindBestMatch(List<Color> colors, Color toMatch)
        {
            Color? bestMatch = null;
            int bestDiff = int.MaxValue;

            if (BestMatchCache.ContainsKey(toMatch))
            {
                return BestMatchCache[toMatch];
            }

            for (int i = 0; i < colors.Count; i++)
            {
                if (colors[i].ToArgb() != 16777215)
                {
                    Color c = colors[i];

                    if (bestMatch == null)
                    {
                        bestMatch = c;
                    }


                    int diff = int.MaxValue;

                    try
                    {
                        float diffd = c.GetColorDistance(toMatch);
                        diff = Convert.ToInt32(diffd);
                    }
                    catch (OverflowException) { }

                    if (diff < bestDiff)
                    {
                        bestMatch = c;
                        bestDiff = diff;
                    }
                }
            }

            var rt = (bestMatch ?? Materials.Air.getAverageColor(true));
            BestMatchCache.Add(toMatch, rt);

            return rt;
        }


        public static List<Color> FindBestMatches(List<Color> colors, Color toMatch, int top)
        {
            var rt = colors
            .Where(c => c.ToArgb() != 16777215)
            .OrderBy(c =>
            {
                float diffd = c.GetColorDistance(toMatch);


                return diffd;
            }).Take(top).ToList();
            return rt;
        }

        private static List<Material> _List = null;
        public static List<Material> List
        {
            get
            {
                if (_List == null)
                {
                    _List = new List<Material>()
                    {
                        new Material("Air", "AIR_00", "Air", 0, 0, Textures.air, Textures.air, $"minecraft:{nameof(Textures.air)}", $"minecraft:{nameof(Textures.air)}", "minecraft:air" ),

                        new Material("Glass", "GLASS_00", "White Glass", 95, 0, Textures.white_stained_glass, Textures.white_stained_glass, $"minecraft:{nameof(Textures.white_stained_glass)}", $"minecraft:{nameof(Textures.white_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_01", "Orange Glass", 95, 1, Textures.orange_stained_glass, Textures.orange_stained_glass, $"minecraft:{nameof(Textures.orange_stained_glass)}", $"minecraft:{nameof(Textures.orange_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_02", "Magenta Glass", 95, 2, Textures.magenta_stained_glass, Textures.magenta_stained_glass, $"minecraft:{nameof(Textures.magenta_stained_glass)}", $"minecraft:{nameof(Textures.magenta_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_03", "Light Blue Glass", 95, 3, Textures.light_blue_stained_glass, Textures.light_blue_stained_glass, $"minecraft:{nameof(Textures.light_blue_stained_glass)}", $"minecraft:{nameof(Textures.light_blue_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_04", "Yellow Glass", 95, 4, Textures.yellow_stained_glass, Textures.yellow_stained_glass, $"minecraft:{nameof(Textures.yellow_stained_glass)}", $"minecraft:{nameof(Textures.yellow_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_05", "Lime Glass", 95, 5, Textures.lime_stained_glass, Textures.lime_stained_glass, $"minecraft:{nameof(Textures.lime_stained_glass)}", $"minecraft:{nameof(Textures.lime_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_06", "Pink Glass", 95, 6, Textures.pink_stained_glass, Textures.pink_stained_glass, $"minecraft:{nameof(Textures.pink_stained_glass)}", $"minecraft:{nameof(Textures.pink_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_07", "Gray Glass", 95, 7, Textures.gray_stained_glass, Textures.gray_stained_glass, $"minecraft:{nameof(Textures.gray_stained_glass)}", $"minecraft:{nameof(Textures.gray_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_08", "Light Gray Glass", 95, 8, Textures.light_gray_stained_glass, Textures.light_gray_stained_glass, $"minecraft:{nameof(Textures.light_gray_stained_glass)}", $"minecraft:{nameof(Textures.light_gray_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_09", "Cyan Glass", 95, 9, Textures.cyan_stained_glass, Textures.cyan_stained_glass, $"minecraft:{nameof(Textures.cyan_stained_glass)}", $"minecraft:{nameof(Textures.cyan_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_10", "Purple Glass", 95, 10, Textures.purple_stained_glass, Textures.purple_stained_glass, $"minecraft:{nameof(Textures.purple_stained_glass)}", $"minecraft:{nameof(Textures.purple_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_11", "Blue Glass", 95, 11, Textures.blue_stained_glass, Textures.blue_stained_glass, $"minecraft:{nameof(Textures.blue_stained_glass)}", $"minecraft:{nameof(Textures.blue_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_12", "Brown Glass", 95, 12, Textures.brown_stained_glass, Textures.brown_stained_glass, $"minecraft:{nameof(Textures.brown_stained_glass)}", $"minecraft:{nameof(Textures.brown_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_13", "Green Glass", 95, 13, Textures.green_stained_glass, Textures.green_stained_glass, $"minecraft:{nameof(Textures.green_stained_glass)}", $"minecraft:{nameof(Textures.green_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_14", "Red Glass", 95, 14, Textures.red_stained_glass, Textures.red_stained_glass, $"minecraft:{nameof(Textures.red_stained_glass)}", $"minecraft:{nameof(Textures.red_stained_glass)}", "minecraft:stained_glass" ),
                        new Material("Glass", "GLASS_15", "Black Glass", 95, 15, Textures.black_stained_glass, Textures.black_stained_glass, $"minecraft:{nameof(Textures.black_stained_glass)}", $"minecraft:{nameof(Textures.black_stained_glass)}", "minecraft:stained_glass" ),

                        new Material("Wool", "WOOL_00", "White Wool", 35, 0, Textures.white_wool, Textures.white_wool, $"minecraft:{nameof(Textures.white_wool)}", $"minecraft:{nameof(Textures.white_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_01", "Orange Wool", 35, 1, Textures.orange_wool, Textures.orange_wool, $"minecraft:{nameof(Textures.orange_wool)}", $"minecraft:{nameof(Textures.orange_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_02", "Magenta Wool", 35, 2, Textures.magenta_wool, Textures.magenta_wool, $"minecraft:{nameof(Textures.magenta_wool)}", $"minecraft:{nameof(Textures.magenta_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_03", "Light Blue Wool", 35, 3, Textures.light_blue_wool, Textures.light_blue_wool, $"minecraft:{nameof(Textures.light_blue_wool)}", $"minecraft:{nameof(Textures.light_blue_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_04", "Yellow Wool", 35, 4, Textures.yellow_wool, Textures.yellow_wool, $"minecraft:{nameof(Textures.yellow_wool)}", $"minecraft:{nameof(Textures.yellow_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_05", "Lime Wool", 35, 5, Textures.lime_wool, Textures.lime_wool, $"minecraft:{nameof(Textures.lime_wool)}", $"minecraft:{nameof(Textures.lime_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_06", "Pink Wool", 35, 6, Textures.pink_wool, Textures.pink_wool, $"minecraft:{nameof(Textures.pink_wool)}", $"minecraft:{nameof(Textures.pink_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_07", "Gray Wool", 35, 7, Textures.gray_wool, Textures.gray_wool, $"minecraft:{nameof(Textures.gray_wool)}", $"minecraft:{nameof(Textures.gray_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_08", "Light Gray Wool", 35, 8, Textures.light_gray_wool, Textures.light_gray_wool, $"minecraft:{nameof(Textures.light_gray_wool)}", $"minecraft:{nameof(Textures.light_gray_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_09", "Cyan Wool", 35, 9, Textures.cyan_wool, Textures.cyan_wool, $"minecraft:{nameof(Textures.cyan_wool)}", $"minecraft:{nameof(Textures.cyan_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_10", "Purple Wool", 35, 10, Textures.purple_wool, Textures.purple_wool, $"minecraft:{nameof(Textures.purple_wool)}", $"minecraft:{nameof(Textures.purple_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_11", "Blue Wool", 35, 11, Textures.blue_wool, Textures.blue_wool, $"minecraft:{nameof(Textures.blue_wool)}", $"minecraft:{nameof(Textures.blue_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_12", "Brown Wool", 35, 12, Textures.brown_wool, Textures.brown_wool, $"minecraft:{nameof(Textures.brown_wool)}", $"minecraft:{nameof(Textures.brown_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_13", "Green Wool", 35, 13, Textures.green_wool, Textures.green_wool, $"minecraft:{nameof(Textures.green_wool)}", $"minecraft:{nameof(Textures.green_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_14", "Red Wool", 35, 14, Textures.red_wool, Textures.red_wool, $"minecraft:{nameof(Textures.red_wool)}", $"minecraft:{nameof(Textures.red_wool)}", "minecraft:wool" ),
                        new Material("Wool", "WOOL_15", "Black Wool", 35, 15, Textures.black_wool, Textures.black_wool, $"minecraft:{nameof(Textures.black_wool)}", $"minecraft:{nameof(Textures.black_wool)}", "minecraft:wool" ),

                        new Material("Concrete", "CONC_00", "White Concrete", 251, 0, Textures.white_concrete, Textures.white_concrete, $"minecraft:{nameof(Textures.white_concrete)}", $"minecraft:{nameof(Textures.white_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_01", "Orange Concrete", 251, 1, Textures.orange_concrete, Textures.orange_concrete, $"minecraft:{nameof(Textures.orange_concrete)}", $"minecraft:{nameof(Textures.orange_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_02", "Magenta Concrete", 251, 2, Textures.magenta_concrete, Textures.magenta_concrete, $"minecraft:{nameof(Textures.magenta_concrete)}", $"minecraft:{nameof(Textures.magenta_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_03", "Light Blue Concrete", 251, 3, Textures.light_blue_concrete, Textures.light_blue_concrete, $"minecraft:{nameof(Textures.light_blue_concrete)}", $"minecraft:{nameof(Textures.light_blue_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_04", "Yellow Concrete", 251, 4, Textures.yellow_concrete, Textures.yellow_concrete, $"minecraft:{nameof(Textures.yellow_concrete)}", $"minecraft:{nameof(Textures.yellow_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_05", "Lime Concrete", 251, 5, Textures.lime_concrete, Textures.lime_concrete, $"minecraft:{nameof(Textures.lime_concrete)}", $"minecraft:{nameof(Textures.lime_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_06", "Pink Concrete", 251, 6, Textures.pink_concrete, Textures.pink_concrete, $"minecraft:{nameof(Textures.pink_concrete)}", $"minecraft:{nameof(Textures.pink_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_07", "Gray Concrete", 251, 7, Textures.gray_concrete, Textures.gray_concrete, $"minecraft:{nameof(Textures.gray_concrete)}", $"minecraft:{nameof(Textures.gray_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_08", "Light Gray Concrete", 251, 8, Textures.light_gray_concrete, Textures.light_gray_concrete, $"minecraft:{nameof(Textures.light_gray_concrete)}", $"minecraft:{nameof(Textures.light_gray_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_09", "Cyan Concrete", 251, 9, Textures.cyan_concrete, Textures.cyan_concrete, $"minecraft:{nameof(Textures.cyan_concrete)}", $"minecraft:{nameof(Textures.cyan_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_10", "Purple Concrete", 251, 10, Textures.purple_concrete, Textures.purple_concrete, $"minecraft:{nameof(Textures.purple_concrete)}", $"minecraft:{nameof(Textures.purple_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_11", "Blue Concrete", 251, 11, Textures.blue_concrete, Textures.blue_concrete, $"minecraft:{nameof(Textures.blue_concrete)}", $"minecraft:{nameof(Textures.blue_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_12", "Brown Concrete", 251, 12, Textures.brown_concrete, Textures.brown_concrete, $"minecraft:{nameof(Textures.brown_concrete)}", $"minecraft:{nameof(Textures.brown_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_13", "Green Concrete", 251, 13, Textures.green_concrete, Textures.green_concrete, $"minecraft:{nameof(Textures.green_concrete)}", $"minecraft:{nameof(Textures.green_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_14", "Red Concrete", 251, 14, Textures.red_concrete, Textures.red_concrete, $"minecraft:{nameof(Textures.red_concrete)}", $"minecraft:{nameof(Textures.red_concrete)}", "mineraft:concrete" ),
                        new Material("Concrete", "CONC_15", "Black Concrete", 251, 15, Textures.black_concrete, Textures.black_concrete, $"minecraft:{nameof(Textures.black_concrete)}", $"minecraft:{nameof(Textures.black_concrete)}", "mineraft:concrete" ),

                        new Material("Powder", "PWDR_00", "White Powder", 252, 0, Textures.white_concrete_powder, Textures.white_concrete_powder, $"minecraft:{nameof(Textures.white_concrete_powder)}", $"minecraft:{nameof(Textures.white_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_01", "Orange Powder", 252, 1, Textures.orange_concrete_powder, Textures.orange_concrete_powder, $"minecraft:{nameof(Textures.orange_concrete_powder)}", $"minecraft:{nameof(Textures.orange_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_02", "Magenta Powder", 252, 2, Textures.magenta_concrete_powder, Textures.magenta_concrete_powder, $"minecraft:{nameof(Textures.magenta_concrete_powder)}", $"minecraft:{nameof(Textures.magenta_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_03", "Light Blue Powder", 252, 3, Textures.light_blue_concrete_powder, Textures.light_blue_concrete_powder, $"minecraft:{nameof(Textures.light_blue_concrete_powder)}", $"minecraft:{nameof(Textures.light_blue_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_04", "Yellow Powder", 252, 4, Textures.yellow_concrete_powder, Textures.yellow_concrete_powder, $"minecraft:{nameof(Textures.yellow_concrete_powder)}", $"minecraft:{nameof(Textures.yellow_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_05", "Lime Powder", 252, 5, Textures.lime_concrete_powder, Textures.lime_concrete_powder, $"minecraft:{nameof(Textures.lime_concrete_powder)}", $"minecraft:{nameof(Textures.lime_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_06", "Pink Powder", 252, 6, Textures.pink_concrete_powder, Textures.pink_concrete_powder, $"minecraft:{nameof(Textures.pink_concrete_powder)}", $"minecraft:{nameof(Textures.pink_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_07", "Gray Powder", 252, 7, Textures.gray_concrete_powder, Textures.gray_concrete_powder, $"minecraft:{nameof(Textures.gray_concrete_powder)}", $"minecraft:{nameof(Textures.gray_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_08", "Light Gray Powder", 252, 8, Textures.light_gray_concrete_powder, Textures.light_gray_concrete_powder, $"minecraft:{nameof(Textures.light_gray_concrete_powder)}", $"minecraft:{nameof(Textures.light_gray_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_09", "Cyan Powder", 252, 9, Textures.cyan_concrete_powder, Textures.cyan_concrete_powder, $"minecraft:{nameof(Textures.cyan_concrete_powder)}", $"minecraft:{nameof(Textures.cyan_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_10", "Purple Powder", 252, 10, Textures.purple_concrete_powder, Textures.purple_concrete_powder, $"minecraft:{nameof(Textures.purple_concrete_powder)}", $"minecraft:{nameof(Textures.purple_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_11", "Blue Powder", 252, 11, Textures.blue_concrete_powder, Textures.blue_concrete_powder, $"minecraft:{nameof(Textures.blue_concrete_powder)}", $"minecraft:{nameof(Textures.blue_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_12", "Brown Powder", 252, 12, Textures.brown_concrete_powder, Textures.brown_concrete_powder, $"minecraft:{nameof(Textures.brown_concrete_powder)}", $"minecraft:{nameof(Textures.brown_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_13", "Green Powder", 252, 13, Textures.green_concrete_powder, Textures.green_concrete_powder, $"minecraft:{nameof(Textures.green_concrete_powder)}", $"minecraft:{nameof(Textures.green_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_14", "Red Powder", 252, 14, Textures.red_concrete_powder, Textures.red_concrete_powder, $"minecraft:{nameof(Textures.red_concrete_powder)}", $"minecraft:{nameof(Textures.red_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "PWDR_15", "Black Powder", 252, 15, Textures.black_concrete_powder, Textures.black_concrete_powder, $"minecraft:{nameof(Textures.black_concrete_powder)}", $"minecraft:{nameof(Textures.black_concrete_powder)}", "mineraft:concrete_powder" ),
                        new Material("Powder", "SAND_00", "zzSand", 12, 0, Textures.sand, Textures.sand, $"minecraft:{nameof(Textures.sand)}", $"minecraft:{nameof(Textures.sand)}", "minecraft:sand" ),
                        new Material("Powder", "SAND_01", "zzSand Red", 12, 1, Textures.red_sand, Textures.red_sand, $"minecraft:{nameof(Textures.red_sand)}", $"minecraft:{nameof(Textures.red_sand)}", "minecraft:sand" ),

                        new Material("Clay", "TERRA_00", "White Clay", 159, 0, Textures.white_terracotta, Textures.white_terracotta, $"minecraft:{nameof(Textures.white_terracotta)}", $"minecraft:{nameof(Textures.white_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_01", "Orange Clay", 159, 1, Textures.orange_terracotta, Textures.orange_terracotta, $"minecraft:{nameof(Textures.orange_terracotta)}", $"minecraft:{nameof(Textures.orange_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_02", "Magenta Clay", 159, 2, Textures.magenta_terracotta, Textures.magenta_terracotta, $"minecraft:{nameof(Textures.magenta_terracotta)}", $"minecraft:{nameof(Textures.magenta_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_03", "Light Blue Clay", 159, 3, Textures.light_blue_terracotta, Textures.light_blue_terracotta, $"minecraft:{nameof(Textures.light_blue_terracotta)}", $"minecraft:{nameof(Textures.light_blue_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_04", "Yellow Clay", 159, 4, Textures.yellow_terracotta, Textures.yellow_terracotta, $"minecraft:{nameof(Textures.yellow_terracotta)}", $"minecraft:{nameof(Textures.yellow_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_05", "Lime Clay", 159, 5, Textures.lime_terracotta, Textures.lime_terracotta, $"minecraft:{nameof(Textures.lime_terracotta)}", $"minecraft:{nameof(Textures.lime_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_06", "Pink Clay", 159, 6, Textures.pink_terracotta, Textures.pink_terracotta, $"minecraft:{nameof(Textures.pink_terracotta)}", $"minecraft:{nameof(Textures.pink_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_07", "Gray Clay", 159, 7, Textures.gray_terracotta, Textures.gray_terracotta, $"minecraft:{nameof(Textures.gray_terracotta)}", $"minecraft:{nameof(Textures.gray_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_08", "Light Gray Clay", 159, 8, Textures.light_gray_terracotta, Textures.light_gray_terracotta, $"minecraft:{nameof(Textures.light_gray_terracotta)}", $"minecraft:{nameof(Textures.light_gray_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_09", "Cyan Clay", 159, 9, Textures.cyan_terracotta, Textures.cyan_terracotta, $"minecraft:{nameof(Textures.cyan_terracotta)}", $"minecraft:{nameof(Textures.cyan_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_10", "Purple Clay", 159, 10, Textures.purple_terracotta, Textures.purple_terracotta, $"minecraft:{nameof(Textures.purple_terracotta)}", $"minecraft:{nameof(Textures.purple_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_11", "Blue Clay", 159, 11, Textures.blue_terracotta, Textures.blue_terracotta, $"minecraft:{nameof(Textures.blue_terracotta)}", $"minecraft:{nameof(Textures.blue_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_12", "Brown Clay", 159, 12, Textures.brown_terracotta, Textures.brown_terracotta, $"minecraft:{nameof(Textures.brown_terracotta)}", $"minecraft:{nameof(Textures.brown_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_13", "Green Clay", 159, 13, Textures.green_terracotta, Textures.green_terracotta, $"minecraft:{nameof(Textures.green_terracotta)}", $"minecraft:{nameof(Textures.green_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_14", "Red Clay", 159, 14, Textures.red_terracotta, Textures.red_terracotta, $"minecraft:{nameof(Textures.red_terracotta)}", $"minecraft:{nameof(Textures.red_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "TERRA_15", "Black Clay", 159, 15, Textures.black_terracotta, Textures.black_terracotta, $"minecraft:{nameof(Textures.black_terracotta)}", $"minecraft:{nameof(Textures.black_terracotta)}", "minecraft:stained_hardened_clay" ),
                        new Material("Clay", "CLAY_HARD_00", "zzHardened Clay", 172, 0, Textures.terracotta, Textures.terracotta, $"minecraft:{nameof(Textures.terracotta)}", $"minecraft:{nameof(Textures.terracotta)}", "minecraft:hardened_clay" ),
                        new Material("Clay", "CLAY_SOFT_00", "zzClay", 82, 0, Textures.clay, Textures.clay, $"minecraft:{nameof(Textures.clay)}", $"minecraft:{nameof(Textures.clay)}", "minecraft:clay" ),

                        new Material("Terracotta", "GLAZED_00", "White Terracotta", 235, 0, Textures.white_glazed_terracotta, Textures.white_glazed_terracotta, $"minecraft:{nameof(Textures.white_glazed_terracotta)}", $"minecraft:{nameof(Textures.white_glazed_terracotta)}", "minecraft:white_glazed_terracotta" ),
                        new Material("Terracotta", "GLAZED_01", "Orange Terracotta", 236, 0, Textures.orange_glazed_terracotta, Textures.orange_glazed_terracotta, $"minecraft:{nameof(Textures.orange_glazed_terracotta)}", $"minecraft:{nameof(Textures.orange_glazed_terracotta)}", "minecraft:orange_glazed_terracotta" ),
                        new Material("Terracotta", "GLAZED_02", "Magenta Terracotta", 237, 0, Textures.magenta_glazed_terracotta, Textures.magenta_glazed_terracotta, $"minecraft:{nameof(Textures.magenta_glazed_terracotta)}", $"minecraft:{nameof(Textures.magenta_glazed_terracotta)}", "minecraft:magenta_glazed_terracotta" ),
                        new Material("Terracotta", "GLAZED_03", "Light Blue Terracotta", 238, 0, Textures.light_blue_glazed_terracotta, Textures.light_blue_glazed_terracotta, $"minecraft:{nameof(Textures.light_blue_glazed_terracotta)}", $"minecraft:{nameof(Textures.light_blue_glazed_terracotta)}", "minecraft:light_blue_glazed_terracotta" ),
                        new Material("Terracotta", "GLAZED_04", "Yellow Terracotta", 239, 0, Textures.yellow_glazed_terracotta, Textures.yellow_glazed_terracotta, $"minecraft:{nameof(Textures.yellow_glazed_terracotta)}", $"minecraft:{nameof(Textures.yellow_glazed_terracotta)}", "minecraft:yellow_glazed_terracotta" ),
                        new Material("Terracotta", "GLAZED_05", "Lime Terracotta", 240, 0, Textures.lime_glazed_terracotta, Textures.lime_glazed_terracotta, $"minecraft:{nameof(Textures.lime_glazed_terracotta)}", $"minecraft:{nameof(Textures.lime_glazed_terracotta)}", "minecraft:lime_glazed_terracotta" ),
                        new Material("Terracotta", "GLAZED_06", "Pink Terracotta", 241, 0, Textures.pink_glazed_terracotta, Textures.pink_glazed_terracotta, $"minecraft:{nameof(Textures.pink_glazed_terracotta)}", $"minecraft:{nameof(Textures.pink_glazed_terracotta)}", "minecraft:pink_glazed_terracotta" ),
                        new Material("Terracotta", "GLAZED_07", "Gray Terracotta", 242, 0, Textures.gray_glazed_terracotta, Textures.gray_glazed_terracotta, $"minecraft:{nameof(Textures.gray_glazed_terracotta)}", $"minecraft:{nameof(Textures.gray_glazed_terracotta)}", "minecraft:gray_glazed_terracotta" ),
                        new Material("Terracotta", "GLAZED_08", "Light Gray Terracotta", 243, 0, Textures.light_gray_glazed_terracotta, Textures.light_gray_glazed_terracotta, $"minecraft:{nameof(Textures.light_gray_glazed_terracotta)}", $"minecraft:{nameof(Textures.light_gray_glazed_terracotta)}", "minecraft:silver_glazed_terracotta"),
                        new Material("Terracotta", "GLAZED_09", "Cyan Terracotta", 244, 0, Textures.cyan_glazed_terracotta, Textures.cyan_glazed_terracotta, $"minecraft:{nameof(Textures.cyan_glazed_terracotta)}", $"minecraft:{nameof(Textures.cyan_glazed_terracotta)}", "minecraft:cyan_glazed_terracotta"),
                        new Material("Terracotta", "GLAZED_10", "Purple Terracotta", 245, 0, Textures.purple_glazed_terracotta, Textures.purple_glazed_terracotta, $"minecraft:{nameof(Textures.purple_glazed_terracotta)}", $"minecraft:{nameof(Textures.purple_glazed_terracotta)}", "minecraft:purple_glazed_terracotta"),
                        new Material("Terracotta", "GLAZED_11", "Blue Terracotta", 246, 0, Textures.blue_glazed_terracotta, Textures.blue_glazed_terracotta, $"minecraft:{nameof(Textures.blue_glazed_terracotta)}", $"minecraft:{nameof(Textures.blue_glazed_terracotta)}", "minecraft:blue_glazed_terracotta"),
                        new Material("Terracotta", "GLAZED_12", "Brown Terracotta", 247, 0, Textures.brown_glazed_terracotta, Textures.brown_glazed_terracotta, $"minecraft:{nameof(Textures.brown_glazed_terracotta)}", $"minecraft:{nameof(Textures.brown_glazed_terracotta)}", "minecraft:brown_glazed_terracotta"),
                        new Material("Terracotta", "GLAZED_13", "Green Terracotta", 248, 0, Textures.green_glazed_terracotta, Textures.green_glazed_terracotta, $"minecraft:{nameof(Textures.green_glazed_terracotta)}", $"minecraft:{nameof(Textures.green_glazed_terracotta)}", "minecraft:green_glazed_terracotta"),
                        new Material("Terracotta", "GLAZED_14", "Red Terracotta", 249, 0, Textures.red_glazed_terracotta, Textures.red_glazed_terracotta, $"minecraft:{nameof(Textures.red_glazed_terracotta)}", $"minecraft:{nameof(Textures.red_glazed_terracotta)}", "minecraft:red_glazed_terracotta"),
                        new Material("Terracotta", "GLAZED_15", "Black Terracotta", 250, 0, Textures.black_glazed_terracotta, Textures.black_glazed_terracotta, $"minecraft:{nameof(Textures.black_glazed_terracotta)}", $"minecraft:{nameof(Textures.black_glazed_terracotta)}", "minecraft:black_glazed_terracotta"),

                        new Material("Planks", "PLANK_00", "Planks Oak", 5, 0, Textures.oak_planks, Textures.oak_planks, $"minecraft:{nameof(Textures.oak_planks)}", $"minecraft:{nameof(Textures.oak_planks)}", "minecraft:planks"),
                        new Material("Planks", "PLANK_01", "Planks Spruce", 5, 1, Textures.spruce_planks, Textures.spruce_planks, $"minecraft:{nameof(Textures.spruce_planks)}", $"minecraft:{nameof(Textures.spruce_planks)}", "minecraft:planks"),
                        new Material("Planks", "PLANK_02", "Planks Birch", 5, 2, Textures.birch_planks, Textures.birch_planks, $"minecraft:{nameof(Textures.birch_planks)}", $"minecraft:{nameof(Textures.birch_planks)}", "minecraft:planks"),
                        new Material("Planks", "PLANK_03", "Planks Jungle", 5, 3, Textures.jungle_planks, Textures.jungle_planks, $"minecraft:{nameof(Textures.jungle_planks)}", $"minecraft:{nameof(Textures.jungle_planks)}", "minecraft:planks"),
                        new Material("Planks", "PLANK_04", "Planks Acacia", 5, 4, Textures.acacia_planks, Textures.acacia_planks, $"minecraft:{nameof(Textures.acacia_planks)}", $"minecraft:{nameof(Textures.acacia_planks)}", "minecraft:planks"),
                        new Material("Planks", "PLANK_05", "Planks Dark Oak", 5, 5, Textures.dark_oak_planks, Textures.dark_oak_planks, $"minecraft:{nameof(Textures.dark_oak_planks)}", $"minecraft:{nameof(Textures.dark_oak_planks)}", "minecraft:planks"),

                        new Material("Wood", "STRIP_LOG_ACA", "Stripped Acacia", 17, 0, Textures.stripped_acacia_log, Textures.stripped_acacia_log, $"minecraft:{nameof(Textures.stripped_acacia_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_acacia_log)}[axis=x]", ""),
                        new Material("Wood", "STRIP_LOG_BIR", "Stripped Birch", 17, 0, Textures.stripped_birch_log, Textures.stripped_birch_log, $"minecraft:{nameof(Textures.stripped_birch_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_birch_log)}[axis=x]", ""),
                        new Material("Wood", "STRIP_LOG_DOK", "Stripped Dark Oak", 17, 0, Textures.stripped_dark_oak_log, Textures.stripped_dark_oak_log, $"minecraft:{nameof(Textures.stripped_dark_oak_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_dark_oak_log)}[axis=x]", ""),
                        new Material("Wood", "STRIP_LOG_JUN", "Stripped Jungle", 17, 0, Textures.stripped_jungle_log, Textures.stripped_jungle_log, $"minecraft:{nameof(Textures.stripped_jungle_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_jungle_log)}[axis=x]", ""),
                        new Material("Wood", "STRIP_LOG_OAK", "Stripped Oak", 17, 0, Textures.stripped_oak_log, Textures.stripped_oak_log, $"minecraft:{nameof(Textures.stripped_oak_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_oak_log)}[axis=x]", ""),
                        new Material("Wood", "STRIP_LOG_SPR", "Stripped Spruce", 17, 0, Textures.stripped_spruce_log, Textures.stripped_spruce_log, $"minecraft:{nameof(Textures.stripped_spruce_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_spruce_log)}[axis=x]", ""),
                        new Material("Wood", "BARK_LOG_ACA", "Bark Acacia", 17, 0, Textures.acacia_log, Textures.acacia_log, $"minecraft:{nameof(Textures.acacia_log)}[axis=x]", $"minecraft:{nameof(Textures.acacia_log)}[axis=x]", ""),
                        new Material("Wood", "BARK_LOG_BIR", "Bark Birch", 17, 0, Textures.birch_log, Textures.birch_log, $"minecraft:{nameof(Textures.birch_log)}[axis=x]", $"minecraft:{nameof(Textures.birch_log)}[axis=x]", ""),
                        new Material("Wood", "BARK_LOG_DOK", "Bark Dark Oak", 17, 0, Textures.dark_oak_log, Textures.dark_oak_log, $"minecraft:{nameof(Textures.dark_oak_log)}[axis=x]", $"minecraft:{nameof(Textures.dark_oak_log)}[axis=x]", ""),
                        new Material("Wood", "BARK_LOG_JUN", "Bark Jungle", 17, 0, Textures.jungle_log, Textures.jungle_log, $"minecraft:{nameof(Textures.jungle_log)}[axis=x]", $"minecraft:{nameof(Textures.jungle_log)}[axis=x]", ""),
                        new Material("Wood", "BARK_LOG_OAK", "Bark Oak", 17, 0, Textures.oak_log, Textures.oak_log, $"minecraft:{nameof(Textures.oak_log)}[axis=x]", $"minecraft:{nameof(Textures.oak_log)}[axis=x]", ""),
                        new Material("Wood", "BARK_LOG_SPR", "Bark Spruce", 17, 0, Textures.spruce_log, Textures.spruce_log, $"minecraft:{nameof(Textures.spruce_log)}[axis=x]", $"minecraft:{nameof(Textures.spruce_log)}[axis=x]", ""),

                        new Material("Good", "SHROOM_BROWN", "Brown Mushroom", 99, 14, Textures.brown_mushroom_block, Textures.brown_mushroom_block, $"minecraft:{nameof(Textures.brown_mushroom_block)}[down=true,east=true,west=true,north=true,south=true,up=true]", $"minecraft:{nameof(Textures.brown_mushroom_block)}[down=true,east=true,west=true,north=true,south=true,up=true]", ""),
                        new Material("Good", "HAY_BLK", "Hay Block", 170, 0, Textures.hay_block_top, Textures.hay_block_top, $"minecraft:hay_block[axis=y]", $"minecraft:hay_block[axis=z]", ""),
                        new Material("Good", "SHROOM_INNER", "Mushroom Inside", 100, 0, Textures.mushroom_block_inside, Textures.mushroom_block_inside, $"minecraft:mushroom_stem[down=false,east=false,west=false,north=false,south=false,up=false]", $"minecraft:mushroom_stem[down=false,east=false,west=false,north=false,south=false,up=false]", ""),
                        new Material("Good", "NETHER_WART", "Netherwart Block", 214, 0, Textures.nether_wart_block, Textures.nether_wart_block, $"minecraft:{nameof(Textures.nether_wart_block)}", $"minecraft:{nameof(Textures.nether_wart_block)}", ""),
                        new Material("Good", "SMOOTH_QRTZ", "Smooth Quartz", 155, 0, Textures.quartz_block_top, Textures.quartz_block_top, $"minecraft:smooth_quartz", $"minecraft:smooth_quartz", ""),
                        new Material("Good", "SMOOTH_RED_SANDSTONE", "Smooth Red Sandstone", 179, 0, Textures.red_sandstone_top, Textures.red_sandstone_top, $"minecraft:smooth_red_sandstone", $"minecraft:smooth_red_sandstone", ""),
                        new Material("Good", "SMOOTH_SANDSTONE", "Smooth Sandstone", 24, 0, Textures.sandstone_top, Textures.sandstone_top, $"minecraft:smooth_sandstone", $"minecraft:smooth_sandstone", ""),
                        new Material("Good", "SNOW_BLK", "Snow", 80, 0, Textures.snow, Textures.snow, $"minecraft:{nameof(Textures.snow)}_block", $"minecraft:{nameof(Textures.snow)}_block", ""),

                        new Material("Okay", "BEDROCK", "Bedrock", 7, 0, Textures.bedrock, Textures.bedrock, $"minecraft:{nameof(Textures.bedrock)}", $"minecraft:{nameof(Textures.bedrock)}", "minecraft:bedrock"),
                        new Material("Okay", "DIRT_COARSE", "Coarse Dirt", 3, 1, Textures.coarse_dirt, Textures.coarse_dirt, $"minecraft:{nameof(Textures.coarse_dirt)}", $"minecraft:{nameof(Textures.coarse_dirt)}", "minecraft:dirt"),
                        new Material("Okay", "COBBLE", "Cobblestone", 4, 0, Textures.cobblestone, Textures.cobblestone, $"minecraft:{nameof(Textures.cobblestone)}", $"minecraft:{nameof(Textures.cobblestone)}", "minecraft:dirt"),
                        new Material("Okay", "DIRT", "Dirt", 3, 0, Textures.dirt, Textures.dirt, $"minecraft:{nameof(Textures.dirt)}", $"minecraft:{nameof(Textures.dirt)}", "minecraft:dirt"),
                        new Material("Okay", "SHROOM_STEM", "Mushroom Stem", 99, 14, Textures.mushroom_stem, Textures.mushroom_stem, $"minecraft:{nameof(Textures.mushroom_stem)}[down=true,east=true,west=true,north=true,south=true,up=true]", $"minecraft:{nameof(Textures.mushroom_stem)}[down=true,east=true,west=true,north=true,south=true,up=true]", ""),
                        new Material("Okay", "PRISMARINE_DARK", "Dark Prismarine", 168, 1, Textures.dark_prismarine, Textures.dark_prismarine, $"minecraft:{nameof(Textures.dark_prismarine)}", $"minecraft:{nameof(Textures.dark_prismarine)}", ""),
                        new Material("Okay", "ENDSTONE", "Endstone", 121, 0, Textures.end_stone, Textures.end_stone, $"minecraft:{nameof(Textures.end_stone)}", $"minecraft:{nameof(Textures.end_stone)}", ""),
                        new Material("Okay", "MAGMA", "Magma", 213, 0, Textures.magma, Textures.magma, $"minecraft:{nameof(Textures.magma)}_block", $"minecraft:{nameof(Textures.magma)}_block", ""),
                        new Material("Okay", "NETHER_BRICK", "Nether Brick", 112, 0, Textures.nether_bricks, Textures.nether_bricks, $"minecraft:{nameof(Textures.nether_bricks)}", $"minecraft:{nameof(Textures.nether_bricks)}", ""),
                        new Material("Okay", "NETHERRACK", "Netherrack", 87, 0, Textures.netherrack, Textures.netherrack, $"minecraft:{nameof(Textures.netherrack)}", $"minecraft:{nameof(Textures.netherrack)}", ""),
                        new Material("Okay", "OBSIDIAN", "Obsidian", 49, 0, Textures.obsidian, Textures.obsidian, $"minecraft:{nameof(Textures.obsidian)}", $"minecraft:{nameof(Textures.obsidian)}", ""),
                        new Material("Okay", "PRISMARINE_BRICK", "Prismarine Bricks", 168, 1, Textures.prismarine_bricks, Textures.prismarine_bricks, $"minecraft:{nameof(Textures.prismarine_bricks)}", $"minecraft:{nameof(Textures.prismarine_bricks)}", ""),
                        new Material("Okay", "PURPUR_BLK", "Purpur Block", 201, 0, Textures.purpur_block, Textures.purpur_block, $"minecraft:{nameof(Textures.purpur_block)}", $"minecraft:{nameof(Textures.purpur_block)}", ""),
                        new Material("Okay", "SHROOM_RED", "Red Mushroom Block", 100, 14, Textures.red_mushroom_block, Textures.red_mushroom_block, $"minecraft:{nameof(Textures.red_mushroom_block)}[down=true,east=true,west=true,north=true,south=true,up=true]", $"minecraft:{nameof(Textures.red_mushroom_block)}[down=true,east=true,west=true,north=true,south=true,up=true]", ""),
                        new Material("Okay", "NETHER_BRICK_RED", "Red Nether Bricks", 215, 0, Textures.red_nether_bricks, Textures.red_nether_bricks, $"minecraft:{nameof(Textures.red_nether_bricks)}", $"minecraft:{nameof(Textures.red_nether_bricks)}", ""),
                        new Material("Okay", "ICE_PACKED", "Packed Ice", 174, 0, Textures.packed_ice, Textures.packed_ice, $"minecraft:{nameof(Textures.packed_ice)}", $"minecraft:{nameof(Textures.packed_ice)}", ""),
                        new Material("Okay", "PRISMARINE_BLK", "Prismarine Block", 168, 0, Textures.prismarine, Textures.prismarine, $"minecraft:{nameof(Textures.prismarine)}", $"minecraft:{nameof(Textures.prismarine)}", ""),
                        new Material("Okay", "SPONGE", "Sponge", 19, 0, Textures.sponge, Textures.sponge, $"minecraft:{nameof(Textures.sponge)}", $"minecraft:{nameof(Textures.sponge)}", ""),
                        new Material("Okay", "STONE", "Stone", 1, 0, Textures.stone, Textures.stone, $"minecraft:{nameof(Textures.stone)}", $"minecraft:{nameof(Textures.stone)}", ""),
                        new Material("Okay", "ANDESITE", "Andesite", 1, 5, Textures.andesite, Textures.andesite, $"minecraft:{nameof(Textures.andesite)}", $"minecraft:{nameof(Textures.andesite)}", ""),
                        new Material("Okay", "ICE_BLUE", "Blue Ice", 174, 0, Textures.blue_ice, Textures.blue_ice, $"minecraft:{nameof(Textures.blue_ice)}", $"minecraft:{nameof(Textures.blue_ice)}", ""),
                        new Material("Okay", "DIORITE", "Diorite", 1, 3, Textures.diorite, Textures.diorite, $"minecraft:{nameof(Textures.diorite)}", $"minecraft:{nameof(Textures.diorite)}", ""),
                        new Material("Okay", "GRANITE", "Granite", 1, 1, Textures.granite, Textures.granite, $"minecraft:{nameof(Textures.granite)}", $"minecraft:{nameof(Textures.granite)}", ""),

                        new Material("Coral", "CRL_BRAIN", "Brain Coral", 1, 0, Textures.brain_coral_block, Textures.brain_coral_block, $"minecraft:{nameof(Textures.brain_coral_block)}", $"minecraft:{nameof(Textures.brain_coral_block)}", ""),
                        new Material("Coral", "CRL_BUBBLE", "Bubble Coral", 1, 0, Textures.bubble_coral_block, Textures.bubble_coral_block, $"minecraft:{nameof(Textures.bubble_coral_block)}", $"minecraft:{nameof(Textures.bubble_coral_block)}", ""),
                        new Material("Coral", "CRL_FIRE", "Fire Coral", 1, 0, Textures.fire_coral_block, Textures.fire_coral_block, $"minecraft:{nameof(Textures.fire_coral_block)}", $"minecraft:{nameof(Textures.fire_coral_block)}", ""),
                        new Material("Coral", "CRL_HORN", "Horn Coral", 1, 0, Textures.horn_coral_block, Textures.horn_coral_block, $"minecraft:{nameof(Textures.horn_coral_block)}", $"minecraft:{nameof(Textures.horn_coral_block)}", ""),
                        new Material("Coral", "CRL_TUBE", "Tube Coral", 1, 0, Textures.tube_coral_block, Textures.tube_coral_block, $"minecraft:{nameof(Textures.tube_coral_block)}", $"minecraft:{nameof(Textures.tube_coral_block)}", ""),
                        new Material("Coral", "KELP_DRIED", "Dried Kelp", 1, 0, Textures.dried_kelp_top, Textures.dried_kelp_side, $"minecraft:dried_kelp_block", $"minecraft:dried_kelp_block", ""),

                        new Material("Dead Coral", "DEAD_CRL_BRAIN", "Dead Brain Coral", 1, 0, Textures.dead_brain_coral_block, Textures.dead_brain_coral_block, $"minecraft:{nameof(Textures.dead_brain_coral_block)}", $"minecraft:{nameof(Textures.dead_brain_coral_block)}", ""),
                        new Material("Dead Coral", "DEAD_CRL_BUBBLE", "Dead Bubble Coral", 1, 0, Textures.dead_bubble_coral_block, Textures.dead_bubble_coral_block, $"minecraft:{nameof(Textures.dead_bubble_coral_block)}", $"minecraft:{nameof(Textures.dead_bubble_coral_block)}", ""),
                        new Material("Dead Coral", "DEAD_CRL_FIRE", "Dead Fire Coral", 1, 0, Textures.dead_fire_coral_block, Textures.dead_fire_coral_block, $"minecraft:{nameof(Textures.dead_fire_coral_block)}", $"minecraft:{nameof(Textures.dead_fire_coral_block)}", ""),
                        new Material("Dead Coral", "DEAD_CRL_HORN", "Dead Horn Coral", 1, 0, Textures.dead_horn_coral_block, Textures.dead_horn_coral_block, $"minecraft:{nameof(Textures.dead_horn_coral_block)}", $"minecraft:{nameof(Textures.dead_horn_coral_block)}", ""),
                        new Material("Dead Coral", "DEAD_CRL_TUBE", "Dead Tube Coral", 1, 0, Textures.dead_tube_coral_block, Textures.dead_tube_coral_block, $"minecraft:{nameof(Textures.dead_tube_coral_block)}", $"minecraft:{nameof(Textures.dead_tube_coral_block)}", ""),

                        new Material("Solid Ores", "SLD_ORE_C", "Coal Block", 173, 0, Textures.coal_block, Textures.coal_block, $"minecraft:{nameof(Textures.coal_block)}", $"minecraft:{nameof(Textures.coal_block)}", "minecraft:coal_block"),
                        new Material("Solid Ores", "SLD_ORE_I", "Iron Block", 42, 0, Textures.iron_block, Textures.iron_block, $"minecraft:{nameof(Textures.iron_block)}", $"minecraft:{nameof(Textures.iron_block)}", "minecraft:iron_block"),
                        new Material("Solid Ores", "SLD_ORE_G", "Gold Block", 41, 0, Textures.gold_block, Textures.gold_block, $"minecraft:{nameof(Textures.gold_block)}", $"minecraft:{nameof(Textures.gold_block)}", "minecraft:gold_block"),
                        new Material("Solid Ores", "SLD_ORE_R", "Redstone Block", 152, 0, Textures.redstone_block, Textures.redstone_block, $"minecraft:{nameof(Textures.redstone_block)}", $"minecraft:{nameof(Textures.redstone_block)}", "minecraft:redstone_block"),
                        new Material("Solid Ores", "SLD_ORE_L", "Lapis Block", 22, 0, Textures.lapis_block, Textures.lapis_block, $"minecraft:{nameof(Textures.lapis_block)}", $"minecraft:{nameof(Textures.lapis_block)}", "minecraft:lapis_block"),
                        new Material("Solid Ores", "SLD_ORE_D", "Diamond Block", 57, 0, Textures.diamond_block, Textures.diamond_block, $"minecraft:{nameof(Textures.diamond_block)}", $"minecraft:{nameof(Textures.diamond_block)}", "minecraft:diamond_block"),
                        new Material("Solid Ores", "SLD_ORE_E", "Emerald Block", 133, 0, Textures.emerald_block, Textures.emerald_block, $"minecraft:{nameof(Textures.emerald_block)}", $"minecraft:{nameof(Textures.emerald_block)}", "minecraft:emerald_block"),

                        new Material("Ores", "ORE_C", "Coal Ore", 16, 0, Textures.coal_ore, Textures.coal_ore, $"minecraft:{nameof(Textures.coal_ore)}", $"minecraft:{nameof(Textures.coal_ore)}", ""),
                        new Material("Ores", "ORE_I", "Iron Ore", 15, 0, Textures.iron_ore, Textures.iron_ore, $"minecraft:{nameof(Textures.iron_ore)}", $"minecraft:{nameof(Textures.iron_ore)}", ""),
                        new Material("Ores", "ORE_G", "Gold Ore", 14, 0, Textures.iron_ore, Textures.iron_ore, $"minecraft:gold_ore", $"minecraft:gold_ore", ""),
                        new Material("Ores", "ORE_R", "Redstone Ore", 73, 0, Textures.redstone_ore, Textures.redstone_ore, $"minecraft:{nameof(Textures.redstone_ore)}", $"minecraft:{nameof(Textures.redstone_ore)}", ""),
                        new Material("Ores", "ORE_L", "Lapis Ore", 21, 0, Textures.lapis_ore, Textures.lapis_ore, $"minecraft:{nameof(Textures.lapis_ore)}", $"minecraft:{nameof(Textures.lapis_ore)}", ""),
                        new Material("Ores", "ORE_D", "Diamond Ore", 56, 0, Textures.diamond_ore, Textures.diamond_ore, $"minecraft:{nameof(Textures.diamond_ore)}", $"minecraft:{nameof(Textures.diamond_ore)}", ""),
                        new Material("Ores", "ORE_E", "Emerald Ore", 129, 0, Textures.emerald_ore, Textures.emerald_ore, $"minecraft:{nameof(Textures.emerald_ore)}", $"minecraft:{nameof(Textures.emerald_ore)}", ""),
                        new Material("Ores", "ORE_QUARTZ", "Quartz Ore", 153, 0, Textures.nether_quartz_ore, Textures.nether_quartz_ore, $"minecraft:{nameof(Textures.nether_quartz_ore)}", $"minecraft:{nameof(Textures.nether_quartz_ore)}", ""),

                        new Material("Common", "BONE_BLK", "Bone Block", 216, 0, Textures.bone_block_side, Textures.bone_block_side, $"minecraft:bone_block[axis=x]", $"minecraft:bone_block[axis=x]", "minecraft:bone_block"),
                        new Material("Common", "BRICKS_RED", "Bricks", 45, 0, Textures.bricks, Textures.bricks, $"minecraft:{nameof(Textures.bricks)}", $"minecraft:{nameof(Textures.bricks)}", ""),
                        new Material("Common", "QUARTZ_CHISELED", "chiseled_quartz_block", 155, 0, Textures.chiseled_quartz_block_top, Textures.chiseled_quartz_block, $"minecraft:chiseled_quartz_block", $"minecraft:chiseled_quartz_block", ""),
                        new Material("Common", "STONE_BRICK_CRACKED", "cracked_stone_bricks", 98, 2, Textures.cracked_stone_bricks, Textures.cracked_stone_bricks, $"minecraft:{nameof(Textures.cracked_stone_bricks)}", $"minecraft:{nameof(Textures.cracked_stone_bricks)}", ""),
                        new Material("Common", "STONE_BRICK", "stone_bricks", 98, 0, Textures.stone_bricks, Textures.stone_bricks, $"minecraft:{nameof(Textures.stone_bricks)}", $"minecraft:{nameof(Textures.stone_bricks)}", ""),
                        new Material("Common", "ENDSTONE_BRICK", "end_stone_bricks", 206, 0, Textures.end_stone_bricks, Textures.end_stone_bricks, $"minecraft:{nameof(Textures.end_stone_bricks)}", $"minecraft:{nameof(Textures.end_stone_bricks)}", ""),
                        new Material("Common", "GRAVEL", "gravel", 13, 0, Textures.gravel, Textures.gravel, $"minecraft:{nameof(Textures.gravel)}", $"minecraft:{nameof(Textures.gravel)}", "minecraft:gravel"),
                        new Material("Common", "COBBLE_MOSSY", "mossy_cobblestone", 48, 0, Textures.mossy_cobblestone, Textures.mossy_cobblestone, $"minecraft:{nameof(Textures.mossy_cobblestone)}", $"minecraft:{nameof(Textures.mossy_cobblestone)}", ""),
                        new Material("Common", "COBBLE_MOSSY_BRICK", "mossy_stone_bricks", 98, 1, Textures.mossy_stone_bricks, Textures.mossy_stone_bricks, $"minecraft:{nameof(Textures.mossy_stone_bricks)}", $"minecraft:{nameof(Textures.mossy_stone_bricks)}", "")
                    };

                    if (_List.GroupBy(x => x.PixelStackerID).Any(x => x.Count() > 1))
                    {
                        throw new ArgumentException("All pixelStackerIDs must be unique!");
                    }
                }

                //string content = string.Join("\r\n", _List.GroupBy(x => x.Category).Select(x => string.Join(",\r\n", _List.Select(xm => xm.toConstructorString()))));


                return Materials._List;
            }
        }
    }
}
