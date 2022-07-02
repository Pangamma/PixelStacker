#if SKIA_SHARP
#pragma warning disable IDE1006 // Naming Styles
namespace PixelStacker.Resources {
	using System;
	using SkiaSharp;

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

        private static SKBitmap _acacia_log = null;
        public static SKBitmap acacia_log {
            get {
                if (_acacia_log == null)
                    _acacia_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("acacia_log"))
                    .Copy(SKColorType.Rgba8888);
                return _acacia_log;
            }
        }

        private static SKBitmap _acacia_planks = null;
        public static SKBitmap acacia_planks {
            get {
                if (_acacia_planks == null)
                    _acacia_planks = SKBitmap.Decode((byte[])ResourceManager.GetObject("acacia_planks"))
                    .Copy(SKColorType.Rgba8888);
                return _acacia_planks;
            }
        }

        private static SKBitmap _air = null;
        public static SKBitmap air {
            get {
                if (_air == null)
                    _air = SKBitmap.Decode((byte[])ResourceManager.GetObject("air"))
                    .Copy(SKColorType.Rgba8888);
                return _air;
            }
        }

        private static SKBitmap _amethyst_block = null;
        public static SKBitmap amethyst_block {
            get {
                if (_amethyst_block == null)
                    _amethyst_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("amethyst_block"))
                    .Copy(SKColorType.Rgba8888);
                return _amethyst_block;
            }
        }

        private static SKBitmap _ancient_debris_side = null;
        public static SKBitmap ancient_debris_side {
            get {
                if (_ancient_debris_side == null)
                    _ancient_debris_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("ancient_debris_side"))
                    .Copy(SKColorType.Rgba8888);
                return _ancient_debris_side;
            }
        }

        private static SKBitmap _ancient_debris_top = null;
        public static SKBitmap ancient_debris_top {
            get {
                if (_ancient_debris_top == null)
                    _ancient_debris_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("ancient_debris_top"))
                    .Copy(SKColorType.Rgba8888);
                return _ancient_debris_top;
            }
        }

        private static SKBitmap _andesite = null;
        public static SKBitmap andesite {
            get {
                if (_andesite == null)
                    _andesite = SKBitmap.Decode((byte[])ResourceManager.GetObject("andesite"))
                    .Copy(SKColorType.Rgba8888);
                return _andesite;
            }
        }

        private static SKBitmap _barrier = null;
        public static SKBitmap barrier {
            get {
                if (_barrier == null)
                    _barrier = SKBitmap.Decode((byte[])ResourceManager.GetObject("barrier"))
                    .Copy(SKColorType.Rgba8888);
                return _barrier;
            }
        }

        private static SKBitmap _basalt_side = null;
        public static SKBitmap basalt_side {
            get {
                if (_basalt_side == null)
                    _basalt_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("basalt_side"))
                    .Copy(SKColorType.Rgba8888);
                return _basalt_side;
            }
        }

        private static SKBitmap _basalt_top = null;
        public static SKBitmap basalt_top {
            get {
                if (_basalt_top == null)
                    _basalt_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("basalt_top"))
                    .Copy(SKColorType.Rgba8888);
                return _basalt_top;
            }
        }

        private static SKBitmap _bedrock = null;
        public static SKBitmap bedrock {
            get {
                if (_bedrock == null)
                    _bedrock = SKBitmap.Decode((byte[])ResourceManager.GetObject("bedrock"))
                    .Copy(SKColorType.Rgba8888);
                return _bedrock;
            }
        }

        private static SKBitmap _beehive_end = null;
        public static SKBitmap beehive_end {
            get {
                if (_beehive_end == null)
                    _beehive_end = SKBitmap.Decode((byte[])ResourceManager.GetObject("beehive_end"))
                    .Copy(SKColorType.Rgba8888);
                return _beehive_end;
            }
        }

        private static SKBitmap _beehive_side = null;
        public static SKBitmap beehive_side {
            get {
                if (_beehive_side == null)
                    _beehive_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("beehive_side"))
                    .Copy(SKColorType.Rgba8888);
                return _beehive_side;
            }
        }

        private static SKBitmap _bee_nest_side = null;
        public static SKBitmap bee_nest_side {
            get {
                if (_bee_nest_side == null)
                    _bee_nest_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("bee_nest_side"))
                    .Copy(SKColorType.Rgba8888);
                return _bee_nest_side;
            }
        }

        private static SKBitmap _bee_nest_top = null;
        public static SKBitmap bee_nest_top {
            get {
                if (_bee_nest_top == null)
                    _bee_nest_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("bee_nest_top"))
                    .Copy(SKColorType.Rgba8888);
                return _bee_nest_top;
            }
        }

        private static SKBitmap _birch_log = null;
        public static SKBitmap birch_log {
            get {
                if (_birch_log == null)
                    _birch_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("birch_log"))
                    .Copy(SKColorType.Rgba8888);
                return _birch_log;
            }
        }

        private static SKBitmap _birch_planks = null;
        public static SKBitmap birch_planks {
            get {
                if (_birch_planks == null)
                    _birch_planks = SKBitmap.Decode((byte[])ResourceManager.GetObject("birch_planks"))
                    .Copy(SKColorType.Rgba8888);
                return _birch_planks;
            }
        }

        private static SKBitmap _blackstone = null;
        public static SKBitmap blackstone {
            get {
                if (_blackstone == null)
                    _blackstone = SKBitmap.Decode((byte[])ResourceManager.GetObject("blackstone"))
                    .Copy(SKColorType.Rgba8888);
                return _blackstone;
            }
        }

        private static SKBitmap _blackstone_top = null;
        public static SKBitmap blackstone_top {
            get {
                if (_blackstone_top == null)
                    _blackstone_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("blackstone_top"))
                    .Copy(SKColorType.Rgba8888);
                return _blackstone_top;
            }
        }

        private static SKBitmap _black_concrete = null;
        public static SKBitmap black_concrete {
            get {
                if (_black_concrete == null)
                    _black_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("black_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _black_concrete;
            }
        }

        private static SKBitmap _black_concrete_powder = null;
        public static SKBitmap black_concrete_powder {
            get {
                if (_black_concrete_powder == null)
                    _black_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("black_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _black_concrete_powder;
            }
        }

        private static SKBitmap _black_glazed_terracotta = null;
        public static SKBitmap black_glazed_terracotta {
            get {
                if (_black_glazed_terracotta == null)
                    _black_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("black_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _black_glazed_terracotta;
            }
        }

        private static SKBitmap _black_stained_glass = null;
        public static SKBitmap black_stained_glass {
            get {
                if (_black_stained_glass == null)
                    _black_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("black_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _black_stained_glass;
            }
        }

        private static SKBitmap _black_terracotta = null;
        public static SKBitmap black_terracotta {
            get {
                if (_black_terracotta == null)
                    _black_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("black_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _black_terracotta;
            }
        }

        private static SKBitmap _black_wool = null;
        public static SKBitmap black_wool {
            get {
                if (_black_wool == null)
                    _black_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("black_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _black_wool;
            }
        }

        private static SKBitmap _blue_concrete = null;
        public static SKBitmap blue_concrete {
            get {
                if (_blue_concrete == null)
                    _blue_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("blue_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _blue_concrete;
            }
        }

        private static SKBitmap _blue_concrete_powder = null;
        public static SKBitmap blue_concrete_powder {
            get {
                if (_blue_concrete_powder == null)
                    _blue_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("blue_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _blue_concrete_powder;
            }
        }

        private static SKBitmap _blue_glazed_terracotta = null;
        public static SKBitmap blue_glazed_terracotta {
            get {
                if (_blue_glazed_terracotta == null)
                    _blue_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("blue_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _blue_glazed_terracotta;
            }
        }

        private static SKBitmap _blue_ice = null;
        public static SKBitmap blue_ice {
            get {
                if (_blue_ice == null)
                    _blue_ice = SKBitmap.Decode((byte[])ResourceManager.GetObject("blue_ice"))
                    .Copy(SKColorType.Rgba8888);
                return _blue_ice;
            }
        }

        private static SKBitmap _blue_stained_glass = null;
        public static SKBitmap blue_stained_glass {
            get {
                if (_blue_stained_glass == null)
                    _blue_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("blue_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _blue_stained_glass;
            }
        }

        private static SKBitmap _blue_terracotta = null;
        public static SKBitmap blue_terracotta {
            get {
                if (_blue_terracotta == null)
                    _blue_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("blue_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _blue_terracotta;
            }
        }

        private static SKBitmap _blue_wool = null;
        public static SKBitmap blue_wool {
            get {
                if (_blue_wool == null)
                    _blue_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("blue_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _blue_wool;
            }
        }

        private static SKBitmap _bone_block_side = null;
        public static SKBitmap bone_block_side {
            get {
                if (_bone_block_side == null)
                    _bone_block_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("bone_block_side"))
                    .Copy(SKColorType.Rgba8888);
                return _bone_block_side;
            }
        }

        private static SKBitmap _bone_block_top = null;
        public static SKBitmap bone_block_top {
            get {
                if (_bone_block_top == null)
                    _bone_block_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("bone_block_top"))
                    .Copy(SKColorType.Rgba8888);
                return _bone_block_top;
            }
        }

        private static SKBitmap _bookshelf = null;
        public static SKBitmap bookshelf {
            get {
                if (_bookshelf == null)
                    _bookshelf = SKBitmap.Decode((byte[])ResourceManager.GetObject("bookshelf"))
                    .Copy(SKColorType.Rgba8888);
                return _bookshelf;
            }
        }

        private static SKBitmap _brain_coral_block = null;
        public static SKBitmap brain_coral_block {
            get {
                if (_brain_coral_block == null)
                    _brain_coral_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("brain_coral_block"))
                    .Copy(SKColorType.Rgba8888);
                return _brain_coral_block;
            }
        }

        private static SKBitmap _bricks = null;
        public static SKBitmap bricks {
            get {
                if (_bricks == null)
                    _bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _bricks;
            }
        }

        private static SKBitmap _brown_concrete = null;
        public static SKBitmap brown_concrete {
            get {
                if (_brown_concrete == null)
                    _brown_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("brown_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _brown_concrete;
            }
        }

        private static SKBitmap _brown_concrete_powder = null;
        public static SKBitmap brown_concrete_powder {
            get {
                if (_brown_concrete_powder == null)
                    _brown_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("brown_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _brown_concrete_powder;
            }
        }

        private static SKBitmap _brown_glazed_terracotta = null;
        public static SKBitmap brown_glazed_terracotta {
            get {
                if (_brown_glazed_terracotta == null)
                    _brown_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("brown_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _brown_glazed_terracotta;
            }
        }

        private static SKBitmap _brown_mushroom_block = null;
        public static SKBitmap brown_mushroom_block {
            get {
                if (_brown_mushroom_block == null)
                    _brown_mushroom_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("brown_mushroom_block"))
                    .Copy(SKColorType.Rgba8888);
                return _brown_mushroom_block;
            }
        }

        private static SKBitmap _brown_stained_glass = null;
        public static SKBitmap brown_stained_glass {
            get {
                if (_brown_stained_glass == null)
                    _brown_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("brown_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _brown_stained_glass;
            }
        }

        private static SKBitmap _brown_terracotta = null;
        public static SKBitmap brown_terracotta {
            get {
                if (_brown_terracotta == null)
                    _brown_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("brown_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _brown_terracotta;
            }
        }

        private static SKBitmap _brown_wool = null;
        public static SKBitmap brown_wool {
            get {
                if (_brown_wool == null)
                    _brown_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("brown_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _brown_wool;
            }
        }

        private static SKBitmap _bubble_coral_block = null;
        public static SKBitmap bubble_coral_block {
            get {
                if (_bubble_coral_block == null)
                    _bubble_coral_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("bubble_coral_block"))
                    .Copy(SKColorType.Rgba8888);
                return _bubble_coral_block;
            }
        }

        private static SKBitmap _budding_amethyst = null;
        public static SKBitmap budding_amethyst {
            get {
                if (_budding_amethyst == null)
                    _budding_amethyst = SKBitmap.Decode((byte[])ResourceManager.GetObject("budding_amethyst"))
                    .Copy(SKColorType.Rgba8888);
                return _budding_amethyst;
            }
        }

        private static SKBitmap _calcite = null;
        public static SKBitmap calcite {
            get {
                if (_calcite == null)
                    _calcite = SKBitmap.Decode((byte[])ResourceManager.GetObject("calcite"))
                    .Copy(SKColorType.Rgba8888);
                return _calcite;
            }
        }

        private static SKBitmap _carved_pumpkin = null;
        public static SKBitmap carved_pumpkin {
            get {
                if (_carved_pumpkin == null)
                    _carved_pumpkin = SKBitmap.Decode((byte[])ResourceManager.GetObject("carved_pumpkin"))
                    .Copy(SKColorType.Rgba8888);
                return _carved_pumpkin;
            }
        }

        private static SKBitmap _chiseled_deepslate = null;
        public static SKBitmap chiseled_deepslate {
            get {
                if (_chiseled_deepslate == null)
                    _chiseled_deepslate = SKBitmap.Decode((byte[])ResourceManager.GetObject("chiseled_deepslate"))
                    .Copy(SKColorType.Rgba8888);
                return _chiseled_deepslate;
            }
        }

        private static SKBitmap _chiseled_nether_bricks = null;
        public static SKBitmap chiseled_nether_bricks {
            get {
                if (_chiseled_nether_bricks == null)
                    _chiseled_nether_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("chiseled_nether_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _chiseled_nether_bricks;
            }
        }

        private static SKBitmap _chiseled_polished_blackstone = null;
        public static SKBitmap chiseled_polished_blackstone {
            get {
                if (_chiseled_polished_blackstone == null)
                    _chiseled_polished_blackstone = SKBitmap.Decode((byte[])ResourceManager.GetObject("chiseled_polished_blackstone"))
                    .Copy(SKColorType.Rgba8888);
                return _chiseled_polished_blackstone;
            }
        }

        private static SKBitmap _chiseled_quartz_block = null;
        public static SKBitmap chiseled_quartz_block {
            get {
                if (_chiseled_quartz_block == null)
                    _chiseled_quartz_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("chiseled_quartz_block"))
                    .Copy(SKColorType.Rgba8888);
                return _chiseled_quartz_block;
            }
        }

        private static SKBitmap _chiseled_quartz_block_top = null;
        public static SKBitmap chiseled_quartz_block_top {
            get {
                if (_chiseled_quartz_block_top == null)
                    _chiseled_quartz_block_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("chiseled_quartz_block_top"))
                    .Copy(SKColorType.Rgba8888);
                return _chiseled_quartz_block_top;
            }
        }

        private static SKBitmap _clay = null;
        public static SKBitmap clay {
            get {
                if (_clay == null)
                    _clay = SKBitmap.Decode((byte[])ResourceManager.GetObject("clay"))
                    .Copy(SKColorType.Rgba8888);
                return _clay;
            }
        }

        private static SKBitmap _coal_block = null;
        public static SKBitmap coal_block {
            get {
                if (_coal_block == null)
                    _coal_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("coal_block"))
                    .Copy(SKColorType.Rgba8888);
                return _coal_block;
            }
        }

        private static SKBitmap _coal_ore = null;
        public static SKBitmap coal_ore {
            get {
                if (_coal_ore == null)
                    _coal_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("coal_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _coal_ore;
            }
        }

        private static SKBitmap _coarse_dirt = null;
        public static SKBitmap coarse_dirt {
            get {
                if (_coarse_dirt == null)
                    _coarse_dirt = SKBitmap.Decode((byte[])ResourceManager.GetObject("coarse_dirt"))
                    .Copy(SKColorType.Rgba8888);
                return _coarse_dirt;
            }
        }

        private static SKBitmap _cobbled_deepslate = null;
        public static SKBitmap cobbled_deepslate {
            get {
                if (_cobbled_deepslate == null)
                    _cobbled_deepslate = SKBitmap.Decode((byte[])ResourceManager.GetObject("cobbled_deepslate"))
                    .Copy(SKColorType.Rgba8888);
                return _cobbled_deepslate;
            }
        }

        private static SKBitmap _cobblestone = null;
        public static SKBitmap cobblestone {
            get {
                if (_cobblestone == null)
                    _cobblestone = SKBitmap.Decode((byte[])ResourceManager.GetObject("cobblestone"))
                    .Copy(SKColorType.Rgba8888);
                return _cobblestone;
            }
        }

        private static SKBitmap _copper_block = null;
        public static SKBitmap copper_block {
            get {
                if (_copper_block == null)
                    _copper_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("copper_block"))
                    .Copy(SKColorType.Rgba8888);
                return _copper_block;
            }
        }

        private static SKBitmap _copper_ore = null;
        public static SKBitmap copper_ore {
            get {
                if (_copper_ore == null)
                    _copper_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("copper_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _copper_ore;
            }
        }

        private static SKBitmap _cracked_deepslate_bricks = null;
        public static SKBitmap cracked_deepslate_bricks {
            get {
                if (_cracked_deepslate_bricks == null)
                    _cracked_deepslate_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("cracked_deepslate_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _cracked_deepslate_bricks;
            }
        }

        private static SKBitmap _cracked_deepslate_tiles = null;
        public static SKBitmap cracked_deepslate_tiles {
            get {
                if (_cracked_deepslate_tiles == null)
                    _cracked_deepslate_tiles = SKBitmap.Decode((byte[])ResourceManager.GetObject("cracked_deepslate_tiles"))
                    .Copy(SKColorType.Rgba8888);
                return _cracked_deepslate_tiles;
            }
        }

        private static SKBitmap _cracked_nether_bricks = null;
        public static SKBitmap cracked_nether_bricks {
            get {
                if (_cracked_nether_bricks == null)
                    _cracked_nether_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("cracked_nether_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _cracked_nether_bricks;
            }
        }

        private static SKBitmap _cracked_polished_blackstone_bricks = null;
        public static SKBitmap cracked_polished_blackstone_bricks {
            get {
                if (_cracked_polished_blackstone_bricks == null)
                    _cracked_polished_blackstone_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("cracked_polished_blackstone_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _cracked_polished_blackstone_bricks;
            }
        }

        private static SKBitmap _cracked_stone_bricks = null;
        public static SKBitmap cracked_stone_bricks {
            get {
                if (_cracked_stone_bricks == null)
                    _cracked_stone_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("cracked_stone_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _cracked_stone_bricks;
            }
        }

        private static SKBitmap _crimson_nylium = null;
        public static SKBitmap crimson_nylium {
            get {
                if (_crimson_nylium == null)
                    _crimson_nylium = SKBitmap.Decode((byte[])ResourceManager.GetObject("crimson_nylium"))
                    .Copy(SKColorType.Rgba8888);
                return _crimson_nylium;
            }
        }

        private static SKBitmap _crimson_nylium_side = null;
        public static SKBitmap crimson_nylium_side {
            get {
                if (_crimson_nylium_side == null)
                    _crimson_nylium_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("crimson_nylium_side"))
                    .Copy(SKColorType.Rgba8888);
                return _crimson_nylium_side;
            }
        }

        private static SKBitmap _crimson_planks = null;
        public static SKBitmap crimson_planks {
            get {
                if (_crimson_planks == null)
                    _crimson_planks = SKBitmap.Decode((byte[])ResourceManager.GetObject("crimson_planks"))
                    .Copy(SKColorType.Rgba8888);
                return _crimson_planks;
            }
        }

        private static SKBitmap _crimson_stem = null;
        public static SKBitmap crimson_stem {
            get {
                if (_crimson_stem == null)
                    _crimson_stem = SKBitmap.Decode((byte[])ResourceManager.GetObject("crimson_stem"))
                    .Copy(SKColorType.Rgba8888);
                return _crimson_stem;
            }
        }

        private static SKBitmap _crying_obsidian = null;
        public static SKBitmap crying_obsidian {
            get {
                if (_crying_obsidian == null)
                    _crying_obsidian = SKBitmap.Decode((byte[])ResourceManager.GetObject("crying_obsidian"))
                    .Copy(SKColorType.Rgba8888);
                return _crying_obsidian;
            }
        }

        private static SKBitmap _cut_copper = null;
        public static SKBitmap cut_copper {
            get {
                if (_cut_copper == null)
                    _cut_copper = SKBitmap.Decode((byte[])ResourceManager.GetObject("cut_copper"))
                    .Copy(SKColorType.Rgba8888);
                return _cut_copper;
            }
        }

        private static SKBitmap _cyan_concrete = null;
        public static SKBitmap cyan_concrete {
            get {
                if (_cyan_concrete == null)
                    _cyan_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("cyan_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _cyan_concrete;
            }
        }

        private static SKBitmap _cyan_concrete_powder = null;
        public static SKBitmap cyan_concrete_powder {
            get {
                if (_cyan_concrete_powder == null)
                    _cyan_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("cyan_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _cyan_concrete_powder;
            }
        }

        private static SKBitmap _cyan_glazed_terracotta = null;
        public static SKBitmap cyan_glazed_terracotta {
            get {
                if (_cyan_glazed_terracotta == null)
                    _cyan_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("cyan_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _cyan_glazed_terracotta;
            }
        }

        private static SKBitmap _cyan_stained_glass = null;
        public static SKBitmap cyan_stained_glass {
            get {
                if (_cyan_stained_glass == null)
                    _cyan_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("cyan_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _cyan_stained_glass;
            }
        }

        private static SKBitmap _cyan_terracotta = null;
        public static SKBitmap cyan_terracotta {
            get {
                if (_cyan_terracotta == null)
                    _cyan_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("cyan_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _cyan_terracotta;
            }
        }

        private static SKBitmap _cyan_wool = null;
        public static SKBitmap cyan_wool {
            get {
                if (_cyan_wool == null)
                    _cyan_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("cyan_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _cyan_wool;
            }
        }

        private static SKBitmap _dark_oak_log = null;
        public static SKBitmap dark_oak_log {
            get {
                if (_dark_oak_log == null)
                    _dark_oak_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("dark_oak_log"))
                    .Copy(SKColorType.Rgba8888);
                return _dark_oak_log;
            }
        }

        private static SKBitmap _dark_oak_planks = null;
        public static SKBitmap dark_oak_planks {
            get {
                if (_dark_oak_planks == null)
                    _dark_oak_planks = SKBitmap.Decode((byte[])ResourceManager.GetObject("dark_oak_planks"))
                    .Copy(SKColorType.Rgba8888);
                return _dark_oak_planks;
            }
        }

        private static SKBitmap _dark_prismarine = null;
        public static SKBitmap dark_prismarine {
            get {
                if (_dark_prismarine == null)
                    _dark_prismarine = SKBitmap.Decode((byte[])ResourceManager.GetObject("dark_prismarine"))
                    .Copy(SKColorType.Rgba8888);
                return _dark_prismarine;
            }
        }

        private static SKBitmap _dead_brain_coral_block = null;
        public static SKBitmap dead_brain_coral_block {
            get {
                if (_dead_brain_coral_block == null)
                    _dead_brain_coral_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("dead_brain_coral_block"))
                    .Copy(SKColorType.Rgba8888);
                return _dead_brain_coral_block;
            }
        }

        private static SKBitmap _dead_bubble_coral_block = null;
        public static SKBitmap dead_bubble_coral_block {
            get {
                if (_dead_bubble_coral_block == null)
                    _dead_bubble_coral_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("dead_bubble_coral_block"))
                    .Copy(SKColorType.Rgba8888);
                return _dead_bubble_coral_block;
            }
        }

        private static SKBitmap _dead_fire_coral_block = null;
        public static SKBitmap dead_fire_coral_block {
            get {
                if (_dead_fire_coral_block == null)
                    _dead_fire_coral_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("dead_fire_coral_block"))
                    .Copy(SKColorType.Rgba8888);
                return _dead_fire_coral_block;
            }
        }

        private static SKBitmap _dead_horn_coral_block = null;
        public static SKBitmap dead_horn_coral_block {
            get {
                if (_dead_horn_coral_block == null)
                    _dead_horn_coral_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("dead_horn_coral_block"))
                    .Copy(SKColorType.Rgba8888);
                return _dead_horn_coral_block;
            }
        }

        private static SKBitmap _dead_tube_coral_block = null;
        public static SKBitmap dead_tube_coral_block {
            get {
                if (_dead_tube_coral_block == null)
                    _dead_tube_coral_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("dead_tube_coral_block"))
                    .Copy(SKColorType.Rgba8888);
                return _dead_tube_coral_block;
            }
        }

        private static SKBitmap _deepslate = null;
        public static SKBitmap deepslate {
            get {
                if (_deepslate == null)
                    _deepslate = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate;
            }
        }

        private static SKBitmap _deepslate_bricks = null;
        public static SKBitmap deepslate_bricks {
            get {
                if (_deepslate_bricks == null)
                    _deepslate_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_bricks;
            }
        }

        private static SKBitmap _deepslate_coal_ore = null;
        public static SKBitmap deepslate_coal_ore {
            get {
                if (_deepslate_coal_ore == null)
                    _deepslate_coal_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_coal_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_coal_ore;
            }
        }

        private static SKBitmap _deepslate_copper_ore = null;
        public static SKBitmap deepslate_copper_ore {
            get {
                if (_deepslate_copper_ore == null)
                    _deepslate_copper_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_copper_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_copper_ore;
            }
        }

        private static SKBitmap _deepslate_diamond_ore = null;
        public static SKBitmap deepslate_diamond_ore {
            get {
                if (_deepslate_diamond_ore == null)
                    _deepslate_diamond_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_diamond_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_diamond_ore;
            }
        }

        private static SKBitmap _deepslate_emerald_ore = null;
        public static SKBitmap deepslate_emerald_ore {
            get {
                if (_deepslate_emerald_ore == null)
                    _deepslate_emerald_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_emerald_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_emerald_ore;
            }
        }

        private static SKBitmap _deepslate_gold_ore = null;
        public static SKBitmap deepslate_gold_ore {
            get {
                if (_deepslate_gold_ore == null)
                    _deepslate_gold_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_gold_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_gold_ore;
            }
        }

        private static SKBitmap _deepslate_iron_ore = null;
        public static SKBitmap deepslate_iron_ore {
            get {
                if (_deepslate_iron_ore == null)
                    _deepslate_iron_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_iron_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_iron_ore;
            }
        }

        private static SKBitmap _deepslate_lapis_ore = null;
        public static SKBitmap deepslate_lapis_ore {
            get {
                if (_deepslate_lapis_ore == null)
                    _deepslate_lapis_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_lapis_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_lapis_ore;
            }
        }

        private static SKBitmap _deepslate_redstone_ore = null;
        public static SKBitmap deepslate_redstone_ore {
            get {
                if (_deepslate_redstone_ore == null)
                    _deepslate_redstone_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_redstone_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_redstone_ore;
            }
        }

        private static SKBitmap _deepslate_tiles = null;
        public static SKBitmap deepslate_tiles {
            get {
                if (_deepslate_tiles == null)
                    _deepslate_tiles = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_tiles"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_tiles;
            }
        }

        private static SKBitmap _deepslate_top = null;
        public static SKBitmap deepslate_top {
            get {
                if (_deepslate_top == null)
                    _deepslate_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("deepslate_top"))
                    .Copy(SKColorType.Rgba8888);
                return _deepslate_top;
            }
        }

        private static SKBitmap _diamond_block = null;
        public static SKBitmap diamond_block {
            get {
                if (_diamond_block == null)
                    _diamond_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("diamond_block"))
                    .Copy(SKColorType.Rgba8888);
                return _diamond_block;
            }
        }

        private static SKBitmap _diamond_ore = null;
        public static SKBitmap diamond_ore {
            get {
                if (_diamond_ore == null)
                    _diamond_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("diamond_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _diamond_ore;
            }
        }

        private static SKBitmap _diorite = null;
        public static SKBitmap diorite {
            get {
                if (_diorite == null)
                    _diorite = SKBitmap.Decode((byte[])ResourceManager.GetObject("diorite"))
                    .Copy(SKColorType.Rgba8888);
                return _diorite;
            }
        }

        private static SKBitmap _dirt = null;
        public static SKBitmap dirt {
            get {
                if (_dirt == null)
                    _dirt = SKBitmap.Decode((byte[])ResourceManager.GetObject("dirt"))
                    .Copy(SKColorType.Rgba8888);
                return _dirt;
            }
        }

        private static SKBitmap _dirt_path_side = null;
        public static SKBitmap dirt_path_side {
            get {
                if (_dirt_path_side == null)
                    _dirt_path_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("dirt_path_side"))
                    .Copy(SKColorType.Rgba8888);
                return _dirt_path_side;
            }
        }

        private static SKBitmap _dirt_path_top = null;
        public static SKBitmap dirt_path_top {
            get {
                if (_dirt_path_top == null)
                    _dirt_path_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("dirt_path_top"))
                    .Copy(SKColorType.Rgba8888);
                return _dirt_path_top;
            }
        }

        private static SKBitmap _disabled = null;
        public static SKBitmap disabled {
            get {
                if (_disabled == null)
                    _disabled = SKBitmap.Decode((byte[])ResourceManager.GetObject("disabled"))
                    .Copy(SKColorType.Rgba8888);
                return _disabled;
            }
        }

        private static SKBitmap _dried_kelp_side = null;
        public static SKBitmap dried_kelp_side {
            get {
                if (_dried_kelp_side == null)
                    _dried_kelp_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("dried_kelp_side"))
                    .Copy(SKColorType.Rgba8888);
                return _dried_kelp_side;
            }
        }

        private static SKBitmap _dried_kelp_top = null;
        public static SKBitmap dried_kelp_top {
            get {
                if (_dried_kelp_top == null)
                    _dried_kelp_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("dried_kelp_top"))
                    .Copy(SKColorType.Rgba8888);
                return _dried_kelp_top;
            }
        }

        private static SKBitmap _dripstone_block = null;
        public static SKBitmap dripstone_block {
            get {
                if (_dripstone_block == null)
                    _dripstone_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("dripstone_block"))
                    .Copy(SKColorType.Rgba8888);
                return _dripstone_block;
            }
        }

        private static SKBitmap _emerald_block = null;
        public static SKBitmap emerald_block {
            get {
                if (_emerald_block == null)
                    _emerald_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("emerald_block"))
                    .Copy(SKColorType.Rgba8888);
                return _emerald_block;
            }
        }

        private static SKBitmap _emerald_ore = null;
        public static SKBitmap emerald_ore {
            get {
                if (_emerald_ore == null)
                    _emerald_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("emerald_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _emerald_ore;
            }
        }

        private static SKBitmap _end_stone = null;
        public static SKBitmap end_stone {
            get {
                if (_end_stone == null)
                    _end_stone = SKBitmap.Decode((byte[])ResourceManager.GetObject("end_stone"))
                    .Copy(SKColorType.Rgba8888);
                return _end_stone;
            }
        }

        private static SKBitmap _end_stone_bricks = null;
        public static SKBitmap end_stone_bricks {
            get {
                if (_end_stone_bricks == null)
                    _end_stone_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("end_stone_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _end_stone_bricks;
            }
        }

        private static SKBitmap _exposed_copper = null;
        public static SKBitmap exposed_copper {
            get {
                if (_exposed_copper == null)
                    _exposed_copper = SKBitmap.Decode((byte[])ResourceManager.GetObject("exposed_copper"))
                    .Copy(SKColorType.Rgba8888);
                return _exposed_copper;
            }
        }

        private static SKBitmap _exposed_cut_copper = null;
        public static SKBitmap exposed_cut_copper {
            get {
                if (_exposed_cut_copper == null)
                    _exposed_cut_copper = SKBitmap.Decode((byte[])ResourceManager.GetObject("exposed_cut_copper"))
                    .Copy(SKColorType.Rgba8888);
                return _exposed_cut_copper;
            }
        }

        private static SKBitmap _fire_coral_block = null;
        public static SKBitmap fire_coral_block {
            get {
                if (_fire_coral_block == null)
                    _fire_coral_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("fire_coral_block"))
                    .Copy(SKColorType.Rgba8888);
                return _fire_coral_block;
            }
        }

        private static SKBitmap _gilded_blackstone = null;
        public static SKBitmap gilded_blackstone {
            get {
                if (_gilded_blackstone == null)
                    _gilded_blackstone = SKBitmap.Decode((byte[])ResourceManager.GetObject("gilded_blackstone"))
                    .Copy(SKColorType.Rgba8888);
                return _gilded_blackstone;
            }
        }

        private static SKBitmap _glass = null;
        public static SKBitmap glass {
            get {
                if (_glass == null)
                    _glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("glass"))
                    .Copy(SKColorType.Rgba8888);
                return _glass;
            }
        }

        private static SKBitmap _glowstone = null;
        public static SKBitmap glowstone {
            get {
                if (_glowstone == null)
                    _glowstone = SKBitmap.Decode((byte[])ResourceManager.GetObject("glowstone"))
                    .Copy(SKColorType.Rgba8888);
                return _glowstone;
            }
        }

        private static SKBitmap _gold_block = null;
        public static SKBitmap gold_block {
            get {
                if (_gold_block == null)
                    _gold_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("gold_block"))
                    .Copy(SKColorType.Rgba8888);
                return _gold_block;
            }
        }

        private static SKBitmap _gold_ore = null;
        public static SKBitmap gold_ore {
            get {
                if (_gold_ore == null)
                    _gold_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("gold_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _gold_ore;
            }
        }

        private static SKBitmap _granite = null;
        public static SKBitmap granite {
            get {
                if (_granite == null)
                    _granite = SKBitmap.Decode((byte[])ResourceManager.GetObject("granite"))
                    .Copy(SKColorType.Rgba8888);
                return _granite;
            }
        }

        private static SKBitmap _gravel = null;
        public static SKBitmap gravel {
            get {
                if (_gravel == null)
                    _gravel = SKBitmap.Decode((byte[])ResourceManager.GetObject("gravel"))
                    .Copy(SKColorType.Rgba8888);
                return _gravel;
            }
        }

        private static SKBitmap _gray_concrete = null;
        public static SKBitmap gray_concrete {
            get {
                if (_gray_concrete == null)
                    _gray_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("gray_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _gray_concrete;
            }
        }

        private static SKBitmap _gray_concrete_powder = null;
        public static SKBitmap gray_concrete_powder {
            get {
                if (_gray_concrete_powder == null)
                    _gray_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("gray_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _gray_concrete_powder;
            }
        }

        private static SKBitmap _gray_glazed_terracotta = null;
        public static SKBitmap gray_glazed_terracotta {
            get {
                if (_gray_glazed_terracotta == null)
                    _gray_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("gray_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _gray_glazed_terracotta;
            }
        }

        private static SKBitmap _gray_stained_glass = null;
        public static SKBitmap gray_stained_glass {
            get {
                if (_gray_stained_glass == null)
                    _gray_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("gray_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _gray_stained_glass;
            }
        }

        private static SKBitmap _gray_terracotta = null;
        public static SKBitmap gray_terracotta {
            get {
                if (_gray_terracotta == null)
                    _gray_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("gray_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _gray_terracotta;
            }
        }

        private static SKBitmap _gray_wool = null;
        public static SKBitmap gray_wool {
            get {
                if (_gray_wool == null)
                    _gray_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("gray_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _gray_wool;
            }
        }

        private static SKBitmap _green_concrete = null;
        public static SKBitmap green_concrete {
            get {
                if (_green_concrete == null)
                    _green_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("green_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _green_concrete;
            }
        }

        private static SKBitmap _green_concrete_powder = null;
        public static SKBitmap green_concrete_powder {
            get {
                if (_green_concrete_powder == null)
                    _green_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("green_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _green_concrete_powder;
            }
        }

        private static SKBitmap _green_glazed_terracotta = null;
        public static SKBitmap green_glazed_terracotta {
            get {
                if (_green_glazed_terracotta == null)
                    _green_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("green_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _green_glazed_terracotta;
            }
        }

        private static SKBitmap _green_stained_glass = null;
        public static SKBitmap green_stained_glass {
            get {
                if (_green_stained_glass == null)
                    _green_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("green_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _green_stained_glass;
            }
        }

        private static SKBitmap _green_terracotta = null;
        public static SKBitmap green_terracotta {
            get {
                if (_green_terracotta == null)
                    _green_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("green_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _green_terracotta;
            }
        }

        private static SKBitmap _green_wool = null;
        public static SKBitmap green_wool {
            get {
                if (_green_wool == null)
                    _green_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("green_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _green_wool;
            }
        }

        private static SKBitmap _hay_block_side = null;
        public static SKBitmap hay_block_side {
            get {
                if (_hay_block_side == null)
                    _hay_block_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("hay_block_side"))
                    .Copy(SKColorType.Rgba8888);
                return _hay_block_side;
            }
        }

        private static SKBitmap _hay_block_top = null;
        public static SKBitmap hay_block_top {
            get {
                if (_hay_block_top == null)
                    _hay_block_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("hay_block_top"))
                    .Copy(SKColorType.Rgba8888);
                return _hay_block_top;
            }
        }

        private static SKBitmap _honeycomb_block = null;
        public static SKBitmap honeycomb_block {
            get {
                if (_honeycomb_block == null)
                    _honeycomb_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("honeycomb_block"))
                    .Copy(SKColorType.Rgba8888);
                return _honeycomb_block;
            }
        }

        private static SKBitmap _honey_block_side = null;
        public static SKBitmap honey_block_side {
            get {
                if (_honey_block_side == null)
                    _honey_block_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("honey_block_side"))
                    .Copy(SKColorType.Rgba8888);
                return _honey_block_side;
            }
        }

        private static SKBitmap _honey_block_top = null;
        public static SKBitmap honey_block_top {
            get {
                if (_honey_block_top == null)
                    _honey_block_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("honey_block_top"))
                    .Copy(SKColorType.Rgba8888);
                return _honey_block_top;
            }
        }

        private static SKBitmap _horn_coral_block = null;
        public static SKBitmap horn_coral_block {
            get {
                if (_horn_coral_block == null)
                    _horn_coral_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("horn_coral_block"))
                    .Copy(SKColorType.Rgba8888);
                return _horn_coral_block;
            }
        }

        private static SKBitmap _iron_block = null;
        public static SKBitmap iron_block {
            get {
                if (_iron_block == null)
                    _iron_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("iron_block"))
                    .Copy(SKColorType.Rgba8888);
                return _iron_block;
            }
        }

        private static SKBitmap _iron_ore = null;
        public static SKBitmap iron_ore {
            get {
                if (_iron_ore == null)
                    _iron_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("iron_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _iron_ore;
            }
        }

        private static SKBitmap _jack_o_lantern = null;
        public static SKBitmap jack_o_lantern {
            get {
                if (_jack_o_lantern == null)
                    _jack_o_lantern = SKBitmap.Decode((byte[])ResourceManager.GetObject("jack_o_lantern"))
                    .Copy(SKColorType.Rgba8888);
                return _jack_o_lantern;
            }
        }

        private static SKBitmap _jungle_log = null;
        public static SKBitmap jungle_log {
            get {
                if (_jungle_log == null)
                    _jungle_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("jungle_log"))
                    .Copy(SKColorType.Rgba8888);
                return _jungle_log;
            }
        }

        private static SKBitmap _jungle_planks = null;
        public static SKBitmap jungle_planks {
            get {
                if (_jungle_planks == null)
                    _jungle_planks = SKBitmap.Decode((byte[])ResourceManager.GetObject("jungle_planks"))
                    .Copy(SKColorType.Rgba8888);
                return _jungle_planks;
            }
        }

        private static SKBitmap _lapis_block = null;
        public static SKBitmap lapis_block {
            get {
                if (_lapis_block == null)
                    _lapis_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("lapis_block"))
                    .Copy(SKColorType.Rgba8888);
                return _lapis_block;
            }
        }

        private static SKBitmap _lapis_ore = null;
        public static SKBitmap lapis_ore {
            get {
                if (_lapis_ore == null)
                    _lapis_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("lapis_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _lapis_ore;
            }
        }

        private static SKBitmap _light_blue_concrete = null;
        public static SKBitmap light_blue_concrete {
            get {
                if (_light_blue_concrete == null)
                    _light_blue_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_blue_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _light_blue_concrete;
            }
        }

        private static SKBitmap _light_blue_concrete_powder = null;
        public static SKBitmap light_blue_concrete_powder {
            get {
                if (_light_blue_concrete_powder == null)
                    _light_blue_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_blue_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _light_blue_concrete_powder;
            }
        }

        private static SKBitmap _light_blue_glazed_terracotta = null;
        public static SKBitmap light_blue_glazed_terracotta {
            get {
                if (_light_blue_glazed_terracotta == null)
                    _light_blue_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_blue_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _light_blue_glazed_terracotta;
            }
        }

        private static SKBitmap _light_blue_stained_glass = null;
        public static SKBitmap light_blue_stained_glass {
            get {
                if (_light_blue_stained_glass == null)
                    _light_blue_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_blue_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _light_blue_stained_glass;
            }
        }

        private static SKBitmap _light_blue_terracotta = null;
        public static SKBitmap light_blue_terracotta {
            get {
                if (_light_blue_terracotta == null)
                    _light_blue_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_blue_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _light_blue_terracotta;
            }
        }

        private static SKBitmap _light_blue_wool = null;
        public static SKBitmap light_blue_wool {
            get {
                if (_light_blue_wool == null)
                    _light_blue_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_blue_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _light_blue_wool;
            }
        }

        private static SKBitmap _light_gray_concrete = null;
        public static SKBitmap light_gray_concrete {
            get {
                if (_light_gray_concrete == null)
                    _light_gray_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_gray_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _light_gray_concrete;
            }
        }

        private static SKBitmap _light_gray_concrete_powder = null;
        public static SKBitmap light_gray_concrete_powder {
            get {
                if (_light_gray_concrete_powder == null)
                    _light_gray_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_gray_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _light_gray_concrete_powder;
            }
        }

        private static SKBitmap _light_gray_glazed_terracotta = null;
        public static SKBitmap light_gray_glazed_terracotta {
            get {
                if (_light_gray_glazed_terracotta == null)
                    _light_gray_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_gray_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _light_gray_glazed_terracotta;
            }
        }

        private static SKBitmap _light_gray_stained_glass = null;
        public static SKBitmap light_gray_stained_glass {
            get {
                if (_light_gray_stained_glass == null)
                    _light_gray_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_gray_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _light_gray_stained_glass;
            }
        }

        private static SKBitmap _light_gray_terracotta = null;
        public static SKBitmap light_gray_terracotta {
            get {
                if (_light_gray_terracotta == null)
                    _light_gray_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_gray_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _light_gray_terracotta;
            }
        }

        private static SKBitmap _light_gray_wool = null;
        public static SKBitmap light_gray_wool {
            get {
                if (_light_gray_wool == null)
                    _light_gray_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("light_gray_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _light_gray_wool;
            }
        }

        private static SKBitmap _lime_concrete = null;
        public static SKBitmap lime_concrete {
            get {
                if (_lime_concrete == null)
                    _lime_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("lime_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _lime_concrete;
            }
        }

        private static SKBitmap _lime_concrete_powder = null;
        public static SKBitmap lime_concrete_powder {
            get {
                if (_lime_concrete_powder == null)
                    _lime_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("lime_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _lime_concrete_powder;
            }
        }

        private static SKBitmap _lime_glazed_terracotta = null;
        public static SKBitmap lime_glazed_terracotta {
            get {
                if (_lime_glazed_terracotta == null)
                    _lime_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("lime_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _lime_glazed_terracotta;
            }
        }

        private static SKBitmap _lime_stained_glass = null;
        public static SKBitmap lime_stained_glass {
            get {
                if (_lime_stained_glass == null)
                    _lime_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("lime_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _lime_stained_glass;
            }
        }

        private static SKBitmap _lime_terracotta = null;
        public static SKBitmap lime_terracotta {
            get {
                if (_lime_terracotta == null)
                    _lime_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("lime_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _lime_terracotta;
            }
        }

        private static SKBitmap _lime_wool = null;
        public static SKBitmap lime_wool {
            get {
                if (_lime_wool == null)
                    _lime_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("lime_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _lime_wool;
            }
        }

        private static SKBitmap _lodestone_side = null;
        public static SKBitmap lodestone_side {
            get {
                if (_lodestone_side == null)
                    _lodestone_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("lodestone_side"))
                    .Copy(SKColorType.Rgba8888);
                return _lodestone_side;
            }
        }

        private static SKBitmap _lodestone_top = null;
        public static SKBitmap lodestone_top {
            get {
                if (_lodestone_top == null)
                    _lodestone_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("lodestone_top"))
                    .Copy(SKColorType.Rgba8888);
                return _lodestone_top;
            }
        }

        private static SKBitmap _magenta_concrete = null;
        public static SKBitmap magenta_concrete {
            get {
                if (_magenta_concrete == null)
                    _magenta_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("magenta_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _magenta_concrete;
            }
        }

        private static SKBitmap _magenta_concrete_powder = null;
        public static SKBitmap magenta_concrete_powder {
            get {
                if (_magenta_concrete_powder == null)
                    _magenta_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("magenta_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _magenta_concrete_powder;
            }
        }

        private static SKBitmap _magenta_glazed_terracotta = null;
        public static SKBitmap magenta_glazed_terracotta {
            get {
                if (_magenta_glazed_terracotta == null)
                    _magenta_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("magenta_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _magenta_glazed_terracotta;
            }
        }

        private static SKBitmap _magenta_stained_glass = null;
        public static SKBitmap magenta_stained_glass {
            get {
                if (_magenta_stained_glass == null)
                    _magenta_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("magenta_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _magenta_stained_glass;
            }
        }

        private static SKBitmap _magenta_terracotta = null;
        public static SKBitmap magenta_terracotta {
            get {
                if (_magenta_terracotta == null)
                    _magenta_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("magenta_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _magenta_terracotta;
            }
        }

        private static SKBitmap _magenta_wool = null;
        public static SKBitmap magenta_wool {
            get {
                if (_magenta_wool == null)
                    _magenta_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("magenta_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _magenta_wool;
            }
        }

        private static SKBitmap _magma = null;
        public static SKBitmap magma {
            get {
                if (_magma == null)
                    _magma = SKBitmap.Decode((byte[])ResourceManager.GetObject("magma"))
                    .Copy(SKColorType.Rgba8888);
                return _magma;
            }
        }

        private static SKBitmap _mangrove_log = null;
        public static SKBitmap mangrove_log {
            get {
                if (_mangrove_log == null)
                    _mangrove_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("mangrove_log"))
                    .Copy(SKColorType.Rgba8888);
                return _mangrove_log;
            }
        }

        private static SKBitmap _mangrove_planks = null;
        public static SKBitmap mangrove_planks {
            get {
                if (_mangrove_planks == null)
                    _mangrove_planks = SKBitmap.Decode((byte[])ResourceManager.GetObject("mangrove_planks"))
                    .Copy(SKColorType.Rgba8888);
                return _mangrove_planks;
            }
        }

        private static SKBitmap _melon_side = null;
        public static SKBitmap melon_side {
            get {
                if (_melon_side == null)
                    _melon_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("melon_side"))
                    .Copy(SKColorType.Rgba8888);
                return _melon_side;
            }
        }

        private static SKBitmap _melon_top = null;
        public static SKBitmap melon_top {
            get {
                if (_melon_top == null)
                    _melon_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("melon_top"))
                    .Copy(SKColorType.Rgba8888);
                return _melon_top;
            }
        }

        private static SKBitmap _mossy_cobblestone = null;
        public static SKBitmap mossy_cobblestone {
            get {
                if (_mossy_cobblestone == null)
                    _mossy_cobblestone = SKBitmap.Decode((byte[])ResourceManager.GetObject("mossy_cobblestone"))
                    .Copy(SKColorType.Rgba8888);
                return _mossy_cobblestone;
            }
        }

        private static SKBitmap _mossy_stone_bricks = null;
        public static SKBitmap mossy_stone_bricks {
            get {
                if (_mossy_stone_bricks == null)
                    _mossy_stone_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("mossy_stone_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _mossy_stone_bricks;
            }
        }

        private static SKBitmap _moss_block = null;
        public static SKBitmap moss_block {
            get {
                if (_moss_block == null)
                    _moss_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("moss_block"))
                    .Copy(SKColorType.Rgba8888);
                return _moss_block;
            }
        }

        private static SKBitmap _mud = null;
        public static SKBitmap mud {
            get {
                if (_mud == null)
                    _mud = SKBitmap.Decode((byte[])ResourceManager.GetObject("mud"))
                    .Copy(SKColorType.Rgba8888);
                return _mud;
            }
        }

        private static SKBitmap _muddy_mangrove_roots_top = null;
        public static SKBitmap muddy_mangrove_roots_top {
            get {
                if (_muddy_mangrove_roots_top == null)
                    _muddy_mangrove_roots_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("muddy_mangrove_roots_top"))
                    .Copy(SKColorType.Rgba8888);
                return _muddy_mangrove_roots_top;
            }
        }

        private static SKBitmap _mud_bricks = null;
        public static SKBitmap mud_bricks {
            get {
                if (_mud_bricks == null)
                    _mud_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("mud_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _mud_bricks;
            }
        }

        private static SKBitmap _mushroom_block_inside = null;
        public static SKBitmap mushroom_block_inside {
            get {
                if (_mushroom_block_inside == null)
                    _mushroom_block_inside = SKBitmap.Decode((byte[])ResourceManager.GetObject("mushroom_block_inside"))
                    .Copy(SKColorType.Rgba8888);
                return _mushroom_block_inside;
            }
        }

        private static SKBitmap _mushroom_stem = null;
        public static SKBitmap mushroom_stem {
            get {
                if (_mushroom_stem == null)
                    _mushroom_stem = SKBitmap.Decode((byte[])ResourceManager.GetObject("mushroom_stem"))
                    .Copy(SKColorType.Rgba8888);
                return _mushroom_stem;
            }
        }

        private static SKBitmap _netherite_block = null;
        public static SKBitmap netherite_block {
            get {
                if (_netherite_block == null)
                    _netherite_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("netherite_block"))
                    .Copy(SKColorType.Rgba8888);
                return _netherite_block;
            }
        }

        private static SKBitmap _netherrack = null;
        public static SKBitmap netherrack {
            get {
                if (_netherrack == null)
                    _netherrack = SKBitmap.Decode((byte[])ResourceManager.GetObject("netherrack"))
                    .Copy(SKColorType.Rgba8888);
                return _netherrack;
            }
        }

        private static SKBitmap _nether_bricks = null;
        public static SKBitmap nether_bricks {
            get {
                if (_nether_bricks == null)
                    _nether_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("nether_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _nether_bricks;
            }
        }

        private static SKBitmap _nether_gold_ore = null;
        public static SKBitmap nether_gold_ore {
            get {
                if (_nether_gold_ore == null)
                    _nether_gold_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("nether_gold_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _nether_gold_ore;
            }
        }

        private static SKBitmap _nether_quartz_ore = null;
        public static SKBitmap nether_quartz_ore {
            get {
                if (_nether_quartz_ore == null)
                    _nether_quartz_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("nether_quartz_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _nether_quartz_ore;
            }
        }

        private static SKBitmap _nether_wart_block = null;
        public static SKBitmap nether_wart_block {
            get {
                if (_nether_wart_block == null)
                    _nether_wart_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("nether_wart_block"))
                    .Copy(SKColorType.Rgba8888);
                return _nether_wart_block;
            }
        }

        private static SKBitmap _note_block = null;
        public static SKBitmap note_block {
            get {
                if (_note_block == null)
                    _note_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("note_block"))
                    .Copy(SKColorType.Rgba8888);
                return _note_block;
            }
        }

        private static SKBitmap _oak_log = null;
        public static SKBitmap oak_log {
            get {
                if (_oak_log == null)
                    _oak_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("oak_log"))
                    .Copy(SKColorType.Rgba8888);
                return _oak_log;
            }
        }

        private static SKBitmap _oak_planks = null;
        public static SKBitmap oak_planks {
            get {
                if (_oak_planks == null)
                    _oak_planks = SKBitmap.Decode((byte[])ResourceManager.GetObject("oak_planks"))
                    .Copy(SKColorType.Rgba8888);
                return _oak_planks;
            }
        }

        private static SKBitmap _obsidian = null;
        public static SKBitmap obsidian {
            get {
                if (_obsidian == null)
                    _obsidian = SKBitmap.Decode((byte[])ResourceManager.GetObject("obsidian"))
                    .Copy(SKColorType.Rgba8888);
                return _obsidian;
            }
        }

        private static SKBitmap _ochre_froglight_side = null;
        public static SKBitmap ochre_froglight_side {
            get {
                if (_ochre_froglight_side == null)
                    _ochre_froglight_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("ochre_froglight_side"))
                    .Copy(SKColorType.Rgba8888);
                return _ochre_froglight_side;
            }
        }

        private static SKBitmap _ochre_froglight_top = null;
        public static SKBitmap ochre_froglight_top {
            get {
                if (_ochre_froglight_top == null)
                    _ochre_froglight_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("ochre_froglight_top"))
                    .Copy(SKColorType.Rgba8888);
                return _ochre_froglight_top;
            }
        }

        private static SKBitmap _orange_concrete = null;
        public static SKBitmap orange_concrete {
            get {
                if (_orange_concrete == null)
                    _orange_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("orange_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _orange_concrete;
            }
        }

        private static SKBitmap _orange_concrete_powder = null;
        public static SKBitmap orange_concrete_powder {
            get {
                if (_orange_concrete_powder == null)
                    _orange_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("orange_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _orange_concrete_powder;
            }
        }

        private static SKBitmap _orange_glazed_terracotta = null;
        public static SKBitmap orange_glazed_terracotta {
            get {
                if (_orange_glazed_terracotta == null)
                    _orange_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("orange_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _orange_glazed_terracotta;
            }
        }

        private static SKBitmap _orange_stained_glass = null;
        public static SKBitmap orange_stained_glass {
            get {
                if (_orange_stained_glass == null)
                    _orange_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("orange_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _orange_stained_glass;
            }
        }

        private static SKBitmap _orange_terracotta = null;
        public static SKBitmap orange_terracotta {
            get {
                if (_orange_terracotta == null)
                    _orange_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("orange_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _orange_terracotta;
            }
        }

        private static SKBitmap _orange_wool = null;
        public static SKBitmap orange_wool {
            get {
                if (_orange_wool == null)
                    _orange_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("orange_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _orange_wool;
            }
        }

        private static SKBitmap _oxidized_copper = null;
        public static SKBitmap oxidized_copper {
            get {
                if (_oxidized_copper == null)
                    _oxidized_copper = SKBitmap.Decode((byte[])ResourceManager.GetObject("oxidized_copper"))
                    .Copy(SKColorType.Rgba8888);
                return _oxidized_copper;
            }
        }

        private static SKBitmap _oxidized_cut_copper = null;
        public static SKBitmap oxidized_cut_copper {
            get {
                if (_oxidized_cut_copper == null)
                    _oxidized_cut_copper = SKBitmap.Decode((byte[])ResourceManager.GetObject("oxidized_cut_copper"))
                    .Copy(SKColorType.Rgba8888);
                return _oxidized_cut_copper;
            }
        }

        private static SKBitmap _packed_ice = null;
        public static SKBitmap packed_ice {
            get {
                if (_packed_ice == null)
                    _packed_ice = SKBitmap.Decode((byte[])ResourceManager.GetObject("packed_ice"))
                    .Copy(SKColorType.Rgba8888);
                return _packed_ice;
            }
        }

        private static SKBitmap _packed_mud = null;
        public static SKBitmap packed_mud {
            get {
                if (_packed_mud == null)
                    _packed_mud = SKBitmap.Decode((byte[])ResourceManager.GetObject("packed_mud"))
                    .Copy(SKColorType.Rgba8888);
                return _packed_mud;
            }
        }

        private static SKBitmap _pearlescent_froglight_side = null;
        public static SKBitmap pearlescent_froglight_side {
            get {
                if (_pearlescent_froglight_side == null)
                    _pearlescent_froglight_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("pearlescent_froglight_side"))
                    .Copy(SKColorType.Rgba8888);
                return _pearlescent_froglight_side;
            }
        }

        private static SKBitmap _pearlescent_froglight_top = null;
        public static SKBitmap pearlescent_froglight_top {
            get {
                if (_pearlescent_froglight_top == null)
                    _pearlescent_froglight_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("pearlescent_froglight_top"))
                    .Copy(SKColorType.Rgba8888);
                return _pearlescent_froglight_top;
            }
        }

        private static SKBitmap _pink_concrete = null;
        public static SKBitmap pink_concrete {
            get {
                if (_pink_concrete == null)
                    _pink_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("pink_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _pink_concrete;
            }
        }

        private static SKBitmap _pink_concrete_powder = null;
        public static SKBitmap pink_concrete_powder {
            get {
                if (_pink_concrete_powder == null)
                    _pink_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("pink_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _pink_concrete_powder;
            }
        }

        private static SKBitmap _pink_glazed_terracotta = null;
        public static SKBitmap pink_glazed_terracotta {
            get {
                if (_pink_glazed_terracotta == null)
                    _pink_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("pink_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _pink_glazed_terracotta;
            }
        }

        private static SKBitmap _pink_stained_glass = null;
        public static SKBitmap pink_stained_glass {
            get {
                if (_pink_stained_glass == null)
                    _pink_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("pink_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _pink_stained_glass;
            }
        }

        private static SKBitmap _pink_terracotta = null;
        public static SKBitmap pink_terracotta {
            get {
                if (_pink_terracotta == null)
                    _pink_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("pink_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _pink_terracotta;
            }
        }

        private static SKBitmap _pink_wool = null;
        public static SKBitmap pink_wool {
            get {
                if (_pink_wool == null)
                    _pink_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("pink_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _pink_wool;
            }
        }

        private static SKBitmap _podzol_side = null;
        public static SKBitmap podzol_side {
            get {
                if (_podzol_side == null)
                    _podzol_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("podzol_side"))
                    .Copy(SKColorType.Rgba8888);
                return _podzol_side;
            }
        }

        private static SKBitmap _podzol_top = null;
        public static SKBitmap podzol_top {
            get {
                if (_podzol_top == null)
                    _podzol_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("podzol_top"))
                    .Copy(SKColorType.Rgba8888);
                return _podzol_top;
            }
        }

        private static SKBitmap _polished_andesite = null;
        public static SKBitmap polished_andesite {
            get {
                if (_polished_andesite == null)
                    _polished_andesite = SKBitmap.Decode((byte[])ResourceManager.GetObject("polished_andesite"))
                    .Copy(SKColorType.Rgba8888);
                return _polished_andesite;
            }
        }

        private static SKBitmap _polished_basalt_side = null;
        public static SKBitmap polished_basalt_side {
            get {
                if (_polished_basalt_side == null)
                    _polished_basalt_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("polished_basalt_side"))
                    .Copy(SKColorType.Rgba8888);
                return _polished_basalt_side;
            }
        }

        private static SKBitmap _polished_basalt_top = null;
        public static SKBitmap polished_basalt_top {
            get {
                if (_polished_basalt_top == null)
                    _polished_basalt_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("polished_basalt_top"))
                    .Copy(SKColorType.Rgba8888);
                return _polished_basalt_top;
            }
        }

        private static SKBitmap _polished_blackstone = null;
        public static SKBitmap polished_blackstone {
            get {
                if (_polished_blackstone == null)
                    _polished_blackstone = SKBitmap.Decode((byte[])ResourceManager.GetObject("polished_blackstone"))
                    .Copy(SKColorType.Rgba8888);
                return _polished_blackstone;
            }
        }

        private static SKBitmap _polished_blackstone_bricks = null;
        public static SKBitmap polished_blackstone_bricks {
            get {
                if (_polished_blackstone_bricks == null)
                    _polished_blackstone_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("polished_blackstone_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _polished_blackstone_bricks;
            }
        }

        private static SKBitmap _polished_deepslate = null;
        public static SKBitmap polished_deepslate {
            get {
                if (_polished_deepslate == null)
                    _polished_deepslate = SKBitmap.Decode((byte[])ResourceManager.GetObject("polished_deepslate"))
                    .Copy(SKColorType.Rgba8888);
                return _polished_deepslate;
            }
        }

        private static SKBitmap _polished_diorite = null;
        public static SKBitmap polished_diorite {
            get {
                if (_polished_diorite == null)
                    _polished_diorite = SKBitmap.Decode((byte[])ResourceManager.GetObject("polished_diorite"))
                    .Copy(SKColorType.Rgba8888);
                return _polished_diorite;
            }
        }

        private static SKBitmap _polished_granite = null;
        public static SKBitmap polished_granite {
            get {
                if (_polished_granite == null)
                    _polished_granite = SKBitmap.Decode((byte[])ResourceManager.GetObject("polished_granite"))
                    .Copy(SKColorType.Rgba8888);
                return _polished_granite;
            }
        }

        private static SKBitmap _prismarine = null;
        public static SKBitmap prismarine {
            get {
                if (_prismarine == null)
                    _prismarine = SKBitmap.Decode((byte[])ResourceManager.GetObject("prismarine"))
                    .Copy(SKColorType.Rgba8888);
                return _prismarine;
            }
        }

        private static SKBitmap _prismarine_bricks = null;
        public static SKBitmap prismarine_bricks {
            get {
                if (_prismarine_bricks == null)
                    _prismarine_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("prismarine_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _prismarine_bricks;
            }
        }

        private static SKBitmap _pumpkin_side = null;
        public static SKBitmap pumpkin_side {
            get {
                if (_pumpkin_side == null)
                    _pumpkin_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("pumpkin_side"))
                    .Copy(SKColorType.Rgba8888);
                return _pumpkin_side;
            }
        }

        private static SKBitmap _pumpkin_top = null;
        public static SKBitmap pumpkin_top {
            get {
                if (_pumpkin_top == null)
                    _pumpkin_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("pumpkin_top"))
                    .Copy(SKColorType.Rgba8888);
                return _pumpkin_top;
            }
        }

        private static SKBitmap _purple_concrete = null;
        public static SKBitmap purple_concrete {
            get {
                if (_purple_concrete == null)
                    _purple_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("purple_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _purple_concrete;
            }
        }

        private static SKBitmap _purple_concrete_powder = null;
        public static SKBitmap purple_concrete_powder {
            get {
                if (_purple_concrete_powder == null)
                    _purple_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("purple_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _purple_concrete_powder;
            }
        }

        private static SKBitmap _purple_glazed_terracotta = null;
        public static SKBitmap purple_glazed_terracotta {
            get {
                if (_purple_glazed_terracotta == null)
                    _purple_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("purple_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _purple_glazed_terracotta;
            }
        }

        private static SKBitmap _purple_stained_glass = null;
        public static SKBitmap purple_stained_glass {
            get {
                if (_purple_stained_glass == null)
                    _purple_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("purple_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _purple_stained_glass;
            }
        }

        private static SKBitmap _purple_terracotta = null;
        public static SKBitmap purple_terracotta {
            get {
                if (_purple_terracotta == null)
                    _purple_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("purple_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _purple_terracotta;
            }
        }

        private static SKBitmap _purple_wool = null;
        public static SKBitmap purple_wool {
            get {
                if (_purple_wool == null)
                    _purple_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("purple_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _purple_wool;
            }
        }

        private static SKBitmap _purpur_block = null;
        public static SKBitmap purpur_block {
            get {
                if (_purpur_block == null)
                    _purpur_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("purpur_block"))
                    .Copy(SKColorType.Rgba8888);
                return _purpur_block;
            }
        }

        private static SKBitmap _purpur_pillar = null;
        public static SKBitmap purpur_pillar {
            get {
                if (_purpur_pillar == null)
                    _purpur_pillar = SKBitmap.Decode((byte[])ResourceManager.GetObject("purpur_pillar"))
                    .Copy(SKColorType.Rgba8888);
                return _purpur_pillar;
            }
        }

        private static SKBitmap _purpur_pillar_top = null;
        public static SKBitmap purpur_pillar_top {
            get {
                if (_purpur_pillar_top == null)
                    _purpur_pillar_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("purpur_pillar_top"))
                    .Copy(SKColorType.Rgba8888);
                return _purpur_pillar_top;
            }
        }

        private static SKBitmap _quartz_block_bottom = null;
        public static SKBitmap quartz_block_bottom {
            get {
                if (_quartz_block_bottom == null)
                    _quartz_block_bottom = SKBitmap.Decode((byte[])ResourceManager.GetObject("quartz_block_bottom"))
                    .Copy(SKColorType.Rgba8888);
                return _quartz_block_bottom;
            }
        }

        private static SKBitmap _quartz_block_top = null;
        public static SKBitmap quartz_block_top {
            get {
                if (_quartz_block_top == null)
                    _quartz_block_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("quartz_block_top"))
                    .Copy(SKColorType.Rgba8888);
                return _quartz_block_top;
            }
        }

        private static SKBitmap _quartz_bricks = null;
        public static SKBitmap quartz_bricks {
            get {
                if (_quartz_bricks == null)
                    _quartz_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("quartz_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _quartz_bricks;
            }
        }

        private static SKBitmap _raw_copper_block = null;
        public static SKBitmap raw_copper_block {
            get {
                if (_raw_copper_block == null)
                    _raw_copper_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("raw_copper_block"))
                    .Copy(SKColorType.Rgba8888);
                return _raw_copper_block;
            }
        }

        private static SKBitmap _raw_gold_block = null;
        public static SKBitmap raw_gold_block {
            get {
                if (_raw_gold_block == null)
                    _raw_gold_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("raw_gold_block"))
                    .Copy(SKColorType.Rgba8888);
                return _raw_gold_block;
            }
        }

        private static SKBitmap _raw_iron_block = null;
        public static SKBitmap raw_iron_block {
            get {
                if (_raw_iron_block == null)
                    _raw_iron_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("raw_iron_block"))
                    .Copy(SKColorType.Rgba8888);
                return _raw_iron_block;
            }
        }

        private static SKBitmap _redstone_block = null;
        public static SKBitmap redstone_block {
            get {
                if (_redstone_block == null)
                    _redstone_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("redstone_block"))
                    .Copy(SKColorType.Rgba8888);
                return _redstone_block;
            }
        }

        private static SKBitmap _redstone_lamp = null;
        public static SKBitmap redstone_lamp {
            get {
                if (_redstone_lamp == null)
                    _redstone_lamp = SKBitmap.Decode((byte[])ResourceManager.GetObject("redstone_lamp"))
                    .Copy(SKColorType.Rgba8888);
                return _redstone_lamp;
            }
        }

        private static SKBitmap _redstone_ore = null;
        public static SKBitmap redstone_ore {
            get {
                if (_redstone_ore == null)
                    _redstone_ore = SKBitmap.Decode((byte[])ResourceManager.GetObject("redstone_ore"))
                    .Copy(SKColorType.Rgba8888);
                return _redstone_ore;
            }
        }

        private static SKBitmap _red_concrete = null;
        public static SKBitmap red_concrete {
            get {
                if (_red_concrete == null)
                    _red_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("red_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _red_concrete;
            }
        }

        private static SKBitmap _red_concrete_powder = null;
        public static SKBitmap red_concrete_powder {
            get {
                if (_red_concrete_powder == null)
                    _red_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("red_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _red_concrete_powder;
            }
        }

        private static SKBitmap _red_glazed_terracotta = null;
        public static SKBitmap red_glazed_terracotta {
            get {
                if (_red_glazed_terracotta == null)
                    _red_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("red_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _red_glazed_terracotta;
            }
        }

        private static SKBitmap _red_mushroom_block = null;
        public static SKBitmap red_mushroom_block {
            get {
                if (_red_mushroom_block == null)
                    _red_mushroom_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("red_mushroom_block"))
                    .Copy(SKColorType.Rgba8888);
                return _red_mushroom_block;
            }
        }

        private static SKBitmap _red_nether_bricks = null;
        public static SKBitmap red_nether_bricks {
            get {
                if (_red_nether_bricks == null)
                    _red_nether_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("red_nether_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _red_nether_bricks;
            }
        }

        private static SKBitmap _red_sand = null;
        public static SKBitmap red_sand {
            get {
                if (_red_sand == null)
                    _red_sand = SKBitmap.Decode((byte[])ResourceManager.GetObject("red_sand"))
                    .Copy(SKColorType.Rgba8888);
                return _red_sand;
            }
        }

        private static SKBitmap _red_sandstone_top = null;
        public static SKBitmap red_sandstone_top {
            get {
                if (_red_sandstone_top == null)
                    _red_sandstone_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("red_sandstone_top"))
                    .Copy(SKColorType.Rgba8888);
                return _red_sandstone_top;
            }
        }

        private static SKBitmap _red_stained_glass = null;
        public static SKBitmap red_stained_glass {
            get {
                if (_red_stained_glass == null)
                    _red_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("red_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _red_stained_glass;
            }
        }

        private static SKBitmap _red_terracotta = null;
        public static SKBitmap red_terracotta {
            get {
                if (_red_terracotta == null)
                    _red_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("red_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _red_terracotta;
            }
        }

        private static SKBitmap _red_wool = null;
        public static SKBitmap red_wool {
            get {
                if (_red_wool == null)
                    _red_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("red_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _red_wool;
            }
        }

        private static SKBitmap _reinforced_deepslate_side = null;
        public static SKBitmap reinforced_deepslate_side {
            get {
                if (_reinforced_deepslate_side == null)
                    _reinforced_deepslate_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("reinforced_deepslate_side"))
                    .Copy(SKColorType.Rgba8888);
                return _reinforced_deepslate_side;
            }
        }

        private static SKBitmap _reinforced_deepslate_top = null;
        public static SKBitmap reinforced_deepslate_top {
            get {
                if (_reinforced_deepslate_top == null)
                    _reinforced_deepslate_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("reinforced_deepslate_top"))
                    .Copy(SKColorType.Rgba8888);
                return _reinforced_deepslate_top;
            }
        }

        private static SKBitmap _rooted_dirt = null;
        public static SKBitmap rooted_dirt {
            get {
                if (_rooted_dirt == null)
                    _rooted_dirt = SKBitmap.Decode((byte[])ResourceManager.GetObject("rooted_dirt"))
                    .Copy(SKColorType.Rgba8888);
                return _rooted_dirt;
            }
        }

        private static SKBitmap _sand = null;
        public static SKBitmap sand {
            get {
                if (_sand == null)
                    _sand = SKBitmap.Decode((byte[])ResourceManager.GetObject("sand"))
                    .Copy(SKColorType.Rgba8888);
                return _sand;
            }
        }

        private static SKBitmap _sandstone_top = null;
        public static SKBitmap sandstone_top {
            get {
                if (_sandstone_top == null)
                    _sandstone_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("sandstone_top"))
                    .Copy(SKColorType.Rgba8888);
                return _sandstone_top;
            }
        }

        private static SKBitmap _sculk_catalyst_top = null;
        public static SKBitmap sculk_catalyst_top {
            get {
                if (_sculk_catalyst_top == null)
                    _sculk_catalyst_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("sculk_catalyst_top"))
                    .Copy(SKColorType.Rgba8888);
                return _sculk_catalyst_top;
            }
        }

        private static SKBitmap _sea_lantern = null;
        public static SKBitmap sea_lantern {
            get {
                if (_sea_lantern == null)
                    _sea_lantern = SKBitmap.Decode((byte[])ResourceManager.GetObject("sea_lantern"))
                    .Copy(SKColorType.Rgba8888);
                return _sea_lantern;
            }
        }

        private static SKBitmap _shroomlight = null;
        public static SKBitmap shroomlight {
            get {
                if (_shroomlight == null)
                    _shroomlight = SKBitmap.Decode((byte[])ResourceManager.GetObject("shroomlight"))
                    .Copy(SKColorType.Rgba8888);
                return _shroomlight;
            }
        }

        private static SKBitmap _slime_block = null;
        public static SKBitmap slime_block {
            get {
                if (_slime_block == null)
                    _slime_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("slime_block"))
                    .Copy(SKColorType.Rgba8888);
                return _slime_block;
            }
        }

        private static SKBitmap _smoker_front = null;
        public static SKBitmap smoker_front {
            get {
                if (_smoker_front == null)
                    _smoker_front = SKBitmap.Decode((byte[])ResourceManager.GetObject("smoker_front"))
                    .Copy(SKColorType.Rgba8888);
                return _smoker_front;
            }
        }

        private static SKBitmap _smoker_side = null;
        public static SKBitmap smoker_side {
            get {
                if (_smoker_side == null)
                    _smoker_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("smoker_side"))
                    .Copy(SKColorType.Rgba8888);
                return _smoker_side;
            }
        }

        private static SKBitmap _smoker_top = null;
        public static SKBitmap smoker_top {
            get {
                if (_smoker_top == null)
                    _smoker_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("smoker_top"))
                    .Copy(SKColorType.Rgba8888);
                return _smoker_top;
            }
        }

        private static SKBitmap _smooth_basalt = null;
        public static SKBitmap smooth_basalt {
            get {
                if (_smooth_basalt == null)
                    _smooth_basalt = SKBitmap.Decode((byte[])ResourceManager.GetObject("smooth_basalt"))
                    .Copy(SKColorType.Rgba8888);
                return _smooth_basalt;
            }
        }

        private static SKBitmap _smooth_stone = null;
        public static SKBitmap smooth_stone {
            get {
                if (_smooth_stone == null)
                    _smooth_stone = SKBitmap.Decode((byte[])ResourceManager.GetObject("smooth_stone"))
                    .Copy(SKColorType.Rgba8888);
                return _smooth_stone;
            }
        }

        private static SKBitmap _smooth_stone_slab_side = null;
        public static SKBitmap smooth_stone_slab_side {
            get {
                if (_smooth_stone_slab_side == null)
                    _smooth_stone_slab_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("smooth_stone_slab_side"))
                    .Copy(SKColorType.Rgba8888);
                return _smooth_stone_slab_side;
            }
        }

        private static SKBitmap _snow = null;
        public static SKBitmap snow {
            get {
                if (_snow == null)
                    _snow = SKBitmap.Decode((byte[])ResourceManager.GetObject("snow"))
                    .Copy(SKColorType.Rgba8888);
                return _snow;
            }
        }

        private static SKBitmap _soul_sand = null;
        public static SKBitmap soul_sand {
            get {
                if (_soul_sand == null)
                    _soul_sand = SKBitmap.Decode((byte[])ResourceManager.GetObject("soul_sand"))
                    .Copy(SKColorType.Rgba8888);
                return _soul_sand;
            }
        }

        private static SKBitmap _soul_soil = null;
        public static SKBitmap soul_soil {
            get {
                if (_soul_soil == null)
                    _soul_soil = SKBitmap.Decode((byte[])ResourceManager.GetObject("soul_soil"))
                    .Copy(SKColorType.Rgba8888);
                return _soul_soil;
            }
        }

        private static SKBitmap _sponge = null;
        public static SKBitmap sponge {
            get {
                if (_sponge == null)
                    _sponge = SKBitmap.Decode((byte[])ResourceManager.GetObject("sponge"))
                    .Copy(SKColorType.Rgba8888);
                return _sponge;
            }
        }

        private static SKBitmap _spruce_log = null;
        public static SKBitmap spruce_log {
            get {
                if (_spruce_log == null)
                    _spruce_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("spruce_log"))
                    .Copy(SKColorType.Rgba8888);
                return _spruce_log;
            }
        }

        private static SKBitmap _spruce_planks = null;
        public static SKBitmap spruce_planks {
            get {
                if (_spruce_planks == null)
                    _spruce_planks = SKBitmap.Decode((byte[])ResourceManager.GetObject("spruce_planks"))
                    .Copy(SKColorType.Rgba8888);
                return _spruce_planks;
            }
        }

        private static SKBitmap _stone = null;
        public static SKBitmap stone {
            get {
                if (_stone == null)
                    _stone = SKBitmap.Decode((byte[])ResourceManager.GetObject("stone"))
                    .Copy(SKColorType.Rgba8888);
                return _stone;
            }
        }

        private static SKBitmap _stone_bricks = null;
        public static SKBitmap stone_bricks {
            get {
                if (_stone_bricks == null)
                    _stone_bricks = SKBitmap.Decode((byte[])ResourceManager.GetObject("stone_bricks"))
                    .Copy(SKColorType.Rgba8888);
                return _stone_bricks;
            }
        }

        private static SKBitmap _stripped_acacia_log = null;
        public static SKBitmap stripped_acacia_log {
            get {
                if (_stripped_acacia_log == null)
                    _stripped_acacia_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_acacia_log"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_acacia_log;
            }
        }

        private static SKBitmap _stripped_acacia_log_top = null;
        public static SKBitmap stripped_acacia_log_top {
            get {
                if (_stripped_acacia_log_top == null)
                    _stripped_acacia_log_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_acacia_log_top"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_acacia_log_top;
            }
        }

        private static SKBitmap _stripped_birch_log = null;
        public static SKBitmap stripped_birch_log {
            get {
                if (_stripped_birch_log == null)
                    _stripped_birch_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_birch_log"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_birch_log;
            }
        }

        private static SKBitmap _stripped_birch_log_top = null;
        public static SKBitmap stripped_birch_log_top {
            get {
                if (_stripped_birch_log_top == null)
                    _stripped_birch_log_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_birch_log_top"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_birch_log_top;
            }
        }

        private static SKBitmap _stripped_crimson_stem = null;
        public static SKBitmap stripped_crimson_stem {
            get {
                if (_stripped_crimson_stem == null)
                    _stripped_crimson_stem = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_crimson_stem"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_crimson_stem;
            }
        }

        private static SKBitmap _stripped_crimson_stem_top = null;
        public static SKBitmap stripped_crimson_stem_top {
            get {
                if (_stripped_crimson_stem_top == null)
                    _stripped_crimson_stem_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_crimson_stem_top"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_crimson_stem_top;
            }
        }

        private static SKBitmap _stripped_dark_oak_log = null;
        public static SKBitmap stripped_dark_oak_log {
            get {
                if (_stripped_dark_oak_log == null)
                    _stripped_dark_oak_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_dark_oak_log"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_dark_oak_log;
            }
        }

        private static SKBitmap _stripped_dark_oak_log_top = null;
        public static SKBitmap stripped_dark_oak_log_top {
            get {
                if (_stripped_dark_oak_log_top == null)
                    _stripped_dark_oak_log_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_dark_oak_log_top"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_dark_oak_log_top;
            }
        }

        private static SKBitmap _stripped_jungle_log = null;
        public static SKBitmap stripped_jungle_log {
            get {
                if (_stripped_jungle_log == null)
                    _stripped_jungle_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_jungle_log"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_jungle_log;
            }
        }

        private static SKBitmap _stripped_jungle_log_top = null;
        public static SKBitmap stripped_jungle_log_top {
            get {
                if (_stripped_jungle_log_top == null)
                    _stripped_jungle_log_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_jungle_log_top"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_jungle_log_top;
            }
        }

        private static SKBitmap _stripped_mangrove_log = null;
        public static SKBitmap stripped_mangrove_log {
            get {
                if (_stripped_mangrove_log == null)
                    _stripped_mangrove_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_mangrove_log"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_mangrove_log;
            }
        }

        private static SKBitmap _stripped_mangrove_log_top = null;
        public static SKBitmap stripped_mangrove_log_top {
            get {
                if (_stripped_mangrove_log_top == null)
                    _stripped_mangrove_log_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_mangrove_log_top"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_mangrove_log_top;
            }
        }

        private static SKBitmap _stripped_oak_log = null;
        public static SKBitmap stripped_oak_log {
            get {
                if (_stripped_oak_log == null)
                    _stripped_oak_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_oak_log"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_oak_log;
            }
        }

        private static SKBitmap _stripped_oak_log_top = null;
        public static SKBitmap stripped_oak_log_top {
            get {
                if (_stripped_oak_log_top == null)
                    _stripped_oak_log_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_oak_log_top"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_oak_log_top;
            }
        }

        private static SKBitmap _stripped_spruce_log = null;
        public static SKBitmap stripped_spruce_log {
            get {
                if (_stripped_spruce_log == null)
                    _stripped_spruce_log = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_spruce_log"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_spruce_log;
            }
        }

        private static SKBitmap _stripped_spruce_log_top = null;
        public static SKBitmap stripped_spruce_log_top {
            get {
                if (_stripped_spruce_log_top == null)
                    _stripped_spruce_log_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_spruce_log_top"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_spruce_log_top;
            }
        }

        private static SKBitmap _stripped_warped_stem = null;
        public static SKBitmap stripped_warped_stem {
            get {
                if (_stripped_warped_stem == null)
                    _stripped_warped_stem = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_warped_stem"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_warped_stem;
            }
        }

        private static SKBitmap _stripped_warped_stem_top = null;
        public static SKBitmap stripped_warped_stem_top {
            get {
                if (_stripped_warped_stem_top == null)
                    _stripped_warped_stem_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("stripped_warped_stem_top"))
                    .Copy(SKColorType.Rgba8888);
                return _stripped_warped_stem_top;
            }
        }

        private static SKBitmap _target_side = null;
        public static SKBitmap target_side {
            get {
                if (_target_side == null)
                    _target_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("target_side"))
                    .Copy(SKColorType.Rgba8888);
                return _target_side;
            }
        }

        private static SKBitmap _target_top = null;
        public static SKBitmap target_top {
            get {
                if (_target_top == null)
                    _target_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("target_top"))
                    .Copy(SKColorType.Rgba8888);
                return _target_top;
            }
        }

        private static SKBitmap _terracotta = null;
        public static SKBitmap terracotta {
            get {
                if (_terracotta == null)
                    _terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _terracotta;
            }
        }

        private static SKBitmap _tinted_glass = null;
        public static SKBitmap tinted_glass {
            get {
                if (_tinted_glass == null)
                    _tinted_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("tinted_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _tinted_glass;
            }
        }

        private static SKBitmap _tnt_side = null;
        public static SKBitmap tnt_side {
            get {
                if (_tnt_side == null)
                    _tnt_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("tnt_side"))
                    .Copy(SKColorType.Rgba8888);
                return _tnt_side;
            }
        }

        private static SKBitmap _tnt_top = null;
        public static SKBitmap tnt_top {
            get {
                if (_tnt_top == null)
                    _tnt_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("tnt_top"))
                    .Copy(SKColorType.Rgba8888);
                return _tnt_top;
            }
        }

        private static SKBitmap _tube_coral_block = null;
        public static SKBitmap tube_coral_block {
            get {
                if (_tube_coral_block == null)
                    _tube_coral_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("tube_coral_block"))
                    .Copy(SKColorType.Rgba8888);
                return _tube_coral_block;
            }
        }

        private static SKBitmap _tuff = null;
        public static SKBitmap tuff {
            get {
                if (_tuff == null)
                    _tuff = SKBitmap.Decode((byte[])ResourceManager.GetObject("tuff"))
                    .Copy(SKColorType.Rgba8888);
                return _tuff;
            }
        }

        private static SKBitmap _verdant_froglight_side = null;
        public static SKBitmap verdant_froglight_side {
            get {
                if (_verdant_froglight_side == null)
                    _verdant_froglight_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("verdant_froglight_side"))
                    .Copy(SKColorType.Rgba8888);
                return _verdant_froglight_side;
            }
        }

        private static SKBitmap _verdant_froglight_top = null;
        public static SKBitmap verdant_froglight_top {
            get {
                if (_verdant_froglight_top == null)
                    _verdant_froglight_top = SKBitmap.Decode((byte[])ResourceManager.GetObject("verdant_froglight_top"))
                    .Copy(SKColorType.Rgba8888);
                return _verdant_froglight_top;
            }
        }

        private static SKBitmap _warped_nylium = null;
        public static SKBitmap warped_nylium {
            get {
                if (_warped_nylium == null)
                    _warped_nylium = SKBitmap.Decode((byte[])ResourceManager.GetObject("warped_nylium"))
                    .Copy(SKColorType.Rgba8888);
                return _warped_nylium;
            }
        }

        private static SKBitmap _warped_nylium_side = null;
        public static SKBitmap warped_nylium_side {
            get {
                if (_warped_nylium_side == null)
                    _warped_nylium_side = SKBitmap.Decode((byte[])ResourceManager.GetObject("warped_nylium_side"))
                    .Copy(SKColorType.Rgba8888);
                return _warped_nylium_side;
            }
        }

        private static SKBitmap _warped_planks = null;
        public static SKBitmap warped_planks {
            get {
                if (_warped_planks == null)
                    _warped_planks = SKBitmap.Decode((byte[])ResourceManager.GetObject("warped_planks"))
                    .Copy(SKColorType.Rgba8888);
                return _warped_planks;
            }
        }

        private static SKBitmap _warped_stem = null;
        public static SKBitmap warped_stem {
            get {
                if (_warped_stem == null)
                    _warped_stem = SKBitmap.Decode((byte[])ResourceManager.GetObject("warped_stem"))
                    .Copy(SKColorType.Rgba8888);
                return _warped_stem;
            }
        }

        private static SKBitmap _warped_wart_block = null;
        public static SKBitmap warped_wart_block {
            get {
                if (_warped_wart_block == null)
                    _warped_wart_block = SKBitmap.Decode((byte[])ResourceManager.GetObject("warped_wart_block"))
                    .Copy(SKColorType.Rgba8888);
                return _warped_wart_block;
            }
        }

        private static SKBitmap _weathered_copper = null;
        public static SKBitmap weathered_copper {
            get {
                if (_weathered_copper == null)
                    _weathered_copper = SKBitmap.Decode((byte[])ResourceManager.GetObject("weathered_copper"))
                    .Copy(SKColorType.Rgba8888);
                return _weathered_copper;
            }
        }

        private static SKBitmap _weathered_cut_copper = null;
        public static SKBitmap weathered_cut_copper {
            get {
                if (_weathered_cut_copper == null)
                    _weathered_cut_copper = SKBitmap.Decode((byte[])ResourceManager.GetObject("weathered_cut_copper"))
                    .Copy(SKColorType.Rgba8888);
                return _weathered_cut_copper;
            }
        }

        private static SKBitmap _wet_sponge = null;
        public static SKBitmap wet_sponge {
            get {
                if (_wet_sponge == null)
                    _wet_sponge = SKBitmap.Decode((byte[])ResourceManager.GetObject("wet_sponge"))
                    .Copy(SKColorType.Rgba8888);
                return _wet_sponge;
            }
        }

        private static SKBitmap _white_concrete = null;
        public static SKBitmap white_concrete {
            get {
                if (_white_concrete == null)
                    _white_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("white_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _white_concrete;
            }
        }

        private static SKBitmap _white_concrete_powder = null;
        public static SKBitmap white_concrete_powder {
            get {
                if (_white_concrete_powder == null)
                    _white_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("white_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _white_concrete_powder;
            }
        }

        private static SKBitmap _white_glazed_terracotta = null;
        public static SKBitmap white_glazed_terracotta {
            get {
                if (_white_glazed_terracotta == null)
                    _white_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("white_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _white_glazed_terracotta;
            }
        }

        private static SKBitmap _white_stained_glass = null;
        public static SKBitmap white_stained_glass {
            get {
                if (_white_stained_glass == null)
                    _white_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("white_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _white_stained_glass;
            }
        }

        private static SKBitmap _white_terracotta = null;
        public static SKBitmap white_terracotta {
            get {
                if (_white_terracotta == null)
                    _white_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("white_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _white_terracotta;
            }
        }

        private static SKBitmap _white_wool = null;
        public static SKBitmap white_wool {
            get {
                if (_white_wool == null)
                    _white_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("white_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _white_wool;
            }
        }

        private static SKBitmap _yellow_concrete = null;
        public static SKBitmap yellow_concrete {
            get {
                if (_yellow_concrete == null)
                    _yellow_concrete = SKBitmap.Decode((byte[])ResourceManager.GetObject("yellow_concrete"))
                    .Copy(SKColorType.Rgba8888);
                return _yellow_concrete;
            }
        }

        private static SKBitmap _yellow_concrete_powder = null;
        public static SKBitmap yellow_concrete_powder {
            get {
                if (_yellow_concrete_powder == null)
                    _yellow_concrete_powder = SKBitmap.Decode((byte[])ResourceManager.GetObject("yellow_concrete_powder"))
                    .Copy(SKColorType.Rgba8888);
                return _yellow_concrete_powder;
            }
        }

        private static SKBitmap _yellow_glazed_terracotta = null;
        public static SKBitmap yellow_glazed_terracotta {
            get {
                if (_yellow_glazed_terracotta == null)
                    _yellow_glazed_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("yellow_glazed_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _yellow_glazed_terracotta;
            }
        }

        private static SKBitmap _yellow_stained_glass = null;
        public static SKBitmap yellow_stained_glass {
            get {
                if (_yellow_stained_glass == null)
                    _yellow_stained_glass = SKBitmap.Decode((byte[])ResourceManager.GetObject("yellow_stained_glass"))
                    .Copy(SKColorType.Rgba8888);
                return _yellow_stained_glass;
            }
        }

        private static SKBitmap _yellow_terracotta = null;
        public static SKBitmap yellow_terracotta {
            get {
                if (_yellow_terracotta == null)
                    _yellow_terracotta = SKBitmap.Decode((byte[])ResourceManager.GetObject("yellow_terracotta"))
                    .Copy(SKColorType.Rgba8888);
                return _yellow_terracotta;
            }
        }

        private static SKBitmap _yellow_wool = null;
        public static SKBitmap yellow_wool {
            get {
                if (_yellow_wool == null)
                    _yellow_wool = SKBitmap.Decode((byte[])ResourceManager.GetObject("yellow_wool"))
                    .Copy(SKColorType.Rgba8888);
                return _yellow_wool;
            }
        }
	}
}
#pragma warning restore IDE1006 // Naming Styles
#endif
