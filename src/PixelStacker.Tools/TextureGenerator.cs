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
                // gImg.DrawImage(m.getImage(isSide), xi, yi, textureSize.Value, textureSize.Value);
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
            Console.WriteLine("Test");
            //string rootShadowsPath = @"D:\git\PixelStacker\src\PixelStacker\Resources\Images\UI\shadows";
            //var saveFolder = $@"{rootShadowsPath}\depth_{depth}";
            //string fToStrip = $@"{rootShadowsPath}\depth_{depth}\source-d{depth}.png";

            //{
            //    var bmSource = Bitmap.FromFile(fToStrip).To32bppBitmap();
            //    Point pntRedDot = new Point(-1, -1);


            //    // Make it so that everything is transparent
            //    bmSource.ToEditStream(null, (int x, int y, Color c) =>
            //    {
            //        if (c.R == 255 && c.B == 0 && c.G == 0)
            //        {
            //            pntRedDot = new Point(x, y);
            //            return c;
            //        }

            //        var b = c.GetBrightness() * 100;
            //        var nAlpha = ((int) (255 * (100 - b))) / 100;
            //        return Color.FromArgb((int) nAlpha, 0, 0, 0);
            //    });

            //    if (pntRedDot.X == -1 || pntRedDot.Y == -1)
            //    {
            //        throw new ArgumentException("A red dot is required on the inside of the top left corner.");
            //    }

            //    #region CORNERS
            //    {
            //        int xStart = -1;
            //        int xFinish = pntRedDot.X;
            //        int yStart = -1;
            //        int yFinish = pntRedDot.Y;

            //        {
            //            // TOP LEFT CORNER
            //            for (int x = 0; x < pntRedDot.X; x++)
            //            {
            //                Color c = bmSource.GetPixel(x, pntRedDot.Y - 1);
            //                if (c.A > 0 && xStart == -1)
            //                {
            //                    xStart = x;
            //                    break;
            //                }
            //            }

            //            for (int y = 0; y < pntRedDot.Y; y++)
            //            {
            //                Color c = bmSource.GetPixel(pntRedDot.X - 1, y);
            //                if (c.A > 0 && yStart == -1)
            //                {
            //                    yStart = y;
            //                    break;
            //                }
            //            }
            //        }

            //        int width = xFinish - xStart;
            //        int height = yFinish - yStart;
            //        using (var bmCorner = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            //        {
            //            for (int x = 0; x < width; x++)
            //            {
            //                for (int y = 0; y < height; y++)
            //                {
            //                    Color c = bmSource.GetPixel(x + xStart, y + yStart);
            //                    bmCorner.SetPixel(x, y, c);
            //                }
            //            }

            //            bmCorner.Save($@"{saveFolder}\d{depth}_BR.png");
            //            bmCorner.RotateFlip(RotateFlipType.RotateNoneFlipX);
            //            bmCorner.Save($@"{saveFolder}\d{depth}_BL.png");
            //            bmCorner.RotateFlip(RotateFlipType.RotateNoneFlipY);
            //            bmCorner.Save($@"{saveFolder}\d{depth}_TL.png");
            //            bmCorner.RotateFlip(RotateFlipType.RotateNoneFlipX);
            //            bmCorner.Save($@"{saveFolder}\d{depth}_TR.png");
            //        }
            //    }
            //    #endregion

            //    #region  LEFT SIDES
            //    {
            //        int xStart = -1;
            //        int xFinish = -1;
            //        for (int x = 0; x < bmSource.Width; x++)
            //        {
            //            Color c = bmSource.GetPixel(x, pntRedDot.Y);
            //            if (c.A > 0 && xStart == -1)
            //            {
            //                xStart = x;
            //                continue;
            //            }

            //            if (c.A == 0 && xStart > -1 && xFinish == -1)
            //            {
            //                xFinish = x - 1;
            //                break;
            //            }
            //        }

            //        int width = xFinish - xStart;
            //        using (var bmCorner = new Bitmap(width, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            //        {
            //            for (int x = 0; x < width; x++)
            //            {
            //                Color c = bmSource.GetPixel(x + xStart, pntRedDot.Y);
            //                bmCorner.SetPixel(x, 0, c);
            //            }

            //            bmCorner.Save($@"{saveFolder}\d{depth}_R.png");
            //            bmCorner.RotateFlip(RotateFlipType.RotateNoneFlipX);
            //            bmCorner.Save($@"{saveFolder}\d{depth}_L.png");
            //        }
            //    }
            //    #endregion

            //    #region TOP
            //    {
            //        int yStart = -1;
            //        int yFinish = -1;
            //        for (int y = 0; y < bmSource.Height; y++)
            //        {
            //            Color c = bmSource.GetPixel(pntRedDot.X, y);
            //            if (c.A > 0 && yStart == -1)
            //            {
            //                yStart = y;
            //                continue;
            //            }

            //            if (c.A == 0 && yStart > -1 && yFinish == -1)
            //            {
            //                yFinish = y - 1;
            //                break;
            //            }
            //        }

            //        int height = yFinish - yStart;
            //        using (var bmCorner = new Bitmap(1, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            //        {
            //            for (int y = 0; y < height; y++)
            //            {
            //                Color c = bmSource.GetPixel(pntRedDot.X, y + yStart);
            //                bmCorner.SetPixel(0, y, c);
            //            }

            //            bmCorner.Save($@"{saveFolder}\d{depth}_B.png");
            //            bmCorner.RotateFlip(RotateFlipType.RotateNoneFlipY);
            //            bmCorner.Save($@"{saveFolder}\d{depth}_T.png");
            //        }
            //    }
            //    #endregion

            //    bmSource.Save(fToStrip + ".stripped.png");
            //}
        }
    }
}
