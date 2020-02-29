using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using PixelStacker.Logic.Extensions;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Drawing.Imaging;

namespace PixelStacker.Tools
{
    [TestClass]
    public class TextureGenerator
    {

        [TestMethod]
        [TestCategory("Generators")]
        public void Tiles_64()
        {
            this.GenerateShadowTiles(64);
        }


        [TestMethod]
        [TestCategory("Generators")]
        public void Tiles_32()
        {
            this.GenerateShadowTiles(32);
        }

        public void StripSpriteSheet(int depth)
        {
            string rootShadowsPath = @"D:\git\PixelStacker\src\PixelStacker\Resources\Images\UI\shadows";

            {
                string fToStrip = $@"{rootShadowsPath}\sprites\sprite-source-x{depth}.png";
                var saveFile = $@"{rootShadowsPath}\sprites\sprite-x{depth}.png";
                // Make it so that everything is transparent
                using (var bmAnyFormat = Bitmap.FromFile(fToStrip))
                {
                    using (var bmSource = bmAnyFormat.To32bppBitmap())
                    {
                        bmSource.ToEditStream(null, (int x, int y, Color c) =>
                        {
                            var b = c.GetBrightness() * 100;
                            var nAlpha = ((int) (255 * (100 - b))) / 100;
                            return Color.FromArgb((int) nAlpha, 0, 0, 0);
                        });

                        bmSource.Save(saveFile);
                    }
                }
            }
        }

        public void GenerateShadowTiles(int rez)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-us");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-us");
            var props = typeof(Resources.Textures).GetProperties().ToList().Where(x => x.PropertyType == typeof(Bitmap));

            string rootSavePath = $@"D:\git\PixelStacker\src\PixelStacker\Resources\Images\Textures\x{rez}";
            var texToSkip = new string[] {
                "tnt_side","tnt_top","bee_nest_top","beehive_side","bone_block_top","bone_block_side",
                "redstone_lamp", "redstone_block","sea_lantern", "pumpkin_side","pumpkin_top",
                "jack_o_lantern","carved_pumpkin","bookshelf","birch_log","smooth_stone","lapis_block",
                "iron_block", "gold_block","emerald_block","diamond_block","smooth_stone_slab_side",
                "quartz_block_top","note_block","soul_sand"
            }.ToList();


            foreach (var prop in props)
            {
                string name = prop.Name;
                if (name.EndsWith("glass")) continue;
                if (name.EndsWith("glazed_terracotta")) continue;
                if (name.EndsWith("_ore")) continue;
                if (name.EndsWith("_log") && name.StartsWith("stripped")) continue;
                if (texToSkip.Any(t => t == name)) continue;


                Bitmap bm = Resources.Textures.ResourceManager.GetObject(name) as Bitmap;
                int tilesPerSquare = rez / bm.Width;
                using (Bitmap bmOut = new Bitmap(rez, rez, PixelFormat.Format32bppArgb))
                {
                    using (Graphics g = Graphics.FromImage(bmOut))
                    {
                        for (int tx = 0; tx < tilesPerSquare; tx++)
                        {
                            for (int ty = 0; ty < tilesPerSquare; ty++)
                            {
                                g.DrawImageUnscaled(bm, 0 + (tx * bm.Width), 0 + (ty * bm.Height), bm.Width, bm.Height);
                            }
                        }
                    }

                    bmOut.Save(rootSavePath + "\\" + name + ".png");
                }
            }
        }
    }
}
