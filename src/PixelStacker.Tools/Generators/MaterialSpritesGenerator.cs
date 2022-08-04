using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PixelStacker.Tools.Generators
{
    public class MaterialSpriteItem
    {
        public int x { get; set; }
        public int y { get; set; }
        public int page { get; set; }
    }

    [TestClass]
    public class MaterialSpritesGenerator
    {
        private string RootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.Tools\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

        [TestMethod]
        [TestCategory("Generators")]
        public void MaterialSpriteSheets()
        {
            string filePath = RootDir + "\\PixelStacker.Resources\\Files\\materialPalette.json";
            const int MAX_WIDTH = 20;
            const int MAX_HEIGHT = 20;

            const int MATS_PER_PAGE = MAX_WIDTH * MAX_HEIGHT;
            int numPages = Materials.List.Count / MATS_PER_PAGE;
            if ((numPages * MATS_PER_PAGE) != Materials.List.Count)
                numPages += 1;

            Dictionary<string, MaterialSpriteItem> map = new Dictionary<string, MaterialSpriteItem>();

            using SKPaint paint = new SKPaint()
            {

            };
            for (int page = 0; page < numPages; page++)
            {
                using SKBitmap bmTop = new SKBitmap(MAX_WIDTH * Constants.DefaultTextureSize, MAX_HEIGHT * Constants.DefaultTextureSize, SKColorType.Rgba8888, SKAlphaType.Premul);
                using SKBitmap bmSide = new SKBitmap(MAX_WIDTH * Constants.DefaultTextureSize, MAX_HEIGHT * Constants.DefaultTextureSize, SKColorType.Rgba8888, SKAlphaType.Premul);
                using SKCanvas cTop = new SKCanvas(bmTop);
                using SKCanvas cSide = new SKCanvas(bmSide);

                for (int y = 0; y < MAX_HEIGHT; y++)
                {
                    for (int x = 0; x < MAX_WIDTH; x++)
                    {
                        int idx = (page * MATS_PER_PAGE) + MAX_WIDTH * y + x;
                        if (idx >= Materials.List.Count)
                            break;

                        Material m = Materials.List[idx];
                        cTop.DrawBitmap(m.GetImage(false), new SKPoint(x * Constants.DefaultTextureSize, y * Constants.DefaultTextureSize), paint);
                        cSide.DrawBitmap(m.GetImage(true), new SKPoint(x * Constants.DefaultTextureSize, y * Constants.DefaultTextureSize), paint);
                        map[m.PixelStackerID] = new MaterialSpriteItem()
                        {
                            x = x,
                            y = y,
                            page = page
                        };
                    }
                }

                using var ms = new MemoryStream();
                if (bmTop.Encode(ms, SKEncodedImageFormat.Png, 100))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    File.WriteAllBytes($"./sprites/top-m-sprite-{page}.png", ms.ToArray());
                }

                using var ms2 = new MemoryStream();
                if (bmSide.Encode(ms2, SKEncodedImageFormat.Png, 100))
                {
                    ms2.Seek(0, SeekOrigin.Begin);
                    File.WriteAllBytes($"./sprites/side-m-sprite-{page}.png", ms2.ToArray());
                }

                File.WriteAllText("./sprites/m-index.json", JsonConvert.SerializeObject(map));
            }
        }

        [TestMethod]
        [TestCategory("Generators")]
        public void MaterialComboSpriteSheets()
        {
            string filePath = RootDir + "\\PixelStacker.Resources\\Files\\materialPalette.json";
            const int MAX_WIDTH = 19;
            const int MAX_HEIGHT = 20;

            const int MATS_PER_PAGE = MAX_WIDTH * MAX_HEIGHT;
            int numPages = MaterialPalette.FromResx().Count / MATS_PER_PAGE;
            if ((numPages * MATS_PER_PAGE) != Materials.List.Count)
                numPages += 1;

            Dictionary<string, MaterialSpriteItem> map = new Dictionary<string, MaterialSpriteItem>();

            using SKPaint paint = new SKPaint()
            {

            };
            for (int page = 0; page < numPages; page++)
            {
                using SKBitmap bmTop = new SKBitmap(MAX_WIDTH * Constants.DefaultTextureSize, MAX_HEIGHT * Constants.DefaultTextureSize, SKColorType.Rgba8888, SKAlphaType.Premul);
                using SKBitmap bmSide = new SKBitmap(MAX_WIDTH * Constants.DefaultTextureSize, MAX_HEIGHT * Constants.DefaultTextureSize, SKColorType.Rgba8888, SKAlphaType.Premul);
                using SKCanvas cTop = new SKCanvas(bmTop);
                using SKCanvas cSide = new SKCanvas(bmSide);

                for (int y = 0; y < MAX_HEIGHT; y++)
                {
                    for (int x = 0; x < MAX_WIDTH; x++)
                    {
                        int idx = (page * MATS_PER_PAGE) + MAX_WIDTH * y + x;
                        if (idx >= MaterialPalette.FromResx().Count)
                            break;
                        var m = MaterialPalette.FromResx()[idx];
                        //Material m = Materials.List[idx];
                        cTop.DrawBitmap(m.GetImage(false), new SKPoint(x * Constants.DefaultTextureSize, y * Constants.DefaultTextureSize), paint);
                        cSide.DrawBitmap(m.GetImage(true), new SKPoint(x * Constants.DefaultTextureSize, y * Constants.DefaultTextureSize), paint);
                        map[idx + ""] = new MaterialSpriteItem()
                        {
                            x = x,
                            y = y,
                            page = page
                        };
                    }
                }

                using var ms = new MemoryStream();
                if (bmTop.Encode(ms, SKEncodedImageFormat.Png, 100))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    File.WriteAllBytes($"./sprites/top-mc-sprite-{page}.png", ms.ToArray());
                }

                using var ms2 = new MemoryStream();
                if (bmSide.Encode(ms2, SKEncodedImageFormat.Png, 100))
                {
                    ms2.Seek(0, SeekOrigin.Begin);
                    File.WriteAllBytes($"./sprites/side-mc-sprite-{page}.png", ms2.ToArray());
                }

                File.WriteAllText("./sprites/mc-index.json", JsonConvert.SerializeObject(map));
            }
        }
    }
}
