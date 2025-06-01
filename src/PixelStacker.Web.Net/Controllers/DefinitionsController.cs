using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.Controllers
{
    public class MaterialSpriteItem
    {
        public int x { get; set; }
        public int y { get; set; }
        public int page { get; set; }
    }

    public class DefinitionsController : BaseApiController
    {
        const int MAX_WIDTH = 20;
        const int MAX_HEIGHT = 20;
        const int MATS_PER_PAGE = MAX_HEIGHT * MAX_WIDTH;


        [HttpGet]
        public Task<JsonResult> MaterialList()
        {
            var data = new List<object>();
            var index = GetMaterialSpriteIndex();
            foreach (var m in Materials.List)
            {
                var sprite = index[m.PixelStackerID];
                data.Add(new
                {
                    pixelStackerID = m.PixelStackerID,
                    label = m.Label,
                    category = m.Category,
                    spriteX = sprite.x,
                    spriteY = sprite.y,
                    spritePage = sprite.page
                });
            }

            return Task.FromResult(Json(data));
        }

        [HttpGet]
        public Task<JsonResult>  MaterialPaletteMap()
        {
            MaterialPalette palette = MaterialPalette.FromResx();
            var jsonObj = palette.ToResxDictionary();
            return Task.FromResult(Json(jsonObj));
        }

        private Dictionary<string, MaterialSpriteItem> GetMaterialSpriteIndex()
        {

            Dictionary<string, MaterialSpriteItem> map = new Dictionary<string, MaterialSpriteItem>();
            const int MATS_PER_PAGE = MAX_WIDTH * MAX_HEIGHT;
            int numPages = Materials.List.Count / MATS_PER_PAGE;
            if ((numPages * MATS_PER_PAGE) != Materials.List.Count)
                numPages += 1;

            for (int page = 0; page < numPages; page++)
            {
                for (int y = 0; y < MAX_HEIGHT; y++)
                {
                    for (int x = 0; x < MAX_WIDTH; x++)
                    {
                        int idx = (page * MATS_PER_PAGE) + MAX_WIDTH * y + x;
                        if (idx >= Materials.List.Count)
                            break;

                        Material m = Materials.List[idx];
                        map[m.PixelStackerID] = new MaterialSpriteItem()
                        {
                            x = x,
                            y = y,
                            page = page
                        };
                    }
                }

            }

            return map;
        }

        [HttpGet]
        public Task<FileContentResult> GetMaterialSpriteSheet(bool isSide, int page)
        {
            var index = this.GetMaterialSpriteIndex().ToList().Where(x => x.Value.page == page);

            using SKBitmap bm = new SKBitmap(MAX_WIDTH * Constants.DefaultTextureSize, MAX_HEIGHT * Constants.DefaultTextureSize, SKColorType.Rgba8888, SKAlphaType.Premul);
            using SKCanvas canvas = new SKCanvas(bm);

            for (int y = 0; y < MAX_HEIGHT; y++)
            {
                for (int x = 0; x < MAX_WIDTH; x++)
                {
                    int idx = (page * MATS_PER_PAGE) + MAX_WIDTH * y + x;
                    if (idx >= Materials.List.Count)
                        break;

                    Material m = Materials.List[idx];
                    using SKImage img = SKImage.FromBitmap(m.GetImage(isSide));
                    canvas.DrawImage(img, new SKPoint(x * Constants.DefaultTextureSize, y * Constants.DefaultTextureSize), Constants.SAMPLE_OPTS_NONE);
                }
            }

            using var ms = new MemoryStream();
            if (bm.Encode(ms, SKEncodedImageFormat.Png, 100))
            {
                ms.Seek(0, SeekOrigin.Begin);
                byte[] dat = ms.ToArray();
                var f = File(dat, "image/png");
                return Task.FromResult(f);
            }

            throw new System.Exception("Failed to generate sprite sheet.");

        }
    }
}
