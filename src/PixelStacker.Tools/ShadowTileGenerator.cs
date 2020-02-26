using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using PixelStacker.Logic.Extensions;

namespace PixelStacker.Tools
{
    [TestClass]
    public class ShadowTileGenerator
    {

        [TestMethod]
        [TestCategory("Generators")]
        public void Depth_16()
        {
            this.GenerateShadowTiles(16);
        }

        //[TestMethod]
        //[TestCategory("Generators")]
        //public void Depth_8()
        //{
        //    this.GenerateShadowTiles(8);
        //}
        [TestMethod]
        [TestCategory("Generators")]
        public void ShadowSprites()
        {
            //this.GenerateShadowTiles(8);
            this.StripSpriteSheet(16);
            this.StripSpriteSheet(32);
            this.StripSpriteSheet(64);
        }

        //[TestMethod]
        //[TestCategory("Generators")]
        //public void Depth_4()
        //{
        //    this.GenerateShadowTiles(4);
        //}


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

        public void GenerateShadowTiles(int depth)
        {
            string rootShadowsPath = @"D:\git\PixelStacker\src\PixelStacker\Resources\Images\UI\shadows";
            var saveFolder = $@"{rootShadowsPath}\depth_{depth}";
            string fToStrip = $@"{rootShadowsPath}\depth_{depth}\source-d{depth}.png";

            {
                var bmSource = Bitmap.FromFile(fToStrip).To32bppBitmap();
                Point pntRedDot = new Point(-1, -1);


                // Make it so that everything is transparent
                bmSource.ToEditStream(null, (int x, int y, Color c) =>
                {
                    if (c.R == 255 && c.B == 0 && c.G == 0)
                    {
                        pntRedDot = new Point(x, y);
                        return c;
                    }

                    var b = c.GetBrightness() * 100;
                    var nAlpha = ((int) (255 * (100 - b))) / 100;
                    return Color.FromArgb((int) nAlpha, 0, 0, 0);
                });

                if (pntRedDot.X == -1 || pntRedDot.Y == -1)
                {
                    throw new ArgumentException("A red dot is required on the inside of the top left corner.");
                }

                #region CORNERS
                {
                    int xStart = -1;
                    int xFinish = pntRedDot.X;
                    int yStart = -1;
                    int yFinish = pntRedDot.Y;

                    {
                        // TOP LEFT CORNER
                        for (int x = 0; x < pntRedDot.X; x++)
                        {
                            Color c = bmSource.GetPixel(x, pntRedDot.Y - 1);
                            if (c.A > 0 && xStart == -1)
                            {
                                xStart = x;
                                break;
                            }
                        }

                        for (int y = 0; y < pntRedDot.Y; y++)
                        {
                            Color c = bmSource.GetPixel(pntRedDot.X - 1, y);
                            if (c.A > 0 && yStart == -1)
                            {
                                yStart = y;
                                break;
                            }
                        }
                    }

                    int width = xFinish - xStart;
                    int height = yFinish - yStart;
                    using (var bmCorner = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        for (int x = 0; x < width; x++)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                Color c = bmSource.GetPixel(x + xStart, y + yStart);
                                bmCorner.SetPixel(x, y, c);
                            }
                        }

                        bmCorner.Save($@"{saveFolder}\d{depth}_BR.png");
                        bmCorner.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        bmCorner.Save($@"{saveFolder}\d{depth}_BL.png");
                        bmCorner.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        bmCorner.Save($@"{saveFolder}\d{depth}_TL.png");
                        bmCorner.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        bmCorner.Save($@"{saveFolder}\d{depth}_TR.png");
                    }
                }
                #endregion

                #region  LEFT SIDES
                {
                    int xStart = -1;
                    int xFinish = -1;
                    for (int x = 0; x < bmSource.Width; x++)
                    {
                        Color c = bmSource.GetPixel(x, pntRedDot.Y);
                        if (c.A > 0 && xStart == -1)
                        {
                            xStart = x;
                            continue;
                        }

                        if (c.A == 0 && xStart > -1 && xFinish == -1)
                        {
                            xFinish = x - 1;
                            break;
                        }
                    }

                    int width = xFinish - xStart;
                    using (var bmCorner = new Bitmap(width, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        for (int x = 0; x < width; x++)
                        {
                            Color c = bmSource.GetPixel(x + xStart, pntRedDot.Y);
                            bmCorner.SetPixel(x, 0, c);
                        }

                        bmCorner.Save($@"{saveFolder}\d{depth}_R.png");
                        bmCorner.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        bmCorner.Save($@"{saveFolder}\d{depth}_L.png");
                    }
                }
                #endregion

                #region TOP
                {
                    int yStart = -1;
                    int yFinish = -1;
                    for (int y = 0; y < bmSource.Height; y++)
                    {
                        Color c = bmSource.GetPixel(pntRedDot.X, y);
                        if (c.A > 0 && yStart == -1)
                        {
                            yStart = y;
                            continue;
                        }

                        if (c.A == 0 && yStart > -1 && yFinish == -1)
                        {
                            yFinish = y - 1;
                            break;
                        }
                    }

                    int height = yFinish - yStart;
                    using (var bmCorner = new Bitmap(1, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        for (int y = 0; y < height; y++)
                        {
                            Color c = bmSource.GetPixel(pntRedDot.X, y + yStart);
                            bmCorner.SetPixel(0, y, c);
                        }

                        bmCorner.Save($@"{saveFolder}\d{depth}_B.png");
                        bmCorner.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        bmCorner.Save($@"{saveFolder}\d{depth}_T.png");
                    }
                }
                #endregion

                bmSource.Save(fToStrip + ".stripped.png");
            }
        }
    }
}
