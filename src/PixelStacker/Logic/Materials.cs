using PixelStacker.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PixelStacker.PreRender.Extensions;
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
                        if (ColorMap.TryGetValue(c, out Material[] mats))
                        {
                            if (mats.Length == 1 || (mats.Length == 2 && mats.Any(x => x.BlockID == 0)))
                            {
                                diffd /= 2;
                            }
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
                        new Material("Air","Air","minecraft:air", 0,0, Textures.air,true, "minecraft:air"),

                        new Material("Glass","White Glass","minecraft:white_stained_glass", 95,0, Textures.white_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Orange Glass","minecraft:orange_stained_glass",95,1, Textures.orange_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Magenta Glass","minecraft:magenta_stained_glass",95,2, Textures.magenta_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Light Blue Glass","minecraft:light_blue_stained_glass",95,3, Textures.light_blue_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Yellow Glass","minecraft:yellow_stained_glass",95,4, Textures.yellow_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Lime Glass","minecraft:lime_stained_glass",95,5, Textures.lime_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Pink Glass","minecraft:pink_stained_glass",95,6, Textures.pink_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Gray Glass","minecraft:gray_stained_glass",95,7, Textures.gray_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Light Gray Glass","minecraft:light_gray_stained_glass",95,8, Textures.light_gray_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Cyan Glass","minecraft:cyan_stained_glass",95,9, Textures.cyan_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Purple Glass","minecraft:purple_stained_glass",95,10, Textures.purple_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Blue Glass","minecraft:blue_stained_glass",95,11, Textures.blue_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Brown Glass","minecraft:brown_stained_glass",95,12, Textures.brown_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Green Glass","minecraft:green_stained_glass",95,13, Textures.green_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Red Glass","minecraft:red_stained_glass",95,14, Textures.red_stained_glass, true, "minecraft:stained_glass"),
                        new Material("Glass","Black Glass","minecraft:black_stained_glass",95,15, Textures.black_stained_glass, true, "minecraft:stained_glass"),

                        new Material("Wool","White Wool","minecraft:white_wool",35,0, Textures.white_wool, true, "minecraft:wool"),
                        new Material("Wool","Orange Wool", "minecraft:orange_wool", 35,1, Textures.orange_wool, true, "minecraft:wool"),
                        new Material("Wool","Magenta Wool", "minecraft:magenta_wool", 35,2, Textures.magenta_wool, true, "minecraft:wool"),
                        new Material("Wool","Light Blue Wool", "minecraft:light_blue_wool", 35,3, Textures.light_blue_wool, true, "minecraft:wool"),
                        new Material("Wool","Yellow Wool", "minecraft:yellow_wool", 35,4, Textures.yellow_wool, true, "minecraft:wool"),
                        new Material("Wool","Lime Wool", "minecraft:lime_wool", 35,5, Textures.lime_wool, true, "minecraft:wool"),
                        new Material("Wool","Pink Wool", "minecraft:pink_wool", 35,6, Textures.pink_wool, true, "minecraft:wool"),
                        new Material("Wool","Gray Wool", "minecraft:gray_wool", 35,7, Textures.gray_wool, true, "minecraft:wool"),
                        new Material("Wool","Light Gray Wool", "minecraft:light_gray_wool", 35,8, Textures.light_gray_wool, true, "minecraft:wool"),
                        new Material("Wool","Cyan Wool", "minecraft:cyan_wool", 35,9, Textures.cyan_wool, true, "minecraft:wool"),
                        new Material("Wool","Purple Wool", "minecraft:purple_wool", 35,10, Textures.purple_wool, true, "minecraft:wool"),
                        new Material("Wool","Blue Wool", "minecraft:blue_wool", 35,11, Textures.blue_wool, true, "minecraft:wool"),
                        new Material("Wool","Brown Wool", "minecraft:brown_wool", 35,12, Textures.brown_wool, true, "minecraft:wool"),
                        new Material("Wool","Green Wool", "minecraft:green_wool", 35,13, Textures.green_wool, true, "minecraft:wool"),
                        new Material("Wool","Red Wool", "minecraft:red_wool", 35,14, Textures.red_wool, true, "minecraft:wool"),
                        new Material("Wool","Black Wool", "minecraft:black_wool", 35,15, Textures.black_wool, true, "minecraft:wool"),

                        new Material("Concrete","White Concrete","minecraft:white_concrete", 251,0, Textures.white_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Orange Concrete","minecraft:orange_concrete", 251,1, Textures.orange_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Magenta Concrete","minecraft:magenta_concrete", 251,2, Textures.magenta_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Light Blue Concrete","minecraft:light_blue_concrete", 251,3, Textures.light_blue_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Yellow Concrete","minecraft:yellow_concrete", 251,4, Textures.yellow_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Lime Concrete","minecraft:lime_concrete", 251,5, Textures.lime_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Pink Concrete","minecraft:pink_concrete", 251,6, Textures.pink_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Gray Concrete","minecraft:gray_concrete", 251,7, Textures.gray_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Light Gray Concrete","minecraft:light_gray_concrete", 251,8, Textures.light_gray_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Cyan Concrete","minecraft:cyan_concrete", 251,9, Textures.cyan_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Purple Concrete","minecraft:purple_concrete", 251,10, Textures.purple_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Blue Concrete","minecraft:blue_concrete", 251,11, Textures.blue_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Brown Concrete","minecraft:brown_concrete", 251,12, Textures.brown_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Green Concrete","minecraft:green_concrete", 251,13, Textures.green_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Red Concrete","minecraft:red_concrete", 251,14, Textures.red_concrete, false, "mineraft:concrete"),
                        new Material("Concrete","Black Concrete","minecraft:black_concrete", 251,15, Textures.black_concrete, false, "mineraft:concrete"),

                        new Material("Powder","White Powder","minecraft:white_concrete_powder", 252,0, Textures.white_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Orange Powder","minecraft:orange_concrete_powder", 252,1, Textures.orange_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Magenta Powder","minecraft:magenta_concrete_powder", 252,2, Textures.magenta_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Light Blue Powder","minecraft:light_blue_concrete_powder", 252,3, Textures.light_blue_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Yellow Powder","minecraft:yellow_concrete_powder", 252,4, Textures.yellow_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Lime Powder","minecraft:lime_concrete_powder", 252,5, Textures.lime_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Pink Powder","minecraft:pink_concrete_powder", 252,6, Textures.pink_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Gray Powder","minecraft:gray_concrete_powder", 252,7, Textures.gray_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Light Gray Powder","minecraft:light_gray_concrete_powder", 252,8, Textures.light_gray_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Cyan Powder","minecraft:cyan_concrete_powder", 252,9, Textures.cyan_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Purple Powder","minecraft:purple_concrete_powder", 252,10, Textures.purple_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Blue Powder","minecraft:blue_concrete_powder", 252,11, Textures.blue_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Brown Powder","minecraft:brown_concrete_powder", 252,12, Textures.brown_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Green Powder","minecraft:green_concrete_powder", 252,13, Textures.green_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Red Powder","minecraft:red_concrete_powder", 252,14, Textures.red_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","Black Powder","minecraft:black_concrete_powder", 252,15, Textures.black_concrete_powder, false, "mineraft:concrete_powder"),
                        new Material("Powder","zzSand","minecraft:sand",12,0, Textures.sand, false, "minecraft:sand"),
                        new Material("Powder","zzSand Red","minecraft:red_sand",12,1, Textures.red_sand, false, "minecraft:sand"),

                        new Material("Clay","White Clay","minecraft:white_terracotta", 159,0, Textures.white_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Orange Clay","minecraft:orange_terracotta", 159,1, Textures.orange_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Magenta Clay","minecraft:magenta_terracotta", 159,2, Textures.magenta_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Light Blue Clay","minecraft:light_blue_terracotta", 159,3, Textures.light_blue_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Yellow Clay","minecraft:yellow_terracotta", 159,4, Textures.yellow_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Lime Clay","minecraft:lime_terracotta", 159,5, Textures.lime_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Pink Clay","minecraft:pink_terracotta", 159,6, Textures.pink_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Gray Clay","minecraft:gray_terracotta", 159,7, Textures.gray_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Light Gray Clay","minecraft:light_gray_terracotta", 159,8, Textures.light_gray_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Cyan Clay","minecraft:cyan_terracotta", 159,9, Textures.cyan_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Purple Clay","minecraft:purple_terracotta", 159,10, Textures.purple_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Blue Clay","minecraft:blue_terracotta", 159,11, Textures.blue_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Brown Clay","minecraft:brown_terracotta", 159,12, Textures.brown_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Green Clay","minecraft:green_terracotta", 159,13, Textures.green_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Red Clay","minecraft:red_terracotta", 159,14, Textures.red_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","Black Clay","minecraft:black_terracotta", 159,15, Textures.black_terracotta, false, "minecraft:stained_hardened_clay"),
                        new Material("Clay","zzHardened Clay","minecraft:terracotta",172,0, Textures.terracotta, false, "minecraft:hardened_clay"),
                        new Material("Clay","zzClay","minecraft:clay",82,0, Textures.clay, false, "minecraft:clay"),

                        new Material("Terracotta","White Terracotta","minecraft:white_glazed_terracotta",235,0, Textures.white_glazed_terracotta, false,"minecraft:white_glazed_terracotta"),
                        new Material("Terracotta","Orange Terracotta","minecraft:orange_glazed_terracotta", 236,0, Textures.orange_glazed_terracotta, false,"minecraft:orange_glazed_terracotta"),
                        new Material("Terracotta","Magenta Terracotta","minecraft:magenta_glazed_terracotta",237,0, Textures.magenta_glazed_terracotta, false,"minecraft:magenta_glazed_terracotta"),
                        new Material("Terracotta","Light Blue Terracotta","minecraft:light_blue_glazed_terracotta",238,0, Textures.light_blue_glazed_terracotta, false,"minecraft:light_blue_glazed_terracotta"),
                        new Material("Terracotta","Yellow Terracotta","minecraft:yellow_glazed_terracotta",239,0, Textures.yellow_glazed_terracotta, false,"minecraft:yellow_glazed_terracotta"),
                        new Material("Terracotta","Lime Terracotta","minecraft:lime_glazed_terracotta",240,0, Textures.lime_glazed_terracotta, false,"minecraft:lime_glazed_terracotta"),
                        new Material("Terracotta","Pink Terracotta","minecraft:pink_glazed_terracotta",241,0, Textures.pink_glazed_terracotta, false,"minecraft:pink_glazed_terracotta"),
                        new Material("Terracotta","Gray Terracotta","minecraft:gray_glazed_terracotta",242,0, Textures.gray_glazed_terracotta, false,"minecraft:gray_glazed_terracotta"),
                        new Material("Terracotta","Light Gray Terracotta","minecraft:light_gray_glazed_terracotta",243,0, Textures.light_gray_glazed_terracotta, false, "minecraft:silver_glazed_terracotta"),
                        new Material("Terracotta","Cyan Terracotta","minecraft:cyan_glazed_terracotta",244,0, Textures.cyan_glazed_terracotta, false,"minecraft:cyan_glazed_terracotta"),
                        new Material("Terracotta","Purple Terracotta","minecraft:purple_glazed_terracotta",245,0, Textures.purple_glazed_terracotta, false,"minecraft:purple_glazed_terracotta"),
                        new Material("Terracotta","Blue Terracotta","minecraft:blue_glazed_terracotta",246,0, Textures.blue_glazed_terracotta, false,"minecraft:blue_glazed_terracotta"),
                        new Material("Terracotta","Brown Terracotta","minecraft:brown_glazed_terracotta",247,0, Textures.brown_glazed_terracotta, false,"minecraft:brown_glazed_terracotta"),
                        new Material("Terracotta","Green Terracotta","minecraft:green_glazed_terracotta",248,0, Textures.green_glazed_terracotta, false,"minecraft:green_glazed_terracotta"),
                        new Material("Terracotta","Red Terracotta","minecraft:red_glazed_terracotta",249,0, Textures.red_glazed_terracotta, false,"minecraft:red_glazed_terracotta"),
                        new Material("Terracotta","Black Terracotta","minecraft:black_glazed_terracotta",250,0, Textures.black_glazed_terracotta, false,"minecraft:black_glazed_terracotta"),

                        new Material("Planks","Planks Oak","minecraft:oak_planks",5,0, Textures.oak_planks,false, "minecraft:planks"),
                        new Material("Planks","Planks Spruce","minecraft:spruce_planks",5,1, Textures.spruce_planks,false, "minecraft:planks"),
                        new Material("Planks","Planks Birch","minecraft:birch_planks",5,2, Textures.birch_planks,false, "minecraft:planks"),
                        new Material("Planks","Planks Jungle","minecraft:jungle_planks",5,3, Textures.jungle_planks,false, "minecraft:planks"),
                        new Material("Planks","Planks Acacia","minecraft:acacia_planks",5,4, Textures.acacia_planks,false, "minecraft:planks"),
                        new Material("Planks","Planks Dark Oak","minecraft:dark_oak_planks",5,5, Textures.dark_oak_planks,false, "minecraft:planks"),

                        new Material("Wood", "Stripped Acacia", "minecraft:stripped_acacia_log[axis=x]","minecraft:stripped_acacia_log[axis=x]", 17, 0, Textures.stripped_acacia_log),
                        new Material("Wood", "Stripped Birch", "minecraft:stripped_birch_log[axis=x]", "minecraft:stripped_birch_log[axis=x]", 17, 0, Textures.stripped_birch_log),
                        new Material("Wood", "Stripped Dark Oak", "minecraft:stripped_dark_oak_log[axis=x]","minecraft:stripped_dark_oak_log[axis=x]", 17, 0, Textures.stripped_dark_oak_log),
                        new Material("Wood", "Stripped Jungle", "minecraft:stripped_jungle_log[axis=x]","minecraft:stripped_jungle_log[axis=x]", 17, 0, Textures.stripped_jungle_log),
                        new Material("Wood", "Stripped Oak", "minecraft:stripped_oak_log[axis=x]","minecraft:stripped_oak_log[axis=x]",  17, 0, Textures.stripped_oak_log),
                        new Material("Wood", "Stripped Spruce", "minecraft:stripped_spruce_log[axis=x]","minecraft:stripped_spruce_log[axis=x]",  17, 0, Textures.stripped_spruce_log),

                        new Material("Wood", "Bark Acacia", "minecraft:acacia_log[axis=x]", "minecraft:acacia_log[axis=x]",  17, 0, Textures.acacia_log),
                        new Material("Wood", "Bark Birch", "minecraft:birch_log[axis=x]","minecraft:birch_log[axis=x]",  17, 0, Textures.birch_log),
                        new Material("Wood", "Bark Dark Oak", "minecraft:dark_oak_log[axis=x]","minecraft:dark_oak_log[axis=x]",  17, 0, Textures.dark_oak_log),
                        new Material("Wood", "Bark Jungle", "minecraft:jungle_log[axis=x]","minecraft:jungle_log[axis=x]",  17, 0, Textures.jungle_log),
                        new Material("Wood", "Bark Oak", "minecraft:oak_log[axis=x]", "minecraft:oak_log[axis=x]",  17, 0, Textures.oak_log),
                        new Material("Wood", "Bark Spruce", "minecraft:spruce_log[axis=x]","minecraft:spruce_log[axis=x]",  17, 0, Textures.spruce_log),

                        new Material("Good", "Brown Mushroom", "minecraft:brown_mushroom_block[down=true,east=true,west=true,north=true,south=true,up=true]",  99, 14, Textures.brown_mushroom_block, false),
                        new Material("Good", "Hay Block", $"minecraft:hay_block[axis=y]", $"minecraft:hay_block[axis=z]", 170, 0, Textures.hay_block_top),
                        new Material("Good", "Mushroom Inside", $"minecraft:mushroom_stem[down=false,east=false,west=false,north=false,south=false,up=false]", 100, 0, Textures.mushroom_block_inside),
                        new Material("Good", "Netherwart Block", $"minecraft:{nameof(Textures.nether_wart_block)}", 214, 0, Textures.nether_wart_block),
                        new Material("Good", "Smooth Quartz", $"minecraft:smooth_quartz",155, 0,Textures.quartz_block_top),
                        new Material("Good", "Smooth Red Sandstone", $"minecraft:smooth_red_sandstone",179, 0,Textures.red_sandstone_top),
                        new Material("Good", "Smooth Sandstone", $"minecraft:smooth_sandstone",24, 0,Textures.sandstone_top),
                        new Material("Good", "Snow", $"minecraft:snow_block",80, 0,Textures.snow),

                        new Material("Okay", "Bedrock", "minecraft:bedrock",  7, 0, Textures.bedrock, true, "minecraft:bedrock"),
                        new Material("Okay","Coarse Dirt","minecraft:coarse_dirt",3,1, Textures.coarse_dirt,false,"minecraft:dirt"),
                        new Material("Okay","Cobblestone","minecraft:cobblestone",4,0, Textures.cobblestone,false,"minecraft:dirt"),
                        new Material("Okay","Dirt","minecraft:dirt",3,0, Textures.dirt,false,"minecraft:dirt"),
                        new Material("Okay","Mushroom Stem","minecraft:mushroom_stem[down=true,east=true,west=true,north=true,south=true,up=true]",99,14, Textures.mushroom_stem,false),
                        new Material("Okay","Dark Prismarine","minecraft:dark_prismarine",168,1, Textures.dark_prismarine,true),
                        new Material("Okay","Endstone","minecraft:end_stone",121,0, Textures.end_stone, false),
                        new Material("Okay","Magma","minecraft:magma_block",213,0, Textures.magma, true),
                        new Material("Okay","Nether Brick","minecraft:nether_bricks",112,0, Textures.nether_bricks, false),
                        new Material("Okay","Netherrack","minecraft:netherrack",87,0, Textures.netherrack, false),
                        new Material("Okay","Obsidian","minecraft:obsidian",49,0, Textures.obsidian,false),
                        new Material("Okay","Prismarine Bricks","minecraft:prismarine_bricks",168,1, Textures.prismarine_bricks,false),
                        new Material("Okay","Purpur Block","minecraft:purpur_block",201,0, Textures.purpur_block, false),
                        new Material("Okay","Red Mushroom Block","minecraft:red_mushroom_block[down=true,east=true,west=true,north=true,south=true,up=true]",100,14, Textures.red_mushroom_block, false),
                        new Material("Okay","Red Nether Bricks","minecraft:red_nether_bricks",215,0, Textures.red_nether_bricks, false),
                        new Material("Okay", "Packed Ice", $"minecraft:{nameof(Textures.packed_ice)}",174, 0,Textures.packed_ice),
                        new Material("Okay", "Prismarine Block", $"minecraft:{nameof(Textures.prismarine)}",168, 0,Textures.prismarine),
                        new Material("Okay", "Sponge", $"minecraft:sponge",19, 0,Textures.sponge),
                        new Material("Okay", "Stone", $"minecraft:stone",1, 0,Textures.stone),
                        new Material("Okay", "Andesite", $"minecraft:{nameof(Textures.andesite)}", 1, 5, Textures.andesite, false),
                        new Material("Okay", "Blue Ice", $"minecraft:{nameof(Textures.blue_ice)}", 174, 0, Textures.blue_ice, false),
                        new Material("Okay", "Diorite", $"minecraft:{nameof(Textures.diorite)}", 1, 3, Textures.diorite, false),
                        new Material("Okay", "Granite", $"minecraft:{nameof(Textures.granite)}", 1, 1, Textures.granite, false),

                        new Material("Coral", "Brain Coral", "minecraft:brain_coral_block",  1, 0, Textures.brain_coral_block),
                        new Material("Coral", "Bubble Coral", "minecraft:bubble_coral_block",  1, 0, Textures.bubble_coral_block),
                        new Material("Coral", "Fire Coral", "minecraft:fire_coral_block",  1, 0, Textures.fire_coral_block),
                        new Material("Coral", "Horn Coral", "minecraft:horn_coral_block",  1, 0, Textures.horn_coral_block),
                        new Material("Coral", "Tube Coral", "minecraft:tube_coral_block",  1, 0, Textures.tube_coral_block),
                        new Material("Coral", "Dried Kelp", "minecraft:dried_kelp_block",  1, 0, Textures.dried_kelp_top, Textures.dried_kelp_side),

                        new Material("Dead Coral", "Dead Brain Coral", "minecraft:dead_brain_coral_block",  1, 0, Textures.dead_brain_coral_block),
                        new Material("Dead Coral", "Dead Bubble Coral", "minecraft:dead_bubble_coral_block",  1, 0, Textures.dead_bubble_coral_block),
                        new Material("Dead Coral", "Dead Fire Coral", "minecraft:dead_fire_coral_block",  1, 0, Textures.dead_fire_coral_block),
                        new Material("Dead Coral", "Dead Horn Coral", "minecraft:dead_horn_coral_block",  1, 0, Textures.dead_horn_coral_block),
                        new Material("Dead Coral", "Dead Tube Coral", "minecraft:dead_tube_coral_block",  1, 0, Textures.dead_tube_coral_block),

                        new Material("Solid Ores", "Coal Block", $"minecraft:{nameof(Textures.coal_block)}",173,0, Textures.coal_block, false, $"minecraft:{nameof(Textures.coal_block)}"),
                        new Material("Solid Ores", "Iron Block", $"minecraft:{nameof(Textures.iron_block)}",42,0, Textures.iron_block, false, $"minecraft:{nameof(Textures.iron_block)}"),
                        new Material("Solid Ores", "Gold Block", $"minecraft:{nameof(Textures.gold_block)}",41,0, Textures.gold_block,false, $"minecraft:{nameof(Textures.gold_block)}"),
                        new Material("Solid Ores", "Redstone Block", $"minecraft:{nameof(Textures.redstone_block)}",152,0, Textures.redstone_block, false, $"minecraft:{nameof(Textures.redstone_block)}"),
                        new Material("Solid Ores", "Lapis Block", $"minecraft:{nameof(Textures.lapis_block)}",22,0, Textures.lapis_block, false, $"minecraft:{nameof(Textures.lapis_block)}"),
                        new Material("Solid Ores", "Diamond Block", $"minecraft:{nameof(Textures.diamond_block)}",57,0, Textures.diamond_block, false, $"minecraft:{nameof(Textures.diamond_block)}"),
                        new Material("Solid Ores", "Emerald Block", $"minecraft:{nameof(Textures.emerald_block)}",133,0, Textures.emerald_block, false, $"minecraft:{nameof(Textures.emerald_block)}"),

                        new Material("Ores", "Coal Ore", $"minecraft:{nameof(Textures.coal_ore)}",16,0, Textures.coal_ore),
                        new Material("Ores", "Iron Ore", $"minecraft:{nameof(Textures.iron_ore)}",15,0, Textures.iron_ore),
                        new Material("Ores", "Gold Ore", $"minecraft:{nameof(Textures.gold_ore)}",14,0, Textures.iron_ore),
                        new Material("Ores", "Redstone Ore", $"minecraft:{nameof(Textures.redstone_ore)}",73,0, Textures.redstone_ore),
                        new Material("Ores", "Lapis Ore", $"minecraft:{nameof(Textures.lapis_ore)}",21,0, Textures.lapis_ore),
                        new Material("Ores", "Diamond Ore", $"minecraft:{nameof(Textures.diamond_ore)}",56,0, Textures.diamond_ore),
                        new Material("Ores", "Emerald Ore", $"minecraft:{nameof(Textures.emerald_ore)}",129,0, Textures.emerald_ore),
                        new Material("Ores", "Quartz Ore", $"minecraft:{nameof(Textures.nether_quartz_ore)}",153,0, Textures.nether_quartz_ore),

                        new Material("Common", "Bone Block", $"minecraft:bone_block[axis=x]",216,0, Textures.bone_block_side, false, "minecraft:bone_block"),
                        new Material("Common", "Bricks", $"minecraft:{nameof(Textures.bricks)}",45,0, Textures.bricks),
                        new Material("Common", "chiseled_quartz_block", $"minecraft:chiseled_quartz_block",155,0, Textures.chiseled_quartz_block_top, Textures.chiseled_quartz_block),
                        new Material("Common", nameof(Textures.cracked_stone_bricks), $"minecraft:{nameof(Textures.cracked_stone_bricks)}",98,2, Textures.cracked_stone_bricks),
                        new Material("Common", nameof(Textures.stone_bricks), $"minecraft:{nameof(Textures.stone_bricks)}",98,0, Textures.stone_bricks),
                        new Material("Common", "end_stone_bricks", $"minecraft:{nameof(Textures.end_stone_bricks)}",206,0, Textures.end_stone_bricks),
                        new Material("Common", "gravel", $"minecraft:{nameof(Textures.gravel)}",13,0, Textures.gravel, false, "minecraft:gravel"),
                        new Material("Common", "mossy_cobblestone", $"minecraft:{nameof(Textures.mossy_cobblestone)}",48,0, Textures.mossy_cobblestone),
                        new Material("Common", "mossy_stone_bricks", $"minecraft:{nameof(Textures.mossy_stone_bricks)}",98,1, Textures.mossy_stone_bricks),
                    };
                }
                return Materials._List;
            }
        }
    }
}
