#if IMAGE_SHARP
#pragma warning disable IDE1006 // Naming Styles
namespace PixelStacker.Resources {
	using System;
	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.PixelFormats;

	public class Textures {

        private static global::System.Resources.ResourceManager resourceMan;
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (resourceMan is null) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PixelStacker.Resources.Textures", typeof(Textures).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        private static Image<Rgba32> _acacia_log = null;
        public static Image<Rgba32> acacia_log {
            get {
                if (_acacia_log == null)
                    _acacia_log = Image.Load((byte[])ResourceManager.GetObject("acacia_log"));
                return _acacia_log;
            }
        }

        private static Image<Rgba32> _acacia_planks = null;
        public static Image<Rgba32> acacia_planks {
            get {
                if (_acacia_planks == null)
                    _acacia_planks = Image.Load((byte[])ResourceManager.GetObject("acacia_planks"));
                return _acacia_planks;
            }
        }

        private static Image<Rgba32> _air = null;
        public static Image<Rgba32> air {
            get {
                if (_air == null)
                    _air = Image.Load((byte[])ResourceManager.GetObject("air"));
                return _air;
            }
        }

        private static Image<Rgba32> _amethyst_block = null;
        public static Image<Rgba32> amethyst_block {
            get {
                if (_amethyst_block == null)
                    _amethyst_block = Image.Load((byte[])ResourceManager.GetObject("amethyst_block"));
                return _amethyst_block;
            }
        }

        private static Image<Rgba32> _ancient_debris_side = null;
        public static Image<Rgba32> ancient_debris_side {
            get {
                if (_ancient_debris_side == null)
                    _ancient_debris_side = Image.Load((byte[])ResourceManager.GetObject("ancient_debris_side"));
                return _ancient_debris_side;
            }
        }

        private static Image<Rgba32> _ancient_debris_top = null;
        public static Image<Rgba32> ancient_debris_top {
            get {
                if (_ancient_debris_top == null)
                    _ancient_debris_top = Image.Load((byte[])ResourceManager.GetObject("ancient_debris_top"));
                return _ancient_debris_top;
            }
        }

        private static Image<Rgba32> _andesite = null;
        public static Image<Rgba32> andesite {
            get {
                if (_andesite == null)
                    _andesite = Image.Load((byte[])ResourceManager.GetObject("andesite"));
                return _andesite;
            }
        }

        private static Image<Rgba32> _barrier = null;
        public static Image<Rgba32> barrier {
            get {
                if (_barrier == null)
                    _barrier = Image.Load((byte[])ResourceManager.GetObject("barrier"));
                return _barrier;
            }
        }

        private static Image<Rgba32> _basalt_side = null;
        public static Image<Rgba32> basalt_side {
            get {
                if (_basalt_side == null)
                    _basalt_side = Image.Load((byte[])ResourceManager.GetObject("basalt_side"));
                return _basalt_side;
            }
        }

        private static Image<Rgba32> _basalt_top = null;
        public static Image<Rgba32> basalt_top {
            get {
                if (_basalt_top == null)
                    _basalt_top = Image.Load((byte[])ResourceManager.GetObject("basalt_top"));
                return _basalt_top;
            }
        }

        private static Image<Rgba32> _bedrock = null;
        public static Image<Rgba32> bedrock {
            get {
                if (_bedrock == null)
                    _bedrock = Image.Load((byte[])ResourceManager.GetObject("bedrock"));
                return _bedrock;
            }
        }

        private static Image<Rgba32> _beehive_end = null;
        public static Image<Rgba32> beehive_end {
            get {
                if (_beehive_end == null)
                    _beehive_end = Image.Load((byte[])ResourceManager.GetObject("beehive_end"));
                return _beehive_end;
            }
        }

        private static Image<Rgba32> _beehive_side = null;
        public static Image<Rgba32> beehive_side {
            get {
                if (_beehive_side == null)
                    _beehive_side = Image.Load((byte[])ResourceManager.GetObject("beehive_side"));
                return _beehive_side;
            }
        }

        private static Image<Rgba32> _bee_nest_side = null;
        public static Image<Rgba32> bee_nest_side {
            get {
                if (_bee_nest_side == null)
                    _bee_nest_side = Image.Load((byte[])ResourceManager.GetObject("bee_nest_side"));
                return _bee_nest_side;
            }
        }

        private static Image<Rgba32> _bee_nest_top = null;
        public static Image<Rgba32> bee_nest_top {
            get {
                if (_bee_nest_top == null)
                    _bee_nest_top = Image.Load((byte[])ResourceManager.GetObject("bee_nest_top"));
                return _bee_nest_top;
            }
        }

        private static Image<Rgba32> _birch_log = null;
        public static Image<Rgba32> birch_log {
            get {
                if (_birch_log == null)
                    _birch_log = Image.Load((byte[])ResourceManager.GetObject("birch_log"));
                return _birch_log;
            }
        }

        private static Image<Rgba32> _birch_planks = null;
        public static Image<Rgba32> birch_planks {
            get {
                if (_birch_planks == null)
                    _birch_planks = Image.Load((byte[])ResourceManager.GetObject("birch_planks"));
                return _birch_planks;
            }
        }

        private static Image<Rgba32> _blackstone = null;
        public static Image<Rgba32> blackstone {
            get {
                if (_blackstone == null)
                    _blackstone = Image.Load((byte[])ResourceManager.GetObject("blackstone"));
                return _blackstone;
            }
        }

        private static Image<Rgba32> _blackstone_top = null;
        public static Image<Rgba32> blackstone_top {
            get {
                if (_blackstone_top == null)
                    _blackstone_top = Image.Load((byte[])ResourceManager.GetObject("blackstone_top"));
                return _blackstone_top;
            }
        }

        private static Image<Rgba32> _black_concrete = null;
        public static Image<Rgba32> black_concrete {
            get {
                if (_black_concrete == null)
                    _black_concrete = Image.Load((byte[])ResourceManager.GetObject("black_concrete"));
                return _black_concrete;
            }
        }

        private static Image<Rgba32> _black_concrete_powder = null;
        public static Image<Rgba32> black_concrete_powder {
            get {
                if (_black_concrete_powder == null)
                    _black_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("black_concrete_powder"));
                return _black_concrete_powder;
            }
        }

        private static Image<Rgba32> _black_glazed_terracotta = null;
        public static Image<Rgba32> black_glazed_terracotta {
            get {
                if (_black_glazed_terracotta == null)
                    _black_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("black_glazed_terracotta"));
                return _black_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _black_stained_glass = null;
        public static Image<Rgba32> black_stained_glass {
            get {
                if (_black_stained_glass == null)
                    _black_stained_glass = Image.Load((byte[])ResourceManager.GetObject("black_stained_glass"));
                return _black_stained_glass;
            }
        }

        private static Image<Rgba32> _black_terracotta = null;
        public static Image<Rgba32> black_terracotta {
            get {
                if (_black_terracotta == null)
                    _black_terracotta = Image.Load((byte[])ResourceManager.GetObject("black_terracotta"));
                return _black_terracotta;
            }
        }

        private static Image<Rgba32> _black_wool = null;
        public static Image<Rgba32> black_wool {
            get {
                if (_black_wool == null)
                    _black_wool = Image.Load((byte[])ResourceManager.GetObject("black_wool"));
                return _black_wool;
            }
        }

        private static Image<Rgba32> _blue_concrete = null;
        public static Image<Rgba32> blue_concrete {
            get {
                if (_blue_concrete == null)
                    _blue_concrete = Image.Load((byte[])ResourceManager.GetObject("blue_concrete"));
                return _blue_concrete;
            }
        }

        private static Image<Rgba32> _blue_concrete_powder = null;
        public static Image<Rgba32> blue_concrete_powder {
            get {
                if (_blue_concrete_powder == null)
                    _blue_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("blue_concrete_powder"));
                return _blue_concrete_powder;
            }
        }

        private static Image<Rgba32> _blue_glazed_terracotta = null;
        public static Image<Rgba32> blue_glazed_terracotta {
            get {
                if (_blue_glazed_terracotta == null)
                    _blue_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("blue_glazed_terracotta"));
                return _blue_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _blue_ice = null;
        public static Image<Rgba32> blue_ice {
            get {
                if (_blue_ice == null)
                    _blue_ice = Image.Load((byte[])ResourceManager.GetObject("blue_ice"));
                return _blue_ice;
            }
        }

        private static Image<Rgba32> _blue_stained_glass = null;
        public static Image<Rgba32> blue_stained_glass {
            get {
                if (_blue_stained_glass == null)
                    _blue_stained_glass = Image.Load((byte[])ResourceManager.GetObject("blue_stained_glass"));
                return _blue_stained_glass;
            }
        }

        private static Image<Rgba32> _blue_terracotta = null;
        public static Image<Rgba32> blue_terracotta {
            get {
                if (_blue_terracotta == null)
                    _blue_terracotta = Image.Load((byte[])ResourceManager.GetObject("blue_terracotta"));
                return _blue_terracotta;
            }
        }

        private static Image<Rgba32> _blue_wool = null;
        public static Image<Rgba32> blue_wool {
            get {
                if (_blue_wool == null)
                    _blue_wool = Image.Load((byte[])ResourceManager.GetObject("blue_wool"));
                return _blue_wool;
            }
        }

        private static Image<Rgba32> _bone_block_side = null;
        public static Image<Rgba32> bone_block_side {
            get {
                if (_bone_block_side == null)
                    _bone_block_side = Image.Load((byte[])ResourceManager.GetObject("bone_block_side"));
                return _bone_block_side;
            }
        }

        private static Image<Rgba32> _bone_block_top = null;
        public static Image<Rgba32> bone_block_top {
            get {
                if (_bone_block_top == null)
                    _bone_block_top = Image.Load((byte[])ResourceManager.GetObject("bone_block_top"));
                return _bone_block_top;
            }
        }

        private static Image<Rgba32> _bookshelf = null;
        public static Image<Rgba32> bookshelf {
            get {
                if (_bookshelf == null)
                    _bookshelf = Image.Load((byte[])ResourceManager.GetObject("bookshelf"));
                return _bookshelf;
            }
        }

        private static Image<Rgba32> _brain_coral_block = null;
        public static Image<Rgba32> brain_coral_block {
            get {
                if (_brain_coral_block == null)
                    _brain_coral_block = Image.Load((byte[])ResourceManager.GetObject("brain_coral_block"));
                return _brain_coral_block;
            }
        }

        private static Image<Rgba32> _bricks = null;
        public static Image<Rgba32> bricks {
            get {
                if (_bricks == null)
                    _bricks = Image.Load((byte[])ResourceManager.GetObject("bricks"));
                return _bricks;
            }
        }

        private static Image<Rgba32> _brown_concrete = null;
        public static Image<Rgba32> brown_concrete {
            get {
                if (_brown_concrete == null)
                    _brown_concrete = Image.Load((byte[])ResourceManager.GetObject("brown_concrete"));
                return _brown_concrete;
            }
        }

        private static Image<Rgba32> _brown_concrete_powder = null;
        public static Image<Rgba32> brown_concrete_powder {
            get {
                if (_brown_concrete_powder == null)
                    _brown_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("brown_concrete_powder"));
                return _brown_concrete_powder;
            }
        }

        private static Image<Rgba32> _brown_glazed_terracotta = null;
        public static Image<Rgba32> brown_glazed_terracotta {
            get {
                if (_brown_glazed_terracotta == null)
                    _brown_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("brown_glazed_terracotta"));
                return _brown_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _brown_mushroom_block = null;
        public static Image<Rgba32> brown_mushroom_block {
            get {
                if (_brown_mushroom_block == null)
                    _brown_mushroom_block = Image.Load((byte[])ResourceManager.GetObject("brown_mushroom_block"));
                return _brown_mushroom_block;
            }
        }

        private static Image<Rgba32> _brown_stained_glass = null;
        public static Image<Rgba32> brown_stained_glass {
            get {
                if (_brown_stained_glass == null)
                    _brown_stained_glass = Image.Load((byte[])ResourceManager.GetObject("brown_stained_glass"));
                return _brown_stained_glass;
            }
        }

        private static Image<Rgba32> _brown_terracotta = null;
        public static Image<Rgba32> brown_terracotta {
            get {
                if (_brown_terracotta == null)
                    _brown_terracotta = Image.Load((byte[])ResourceManager.GetObject("brown_terracotta"));
                return _brown_terracotta;
            }
        }

        private static Image<Rgba32> _brown_wool = null;
        public static Image<Rgba32> brown_wool {
            get {
                if (_brown_wool == null)
                    _brown_wool = Image.Load((byte[])ResourceManager.GetObject("brown_wool"));
                return _brown_wool;
            }
        }

        private static Image<Rgba32> _bubble_coral_block = null;
        public static Image<Rgba32> bubble_coral_block {
            get {
                if (_bubble_coral_block == null)
                    _bubble_coral_block = Image.Load((byte[])ResourceManager.GetObject("bubble_coral_block"));
                return _bubble_coral_block;
            }
        }

        private static Image<Rgba32> _budding_amethyst = null;
        public static Image<Rgba32> budding_amethyst {
            get {
                if (_budding_amethyst == null)
                    _budding_amethyst = Image.Load((byte[])ResourceManager.GetObject("budding_amethyst"));
                return _budding_amethyst;
            }
        }

        private static Image<Rgba32> _calcite = null;
        public static Image<Rgba32> calcite {
            get {
                if (_calcite == null)
                    _calcite = Image.Load((byte[])ResourceManager.GetObject("calcite"));
                return _calcite;
            }
        }

        private static Image<Rgba32> _carved_pumpkin = null;
        public static Image<Rgba32> carved_pumpkin {
            get {
                if (_carved_pumpkin == null)
                    _carved_pumpkin = Image.Load((byte[])ResourceManager.GetObject("carved_pumpkin"));
                return _carved_pumpkin;
            }
        }

        private static Image<Rgba32> _chiseled_deepslate = null;
        public static Image<Rgba32> chiseled_deepslate {
            get {
                if (_chiseled_deepslate == null)
                    _chiseled_deepslate = Image.Load((byte[])ResourceManager.GetObject("chiseled_deepslate"));
                return _chiseled_deepslate;
            }
        }

        private static Image<Rgba32> _chiseled_nether_bricks = null;
        public static Image<Rgba32> chiseled_nether_bricks {
            get {
                if (_chiseled_nether_bricks == null)
                    _chiseled_nether_bricks = Image.Load((byte[])ResourceManager.GetObject("chiseled_nether_bricks"));
                return _chiseled_nether_bricks;
            }
        }

        private static Image<Rgba32> _chiseled_polished_blackstone = null;
        public static Image<Rgba32> chiseled_polished_blackstone {
            get {
                if (_chiseled_polished_blackstone == null)
                    _chiseled_polished_blackstone = Image.Load((byte[])ResourceManager.GetObject("chiseled_polished_blackstone"));
                return _chiseled_polished_blackstone;
            }
        }

        private static Image<Rgba32> _chiseled_quartz_block = null;
        public static Image<Rgba32> chiseled_quartz_block {
            get {
                if (_chiseled_quartz_block == null)
                    _chiseled_quartz_block = Image.Load((byte[])ResourceManager.GetObject("chiseled_quartz_block"));
                return _chiseled_quartz_block;
            }
        }

        private static Image<Rgba32> _chiseled_quartz_block_top = null;
        public static Image<Rgba32> chiseled_quartz_block_top {
            get {
                if (_chiseled_quartz_block_top == null)
                    _chiseled_quartz_block_top = Image.Load((byte[])ResourceManager.GetObject("chiseled_quartz_block_top"));
                return _chiseled_quartz_block_top;
            }
        }

        private static Image<Rgba32> _clay = null;
        public static Image<Rgba32> clay {
            get {
                if (_clay == null)
                    _clay = Image.Load((byte[])ResourceManager.GetObject("clay"));
                return _clay;
            }
        }

        private static Image<Rgba32> _coal_block = null;
        public static Image<Rgba32> coal_block {
            get {
                if (_coal_block == null)
                    _coal_block = Image.Load((byte[])ResourceManager.GetObject("coal_block"));
                return _coal_block;
            }
        }

        private static Image<Rgba32> _coal_ore = null;
        public static Image<Rgba32> coal_ore {
            get {
                if (_coal_ore == null)
                    _coal_ore = Image.Load((byte[])ResourceManager.GetObject("coal_ore"));
                return _coal_ore;
            }
        }

        private static Image<Rgba32> _coarse_dirt = null;
        public static Image<Rgba32> coarse_dirt {
            get {
                if (_coarse_dirt == null)
                    _coarse_dirt = Image.Load((byte[])ResourceManager.GetObject("coarse_dirt"));
                return _coarse_dirt;
            }
        }

        private static Image<Rgba32> _cobbled_deepslate = null;
        public static Image<Rgba32> cobbled_deepslate {
            get {
                if (_cobbled_deepslate == null)
                    _cobbled_deepslate = Image.Load((byte[])ResourceManager.GetObject("cobbled_deepslate"));
                return _cobbled_deepslate;
            }
        }

        private static Image<Rgba32> _cobblestone = null;
        public static Image<Rgba32> cobblestone {
            get {
                if (_cobblestone == null)
                    _cobblestone = Image.Load((byte[])ResourceManager.GetObject("cobblestone"));
                return _cobblestone;
            }
        }

        private static Image<Rgba32> _copper_block = null;
        public static Image<Rgba32> copper_block {
            get {
                if (_copper_block == null)
                    _copper_block = Image.Load((byte[])ResourceManager.GetObject("copper_block"));
                return _copper_block;
            }
        }

        private static Image<Rgba32> _copper_ore = null;
        public static Image<Rgba32> copper_ore {
            get {
                if (_copper_ore == null)
                    _copper_ore = Image.Load((byte[])ResourceManager.GetObject("copper_ore"));
                return _copper_ore;
            }
        }

        private static Image<Rgba32> _cracked_deepslate_bricks = null;
        public static Image<Rgba32> cracked_deepslate_bricks {
            get {
                if (_cracked_deepslate_bricks == null)
                    _cracked_deepslate_bricks = Image.Load((byte[])ResourceManager.GetObject("cracked_deepslate_bricks"));
                return _cracked_deepslate_bricks;
            }
        }

        private static Image<Rgba32> _cracked_deepslate_tiles = null;
        public static Image<Rgba32> cracked_deepslate_tiles {
            get {
                if (_cracked_deepslate_tiles == null)
                    _cracked_deepslate_tiles = Image.Load((byte[])ResourceManager.GetObject("cracked_deepslate_tiles"));
                return _cracked_deepslate_tiles;
            }
        }

        private static Image<Rgba32> _cracked_nether_bricks = null;
        public static Image<Rgba32> cracked_nether_bricks {
            get {
                if (_cracked_nether_bricks == null)
                    _cracked_nether_bricks = Image.Load((byte[])ResourceManager.GetObject("cracked_nether_bricks"));
                return _cracked_nether_bricks;
            }
        }

        private static Image<Rgba32> _cracked_polished_blackstone_bricks = null;
        public static Image<Rgba32> cracked_polished_blackstone_bricks {
            get {
                if (_cracked_polished_blackstone_bricks == null)
                    _cracked_polished_blackstone_bricks = Image.Load((byte[])ResourceManager.GetObject("cracked_polished_blackstone_bricks"));
                return _cracked_polished_blackstone_bricks;
            }
        }

        private static Image<Rgba32> _cracked_stone_bricks = null;
        public static Image<Rgba32> cracked_stone_bricks {
            get {
                if (_cracked_stone_bricks == null)
                    _cracked_stone_bricks = Image.Load((byte[])ResourceManager.GetObject("cracked_stone_bricks"));
                return _cracked_stone_bricks;
            }
        }

        private static Image<Rgba32> _crimson_nylium = null;
        public static Image<Rgba32> crimson_nylium {
            get {
                if (_crimson_nylium == null)
                    _crimson_nylium = Image.Load((byte[])ResourceManager.GetObject("crimson_nylium"));
                return _crimson_nylium;
            }
        }

        private static Image<Rgba32> _crimson_nylium_side = null;
        public static Image<Rgba32> crimson_nylium_side {
            get {
                if (_crimson_nylium_side == null)
                    _crimson_nylium_side = Image.Load((byte[])ResourceManager.GetObject("crimson_nylium_side"));
                return _crimson_nylium_side;
            }
        }

        private static Image<Rgba32> _crimson_planks = null;
        public static Image<Rgba32> crimson_planks {
            get {
                if (_crimson_planks == null)
                    _crimson_planks = Image.Load((byte[])ResourceManager.GetObject("crimson_planks"));
                return _crimson_planks;
            }
        }

        private static Image<Rgba32> _crimson_stem = null;
        public static Image<Rgba32> crimson_stem {
            get {
                if (_crimson_stem == null)
                    _crimson_stem = Image.Load((byte[])ResourceManager.GetObject("crimson_stem"));
                return _crimson_stem;
            }
        }

        private static Image<Rgba32> _crying_obsidian = null;
        public static Image<Rgba32> crying_obsidian {
            get {
                if (_crying_obsidian == null)
                    _crying_obsidian = Image.Load((byte[])ResourceManager.GetObject("crying_obsidian"));
                return _crying_obsidian;
            }
        }

        private static Image<Rgba32> _cut_copper = null;
        public static Image<Rgba32> cut_copper {
            get {
                if (_cut_copper == null)
                    _cut_copper = Image.Load((byte[])ResourceManager.GetObject("cut_copper"));
                return _cut_copper;
            }
        }

        private static Image<Rgba32> _cyan_concrete = null;
        public static Image<Rgba32> cyan_concrete {
            get {
                if (_cyan_concrete == null)
                    _cyan_concrete = Image.Load((byte[])ResourceManager.GetObject("cyan_concrete"));
                return _cyan_concrete;
            }
        }

        private static Image<Rgba32> _cyan_concrete_powder = null;
        public static Image<Rgba32> cyan_concrete_powder {
            get {
                if (_cyan_concrete_powder == null)
                    _cyan_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("cyan_concrete_powder"));
                return _cyan_concrete_powder;
            }
        }

        private static Image<Rgba32> _cyan_glazed_terracotta = null;
        public static Image<Rgba32> cyan_glazed_terracotta {
            get {
                if (_cyan_glazed_terracotta == null)
                    _cyan_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("cyan_glazed_terracotta"));
                return _cyan_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _cyan_stained_glass = null;
        public static Image<Rgba32> cyan_stained_glass {
            get {
                if (_cyan_stained_glass == null)
                    _cyan_stained_glass = Image.Load((byte[])ResourceManager.GetObject("cyan_stained_glass"));
                return _cyan_stained_glass;
            }
        }

        private static Image<Rgba32> _cyan_terracotta = null;
        public static Image<Rgba32> cyan_terracotta {
            get {
                if (_cyan_terracotta == null)
                    _cyan_terracotta = Image.Load((byte[])ResourceManager.GetObject("cyan_terracotta"));
                return _cyan_terracotta;
            }
        }

        private static Image<Rgba32> _cyan_wool = null;
        public static Image<Rgba32> cyan_wool {
            get {
                if (_cyan_wool == null)
                    _cyan_wool = Image.Load((byte[])ResourceManager.GetObject("cyan_wool"));
                return _cyan_wool;
            }
        }

        private static Image<Rgba32> _dark_oak_log = null;
        public static Image<Rgba32> dark_oak_log {
            get {
                if (_dark_oak_log == null)
                    _dark_oak_log = Image.Load((byte[])ResourceManager.GetObject("dark_oak_log"));
                return _dark_oak_log;
            }
        }

        private static Image<Rgba32> _dark_oak_planks = null;
        public static Image<Rgba32> dark_oak_planks {
            get {
                if (_dark_oak_planks == null)
                    _dark_oak_planks = Image.Load((byte[])ResourceManager.GetObject("dark_oak_planks"));
                return _dark_oak_planks;
            }
        }

        private static Image<Rgba32> _dark_prismarine = null;
        public static Image<Rgba32> dark_prismarine {
            get {
                if (_dark_prismarine == null)
                    _dark_prismarine = Image.Load((byte[])ResourceManager.GetObject("dark_prismarine"));
                return _dark_prismarine;
            }
        }

        private static Image<Rgba32> _dead_brain_coral_block = null;
        public static Image<Rgba32> dead_brain_coral_block {
            get {
                if (_dead_brain_coral_block == null)
                    _dead_brain_coral_block = Image.Load((byte[])ResourceManager.GetObject("dead_brain_coral_block"));
                return _dead_brain_coral_block;
            }
        }

        private static Image<Rgba32> _dead_bubble_coral_block = null;
        public static Image<Rgba32> dead_bubble_coral_block {
            get {
                if (_dead_bubble_coral_block == null)
                    _dead_bubble_coral_block = Image.Load((byte[])ResourceManager.GetObject("dead_bubble_coral_block"));
                return _dead_bubble_coral_block;
            }
        }

        private static Image<Rgba32> _dead_fire_coral_block = null;
        public static Image<Rgba32> dead_fire_coral_block {
            get {
                if (_dead_fire_coral_block == null)
                    _dead_fire_coral_block = Image.Load((byte[])ResourceManager.GetObject("dead_fire_coral_block"));
                return _dead_fire_coral_block;
            }
        }

        private static Image<Rgba32> _dead_horn_coral_block = null;
        public static Image<Rgba32> dead_horn_coral_block {
            get {
                if (_dead_horn_coral_block == null)
                    _dead_horn_coral_block = Image.Load((byte[])ResourceManager.GetObject("dead_horn_coral_block"));
                return _dead_horn_coral_block;
            }
        }

        private static Image<Rgba32> _dead_tube_coral_block = null;
        public static Image<Rgba32> dead_tube_coral_block {
            get {
                if (_dead_tube_coral_block == null)
                    _dead_tube_coral_block = Image.Load((byte[])ResourceManager.GetObject("dead_tube_coral_block"));
                return _dead_tube_coral_block;
            }
        }

        private static Image<Rgba32> _deepslate = null;
        public static Image<Rgba32> deepslate {
            get {
                if (_deepslate == null)
                    _deepslate = Image.Load((byte[])ResourceManager.GetObject("deepslate"));
                return _deepslate;
            }
        }

        private static Image<Rgba32> _deepslate_bricks = null;
        public static Image<Rgba32> deepslate_bricks {
            get {
                if (_deepslate_bricks == null)
                    _deepslate_bricks = Image.Load((byte[])ResourceManager.GetObject("deepslate_bricks"));
                return _deepslate_bricks;
            }
        }

        private static Image<Rgba32> _deepslate_coal_ore = null;
        public static Image<Rgba32> deepslate_coal_ore {
            get {
                if (_deepslate_coal_ore == null)
                    _deepslate_coal_ore = Image.Load((byte[])ResourceManager.GetObject("deepslate_coal_ore"));
                return _deepslate_coal_ore;
            }
        }

        private static Image<Rgba32> _deepslate_copper_ore = null;
        public static Image<Rgba32> deepslate_copper_ore {
            get {
                if (_deepslate_copper_ore == null)
                    _deepslate_copper_ore = Image.Load((byte[])ResourceManager.GetObject("deepslate_copper_ore"));
                return _deepslate_copper_ore;
            }
        }

        private static Image<Rgba32> _deepslate_diamond_ore = null;
        public static Image<Rgba32> deepslate_diamond_ore {
            get {
                if (_deepslate_diamond_ore == null)
                    _deepslate_diamond_ore = Image.Load((byte[])ResourceManager.GetObject("deepslate_diamond_ore"));
                return _deepslate_diamond_ore;
            }
        }

        private static Image<Rgba32> _deepslate_emerald_ore = null;
        public static Image<Rgba32> deepslate_emerald_ore {
            get {
                if (_deepslate_emerald_ore == null)
                    _deepslate_emerald_ore = Image.Load((byte[])ResourceManager.GetObject("deepslate_emerald_ore"));
                return _deepslate_emerald_ore;
            }
        }

        private static Image<Rgba32> _deepslate_gold_ore = null;
        public static Image<Rgba32> deepslate_gold_ore {
            get {
                if (_deepslate_gold_ore == null)
                    _deepslate_gold_ore = Image.Load((byte[])ResourceManager.GetObject("deepslate_gold_ore"));
                return _deepslate_gold_ore;
            }
        }

        private static Image<Rgba32> _deepslate_iron_ore = null;
        public static Image<Rgba32> deepslate_iron_ore {
            get {
                if (_deepslate_iron_ore == null)
                    _deepslate_iron_ore = Image.Load((byte[])ResourceManager.GetObject("deepslate_iron_ore"));
                return _deepslate_iron_ore;
            }
        }

        private static Image<Rgba32> _deepslate_lapis_ore = null;
        public static Image<Rgba32> deepslate_lapis_ore {
            get {
                if (_deepslate_lapis_ore == null)
                    _deepslate_lapis_ore = Image.Load((byte[])ResourceManager.GetObject("deepslate_lapis_ore"));
                return _deepslate_lapis_ore;
            }
        }

        private static Image<Rgba32> _deepslate_redstone_ore = null;
        public static Image<Rgba32> deepslate_redstone_ore {
            get {
                if (_deepslate_redstone_ore == null)
                    _deepslate_redstone_ore = Image.Load((byte[])ResourceManager.GetObject("deepslate_redstone_ore"));
                return _deepslate_redstone_ore;
            }
        }

        private static Image<Rgba32> _deepslate_tiles = null;
        public static Image<Rgba32> deepslate_tiles {
            get {
                if (_deepslate_tiles == null)
                    _deepslate_tiles = Image.Load((byte[])ResourceManager.GetObject("deepslate_tiles"));
                return _deepslate_tiles;
            }
        }

        private static Image<Rgba32> _deepslate_top = null;
        public static Image<Rgba32> deepslate_top {
            get {
                if (_deepslate_top == null)
                    _deepslate_top = Image.Load((byte[])ResourceManager.GetObject("deepslate_top"));
                return _deepslate_top;
            }
        }

        private static Image<Rgba32> _diamond_block = null;
        public static Image<Rgba32> diamond_block {
            get {
                if (_diamond_block == null)
                    _diamond_block = Image.Load((byte[])ResourceManager.GetObject("diamond_block"));
                return _diamond_block;
            }
        }

        private static Image<Rgba32> _diamond_ore = null;
        public static Image<Rgba32> diamond_ore {
            get {
                if (_diamond_ore == null)
                    _diamond_ore = Image.Load((byte[])ResourceManager.GetObject("diamond_ore"));
                return _diamond_ore;
            }
        }

        private static Image<Rgba32> _diorite = null;
        public static Image<Rgba32> diorite {
            get {
                if (_diorite == null)
                    _diorite = Image.Load((byte[])ResourceManager.GetObject("diorite"));
                return _diorite;
            }
        }

        private static Image<Rgba32> _dirt = null;
        public static Image<Rgba32> dirt {
            get {
                if (_dirt == null)
                    _dirt = Image.Load((byte[])ResourceManager.GetObject("dirt"));
                return _dirt;
            }
        }

        private static Image<Rgba32> _dirt_path_side = null;
        public static Image<Rgba32> dirt_path_side {
            get {
                if (_dirt_path_side == null)
                    _dirt_path_side = Image.Load((byte[])ResourceManager.GetObject("dirt_path_side"));
                return _dirt_path_side;
            }
        }

        private static Image<Rgba32> _dirt_path_top = null;
        public static Image<Rgba32> dirt_path_top {
            get {
                if (_dirt_path_top == null)
                    _dirt_path_top = Image.Load((byte[])ResourceManager.GetObject("dirt_path_top"));
                return _dirt_path_top;
            }
        }

        private static Image<Rgba32> _disabled = null;
        public static Image<Rgba32> disabled {
            get {
                if (_disabled == null)
                    _disabled = Image.Load((byte[])ResourceManager.GetObject("disabled"));
                return _disabled;
            }
        }

        private static Image<Rgba32> _dried_kelp_side = null;
        public static Image<Rgba32> dried_kelp_side {
            get {
                if (_dried_kelp_side == null)
                    _dried_kelp_side = Image.Load((byte[])ResourceManager.GetObject("dried_kelp_side"));
                return _dried_kelp_side;
            }
        }

        private static Image<Rgba32> _dried_kelp_top = null;
        public static Image<Rgba32> dried_kelp_top {
            get {
                if (_dried_kelp_top == null)
                    _dried_kelp_top = Image.Load((byte[])ResourceManager.GetObject("dried_kelp_top"));
                return _dried_kelp_top;
            }
        }

        private static Image<Rgba32> _dripstone_block = null;
        public static Image<Rgba32> dripstone_block {
            get {
                if (_dripstone_block == null)
                    _dripstone_block = Image.Load((byte[])ResourceManager.GetObject("dripstone_block"));
                return _dripstone_block;
            }
        }

        private static Image<Rgba32> _emerald_block = null;
        public static Image<Rgba32> emerald_block {
            get {
                if (_emerald_block == null)
                    _emerald_block = Image.Load((byte[])ResourceManager.GetObject("emerald_block"));
                return _emerald_block;
            }
        }

        private static Image<Rgba32> _emerald_ore = null;
        public static Image<Rgba32> emerald_ore {
            get {
                if (_emerald_ore == null)
                    _emerald_ore = Image.Load((byte[])ResourceManager.GetObject("emerald_ore"));
                return _emerald_ore;
            }
        }

        private static Image<Rgba32> _end_stone = null;
        public static Image<Rgba32> end_stone {
            get {
                if (_end_stone == null)
                    _end_stone = Image.Load((byte[])ResourceManager.GetObject("end_stone"));
                return _end_stone;
            }
        }

        private static Image<Rgba32> _end_stone_bricks = null;
        public static Image<Rgba32> end_stone_bricks {
            get {
                if (_end_stone_bricks == null)
                    _end_stone_bricks = Image.Load((byte[])ResourceManager.GetObject("end_stone_bricks"));
                return _end_stone_bricks;
            }
        }

        private static Image<Rgba32> _exposed_copper = null;
        public static Image<Rgba32> exposed_copper {
            get {
                if (_exposed_copper == null)
                    _exposed_copper = Image.Load((byte[])ResourceManager.GetObject("exposed_copper"));
                return _exposed_copper;
            }
        }

        private static Image<Rgba32> _exposed_cut_copper = null;
        public static Image<Rgba32> exposed_cut_copper {
            get {
                if (_exposed_cut_copper == null)
                    _exposed_cut_copper = Image.Load((byte[])ResourceManager.GetObject("exposed_cut_copper"));
                return _exposed_cut_copper;
            }
        }

        private static Image<Rgba32> _fire_coral_block = null;
        public static Image<Rgba32> fire_coral_block {
            get {
                if (_fire_coral_block == null)
                    _fire_coral_block = Image.Load((byte[])ResourceManager.GetObject("fire_coral_block"));
                return _fire_coral_block;
            }
        }

        private static Image<Rgba32> _gilded_blackstone = null;
        public static Image<Rgba32> gilded_blackstone {
            get {
                if (_gilded_blackstone == null)
                    _gilded_blackstone = Image.Load((byte[])ResourceManager.GetObject("gilded_blackstone"));
                return _gilded_blackstone;
            }
        }

        private static Image<Rgba32> _glass = null;
        public static Image<Rgba32> glass {
            get {
                if (_glass == null)
                    _glass = Image.Load((byte[])ResourceManager.GetObject("glass"));
                return _glass;
            }
        }

        private static Image<Rgba32> _glowstone = null;
        public static Image<Rgba32> glowstone {
            get {
                if (_glowstone == null)
                    _glowstone = Image.Load((byte[])ResourceManager.GetObject("glowstone"));
                return _glowstone;
            }
        }

        private static Image<Rgba32> _gold_block = null;
        public static Image<Rgba32> gold_block {
            get {
                if (_gold_block == null)
                    _gold_block = Image.Load((byte[])ResourceManager.GetObject("gold_block"));
                return _gold_block;
            }
        }

        private static Image<Rgba32> _gold_ore = null;
        public static Image<Rgba32> gold_ore {
            get {
                if (_gold_ore == null)
                    _gold_ore = Image.Load((byte[])ResourceManager.GetObject("gold_ore"));
                return _gold_ore;
            }
        }

        private static Image<Rgba32> _granite = null;
        public static Image<Rgba32> granite {
            get {
                if (_granite == null)
                    _granite = Image.Load((byte[])ResourceManager.GetObject("granite"));
                return _granite;
            }
        }

        private static Image<Rgba32> _gravel = null;
        public static Image<Rgba32> gravel {
            get {
                if (_gravel == null)
                    _gravel = Image.Load((byte[])ResourceManager.GetObject("gravel"));
                return _gravel;
            }
        }

        private static Image<Rgba32> _gray_concrete = null;
        public static Image<Rgba32> gray_concrete {
            get {
                if (_gray_concrete == null)
                    _gray_concrete = Image.Load((byte[])ResourceManager.GetObject("gray_concrete"));
                return _gray_concrete;
            }
        }

        private static Image<Rgba32> _gray_concrete_powder = null;
        public static Image<Rgba32> gray_concrete_powder {
            get {
                if (_gray_concrete_powder == null)
                    _gray_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("gray_concrete_powder"));
                return _gray_concrete_powder;
            }
        }

        private static Image<Rgba32> _gray_glazed_terracotta = null;
        public static Image<Rgba32> gray_glazed_terracotta {
            get {
                if (_gray_glazed_terracotta == null)
                    _gray_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("gray_glazed_terracotta"));
                return _gray_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _gray_stained_glass = null;
        public static Image<Rgba32> gray_stained_glass {
            get {
                if (_gray_stained_glass == null)
                    _gray_stained_glass = Image.Load((byte[])ResourceManager.GetObject("gray_stained_glass"));
                return _gray_stained_glass;
            }
        }

        private static Image<Rgba32> _gray_terracotta = null;
        public static Image<Rgba32> gray_terracotta {
            get {
                if (_gray_terracotta == null)
                    _gray_terracotta = Image.Load((byte[])ResourceManager.GetObject("gray_terracotta"));
                return _gray_terracotta;
            }
        }

        private static Image<Rgba32> _gray_wool = null;
        public static Image<Rgba32> gray_wool {
            get {
                if (_gray_wool == null)
                    _gray_wool = Image.Load((byte[])ResourceManager.GetObject("gray_wool"));
                return _gray_wool;
            }
        }

        private static Image<Rgba32> _green_concrete = null;
        public static Image<Rgba32> green_concrete {
            get {
                if (_green_concrete == null)
                    _green_concrete = Image.Load((byte[])ResourceManager.GetObject("green_concrete"));
                return _green_concrete;
            }
        }

        private static Image<Rgba32> _green_concrete_powder = null;
        public static Image<Rgba32> green_concrete_powder {
            get {
                if (_green_concrete_powder == null)
                    _green_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("green_concrete_powder"));
                return _green_concrete_powder;
            }
        }

        private static Image<Rgba32> _green_glazed_terracotta = null;
        public static Image<Rgba32> green_glazed_terracotta {
            get {
                if (_green_glazed_terracotta == null)
                    _green_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("green_glazed_terracotta"));
                return _green_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _green_stained_glass = null;
        public static Image<Rgba32> green_stained_glass {
            get {
                if (_green_stained_glass == null)
                    _green_stained_glass = Image.Load((byte[])ResourceManager.GetObject("green_stained_glass"));
                return _green_stained_glass;
            }
        }

        private static Image<Rgba32> _green_terracotta = null;
        public static Image<Rgba32> green_terracotta {
            get {
                if (_green_terracotta == null)
                    _green_terracotta = Image.Load((byte[])ResourceManager.GetObject("green_terracotta"));
                return _green_terracotta;
            }
        }

        private static Image<Rgba32> _green_wool = null;
        public static Image<Rgba32> green_wool {
            get {
                if (_green_wool == null)
                    _green_wool = Image.Load((byte[])ResourceManager.GetObject("green_wool"));
                return _green_wool;
            }
        }

        private static Image<Rgba32> _hay_block_side = null;
        public static Image<Rgba32> hay_block_side {
            get {
                if (_hay_block_side == null)
                    _hay_block_side = Image.Load((byte[])ResourceManager.GetObject("hay_block_side"));
                return _hay_block_side;
            }
        }

        private static Image<Rgba32> _hay_block_top = null;
        public static Image<Rgba32> hay_block_top {
            get {
                if (_hay_block_top == null)
                    _hay_block_top = Image.Load((byte[])ResourceManager.GetObject("hay_block_top"));
                return _hay_block_top;
            }
        }

        private static Image<Rgba32> _honeycomb_block = null;
        public static Image<Rgba32> honeycomb_block {
            get {
                if (_honeycomb_block == null)
                    _honeycomb_block = Image.Load((byte[])ResourceManager.GetObject("honeycomb_block"));
                return _honeycomb_block;
            }
        }

        private static Image<Rgba32> _honey_block_side = null;
        public static Image<Rgba32> honey_block_side {
            get {
                if (_honey_block_side == null)
                    _honey_block_side = Image.Load((byte[])ResourceManager.GetObject("honey_block_side"));
                return _honey_block_side;
            }
        }

        private static Image<Rgba32> _honey_block_top = null;
        public static Image<Rgba32> honey_block_top {
            get {
                if (_honey_block_top == null)
                    _honey_block_top = Image.Load((byte[])ResourceManager.GetObject("honey_block_top"));
                return _honey_block_top;
            }
        }

        private static Image<Rgba32> _horn_coral_block = null;
        public static Image<Rgba32> horn_coral_block {
            get {
                if (_horn_coral_block == null)
                    _horn_coral_block = Image.Load((byte[])ResourceManager.GetObject("horn_coral_block"));
                return _horn_coral_block;
            }
        }

        private static Image<Rgba32> _iron_block = null;
        public static Image<Rgba32> iron_block {
            get {
                if (_iron_block == null)
                    _iron_block = Image.Load((byte[])ResourceManager.GetObject("iron_block"));
                return _iron_block;
            }
        }

        private static Image<Rgba32> _iron_ore = null;
        public static Image<Rgba32> iron_ore {
            get {
                if (_iron_ore == null)
                    _iron_ore = Image.Load((byte[])ResourceManager.GetObject("iron_ore"));
                return _iron_ore;
            }
        }

        private static Image<Rgba32> _jack_o_lantern = null;
        public static Image<Rgba32> jack_o_lantern {
            get {
                if (_jack_o_lantern == null)
                    _jack_o_lantern = Image.Load((byte[])ResourceManager.GetObject("jack_o_lantern"));
                return _jack_o_lantern;
            }
        }

        private static Image<Rgba32> _jungle_log = null;
        public static Image<Rgba32> jungle_log {
            get {
                if (_jungle_log == null)
                    _jungle_log = Image.Load((byte[])ResourceManager.GetObject("jungle_log"));
                return _jungle_log;
            }
        }

        private static Image<Rgba32> _jungle_planks = null;
        public static Image<Rgba32> jungle_planks {
            get {
                if (_jungle_planks == null)
                    _jungle_planks = Image.Load((byte[])ResourceManager.GetObject("jungle_planks"));
                return _jungle_planks;
            }
        }

        private static Image<Rgba32> _lapis_block = null;
        public static Image<Rgba32> lapis_block {
            get {
                if (_lapis_block == null)
                    _lapis_block = Image.Load((byte[])ResourceManager.GetObject("lapis_block"));
                return _lapis_block;
            }
        }

        private static Image<Rgba32> _lapis_ore = null;
        public static Image<Rgba32> lapis_ore {
            get {
                if (_lapis_ore == null)
                    _lapis_ore = Image.Load((byte[])ResourceManager.GetObject("lapis_ore"));
                return _lapis_ore;
            }
        }

        private static Image<Rgba32> _light_blue_concrete = null;
        public static Image<Rgba32> light_blue_concrete {
            get {
                if (_light_blue_concrete == null)
                    _light_blue_concrete = Image.Load((byte[])ResourceManager.GetObject("light_blue_concrete"));
                return _light_blue_concrete;
            }
        }

        private static Image<Rgba32> _light_blue_concrete_powder = null;
        public static Image<Rgba32> light_blue_concrete_powder {
            get {
                if (_light_blue_concrete_powder == null)
                    _light_blue_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("light_blue_concrete_powder"));
                return _light_blue_concrete_powder;
            }
        }

        private static Image<Rgba32> _light_blue_glazed_terracotta = null;
        public static Image<Rgba32> light_blue_glazed_terracotta {
            get {
                if (_light_blue_glazed_terracotta == null)
                    _light_blue_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("light_blue_glazed_terracotta"));
                return _light_blue_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _light_blue_stained_glass = null;
        public static Image<Rgba32> light_blue_stained_glass {
            get {
                if (_light_blue_stained_glass == null)
                    _light_blue_stained_glass = Image.Load((byte[])ResourceManager.GetObject("light_blue_stained_glass"));
                return _light_blue_stained_glass;
            }
        }

        private static Image<Rgba32> _light_blue_terracotta = null;
        public static Image<Rgba32> light_blue_terracotta {
            get {
                if (_light_blue_terracotta == null)
                    _light_blue_terracotta = Image.Load((byte[])ResourceManager.GetObject("light_blue_terracotta"));
                return _light_blue_terracotta;
            }
        }

        private static Image<Rgba32> _light_blue_wool = null;
        public static Image<Rgba32> light_blue_wool {
            get {
                if (_light_blue_wool == null)
                    _light_blue_wool = Image.Load((byte[])ResourceManager.GetObject("light_blue_wool"));
                return _light_blue_wool;
            }
        }

        private static Image<Rgba32> _light_gray_concrete = null;
        public static Image<Rgba32> light_gray_concrete {
            get {
                if (_light_gray_concrete == null)
                    _light_gray_concrete = Image.Load((byte[])ResourceManager.GetObject("light_gray_concrete"));
                return _light_gray_concrete;
            }
        }

        private static Image<Rgba32> _light_gray_concrete_powder = null;
        public static Image<Rgba32> light_gray_concrete_powder {
            get {
                if (_light_gray_concrete_powder == null)
                    _light_gray_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("light_gray_concrete_powder"));
                return _light_gray_concrete_powder;
            }
        }

        private static Image<Rgba32> _light_gray_glazed_terracotta = null;
        public static Image<Rgba32> light_gray_glazed_terracotta {
            get {
                if (_light_gray_glazed_terracotta == null)
                    _light_gray_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("light_gray_glazed_terracotta"));
                return _light_gray_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _light_gray_stained_glass = null;
        public static Image<Rgba32> light_gray_stained_glass {
            get {
                if (_light_gray_stained_glass == null)
                    _light_gray_stained_glass = Image.Load((byte[])ResourceManager.GetObject("light_gray_stained_glass"));
                return _light_gray_stained_glass;
            }
        }

        private static Image<Rgba32> _light_gray_terracotta = null;
        public static Image<Rgba32> light_gray_terracotta {
            get {
                if (_light_gray_terracotta == null)
                    _light_gray_terracotta = Image.Load((byte[])ResourceManager.GetObject("light_gray_terracotta"));
                return _light_gray_terracotta;
            }
        }

        private static Image<Rgba32> _light_gray_wool = null;
        public static Image<Rgba32> light_gray_wool {
            get {
                if (_light_gray_wool == null)
                    _light_gray_wool = Image.Load((byte[])ResourceManager.GetObject("light_gray_wool"));
                return _light_gray_wool;
            }
        }

        private static Image<Rgba32> _lime_concrete = null;
        public static Image<Rgba32> lime_concrete {
            get {
                if (_lime_concrete == null)
                    _lime_concrete = Image.Load((byte[])ResourceManager.GetObject("lime_concrete"));
                return _lime_concrete;
            }
        }

        private static Image<Rgba32> _lime_concrete_powder = null;
        public static Image<Rgba32> lime_concrete_powder {
            get {
                if (_lime_concrete_powder == null)
                    _lime_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("lime_concrete_powder"));
                return _lime_concrete_powder;
            }
        }

        private static Image<Rgba32> _lime_glazed_terracotta = null;
        public static Image<Rgba32> lime_glazed_terracotta {
            get {
                if (_lime_glazed_terracotta == null)
                    _lime_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("lime_glazed_terracotta"));
                return _lime_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _lime_stained_glass = null;
        public static Image<Rgba32> lime_stained_glass {
            get {
                if (_lime_stained_glass == null)
                    _lime_stained_glass = Image.Load((byte[])ResourceManager.GetObject("lime_stained_glass"));
                return _lime_stained_glass;
            }
        }

        private static Image<Rgba32> _lime_terracotta = null;
        public static Image<Rgba32> lime_terracotta {
            get {
                if (_lime_terracotta == null)
                    _lime_terracotta = Image.Load((byte[])ResourceManager.GetObject("lime_terracotta"));
                return _lime_terracotta;
            }
        }

        private static Image<Rgba32> _lime_wool = null;
        public static Image<Rgba32> lime_wool {
            get {
                if (_lime_wool == null)
                    _lime_wool = Image.Load((byte[])ResourceManager.GetObject("lime_wool"));
                return _lime_wool;
            }
        }

        private static Image<Rgba32> _lodestone_side = null;
        public static Image<Rgba32> lodestone_side {
            get {
                if (_lodestone_side == null)
                    _lodestone_side = Image.Load((byte[])ResourceManager.GetObject("lodestone_side"));
                return _lodestone_side;
            }
        }

        private static Image<Rgba32> _lodestone_top = null;
        public static Image<Rgba32> lodestone_top {
            get {
                if (_lodestone_top == null)
                    _lodestone_top = Image.Load((byte[])ResourceManager.GetObject("lodestone_top"));
                return _lodestone_top;
            }
        }

        private static Image<Rgba32> _magenta_concrete = null;
        public static Image<Rgba32> magenta_concrete {
            get {
                if (_magenta_concrete == null)
                    _magenta_concrete = Image.Load((byte[])ResourceManager.GetObject("magenta_concrete"));
                return _magenta_concrete;
            }
        }

        private static Image<Rgba32> _magenta_concrete_powder = null;
        public static Image<Rgba32> magenta_concrete_powder {
            get {
                if (_magenta_concrete_powder == null)
                    _magenta_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("magenta_concrete_powder"));
                return _magenta_concrete_powder;
            }
        }

        private static Image<Rgba32> _magenta_glazed_terracotta = null;
        public static Image<Rgba32> magenta_glazed_terracotta {
            get {
                if (_magenta_glazed_terracotta == null)
                    _magenta_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("magenta_glazed_terracotta"));
                return _magenta_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _magenta_stained_glass = null;
        public static Image<Rgba32> magenta_stained_glass {
            get {
                if (_magenta_stained_glass == null)
                    _magenta_stained_glass = Image.Load((byte[])ResourceManager.GetObject("magenta_stained_glass"));
                return _magenta_stained_glass;
            }
        }

        private static Image<Rgba32> _magenta_terracotta = null;
        public static Image<Rgba32> magenta_terracotta {
            get {
                if (_magenta_terracotta == null)
                    _magenta_terracotta = Image.Load((byte[])ResourceManager.GetObject("magenta_terracotta"));
                return _magenta_terracotta;
            }
        }

        private static Image<Rgba32> _magenta_wool = null;
        public static Image<Rgba32> magenta_wool {
            get {
                if (_magenta_wool == null)
                    _magenta_wool = Image.Load((byte[])ResourceManager.GetObject("magenta_wool"));
                return _magenta_wool;
            }
        }

        private static Image<Rgba32> _magma = null;
        public static Image<Rgba32> magma {
            get {
                if (_magma == null)
                    _magma = Image.Load((byte[])ResourceManager.GetObject("magma"));
                return _magma;
            }
        }

        private static Image<Rgba32> _mangrove_log = null;
        public static Image<Rgba32> mangrove_log {
            get {
                if (_mangrove_log == null)
                    _mangrove_log = Image.Load((byte[])ResourceManager.GetObject("mangrove_log"));
                return _mangrove_log;
            }
        }

        private static Image<Rgba32> _mangrove_planks = null;
        public static Image<Rgba32> mangrove_planks {
            get {
                if (_mangrove_planks == null)
                    _mangrove_planks = Image.Load((byte[])ResourceManager.GetObject("mangrove_planks"));
                return _mangrove_planks;
            }
        }

        private static Image<Rgba32> _melon_side = null;
        public static Image<Rgba32> melon_side {
            get {
                if (_melon_side == null)
                    _melon_side = Image.Load((byte[])ResourceManager.GetObject("melon_side"));
                return _melon_side;
            }
        }

        private static Image<Rgba32> _melon_top = null;
        public static Image<Rgba32> melon_top {
            get {
                if (_melon_top == null)
                    _melon_top = Image.Load((byte[])ResourceManager.GetObject("melon_top"));
                return _melon_top;
            }
        }

        private static Image<Rgba32> _mossy_cobblestone = null;
        public static Image<Rgba32> mossy_cobblestone {
            get {
                if (_mossy_cobblestone == null)
                    _mossy_cobblestone = Image.Load((byte[])ResourceManager.GetObject("mossy_cobblestone"));
                return _mossy_cobblestone;
            }
        }

        private static Image<Rgba32> _mossy_stone_bricks = null;
        public static Image<Rgba32> mossy_stone_bricks {
            get {
                if (_mossy_stone_bricks == null)
                    _mossy_stone_bricks = Image.Load((byte[])ResourceManager.GetObject("mossy_stone_bricks"));
                return _mossy_stone_bricks;
            }
        }

        private static Image<Rgba32> _moss_block = null;
        public static Image<Rgba32> moss_block {
            get {
                if (_moss_block == null)
                    _moss_block = Image.Load((byte[])ResourceManager.GetObject("moss_block"));
                return _moss_block;
            }
        }

        private static Image<Rgba32> _mud = null;
        public static Image<Rgba32> mud {
            get {
                if (_mud == null)
                    _mud = Image.Load((byte[])ResourceManager.GetObject("mud"));
                return _mud;
            }
        }

        private static Image<Rgba32> _muddy_mangrove_roots_top = null;
        public static Image<Rgba32> muddy_mangrove_roots_top {
            get {
                if (_muddy_mangrove_roots_top == null)
                    _muddy_mangrove_roots_top = Image.Load((byte[])ResourceManager.GetObject("muddy_mangrove_roots_top"));
                return _muddy_mangrove_roots_top;
            }
        }

        private static Image<Rgba32> _mud_bricks = null;
        public static Image<Rgba32> mud_bricks {
            get {
                if (_mud_bricks == null)
                    _mud_bricks = Image.Load((byte[])ResourceManager.GetObject("mud_bricks"));
                return _mud_bricks;
            }
        }

        private static Image<Rgba32> _mushroom_block_inside = null;
        public static Image<Rgba32> mushroom_block_inside {
            get {
                if (_mushroom_block_inside == null)
                    _mushroom_block_inside = Image.Load((byte[])ResourceManager.GetObject("mushroom_block_inside"));
                return _mushroom_block_inside;
            }
        }

        private static Image<Rgba32> _mushroom_stem = null;
        public static Image<Rgba32> mushroom_stem {
            get {
                if (_mushroom_stem == null)
                    _mushroom_stem = Image.Load((byte[])ResourceManager.GetObject("mushroom_stem"));
                return _mushroom_stem;
            }
        }

        private static Image<Rgba32> _netherite_block = null;
        public static Image<Rgba32> netherite_block {
            get {
                if (_netherite_block == null)
                    _netherite_block = Image.Load((byte[])ResourceManager.GetObject("netherite_block"));
                return _netherite_block;
            }
        }

        private static Image<Rgba32> _netherrack = null;
        public static Image<Rgba32> netherrack {
            get {
                if (_netherrack == null)
                    _netherrack = Image.Load((byte[])ResourceManager.GetObject("netherrack"));
                return _netherrack;
            }
        }

        private static Image<Rgba32> _nether_bricks = null;
        public static Image<Rgba32> nether_bricks {
            get {
                if (_nether_bricks == null)
                    _nether_bricks = Image.Load((byte[])ResourceManager.GetObject("nether_bricks"));
                return _nether_bricks;
            }
        }

        private static Image<Rgba32> _nether_gold_ore = null;
        public static Image<Rgba32> nether_gold_ore {
            get {
                if (_nether_gold_ore == null)
                    _nether_gold_ore = Image.Load((byte[])ResourceManager.GetObject("nether_gold_ore"));
                return _nether_gold_ore;
            }
        }

        private static Image<Rgba32> _nether_quartz_ore = null;
        public static Image<Rgba32> nether_quartz_ore {
            get {
                if (_nether_quartz_ore == null)
                    _nether_quartz_ore = Image.Load((byte[])ResourceManager.GetObject("nether_quartz_ore"));
                return _nether_quartz_ore;
            }
        }

        private static Image<Rgba32> _nether_wart_block = null;
        public static Image<Rgba32> nether_wart_block {
            get {
                if (_nether_wart_block == null)
                    _nether_wart_block = Image.Load((byte[])ResourceManager.GetObject("nether_wart_block"));
                return _nether_wart_block;
            }
        }

        private static Image<Rgba32> _note_block = null;
        public static Image<Rgba32> note_block {
            get {
                if (_note_block == null)
                    _note_block = Image.Load((byte[])ResourceManager.GetObject("note_block"));
                return _note_block;
            }
        }

        private static Image<Rgba32> _oak_log = null;
        public static Image<Rgba32> oak_log {
            get {
                if (_oak_log == null)
                    _oak_log = Image.Load((byte[])ResourceManager.GetObject("oak_log"));
                return _oak_log;
            }
        }

        private static Image<Rgba32> _oak_planks = null;
        public static Image<Rgba32> oak_planks {
            get {
                if (_oak_planks == null)
                    _oak_planks = Image.Load((byte[])ResourceManager.GetObject("oak_planks"));
                return _oak_planks;
            }
        }

        private static Image<Rgba32> _obsidian = null;
        public static Image<Rgba32> obsidian {
            get {
                if (_obsidian == null)
                    _obsidian = Image.Load((byte[])ResourceManager.GetObject("obsidian"));
                return _obsidian;
            }
        }

        private static Image<Rgba32> _ochre_froglight_side = null;
        public static Image<Rgba32> ochre_froglight_side {
            get {
                if (_ochre_froglight_side == null)
                    _ochre_froglight_side = Image.Load((byte[])ResourceManager.GetObject("ochre_froglight_side"));
                return _ochre_froglight_side;
            }
        }

        private static Image<Rgba32> _ochre_froglight_top = null;
        public static Image<Rgba32> ochre_froglight_top {
            get {
                if (_ochre_froglight_top == null)
                    _ochre_froglight_top = Image.Load((byte[])ResourceManager.GetObject("ochre_froglight_top"));
                return _ochre_froglight_top;
            }
        }

        private static Image<Rgba32> _orange_concrete = null;
        public static Image<Rgba32> orange_concrete {
            get {
                if (_orange_concrete == null)
                    _orange_concrete = Image.Load((byte[])ResourceManager.GetObject("orange_concrete"));
                return _orange_concrete;
            }
        }

        private static Image<Rgba32> _orange_concrete_powder = null;
        public static Image<Rgba32> orange_concrete_powder {
            get {
                if (_orange_concrete_powder == null)
                    _orange_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("orange_concrete_powder"));
                return _orange_concrete_powder;
            }
        }

        private static Image<Rgba32> _orange_glazed_terracotta = null;
        public static Image<Rgba32> orange_glazed_terracotta {
            get {
                if (_orange_glazed_terracotta == null)
                    _orange_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("orange_glazed_terracotta"));
                return _orange_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _orange_stained_glass = null;
        public static Image<Rgba32> orange_stained_glass {
            get {
                if (_orange_stained_glass == null)
                    _orange_stained_glass = Image.Load((byte[])ResourceManager.GetObject("orange_stained_glass"));
                return _orange_stained_glass;
            }
        }

        private static Image<Rgba32> _orange_terracotta = null;
        public static Image<Rgba32> orange_terracotta {
            get {
                if (_orange_terracotta == null)
                    _orange_terracotta = Image.Load((byte[])ResourceManager.GetObject("orange_terracotta"));
                return _orange_terracotta;
            }
        }

        private static Image<Rgba32> _orange_wool = null;
        public static Image<Rgba32> orange_wool {
            get {
                if (_orange_wool == null)
                    _orange_wool = Image.Load((byte[])ResourceManager.GetObject("orange_wool"));
                return _orange_wool;
            }
        }

        private static Image<Rgba32> _oxidized_copper = null;
        public static Image<Rgba32> oxidized_copper {
            get {
                if (_oxidized_copper == null)
                    _oxidized_copper = Image.Load((byte[])ResourceManager.GetObject("oxidized_copper"));
                return _oxidized_copper;
            }
        }

        private static Image<Rgba32> _oxidized_cut_copper = null;
        public static Image<Rgba32> oxidized_cut_copper {
            get {
                if (_oxidized_cut_copper == null)
                    _oxidized_cut_copper = Image.Load((byte[])ResourceManager.GetObject("oxidized_cut_copper"));
                return _oxidized_cut_copper;
            }
        }

        private static Image<Rgba32> _packed_ice = null;
        public static Image<Rgba32> packed_ice {
            get {
                if (_packed_ice == null)
                    _packed_ice = Image.Load((byte[])ResourceManager.GetObject("packed_ice"));
                return _packed_ice;
            }
        }

        private static Image<Rgba32> _packed_mud = null;
        public static Image<Rgba32> packed_mud {
            get {
                if (_packed_mud == null)
                    _packed_mud = Image.Load((byte[])ResourceManager.GetObject("packed_mud"));
                return _packed_mud;
            }
        }

        private static Image<Rgba32> _pearlescent_froglight_side = null;
        public static Image<Rgba32> pearlescent_froglight_side {
            get {
                if (_pearlescent_froglight_side == null)
                    _pearlescent_froglight_side = Image.Load((byte[])ResourceManager.GetObject("pearlescent_froglight_side"));
                return _pearlescent_froglight_side;
            }
        }

        private static Image<Rgba32> _pearlescent_froglight_top = null;
        public static Image<Rgba32> pearlescent_froglight_top {
            get {
                if (_pearlescent_froglight_top == null)
                    _pearlescent_froglight_top = Image.Load((byte[])ResourceManager.GetObject("pearlescent_froglight_top"));
                return _pearlescent_froglight_top;
            }
        }

        private static Image<Rgba32> _pink_concrete = null;
        public static Image<Rgba32> pink_concrete {
            get {
                if (_pink_concrete == null)
                    _pink_concrete = Image.Load((byte[])ResourceManager.GetObject("pink_concrete"));
                return _pink_concrete;
            }
        }

        private static Image<Rgba32> _pink_concrete_powder = null;
        public static Image<Rgba32> pink_concrete_powder {
            get {
                if (_pink_concrete_powder == null)
                    _pink_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("pink_concrete_powder"));
                return _pink_concrete_powder;
            }
        }

        private static Image<Rgba32> _pink_glazed_terracotta = null;
        public static Image<Rgba32> pink_glazed_terracotta {
            get {
                if (_pink_glazed_terracotta == null)
                    _pink_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("pink_glazed_terracotta"));
                return _pink_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _pink_stained_glass = null;
        public static Image<Rgba32> pink_stained_glass {
            get {
                if (_pink_stained_glass == null)
                    _pink_stained_glass = Image.Load((byte[])ResourceManager.GetObject("pink_stained_glass"));
                return _pink_stained_glass;
            }
        }

        private static Image<Rgba32> _pink_terracotta = null;
        public static Image<Rgba32> pink_terracotta {
            get {
                if (_pink_terracotta == null)
                    _pink_terracotta = Image.Load((byte[])ResourceManager.GetObject("pink_terracotta"));
                return _pink_terracotta;
            }
        }

        private static Image<Rgba32> _pink_wool = null;
        public static Image<Rgba32> pink_wool {
            get {
                if (_pink_wool == null)
                    _pink_wool = Image.Load((byte[])ResourceManager.GetObject("pink_wool"));
                return _pink_wool;
            }
        }

        private static Image<Rgba32> _podzol_side = null;
        public static Image<Rgba32> podzol_side {
            get {
                if (_podzol_side == null)
                    _podzol_side = Image.Load((byte[])ResourceManager.GetObject("podzol_side"));
                return _podzol_side;
            }
        }

        private static Image<Rgba32> _podzol_top = null;
        public static Image<Rgba32> podzol_top {
            get {
                if (_podzol_top == null)
                    _podzol_top = Image.Load((byte[])ResourceManager.GetObject("podzol_top"));
                return _podzol_top;
            }
        }

        private static Image<Rgba32> _polished_andesite = null;
        public static Image<Rgba32> polished_andesite {
            get {
                if (_polished_andesite == null)
                    _polished_andesite = Image.Load((byte[])ResourceManager.GetObject("polished_andesite"));
                return _polished_andesite;
            }
        }

        private static Image<Rgba32> _polished_basalt_side = null;
        public static Image<Rgba32> polished_basalt_side {
            get {
                if (_polished_basalt_side == null)
                    _polished_basalt_side = Image.Load((byte[])ResourceManager.GetObject("polished_basalt_side"));
                return _polished_basalt_side;
            }
        }

        private static Image<Rgba32> _polished_basalt_top = null;
        public static Image<Rgba32> polished_basalt_top {
            get {
                if (_polished_basalt_top == null)
                    _polished_basalt_top = Image.Load((byte[])ResourceManager.GetObject("polished_basalt_top"));
                return _polished_basalt_top;
            }
        }

        private static Image<Rgba32> _polished_blackstone = null;
        public static Image<Rgba32> polished_blackstone {
            get {
                if (_polished_blackstone == null)
                    _polished_blackstone = Image.Load((byte[])ResourceManager.GetObject("polished_blackstone"));
                return _polished_blackstone;
            }
        }

        private static Image<Rgba32> _polished_blackstone_bricks = null;
        public static Image<Rgba32> polished_blackstone_bricks {
            get {
                if (_polished_blackstone_bricks == null)
                    _polished_blackstone_bricks = Image.Load((byte[])ResourceManager.GetObject("polished_blackstone_bricks"));
                return _polished_blackstone_bricks;
            }
        }

        private static Image<Rgba32> _polished_deepslate = null;
        public static Image<Rgba32> polished_deepslate {
            get {
                if (_polished_deepslate == null)
                    _polished_deepslate = Image.Load((byte[])ResourceManager.GetObject("polished_deepslate"));
                return _polished_deepslate;
            }
        }

        private static Image<Rgba32> _polished_diorite = null;
        public static Image<Rgba32> polished_diorite {
            get {
                if (_polished_diorite == null)
                    _polished_diorite = Image.Load((byte[])ResourceManager.GetObject("polished_diorite"));
                return _polished_diorite;
            }
        }

        private static Image<Rgba32> _polished_granite = null;
        public static Image<Rgba32> polished_granite {
            get {
                if (_polished_granite == null)
                    _polished_granite = Image.Load((byte[])ResourceManager.GetObject("polished_granite"));
                return _polished_granite;
            }
        }

        private static Image<Rgba32> _prismarine = null;
        public static Image<Rgba32> prismarine {
            get {
                if (_prismarine == null)
                    _prismarine = Image.Load((byte[])ResourceManager.GetObject("prismarine"));
                return _prismarine;
            }
        }

        private static Image<Rgba32> _prismarine_bricks = null;
        public static Image<Rgba32> prismarine_bricks {
            get {
                if (_prismarine_bricks == null)
                    _prismarine_bricks = Image.Load((byte[])ResourceManager.GetObject("prismarine_bricks"));
                return _prismarine_bricks;
            }
        }

        private static Image<Rgba32> _pumpkin_side = null;
        public static Image<Rgba32> pumpkin_side {
            get {
                if (_pumpkin_side == null)
                    _pumpkin_side = Image.Load((byte[])ResourceManager.GetObject("pumpkin_side"));
                return _pumpkin_side;
            }
        }

        private static Image<Rgba32> _pumpkin_top = null;
        public static Image<Rgba32> pumpkin_top {
            get {
                if (_pumpkin_top == null)
                    _pumpkin_top = Image.Load((byte[])ResourceManager.GetObject("pumpkin_top"));
                return _pumpkin_top;
            }
        }

        private static Image<Rgba32> _purple_concrete = null;
        public static Image<Rgba32> purple_concrete {
            get {
                if (_purple_concrete == null)
                    _purple_concrete = Image.Load((byte[])ResourceManager.GetObject("purple_concrete"));
                return _purple_concrete;
            }
        }

        private static Image<Rgba32> _purple_concrete_powder = null;
        public static Image<Rgba32> purple_concrete_powder {
            get {
                if (_purple_concrete_powder == null)
                    _purple_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("purple_concrete_powder"));
                return _purple_concrete_powder;
            }
        }

        private static Image<Rgba32> _purple_glazed_terracotta = null;
        public static Image<Rgba32> purple_glazed_terracotta {
            get {
                if (_purple_glazed_terracotta == null)
                    _purple_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("purple_glazed_terracotta"));
                return _purple_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _purple_stained_glass = null;
        public static Image<Rgba32> purple_stained_glass {
            get {
                if (_purple_stained_glass == null)
                    _purple_stained_glass = Image.Load((byte[])ResourceManager.GetObject("purple_stained_glass"));
                return _purple_stained_glass;
            }
        }

        private static Image<Rgba32> _purple_terracotta = null;
        public static Image<Rgba32> purple_terracotta {
            get {
                if (_purple_terracotta == null)
                    _purple_terracotta = Image.Load((byte[])ResourceManager.GetObject("purple_terracotta"));
                return _purple_terracotta;
            }
        }

        private static Image<Rgba32> _purple_wool = null;
        public static Image<Rgba32> purple_wool {
            get {
                if (_purple_wool == null)
                    _purple_wool = Image.Load((byte[])ResourceManager.GetObject("purple_wool"));
                return _purple_wool;
            }
        }

        private static Image<Rgba32> _purpur_block = null;
        public static Image<Rgba32> purpur_block {
            get {
                if (_purpur_block == null)
                    _purpur_block = Image.Load((byte[])ResourceManager.GetObject("purpur_block"));
                return _purpur_block;
            }
        }

        private static Image<Rgba32> _purpur_pillar = null;
        public static Image<Rgba32> purpur_pillar {
            get {
                if (_purpur_pillar == null)
                    _purpur_pillar = Image.Load((byte[])ResourceManager.GetObject("purpur_pillar"));
                return _purpur_pillar;
            }
        }

        private static Image<Rgba32> _purpur_pillar_top = null;
        public static Image<Rgba32> purpur_pillar_top {
            get {
                if (_purpur_pillar_top == null)
                    _purpur_pillar_top = Image.Load((byte[])ResourceManager.GetObject("purpur_pillar_top"));
                return _purpur_pillar_top;
            }
        }

        private static Image<Rgba32> _quartz_block_bottom = null;
        public static Image<Rgba32> quartz_block_bottom {
            get {
                if (_quartz_block_bottom == null)
                    _quartz_block_bottom = Image.Load((byte[])ResourceManager.GetObject("quartz_block_bottom"));
                return _quartz_block_bottom;
            }
        }

        private static Image<Rgba32> _quartz_block_top = null;
        public static Image<Rgba32> quartz_block_top {
            get {
                if (_quartz_block_top == null)
                    _quartz_block_top = Image.Load((byte[])ResourceManager.GetObject("quartz_block_top"));
                return _quartz_block_top;
            }
        }

        private static Image<Rgba32> _quartz_bricks = null;
        public static Image<Rgba32> quartz_bricks {
            get {
                if (_quartz_bricks == null)
                    _quartz_bricks = Image.Load((byte[])ResourceManager.GetObject("quartz_bricks"));
                return _quartz_bricks;
            }
        }

        private static Image<Rgba32> _raw_copper_block = null;
        public static Image<Rgba32> raw_copper_block {
            get {
                if (_raw_copper_block == null)
                    _raw_copper_block = Image.Load((byte[])ResourceManager.GetObject("raw_copper_block"));
                return _raw_copper_block;
            }
        }

        private static Image<Rgba32> _raw_gold_block = null;
        public static Image<Rgba32> raw_gold_block {
            get {
                if (_raw_gold_block == null)
                    _raw_gold_block = Image.Load((byte[])ResourceManager.GetObject("raw_gold_block"));
                return _raw_gold_block;
            }
        }

        private static Image<Rgba32> _raw_iron_block = null;
        public static Image<Rgba32> raw_iron_block {
            get {
                if (_raw_iron_block == null)
                    _raw_iron_block = Image.Load((byte[])ResourceManager.GetObject("raw_iron_block"));
                return _raw_iron_block;
            }
        }

        private static Image<Rgba32> _redstone_block = null;
        public static Image<Rgba32> redstone_block {
            get {
                if (_redstone_block == null)
                    _redstone_block = Image.Load((byte[])ResourceManager.GetObject("redstone_block"));
                return _redstone_block;
            }
        }

        private static Image<Rgba32> _redstone_lamp = null;
        public static Image<Rgba32> redstone_lamp {
            get {
                if (_redstone_lamp == null)
                    _redstone_lamp = Image.Load((byte[])ResourceManager.GetObject("redstone_lamp"));
                return _redstone_lamp;
            }
        }

        private static Image<Rgba32> _redstone_ore = null;
        public static Image<Rgba32> redstone_ore {
            get {
                if (_redstone_ore == null)
                    _redstone_ore = Image.Load((byte[])ResourceManager.GetObject("redstone_ore"));
                return _redstone_ore;
            }
        }

        private static Image<Rgba32> _red_concrete = null;
        public static Image<Rgba32> red_concrete {
            get {
                if (_red_concrete == null)
                    _red_concrete = Image.Load((byte[])ResourceManager.GetObject("red_concrete"));
                return _red_concrete;
            }
        }

        private static Image<Rgba32> _red_concrete_powder = null;
        public static Image<Rgba32> red_concrete_powder {
            get {
                if (_red_concrete_powder == null)
                    _red_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("red_concrete_powder"));
                return _red_concrete_powder;
            }
        }

        private static Image<Rgba32> _red_glazed_terracotta = null;
        public static Image<Rgba32> red_glazed_terracotta {
            get {
                if (_red_glazed_terracotta == null)
                    _red_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("red_glazed_terracotta"));
                return _red_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _red_mushroom_block = null;
        public static Image<Rgba32> red_mushroom_block {
            get {
                if (_red_mushroom_block == null)
                    _red_mushroom_block = Image.Load((byte[])ResourceManager.GetObject("red_mushroom_block"));
                return _red_mushroom_block;
            }
        }

        private static Image<Rgba32> _red_nether_bricks = null;
        public static Image<Rgba32> red_nether_bricks {
            get {
                if (_red_nether_bricks == null)
                    _red_nether_bricks = Image.Load((byte[])ResourceManager.GetObject("red_nether_bricks"));
                return _red_nether_bricks;
            }
        }

        private static Image<Rgba32> _red_sand = null;
        public static Image<Rgba32> red_sand {
            get {
                if (_red_sand == null)
                    _red_sand = Image.Load((byte[])ResourceManager.GetObject("red_sand"));
                return _red_sand;
            }
        }

        private static Image<Rgba32> _red_sandstone_top = null;
        public static Image<Rgba32> red_sandstone_top {
            get {
                if (_red_sandstone_top == null)
                    _red_sandstone_top = Image.Load((byte[])ResourceManager.GetObject("red_sandstone_top"));
                return _red_sandstone_top;
            }
        }

        private static Image<Rgba32> _red_stained_glass = null;
        public static Image<Rgba32> red_stained_glass {
            get {
                if (_red_stained_glass == null)
                    _red_stained_glass = Image.Load((byte[])ResourceManager.GetObject("red_stained_glass"));
                return _red_stained_glass;
            }
        }

        private static Image<Rgba32> _red_terracotta = null;
        public static Image<Rgba32> red_terracotta {
            get {
                if (_red_terracotta == null)
                    _red_terracotta = Image.Load((byte[])ResourceManager.GetObject("red_terracotta"));
                return _red_terracotta;
            }
        }

        private static Image<Rgba32> _red_wool = null;
        public static Image<Rgba32> red_wool {
            get {
                if (_red_wool == null)
                    _red_wool = Image.Load((byte[])ResourceManager.GetObject("red_wool"));
                return _red_wool;
            }
        }

        private static Image<Rgba32> _reinforced_deepslate_side = null;
        public static Image<Rgba32> reinforced_deepslate_side {
            get {
                if (_reinforced_deepslate_side == null)
                    _reinforced_deepslate_side = Image.Load((byte[])ResourceManager.GetObject("reinforced_deepslate_side"));
                return _reinforced_deepslate_side;
            }
        }

        private static Image<Rgba32> _reinforced_deepslate_top = null;
        public static Image<Rgba32> reinforced_deepslate_top {
            get {
                if (_reinforced_deepslate_top == null)
                    _reinforced_deepslate_top = Image.Load((byte[])ResourceManager.GetObject("reinforced_deepslate_top"));
                return _reinforced_deepslate_top;
            }
        }

        private static Image<Rgba32> _rooted_dirt = null;
        public static Image<Rgba32> rooted_dirt {
            get {
                if (_rooted_dirt == null)
                    _rooted_dirt = Image.Load((byte[])ResourceManager.GetObject("rooted_dirt"));
                return _rooted_dirt;
            }
        }

        private static Image<Rgba32> _sand = null;
        public static Image<Rgba32> sand {
            get {
                if (_sand == null)
                    _sand = Image.Load((byte[])ResourceManager.GetObject("sand"));
                return _sand;
            }
        }

        private static Image<Rgba32> _sandstone_top = null;
        public static Image<Rgba32> sandstone_top {
            get {
                if (_sandstone_top == null)
                    _sandstone_top = Image.Load((byte[])ResourceManager.GetObject("sandstone_top"));
                return _sandstone_top;
            }
        }

        private static Image<Rgba32> _sculk_catalyst_top = null;
        public static Image<Rgba32> sculk_catalyst_top {
            get {
                if (_sculk_catalyst_top == null)
                    _sculk_catalyst_top = Image.Load((byte[])ResourceManager.GetObject("sculk_catalyst_top"));
                return _sculk_catalyst_top;
            }
        }

        private static Image<Rgba32> _sea_lantern = null;
        public static Image<Rgba32> sea_lantern {
            get {
                if (_sea_lantern == null)
                    _sea_lantern = Image.Load((byte[])ResourceManager.GetObject("sea_lantern"));
                return _sea_lantern;
            }
        }

        private static Image<Rgba32> _shroomlight = null;
        public static Image<Rgba32> shroomlight {
            get {
                if (_shroomlight == null)
                    _shroomlight = Image.Load((byte[])ResourceManager.GetObject("shroomlight"));
                return _shroomlight;
            }
        }

        private static Image<Rgba32> _slime_block = null;
        public static Image<Rgba32> slime_block {
            get {
                if (_slime_block == null)
                    _slime_block = Image.Load((byte[])ResourceManager.GetObject("slime_block"));
                return _slime_block;
            }
        }

        private static Image<Rgba32> _smoker_front = null;
        public static Image<Rgba32> smoker_front {
            get {
                if (_smoker_front == null)
                    _smoker_front = Image.Load((byte[])ResourceManager.GetObject("smoker_front"));
                return _smoker_front;
            }
        }

        private static Image<Rgba32> _smoker_side = null;
        public static Image<Rgba32> smoker_side {
            get {
                if (_smoker_side == null)
                    _smoker_side = Image.Load((byte[])ResourceManager.GetObject("smoker_side"));
                return _smoker_side;
            }
        }

        private static Image<Rgba32> _smoker_top = null;
        public static Image<Rgba32> smoker_top {
            get {
                if (_smoker_top == null)
                    _smoker_top = Image.Load((byte[])ResourceManager.GetObject("smoker_top"));
                return _smoker_top;
            }
        }

        private static Image<Rgba32> _smooth_basalt = null;
        public static Image<Rgba32> smooth_basalt {
            get {
                if (_smooth_basalt == null)
                    _smooth_basalt = Image.Load((byte[])ResourceManager.GetObject("smooth_basalt"));
                return _smooth_basalt;
            }
        }

        private static Image<Rgba32> _smooth_stone = null;
        public static Image<Rgba32> smooth_stone {
            get {
                if (_smooth_stone == null)
                    _smooth_stone = Image.Load((byte[])ResourceManager.GetObject("smooth_stone"));
                return _smooth_stone;
            }
        }

        private static Image<Rgba32> _smooth_stone_slab_side = null;
        public static Image<Rgba32> smooth_stone_slab_side {
            get {
                if (_smooth_stone_slab_side == null)
                    _smooth_stone_slab_side = Image.Load((byte[])ResourceManager.GetObject("smooth_stone_slab_side"));
                return _smooth_stone_slab_side;
            }
        }

        private static Image<Rgba32> _snow = null;
        public static Image<Rgba32> snow {
            get {
                if (_snow == null)
                    _snow = Image.Load((byte[])ResourceManager.GetObject("snow"));
                return _snow;
            }
        }

        private static Image<Rgba32> _soul_sand = null;
        public static Image<Rgba32> soul_sand {
            get {
                if (_soul_sand == null)
                    _soul_sand = Image.Load((byte[])ResourceManager.GetObject("soul_sand"));
                return _soul_sand;
            }
        }

        private static Image<Rgba32> _soul_soil = null;
        public static Image<Rgba32> soul_soil {
            get {
                if (_soul_soil == null)
                    _soul_soil = Image.Load((byte[])ResourceManager.GetObject("soul_soil"));
                return _soul_soil;
            }
        }

        private static Image<Rgba32> _sponge = null;
        public static Image<Rgba32> sponge {
            get {
                if (_sponge == null)
                    _sponge = Image.Load((byte[])ResourceManager.GetObject("sponge"));
                return _sponge;
            }
        }

        private static Image<Rgba32> _spruce_log = null;
        public static Image<Rgba32> spruce_log {
            get {
                if (_spruce_log == null)
                    _spruce_log = Image.Load((byte[])ResourceManager.GetObject("spruce_log"));
                return _spruce_log;
            }
        }

        private static Image<Rgba32> _spruce_planks = null;
        public static Image<Rgba32> spruce_planks {
            get {
                if (_spruce_planks == null)
                    _spruce_planks = Image.Load((byte[])ResourceManager.GetObject("spruce_planks"));
                return _spruce_planks;
            }
        }

        private static Image<Rgba32> _stone = null;
        public static Image<Rgba32> stone {
            get {
                if (_stone == null)
                    _stone = Image.Load((byte[])ResourceManager.GetObject("stone"));
                return _stone;
            }
        }

        private static Image<Rgba32> _stone_bricks = null;
        public static Image<Rgba32> stone_bricks {
            get {
                if (_stone_bricks == null)
                    _stone_bricks = Image.Load((byte[])ResourceManager.GetObject("stone_bricks"));
                return _stone_bricks;
            }
        }

        private static Image<Rgba32> _stripped_acacia_log = null;
        public static Image<Rgba32> stripped_acacia_log {
            get {
                if (_stripped_acacia_log == null)
                    _stripped_acacia_log = Image.Load((byte[])ResourceManager.GetObject("stripped_acacia_log"));
                return _stripped_acacia_log;
            }
        }

        private static Image<Rgba32> _stripped_acacia_log_top = null;
        public static Image<Rgba32> stripped_acacia_log_top {
            get {
                if (_stripped_acacia_log_top == null)
                    _stripped_acacia_log_top = Image.Load((byte[])ResourceManager.GetObject("stripped_acacia_log_top"));
                return _stripped_acacia_log_top;
            }
        }

        private static Image<Rgba32> _stripped_birch_log = null;
        public static Image<Rgba32> stripped_birch_log {
            get {
                if (_stripped_birch_log == null)
                    _stripped_birch_log = Image.Load((byte[])ResourceManager.GetObject("stripped_birch_log"));
                return _stripped_birch_log;
            }
        }

        private static Image<Rgba32> _stripped_birch_log_top = null;
        public static Image<Rgba32> stripped_birch_log_top {
            get {
                if (_stripped_birch_log_top == null)
                    _stripped_birch_log_top = Image.Load((byte[])ResourceManager.GetObject("stripped_birch_log_top"));
                return _stripped_birch_log_top;
            }
        }

        private static Image<Rgba32> _stripped_crimson_stem = null;
        public static Image<Rgba32> stripped_crimson_stem {
            get {
                if (_stripped_crimson_stem == null)
                    _stripped_crimson_stem = Image.Load((byte[])ResourceManager.GetObject("stripped_crimson_stem"));
                return _stripped_crimson_stem;
            }
        }

        private static Image<Rgba32> _stripped_crimson_stem_top = null;
        public static Image<Rgba32> stripped_crimson_stem_top {
            get {
                if (_stripped_crimson_stem_top == null)
                    _stripped_crimson_stem_top = Image.Load((byte[])ResourceManager.GetObject("stripped_crimson_stem_top"));
                return _stripped_crimson_stem_top;
            }
        }

        private static Image<Rgba32> _stripped_dark_oak_log = null;
        public static Image<Rgba32> stripped_dark_oak_log {
            get {
                if (_stripped_dark_oak_log == null)
                    _stripped_dark_oak_log = Image.Load((byte[])ResourceManager.GetObject("stripped_dark_oak_log"));
                return _stripped_dark_oak_log;
            }
        }

        private static Image<Rgba32> _stripped_dark_oak_log_top = null;
        public static Image<Rgba32> stripped_dark_oak_log_top {
            get {
                if (_stripped_dark_oak_log_top == null)
                    _stripped_dark_oak_log_top = Image.Load((byte[])ResourceManager.GetObject("stripped_dark_oak_log_top"));
                return _stripped_dark_oak_log_top;
            }
        }

        private static Image<Rgba32> _stripped_jungle_log = null;
        public static Image<Rgba32> stripped_jungle_log {
            get {
                if (_stripped_jungle_log == null)
                    _stripped_jungle_log = Image.Load((byte[])ResourceManager.GetObject("stripped_jungle_log"));
                return _stripped_jungle_log;
            }
        }

        private static Image<Rgba32> _stripped_jungle_log_top = null;
        public static Image<Rgba32> stripped_jungle_log_top {
            get {
                if (_stripped_jungle_log_top == null)
                    _stripped_jungle_log_top = Image.Load((byte[])ResourceManager.GetObject("stripped_jungle_log_top"));
                return _stripped_jungle_log_top;
            }
        }

        private static Image<Rgba32> _stripped_mangrove_log = null;
        public static Image<Rgba32> stripped_mangrove_log {
            get {
                if (_stripped_mangrove_log == null)
                    _stripped_mangrove_log = Image.Load((byte[])ResourceManager.GetObject("stripped_mangrove_log"));
                return _stripped_mangrove_log;
            }
        }

        private static Image<Rgba32> _stripped_mangrove_log_top = null;
        public static Image<Rgba32> stripped_mangrove_log_top {
            get {
                if (_stripped_mangrove_log_top == null)
                    _stripped_mangrove_log_top = Image.Load((byte[])ResourceManager.GetObject("stripped_mangrove_log_top"));
                return _stripped_mangrove_log_top;
            }
        }

        private static Image<Rgba32> _stripped_oak_log = null;
        public static Image<Rgba32> stripped_oak_log {
            get {
                if (_stripped_oak_log == null)
                    _stripped_oak_log = Image.Load((byte[])ResourceManager.GetObject("stripped_oak_log"));
                return _stripped_oak_log;
            }
        }

        private static Image<Rgba32> _stripped_oak_log_top = null;
        public static Image<Rgba32> stripped_oak_log_top {
            get {
                if (_stripped_oak_log_top == null)
                    _stripped_oak_log_top = Image.Load((byte[])ResourceManager.GetObject("stripped_oak_log_top"));
                return _stripped_oak_log_top;
            }
        }

        private static Image<Rgba32> _stripped_spruce_log = null;
        public static Image<Rgba32> stripped_spruce_log {
            get {
                if (_stripped_spruce_log == null)
                    _stripped_spruce_log = Image.Load((byte[])ResourceManager.GetObject("stripped_spruce_log"));
                return _stripped_spruce_log;
            }
        }

        private static Image<Rgba32> _stripped_spruce_log_top = null;
        public static Image<Rgba32> stripped_spruce_log_top {
            get {
                if (_stripped_spruce_log_top == null)
                    _stripped_spruce_log_top = Image.Load((byte[])ResourceManager.GetObject("stripped_spruce_log_top"));
                return _stripped_spruce_log_top;
            }
        }

        private static Image<Rgba32> _stripped_warped_stem = null;
        public static Image<Rgba32> stripped_warped_stem {
            get {
                if (_stripped_warped_stem == null)
                    _stripped_warped_stem = Image.Load((byte[])ResourceManager.GetObject("stripped_warped_stem"));
                return _stripped_warped_stem;
            }
        }

        private static Image<Rgba32> _stripped_warped_stem_top = null;
        public static Image<Rgba32> stripped_warped_stem_top {
            get {
                if (_stripped_warped_stem_top == null)
                    _stripped_warped_stem_top = Image.Load((byte[])ResourceManager.GetObject("stripped_warped_stem_top"));
                return _stripped_warped_stem_top;
            }
        }

        private static Image<Rgba32> _target_side = null;
        public static Image<Rgba32> target_side {
            get {
                if (_target_side == null)
                    _target_side = Image.Load((byte[])ResourceManager.GetObject("target_side"));
                return _target_side;
            }
        }

        private static Image<Rgba32> _target_top = null;
        public static Image<Rgba32> target_top {
            get {
                if (_target_top == null)
                    _target_top = Image.Load((byte[])ResourceManager.GetObject("target_top"));
                return _target_top;
            }
        }

        private static Image<Rgba32> _terracotta = null;
        public static Image<Rgba32> terracotta {
            get {
                if (_terracotta == null)
                    _terracotta = Image.Load((byte[])ResourceManager.GetObject("terracotta"));
                return _terracotta;
            }
        }

        private static Image<Rgba32> _tinted_glass = null;
        public static Image<Rgba32> tinted_glass {
            get {
                if (_tinted_glass == null)
                    _tinted_glass = Image.Load((byte[])ResourceManager.GetObject("tinted_glass"));
                return _tinted_glass;
            }
        }

        private static Image<Rgba32> _tnt_side = null;
        public static Image<Rgba32> tnt_side {
            get {
                if (_tnt_side == null)
                    _tnt_side = Image.Load((byte[])ResourceManager.GetObject("tnt_side"));
                return _tnt_side;
            }
        }

        private static Image<Rgba32> _tnt_top = null;
        public static Image<Rgba32> tnt_top {
            get {
                if (_tnt_top == null)
                    _tnt_top = Image.Load((byte[])ResourceManager.GetObject("tnt_top"));
                return _tnt_top;
            }
        }

        private static Image<Rgba32> _tube_coral_block = null;
        public static Image<Rgba32> tube_coral_block {
            get {
                if (_tube_coral_block == null)
                    _tube_coral_block = Image.Load((byte[])ResourceManager.GetObject("tube_coral_block"));
                return _tube_coral_block;
            }
        }

        private static Image<Rgba32> _tuff = null;
        public static Image<Rgba32> tuff {
            get {
                if (_tuff == null)
                    _tuff = Image.Load((byte[])ResourceManager.GetObject("tuff"));
                return _tuff;
            }
        }

        private static Image<Rgba32> _verdant_froglight_side = null;
        public static Image<Rgba32> verdant_froglight_side {
            get {
                if (_verdant_froglight_side == null)
                    _verdant_froglight_side = Image.Load((byte[])ResourceManager.GetObject("verdant_froglight_side"));
                return _verdant_froglight_side;
            }
        }

        private static Image<Rgba32> _verdant_froglight_top = null;
        public static Image<Rgba32> verdant_froglight_top {
            get {
                if (_verdant_froglight_top == null)
                    _verdant_froglight_top = Image.Load((byte[])ResourceManager.GetObject("verdant_froglight_top"));
                return _verdant_froglight_top;
            }
        }

        private static Image<Rgba32> _warped_nylium = null;
        public static Image<Rgba32> warped_nylium {
            get {
                if (_warped_nylium == null)
                    _warped_nylium = Image.Load((byte[])ResourceManager.GetObject("warped_nylium"));
                return _warped_nylium;
            }
        }

        private static Image<Rgba32> _warped_nylium_side = null;
        public static Image<Rgba32> warped_nylium_side {
            get {
                if (_warped_nylium_side == null)
                    _warped_nylium_side = Image.Load((byte[])ResourceManager.GetObject("warped_nylium_side"));
                return _warped_nylium_side;
            }
        }

        private static Image<Rgba32> _warped_planks = null;
        public static Image<Rgba32> warped_planks {
            get {
                if (_warped_planks == null)
                    _warped_planks = Image.Load((byte[])ResourceManager.GetObject("warped_planks"));
                return _warped_planks;
            }
        }

        private static Image<Rgba32> _warped_stem = null;
        public static Image<Rgba32> warped_stem {
            get {
                if (_warped_stem == null)
                    _warped_stem = Image.Load((byte[])ResourceManager.GetObject("warped_stem"));
                return _warped_stem;
            }
        }

        private static Image<Rgba32> _warped_wart_block = null;
        public static Image<Rgba32> warped_wart_block {
            get {
                if (_warped_wart_block == null)
                    _warped_wart_block = Image.Load((byte[])ResourceManager.GetObject("warped_wart_block"));
                return _warped_wart_block;
            }
        }

        private static Image<Rgba32> _weathered_copper = null;
        public static Image<Rgba32> weathered_copper {
            get {
                if (_weathered_copper == null)
                    _weathered_copper = Image.Load((byte[])ResourceManager.GetObject("weathered_copper"));
                return _weathered_copper;
            }
        }

        private static Image<Rgba32> _weathered_cut_copper = null;
        public static Image<Rgba32> weathered_cut_copper {
            get {
                if (_weathered_cut_copper == null)
                    _weathered_cut_copper = Image.Load((byte[])ResourceManager.GetObject("weathered_cut_copper"));
                return _weathered_cut_copper;
            }
        }

        private static Image<Rgba32> _wet_sponge = null;
        public static Image<Rgba32> wet_sponge {
            get {
                if (_wet_sponge == null)
                    _wet_sponge = Image.Load((byte[])ResourceManager.GetObject("wet_sponge"));
                return _wet_sponge;
            }
        }

        private static Image<Rgba32> _white_concrete = null;
        public static Image<Rgba32> white_concrete {
            get {
                if (_white_concrete == null)
                    _white_concrete = Image.Load((byte[])ResourceManager.GetObject("white_concrete"));
                return _white_concrete;
            }
        }

        private static Image<Rgba32> _white_concrete_powder = null;
        public static Image<Rgba32> white_concrete_powder {
            get {
                if (_white_concrete_powder == null)
                    _white_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("white_concrete_powder"));
                return _white_concrete_powder;
            }
        }

        private static Image<Rgba32> _white_glazed_terracotta = null;
        public static Image<Rgba32> white_glazed_terracotta {
            get {
                if (_white_glazed_terracotta == null)
                    _white_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("white_glazed_terracotta"));
                return _white_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _white_stained_glass = null;
        public static Image<Rgba32> white_stained_glass {
            get {
                if (_white_stained_glass == null)
                    _white_stained_glass = Image.Load((byte[])ResourceManager.GetObject("white_stained_glass"));
                return _white_stained_glass;
            }
        }

        private static Image<Rgba32> _white_terracotta = null;
        public static Image<Rgba32> white_terracotta {
            get {
                if (_white_terracotta == null)
                    _white_terracotta = Image.Load((byte[])ResourceManager.GetObject("white_terracotta"));
                return _white_terracotta;
            }
        }

        private static Image<Rgba32> _white_wool = null;
        public static Image<Rgba32> white_wool {
            get {
                if (_white_wool == null)
                    _white_wool = Image.Load((byte[])ResourceManager.GetObject("white_wool"));
                return _white_wool;
            }
        }

        private static Image<Rgba32> _yellow_concrete = null;
        public static Image<Rgba32> yellow_concrete {
            get {
                if (_yellow_concrete == null)
                    _yellow_concrete = Image.Load((byte[])ResourceManager.GetObject("yellow_concrete"));
                return _yellow_concrete;
            }
        }

        private static Image<Rgba32> _yellow_concrete_powder = null;
        public static Image<Rgba32> yellow_concrete_powder {
            get {
                if (_yellow_concrete_powder == null)
                    _yellow_concrete_powder = Image.Load((byte[])ResourceManager.GetObject("yellow_concrete_powder"));
                return _yellow_concrete_powder;
            }
        }

        private static Image<Rgba32> _yellow_glazed_terracotta = null;
        public static Image<Rgba32> yellow_glazed_terracotta {
            get {
                if (_yellow_glazed_terracotta == null)
                    _yellow_glazed_terracotta = Image.Load((byte[])ResourceManager.GetObject("yellow_glazed_terracotta"));
                return _yellow_glazed_terracotta;
            }
        }

        private static Image<Rgba32> _yellow_stained_glass = null;
        public static Image<Rgba32> yellow_stained_glass {
            get {
                if (_yellow_stained_glass == null)
                    _yellow_stained_glass = Image.Load((byte[])ResourceManager.GetObject("yellow_stained_glass"));
                return _yellow_stained_glass;
            }
        }

        private static Image<Rgba32> _yellow_terracotta = null;
        public static Image<Rgba32> yellow_terracotta {
            get {
                if (_yellow_terracotta == null)
                    _yellow_terracotta = Image.Load((byte[])ResourceManager.GetObject("yellow_terracotta"));
                return _yellow_terracotta;
            }
        }

        private static Image<Rgba32> _yellow_wool = null;
        public static Image<Rgba32> yellow_wool {
            get {
                if (_yellow_wool == null)
                    _yellow_wool = Image.Load((byte[])ResourceManager.GetObject("yellow_wool"));
                return _yellow_wool;
            }
        }
        // add bamboo_planks, bamboo_mosaic, cherry_planks, stripped_bamboo_block, stripped_cherry_log, stripped_bamboo_block_top, stripped_cherry_log_top
        private static Image<Rgba32> _bamboo_planks = null;
        public static Image<Rgba32> bamboo_planks {
            get {
                if (_bamboo_planks == null)
                    _bamboo_planks = Image.Load((byte[])ResourceManager.GetObject("bamboo_planks"));
                return _bamboo_planks;
            }
        }

        private static Image<Rgba32> _bamboo_mosaic = null;
        public static Image<Rgba32> bamboo_mosaic {
            get {
                if (_bamboo_mosaic == null)
                    _bamboo_mosaic = Image.Load((byte[])ResourceManager.GetObject("bamboo_mosaic"));
                return _bamboo_mosaic;
            }
        }

        private static Image<Rgba32> _cherry_planks = null;
        public static Image<Rgba32> cherry_planks {
            get {
                if (_cherry_planks == null)
                    _cherry_planks = Image.Load((byte[])ResourceManager.GetObject("cherry_planks"));
                return _cherry_planks;
            }
        }

        private static Image<Rgba32> _stripped_bamboo_block = null;
        public static Image<Rgba32> stripped_bamboo_block {
            get {
                if (_stripped_bamboo_block == null)
                    _stripped_bamboo_block = Image.Load((byte[])ResourceManager.GetObject("stripped_bamboo_block"));
                return _stripped_bamboo_block;
            }
	    }

        private static Image<Rgba32> _stripped_cherry_log = null;
        public static Image<Rgba32> stripped_cherry_log {
            get {
                if (_stripped_cherry_log == null)
                    _stripped_cherry_log = Image.Load((byte[])ResourceManager.GetObject("stripped_cherry_log"));
                return _stripped_cherry_log;
            }
        }

        private static Image<Rgba32> _stripped_bamboo_block_top = null;
        public static Image<Rgba32> stripped_bamboo_block_top {
            get {
                if (_stripped_bamboo_block_top == null)
                    _stripped_bamboo_block_top = Image.Load((byte[])ResourceManager.GetObject("stripped_bamboo_block_top"));
                return _stripped_bamboo_block_top;
            }
        }

        private static Image<Rgba32> _stripped_cherry_log_top = null;
        public static Image<Rgba32> stripped_cherry_log_top {
            get {
                if (_stripped_cherry_log_top == null)
                    _stripped_cherry_log_top = Image.Load((byte[])ResourceManager.GetObject("stripped_cherry_log_top"));
                return _stripped_cherry_log_top;
            }
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles
#endif
