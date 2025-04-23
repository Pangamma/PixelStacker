using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Resources;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

/**
 * 

Crafter


Needs investigation:
Shulker Box
White Shulker Box
Light Gray Shulker Box
Gray Shulker Box
Black Shulker Box
Brown Shulker Box
Red Shulker Box
Orange Shulker Box
Yellow Shulker Box
Lime Shulker Box
Green Shulker Box
Cyan Shulker Box
Light Blue Shulker Box
Blue Shulker Box
Purple Shulker Box
Magenta Shulker Box
Pink Shulker Box

Rejected due to instable state:
Certain states of Creaking Heart
Respawn Anchor
Full Respawn Anchor
Lit Redstone Lamp
Trial Spawner
Trial Vault
Grass Block
Mycelium
Farmland

Oak Leaves
Spruce Leaves
Birch Leaves
Jungle Leaves
Acacia Leaves
Dark Oak Leaves
Mangrove Leaves
Cherry Leaves
Pale Oak Leaves
Azalea Leaves
Flowering Azalea Leaves

https://minecraft.wiki/w/List_of_block_textures

                    https://mcasset.cloud/1.21.5/assets/minecraft/lang
https://github.com/InventivetalentDev/minecraft-assets/blob/1.21.5/assets/minecraft/lang/ja_jp.json

*/
namespace PixelStacker.Logic.Model
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
                     * TODO Go through blocks with edges, see if there are any borderless alternatives.
                        TODO: Add shulker boxes maybe?
                        Add crafter
                     */

                    _List = new List<Material>()
                    {
                        new Material("1.7", false, "Air", "AIR", "Air", Textures.air, Textures.air, $"minecraft:air", $"minecraft:air", "minecraft:air"),

                        new Material("1.7", false, "Glass", "GLASS_00", "White Glass", Textures.GetBitmap("white_stained_glass"), Textures.GetBitmap("white_stained_glass"), $"minecraft:white_stained_glass", $"minecraft:white_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_01", "Orange Glass", Textures.GetBitmap("orange_stained_glass"), Textures.GetBitmap("orange_stained_glass"), $"minecraft:orange_stained_glass", $"minecraft:orange_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_02", "Magenta Glass", Textures.GetBitmap("magenta_stained_glass"), Textures.GetBitmap("magenta_stained_glass"), $"minecraft:magenta_stained_glass", $"minecraft:magenta_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_03", "Light Blue Glass", Textures.GetBitmap("light_blue_stained_glass"), Textures.GetBitmap("light_blue_stained_glass"), $"minecraft:light_blue_stained_glass", $"minecraft:light_blue_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_04", "Yellow Glass", Textures.GetBitmap("yellow_stained_glass"), Textures.GetBitmap("yellow_stained_glass"), $"minecraft:yellow_stained_glass", $"minecraft:yellow_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_05", "Lime Glass", Textures.GetBitmap("lime_stained_glass"), Textures.GetBitmap("lime_stained_glass"), $"minecraft:lime_stained_glass", $"minecraft:lime_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_06", "Pink Glass",  Textures.GetBitmap("pink_stained_glass"), Textures.GetBitmap("pink_stained_glass"), $"minecraft:pink_stained_glass", $"minecraft:pink_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_07", "Gray Glass",Textures.GetBitmap("gray_stained_glass"), Textures.GetBitmap("gray_stained_glass"), $"minecraft:gray_stained_glass", $"minecraft:gray_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_08", "Light Gray Glass",  Textures.GetBitmap("light_gray_stained_glass"), Textures.GetBitmap("light_gray_stained_glass"), $"minecraft:light_gray_stained_glass", $"minecraft:light_gray_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_09", "Cyan Glass", Textures.GetBitmap("cyan_stained_glass"), Textures.GetBitmap("cyan_stained_glass"), $"minecraft:cyan_stained_glass", $"minecraft:cyan_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_10", "Purple Glass",  Textures.GetBitmap("purple_stained_glass"), Textures.GetBitmap("purple_stained_glass"), $"minecraft:purple_stained_glass", $"minecraft:purple_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_11", "Blue Glass",  Textures.GetBitmap("blue_stained_glass"), Textures.GetBitmap("blue_stained_glass"), $"minecraft:blue_stained_glass", $"minecraft:blue_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_12", "Brown Glass", Textures.GetBitmap("brown_stained_glass"), Textures.GetBitmap("brown_stained_glass"), $"minecraft:brown_stained_glass", $"minecraft:brown_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_13", "Green Glass",  Textures.GetBitmap("green_stained_glass"), Textures.GetBitmap("green_stained_glass"), $"minecraft:green_stained_glass", $"minecraft:green_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_14", "Red Glass", Textures.GetBitmap("red_stained_glass"), Textures.GetBitmap("red_stained_glass"), $"minecraft:red_stained_glass", $"minecraft:red_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", false, "Glass", "GLASS_15", "Black Glass", Textures.GetBitmap("black_stained_glass"), Textures.GetBitmap("black_stained_glass"), $"minecraft:black_stained_glass", $"minecraft:black_stained_glass", "minecraft:stained_glass"),
                        new Material("1.7", true, "Glass", "GLASS_CLR", "Clear Glass", Textures.GetBitmap("glass"), Textures.GetBitmap("glass"), $"minecraft:glass", $"minecraft:glass", "minecraft:glass"),
                        new Material("1.17", true, "Glass", "GLASS_TINTED", "Tinted Glass",  Textures.GetBitmap("tinted_glass"), Textures.GetBitmap("tinted_glass"), $"minecraft:tinted_glass", $"minecraft:tinted_glass", ""),

                        new Material("1.7", false, "Wool", "WOOL_00", "White Wool", Textures.GetBitmap("white_wool"), Textures.GetBitmap("white_wool"), $"minecraft:white_wool", $"minecraft:white_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_01", "Orange Wool", Textures.GetBitmap("orange_wool"), Textures.GetBitmap("orange_wool"), $"minecraft:orange_wool", $"minecraft:orange_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_02", "Magenta Wool", Textures.GetBitmap("magenta_wool"), Textures.GetBitmap("magenta_wool"), $"minecraft:magenta_wool", $"minecraft:magenta_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_03", "Light Blue Wool",  Textures.GetBitmap("light_blue_wool"), Textures.GetBitmap("light_blue_wool"), $"minecraft:light_blue_wool", $"minecraft:light_blue_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_04", "Yellow Wool",  Textures.GetBitmap("yellow_wool"), Textures.GetBitmap("yellow_wool"), $"minecraft:yellow_wool", $"minecraft:yellow_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_05", "Lime Wool", Textures.GetBitmap("lime_wool"), Textures.GetBitmap("lime_wool"), $"minecraft:lime_wool", $"minecraft:lime_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_06", "Pink Wool", Textures.GetBitmap("pink_wool"), Textures.GetBitmap("pink_wool"), $"minecraft:pink_wool", $"minecraft:pink_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_07", "Gray Wool",  Textures.GetBitmap("gray_wool"), Textures.GetBitmap("gray_wool"), $"minecraft:gray_wool", $"minecraft:gray_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_08", "Light Gray Wool", Textures.GetBitmap("light_gray_wool"), Textures.GetBitmap("light_gray_wool"), $"minecraft:light_gray_wool", $"minecraft:light_gray_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_09", "Cyan Wool",  Textures.GetBitmap("cyan_wool"), Textures.GetBitmap("cyan_wool"), $"minecraft:cyan_wool", $"minecraft:cyan_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_10", "Purple Wool", Textures.GetBitmap("purple_wool"), Textures.GetBitmap("purple_wool"), $"minecraft:purple_wool", $"minecraft:purple_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_11", "Blue Wool",  Textures.GetBitmap("blue_wool"), Textures.GetBitmap("blue_wool"), $"minecraft:blue_wool", $"minecraft:blue_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_12", "Brown Wool",  Textures.GetBitmap("brown_wool"), Textures.GetBitmap("brown_wool"), $"minecraft:brown_wool", $"minecraft:brown_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_13", "Green Wool",  Textures.GetBitmap("green_wool"), Textures.GetBitmap("green_wool"), $"minecraft:green_wool", $"minecraft:green_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_14", "Red Wool",  Textures.GetBitmap("red_wool"), Textures.GetBitmap("red_wool"), $"minecraft:red_wool", $"minecraft:red_wool", "minecraft:wool"),
                        new Material("1.7", false, "Wool", "WOOL_15", "Black Wool",  Textures.GetBitmap("black_wool"), Textures.GetBitmap("black_wool"), $"minecraft:black_wool", $"minecraft:black_wool", "minecraft:wool"),

                        new Material("1.12", false, "Concrete", "CONC_00", "White Concrete", Textures.GetBitmap("white_concrete"), Textures.GetBitmap("white_concrete"), $"minecraft:white_concrete", $"minecraft:white_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_01", "Orange Concrete", Textures.GetBitmap("orange_concrete"), Textures.GetBitmap("orange_concrete"), $"minecraft:orange_concrete", $"minecraft:orange_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_02", "Magenta Concrete", Textures.GetBitmap("magenta_concrete"), Textures.GetBitmap("magenta_concrete"), $"minecraft:magenta_concrete", $"minecraft:magenta_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_03", "Light Blue Concrete",Textures.GetBitmap("light_blue_concrete"), Textures.GetBitmap("light_blue_concrete"), $"minecraft:light_blue_concrete", $"minecraft:light_blue_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_04", "Yellow Concrete",  Textures.GetBitmap("yellow_concrete"), Textures.GetBitmap("yellow_concrete"), $"minecraft:yellow_concrete", $"minecraft:yellow_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_05", "Lime Concrete",  Textures.GetBitmap("lime_concrete"), Textures.GetBitmap("lime_concrete"), $"minecraft:lime_concrete", $"minecraft:lime_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_06", "Pink Concrete", Textures.GetBitmap("pink_concrete"), Textures.GetBitmap("pink_concrete"), $"minecraft:pink_concrete", $"minecraft:pink_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_07", "Gray Concrete",  Textures.GetBitmap("gray_concrete"), Textures.GetBitmap("gray_concrete"), $"minecraft:gray_concrete", $"minecraft:gray_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_08", "Light Gray Concrete",  Textures.GetBitmap("light_gray_concrete"), Textures.GetBitmap("light_gray_concrete"), $"minecraft:light_gray_concrete", $"minecraft:light_gray_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_09", "Cyan Concrete", Textures.GetBitmap("cyan_concrete"), Textures.GetBitmap("cyan_concrete"), $"minecraft:cyan_concrete", $"minecraft:cyan_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_10", "Purple Concrete",  Textures.GetBitmap("purple_concrete"), Textures.GetBitmap("purple_concrete"), $"minecraft:purple_concrete", $"minecraft:purple_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_11", "Blue Concrete",  Textures.GetBitmap("blue_concrete"), Textures.GetBitmap("blue_concrete"), $"minecraft:blue_concrete", $"minecraft:blue_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_12", "Brown Concrete", Textures.GetBitmap("brown_concrete"), Textures.GetBitmap("brown_concrete"), $"minecraft:brown_concrete", $"minecraft:brown_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_13", "Green Concrete",Textures.GetBitmap("green_concrete"), Textures.GetBitmap("green_concrete"), $"minecraft:green_concrete", $"minecraft:green_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_14", "Red Concrete", Textures.GetBitmap("red_concrete"), Textures.GetBitmap("red_concrete"), $"minecraft:red_concrete", $"minecraft:red_concrete", "minecraft:concrete"),
                        new Material("1.12", false, "Concrete", "CONC_15", "Black Concrete", Textures.GetBitmap("black_concrete"), Textures.GetBitmap("black_concrete"), $"minecraft:black_concrete", $"minecraft:black_concrete", "minecraft:concrete"),

                        new Material("1.12", false, "Powder", "PWDR_00", "White Powder", Textures.GetBitmap("white_concrete_powder"), Textures.GetBitmap("white_concrete_powder"), $"minecraft:white_concrete_powder", $"minecraft:white_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_01", "Orange Powder", Textures.GetBitmap("orange_concrete_powder"), Textures.GetBitmap("orange_concrete_powder"), $"minecraft:orange_concrete_powder", $"minecraft:orange_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_02", "Magenta Powder", Textures.GetBitmap("magenta_concrete_powder"), Textures.GetBitmap("magenta_concrete_powder"), $"minecraft:magenta_concrete_powder", $"minecraft:magenta_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_03", "Light Blue Powder", Textures.GetBitmap("light_blue_concrete_powder"), Textures.GetBitmap("light_blue_concrete_powder"), $"minecraft:light_blue_concrete_powder", $"minecraft:light_blue_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_04", "Yellow Powder", Textures.GetBitmap("yellow_concrete_powder"), Textures.GetBitmap("yellow_concrete_powder"), $"minecraft:yellow_concrete_powder", $"minecraft:yellow_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_05", "Lime Powder", Textures.GetBitmap("lime_concrete_powder"), Textures.GetBitmap("lime_concrete_powder"), $"minecraft:lime_concrete_powder", $"minecraft:lime_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_06", "Pink Powder",  Textures.GetBitmap("pink_concrete_powder"), Textures.GetBitmap("pink_concrete_powder"), $"minecraft:pink_concrete_powder", $"minecraft:pink_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_07", "Gray Powder", Textures.GetBitmap("gray_concrete_powder"), Textures.GetBitmap("gray_concrete_powder"), $"minecraft:gray_concrete_powder", $"minecraft:gray_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_08", "Light Gray Powder", Textures.GetBitmap("light_gray_concrete_powder"), Textures.GetBitmap("light_gray_concrete_powder"), $"minecraft:light_gray_concrete_powder", $"minecraft:light_gray_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_09", "Cyan Powder",  Textures.GetBitmap("cyan_concrete_powder"), Textures.GetBitmap("cyan_concrete_powder"), $"minecraft:cyan_concrete_powder", $"minecraft:cyan_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_10", "Purple Powder",  Textures.GetBitmap("purple_concrete_powder"), Textures.GetBitmap("purple_concrete_powder"), $"minecraft:purple_concrete_powder", $"minecraft:purple_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_11", "Blue Powder",  Textures.GetBitmap("blue_concrete_powder"), Textures.GetBitmap("blue_concrete_powder"), $"minecraft:blue_concrete_powder", $"minecraft:blue_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_12", "Brown Powder", Textures.GetBitmap("brown_concrete_powder"), Textures.GetBitmap("brown_concrete_powder"), $"minecraft:brown_concrete_powder", $"minecraft:brown_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_13", "Green Powder", Textures.GetBitmap("green_concrete_powder"), Textures.GetBitmap("green_concrete_powder"), $"minecraft:green_concrete_powder", $"minecraft:green_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_14", "Red Powder", Textures.GetBitmap("red_concrete_powder"), Textures.GetBitmap("red_concrete_powder"), $"minecraft:red_concrete_powder", $"minecraft:red_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.12", false, "Powder", "PWDR_15", "Black Powder",Textures.GetBitmap("black_concrete_powder"), Textures.GetBitmap("black_concrete_powder"), $"minecraft:black_concrete_powder", $"minecraft:black_concrete_powder", "minecraft:concrete_powder"),
                        new Material("1.7", false, "Powder", "SAND_00", "Sand", Textures.GetBitmap("sand"), Textures.GetBitmap("sand"), $"minecraft:sand", $"minecraft:sand", "minecraft:sand"),
                        new Material("1.7", false, "Powder", "SAND_01", "Sand Red", Textures.GetBitmap("red_sand"), Textures.GetBitmap("red_sand"), $"minecraft:red_sand", $"minecraft:red_sand", "minecraft:sand"),

                        new Material("1.7", false, "Clay", "TERRA_00", "White Terracotta", Textures.GetBitmap("white_terracotta"), Textures.GetBitmap("white_terracotta"), $"minecraft:white_terracotta", $"minecraft:white_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_01", "Orange Terracotta",  Textures.GetBitmap("orange_terracotta"), Textures.GetBitmap("orange_terracotta"), $"minecraft:orange_terracotta", $"minecraft:orange_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_02", "Magenta Terracotta", Textures.GetBitmap("magenta_terracotta"), Textures.GetBitmap("magenta_terracotta"), $"minecraft:magenta_terracotta", $"minecraft:magenta_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_03", "Light Blue Terracotta",  Textures.GetBitmap("light_blue_terracotta"), Textures.GetBitmap("light_blue_terracotta"), $"minecraft:light_blue_terracotta", $"minecraft:light_blue_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_04", "Yellow Terracotta", Textures.GetBitmap("yellow_terracotta"), Textures.GetBitmap("yellow_terracotta"), $"minecraft:yellow_terracotta", $"minecraft:yellow_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_05", "Lime Terracotta",  Textures.GetBitmap("lime_terracotta"), Textures.GetBitmap("lime_terracotta"), $"minecraft:lime_terracotta", $"minecraft:lime_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_06", "Pink Terracotta", Textures.GetBitmap("pink_terracotta"), Textures.GetBitmap("pink_terracotta"), $"minecraft:pink_terracotta", $"minecraft:pink_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_07", "Gray Terracotta",  Textures.GetBitmap("gray_terracotta"), Textures.GetBitmap("gray_terracotta"), $"minecraft:gray_terracotta", $"minecraft:gray_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_08", "Light Gray Terracotta", Textures.GetBitmap("light_gray_terracotta"), Textures.GetBitmap("light_gray_terracotta"), $"minecraft:light_gray_terracotta", $"minecraft:light_gray_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_09", "Cyan Terracotta",  Textures.GetBitmap("cyan_terracotta"), Textures.GetBitmap("cyan_terracotta"), $"minecraft:cyan_terracotta", $"minecraft:cyan_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_10", "Purple Terracotta",  Textures.GetBitmap("purple_terracotta"), Textures.GetBitmap("purple_terracotta"), $"minecraft:purple_terracotta", $"minecraft:purple_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_11", "Blue Terracotta",  Textures.GetBitmap("blue_terracotta"), Textures.GetBitmap("blue_terracotta"), $"minecraft:blue_terracotta", $"minecraft:blue_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_12", "Brown Terracotta", Textures.GetBitmap("brown_terracotta"), Textures.GetBitmap("brown_terracotta"), $"minecraft:brown_terracotta", $"minecraft:brown_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_13", "Green Terracotta", Textures.GetBitmap("green_terracotta"), Textures.GetBitmap("green_terracotta"), $"minecraft:green_terracotta", $"minecraft:green_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_14", "Red Terracotta",  Textures.GetBitmap("red_terracotta"), Textures.GetBitmap("red_terracotta"), $"minecraft:red_terracotta", $"minecraft:red_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "TERRA_15", "Black Terracotta", Textures.GetBitmap("black_terracotta"), Textures.GetBitmap("black_terracotta"), $"minecraft:black_terracotta", $"minecraft:black_terracotta", "minecraft:stained_hardened_clay"),
                        new Material("1.7", false, "Clay", "CLAY_HARD_00", "Hardened Terracotta", Textures.GetBitmap("terracotta"), Textures.GetBitmap("terracotta"), $"minecraft:terracotta", $"minecraft:terracotta", "minecraft:hardened_clay"),
                        new Material("1.7", false, "Clay", "CLAY_SOFT_00", "Clay", Textures.GetBitmap("clay"), Textures.GetBitmap("clay"), $"minecraft:clay", $"minecraft:clay", "minecraft:clay"),

                        new Material("1.21.5", false, "Planks", "PALE_OAK_PLANKS", "Pale Oak Planks", Textures.GetBitmap("pale_oak_planks"), Textures.GetBitmap("pale_oak_planks"), $"minecraft:pale_oak_planks", $"minecraft:pale_oak_planks", ""),
                        new Material("1.7", false, "Planks", "PLANK_OAK", "Planks Oak", Textures.GetBitmap("oak_planks"), Textures.GetBitmap("oak_planks"), $"minecraft:oak_planks", $"minecraft:oak_planks", "minecraft:planks"),
                        new Material("1.7", false, "Planks", "PLANK_SPR", "Planks Spruce", Textures.GetBitmap("spruce_planks"), Textures.GetBitmap("spruce_planks"), $"minecraft:spruce_planks", $"minecraft:spruce_planks", "minecraft:planks"),
                        new Material("1.7", false, "Planks", "PLANK_BIR", "Planks Birch", Textures.GetBitmap("birch_planks"), Textures.GetBitmap("birch_planks"), $"minecraft:birch_planks", $"minecraft:birch_planks", "minecraft:planks"),
                        new Material("1.7", false, "Planks", "PLANK_JUN", "Planks Jungle", Textures.GetBitmap("jungle_planks"), Textures.GetBitmap("jungle_planks"), $"minecraft:jungle_planks", $"minecraft:jungle_planks", "minecraft:planks"),
                        new Material("1.7", false, "Planks", "PLANK_ACA", "Planks Acacia", Textures.GetBitmap("acacia_planks"), Textures.GetBitmap("acacia_planks"), $"minecraft:acacia_planks", $"minecraft:acacia_planks", "minecraft:planks"),
                        new Material("1.7", false, "Planks", "PLANK_DOK", "Planks Dark Oak", Textures.GetBitmap("dark_oak_planks"), Textures.GetBitmap("dark_oak_planks"), $"minecraft:dark_oak_planks", $"minecraft:dark_oak_planks", "minecraft:planks"),
                        new Material("1.19", false, "Planks", "PLANK_MANGROVE", "Planks Mangrove", Textures.GetBitmap("mangrove_planks"), Textures.GetBitmap("mangrove_planks"), $"minecraft:mangrove_planks", $"minecraft:mangrove_planks", "minecraft:planks"),
                        new Material("1.16", false, "Planks", "PLANK_CRIMSON", "Planks Crimson", Textures.GetBitmap("crimson_planks"), Textures.GetBitmap("crimson_planks"), $"minecraft:crimson_planks", $"minecraft:crimson_planks", ""),
                        new Material("1.16", false, "Planks", "PLANK_WARPED", "Planks Warped", Textures.GetBitmap("warped_planks"), Textures.GetBitmap("warped_planks"), $"minecraft:warped_planks", $"minecraft:warped_planks", ""),
                        new Material("1.20", false, "Planks", "PLANK_CHERRY", "Planks Cherry", Textures.GetBitmap("cherry_planks"), Textures.GetBitmap("cherry_planks"), $"minecraft:cherry_planks", $"minecraft:cherry_planks", "minecraft:planks"),
                        new Material("1.20", false, "Planks", "PLANK_BAMBOO", "Planks Bamboo", Textures.GetBitmap("bamboo_planks"), Textures.GetBitmap("bamboo_planks"), $"minecraft:bamboo_planks", $"minecraft:bamboo_planks", "minecraft:planks"),
                        new Material("1.20", false, "Planks", "PLANK_BAMBOOMOSAIC", "Mosaics Bamboo", Textures.GetBitmap("bamboo_mosaic"), Textures.GetBitmap("bamboo_mosaic"), $"minecraft:bamboo_mosaic", $"minecraft:bamboo_mosaic", "minecraft:planks"),

                        new Material("1.21.5", false, "Stripped", "STRIPPED_PALE_OAK_WOOD", "Stripped Pale Oak Wood", Textures.GetBitmap("stripped_pale_oak_log", 90), Textures.GetBitmap("stripped_pale_oak_log", 90), $"minecraft:stripped_pale_oak_wood[axis=x]", $"minecraft:stripped_pale_oak_wood[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_OAK", "Stripped Oak", Textures.GetBitmap("stripped_oak_log", 90), Textures.GetBitmap("stripped_oak_log", 90), $"minecraft:stripped_oak_log[axis=x]", $"minecraft:stripped_oak_log[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_SPR", "Stripped Spruce", Textures.GetBitmap("stripped_spruce_log"), Textures.GetBitmap("stripped_spruce_log"), $"minecraft:stripped_spruce_log[axis=x]", $"minecraft:stripped_spruce_log[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_BIR", "Stripped Birch", Textures.GetBitmap("stripped_birch_log"), Textures.GetBitmap("stripped_birch_log"), $"minecraft:stripped_birch_log[axis=x]", $"minecraft:stripped_birch_log[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_JUN", "Stripped Jungle", Textures.GetBitmap("stripped_jungle_log"), Textures.GetBitmap("stripped_jungle_log"), $"minecraft:stripped_jungle_log[axis=x]", $"minecraft:stripped_jungle_log[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_ACA", "Stripped Acacia", Textures.GetBitmap("stripped_acacia_log"), Textures.GetBitmap("stripped_acacia_log"), $"minecraft:stripped_acacia_log[axis=x]", $"minecraft:stripped_acacia_log[axis=x]", ""),
                        new Material("1.13", false, "Stripped", "STRIP_LOG_DOK", "Stripped Dark Oak", Textures.GetBitmap("stripped_dark_oak_log"), Textures.GetBitmap("stripped_dark_oak_log"), $"minecraft:stripped_dark_oak_log[axis=x]", $"minecraft:stripped_dark_oak_log[axis=x]", ""),
                        new Material("1.19", false, "Stripped", "STRIP_LOG_MANGRO", "Stripped Mangrove", Textures.GetBitmap("stripped_mangrove_log"), Textures.GetBitmap("stripped_mangrove_log"), $"minecraft:stripped_mangrove_log[axis=x]", $"minecraft:stripped_mangrove_log[axis=x]", ""),
                        new Material("1.16", false, "Stripped", "STRIP_LOG_CRIMSON", "Stripped Crimson Hyphae", Textures.GetBitmap("stripped_crimson_stem"), Textures.GetBitmap("stripped_crimson_stem"), $"minecraft:stripped_crimson_hyphae[axis=x]", $"minecraft:stripped_crimson_hyphae[axis=x]", ""),
                        new Material("1.16", false, "Stripped", "STRIP_LOG_WARPED", "Stripped Warped Hyphae", Textures.GetBitmap("stripped_warped_stem"), Textures.GetBitmap("stripped_warped_stem"), $"minecraft:stripped_warped_hyphae[axis=x]", $"minecraft:stripped_warped_hyphae[axis=x]", ""),
                        new Material("1.20", false, "Stripped", "STRIP_LOG_CHERRY", "Stripped Cherry", Textures.GetBitmap("stripped_cherry_log", 90), Textures.GetBitmap("stripped_cherry_log", 90), $"minecraft:stripped_cherry_log[axis=x]", $"minecraft:stripped_cherry_log[axis=x]", ""),
                        new Material("1.20", false, "Stripped", "STRIP_LOG_BAMBOO", "Stripped Bamboo", Textures.GetBitmap("stripped_bamboo_block", 90), Textures.GetBitmap("stripped_bamboo_block", 90), $"minecraft:stripped_bamboo_block[axis=x]", $"minecraft:stripped_bamboo_block[axis=x]", ""),

                        new Material("1.21.5", false, "Logs Top", "STUMP_STRIPPED_PALE_OAK", "Stripped Pale Oak Log (Top)", Textures.GetBitmap("stripped_pale_oak_log_top"), Textures.GetBitmap("stripped_pale_oak_log_top"), $"minecraft:stripped_pale_oak_log[axis=y]", $"minecraft:stripped_pale_oak_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_OAK", "Stripped Oak (Top)", Textures.GetBitmap("stripped_oak_log_top"), Textures.GetBitmap("stripped_oak_log_top"), $"minecraft:stripped_oak_log[axis=y]", $"minecraft:stripped_oak_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_SPR", "Stripped Spruce (Top)", Textures.GetBitmap("stripped_spruce_log_top"), Textures.GetBitmap("stripped_spruce_log_top"), $"minecraft:stripped_spruce_log[axis=y]", $"minecraft:stripped_spruce_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_BIR", "Stripped Birch (Top)", Textures.GetBitmap("stripped_birch_log_top"), Textures.GetBitmap("stripped_birch_log_top"), $"minecraft:stripped_birch_log[axis=y]", $"minecraft:stripped_birch_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_JUN", "Stripped Jungle (Top)", Textures.GetBitmap("stripped_jungle_log_top"), Textures.GetBitmap("stripped_jungle_log_top"), $"minecraft:stripped_jungle_log[axis=y]", $"minecraft:stripped_jungle_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_ACA", "Stripped Acacia (Top)", Textures.GetBitmap("stripped_acacia_log_top"), Textures.GetBitmap("stripped_acacia_log_top"), $"minecraft:stripped_acacia_log[axis=y]", $"minecraft:stripped_acacia_log[axis=z]", ""),
                        new Material("1.7", false, "Logs Top", "STUMP_LOG_DOK", "Stripped Dark Oak (Top)", Textures.GetBitmap("stripped_dark_oak_log_top"), Textures.GetBitmap("stripped_dark_oak_log_top"), $"minecraft:stripped_dark_oak_log[axis=y]", $"minecraft:stripped_dark_oak_log[axis=z]", ""),
                        new Material("1.19", false, "Logs Top", "STUMP_LOG_MANGRO", "Stripped Mangrove (Top)", Textures.GetBitmap("stripped_mangrove_log_top"), Textures.GetBitmap("stripped_mangrove_log_top"), $"minecraft:stripped_mangrove_log[axis=y]", $"minecraft:stripped_mangrove_log[axis=z]", ""),
                        new Material("1.16", false, "Logs Top", "STUMP_LOG_CRIMSON", "Crimson Stem (Top)", Textures.GetBitmap("stripped_crimson_stem_top"), Textures.GetBitmap("stripped_crimson_stem_top"), $"minecraft:stripped_crimson_stem[axis=y]", $"minecraft:stripped_crimson_stem[axis=z]", ""),
                        new Material("1.16", false, "Logs Top", "STUMP_LOG_WARPED", "Warped Stem (Top)", Textures.GetBitmap("stripped_warped_stem_top"), Textures.GetBitmap("stripped_warped_stem_top"), $"minecraft:stripped_warped_stem[axis=y]", $"minecraft:stripped_warped_stem[axis=z]", ""),
                        new Material("1.20", false, "Logs Top", "STUMP_LOG_CHERRY", "Stripped Cherry (Top)", Textures.GetBitmap("stripped_cherry_log_top"), Textures.GetBitmap("stripped_cherry_log_top"), $"minecraft:stripped_cherry_log[axis=y]", $"minecraft:stripped_cherry_log[axis=z]", ""),
                        new Material("1.20", false, "Logs Top", "STUMP_LOG_BAMBOO_2", "Block of Bamboo (Top)", Textures.GetBitmap("bamboo_block_top"), Textures.GetBitmap("bamboo_block_top"), $"minecraft:bamboo_block[axis=y]", $"minecraft:bamboo_block[axis=z]", ""),
                        new Material("1.20", false, "Logs Top", "STUMP_LOG_BAMBOO", "Stripped Bamboo (Top)", Textures.GetBitmap("stripped_bamboo_block_top"), Textures.GetBitmap("stripped_bamboo_block_top"), $"minecraft:stripped_bamboo_block[axis=y]", $"minecraft:stripped_bamboo_block[axis=z]", ""),

                        new Material("1.21.5", false, "Logs", "BARK_LOG_PALE_OAK", "Pale Oak Wood", Textures.GetBitmap("pale_oak_log", 90), Textures.GetBitmap("pale_oak_log", 90), $"minecraft:pale_oak_wood[axis=x]", $"minecraft:pale_oak_wood[axis=x]", ""),
                        new Material("1.13", false, "Logs", "BARK_LOG_OAK", "Bark Oak", Textures.GetBitmap("oak_log"), Textures.GetBitmap("oak_log"), $"minecraft:oak_log[axis=x]", $"minecraft:oak_log[axis=x]", ""),
                        new Material("1.13", false, "Logs", "BARK_LOG_SPR", "Bark Spruce", Textures.GetBitmap("spruce_log"), Textures.GetBitmap("spruce_log"), $"minecraft:spruce_log[axis=x]", $"minecraft:spruce_log[axis=x]", ""),
                        new Material("1.13", true, "Logs", "BARK_LOG_BIR", "Bark Birch", Textures.GetBitmap("birch_log"), Textures.GetBitmap("birch_log"), $"minecraft:birch_log[axis=x]", $"minecraft:birch_log[axis=x]", ""),
                        new Material("1.13", false, "Logs", "BARK_LOG_JUN", "Bark Jungle", Textures.GetBitmap("jungle_log"), Textures.GetBitmap("jungle_log"), $"minecraft:jungle_log[axis=x]", $"minecraft:jungle_log[axis=x]", ""),
                        new Material("1.13", false, "Logs", "BARK_LOG_ACA", "Bark Acacia", Textures.GetBitmap("acacia_log"), Textures.GetBitmap("acacia_log"), $"minecraft:acacia_log[axis=x]", $"minecraft:acacia_log[axis=x]", ""),
                        new Material("1.13", false, "Logs", "BARK_LOG_DOK", "Bark Dark Oak", Textures.GetBitmap("dark_oak_log"), Textures.GetBitmap("dark_oak_log"), $"minecraft:dark_oak_log[axis=x]", $"minecraft:dark_oak_log[axis=x]", ""),
                        new Material("1.19", false, "Logs", "BARK_LOG_MANGRO", "Bark Mangrove", Textures.GetBitmap("mangrove_log"), Textures.GetBitmap("mangrove_log"), $"minecraft:mangrove_log[axis=x]", $"minecraft:mangrove_log[axis=x]", ""),
                        new Material("1.16", false, "Logs", "BARK_LOG_CRIMSON", "Crimson Hyphae", Textures.GetBitmap("crimson_stem"), Textures.GetBitmap("crimson_stem"), $"minecraft:crimson_hyphae", $"minecraft:crimson_hyphae", ""),
                        new Material("1.16", false, "Logs", "BARK_LOG_WARPED", "Warped Hyphae", Textures.GetBitmap("warped_stem"), Textures.GetBitmap("warped_stem"), $"minecraft:warped_hyphae", $"minecraft:warped_hyphae", ""),
                        new Material("1.20", false, "Logs", "BARK_LOG_CHERRY", "Cherry Wood", Textures.GetBitmap("cherry_log", 90), Textures.GetBitmap("cherry_log", 90), $"minecraft:cherry_wood[axis=x]", $"minecraft:cherry_wood[axis=x]", ""),
                        new Material("1.20", false, "Logs", "BARK_LOG_BAMBOO", "Block of Bamboo", Textures.GetBitmap("bamboo_block", 90), Textures.GetBitmap("bamboo_block", 90), $"minecraft:bamboo_block[axis=x]", $"minecraft:bamboo_block[axis=x]", ""),

                        new Material("1.7", false, "Mushrooms", "SHROOM_INNER", "Mushroom Inside", Textures.GetBitmap("mushroom_block_inside"), Textures.GetBitmap("mushroom_block_inside"), $"minecraft:mushroom_stem[down=false,east=false,west=false,north=false,south=false,up=false]", $"minecraft:mushroom_stem[down=false,east=false,west=false,north=false,south=false,up=false]", "minecraft:brown_mushroom_block"),
                        new Material("1.7", false, "Mushrooms", "SHROOM_BROWN", "Brown Mushroom", Textures.GetBitmap("brown_mushroom_block"), Textures.GetBitmap("brown_mushroom_block"), $"minecraft:brown_mushroom_block[down=true,east=true,west=true,north=true,south=true,up=true]", $"minecraft:brown_mushroom_block[down=true,east=true,west=true,north=true,south=true,up=true]", "minecraft:brown_mushroom_block"),
                        new Material("1.7", false, "Mushrooms", "SHROOM_STEM", "Mushroom Stem", Textures.GetBitmap("mushroom_stem"), Textures.GetBitmap("mushroom_stem"), $"minecraft:mushroom_stem[down=true,east=true,west=true,north=true,south=true,up=true]", $"minecraft:mushroom_stem[down=true,east=true,west=true,north=true,south=true,up=true]", "minecraft:red_mushroom_block"),
                        new Material("1.7", false, "Mushrooms", "SHROOM_RED", "Red Mushroom Block", Textures.GetBitmap("red_mushroom_block"), Textures.GetBitmap("red_mushroom_block"), $"minecraft:red_mushroom_block[down=true,east=true,west=true,north=true,south=true,up=true]", $"minecraft:red_mushroom_block[down=true,east=true,west=true,north=true,south=true,up=true]", "minecraft:red_mushroom_block"),

                        new Material("1.10", false, "Good", "NETHER_WART", "Nether Wart Block",  Textures.GetBitmap("nether_wart_block"), Textures.GetBitmap("nether_wart_block"), $"minecraft:nether_wart_block", $"minecraft:nether_wart_block", "minecraft:nether_wart_block"),
                        new Material("1.16", false, "Good", "NETHER_WART_WARPED", "Warped Wart Block", Textures.GetBitmap("warped_wart_block"), Textures.GetBitmap("warped_wart_block"), $"minecraft:warped_wart_block", $"minecraft:warped_wart_block", ""),
                        new Material("1.8", false, "Good", "SMOOTH_RED_SANDSTONE", "Smooth Red Sandstone", Textures.GetBitmap("red_sandstone_top"), Textures.GetBitmap("red_sandstone_top"), $"minecraft:smooth_red_sandstone", $"minecraft:smooth_red_sandstone", "minecraft:red_sandstone"),
                        new Material("1.7", false, "Good", "SMOOTH_SANDSTONE", "Smooth Sandstone", Textures.GetBitmap("sandstone_top"), Textures.GetBitmap("sandstone_top"), $"minecraft:smooth_sandstone", $"minecraft:smooth_sandstone", "minecraft:sandstone"),
                        new Material("1.7", false, "Good", "SNOW_BLK", "Snow", Textures.GetBitmap("snow"), Textures.GetBitmap("snow"), $"minecraft:snow_block", $"minecraft:snow_block", "minecraft:snow"),
                        new Material("1.7", false, "Good", "HAY_BLK_TOP", "Hay Block (Top)",  Textures.GetBitmap("hay_block_top"), Textures.GetBitmap("hay_block_top"), $"minecraft:hay_block[axis=y]", $"minecraft:hay_block[axis=z]", ""),
                        new Material("1.7", true, "Good", "HAY_BLK", "Hay Block",  Textures.GetBitmap("hay_block_side"), Textures.GetBitmap("hay_block_side"), $"minecraft:hay_block[axis=x]", $"minecraft:hay_block[axis=x]", "minecraft:hay_block"),
                        new Material("1.7", false, "Good", "SMOOTH_QRTZ", "Smooth Quartz Block", Textures.GetBitmap("quartz_block_bottom"), Textures.GetBitmap("quartz_block_bottom"), $"minecraft:smooth_quartz", $"minecraft:smooth_quartz", "minecraft:quartz_block"),

                        new Material("1.8", true, "Okay", "SLIME_BLK", "Slime Block", Textures.GetBitmap("slime_block"), Textures.GetBitmap("slime_block"), $"minecraft:slime_block", $"minecraft:slime_block", ""),
                        new Material("1.15", true, "Okay", "HONEY_BLK", "Honey Block", Textures.GetBitmap("honey_block_top"), Textures.GetBitmap("honey_block_side"), $"minecraft:honey_block", $"minecraft:honey_block", ""),
                        new Material("1.15", false, "Okay", "HONEYCOMB", "Honeycomb Block", Textures.GetBitmap("honeycomb_block"), Textures.GetBitmap("honeycomb_block"), $"minecraft:honeycomb_block", $"minecraft:honeycomb_block", ""),
                        new Material("1.15", false, "Okay", "BEE_HIVE", "Bee Hive", Textures.GetBitmap("beehive_end"), Textures.GetBitmap("beehive_side"), $"minecraft:beehive[facing=north]", $"minecraft:beehive[facing=north]", ""),
                        new Material("1.15", true, "Okay", "BEE_NEST", "Bee Nest", Textures.GetBitmap("bee_nest_top"), Textures.GetBitmap("bee_nest_side"), $"minecraft:bee_nest[facing=west]", $"minecraft:bee_nest[facing=west]", ""),
                        new Material("1.7", false, "Okay", "BEDROCK", "Bedrock", Textures.GetBitmap("bedrock"), Textures.GetBitmap("bedrock"), $"minecraft:bedrock", $"minecraft:bedrock", "minecraft:bedrock"),
                        new Material("1.7", false, "Okay", "COBBLE", "Cobblestone", Textures.GetBitmap("cobblestone"), Textures.GetBitmap("cobblestone"), $"minecraft:cobblestone", $"minecraft:cobblestone", "minecraft:cobblestone"),
                        new Material("1.17", false, "Okay", "MOSS_BLOCK", "Moss Block", Textures.GetBitmap("moss_block"), Textures.GetBitmap("moss_block"), $"minecraft:moss_block", $"minecraft:moss_block", ""),
                        new Material("1.17", false, "Okay", "DRIPSTONE", "Dripstone", Textures.GetBitmap("dripstone_block"), Textures.GetBitmap("dripstone_block"), $"minecraft:dripstone_block", $"minecraft:dripstone_block", "minecraft:dripstone_block"),
                        new Material("1.7", false, "Okay", "ENDSTONE", "Endstone", Textures.GetBitmap("end_stone"), Textures.GetBitmap("end_stone"), $"minecraft:end_stone", $"minecraft:end_stone", "minecraft:end_stone"),
                        new Material("1.7", false, "Okay", "NETHERRACK", "Netherrack", Textures.GetBitmap("netherrack"), Textures.GetBitmap("netherrack"), $"minecraft:netherrack", $"minecraft:netherrack", "minecraft:netherrack"),
                        new Material("1.7", false, "Okay", "OBSIDIAN", "Obsidian", Textures.GetBitmap("obsidian"), Textures.GetBitmap("obsidian"), $"minecraft:obsidian", $"minecraft:obsidian", "minecraft:obsidian"),
                        new Material("1.9", false, "Okay", "PURPUR_BLK", "Purpur Block", Textures.GetBitmap("purpur_block"), Textures.GetBitmap("purpur_block"), $"minecraft:purpur_block", $"minecraft:purpur_block", "minecraft:purpur_block"),
                        new Material("1.9", false, "Okay", "PURPUR_PLR_TOP", "Purpur Pillar (Top)", Textures.GetBitmap("purpur_pillar_top"), Textures.GetBitmap("purpur_pillar_top"), $"minecraft:purpur_pillar[axis=y]", $"minecraft:purpur_pillar[axis=z]", ""),
                        new Material("1.9", false, "Okay", "PURPUR_PLR_SIDE", "Purpur Pillar (Side)", Textures.GetBitmap("purpur_pillar", 90), Textures.GetBitmap("purpur_pillar", 90), $"minecraft:purpur_pillar[axis=x]", $"minecraft:purpur_pillar[axis=x]", ""),

                        new Material("1.7", false, "Okay", "ICE_PACKED", "Packed Ice", Textures.GetBitmap("packed_ice"), Textures.GetBitmap("packed_ice"), $"minecraft:packed_ice", $"minecraft:packed_ice", "minecraft:packed_ice"),
                        new Material("1.7", false, "Okay", "SPONGE", "Sponge", Textures.GetBitmap("sponge"), Textures.GetBitmap("sponge"), $"minecraft:sponge", $"minecraft:sponge", "minecraft:sponge"),
                        new Material("1.8", false, "Okay", "SPONGE_WET", "Sponge", Textures.GetBitmap("wet_sponge"), Textures.GetBitmap("wet_sponge"), $"minecraft:wet_sponge", $"minecraft:wet_sponge", "minecraft:sponge"),
                        new Material("1.7", false, "Okay", "STONE", "Stone", Textures.GetBitmap("stone"), Textures.GetBitmap("stone"), $"minecraft:stone", $"minecraft:stone", "minecraft:stone"),
                        new Material("1.13", false, "Okay", "ICE_BLUE", "Blue Ice", Textures.GetBitmap("blue_ice"), Textures.GetBitmap("blue_ice"), $"minecraft:blue_ice", $"minecraft:blue_ice", ""),
                        new Material("1.8", false, "Okay", "ANDESITE", "Andesite", Textures.GetBitmap("andesite"), Textures.GetBitmap("andesite"), $"minecraft:andesite", $"minecraft:andesite", "minecraft:stone"),
                        new Material("1.8", false, "Okay", "DIORITE", "Diorite", Textures.GetBitmap("diorite"), Textures.GetBitmap("diorite"), $"minecraft:diorite", $"minecraft:diorite", "minecraft:stone"),
                        new Material("1.8", false, "Okay", "GRANITE", "Granite", Textures.GetBitmap("granite"), Textures.GetBitmap("granite"), $"minecraft:granite", $"minecraft:granite", "minecraft:stone"),
                        new Material("1.16", true, "Okay", "TARGET", "Target", Textures.GetBitmap("target_top"), Textures.GetBitmap("target_side"), $"minecraft:target", $"minecraft:target", ""),
                        new Material("1.19", false, "Okay", "SCULK", "Sculk", Textures.GetBitmap("sculk_catalyst_top"), Textures.GetBitmap("sculk_catalyst_top"), $"minecraft:sculk", $"minecraft:sculk", ""),

                        new Material("1.8", false, "Prismarine", "PRISMARINE_DARK", "Dark Prismarine", Textures.GetBitmap("dark_prismarine"), Textures.GetBitmap("dark_prismarine"), $"minecraft:dark_prismarine", $"minecraft:dark_prismarine", "minecraft:prismarine"),
                        new Material("1.8", false, "Prismarine", "PRISMARINE_BRICK", "Prismarine Bricks", Textures.GetBitmap("prismarine_bricks"), Textures.GetBitmap("prismarine_bricks"), $"minecraft:prismarine_bricks", $"minecraft:prismarine_bricks", "minecraft:prismarine"),
                        new Material("1.8", false, "Prismarine", "PRISMARINE_BLK", "Prismarine Block", Textures.GetBitmap("prismarine"), Textures.GetBitmap("prismarine"), $"minecraft:prismarine", $"minecraft:prismarine", "minecraft:prismarine"),

                        new Material("1.10", false, "Bricks", "NETHER_BRICK_RED", "Red Nether Bricks", Textures.GetBitmap("red_nether_bricks"), Textures.GetBitmap("red_nether_bricks"), $"minecraft:red_nether_bricks", $"minecraft:red_nether_bricks", "minecraft:red_nether_brick"),
                        new Material("1.7", false, "Bricks", "NETHER_BRICK", "Nether Brick", Textures.GetBitmap("nether_bricks"), Textures.GetBitmap("nether_bricks"), $"minecraft:nether_bricks", $"minecraft:nether_bricks", "minecraft:nether_brick"),
                        new Material("1.16", false, "Bricks", "NETHER_BRICK_CHIZ", "Chiseled Nether Brick", Textures.GetBitmap("chiseled_nether_bricks"), Textures.GetBitmap("chiseled_nether_bricks"), $"minecraft:chiseled_nether_bricks", $"minecraft:chiseled_nether_bricks", ""),
                        new Material("1.16", true, "Bricks", "NETHER_BRICK_CRK", "Cracked Nether Brick", Textures.GetBitmap("cracked_nether_bricks"), Textures.GetBitmap("cracked_nether_bricks"), $"minecraft:cracked_nether_bricks", $"minecraft:cracked_nether_bricks", ""),
                        new Material("1.16", true, "Bricks", "BLK_STONE_CRK_PLSHD_BRK", "Cracked Polished Blackstone Bricks", Textures.GetBitmap("cracked_polished_blackstone_bricks"), Textures.GetBitmap("cracked_polished_blackstone_bricks"), $"minecraft:cracked_polished_blackstone_bricks", $"cracked_polished_blackstone_bricks", ""),
                        new Material("1.16", false, "Bricks", "BLK_STONE_PLSHD_BRK", "Polished Blackstone Bricks", Textures.GetBitmap("polished_blackstone_bricks"), Textures.GetBitmap("polished_blackstone_bricks"), $"minecraft:polished_blackstone_bricks", $"minecraft:polished_blackstone_bricks", ""),
                        new Material("1.16", false, "Bricks", "BLK_STONE_PLSHD", "Polished Blackstone", Textures.GetBitmap("polished_blackstone"), Textures.GetBitmap("polished_blackstone"), $"minecraft:polished_blackstone", $"minecraft:polished_blackstone", ""),
                        new Material("1.16", false, "Bricks", "BLK_STONE_CHISELED", "Chiseled Black Stone", Textures.GetBitmap("chiseled_polished_blackstone"), Textures.GetBitmap("chiseled_polished_blackstone"), $"minecraft:chiseled_polished_blackstone", $"minecraft:chiseled_polished_blackstone", ""),
                        new Material("1.7", false, "Bricks", "COBBLE_MOSSY_BRICK", "mossy_stone_bricks", Textures.GetBitmap("mossy_stone_bricks"), Textures.GetBitmap("mossy_stone_bricks"), $"minecraft:mossy_stone_bricks", $"minecraft:mossy_stone_bricks", "minecraft:stonebrick"),
                        new Material("1.7", false, "Bricks", "STONE_BRICK", "stone_bricks", Textures.GetBitmap("stone_bricks"), Textures.GetBitmap("stone_bricks"), $"minecraft:stone_bricks", $"minecraft:stone_bricks", "minecraft:stonebrick"),
                        new Material("1.7", true, "Bricks", "STONE_BRICK_CRACKED", "cracked_stone_bricks", Textures.GetBitmap("cracked_stone_bricks"), Textures.GetBitmap("cracked_stone_bricks"), $"minecraft:cracked_stone_bricks", $"minecraft:cracked_stone_bricks", "minecraft:stonebrick"),
                        new Material("1.21.5", false, "Bricks", "CHZL_STONE_BRICKS", "Chiseled Stone Bricks", Textures.GetBitmap("chiseled_stone_bricks"), Textures.GetBitmap("chiseled_stone_bricks"), $"minecraft:chiseled_stone_bricks", $"minecraft:chiseled_stone_bricks", ""),
                        new Material("1.19", false, "Bricks", "MUD_BRICK", "Muddy Bricks", Textures.GetBitmap("mud_bricks"), Textures.GetBitmap("mud_bricks"), $"minecraft:mud_bricks",$"minecraft:mud_bricks", ""),
                        new Material("1.9", false, "Bricks", "ENDSTONE_BRICK", "end_stone_bricks", Textures.GetBitmap("end_stone_bricks"), Textures.GetBitmap("end_stone_bricks"), $"minecraft:end_stone_bricks", $"minecraft:end_stone_bricks", "minecraft:end_bricks"),
                        new Material("1.16", false, "Bricks", "QUARTZ_BRICK", "Quartz Bricks", Textures.GetBitmap("quartz_bricks"), Textures.GetBitmap("quartz_bricks"), $"minecraft:quartz_bricks", $"minecraft:quartz_bricks", ""),
                        new Material("1.7", false, "Bricks", "BRICKS_RED", "Bricks", Textures.GetBitmap("bricks"), Textures.GetBitmap("bricks"), $"minecraft:bricks", $"minecraft:bricks", "minecraft:brick_block"),
                        new Material("1.21.5", false, "Bricks", "TUFF_BRICKS", "Tuff Bricks", Textures.GetBitmap("tuff_bricks"), Textures.GetBitmap("tuff_bricks"), $"minecraft:tuff_bricks", $"minecraft:tuff_bricks", ""),
                        new Material("1.21.5", false, "Bricks", "CHZL_TUFF_BRICKS", "Chiseled Tuff Bricks", Textures.GetBitmap("chiseled_tuff_bricks_top"), Textures.GetBitmap("chiseled_tuff_bricks"), $"minecraft:chiseled_tuff_bricks", $"minecraft:chiseled_tuff_bricks", ""),
                        new Material("1.21.5", false, "Bricks", "CHZL_RESIN_BRICKS", "Chiseled Resin Bricks", Textures.GetBitmap("chiseled_resin_bricks"), Textures.GetBitmap("chiseled_resin_bricks"), $"minecraft:chiseled_resin_bricks", $"minecraft:chiseled_resin_bricks", ""),
                        new Material("1.21.5", false, "Bricks", "RESIN_BRICKS", "Resin Bricks", Textures.GetBitmap("resin_bricks"), Textures.GetBitmap("resin_bricks"), $"minecraft:resin_bricks", $"minecraft:resin_bricks", ""),
                        new Material("1.21.5", false, "Other", "RESIN_BLK", "Block of Resin", Textures.GetBitmap("resin_block"), Textures.GetBitmap("resin_block"), $"minecraft:resin_block", $"minecraft:resin_block", ""),

                        new Material("1.16", true, "Nether", "LODESTONE", "Lodestone", Textures.GetBitmap("lodestone_top"), Textures.GetBitmap("lodestone_side"), $"minecraft:lodestone", $"minecraft:lodestone", ""),
                        new Material("1.16", false, "Nether", "SOUL_SOIL", "Soul Soil", Textures.GetBitmap("soul_soil"), Textures.GetBitmap("soul_soil"), $"minecraft:soul_soil", $"minecraft:soul_soil", ""),
                        new Material("1.16", false, "Nether", "SOUL_SAND", "Soul Sand", Textures.GetBitmap("soul_sand"), Textures.GetBitmap("soul_sand"), $"minecraft:soul_sand", $"minecraft:soul_sand", "minecraft:soul_sand"),
                        new Material("1.16", false, "Nether", "ANCIENT_DEBRIS", "Ancient Debris", Textures.GetBitmap("ancient_debris_top"), Textures.GetBitmap("ancient_debris_side"), $"minecraft:ancient_debris", $"minecraft:ancient_debris", ""),
                        new Material("1.16", false, "Nether", "BASALT_TOP", "Basalt", Textures.GetBitmap("basalt_top"), Textures.GetBitmap("basalt_top"), $"minecraft:basalt[axis=y]", $"minecraft:basalt[axis=z]", ""),
                        new Material("1.16", false, "Nether", "BASALT_SIDE", "Basalt", Textures.GetBitmap("basalt_side"), Textures.GetBitmap("basalt_side"), $"minecraft:basalt[axis=x]", $"minecraft:basalt[axis=x]", ""),
                        new Material("1.16", false, "Nether", "BASALT_POLISHED_TOP", "Basalt (Polished)", Textures.GetBitmap("polished_basalt_top"), Textures.GetBitmap("polished_basalt_top"), $"minecraft:polished_basalt[axis=y]", $"minecraft:polished_basalt[axis=z]", ""),
                        new Material("1.16", false, "Nether", "BASALT_POLISHED_SIDE", "Basalt (Polished)", Textures.GetBitmap("polished_basalt_side"), Textures.GetBitmap("polished_basalt_side"), $"minecraft:polished_basalt[axis=x]", $"minecraft:polished_basalt[axis=x]", ""),
                        new Material("1.17", false, "Nether", "BASALT_SMOOTH", "Basalt (Smooth)", Textures.GetBitmap("smooth_basalt"), Textures.GetBitmap("smooth_basalt"), $"minecraft:smooth_basalt", $"minecraft:smooth_basalt", ""),
                        new Material("1.16", false, "Nether", "NETHERITE_BLOCK", "Netherite Block", Textures.GetBitmap("netherite_block"), Textures.GetBitmap("netherite_block"), $"minecraft:netherite_block", $"minecraft:netherite_block", ""),
                        new Material("1.16", false, "Nether", "NYLIUM_CRIMSON", "Crimson Nylium", Textures.GetBitmap("crimson_nylium"), Textures.GetBitmap("crimson_nylium_side"), $"minecraft:crimson_nylium", $"minecraft:crimson_nylium", ""),
                        new Material("1.16", false, "Nether", "NYLIUM_WARPED", "Warped Nylium",  Textures.GetBitmap("warped_nylium"), Textures.GetBitmap("warped_nylium_side"), $"minecraft:warped_nylium", $"minecraft:warped_nylium", ""),
                        new Material("1.16", false, "Nether", "BLK_STONE", "Blackstone", Textures.GetBitmap("blackstone_top"), Textures.GetBitmap("blackstone"), $"minecraft:blackstone", $"minecraft:blackstone", ""),
                        new Material("1.7", false, "Nether", "QUARTZ_CHISELED", "Chiseled Quartz Block", Textures.GetBitmap("chiseled_quartz_block_top"), Textures.GetBitmap("chiseled_quartz_block"), $"minecraft:chiseled_quartz_block", $"minecraft:chiseled_quartz_block", "minecraft:quartz_block"),

                        new Material("1.17", false, "Other", "CALCITE", "Calcite", Textures.GetBitmap("calcite"), Textures.GetBitmap("calcite"), $"minecraft:calcite", $"minecraft:calcite", ""),
                        new Material("1.17", false, "Other", "CHZL_DEEPSLATE", "Chiseled Deepslate", Textures.GetBitmap("chiseled_deepslate"), Textures.GetBitmap("chiseled_deepslate"), $"minecraft:chiseled_deepslate", $"minecraft:chiseled_deepslate", ""),
                        new Material("1.17", true, "Other", "CRACKED_DEEPSLATE_BRICK", "Cracked Deepslate Bricks", Textures.GetBitmap("cracked_deepslate_bricks"), Textures.GetBitmap("cracked_deepslate_bricks"), $"minecraft:cracked_deepslate_bricks", $"minecraft:cracked_deepslate_bricks", ""),
                        new Material("1.17", true, "Other", "CRACKED_DEEPSLATE_TILE", "Cracked Deepslate Tiles", Textures.GetBitmap("cracked_deepslate_tiles"), Textures.GetBitmap("cracked_deepslate_tiles"), $"minecraft:cracked_deepslate_tiles", $"minecraft:cracked_deepslate_tiles", ""),
                        new Material("1.17", false, "Other", "COBBLE_DEEPSLATE", "Cobbled Deepslate", Textures.GetBitmap("cobbled_deepslate"), Textures.GetBitmap("cobbled_deepslate"), $"minecraft:cobbled_deepslate", $"minecraft:cobbled_deepslate", ""),
                        new Material("1.17", false, "Other", "DEEPSLATE", "Deepslate", Textures.GetBitmap("deepslate_top"), Textures.GetBitmap("deepslate"), $"minecraft:deepslate", $"minecraft:deepslate", ""),
                        new Material("1.17", false, "Other", "DEEPSLATE_BRICK", "Deepslate Bricks", Textures.GetBitmap("deepslate_bricks"), Textures.GetBitmap("deepslate_bricks"), $"minecraft:deepslate_bricks", $"minecraft:deepslate_bricks", ""),
                        new Material("1.17", false, "Other", "DEEPSLATE_TILE", "Deepslate Tiles", Textures.GetBitmap("deepslate_tiles"), Textures.GetBitmap("deepslate_tiles"), $"minecraft:deepslate_tiles", $"minecraft:deepslate_tiles", ""),
                        new Material("1.17", false, "Other", "POLISH_DEEPSLATE", "Polished Deepslatee", Textures.GetBitmap("polished_deepslate"), Textures.GetBitmap("polished_deepslate"), $"minecraft:polished_deepslate", $"minecraft:polished_deepslate", ""),
                        new Material("1.17", false, "Other", "TUFF", "Tuff", Textures.GetBitmap("tuff"), Textures.GetBitmap("tuff"), $"minecraft:tuff", $"minecraft:tuff", ""),
                        new Material("1.21.5", false, "Other", "CHZL_TUFF", "Chiseled Tuff", Textures.GetBitmap("chiseled_tuff_top"), Textures.GetBitmap("chiseled_tuff"), $"minecraft:chiseled_tuff", $"minecraft:chiseled_tuff", ""),

                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_C", "Coal Block", Textures.GetBitmap("coal_block"), Textures.GetBitmap("coal_block"), $"minecraft:coal_block", $"minecraft:coal_block", "minecraft:coal_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_I", "Iron Block", Textures.GetBitmap("iron_block"), Textures.GetBitmap("iron_block"), $"minecraft:iron_block", $"minecraft:iron_block", "minecraft:iron_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_G", "Gold Block", Textures.GetBitmap("gold_block"), Textures.GetBitmap("gold_block"), $"minecraft:gold_block", $"minecraft:gold_block", "minecraft:gold_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_R", "Redstone Block", Textures.GetBitmap("redstone_block"), Textures.GetBitmap("redstone_block"), $"minecraft:redstone_block", $"minecraft:redstone_block", "minecraft:redstone_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_L", "Lapis Block", Textures.GetBitmap("lapis_block"), Textures.GetBitmap("lapis_block"), $"minecraft:lapis_block", $"minecraft:lapis_block", "minecraft:lapis_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_D", "Diamond Block", Textures.GetBitmap("diamond_block"), Textures.GetBitmap("diamond_block"), $"minecraft:diamond_block", $"minecraft:diamond_block", "minecraft:diamond_block"),
                        new Material("1.7", false, "Ores (Solid)", "SLD_ORE_E", "Emerald Block", Textures.GetBitmap("emerald_block"), Textures.GetBitmap("emerald_block"), $"minecraft:emerald_block", $"minecraft:emerald_block", "minecraft:emerald_block"),
                        new Material("1.17", false, "Ores (Solid)", "AMETHYST_BD", "Amethyst Block", Textures.GetBitmap("amethyst_block"), Textures.GetBitmap("amethyst_block"), $"minecraft:amethyst_block", $"minecraft:amethyst_block", ""),
                        new Material("1.17", false, "Ores (Solid)", "AMETHYST_BLK", "Budding Amethyst", Textures.GetBitmap("budding_amethyst"), Textures.GetBitmap("budding_amethyst"), $"minecraft:budding_amethyst", $"minecraft:budding_amethyst", ""),

                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_BLK", "Waxed Block of Copper", Textures.GetBitmap("copper_block"), Textures.GetBitmap("copper_block"), $"minecraft:waxed_copper_block", $"minecraft:waxed_copper_block", ""),
                        new Material("1.17", false, "Ores (Solid)", "CHZL_Cu", "Waxed Chiseled Copper", Textures.GetBitmap("chiseled_copper"), Textures.GetBitmap("chiseled_copper"), $"minecraft:waxed_chiseled_copper", $"minecraft:waxed_chiseled_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_CUT", "Waxed Cut Copper", Textures.GetBitmap("cut_copper"), Textures.GetBitmap("cut_copper"), $"minecraft:waxed_cut_copper", $"minecraft:waxed_cut_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "BULB_Cu", "Waxed Copper Bulb", Textures.GetBitmap("copper_bulb"), Textures.GetBitmap("copper_bulb"), $"minecraft:waxed_copper_bulb[lit=false,powered=false]", $"minecraft:waxed_copper_bulb[lit=false,powered=false]", ""),

                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_EXPO", "Waxed Exposed Copper", Textures.GetBitmap("exposed_copper"), Textures.GetBitmap("exposed_copper"), $"minecraft:waxed_exposed_copper", $"minecraft:waxed_exposed_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "CHZL_Cu_EXPO", "Waxed Exposed Chiseled Copper", Textures.GetBitmap("exposed_chiseled_copper"), Textures.GetBitmap("exposed_chiseled_copper"), $"minecraft:waxed_exposed_chiseled_copper", $"minecraft:waxed_exposed_chiseled_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_EXPO_CUT", "Waxed Exposed Cut Copper", Textures.GetBitmap("exposed_cut_copper"), Textures.GetBitmap("exposed_cut_copper"), $"minecraft:waxed_exposed_cut_copper", $"minecraft:waxed_exposed_cut_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "BULB_Cu_EXPO", "Waxed Exposed Copper Bulb", Textures.GetBitmap("exposed_copper_bulb"), Textures.GetBitmap("exposed_copper_bulb"), $"minecraft:waxed_exposed_copper_bulb[lit=false,powered=false]", $"minecraft:waxed_exposed_copper_bulb[lit=false,powered=false]", ""),

                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_WTHER", "Waxed Weathered Copper", Textures.GetBitmap("weathered_copper"), Textures.GetBitmap("weathered_copper"), $"minecraft:waxed_weathered_copper", $"minecraft:waxed_weathered_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "CHZL_Cu_WTHER", "Waxed Weathered Chiseled Copper", Textures.GetBitmap("weathered_chiseled_copper"), Textures.GetBitmap("weathered_chiseled_copper"), $"minecraft:waxed_weathered_chiseled_copper", $"minecraft:waxed_weathered_chiseled_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_WTHER_CUT", "Waxed Weathered Cut Copper", Textures.GetBitmap("weathered_cut_copper"), Textures.GetBitmap("weathered_cut_copper"), $"minecraft:waxed_weathered_cut_copper", $"minecraft:waxed_weathered_cut_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "BULB_Cu_WTHER", "Waxed Weathered Copper Bulb", Textures.GetBitmap("weathered_copper_bulb"), Textures.GetBitmap("weathered_copper_bulb"), $"minecraft:waxed_weathered_copper_bulb[lit=false,powered=false]", $"minecraft:waxed_weathered_copper_bulb[lit=false,powered=false]", ""),

                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_OXI", "Waxed Oxidized Copper", Textures.GetBitmap("oxidized_copper"), Textures.GetBitmap("oxidized_copper"), $"minecraft:waxed_oxidized_copper", $"minecraft:waxed_oxidized_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "CHZL_Cu_OXI", "Waxed Oxidized Chiseled Copper", Textures.GetBitmap("oxidized_chiseled_copper"), Textures.GetBitmap("oxidized_chiseled_copper"), $"minecraft:waxed_oxidized_chiseled_copper", $"minecraft:waxed_oxidized_chiseled_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "SLD_ORE_Cu_OXI_CUT", "Waxed Oxidized Cut Copper", Textures.GetBitmap("oxidized_cut_copper"), Textures.GetBitmap("oxidized_cut_copper"), $"minecraft:waxed_oxidized_cut_copper", $"minecraft:waxed_oxidized_cut_copper", ""),
                        new Material("1.17", false, "Ores (Solid)", "BULB_Cu_OXI", "Waxed Oxidized Copper Bulb", Textures.GetBitmap("oxidized_copper_bulb"), Textures.GetBitmap("oxidized_copper_bulb"), $"minecraft:waxed_oxidized_copper_bulb[lit=false,powered=false]", $"minecraft:waxed_oxidized_copper_bulb[lit=false,powered=false]", ""),

                        new Material("1.10", false, "Common", "BONE_BLK", "Bone Block", Textures.GetBitmap("bone_block_side"), Textures.GetBitmap("bone_block_side"), $"minecraft:bone_block[axis=x]", $"minecraft:bone_block[axis=x]", "minecraft:bone_block"),
                        new Material("1.10", false, "Common", "BONE_BLK_TOP", "Bone Block (Top)", Textures.GetBitmap("bone_block_top"), Textures.GetBitmap("bone_block_top"), $"minecraft:bone_block[axis=y]", $"minecraft:bone_block[axis=z]", "minecraft:bone_block"),
                        new Material("1.7", false, "Common", "GRAVEL", "gravel", Textures.GetBitmap("gravel"), Textures.GetBitmap("gravel"), $"minecraft:gravel", $"minecraft:gravel", "minecraft:gravel"),
                        new Material("1.7", false, "Common", "COBBLE_MOSSY", "mossy_cobblestone", Textures.GetBitmap("mossy_cobblestone"), Textures.GetBitmap("mossy_cobblestone"), $"minecraft:mossy_cobblestone", $"minecraft:mossy_cobblestone", "minecraft:mossy_cobblestone"),

                        new Material("1.7", true, "Other", "TNT", "TNT", Textures.GetBitmap("tnt_top"), Textures.GetBitmap("tnt_side"), $"minecraft:tnt", $"minecraft:tnt", "minecraft:tnt"),
                        new Material("1.8", false, "Other", "ANDESITE_POLISHED", "Andesite (Polished)", Textures.GetBitmap("polished_andesite"), Textures.GetBitmap("polished_andesite"), $"minecraft:andesite", $"minecraft:andesite", "minecraft:stone"),
                        new Material("1.8", false, "Other", "DIORITE_POLISHED", "Diorite (Polished)", Textures.GetBitmap("polished_diorite"), Textures.GetBitmap("polished_diorite"), $"minecraft:diorite", $"minecraft:diorite", "minecraft:stone"),
                        new Material("1.8", false, "Other", "GRANITE_POLISHED", "Granite (Polished)", Textures.GetBitmap("polished_granite"), Textures.GetBitmap("polished_granite"), $"minecraft:granite", $"minecraft:granite", "minecraft:stone"),
                        new Material("1.7", true, "Other", "BOOK_SHELF", "bookshelf", Textures.GetBitmap("oak_planks"), Textures.GetBitmap("bookshelf"), $"minecraft:bookshelf", $"minecraft:bookshelf", "minecraft:bookshelf"),
                        new Material("1.7", false, "Other", "NOTEBLOCK", "Note Block", Textures.GetBitmap("note_block"), Textures.GetBitmap("note_block"), $"minecraft:note_block", $"minecraft:note_block", "minecraft:noteblock"),
                        new Material("1.7", false, "Other", "MELON_BLOCK", "Melon", Textures.GetBitmap("melon_top"), Textures.GetBitmap("melon_side"), $"minecraft:melon", $"minecraft:melon", "minecraft:melon_block"),
                        new Material("1.7", false, "Other", "PUMPKIN_BLOCK", "Pumpkin", Textures.GetBitmap("pumpkin_top"), Textures.GetBitmap("pumpkin_side"), $"minecraft:pumpkin", $"minecraft:pumpkin", "minecraft:pumpkin"),
                        new Material("1.7", true, "Other", "PUMPKIN_CARVED", "Pumpkin (Carved)", Textures.GetBitmap("pumpkin_top"), Textures.GetBitmap("carved_pumpkin"), $"minecraft:carved_pumpkin", $"minecraft:carved_pumpkin", "minecraft:pumpkin"),
                        new Material("1.19", false, "Other", "REINFORCED_DS", "Reinforced Deepslate", Textures.GetBitmap("reinforced_deepslate_top"), Textures.GetBitmap("reinforced_deepslate_side"), $"minecraft:reinforced_deepslate", $"minecraft:reinforced_deepslate", ""),
                        new Material("1.21.5", false, "Other", "SMOOTH_STONE", "Smooth Stone", Textures.GetBitmap("smooth_stone"), Textures.GetBitmap("smooth_stone"), $"minecraft:smooth_stone", $"minecraft:smooth_stone", ""),

                        new Material("1.13", true, "Coral", "CRL_BRAIN", "Brain Coral", Textures.GetBitmap("brain_coral_block"), Textures.GetBitmap("brain_coral_block"), $"minecraft:brain_coral_block", $"minecraft:brain_coral_block", ""),
                        new Material("1.13", true, "Coral", "CRL_BUBBLE", "Bubble Coral", Textures.GetBitmap("bubble_coral_block"), Textures.GetBitmap("bubble_coral_block"), $"minecraft:bubble_coral_block", $"minecraft:bubble_coral_block", ""),
                        new Material("1.13", true, "Coral", "CRL_FIRE", "Fire Coral", Textures.GetBitmap("fire_coral_block"), Textures.GetBitmap("fire_coral_block"), $"minecraft:fire_coral_block", $"minecraft:fire_coral_block", ""),
                        new Material("1.13", true, "Coral", "CRL_HORN", "Horn Coral", Textures.GetBitmap("horn_coral_block"), Textures.GetBitmap("horn_coral_block"), $"minecraft:horn_coral_block", $"minecraft:horn_coral_block", ""),
                        new Material("1.13", true, "Coral", "CRL_TUBE", "Tube Coral", Textures.GetBitmap("tube_coral_block"), Textures.GetBitmap("tube_coral_block"), $"minecraft:tube_coral_block", $"minecraft:tube_coral_block", ""),
                        new Material("1.13", false, "Coral", "KELP_DRIED", "Dried Kelp", Textures.GetBitmap("dried_kelp_top"), Textures.GetBitmap("dried_kelp_side"), $"minecraft:dried_kelp_block", $"minecraft:dried_kelp_block", ""),

                        new Material("1.13", false, "Dead Coral", "DEAD_CRL_BRAIN", "Dead Brain Coral", Textures.GetBitmap("dead_brain_coral_block"), Textures.GetBitmap("dead_brain_coral_block"), $"minecraft:dead_brain_coral_block", $"minecraft:dead_brain_coral_block", ""),
                        new Material("1.13", false, "Dead Coral", "DEAD_CRL_BUBBLE", "Dead Bubble Coral", Textures.GetBitmap("dead_bubble_coral_block"), Textures.GetBitmap("dead_bubble_coral_block"), $"minecraft:dead_bubble_coral_block", $"minecraft:dead_bubble_coral_block", ""),
                        new Material("1.13", false, "Dead Coral", "DEAD_CRL_FIRE", "Dead Fire Coral", Textures.GetBitmap("dead_fire_coral_block"), Textures.GetBitmap("dead_fire_coral_block"), $"minecraft:dead_fire_coral_block", $"minecraft:dead_fire_coral_block", ""),
                        new Material("1.13", false, "Dead Coral", "DEAD_CRL_HORN", "Dead Horn Coral", Textures.GetBitmap("dead_horn_coral_block"), Textures.GetBitmap("dead_horn_coral_block"), $"minecraft:dead_horn_coral_block", $"minecraft:dead_horn_coral_block", ""),
                        new Material("1.13", false, "Dead Coral", "DEAD_CRL_TUBE", "Dead Tube Coral", Textures.GetBitmap("dead_tube_coral_block"), Textures.GetBitmap("dead_tube_coral_block"), $"minecraft:dead_tube_coral_block", $"minecraft:dead_tube_coral_block", ""),

                        new Material("1.7", true, "Ores", "ORE_C", "Coal Ore", Textures.GetBitmap("coal_ore"), Textures.GetBitmap("coal_ore"), $"minecraft:coal_ore", $"minecraft:coal_ore", "minecraft:coal_ore"),
                        new Material("1.7", true, "Ores", "ORE_Cu", "Copper Ore", Textures.GetBitmap("copper_ore"), Textures.GetBitmap("copper_ore"), $"minecraft:copper_ore", $"minecraft:copper_ore", "minecraft:copper_ore"),
                        new Material("1.7", true, "Ores", "ORE_I", "Iron Ore", Textures.GetBitmap("iron_ore"), Textures.GetBitmap("iron_ore"), $"minecraft:iron_ore", $"minecraft:iron_ore", "minecraft:iron_ore"),
                        new Material("1.7", true, "Ores", "ORE_G", "Gold Ore", Textures.GetBitmap("gold_ore"), Textures.GetBitmap("gold_ore"), $"minecraft:gold_ore", $"minecraft:gold_ore", "minecraft:gold_ore"),
                        new Material("1.7", true, "Ores", "ORE_R", "Redstone Ore", Textures.GetBitmap("redstone_ore"), Textures.GetBitmap("redstone_ore"), $"minecraft:redstone_ore", $"minecraft:redstone_ore", "minecraft:redstone_ore"),
                        new Material("1.7", true, "Ores", "ORE_L", "Lapis Ore", Textures.GetBitmap("lapis_ore"), Textures.GetBitmap("lapis_ore"), $"minecraft:lapis_ore", $"minecraft:lapis_ore", "minecraft:lapis_ore"),
                        new Material("1.7", true, "Ores", "ORE_D", "Diamond Ore", Textures.GetBitmap("diamond_ore"), Textures.GetBitmap("diamond_ore"), $"minecraft:diamond_ore", $"minecraft:diamond_ore", "minecraft:diamond_ore"),
                        new Material("1.7", true, "Ores", "ORE_E", "Emerald Ore", Textures.GetBitmap("emerald_ore"), Textures.GetBitmap("emerald_ore"), $"minecraft:emerald_ore", $"minecraft:emerald_ore", "minecraft:emerald_ore"),
                        new Material("1.7", false, "Ores", "ORE_QUARTZ", "Quartz Ore", Textures.GetBitmap("nether_quartz_ore"), Textures.GetBitmap("nether_quartz_ore"), $"minecraft:nether_quartz_ore", $"minecraft:nether_quartz_ore", "minecraft:quartz_ore"),
                        new Material("1.16", false, "Ores", "ORE_G_NETHER", "Nether Gold Ore", Textures.GetBitmap("nether_gold_ore"), Textures.GetBitmap("nether_gold_ore"), $"minecraft:nether_gold_ore", $"minecraft:nether_gold_ore", ""),
                        new Material("1.16", false, "Ores", "BLK_STONE_GILDED", "Guilded Blackstone", Textures.GetBitmap("gilded_blackstone"), Textures.GetBitmap("gilded_blackstone"), $"minecraft:gilded_blackstone", $"minecraft:gilded_blackstone", ""),
                        new Material("1.17", false, "Ores", "ORE_G_RAW", "Raw Gold Block", Textures.GetBitmap("raw_gold_block"), Textures.GetBitmap("raw_gold_block"), $"minecraft:raw_gold_block", $"minecraft:raw_gold_block", ""),
                        new Material("1.17", false, "Ores", "ORE_Cp_RAW", "Raw Copper Block", Textures.GetBitmap("raw_copper_block"), Textures.GetBitmap("raw_copper_block"), $"minecraft:raw_copper_block", $"minecraft:raw_copper_block", ""),
                        new Material("1.17", false, "Ores", "ORE_I_RAW", "Raw Iron Block", Textures.GetBitmap("raw_iron_block"), Textures.GetBitmap("raw_iron_block"), $"minecraft:raw_iron_block", $"minecraft:raw_iron_block", ""),

                        new Material("1.17", true, "Ores (Deepslate)", "ORE_C_DS", "Deepslate Coal Ore", Textures.GetBitmap("deepslate_coal_ore"), Textures.GetBitmap("deepslate_coal_ore"), $"minecraft:deepslate_coal_ore", $"minecraft:deepslate_coal_ore", "minecraft:deepslate_coal_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_Cu_DS", "Deepslate Copper Ore", Textures.GetBitmap("deepslate_copper_ore"), Textures.GetBitmap("deepslate_copper_ore"), $"minecraft:deepslate_copper_ore", $"minecraft:deepslate_copper_ore", "minecraft:deepslate_copper_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_I_DS", "Deepslate Iron Ore", Textures.GetBitmap("deepslate_iron_ore"), Textures.GetBitmap("deepslate_iron_ore"), $"minecraft:deepslate_iron_ore", $"minecraft:deepslate_iron_ore", "minecraft:deepslate_iron_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_G_DS", "Deepslate Gold Ore", Textures.GetBitmap("deepslate_gold_ore"), Textures.GetBitmap("deepslate_gold_ore"), $"minecraft:gold_ore", $"minecraft:gold_ore", "minecraft:deepslate_gold_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_R_DS", "Deepslate Redstone Ore", Textures.GetBitmap("deepslate_redstone_ore"), Textures.GetBitmap("deepslate_redstone_ore"), $"minecraft:deepslate_redstone_ore", $"minecraft:deepslate_redstone_ore", "minecraft:deepslate_redstone_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_L_DS", "Deepslate Lapis Ore", Textures.GetBitmap("deepslate_lapis_ore"), Textures.GetBitmap("deepslate_lapis_ore"), $"minecraft:deepslate_lapis_ore", $"minecraft:deepslate_lapis_ore", "minecraft:deepslate_lapis_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_D_DS", "Deepslate Diamond Ore", Textures.GetBitmap("deepslate_diamond_ore"), Textures.GetBitmap("deepslate_diamond_ore"), $"minecraft:deepslate_diamond_ore", $"minecraft:deepslate_diamond_ore", "minecraft:deepslate_diamond_ore"),
                        new Material("1.17", true, "Ores (Deepslate)", "ORE_E_DS", "Deepslate Emerald Ore", Textures.GetBitmap("deepslate_emerald_ore"), Textures.GetBitmap("deepslate_emerald_ore"), $"minecraft:deepslate_emerald_ore", $"minecraft:deepslate_emerald_ore", "minecraft:deepslate_emerald_ore"),

                        new Material("1.12", true, "Terracotta", "GLAZED_00", "White Terracotta", Textures.GetBitmap("white_glazed_terracotta"), Textures.GetBitmap("white_glazed_terracotta"), $"minecraft:white_glazed_terracotta", $"minecraft:white_glazed_terracotta", "minecraft:white_glazed_terracotta"),
                        new Material("1.12", true, "Terracotta", "GLAZED_01", "Orange Terracotta", Textures.GetBitmap("orange_glazed_terracotta"), Textures.GetBitmap("orange_glazed_terracotta"), $"minecraft:orange_glazed_terracotta", $"minecraft:orange_glazed_terracotta", "minecraft:orange_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_02", "Magenta Terracotta", Textures.GetBitmap("magenta_glazed_terracotta"), Textures.GetBitmap("magenta_glazed_terracotta"), $"minecraft:magenta_glazed_terracotta", $"minecraft:magenta_glazed_terracotta", "minecraft:magenta_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_03", "Light Blue Terracotta", Textures.GetBitmap("light_blue_glazed_terracotta"), Textures.GetBitmap("light_blue_glazed_terracotta"), $"minecraft:light_blue_glazed_terracotta", $"minecraft:light_blue_glazed_terracotta", "minecraft:light_blue_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_04", "Yellow Terracotta", Textures.GetBitmap("yellow_glazed_terracotta"), Textures.GetBitmap("yellow_glazed_terracotta"), $"minecraft:yellow_glazed_terracotta", $"minecraft:yellow_glazed_terracotta", "minecraft:yellow_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_05", "Lime Terracotta", Textures.GetBitmap("lime_glazed_terracotta"), Textures.GetBitmap("lime_glazed_terracotta"), $"minecraft:lime_glazed_terracotta", $"minecraft:lime_glazed_terracotta", "minecraft:lime_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_06", "Pink Terracotta", Textures.GetBitmap("pink_glazed_terracotta"), Textures.GetBitmap("pink_glazed_terracotta"), $"minecraft:pink_glazed_terracotta", $"minecraft:pink_glazed_terracotta", "minecraft:pink_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_07", "Gray Terracotta", Textures.GetBitmap("gray_glazed_terracotta"), Textures.GetBitmap("gray_glazed_terracotta"), $"minecraft:gray_glazed_terracotta", $"minecraft:gray_glazed_terracotta", "minecraft:gray_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_08", "Light Gray Terracotta", Textures.GetBitmap("light_gray_glazed_terracotta"), Textures.GetBitmap("light_gray_glazed_terracotta"), $"minecraft:light_gray_glazed_terracotta", $"minecraft:light_gray_glazed_terracotta", "minecraft:silver_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_09", "Cyan Terracotta", Textures.GetBitmap("cyan_glazed_terracotta"), Textures.GetBitmap("cyan_glazed_terracotta"), $"minecraft:cyan_glazed_terracotta", $"minecraft:cyan_glazed_terracotta", "minecraft:cyan_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_10", "Purple Terracotta", Textures.GetBitmap("purple_glazed_terracotta"), Textures.GetBitmap("purple_glazed_terracotta"), $"minecraft:purple_glazed_terracotta", $"minecraft:purple_glazed_terracotta", "minecraft:purple_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_11", "Blue Terracotta", Textures.GetBitmap("blue_glazed_terracotta"), Textures.GetBitmap("blue_glazed_terracotta"), $"minecraft:blue_glazed_terracotta", $"minecraft:blue_glazed_terracotta", "minecraft:blue_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_12", "Brown Terracotta", Textures.GetBitmap("brown_glazed_terracotta"), Textures.GetBitmap("brown_glazed_terracotta"), $"minecraft:brown_glazed_terracotta", $"minecraft:brown_glazed_terracotta", "minecraft:brown_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_13", "Green Terracotta", Textures.GetBitmap("green_glazed_terracotta"), Textures.GetBitmap("green_glazed_terracotta"), $"minecraft:green_glazed_terracotta", $"minecraft:green_glazed_terracotta", "minecraft:green_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_14", "Red Terracotta", Textures.GetBitmap("red_glazed_terracotta"), Textures.GetBitmap("red_glazed_terracotta"), $"minecraft:red_glazed_terracotta", $"minecraft:red_glazed_terracotta", "minecraft:red_glazed_terracotta"),
                        new Material("1.12", false, "Terracotta", "GLAZED_15", "Black Terracotta", Textures.GetBitmap("black_glazed_terracotta"), Textures.GetBitmap("black_glazed_terracotta"), $"minecraft:black_glazed_terracotta", $"minecraft:black_glazed_terracotta", "minecraft:black_glazed_terracotta"),

                        new Material("1.7", true, "Carpet", "CARPET_00", "White Carpet", Textures.GetBitmap("white_wool"), Textures.GetBitmap("white_wool"), $"minecraft:white_carpet", $"minecraft:white_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_01", "Orange Carpet", Textures.GetBitmap("orange_wool"), Textures.GetBitmap("orange_wool"), $"minecraft:orange_carpet", $"minecraft:orange_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_02", "Magenta Carpet", Textures.GetBitmap("magenta_wool"), Textures.GetBitmap("magenta_wool"), $"minecraft:magenta_carpet", $"minecraft:magenta_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_03", "Light Blue Carpet", Textures.GetBitmap("light_blue_wool"), Textures.GetBitmap("light_blue_wool"), $"minecraft:light_blue_carpet", $"minecraft:light_blue_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_04", "Yellow Carpet", Textures.GetBitmap("yellow_wool"), Textures.GetBitmap("yellow_wool"), $"minecraft:yellow_carpet", $"minecraft:yellow_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_05", "Lime Carpet", Textures.GetBitmap("lime_wool"), Textures.GetBitmap("lime_wool"), $"minecraft:lime_carpet", $"minecraft:lime_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_06", "Pink Carpet", Textures.GetBitmap("pink_wool"), Textures.GetBitmap("pink_wool"), $"minecraft:pink_carpet", $"minecraft:pink_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_07", "Gray Carpet", Textures.GetBitmap("gray_wool"), Textures.GetBitmap("gray_wool"), $"minecraft:gray_carpet", $"minecraft:gray_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_08", "Light Gray Carpet", Textures.GetBitmap("light_gray_wool"), Textures.GetBitmap("light_gray_wool"), $"minecraft:light_gray_carpet", $"minecraft:light_gray_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_09", "Cyan Carpet", Textures.GetBitmap("cyan_wool"), Textures.GetBitmap("cyan_wool"), $"minecraft:cyan_carpet", $"minecraft:cyan_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_10", "Purple Carpet", Textures.GetBitmap("purple_wool"), Textures.GetBitmap("purple_wool"), $"minecraft:purple_carpet", $"minecraft:purple_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_11", "Blue Carpet", Textures.GetBitmap("blue_wool"), Textures.GetBitmap("blue_wool"), $"minecraft:blue_carpet", $"minecraft:blue_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_12", "Brown Carpet", Textures.GetBitmap("brown_wool"), Textures.GetBitmap("brown_wool"), $"minecraft:brown_carpet", $"minecraft:brown_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_13", "Green Carpet", Textures.GetBitmap("green_wool"), Textures.GetBitmap("green_wool"), $"minecraft:green_carpet", $"minecraft:green_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_14", "Red Carpet", Textures.GetBitmap("red_wool"), Textures.GetBitmap("red_wool"), $"minecraft:red_carpet", $"minecraft:red_carpet", "minecraft:carpet"),
                        new Material("1.7", true, "Carpet", "CARPET_15", "Black Carpet", Textures.GetBitmap("black_wool"), Textures.GetBitmap("black_wool"), $"minecraft:black_carpet", $"minecraft:black_carpet", "minecraft:carpet"),
                        new Material("1.17", true, "Carpet", "CARPET_MOSS", "Moss Carpet", Textures.GetBitmap("moss_block"), Textures.GetBitmap("moss_block"), $"minecraft:moss_carpet", $"minecraft:moss_carpet", ""),

                        new Material("1.19", false, "Dirt", "MUD_PKD", "Packed Mud", Textures.GetBitmap("packed_mud"), Textures.GetBitmap("packed_mud"), $"minecraft:packed_mud", $"minecraft:packed_mud", ""),
                        new Material("1.17", false, "Dirt", "DIRT_ROOTED", "Rooted Dirt", Textures.GetBitmap("rooted_dirt"), Textures.GetBitmap("rooted_dirt"), $"minecraft:rooted_dirt", $"minecraft:rooted_dirt", ""),
                        new Material("1.7", false, "Dirt", "DIRT", "Dirt", Textures.GetBitmap("dirt"), Textures.GetBitmap("dirt"), $"minecraft:dirt", $"minecraft:dirt", "minecraft:dirt"),
                        new Material("1.8", false, "Dirt", "DIRT_COARSE", "Coarse Dirt", Textures.GetBitmap("coarse_dirt"), Textures.GetBitmap("coarse_dirt"), $"minecraft:coarse_dirt", $"minecraft:coarse_dirt", "minecraft:dirt"),
                        new Material("1.17", false, "Dirt", "GRASS_PATH", "Dirt Path", Textures.GetBitmap("dirt_path_top"), Textures.GetBitmap("dirt_path_side"), $"minecraft:dirt_path", $"minecraft:dirt_path", "minecraft:dirt_path"),
                        new Material("1.7", false, "Dirt", "DIRT_Podzol", "Podzol", Textures.GetBitmap("podzol_top"), Textures.GetBitmap("podzol_side"), $"minecraft:podzol", $"minecraft:podzol", "minecraft:dirt"),
                        new Material("1.19", false, "Dirt", "MUD_MNGRO_ROOT", "Muddy Mangrove Roots", Textures.GetBitmap("muddy_mangrove_roots_top"), Textures.GetBitmap("muddy_mangrove_roots_top"), $"minecraft:muddy_mangrove_roots[axis=y]", $"minecraft:muddy_mangrove_roots[axis=z]", ""),
                        new Material("1.19", false, "Dirt", "MUD", "Mud", Textures.GetBitmap("mud"), Textures.GetBitmap("mud"), $"minecraft:mud", $"minecraft:mud", ""),

                        new Material("1.17", false, "Transparent", "Cu_GRATE", "Waxed Copper Grate", Textures.GetBitmap("copper_grate"), Textures.GetBitmap("copper_grate"), $"minecraft:waxed_copper_grate", $"minecraft:waxed_copper_grate", ""),
                        new Material("1.17", false, "Transparent", "Cu_GRATE_EXPO", "Waxed Exposed Copper Grate", Textures.GetBitmap("exposed_copper_grate"), Textures.GetBitmap("exposed_copper_grate"), $"minecraft:waxed_exposed_copper_grate", $"minecraft:waxed_exposed_copper_grate", ""),
                        new Material("1.17", false, "Transparent", "Cu_GRATE_WTHER", "Waxed Weathered Copper Grate", Textures.GetBitmap("weathered_copper_grate"), Textures.GetBitmap("weathered_copper_grate"), $"minecraft:waxed_weathered_copper_grate", $"minecraft:waxed_weathered_copper_grate", ""),
                        new Material("1.17", false, "Transparent", "Cu_GRATE_OXI", "Waxed Oxidized Copper Grate", Textures.GetBitmap("oxidized_copper_grate"), Textures.GetBitmap("oxidized_copper_grate"), $"minecraft:waxed_oxidized_copper_grate", $"minecraft:waxed_oxidized_copper_grate", ""),

                        new Material("1.21.5", false, "Other", "SANDSTONE", "Sandstone", Textures.GetBitmap("sandstone_top"), Textures.GetBitmap("sandstone"), $"minecraft:sandstone", $"minecraft:sandstone", ""),
                        new Material("1.21.5", false, "Other", "CHZL_SANDSTONE", "Chiseled Sandstone", Textures.GetBitmap("sandstone_top"), Textures.GetBitmap("chiseled_sandstone"), $"minecraft:chiseled_sandstone", $"minecraft:chiseled_sandstone", ""),
                        new Material("1.21.5", false, "Other", "CUT_SANDSTONE", "Cut Sandstone", Textures.GetBitmap("sandstone_top"), Textures.GetBitmap("cut_sandstone"), $"minecraft:cut_sandstone", $"minecraft:cut_sandstone", ""),

                        new Material("1.21.5", false, "Other", "RED_SANDSTONE", "Red Sandstone", Textures.GetBitmap("red_sandstone_top"), Textures.GetBitmap("red_sandstone"), $"minecraft:red_sandstone", $"minecraft:red_sandstone", ""),
                        new Material("1.21.5", false, "Other", "CHZL_RED_SANDSTONE", "Chiseled Red Sandstone", Textures.GetBitmap("red_sandstone_top"), Textures.GetBitmap("chiseled_red_sandstone"), $"minecraft:chiseled_red_sandstone", $"minecraft:chiseled_red_sandstone", ""),
                        new Material("1.21.5", false, "Other", "CUT_RED_SANDSTONE", "Cut Red Sandstone", Textures.GetBitmap("red_sandstone_top"), Textures.GetBitmap("cut_red_sandstone"), $"minecraft:cut_red_sandstone", $"minecraft:cut_red_sandstone", ""),

                        new Material("1.21.5", false, "Other", "REDSTONE_LAMP", "Redstone Lamp", Textures.GetBitmap("redstone_lamp"), Textures.GetBitmap("redstone_lamp"), $"minecraft:redstone_lamp[lit=false]", $"minecraft:redstone_lamp[lit=false]", ""),

                        new Material("1.21.5", false, "Crafting", "DROPPER", "Dropper", Textures.GetBitmap("furnace_top"), Textures.GetBitmap("dropper_front"), $"minecraft:dropper[facing=south]", $"minecraft:dropper[facing=south]", ""),
                        new Material("1.21.5", false, "Crafting", "DISPENSER_TOP", "Dispenser (Top)", Textures.GetBitmap("dispenser_front_vertical"), Textures.GetBitmap("furnace_top"), $"minecraft:dispenser[facing=up]", $"minecraft:dispenser[facing=up]", ""),
                        new Material("1.21.5", false, "Crafting", "DISPENSER_FRONT", "Dispenser (Front)", Textures.GetBitmap("furnace_top"), Textures.GetBitmap("dispenser_front"), $"minecraft:dispenser[facing=south]", $"minecraft:dispenser[facing=south]", ""),
                        new Material("1.21.5", false, "Crafting", "DISPENSER_SIDE", "Dispenser (Side)", Textures.GetBitmap("furnace_top"), Textures.GetBitmap("furnace_side"), $"minecraft:dispenser[facing=north]", $"minecraft:dispenser[facing=north]", ""),
                        new Material("1.21.5", false, "Crafting", "LOOM_FRONT", "Loom (Front)", Textures.GetBitmap("loom_top"), Textures.GetBitmap("loom_front"), $"minecraft:loom[facing=south]", $"minecraft:loom[facing=south]", ""),
                        new Material("1.21.5", false, "Crafting", "LOOM_SIDE", "Loom (Top)", Textures.GetBitmap("loom_top"), Textures.GetBitmap("loom_side"), $"minecraft:loom[facing=north]", $"minecraft:loom[facing=north]", ""),
                        new Material("1.21.5", false, "Crafting", "SMOKER_FRONT", "Smoker (Front)", Textures.GetBitmap("smoker_top"), Textures.GetBitmap("smoker_front"), $"minecraft:smoker[facing=south,lit=false]", $"minecraft:smoker[facing=south,lit=false]", ""),
                        new Material("1.21.5", false, "Crafting", "SMOKER_SIDE", "Smoker (Side)", Textures.GetBitmap("smoker_top"), Textures.GetBitmap("smoker_side"), $"minecraft:smoker[facing=north,lit=true]", $"minecraft:smoker[facing=north,lit=true]", ""),
                        new Material("1.21.5", false, "Crafting", "BLAST_FURNACE_FRONT", "Blast Furnace (Front)", Textures.GetBitmap("blast_furnace_top"), Textures.GetBitmap("blast_furnace_front"), $"minecraft:blast_furnace[facing=south,lit=false]", $"minecraft:blast_furnace[facing=south,lit=false]", ""),
                        new Material("1.21.5", false, "Crafting", "BLAST_FURNACE_SIDE", "Blast Furnace (Side)", Textures.GetBitmap("blast_furnace_top"), Textures.GetBitmap("blast_furnace_side"), $"minecraft:blast_furnace[facing=north,lit=true]", $"minecraft:blast_furnace[facing=north,lit=true]", ""),
                        new Material("1.21.5", false, "Crafting", "FURNACE", "Furnace", Textures.GetBitmap("furnace_top"), Textures.GetBitmap("furnace_front"), $"minecraft:furnace[facing=south,lit=false]", $"minecraft:furnace[facing=south,lit=false]", ""),
                        new Material("1.21.5", false, "Crafting", "CRAFT_TABLE", "Crafting Table", Textures.GetBitmap("crafting_table_top"), Textures.GetBitmap("crafting_table_side"), $"minecraft:crafting_table", $"minecraft:crafting_table", ""),
                        new Material("1.21.5", false, "Crafting", "CARTOGRAPHY_TABLE", "Cartography Table", Textures.GetBitmap("cartography_table_top"), Textures.GetBitmap("cartography_table_side2"), $"minecraft:cartography_table", $"minecraft:cartography_table", ""),
                        new Material("1.21.5", false, "Crafting", "FLETCHING_TABLE", "Fletching Table", Textures.GetBitmap("fletching_table_top"), Textures.GetBitmap("fletching_table_front"), $"minecraft:fletching_table", $"minecraft:fletching_table", ""),
                        new Material("1.21.5", false, "Crafting", "SMITHING_TABLE", "Smithing Table", Textures.GetBitmap("smithing_table_top"), Textures.GetBitmap("smithing_table_front"), $"minecraft:smithing_table", $"minecraft:smithing_table", ""),

                        new Material("1.21.5", false, "Other", "PALE_MOSS_BLK", "Pale Moss Block", Textures.GetBitmap("pale_moss_block"), Textures.GetBitmap("pale_moss_block"), $"minecraft:pale_moss_block", $"minecraft:pale_moss_block", ""),

                        new Material("1.21.5", false, "Other", "CREAKING_HEART_TOP", "Creaking Heart (Top)", Textures.GetBitmap("creaking_heart_top"), Textures.GetBitmap("creaking_heart_top"), $"minecraft:creaking_heart[axis=y,creaking_heart_state=uprooted]", $"minecraft:creaking_heart[axis=z,creaking_heart_state=uprooted]", ""),
                        new Material("1.21.5", false, "Other", "CREAKING_HEART_SIDE", "Creaking Heart (Side)", Textures.GetBitmap("creaking_heart", 90), Textures.GetBitmap("creaking_heart", 90), $"minecraft:creaking_heart[axis=x,creaking_heart_state=uprooted]", $"minecraft:creaking_heart[axis=x,creaking_heart_state=uprooted]", ""),
                        new Material("1.21.5", false, "Other", "QUARTZ_PILLAR_TOP", "Quartz Pillar (Top)", Textures.GetBitmap("quartz_pillar_top"), Textures.GetBitmap("quartz_pillar_top"), $"minecraft:quartz_pillar[axis=y]", $"minecraft:quartz_pillar[axis=z]", ""),
                        new Material("1.21.5", false, "Other", "QUARTZ_PILLAR_SIDE", "Quartz Pillar (Side)", Textures.GetBitmap("quartz_pillar", 90), Textures.GetBitmap("quartz_pillar", 90), $"minecraft:quartz_pillar[axis=x]", $"minecraft:quartz_pillar[axis=x]", ""),

                        new Material("1.19", true, "Glowing", "FRG_LT_YELLOW", "Frog Light (Yellow Top)", Textures.GetBitmap("ochre_froglight_top"), Textures.GetBitmap("ochre_froglight_top"), $"minecraft:ochre_froglight[axis=y]", $"minecraft:ochre_froglight[axis=z]", ""),
                        new Material("1.19", true, "Glowing", "FRG_LT_PURPLE", "Frog Light (Purple Top)", Textures.GetBitmap("pearlescent_froglight_top"), Textures.GetBitmap("pearlescent_froglight_top"), $"minecraft:pearlescent_froglight[axis=y]", $"minecraft:pearlescent_froglight[axis=z]", ""),
                        new Material("1.19", true, "Glowing", "FRG_LT_GREEN", "Frog Light (Green Top)", Textures.GetBitmap("verdant_froglight_top"), Textures.GetBitmap("verdant_froglight_top"), $"minecraft:verdant_froglight[axis=y]", $"minecraft:verdant_froglight[axis=z]", ""),
                        new Material("1.19", true, "Glowing", "FRG_LT_YELLOW_SD", "Frog Light (Yellow Side)", Textures.GetBitmap("ochre_froglight_side"), Textures.GetBitmap("ochre_froglight_side"), $"minecraft:ochre_froglight[axis=x]", $"minecraft:ochre_froglight[axis=x]", ""),
                        new Material("1.19", true, "Glowing", "FRG_LT_PURPLE_SD", "Frog Light (Purple Side)", Textures.GetBitmap("pearlescent_froglight_side"), Textures.GetBitmap("pearlescent_froglight_side"), $"minecraft:pearlescent_froglight[axis=x]", $"minecraft:pearlescent_froglight[axis=x]", ""),
                        new Material("1.19", true, "Glowing", "FRG_LT_GREEN_SD", "Frog Light (Green Side)", Textures.GetBitmap("verdant_froglight_side"), Textures.GetBitmap("verdant_froglight_side"), $"minecraft:verdant_froglight[axis=x]", $"minecraft:verdant_froglight[axis=x]", ""),
                        new Material("1.7", true, "Glowing", "JACKOLANTERN", "Jackolantern", Textures.GetBitmap("pumpkin_top"), Textures.GetBitmap("jack_o_lantern"), $"minecraft:jack_o_lantern", $"minecraft:jack_o_lantern", "minecraft:pumpkin"),
                        new Material("1.16", true, "Glowing", "SHROOMLIGHT", "Shroomlight", Textures.GetBitmap("shroomlight"), Textures.GetBitmap("shroomlight"), $"minecraft:shroomlight", $"minecraft:shroomlight", ""),
                        new Material("1.16", false, "Glowing", "OBSIDIAN_CRYING", "Crying Obsidian", Textures.GetBitmap("crying_obsidian"), Textures.GetBitmap("crying_obsidian"), $"minecraft:crying_obsidian", $"minecraft:crying_obsidian", ""),
                        new Material("1.10", false, "Glowing", "MAGMA", "Magma", Textures.GetBitmap("magma"), Textures.GetBitmap("magma"), $"minecraft:magma_block", $"minecraft:magma_block", "minecraft:magma"),
                        new Material("1.13", true, "Glowing", "SEA_LANTURN", "Sea Lanturn", Textures.GetBitmap("sea_lantern"), Textures.GetBitmap("sea_lantern"), $"minecraft:sea_lantern", $"minecraft:sea_lantern", "minecraft:sea_lantern"),
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
                        //m.Category.Contains("Stripped") && m.BlockID == 17
                        //|| m.Category.Contains("Planks") && m.BlockID == 5
                        //|| m.Category.Contains("Log") && m.BlockID == 17 && !m.Label.Contains("Birch")
                        m.PixelStackerID == "SHROOM_INNER"
                        || m.PixelStackerID == "CLAY_HARD_00"
                        || m.PixelStackerID.Contains("GRANITE")
                        || m.PixelStackerID == "DIRT"
                        || m.PixelStackerID == "DRIPSTONE"
                        || m.PixelStackerID == "DIRT_ROOTED"
                        || m.PixelStackerID == "DIRT_COARSE"
                        || m.PixelStackerID == "SOUL_SOIL"
                        || m.PixelStackerID == "SOUL_SAND"
                        || m.PixelStackerID == "MUD"
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

        private static void AddTagsForColor(SKColor c, ref List<string> tags)
        {
            if (c.Alpha < 240 || c.Alpha < 240) { tags.Add("transparent"); } else { tags.Add("opaque"); }
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
