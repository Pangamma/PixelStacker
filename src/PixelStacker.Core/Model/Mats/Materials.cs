using PixelStacker.Core.Extensions;
using PixelStacker.Core.Model.Drawing;
using PixelStacker.Resources;

namespace PixelStacker.Core.Model.Mats
{
    public class Materials
    {
        public static Material Air { get; set; } = List.FirstOrDefault(x => x.PixelStackerID == "AIR");

        public static Material FromPixelStackerID(string pixelStackerID)
        {
            return List.FirstOrDefault(x => x.PixelStackerID == pixelStackerID);
        }

        private static List<Material> _List = null;
        public static List<Material> List
        {
            get
            {
                if (_List == null)
                {

                    /**
                     * 1.14
                     * https://minecraft.gamepedia.com/Scaffolding
                     * https://minecraft.gamepedia.com/
                     * https://minecraft.gamepedia.com/Blast_Furnace
                     * https://minecraft.gamepedia.com/Smoker
                     * https://minecraft.gamepedia.com/Cartography_Table
                     * https://minecraft.gamepedia.com/Composter
                     * https://minecraft.gamepedia.com/Fletching_Table
                     * https://minecraft.gamepedia.com/Smithing_Table
                     * https://minecraft.gamepedia.com/Loom
                     * 
                     * 1.11
                     * https://minecraft.gamepedia.com/Shulker_Box
                     * 
                     * "1.8",  TODO ADD SLIME BLOCKS
                     * TODO Go through blocks with edges, see if there are any borderless alternatives.
                     * 
                     * "1.8",  Sea lanturns? 
                     */


                    _List = new List<Material>()
                    {
                        new Material("1.7", false, "Air", "AIR", "Air", 0, 0, Textures.air.ToPxBitmap(), Textures.air.ToPxBitmap(), $"minecraft:{nameof(Textures.air)}", $"minecraft:{nameof(Textures.air)}", "minecraft:air"),

                        new Material("1.7", false, "Glass", "GLASS_00", "White Glass", 95, 0, Textures.white_stained_glass.ToPxBitmap(), Textures.white_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.white_stained_glass)}", $"minecraft:{nameof(Textures.white_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_01", "Orange Glass", 95, 1, Textures.orange_stained_glass.ToPxBitmap(), Textures.orange_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.orange_stained_glass)}", $"minecraft:{nameof(Textures.orange_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_02", "Magenta Glass", 95, 2, Textures.magenta_stained_glass.ToPxBitmap(), Textures.magenta_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.magenta_stained_glass)}", $"minecraft:{nameof(Textures.magenta_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", true, "Glass", "GLASS_03", "Light Blue Glass", 95, 3, Textures.light_blue_stained_glass.ToPxBitmap(), Textures.light_blue_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.light_blue_stained_glass)}", $"minecraft:{nameof(Textures.light_blue_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_04", "Yellow Glass", 95, 4, Textures.yellow_stained_glass.ToPxBitmap(), Textures.yellow_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.yellow_stained_glass)}", $"minecraft:{nameof(Textures.yellow_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_05", "Lime Glass", 95, 5, Textures.lime_stained_glass.ToPxBitmap(), Textures.lime_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.lime_stained_glass)}", $"minecraft:{nameof(Textures.lime_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_06", "Pink Glass", 95, 6, Textures.pink_stained_glass.ToPxBitmap(), Textures.pink_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.pink_stained_glass)}", $"minecraft:{nameof(Textures.pink_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_07", "Gray Glass", 95, 7, Textures.gray_stained_glass.ToPxBitmap(), Textures.gray_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.gray_stained_glass)}", $"minecraft:{nameof(Textures.gray_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_08", "Light Gray Glass", 95, 8, Textures.light_gray_stained_glass.ToPxBitmap(), Textures.light_gray_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.light_gray_stained_glass)}", $"minecraft:{nameof(Textures.light_gray_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_09", "Cyan Glass", 95, 9, Textures.cyan_stained_glass.ToPxBitmap(), Textures.cyan_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.cyan_stained_glass)}", $"minecraft:{nameof(Textures.cyan_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_10", "Purple Glass", 95, 10, Textures.purple_stained_glass.ToPxBitmap(), Textures.purple_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.purple_stained_glass)}", $"minecraft:{nameof(Textures.purple_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_11", "Blue Glass", 95, 11, Textures.blue_stained_glass.ToPxBitmap(), Textures.blue_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.blue_stained_glass)}", $"minecraft:{nameof(Textures.blue_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_12", "Brown Glass", 95, 12, Textures.brown_stained_glass.ToPxBitmap(), Textures.brown_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.brown_stained_glass)}", $"minecraft:{nameof(Textures.brown_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_13", "Green Glass", 95, 13, Textures.green_stained_glass.ToPxBitmap(), Textures.green_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.green_stained_glass)}", $"minecraft:{nameof(Textures.green_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_14", "Red Glass", 95, 14, Textures.red_stained_glass.ToPxBitmap(), Textures.red_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.red_stained_glass)}", $"minecraft:{nameof(Textures.red_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_15", "Black Glass", 95, 15, Textures.black_stained_glass.ToPxBitmap(), Textures.black_stained_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.black_stained_glass)}", $"minecraft:{nameof(Textures.black_stained_glass)}", "minecraft:stained_glass"),
                        new Material("1.7", true, "Glass", "GLASS_CLR", "Clear Glass", 20, 0, Textures.glass.ToPxBitmap(), Textures.glass.ToPxBitmap(), $"minecraft:{nameof(Textures.glass)}", $"minecraft:{nameof(Textures.glass)}", "minecraft:glass"),
                        new Material("1.17", true, "Glass", "GLASS_TINTED", "Tinted Glass", 95, 15, Textures.tinted_glass.ToPxBitmap(), Textures.tinted_glass.ToPxBitmap(), $"minecraft:{nameof(Textures.tinted_glass)}", $"minecraft:{nameof(Textures.tinted_glass)}", ""),

                        new Material("1.7", false, "Wool", "WOOL_00", "White Wool", 35, 0, Textures.white_wool.ToPxBitmap(), Textures.white_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.white_wool)}", $"minecraft:{nameof(Textures.white_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_01", "Orange Wool", 35, 1, Textures.orange_wool.ToPxBitmap(), Textures.orange_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.orange_wool)}", $"minecraft:{nameof(Textures.orange_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_02", "Magenta Wool", 35, 2, Textures.magenta_wool.ToPxBitmap(), Textures.magenta_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.magenta_wool)}", $"minecraft:{nameof(Textures.magenta_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_03", "Light Blue Wool", 35, 3, Textures.light_blue_wool.ToPxBitmap(), Textures.light_blue_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.light_blue_wool)}", $"minecraft:{nameof(Textures.light_blue_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_04", "Yellow Wool", 35, 4, Textures.yellow_wool.ToPxBitmap(), Textures.yellow_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.yellow_wool)}", $"minecraft:{nameof(Textures.yellow_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_05", "Lime Wool", 35, 5, Textures.lime_wool.ToPxBitmap(), Textures.lime_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.lime_wool)}", $"minecraft:{nameof(Textures.lime_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_06", "Pink Wool", 35, 6, Textures.pink_wool.ToPxBitmap(), Textures.pink_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.pink_wool)}", $"minecraft:{nameof(Textures.pink_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_07", "Gray Wool", 35, 7, Textures.gray_wool.ToPxBitmap(), Textures.gray_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.gray_wool)}", $"minecraft:{nameof(Textures.gray_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_08", "Light Gray Wool", 35, 8, Textures.light_gray_wool.ToPxBitmap(), Textures.light_gray_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.light_gray_wool)}", $"minecraft:{nameof(Textures.light_gray_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_09", "Cyan Wool", 35, 9, Textures.cyan_wool.ToPxBitmap(), Textures.cyan_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.cyan_wool)}", $"minecraft:{nameof(Textures.cyan_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_10", "Purple Wool", 35, 10, Textures.purple_wool.ToPxBitmap(), Textures.purple_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.purple_wool)}", $"minecraft:{nameof(Textures.purple_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_11", "Blue Wool", 35, 11, Textures.blue_wool.ToPxBitmap(), Textures.blue_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.blue_wool)}", $"minecraft:{nameof(Textures.blue_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_12", "Brown Wool", 35, 12, Textures.brown_wool.ToPxBitmap(), Textures.brown_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.brown_wool)}", $"minecraft:{nameof(Textures.brown_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_13", "Green Wool", 35, 13, Textures.green_wool.ToPxBitmap(), Textures.green_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.green_wool)}", $"minecraft:{nameof(Textures.green_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_14", "Red Wool", 35, 14, Textures.red_wool.ToPxBitmap(), Textures.red_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.red_wool)}", $"minecraft:{nameof(Textures.red_wool)}", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_15", "Black Wool", 35, 15, Textures.black_wool.ToPxBitmap(), Textures.black_wool.ToPxBitmap(), $"minecraft:{nameof(Textures.black_wool)}", $"minecraft:{nameof(Textures.black_wool)}", "minecraft:wool"),

                        new Material("1.12", false, "Concrete", "CONC_00", "White Concrete", 251, 0, Textures.white_concrete.ToPxBitmap(), Textures.white_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.white_concrete)}", $"minecraft:{nameof(Textures.white_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_01", "Orange Concrete", 251, 1, Textures.orange_concrete.ToPxBitmap(), Textures.orange_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.orange_concrete)}", $"minecraft:{nameof(Textures.orange_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_02", "Magenta Concrete", 251, 2, Textures.magenta_concrete.ToPxBitmap(), Textures.magenta_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.magenta_concrete)}", $"minecraft:{nameof(Textures.magenta_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_03", "Light Blue Concrete", 251, 3, Textures.light_blue_concrete.ToPxBitmap(), Textures.light_blue_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.light_blue_concrete)}", $"minecraft:{nameof(Textures.light_blue_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_04", "Yellow Concrete", 251, 4, Textures.yellow_concrete.ToPxBitmap(), Textures.yellow_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.yellow_concrete)}", $"minecraft:{nameof(Textures.yellow_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_05", "Lime Concrete", 251, 5, Textures.lime_concrete.ToPxBitmap(), Textures.lime_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.lime_concrete)}", $"minecraft:{nameof(Textures.lime_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_06", "Pink Concrete", 251, 6, Textures.pink_concrete.ToPxBitmap(), Textures.pink_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.pink_concrete)}", $"minecraft:{nameof(Textures.pink_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_07", "Gray Concrete", 251, 7, Textures.gray_concrete.ToPxBitmap(), Textures.gray_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.gray_concrete)}", $"minecraft:{nameof(Textures.gray_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_08", "Light Gray Concrete", 251, 8, Textures.light_gray_concrete.ToPxBitmap(), Textures.light_gray_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.light_gray_concrete)}", $"minecraft:{nameof(Textures.light_gray_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_09", "Cyan Concrete", 251, 9, Textures.cyan_concrete.ToPxBitmap(), Textures.cyan_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.cyan_concrete)}", $"minecraft:{nameof(Textures.cyan_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_10", "Purple Concrete", 251, 10, Textures.purple_concrete.ToPxBitmap(), Textures.purple_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.purple_concrete)}", $"minecraft:{nameof(Textures.purple_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_11", "Blue Concrete", 251, 11, Textures.blue_concrete.ToPxBitmap(), Textures.blue_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.blue_concrete)}", $"minecraft:{nameof(Textures.blue_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_12", "Brown Concrete", 251, 12, Textures.brown_concrete.ToPxBitmap(), Textures.brown_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.brown_concrete)}", $"minecraft:{nameof(Textures.brown_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_13", "Green Concrete", 251, 13, Textures.green_concrete.ToPxBitmap(), Textures.green_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.green_concrete)}", $"minecraft:{nameof(Textures.green_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_14", "Red Concrete", 251, 14, Textures.red_concrete.ToPxBitmap(), Textures.red_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.red_concrete)}", $"minecraft:{nameof(Textures.red_concrete)}", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_15", "Black Concrete", 251, 15, Textures.black_concrete.ToPxBitmap(), Textures.black_concrete.ToPxBitmap(), $"minecraft:{nameof(Textures.black_concrete)}", $"minecraft:{nameof(Textures.black_concrete)}", "minecraft:concrete"),

                        new Material("1.12", false, "Powder", "PWDR_00", "White Powder", 252, 0, Textures.white_concrete_powder.ToPxBitmap(), Textures.white_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.white_concrete_powder)}", $"minecraft:{nameof(Textures.white_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_01", "Orange Powder", 252, 1, Textures.orange_concrete_powder.ToPxBitmap(), Textures.orange_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.orange_concrete_powder)}", $"minecraft:{nameof(Textures.orange_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_02", "Magenta Powder", 252, 2, Textures.magenta_concrete_powder.ToPxBitmap(), Textures.magenta_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.magenta_concrete_powder)}", $"minecraft:{nameof(Textures.magenta_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_03", "Light Blue Powder", 252, 3, Textures.light_blue_concrete_powder.ToPxBitmap(), Textures.light_blue_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.light_blue_concrete_powder)}", $"minecraft:{nameof(Textures.light_blue_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_04", "Yellow Powder", 252, 4, Textures.yellow_concrete_powder.ToPxBitmap(), Textures.yellow_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.yellow_concrete_powder)}", $"minecraft:{nameof(Textures.yellow_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_05", "Lime Powder", 252, 5, Textures.lime_concrete_powder.ToPxBitmap(), Textures.lime_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.lime_concrete_powder)}", $"minecraft:{nameof(Textures.lime_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_06", "Pink Powder", 252, 6, Textures.pink_concrete_powder.ToPxBitmap(), Textures.pink_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.pink_concrete_powder)}", $"minecraft:{nameof(Textures.pink_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_07", "Gray Powder", 252, 7, Textures.gray_concrete_powder.ToPxBitmap(), Textures.gray_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.gray_concrete_powder)}", $"minecraft:{nameof(Textures.gray_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_08", "Light Gray Powder", 252, 8, Textures.light_gray_concrete_powder.ToPxBitmap(), Textures.light_gray_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.light_gray_concrete_powder)}", $"minecraft:{nameof(Textures.light_gray_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_09", "Cyan Powder", 252, 9, Textures.cyan_concrete_powder.ToPxBitmap(), Textures.cyan_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.cyan_concrete_powder)}", $"minecraft:{nameof(Textures.cyan_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_10", "Purple Powder", 252, 10, Textures.purple_concrete_powder.ToPxBitmap(), Textures.purple_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.purple_concrete_powder)}", $"minecraft:{nameof(Textures.purple_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_11", "Blue Powder", 252, 11, Textures.blue_concrete_powder.ToPxBitmap(), Textures.blue_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.blue_concrete_powder)}", $"minecraft:{nameof(Textures.blue_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_12", "Brown Powder", 252, 12, Textures.brown_concrete_powder.ToPxBitmap(), Textures.brown_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.brown_concrete_powder)}", $"minecraft:{nameof(Textures.brown_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_13", "Green Powder", 252, 13, Textures.green_concrete_powder.ToPxBitmap(), Textures.green_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.green_concrete_powder)}", $"minecraft:{nameof(Textures.green_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_14", "Red Powder", 252, 14, Textures.red_concrete_powder.ToPxBitmap(), Textures.red_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.red_concrete_powder)}", $"minecraft:{nameof(Textures.red_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_15", "Black Powder", 252, 15, Textures.black_concrete_powder.ToPxBitmap(), Textures.black_concrete_powder.ToPxBitmap(), $"minecraft:{nameof(Textures.black_concrete_powder)}", $"minecraft:{nameof(Textures.black_concrete_powder)}", "minecraft:concrete_powder"),
                        new Material("1.7", false, "Powder", "SAND_00", "zzSand", 12, 0, Textures.sand.ToPxBitmap(), Textures.sand.ToPxBitmap(), $"minecraft:{nameof(Textures.sand)}", $"minecraft:{nameof(Textures.sand)}", "minecraft:sand"),
                        new Material("1.7", false, "Powder", "SAND_01", "zzSand Red", 12, 1, Textures.red_sand.ToPxBitmap(), Textures.red_sand.ToPxBitmap(), $"minecraft:{nameof(Textures.red_sand)}", $"minecraft:{nameof(Textures.red_sand)}", "minecraft:sand"),

                        new Material("1.7", false, "Clay", "TERRA_00", "White Clay", 159, 0, Textures.white_terracotta.ToPxBitmap(), Textures.white_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.white_terracotta)}", $"minecraft:{nameof(Textures.white_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_01", "Orange Clay", 159, 1, Textures.orange_terracotta.ToPxBitmap(), Textures.orange_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.orange_terracotta)}", $"minecraft:{nameof(Textures.orange_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_02", "Magenta Clay", 159, 2, Textures.magenta_terracotta.ToPxBitmap(), Textures.magenta_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.magenta_terracotta)}", $"minecraft:{nameof(Textures.magenta_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_03", "Light Blue Clay", 159, 3, Textures.light_blue_terracotta.ToPxBitmap(), Textures.light_blue_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.light_blue_terracotta)}", $"minecraft:{nameof(Textures.light_blue_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_04", "Yellow Clay", 159, 4, Textures.yellow_terracotta.ToPxBitmap(), Textures.yellow_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.yellow_terracotta)}", $"minecraft:{nameof(Textures.yellow_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_05", "Lime Clay", 159, 5, Textures.lime_terracotta.ToPxBitmap(), Textures.lime_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.lime_terracotta)}", $"minecraft:{nameof(Textures.lime_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_06", "Pink Clay", 159, 6, Textures.pink_terracotta.ToPxBitmap(), Textures.pink_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.pink_terracotta)}", $"minecraft:{nameof(Textures.pink_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_07", "Gray Clay", 159, 7, Textures.gray_terracotta.ToPxBitmap(), Textures.gray_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.gray_terracotta)}", $"minecraft:{nameof(Textures.gray_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_08", "Light Gray Clay", 159, 8, Textures.light_gray_terracotta.ToPxBitmap(), Textures.light_gray_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.light_gray_terracotta)}", $"minecraft:{nameof(Textures.light_gray_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_09", "Cyan Clay", 159, 9, Textures.cyan_terracotta.ToPxBitmap(), Textures.cyan_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.cyan_terracotta)}", $"minecraft:{nameof(Textures.cyan_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_10", "Purple Clay", 159, 10, Textures.purple_terracotta.ToPxBitmap(), Textures.purple_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.purple_terracotta)}", $"minecraft:{nameof(Textures.purple_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_11", "Blue Clay", 159, 11, Textures.blue_terracotta.ToPxBitmap(), Textures.blue_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.blue_terracotta)}", $"minecraft:{nameof(Textures.blue_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_12", "Brown Clay", 159, 12, Textures.brown_terracotta.ToPxBitmap(), Textures.brown_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.brown_terracotta)}", $"minecraft:{nameof(Textures.brown_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_13", "Green Clay", 159, 13, Textures.green_terracotta.ToPxBitmap(), Textures.green_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.green_terracotta)}", $"minecraft:{nameof(Textures.green_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_14", "Red Clay", 159, 14, Textures.red_terracotta.ToPxBitmap(), Textures.red_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.red_terracotta)}", $"minecraft:{nameof(Textures.red_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_15", "Black Clay", 159, 15, Textures.black_terracotta.ToPxBitmap(), Textures.black_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.black_terracotta)}", $"minecraft:{nameof(Textures.black_terracotta)}", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "CLAY_HARD_00", "zzHardened Clay", 172, 0, Textures.terracotta.ToPxBitmap(), Textures.terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.terracotta)}", $"minecraft:{nameof(Textures.terracotta)}", "minecraft:hardened_clay"),
                        new Material("1.7", false, "Clay", "CLAY_SOFT_00", "zzClay", 82, 0, Textures.clay.ToPxBitmap(), Textures.clay.ToPxBitmap(), $"minecraft:{nameof(Textures.clay)}", $"minecraft:{nameof(Textures.clay)}", "minecraft:clay"),

                        new Material("1.7", false, "Planks", "PLANK_OAK", "Planks Oak", 5, 0, Textures.oak_planks.ToPxBitmap(), Textures.oak_planks.ToPxBitmap(), $"minecraft:{nameof(Textures.oak_planks)}", $"minecraft:{nameof(Textures.oak_planks)}", "minecraft:planks"),
                        new Material("1.7", false, "Planks", "PLANK_SPR", "Planks Spruce", 5, 1, Textures.spruce_planks.ToPxBitmap(), Textures.spruce_planks.ToPxBitmap(), $"minecraft:{nameof(Textures.spruce_planks)}", $"minecraft:{nameof(Textures.spruce_planks)}", "minecraft:planks"),
                        new Material("1.7", false, "Planks", "PLANK_BIR", "Planks Birch", 5, 2, Textures.birch_planks.ToPxBitmap(), Textures.birch_planks.ToPxBitmap(), $"minecraft:{nameof(Textures.birch_planks)}", $"minecraft:{nameof(Textures.birch_planks)}", "minecraft:planks"),
                        new Material("1.7", false, "Planks", "PLANK_JUN", "Planks Jungle", 5, 3, Textures.jungle_planks.ToPxBitmap(), Textures.jungle_planks.ToPxBitmap(), $"minecraft:{nameof(Textures.jungle_planks)}", $"minecraft:{nameof(Textures.jungle_planks)}", "minecraft:planks"),
                        new Material("1.7", false, "Planks", "PLANK_ACA", "Planks Acacia", 5, 4, Textures.acacia_planks.ToPxBitmap(), Textures.acacia_planks.ToPxBitmap(), $"minecraft:{nameof(Textures.acacia_planks)}", $"minecraft:{nameof(Textures.acacia_planks)}", "minecraft:planks"),
                        new Material("1.7", false, "Planks", "PLANK_DOK", "Planks Dark Oak", 5, 5, Textures.dark_oak_planks.ToPxBitmap(), Textures.dark_oak_planks.ToPxBitmap(), $"minecraft:{nameof(Textures.dark_oak_planks)}", $"minecraft:{nameof(Textures.dark_oak_planks)}", "minecraft:planks"),
                        new Material("1.16", false, "Planks", "PLANK_CRIMSON", "Planks Crimson", 166, 0, Textures.crimson_planks.ToPxBitmap(), Textures.crimson_planks.ToPxBitmap(), $"minecraft:{nameof(Textures.crimson_planks)}", $"minecraft:{nameof(Textures.crimson_planks)}", ""),
                        new Material("1.16", false, "Planks", "PLANK_WARPED", "Planks Warped", 166, 0, Textures.warped_planks.ToPxBitmap(), Textures.warped_planks.ToPxBitmap(), $"minecraft:{nameof(Textures.warped_planks)}", $"minecraft:{nameof(Textures.warped_planks)}", ""),

                        new Material("1.13", false, "Stripped", "STRIP_LOG_OAK", "Stripped Oak", 17, 0, Textures.stripped_oak_log.ToPxBitmap(), Textures.stripped_oak_log.ToPxBitmap(), $"minecraft:{nameof(Textures.stripped_oak_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_oak_log)}[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_SPR", "Stripped Spruce", 17, 0, Textures.stripped_spruce_log.ToPxBitmap(), Textures.stripped_spruce_log.ToPxBitmap(), $"minecraft:{nameof(Textures.stripped_spruce_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_spruce_log)}[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_BIR", "Stripped Birch", 17, 0, Textures.stripped_birch_log.ToPxBitmap(), Textures.stripped_birch_log.ToPxBitmap(), $"minecraft:{nameof(Textures.stripped_birch_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_birch_log)}[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_JUN", "Stripped Jungle", 17, 0, Textures.stripped_jungle_log.ToPxBitmap(), Textures.stripped_jungle_log.ToPxBitmap(), $"minecraft:{nameof(Textures.stripped_jungle_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_jungle_log)}[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_ACA", "Stripped Acacia", 17, 0, Textures.stripped_acacia_log.ToPxBitmap(), Textures.stripped_acacia_log.ToPxBitmap(), $"minecraft:{nameof(Textures.stripped_acacia_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_acacia_log)}[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_DOK", "Stripped Dark Oak", 17, 0, Textures.stripped_dark_oak_log.ToPxBitmap(), Textures.stripped_dark_oak_log.ToPxBitmap(), $"minecraft:{nameof(Textures.stripped_dark_oak_log)}[axis=x]", $"minecraft:{nameof(Textures.stripped_dark_oak_log)}[axis=x]", ""),
                        new Material("1.16", false, "Stripped", "STRIP_LOG_CRIMSON", "Stripped Crimson Hyphae", 166, 0, Textures.stripped_crimson_stem.ToPxBitmap(), Textures.stripped_crimson_stem.ToPxBitmap(), $"minecraft:stripped_crimson_hyphae[axis=x]", $"minecraft:stripped_crimson_hyphae[axis=x]", ""),
                        new Material("1.16", false, "Stripped", "STRIP_LOG_WARPED", "Stripped Warped Hyphae", 166, 0, Textures.stripped_warped_stem.ToPxBitmap(), Textures.stripped_warped_stem.ToPxBitmap(), $"minecraft:stripped_warped_hyphae[axis=x]", $"minecraft:stripped_warped_hyphae[axis=x]", ""),

                        new Material("1.7", false, "Logs Top", "STUMP_LOG_OAK", "Stripped Oak (Top)", 17, 0, Textures.stripped_oak_log_top.ToPxBitmap(), Textures.stripped_oak_log_top.ToPxBitmap(), $"minecraft:stripped_oak_log[axis=y]", $"minecraft:stripped_oak_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_SPR", "Stripped Spruce (Top)", 17, 0, Textures.stripped_spruce_log_top.ToPxBitmap(), Textures.stripped_spruce_log_top.ToPxBitmap(), $"minecraft:stripped_spruce_log[axis=y]", $"minecraft:stripped_spruce_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_BIR", "Stripped Birch (Top)", 17, 0, Textures.stripped_birch_log_top.ToPxBitmap(), Textures.stripped_birch_log_top.ToPxBitmap(), $"minecraft:stripped_birch_log[axis=y]", $"minecraft:stripped_birch_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_JUN", "Stripped Jungle (Top)", 17, 0, Textures.stripped_jungle_log_top.ToPxBitmap(), Textures.stripped_jungle_log_top.ToPxBitmap(), $"minecraft:stripped_jungle_log[axis=y]", $"minecraft:stripped_jungle_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_ACA", "Stripped Acacia (Top)", 17, 0, Textures.stripped_acacia_log_top.ToPxBitmap(), Textures.stripped_acacia_log_top.ToPxBitmap(), $"minecraft:stripped_acacia_log[axis=y]", $"minecraft:stripped_acacia_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_DOK", "Stripped Dark Oak (Top)", 17, 0, Textures.stripped_dark_oak_log_top.ToPxBitmap(), Textures.stripped_dark_oak_log_top.ToPxBitmap(), $"minecraft:stripped_dark_oak_log[axis=y]", $"minecraft:stripped_dark_oak_log[axis=z]", ""),
                        new Material("1.16", false, "Logs Top", "STUMP_LOG_CRIMSON", "Crimson Stem (Top)", 166, 0, Textures.stripped_crimson_stem_top.ToPxBitmap(), Textures.stripped_crimson_stem_top.ToPxBitmap(), $"minecraft:stripped_crimson_stem[axis=y]", $"minecraft:stripped_crimson_stem[axis=z]", ""),
                        new Material("1.16", false, "Logs Top", "STUMP_LOG_WARPED", "Warped Stem (Top)", 166, 0, Textures.stripped_warped_stem_top.ToPxBitmap(), Textures.stripped_warped_stem_top.ToPxBitmap(), $"minecraft:stripped_warped_stem[axis=y]", $"minecraft:stripped_warped_stem[axis=z]", ""),

                        new Material("1.13", false, "Logs", "BARK_LOG_OAK", "Bark Oak", 17, 0, Textures.oak_log.ToPxBitmap(), Textures.oak_log.ToPxBitmap(), $"minecraft:{nameof(Textures.oak_log)}[axis=x]", $"minecraft:{nameof(Textures.oak_log)}[axis=x]", ""),
                        new Material("1.13", false, "Logs", "BARK_LOG_SPR", "Bark Spruce", 17, 0, Textures.spruce_log.ToPxBitmap(), Textures.spruce_log.ToPxBitmap(), $"minecraft:{nameof(Textures.spruce_log)}[axis=x]", $"minecraft:{nameof(Textures.spruce_log)}[axis=x]", ""),
                        new Material("1.13", true, "Logs", "BARK_LOG_BIR", "Bark Birch", 17, 0, Textures.birch_log.ToPxBitmap(), Textures.birch_log.ToPxBitmap(), $"minecraft:{nameof(Textures.birch_log)}[axis=x]", $"minecraft:{nameof(Textures.birch_log)}[axis=x]", ""),
                        new Material("1.13", false, "Logs", "BARK_LOG_JUN", "Bark Jungle", 17, 0, Textures.jungle_log.ToPxBitmap(), Textures.jungle_log.ToPxBitmap(), $"minecraft:{nameof(Textures.jungle_log)}[axis=x]", $"minecraft:{nameof(Textures.jungle_log)}[axis=x]", ""),
                        new Material("1.13", false, "Logs", "BARK_LOG_ACA", "Bark Acacia", 17, 0, Textures.acacia_log.ToPxBitmap(), Textures.acacia_log.ToPxBitmap(), $"minecraft:{nameof(Textures.acacia_log)}[axis=x]", $"minecraft:{nameof(Textures.acacia_log)}[axis=x]", ""),
                        new Material("1.13", false, "Logs", "BARK_LOG_DOK", "Bark Dark Oak", 17, 0, Textures.dark_oak_log.ToPxBitmap(), Textures.dark_oak_log.ToPxBitmap(), $"minecraft:{nameof(Textures.dark_oak_log)}[axis=x]", $"minecraft:{nameof(Textures.dark_oak_log)}[axis=x]", ""),
                        new Material("1.16", false, "Logs", "BARK_LOG_CRIMSON", "Crimson Hyphae", 166, 0, Textures.crimson_stem.ToPxBitmap(), Textures.crimson_stem.ToPxBitmap(), $"minecraft:crimson_hyphae", $"minecraft:crimson_hyphae", ""),
                        new Material("1.16", false, "Logs", "BARK_LOG_WARPED", "Warped Hyphae", 166, 0, Textures.warped_stem.ToPxBitmap(), Textures.warped_stem.ToPxBitmap(), $"minecraft:warped_hyphae", $"minecraft:warped_hyphae", ""),

                        new Material("1.7", false, "Mushrooms", "SHROOM_INNER", "Mushroom Inside", 100, 0, Textures.mushroom_block_inside.ToPxBitmap(), Textures.mushroom_block_inside.ToPxBitmap(), $"minecraft:mushroom_stem[down=false,east=false,west=false,north=false,south=false,up=false]", $"minecraft:mushroom_stem[down=false,east=false,west=false,north=false,south=false,up=false]", "minecraft:brown_mushroom_block"),
                        new Material("1.7", false, "Mushrooms", "SHROOM_BROWN", "Brown Mushroom", 99, 14, Textures.brown_mushroom_block.ToPxBitmap(), Textures.brown_mushroom_block.ToPxBitmap(), $"minecraft:{nameof(Textures.brown_mushroom_block)}[down=true.ToPxBitmap(),east=true,west=true,north=true,south=true,up=true]", $"minecraft:{nameof(Textures.brown_mushroom_block)}[down=true.ToPxBitmap(),east=true,west=true,north=true,south=true,up=true]", "minecraft:brown_mushroom_block"),
                        new Material("1.7", false, "Mushrooms", "SHROOM_STEM", "Mushroom Stem", 100, 15, Textures.mushroom_stem.ToPxBitmap(), Textures.mushroom_stem.ToPxBitmap(), $"minecraft:{nameof(Textures.mushroom_stem)}[down=true.ToPxBitmap(),east=true,west=true,north=true,south=true,up=true]", $"minecraft:{nameof(Textures.mushroom_stem)}[down=true.ToPxBitmap(),east=true,west=true,north=true,south=true,up=true]", "minecraft:red_mushroom_block"),
                        new Material("1.7", false, "Mushrooms", "SHROOM_RED", "Red Mushroom Block", 100, 14, Textures.red_mushroom_block.ToPxBitmap(), Textures.red_mushroom_block.ToPxBitmap(), $"minecraft:{nameof(Textures.red_mushroom_block)}[down=true.ToPxBitmap(),east=true,west=true,north=true,south=true,up=true]", $"minecraft:{nameof(Textures.red_mushroom_block)}[down=true.ToPxBitmap(),east=true,west=true,north=true,south=true,up=true]", "minecraft:red_mushroom_block"),

                        new Material("1.10", false, "Good", "NETHER_WART", "Netherwart Block", 214, 0, Textures.nether_wart_block.ToPxBitmap(), Textures.nether_wart_block.ToPxBitmap(), $"minecraft:{nameof(Textures.nether_wart_block)}", $"minecraft:{nameof(Textures.nether_wart_block)}", "minecraft:nether_wart_block"),
                        new Material("1.16", false, "Good", "NETHER_WART_WARPED", "Warped Netherwart", 166, 0, Textures.warped_wart_block.ToPxBitmap(), Textures.warped_wart_block.ToPxBitmap(), $"minecraft:{nameof(Textures.warped_wart_block)}", $"minecraft:{nameof(Textures.warped_wart_block)}", ""),
                        new Material("1.8", false, "Good", "SMOOTH_RED_SANDSTONE", "Smooth Red Sandstone", 179, 0, Textures.red_sandstone_top.ToPxBitmap(), Textures.red_sandstone_top.ToPxBitmap(), $"minecraft:smooth_red_sandstone", $"minecraft:smooth_red_sandstone", "minecraft:red_sandstone"),
                        new Material("1.7", false, "Good", "SMOOTH_SANDSTONE", "Smooth Sandstone", 24, 2, Textures.sandstone_top.ToPxBitmap(), Textures.sandstone_top.ToPxBitmap(), $"minecraft:smooth_sandstone", $"minecraft:smooth_sandstone", "minecraft:sandstone"),
                        new Material("1.7", false, "Good", "SNOW_BLK", "Snow", 80, 0, Textures.snow.ToPxBitmap(), Textures.snow.ToPxBitmap(), $"minecraft:{nameof(Textures.snow)}_block", $"minecraft:{nameof(Textures.snow)}_block", "minecraft:snow"),
                        new Material("1.7", false, "Good", "HAY_BLK_TOP", "Hay Block (Top)", 170, 0, Textures.hay_block_top.ToPxBitmap(), Textures.hay_block_top.ToPxBitmap(), $"minecraft:hay_block[axis=y]", $"minecraft:hay_block[axis=z]", ""),
                        new Material("1.7", true, "Good", "HAY_BLK", "Hay Block", 170, 4, Textures.hay_block_side.ToPxBitmap(), Textures.hay_block_side.ToPxBitmap(), $"minecraft:hay_block[axis=x]", $"minecraft:hay_block[axis=x]", "minecraft:hay_block"),
                        new Material("1.7", false, "Good", "SMOOTH_QRTZ", "Smooth Quartz", 155, 0, Textures.quartz_block_bottom.ToPxBitmap(), Textures.quartz_block_bottom.ToPxBitmap(), $"minecraft:smooth_quartz", $"minecraft:smooth_quartz", "minecraft:quartz_block"),

                        new Material("1.15", true, "Okay", "HONEY_BLK", "Honey Block", 473, 0, Textures.honey_block_top.ToPxBitmap(), Textures.honey_block_side.ToPxBitmap(), $"minecraft:honey_block", $"minecraft:honey_block", ""),
                        new Material("1.15", false, "Okay", "HONEYCOMB", "Honeycomb Block", 166, 0, Textures.honeycomb_block.ToPxBitmap(), Textures.honeycomb_block.ToPxBitmap(), $"minecraft:{nameof(Textures.honeycomb_block)}", $"minecraft:{nameof(Textures.honeycomb_block)}", ""),
                        new Material("1.15", false, "Okay", "BEE_HIVE", "Bee Hive", 474, 0, Textures.beehive_end.ToPxBitmap(), Textures.beehive_side.ToPxBitmap(), $"minecraft:beehive[facing=north]", $"minecraft:beehive[facing=north]", ""),
                        new Material("1.15", true, "Okay", "BEE_NEST", "Bee Nest", 473, 0, Textures.bee_nest_top.ToPxBitmap(), Textures.bee_nest_side.ToPxBitmap(), $"minecraft:bee_nest[facing=west]", $"minecraft:bee_nest[facing=west]", ""),
                        new Material("1.7", false, "Okay", "BEDROCK", "Bedrock", 7, 0, Textures.bedrock.ToPxBitmap(), Textures.bedrock.ToPxBitmap(), $"minecraft:{nameof(Textures.bedrock)}", $"minecraft:{nameof(Textures.bedrock)}", "minecraft:bedrock"),
                        new Material("1.7", false, "Okay", "COBBLE", "Cobblestone", 4, 0, Textures.cobblestone.ToPxBitmap(), Textures.cobblestone.ToPxBitmap(), $"minecraft:{nameof(Textures.cobblestone)}", $"minecraft:{nameof(Textures.cobblestone)}", "minecraft:cobblestone"),
                        new Material("1.17", false, "Okay", "MOSS_BLOCK", "Moss Block", 1, 0, Textures.moss_block.ToPxBitmap(), Textures.moss_block.ToPxBitmap(), $"minecraft:{nameof(Textures.moss_block)}", $"minecraft:{nameof(Textures.moss_block)}", ""),
                        new Material("1.17", false, "Okay", "DIRT_ROOTED", "Rooted Dirt", 3, 1, Textures.rooted_dirt.ToPxBitmap(), Textures.rooted_dirt.ToPxBitmap(), $"minecraft:{nameof(Textures.rooted_dirt)}", $"minecraft:{nameof(Textures.rooted_dirt)}", ""),
                        new Material("1.7", false, "Okay", "DIRT", "Dirt", 3, 0, Textures.dirt.ToPxBitmap(), Textures.dirt.ToPxBitmap(), $"minecraft:{nameof(Textures.dirt)}", $"minecraft:{nameof(Textures.dirt)}", "minecraft:dirt"),
                        new Material("1.17", false, "Okay", "DRIPSTONE", "Dripstone", 3, 0, Textures.dripstone_block.ToPxBitmap(), Textures.dripstone_block.ToPxBitmap(), $"minecraft:{nameof(Textures.dripstone_block)}", $"minecraft:{nameof(Textures.dripstone_block)}", "minecraft:dripstone_block"),
                        new Material("1.8", false, "Okay", "DIRT_COARSE", "Coarse Dirt", 3, 1, Textures.coarse_dirt.ToPxBitmap(), Textures.coarse_dirt.ToPxBitmap(), $"minecraft:{nameof(Textures.coarse_dirt)}", $"minecraft:{nameof(Textures.coarse_dirt)}", "minecraft:dirt"),
                        new Material("1.7", false, "Okay", "DIRT_Podzol", "Podzol", 3, 2, Textures.podzol_top.ToPxBitmap(), Textures.podzol_side.ToPxBitmap(), $"minecraft:podzol", $"minecraft:podzol", "minecraft:dirt"),
                        new Material("1.7", false, "Okay", "ENDSTONE", "Endstone", 121, 0, Textures.end_stone.ToPxBitmap(), Textures.end_stone.ToPxBitmap(), $"minecraft:{nameof(Textures.end_stone)}", $"minecraft:{nameof(Textures.end_stone)}", "minecraft:end_stone"),
                        new Material("1.10", false, "Okay", "MAGMA", "Magma", 213, 0, Textures.magma.ToPxBitmap(), Textures.magma.ToPxBitmap(), $"minecraft:{nameof(Textures.magma)}_block", $"minecraft:{nameof(Textures.magma)}_block", "minecraft:magma"),
                        new Material("1.7", false, "Okay", "NETHERRACK", "Netherrack", 87, 0, Textures.netherrack.ToPxBitmap(), Textures.netherrack.ToPxBitmap(), $"minecraft:{nameof(Textures.netherrack)}", $"minecraft:{nameof(Textures.netherrack)}", "minecraft:netherrack"),
                        new Material("1.7", false, "Okay", "OBSIDIAN", "Obsidian", 49, 0, Textures.obsidian.ToPxBitmap(), Textures.obsidian.ToPxBitmap(), $"minecraft:{nameof(Textures.obsidian)}", $"minecraft:{nameof(Textures.obsidian)}", "minecraft:obsidian"),
                        new Material("1.16", false, "Okay", "OBSIDIAN_CRYING", "Crying Obsidian", 166, 0, Textures.crying_obsidian.ToPxBitmap(), Textures.crying_obsidian.ToPxBitmap(), $"minecraft:{nameof(Textures.crying_obsidian)}", $"minecraft:{nameof(Textures.crying_obsidian)}", ""),
                        new Material("1.9", false, "Okay", "PURPUR_BLK", "Purpur Block", 201, 0, Textures.purpur_block.ToPxBitmap(), Textures.purpur_block.ToPxBitmap(), $"minecraft:{nameof(Textures.purpur_block)}", $"minecraft:{nameof(Textures.purpur_block)}", "minecraft:purpur_block"),
                        new Material("1.7", false, "Okay", "ICE_PACKED", "Packed Ice", 174, 0, Textures.packed_ice.ToPxBitmap(), Textures.packed_ice.ToPxBitmap(), $"minecraft:{nameof(Textures.packed_ice)}", $"minecraft:{nameof(Textures.packed_ice)}", "minecraft:packed_ice"),
                        new Material("1.7", false, "Okay", "SPONGE", "Sponge", 19, 0, Textures.sponge.ToPxBitmap(), Textures.sponge.ToPxBitmap(), $"minecraft:{nameof(Textures.sponge)}", $"minecraft:{nameof(Textures.sponge)}", "minecraft:sponge"),
                        new Material("1.8", false, "Okay", "SPONGE_WET", "Sponge", 19, 1, Textures.wet_sponge.ToPxBitmap(), Textures.wet_sponge.ToPxBitmap(), $"minecraft:{nameof(Textures.wet_sponge)}", $"minecraft:{nameof(Textures.wet_sponge)}", "minecraft:sponge"),
                        new Material("1.7", false, "Okay", "STONE", "Stone", 1, 0, Textures.stone.ToPxBitmap(), Textures.stone.ToPxBitmap(), $"minecraft:{nameof(Textures.stone)}", $"minecraft:{nameof(Textures.stone)}", "minecraft:stone"),
                        new Material("1.13", false, "Okay", "ICE_BLUE", "Blue Ice", 174, 0, Textures.blue_ice.ToPxBitmap(), Textures.blue_ice.ToPxBitmap(), $"minecraft:{nameof(Textures.blue_ice)}", $"minecraft:{nameof(Textures.blue_ice)}", ""),
                        new Material("1.8", false, "Okay", "ANDESITE", "Andesite", 1, 5, Textures.andesite.ToPxBitmap(), Textures.andesite.ToPxBitmap(), $"minecraft:{nameof(Textures.andesite)}", $"minecraft:{nameof(Textures.andesite)}", "minecraft:stone"),
                        new Material("1.8", false, "Okay", "DIORITE", "Diorite", 1, 3, Textures.diorite.ToPxBitmap(), Textures.diorite.ToPxBitmap(), $"minecraft:{nameof(Textures.diorite)}", $"minecraft:{nameof(Textures.diorite)}", "minecraft:stone"),
                        new Material("1.8", false, "Okay", "GRANITE", "Granite", 1, 1, Textures.granite.ToPxBitmap(), Textures.granite.ToPxBitmap(), $"minecraft:{nameof(Textures.granite)}", $"minecraft:{nameof(Textures.granite)}", "minecraft:stone"),
                        new Material("1.16", true, "Okay", "TARGET", "Target", 166, 0, Textures.target_top.ToPxBitmap(), Textures.target_side.ToPxBitmap(), $"minecraft:target", $"minecraft:target", ""),

                        new Material("1.8", false, "Prismarine", "PRISMARINE_DARK", "Dark Prismarine", 168, 2, Textures.dark_prismarine.ToPxBitmap(), Textures.dark_prismarine.ToPxBitmap(), $"minecraft:{nameof(Textures.dark_prismarine)}", $"minecraft:{nameof(Textures.dark_prismarine)}", "minecraft:prismarine"),
                        new Material("1.8", false, "Prismarine", "PRISMARINE_BRICK", "Prismarine Bricks", 168, 1, Textures.prismarine_bricks.ToPxBitmap(), Textures.prismarine_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.prismarine_bricks)}", $"minecraft:{nameof(Textures.prismarine_bricks)}", "minecraft:prismarine"),
                        new Material("1.8", false, "Prismarine", "PRISMARINE_BLK", "Prismarine Block", 168, 0, Textures.prismarine.ToPxBitmap(), Textures.prismarine.ToPxBitmap(), $"minecraft:{nameof(Textures.prismarine)}", $"minecraft:{nameof(Textures.prismarine)}", "minecraft:prismarine"),

                        new Material("1.10", false, "Bricks", "NETHER_BRICK_RED", "Red Nether Bricks", 215, 0, Textures.red_nether_bricks.ToPxBitmap(), Textures.red_nether_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.red_nether_bricks)}", $"minecraft:{nameof(Textures.red_nether_bricks)}", "minecraft:red_nether_brick"),
                        new Material("1.7", false, "Bricks", "NETHER_BRICK", "Nether Brick", 112, 0, Textures.nether_bricks.ToPxBitmap(), Textures.nether_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.nether_bricks)}", $"minecraft:{nameof(Textures.nether_bricks)}", "minecraft:nether_brick"),
                        new Material("1.16", false, "Bricks", "NETHER_BRICK_CHIZ", "Chiseled Nether Brick", 112, 0, Textures.chiseled_nether_bricks.ToPxBitmap(), Textures.chiseled_nether_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.chiseled_nether_bricks)}", $"minecraft:{nameof(Textures.chiseled_nether_bricks)}", ""),
                        new Material("1.16", true, "Bricks", "NETHER_BRICK_CRK", "Cracked Nether Brick", 112, 0, Textures.cracked_nether_bricks.ToPxBitmap(), Textures.cracked_nether_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.cracked_nether_bricks)}", $"minecraft:{nameof(Textures.cracked_nether_bricks)}", ""),
                        new Material("1.16", true, "Bricks", "BLK_STONE_CRK_PLSHD_BRK", "Cracked Polished Blackstone Bricks", 166, 0, Textures.cracked_polished_blackstone_bricks.ToPxBitmap(), Textures.cracked_polished_blackstone_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.cracked_polished_blackstone_bricks)}", $"cracked_polished_blackstone_bricks", ""),
                        new Material("1.16", false, "Bricks", "BLK_STONE_PLSHD_BRK", "Polished Blackstone Bricks", 166, 0, Textures.polished_blackstone_bricks.ToPxBitmap(), Textures.polished_blackstone_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.polished_blackstone_bricks)}", $"minecraft:{nameof(Textures.polished_blackstone_bricks)}", ""),
                        new Material("1.16", false, "Bricks", "BLK_STONE_PLSHD", "Polished Blackstone", 166, 0, Textures.polished_blackstone.ToPxBitmap(), Textures.polished_blackstone.ToPxBitmap(), $"minecraft:{nameof(Textures.polished_blackstone)}", $"minecraft:{nameof(Textures.polished_blackstone)}", ""),
                        new Material("1.16", false, "Bricks", "BLK_STONE_CHISELED", "Chiseled Black Stone", 166, 0, Textures.chiseled_polished_blackstone.ToPxBitmap(), Textures.chiseled_polished_blackstone.ToPxBitmap(), $"minecraft:{nameof(Textures.chiseled_polished_blackstone)}", $"minecraft:{nameof(Textures.chiseled_polished_blackstone)}", ""),
                        new Material("1.16", false, "Bricks", "QUARTZ_BRICK", "Quartz Bricks", 166, 0, Textures.quartz_bricks.ToPxBitmap(), Textures.quartz_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.quartz_bricks)}", $"minecraft:{nameof(Textures.quartz_bricks)}", ""),
                        new Material("1.7", false, "Bricks", "BRICKS_RED", "Bricks", 45, 0, Textures.bricks.ToPxBitmap(), Textures.bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.bricks)}", $"minecraft:{nameof(Textures.bricks)}", "minecraft:brick_block"),
                        new Material("1.7", true, "Bricks", "STONE_BRICK_CRACKED", "cracked_stone_bricks", 98, 2, Textures.cracked_stone_bricks.ToPxBitmap(), Textures.cracked_stone_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.cracked_stone_bricks)}", $"minecraft:{nameof(Textures.cracked_stone_bricks)}", "minecraft:stonebrick"),
                        new Material("1.7", false, "Bricks", "STONE_BRICK", "stone_bricks", 98, 0, Textures.stone_bricks.ToPxBitmap(), Textures.stone_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.stone_bricks)}", $"minecraft:{nameof(Textures.stone_bricks)}", "minecraft:stonebrick"),
                        new Material("1.9", false, "Bricks", "ENDSTONE_BRICK", "end_stone_bricks", 206, 0, Textures.end_stone_bricks.ToPxBitmap(), Textures.end_stone_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.end_stone_bricks)}", $"minecraft:{nameof(Textures.end_stone_bricks)}", "minecraft:end_bricks"),
                        new Material("1.7", false, "Bricks", "COBBLE_MOSSY_BRICK", "mossy_stone_bricks", 98, 1, Textures.mossy_stone_bricks.ToPxBitmap(), Textures.mossy_stone_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.mossy_stone_bricks)}", $"minecraft:{nameof(Textures.mossy_stone_bricks)}", "minecraft:stonebrick"),

                        new Material("1.16", true, "Nether", "LODESTONE", "Lodestone", 166, 0, Textures.lodestone_top.ToPxBitmap(), Textures.lodestone_side.ToPxBitmap(), $"minecraft:lodestone", $"minecraft:lodestone", ""),
                        new Material("1.16", true, "Nether", "SHROOMLIGHT", "Shroomlight", 166, 0, Textures.shroomlight.ToPxBitmap(), Textures.shroomlight.ToPxBitmap(), $"minecraft:{nameof(Textures.shroomlight)}", $"minecraft:{nameof(Textures.shroomlight)}", ""),
                        new Material("1.16", false, "Nether", "SOUL_SOIL", "Soul Soil", 166, 0, Textures.soul_soil.ToPxBitmap(), Textures.soul_soil.ToPxBitmap(), $"minecraft:{nameof(Textures.soul_soil)}", $"minecraft:{nameof(Textures.soul_soil)}", ""),
                        new Material("1.16", false, "Nether", "SOUL_SAND", "Soul Sand", 88, 0, Textures.soul_sand.ToPxBitmap(), Textures.soul_sand.ToPxBitmap(), $"minecraft:{nameof(Textures.soul_sand)}", $"minecraft:{nameof(Textures.soul_sand)}", "minecraft:soul_sand"),
                        new Material("1.16", false, "Nether", "ANCIENT_DEBRIS", "Ancient Debris", 166, 0, Textures.ancient_debris_top.ToPxBitmap(), Textures.ancient_debris_side.ToPxBitmap(), $"minecraft:ancient_debris", $"minecraft:ancient_debris", ""),
                        new Material("1.16", false, "Nether", "BASALT_TOP", "Basalt", 166, 0, Textures.basalt_top.ToPxBitmap(), Textures.basalt_top.ToPxBitmap(), $"minecraft:basalt[axis=y]", $"minecraft:basalt[axis=z]", ""),
                        new Material("1.16", false, "Nether", "BASALT_SIDE", "Basalt", 166, 0, Textures.basalt_side.ToPxBitmap(), Textures.basalt_side.ToPxBitmap(), $"minecraft:basalt[axis=x]", $"minecraft:basalt[axis=x]", ""),
                        new Material("1.16", false, "Nether", "BASALT_POLISHED_TOP", "Basalt (Polished)", 166, 0, Textures.polished_basalt_top.ToPxBitmap(), Textures.polished_basalt_top.ToPxBitmap(), $"minecraft:polished_basalt[axis=y]", $"minecraft:polished_basalt[axis=z]", ""),
                        new Material("1.16", false, "Nether", "BASALT_POLISHED_SIDE", "Basalt (Polished)", 166, 0, Textures.polished_basalt_side.ToPxBitmap(), Textures.polished_basalt_side.ToPxBitmap(), $"minecraft:polished_basalt[axis=x]", $"minecraft:polished_basalt[axis=x]", ""),
                        new Material("1.17", false, "Nether", "BASALT_SMOOTH", "Basalt (Smooth)", 166, 0, Textures.smooth_basalt.ToPxBitmap(), Textures.smooth_basalt.ToPxBitmap(), $"minecraft:smooth_basalt", $"minecraft:smooth_basalt", ""),
                        new Material("1.16", false, "Nether", "NETHERITE_BLOCK", "Netherite Block", 166, 0, Textures.netherite_block.ToPxBitmap(), Textures.netherite_block.ToPxBitmap(), $"minecraft:{nameof(Textures.netherite_block)}", $"minecraft:{nameof(Textures.netherite_block)}", ""),
                        new Material("1.16", false, "Nether", "NYLIUM_CRIMSON", "Crimson Nylium", 166, 0, Textures.crimson_nylium.ToPxBitmap(), Textures.crimson_nylium_side.ToPxBitmap(), $"minecraft:{nameof(Textures.crimson_nylium)}", $"minecraft:{nameof(Textures.crimson_nylium)}", ""),
                        new Material("1.16", false, "Nether", "NYLIUM_WARPED", "Warped Nylium", 166, 0, Textures.warped_nylium.ToPxBitmap(), Textures.warped_nylium_side.ToPxBitmap(), $"minecraft:{nameof(Textures.warped_nylium)}", $"minecraft:{nameof(Textures.warped_nylium)}", ""),
                        new Material("1.16", false, "Nether", "BLK_STONE", "Blackstone", 166, 0, Textures.blackstone_top.ToPxBitmap(), Textures.blackstone.ToPxBitmap(), $"minecraft:blackstone", $"minecraft:blackstone", ""),
                        new Material("1.7", false, "Nether", "QUARTZ_CHISELED", "Chiseled Quartz Block", 155, 1, Textures.chiseled_quartz_block_top.ToPxBitmap(), Textures.chiseled_quartz_block.ToPxBitmap(), $"minecraft:chiseled_quartz_block", $"minecraft:chiseled_quartz_block", "minecraft:quartz_block"),

                        new Material("1.17", false, "Other", "CALCITE", "Calcite", 1, 0, Textures.calcite.ToPxBitmap(), Textures.calcite.ToPxBitmap(), $"minecraft:{nameof(Textures.calcite)}", $"minecraft:{nameof(Textures.calcite)}",""),
                        new Material("1.17", false, "Other", "CHZL_DEEPSLATE", "Chiseled Deepslate", 1, 0, Textures.chiseled_deepslate.ToPxBitmap(), Textures.chiseled_deepslate.ToPxBitmap(), $"minecraft:{nameof(Textures.chiseled_deepslate)}", $"minecraft:{nameof(Textures.chiseled_deepslate)}",""),
                        new Material("1.17", true, "Other", "CRACKED_DEEPSLATE_BRICK", "Cracked Deepslate Bricks", 1, 0, Textures.cracked_deepslate_bricks.ToPxBitmap(), Textures.cracked_deepslate_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.cracked_deepslate_bricks)}", $"minecraft:{nameof(Textures.cracked_deepslate_bricks)}",""),
                        new Material("1.17", true, "Other", "CRACKED_DEEPSLATE_TILE", "Cracked Deepslate Tiles", 1, 0, Textures.cracked_deepslate_tiles.ToPxBitmap(), Textures.cracked_deepslate_tiles.ToPxBitmap(), $"minecraft:{nameof(Textures.cracked_deepslate_tiles)}", $"minecraft:{nameof(Textures.cracked_deepslate_tiles)}",""),
                        new Material("1.17", false, "Other", "COBBLE_DEEPSLATE", "Cobbled Deepslate", 1, 0, Textures.cobbled_deepslate.ToPxBitmap(), Textures.cobbled_deepslate.ToPxBitmap(), $"minecraft:{nameof(Textures.cobbled_deepslate)}", $"minecraft:{nameof(Textures.cobbled_deepslate)}",""),
                        new Material("1.17", false, "Other", "DEEPSLATE", "Deepslate", 1, 0, Textures.deepslate_top.ToPxBitmap(), Textures.deepslate.ToPxBitmap(), $"minecraft:{nameof(Textures.deepslate)}", $"minecraft:{nameof(Textures.deepslate)}",""),
                        new Material("1.17", false, "Other", "DEEPSLATE_BRICK", "Deepslate Bricks", 1, 0, Textures.deepslate_bricks.ToPxBitmap(), Textures.deepslate_bricks.ToPxBitmap(), $"minecraft:{nameof(Textures.deepslate_bricks)}", $"minecraft:{nameof(Textures.deepslate_bricks)}",""),
                        new Material("1.17", false, "Other", "DEEPSLATE_TILE", "Deepslate Tiles", 1, 0, Textures.deepslate_tiles.ToPxBitmap(), Textures.deepslate_tiles.ToPxBitmap(), $"minecraft:{nameof(Textures.deepslate_tiles)}", $"minecraft:{nameof(Textures.deepslate_tiles)}",""),
                        new Material("1.17", false, "Other", "POLISH_DEEPSLATE", "Polished Deepslatee", 1, 0, Textures.polished_deepslate.ToPxBitmap(), Textures.polished_deepslate.ToPxBitmap(), $"minecraft:{nameof(Textures.polished_deepslate)}", $"minecraft:{nameof(Textures.polished_deepslate)}",""),
                        new Material("1.17", false, "Other", "TUFF", "Tuff", 1, 0, Textures.tuff.ToPxBitmap(), Textures.tuff.ToPxBitmap(), $"minecraft:{nameof(Textures.tuff)}", $"minecraft:{nameof(Textures.tuff)}",""),

                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_C", "Coal Block", 173, 0, Textures.coal_block.ToPxBitmap(), Textures.coal_block.ToPxBitmap(), $"minecraft:{nameof(Textures.coal_block)}", $"minecraft:{nameof(Textures.coal_block)}", "minecraft:coal_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_I", "Iron Block", 42, 0, Textures.iron_block.ToPxBitmap(), Textures.iron_block.ToPxBitmap(), $"minecraft:{nameof(Textures.iron_block)}", $"minecraft:{nameof(Textures.iron_block)}", "minecraft:iron_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_G", "Gold Block", 41, 0, Textures.gold_block.ToPxBitmap(), Textures.gold_block.ToPxBitmap(), $"minecraft:{nameof(Textures.gold_block)}", $"minecraft:{nameof(Textures.gold_block)}", "minecraft:gold_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_R", "Redstone Block", 152, 0, Textures.redstone_block.ToPxBitmap(), Textures.redstone_block.ToPxBitmap(), $"minecraft:{nameof(Textures.redstone_block)}", $"minecraft:{nameof(Textures.redstone_block)}", "minecraft:redstone_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_L", "Lapis Block", 22, 0, Textures.lapis_block.ToPxBitmap(), Textures.lapis_block.ToPxBitmap(), $"minecraft:{nameof(Textures.lapis_block)}", $"minecraft:{nameof(Textures.lapis_block)}", "minecraft:lapis_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_D", "Diamond Block", 57, 0, Textures.diamond_block.ToPxBitmap(), Textures.diamond_block.ToPxBitmap(), $"minecraft:{nameof(Textures.diamond_block)}", $"minecraft:{nameof(Textures.diamond_block)}", "minecraft:diamond_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_E", "Emerald Block", 133, 0, Textures.emerald_block.ToPxBitmap(), Textures.emerald_block.ToPxBitmap(), $"minecraft:{nameof(Textures.emerald_block)}", $"minecraft:{nameof(Textures.emerald_block)}", "minecraft:emerald_block"),
                        new Material("1.17", false, "Ores (Solid)", "AMETHYST_BD", "Amethyst Block", 1, 0, Textures.amethyst_block.ToPxBitmap(), Textures.amethyst_block.ToPxBitmap(), $"minecraft:{nameof(Textures.amethyst_block)}", $"minecraft:{nameof(Textures.amethyst_block)}", ""),
                        new Material("1.17", false, "Ores (Solid)", "AMETHYST_BLK", "Budding Amethyst", 1, 0, Textures.budding_amethyst.ToPxBitmap(), Textures.budding_amethyst.ToPxBitmap(), $"minecraft:{nameof(Textures.budding_amethyst)}", $"minecraft:{nameof(Textures.budding_amethyst)}", ""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_BLK", "Copper Block (Waxed)", 1, 0, Textures.copper_block.ToPxBitmap(), Textures.copper_block.ToPxBitmap(), $"minecraft:waxed_copper", $"minecraft:waxed_copper",""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_CUT", "Cut Copper Block (Waxed)", 1, 0, Textures.cut_copper.ToPxBitmap(), Textures.cut_copper.ToPxBitmap(), $"minecraft:waxed_cut_copper", $"minecraft:waxed_cut_copper",""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_EXPO", "Exposed Copper Block (Waxed)", 1, 0, Textures.exposed_copper.ToPxBitmap(), Textures.exposed_copper.ToPxBitmap(), $"minecraft:waxed_exposed_copper", $"minecraft:waxed_exposed_copper",""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_EXPO_CUT", "Exposed Cut Copper Block (Waxed)", 1, 0, Textures.exposed_cut_copper.ToPxBitmap(), Textures.exposed_cut_copper.ToPxBitmap(), $"minecraft:waxed_exposed_cut_copper", $"minecraft:waxed_exposed_cut_copper",""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_OXI", "Oxidized Copper Block (Waxed)", 1, 0, Textures.oxidized_copper.ToPxBitmap(), Textures.oxidized_copper.ToPxBitmap(), $"minecraft:waxed_oxidized_copper", $"minecraft:waxed_oxidized_copper",""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_OXI_CUT", "Oxidized Cut Copper Block (Waxed)", 1, 0, Textures.oxidized_cut_copper.ToPxBitmap(), Textures.oxidized_cut_copper.ToPxBitmap(), $"minecraft:waxed_oxidized_cut_copper", $"minecraft:waxed_oxidized_cut_copper",""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_WTHER", "Weathered Copper Block (Waxed)", 1, 0, Textures.weathered_copper.ToPxBitmap(), Textures.weathered_copper.ToPxBitmap(), $"minecraft:waxed_weathered_copper", $"minecraft:waxed_weathered_copper",""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_WTHER_CUT", "Weathered Cut Copper Block (Waxed)", 1, 0, Textures.weathered_cut_copper.ToPxBitmap(), Textures.weathered_cut_copper.ToPxBitmap(), $"minecraft:waxed_weathered_cut_copper", $"minecraft:waxed_weathered_cut_copper",""),

                        new Material("1.17", false, "Common", "GRASS_PATH", "Dirt Path", 208, 0, Textures.dirt_path_top.ToPxBitmap(), Textures.dirt_path_side.ToPxBitmap(), $"minecraft:dirt_path", $"minecraft:dirt_path", "minecraft:dirt_path"),
                        new Material("1.10", false, "Common", "BONE_BLK", "Bone Block", 216, 4, Textures.bone_block_side.ToPxBitmap(), Textures.bone_block_side.ToPxBitmap(), $"minecraft:bone_block[axis=x]", $"minecraft:bone_block[axis=x]", "minecraft:bone_block"),
                        new Material("1.10", false, "Common", "BONE_BLK_TOP", "Bone Block (Top)", 216, 0, Textures.bone_block_top.ToPxBitmap(), Textures.bone_block_top.ToPxBitmap(), $"minecraft:bone_block[axis=y]", $"minecraft:bone_block[axis=z]", "minecraft:bone_block"),
                        new Material("1.7", false, "Common", "GRAVEL", "gravel", 13, 0, Textures.gravel.ToPxBitmap(), Textures.gravel.ToPxBitmap(), $"minecraft:{nameof(Textures.gravel)}", $"minecraft:{nameof(Textures.gravel)}", "minecraft:gravel"),
                        new Material("1.7", false, "Common", "COBBLE_MOSSY", "mossy_cobblestone", 48, 0, Textures.mossy_cobblestone.ToPxBitmap(), Textures.mossy_cobblestone.ToPxBitmap(), $"minecraft:{nameof(Textures.mossy_cobblestone)}", $"minecraft:{nameof(Textures.mossy_cobblestone)}", "minecraft:mossy_cobblestone"),

                        new Material("1.7", true, "Other", "TNT", "TNT", 46, 0, Textures.tnt_top.ToPxBitmap(), Textures.tnt_side.ToPxBitmap(), $"minecraft:tnt", $"minecraft:tnt", "minecraft:tnt"),
                        new Material("1.8", false, "Other", "ANDESITE_POLISHED", "Andesite (Polished)", 1, 6, Textures.polished_andesite.ToPxBitmap(), Textures.polished_andesite.ToPxBitmap(), $"minecraft:andesite", $"minecraft:andesite", "minecraft:stone"),
                        new Material("1.8", false, "Other", "DIORITE_POLISHED", "Diorite (Polished)", 1, 4, Textures.polished_diorite.ToPxBitmap(), Textures.polished_diorite.ToPxBitmap(), $"minecraft:diorite", $"minecraft:diorite", "minecraft:stone"),
                        new Material("1.8", false, "Other", "GRANITE_POLISHED", "Granite (Polished)", 1, 2, Textures.polished_granite.ToPxBitmap(), Textures.polished_granite.ToPxBitmap(), $"minecraft:granite", $"minecraft:granite", "minecraft:stone"),
                        new Material("1.7", true, "Other", "BOOK_SHELF", "bookshelf", 47, 0, Textures.oak_planks.ToPxBitmap(), Textures.bookshelf.ToPxBitmap(), $"minecraft:bookshelf", $"minecraft:bookshelf", "minecraft:bookshelf"),
                        new Material("1.7", false, "Other", "NOTEBLOCK", "Note Block", 25, 0, Textures.note_block.ToPxBitmap(), Textures.note_block.ToPxBitmap(), $"minecraft:{nameof(Textures.note_block)}", $"minecraft:{nameof(Textures.note_block)}", "minecraft:noteblock"),
                        new Material("1.7", false, "Other", "MELON_BLOCK", "Melon", 103, 0, Textures.melon_top.ToPxBitmap(), Textures.melon_side.ToPxBitmap(), $"minecraft:melon", $"minecraft:melon", "minecraft:melon_block"),
                        new Material("1.7", false, "Other", "PUMPKIN_BLOCK", "Pumpkin", 86, 1, Textures.pumpkin_top.ToPxBitmap(), Textures.pumpkin_side.ToPxBitmap(), $"minecraft:pumpkin", $"minecraft:pumpkin", "minecraft:pumpkin"),
                        new Material("1.7", true, "Other", "PUMPKIN_CARVED", "Pumpkin (Carved)", 86, 0, Textures.pumpkin_top.ToPxBitmap(), Textures.carved_pumpkin.ToPxBitmap(), $"minecraft:carved_pumpkin", $"minecraft:carved_pumpkin", "minecraft:pumpkin"),

                        new Material("1.13", true, "Coral", "CRL_BRAIN", "Brain Coral", 166, 0, Textures.brain_coral_block.ToPxBitmap(), Textures.brain_coral_block.ToPxBitmap(), $"minecraft:{nameof(Textures.brain_coral_block)}", $"minecraft:{nameof(Textures.brain_coral_block)}", ""),
                        new Material("1.13", true, "Coral", "CRL_BUBBLE", "Bubble Coral", 166, 0, Textures.bubble_coral_block.ToPxBitmap(), Textures.bubble_coral_block.ToPxBitmap(), $"minecraft:{nameof(Textures.bubble_coral_block)}", $"minecraft:{nameof(Textures.bubble_coral_block)}", ""),
                        new Material("1.13", true, "Coral", "CRL_FIRE", "Fire Coral", 166, 0, Textures.fire_coral_block.ToPxBitmap(), Textures.fire_coral_block.ToPxBitmap(), $"minecraft:{nameof(Textures.fire_coral_block)}", $"minecraft:{nameof(Textures.fire_coral_block)}", ""),
                        new Material("1.13", true, "Coral", "CRL_HORN", "Horn Coral", 166, 0, Textures.horn_coral_block.ToPxBitmap(), Textures.horn_coral_block.ToPxBitmap(), $"minecraft:{nameof(Textures.horn_coral_block)}", $"minecraft:{nameof(Textures.horn_coral_block)}", ""),
                        new Material("1.13", true, "Coral", "CRL_TUBE", "Tube Coral", 166, 0, Textures.tube_coral_block.ToPxBitmap(), Textures.tube_coral_block.ToPxBitmap(), $"minecraft:{nameof(Textures.tube_coral_block)}", $"minecraft:{nameof(Textures.tube_coral_block)}", ""),
                        new Material("1.13", false, "Coral", "KELP_DRIED", "Dried Kelp", 166, 0, Textures.dried_kelp_top.ToPxBitmap(), Textures.dried_kelp_side.ToPxBitmap(), $"minecraft:dried_kelp_block", $"minecraft:dried_kelp_block", ""),

                        new Material("1.13", false, "Dead Coral", "DEAD_CRL_BRAIN", "Dead Brain Coral", 166, 0, Textures.dead_brain_coral_block.ToPxBitmap(), Textures.dead_brain_coral_block.ToPxBitmap(), $"minecraft:{nameof(Textures.dead_brain_coral_block)}", $"minecraft:{nameof(Textures.dead_brain_coral_block)}", ""),
                        new Material("1.13", false, "Dead Coral", "DEAD_CRL_BUBBLE", "Dead Bubble Coral", 166, 0, Textures.dead_bubble_coral_block.ToPxBitmap(), Textures.dead_bubble_coral_block.ToPxBitmap(), $"minecraft:{nameof(Textures.dead_bubble_coral_block)}", $"minecraft:{nameof(Textures.dead_bubble_coral_block)}", ""),
                        new Material("1.13", false, "Dead Coral", "DEAD_CRL_FIRE", "Dead Fire Coral", 166, 0, Textures.dead_fire_coral_block.ToPxBitmap(), Textures.dead_fire_coral_block.ToPxBitmap(), $"minecraft:{nameof(Textures.dead_fire_coral_block)}", $"minecraft:{nameof(Textures.dead_fire_coral_block)}", ""),
                        new Material("1.13", false, "Dead Coral", "DEAD_CRL_HORN", "Dead Horn Coral", 166, 0, Textures.dead_horn_coral_block.ToPxBitmap(), Textures.dead_horn_coral_block.ToPxBitmap(), $"minecraft:{nameof(Textures.dead_horn_coral_block)}", $"minecraft:{nameof(Textures.dead_horn_coral_block)}", ""),
                        new Material("1.13", false, "Dead Coral", "DEAD_CRL_TUBE", "Dead Tube Coral", 166, 0, Textures.dead_tube_coral_block.ToPxBitmap(), Textures.dead_tube_coral_block.ToPxBitmap(), $"minecraft:{nameof(Textures.dead_tube_coral_block)}", $"minecraft:{nameof(Textures.dead_tube_coral_block)}", ""),

                        new Material("1.7", true, "Ores", "ORE_C", "Coal Ore", 16, 0, Textures.coal_ore.ToPxBitmap(), Textures.coal_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.coal_ore)}", $"minecraft:{nameof(Textures.coal_ore)}", "minecraft:coal_ore"),
                        new Material("1.7", true, "Ores", "ORE_Cu", "Copper Ore", 1, 0, Textures.copper_ore.ToPxBitmap(), Textures.copper_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.copper_ore)}", $"minecraft:{nameof(Textures.copper_ore)}", "minecraft:copper_ore"),
                        new Material("1.7", true, "Ores", "ORE_I", "Iron Ore", 15, 0, Textures.iron_ore.ToPxBitmap(), Textures.iron_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.iron_ore)}", $"minecraft:{nameof(Textures.iron_ore)}", "minecraft:iron_ore"),
                        new Material("1.7", true, "Ores", "ORE_G", "Gold Ore", 14, 0, Textures.gold_ore.ToPxBitmap(), Textures.gold_ore.ToPxBitmap(), $"minecraft:gold_ore", $"minecraft:gold_ore", "minecraft:gold_ore"),
                        new Material("1.7", true, "Ores", "ORE_R", "Redstone Ore", 73, 0, Textures.redstone_ore.ToPxBitmap(), Textures.redstone_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.redstone_ore)}", $"minecraft:{nameof(Textures.redstone_ore)}", "minecraft:redstone_ore"),
                        new Material("1.7", true, "Ores", "ORE_L", "Lapis Ore", 21, 0, Textures.lapis_ore.ToPxBitmap(), Textures.lapis_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.lapis_ore)}", $"minecraft:{nameof(Textures.lapis_ore)}", "minecraft:lapis_ore"),
                        new Material("1.7", true, "Ores", "ORE_D", "Diamond Ore", 56, 0, Textures.diamond_ore.ToPxBitmap(), Textures.diamond_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.diamond_ore)}", $"minecraft:{nameof(Textures.diamond_ore)}", "minecraft:diamond_ore"),
                        new Material("1.7", true, "Ores", "ORE_E", "Emerald Ore", 129, 0, Textures.emerald_ore.ToPxBitmap(), Textures.emerald_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.emerald_ore)}", $"minecraft:{nameof(Textures.emerald_ore)}", "minecraft:emerald_ore"),
                        new Material("1.7", false, "Ores", "ORE_QUARTZ", "Quartz Ore", 153, 0, Textures.nether_quartz_ore.ToPxBitmap(), Textures.nether_quartz_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.nether_quartz_ore)}", $"minecraft:{nameof(Textures.nether_quartz_ore)}", "minecraft:quartz_ore"),
                        new Material("1.16", false, "Ores", "ORE_G_NETHER", "Nether Gold Ore", 166, 0, Textures.nether_gold_ore.ToPxBitmap(), Textures.nether_gold_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.nether_gold_ore)}", $"minecraft:{nameof(Textures.nether_gold_ore)}", ""),
                        new Material("1.16", false, "Ores", "BLK_STONE_GILDED", "Guilded Blackstone", 166, 0, Textures.gilded_blackstone.ToPxBitmap(), Textures.gilded_blackstone.ToPxBitmap(), $"minecraft:{nameof(Textures.gilded_blackstone)}", $"minecraft:{nameof(Textures.gilded_blackstone)}", ""),
                        new Material("1.17", false, "Ores", "ORE_G_RAW", "Raw Gold Block", 41, 0, Textures.raw_gold_block.ToPxBitmap(), Textures.raw_gold_block.ToPxBitmap(), $"minecraft:{nameof(Textures.raw_gold_block)}", $"minecraft:{nameof(Textures.raw_gold_block)}", ""),
                        new Material("1.17", false, "Ores", "ORE_Cp_RAW", "Raw Copper Block", 3, 0, Textures.raw_copper_block.ToPxBitmap(), Textures.raw_copper_block.ToPxBitmap(), $"minecraft:{nameof(Textures.raw_copper_block)}", $"minecraft:{nameof(Textures.raw_copper_block)}", ""),
                        new Material("1.17", false, "Ores", "ORE_I_RAW", "Raw Iron Block", 42, 0, Textures.raw_iron_block.ToPxBitmap(), Textures.raw_iron_block.ToPxBitmap(), $"minecraft:{nameof(Textures.raw_iron_block)}", $"minecraft:{nameof(Textures.raw_iron_block)}", ""),

                        new Material("1.17", true, "Ores (Deepslate)", "ORE_C_DS", "Deepslate Coal Ore", 16, 0, Textures.deepslate_coal_ore.ToPxBitmap(), Textures.deepslate_coal_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.deepslate_coal_ore)}", $"minecraft:{nameof(Textures.deepslate_coal_ore)}", "minecraft:deepslate_coal_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_Cu_DS", "Deepslate Copper Ore", 16, 0, Textures.deepslate_copper_ore.ToPxBitmap(), Textures.deepslate_copper_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.deepslate_copper_ore)}", $"minecraft:{nameof(Textures.deepslate_copper_ore)}", "minecraft:deepslate_copper_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_I_DS", "Deepslate Iron Ore", 15, 0, Textures.deepslate_iron_ore.ToPxBitmap(), Textures.deepslate_iron_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.deepslate_iron_ore)}", $"minecraft:{nameof(Textures.deepslate_iron_ore)}", "minecraft:deepslate_iron_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_G_DS", "Deepslate Gold Ore", 14, 0, Textures.deepslate_gold_ore.ToPxBitmap(), Textures.deepslate_gold_ore.ToPxBitmap(), $"minecraft:gold_ore", $"minecraft:gold_ore", "minecraft:deepslate_gold_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_R_DS", "Deepslate Redstone Ore", 73, 0, Textures.deepslate_redstone_ore.ToPxBitmap(), Textures.deepslate_redstone_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.deepslate_redstone_ore)}", $"minecraft:{nameof(Textures.deepslate_redstone_ore)}", "minecraft:deepslate_redstone_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_L_DS", "Deepslate Lapis Ore", 21, 0, Textures.deepslate_lapis_ore.ToPxBitmap(), Textures.deepslate_lapis_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.deepslate_lapis_ore)}", $"minecraft:{nameof(Textures.deepslate_lapis_ore)}", "minecraft:deepslate_lapis_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_D_DS", "Deepslate Diamond Ore", 56, 0, Textures.deepslate_diamond_ore.ToPxBitmap(), Textures.deepslate_diamond_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.deepslate_diamond_ore)}", $"minecraft:{nameof(Textures.deepslate_diamond_ore)}", "minecraft:deepslate_diamond_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_E_DS", "Deepslate Emerald Ore", 129, 0, Textures.deepslate_emerald_ore.ToPxBitmap(), Textures.deepslate_emerald_ore.ToPxBitmap(), $"minecraft:{nameof(Textures.deepslate_emerald_ore)}", $"minecraft:{nameof(Textures.deepslate_emerald_ore)}", "minecraft:deepslate_emerald_ore"),

                        new Material("1.12", true, "Terracotta", "GLAZED_00", "White Terracotta", 235, 0, Textures.white_glazed_terracotta.ToPxBitmap(), Textures.white_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.white_glazed_terracotta)}", $"minecraft:{nameof(Textures.white_glazed_terracotta)}", "minecraft:white_glazed_terracotta"),
                        new Material("1.12", true, "Terracotta", "GLAZED_01", "Orange Terracotta", 236, 0, Textures.orange_glazed_terracotta.ToPxBitmap(), Textures.orange_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.orange_glazed_terracotta)}", $"minecraft:{nameof(Textures.orange_glazed_terracotta)}", "minecraft:orange_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_02", "Magenta Terracotta", 237, 0, Textures.magenta_glazed_terracotta.ToPxBitmap(), Textures.magenta_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.magenta_glazed_terracotta)}", $"minecraft:{nameof(Textures.magenta_glazed_terracotta)}", "minecraft:magenta_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_03", "Light Blue Terracotta", 238, 0, Textures.light_blue_glazed_terracotta.ToPxBitmap(), Textures.light_blue_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.light_blue_glazed_terracotta)}", $"minecraft:{nameof(Textures.light_blue_glazed_terracotta)}", "minecraft:light_blue_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_04", "Yellow Terracotta", 239, 0, Textures.yellow_glazed_terracotta.ToPxBitmap(), Textures.yellow_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.yellow_glazed_terracotta)}", $"minecraft:{nameof(Textures.yellow_glazed_terracotta)}", "minecraft:yellow_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_05", "Lime Terracotta", 240, 0, Textures.lime_glazed_terracotta.ToPxBitmap(), Textures.lime_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.lime_glazed_terracotta)}", $"minecraft:{nameof(Textures.lime_glazed_terracotta)}", "minecraft:lime_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_06", "Pink Terracotta", 241, 0, Textures.pink_glazed_terracotta.ToPxBitmap(), Textures.pink_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.pink_glazed_terracotta)}", $"minecraft:{nameof(Textures.pink_glazed_terracotta)}", "minecraft:pink_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_07", "Gray Terracotta", 242, 0, Textures.gray_glazed_terracotta.ToPxBitmap(), Textures.gray_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.gray_glazed_terracotta)}", $"minecraft:{nameof(Textures.gray_glazed_terracotta)}", "minecraft:gray_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_08", "Light Gray Terracotta", 243, 0, Textures.light_gray_glazed_terracotta.ToPxBitmap(), Textures.light_gray_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.light_gray_glazed_terracotta)}", $"minecraft:{nameof(Textures.light_gray_glazed_terracotta)}", "minecraft:silver_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_09", "Cyan Terracotta", 244, 0, Textures.cyan_glazed_terracotta.ToPxBitmap(), Textures.cyan_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.cyan_glazed_terracotta)}", $"minecraft:{nameof(Textures.cyan_glazed_terracotta)}", "minecraft:cyan_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_10", "Purple Terracotta", 245, 0, Textures.purple_glazed_terracotta.ToPxBitmap(), Textures.purple_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.purple_glazed_terracotta)}", $"minecraft:{nameof(Textures.purple_glazed_terracotta)}", "minecraft:purple_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_11", "Blue Terracotta", 246, 0, Textures.blue_glazed_terracotta.ToPxBitmap(), Textures.blue_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.blue_glazed_terracotta)}", $"minecraft:{nameof(Textures.blue_glazed_terracotta)}", "minecraft:blue_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_12", "Brown Terracotta", 247, 0, Textures.brown_glazed_terracotta.ToPxBitmap(), Textures.brown_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.brown_glazed_terracotta)}", $"minecraft:{nameof(Textures.brown_glazed_terracotta)}", "minecraft:brown_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_13", "Green Terracotta", 248, 0, Textures.green_glazed_terracotta.ToPxBitmap(), Textures.green_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.green_glazed_terracotta)}", $"minecraft:{nameof(Textures.green_glazed_terracotta)}", "minecraft:green_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_14", "Red Terracotta", 249, 0, Textures.red_glazed_terracotta.ToPxBitmap(), Textures.red_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.red_glazed_terracotta)}", $"minecraft:{nameof(Textures.red_glazed_terracotta)}", "minecraft:red_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_15", "Black Terracotta", 250, 0, Textures.black_glazed_terracotta.ToPxBitmap(), Textures.black_glazed_terracotta.ToPxBitmap(), $"minecraft:{nameof(Textures.black_glazed_terracotta)}", $"minecraft:{nameof(Textures.black_glazed_terracotta)}", "minecraft:black_glazed_terracotta"),

                        new Material("1.7", true, "Carpet", "CARPET_00", "White Carpet", 171, 0, Textures.white_wool.ToPxBitmap(), Textures.white_wool.ToPxBitmap(), $"minecraft:white_carpet", $"minecraft:white_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_01", "Orange Carpet", 171, 1, Textures.orange_wool.ToPxBitmap(), Textures.orange_wool.ToPxBitmap(), $"minecraft:orange_carpet", $"minecraft:orange_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_02", "Magenta Carpet", 171, 2, Textures.magenta_wool.ToPxBitmap(), Textures.magenta_wool.ToPxBitmap(), $"minecraft:magenta_carpet", $"minecraft:magenta_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_03", "Light Blue Carpet", 171, 3, Textures.light_blue_wool.ToPxBitmap(), Textures.light_blue_wool.ToPxBitmap(), $"minecraft:light_blue_carpet", $"minecraft:light_blue_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_04", "Yellow Carpet", 171, 4, Textures.yellow_wool.ToPxBitmap(), Textures.yellow_wool.ToPxBitmap(), $"minecraft:yellow_carpet", $"minecraft:yellow_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_05", "Lime Carpet", 171, 5, Textures.lime_wool.ToPxBitmap(), Textures.lime_wool.ToPxBitmap(), $"minecraft:lime_carpet", $"minecraft:lime_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_06", "Pink Carpet", 171, 6, Textures.pink_wool.ToPxBitmap(), Textures.pink_wool.ToPxBitmap(), $"minecraft:pink_carpet", $"minecraft:pink_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_07", "Gray Carpet", 171, 7, Textures.gray_wool.ToPxBitmap(), Textures.gray_wool.ToPxBitmap(), $"minecraft:gray_carpet", $"minecraft:gray_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_08", "Light Gray Carpet", 171, 8, Textures.light_gray_wool.ToPxBitmap(), Textures.light_gray_wool.ToPxBitmap(), $"minecraft:light_gray_carpet", $"minecraft:light_gray_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_09", "Cyan Carpet", 171, 9, Textures.cyan_wool.ToPxBitmap(), Textures.cyan_wool.ToPxBitmap(), $"minecraft:cyan_carpet", $"minecraft:cyan_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_10", "Purple Carpet", 171, 10, Textures.purple_wool.ToPxBitmap(), Textures.purple_wool.ToPxBitmap(), $"minecraft:purple_carpet", $"minecraft:purple_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_11", "Blue Carpet", 171, 11, Textures.blue_wool.ToPxBitmap(), Textures.blue_wool.ToPxBitmap(), $"minecraft:blue_carpet", $"minecraft:blue_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_12", "Brown Carpet", 171, 12, Textures.brown_wool.ToPxBitmap(), Textures.brown_wool.ToPxBitmap(), $"minecraft:brown_carpet", $"minecraft:brown_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_13", "Green Carpet", 171, 13, Textures.green_wool.ToPxBitmap(), Textures.green_wool.ToPxBitmap(), $"minecraft:green_carpet", $"minecraft:green_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_14", "Red Carpet", 171, 14, Textures.red_wool.ToPxBitmap(), Textures.red_wool.ToPxBitmap(), $"minecraft:red_carpet", $"minecraft:red_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_15", "Black Carpet", 171, 15, Textures.black_wool.ToPxBitmap(), Textures.black_wool.ToPxBitmap(), $"minecraft:black_carpet", $"minecraft:black_carpet", "minecraft:carpet"),
                        new Material("1.17", true, "Carpet", "CARPET_MOSS", "Moss Carpet", 171, 13, Textures.moss_block.ToPxBitmap(), Textures.moss_block.ToPxBitmap(), $"minecraft:moss_carpet", $"minecraft:moss_carpet", ""),
                    };

                    var notUnique = _List.GroupBy(x => x.PixelStackerID).Where(x => x.Count() > 1);
                    if (notUnique.Any())
                    {
                        throw new ArgumentException("All pixelStackerIDs must be unique! [" + string.Join(", ", notUnique) + "]");
                    }

                    _List.ForEach(m =>
                    {
                        List<string> tags = m.Tags;
                        AddTagsForColor(m.GetAverageColor(true), ref tags);
                        AddTagsForColor(m.GetAverageColor(false), ref tags);

                        if (m.IsAdvanced)
                        {
                            tags.Add("IsAdvanced");
                        }

                        // The color brown is not as easy to pin down. The brown range is like a rounded triangle on the color grid.
                        if (
                        m.Category.Contains("Stripped") && m.BlockID == 17
                        || m.Category.Contains("Planks") && m.BlockID == 5
                        || m.Category.Contains("Log") && m.BlockID == 17 && !m.Label.Contains("Birch")
                        || m.PixelStackerID == "SHROOM_INNER"
                        || m.PixelStackerID == "CLAY_HARD_00"
                        || m.PixelStackerID.Contains("GRANITE")
                        || m.PixelStackerID == "DIRT"
                        || m.PixelStackerID == "DRIPSTONE"
                        || m.PixelStackerID == "DIRT_ROOTED"
                        || m.PixelStackerID == "DIRT_COARSE"
                        || m.PixelStackerID == "SOUL_SOIL"
                        || m.PixelStackerID == "SOUL_SAND"
                        )
                        {
                            m.Tags.Add("brown");
                        }

                        m.Tags = tags.Distinct().ToList();
                    });

                    //int n = 1;
                    //int diff = 50;
                    //Materials._List.Where(m => m.Category == "Glass").ToList().ForEach(m => m.Roughness = n * diff); n++;
                    //Materials._List.Where(m => m.Category == "Concrete").ToList().ForEach(m => m.Roughness = n * diff); n++;
                    //Materials._List.Where(m => m.Category == "Clay").ToList().ForEach(m => m.Roughness = n * diff); n++;
                    //Materials._List.Where(m => m.Category == "Powder").ToList().ForEach(m => m.Roughness = n * diff); n++;
                    //Materials._List.Where(m => m.Category == "Wool").ToList().ForEach(m => m.Roughness = n * diff); n++;
                    ////Materials._List.Where(m => m.Roughness == null)
                    ////    .ToList()
                    ////    .ForEach(m => m.Roughness = (int) (n * diff + diff + m.GetTextureRoughness(true)));
                    //Materials._List = Materials._List.OrderBy(x => x.Roughness).ToList();
                }

                return _List;
            }
        }

        private static void AddTagsForColor(PxColor c, ref List<string> tags)
        {
            if (c.A < 240 || c.A < 240) { tags.Add("transparent"); } else { tags.Add("opaque"); }
            var hue = c.GetHue();
            var sat = c.GetSaturation();
            var bri = c.GetBrightness();

            if (sat <= 0.20 || bri <= 0.15 || bri >= 0.85)
            {
                tags.Add("grayscale");
                tags.Add("desaturated");
                tags.Add("gray");

                if (bri <= 0.15)
                {
                    tags.Add("dark");
                    tags.Add("black");
                }
                else if (bri >= 0.85)
                {
                    tags.Add("bright");
                    tags.Add("white");
                    tags.Add("light");
                }

                return;
            }
            else
            {
                tags.Add("colorful");
                if (hue >= 70 && hue <= 155) { tags.Add("green"); }
                if (hue >= 40 && hue <= 65) { tags.Add("yellow"); }
                if (hue >= 20 && hue <= 50) { tags.Add("orange"); }
                if (hue >= 0 && hue <= 25 || hue >= 345 && hue <= 360) { tags.Add("red"); }
                if (hue >= 250 && hue <= 330) { tags.Add("purple"); }
                if (hue >= 160 && hue <= 215) { tags.Add("aqua"); }
                if (hue >= 160 && hue <= 240) { tags.Add("blue"); }
                if ((hue >= 0 && hue <= 25 || hue >= 345 && hue <= 360) && bri > 0.75) { tags.Add("pink"); }
            }
        }
    }
}
