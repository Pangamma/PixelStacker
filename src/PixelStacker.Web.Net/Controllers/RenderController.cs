using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Logic.Model;
using PixelStacker.Web.Net.Models;
using PixelStacker.Web.Net.Utility;
using SkiaSharp;

namespace PixelStacker.Web.Net.Controllers
{
    public class RenderController : BaseApiController
    {

        // TODO make this into an LRU to avoid mem crashes and leaks from abusive actors
        const string contentType = "image/jpeg"; // PNG
        private static LruCache<string, byte[]> Cache { get; set; } = new LruCache<string, byte[]>(200, TimeSpan.FromHours(24));
        
        private static Lazy<IColorMapper> ColorMapperSideView = new Lazy<IColorMapper>(() =>
        {
            var cm = new KdTreeMapper();
            var palette = MaterialPalette.FromResx();
            var opts = new StaticJsonOptionsProvider().Load();
            var combosAvailable = palette.ToValidCombinationList(opts);
            cm.SetSeedData(combosAvailable, palette, true);
            return cm;
        });

        private static Lazy<IColorMapper> ColorMapperTopView = new Lazy<IColorMapper>(() =>
        {
            var cm = new KdTreeMapper();
            var palette = MaterialPalette.FromResx();
            var opts = new StaticJsonOptionsProvider().Load();
            var combosAvailable = palette.ToValidCombinationList(opts);
            cm.SetSeedData(combosAvailable, palette, false);
            return cm;
        });

        [HttpGet]
        public async Task<ActionResult> ByURL(string url)
        {
            if (Cache.TryGetValue(url, out byte[] cachedData)){
                return File(cachedData, contentType);
            }

            byte[] dataFromUrl = new WebClient().DownloadData(url);
            var bm = SKBitmap.Decode(dataFromUrl);
            return await this.DoSimple(bm, url);
        }

        [HttpPost]
        public async Task<ActionResult> ByFile(IFormFile file)
        {
            SKBitmap bm = file.ToSKBitmap();
            return await this.DoSimple(bm, null);
        }

        [HttpGet]
        public async Task<ActionResult> ByURLAdvanced(UrlRenderRequest model)
        {
            byte[] dataFromUrl = new WebClient().DownloadData(model.Url);
            var bm = SKBitmap.Decode(dataFromUrl);
            return await DoAdvanced(model, bm);
        }

        [HttpPost]
        public async Task<ActionResult> ByFileAdvanced(FileRenderRequest model)
        {
            SKBitmap bm = model.File.ToSKBitmap();
            return await DoAdvanced(model, bm);
        }

        private async Task<ActionResult> DoSimple(SKBitmap bm, string cacheKey = null)
        {
            var engine = new RenderCanvasEngine();
            var palette = MaterialPalette.FromResx();
            var preprocessed = await engine.PreprocessImageAsync(null, bm, new Logic.IO.Config.CanvasPreprocessorSettings()
            {
                IsSideView = false,
                MaxHeight = 300,
                MaxWidth = 300,
                RgbBucketSize = 1,
                QuantizerSettings = new Logic.IO.Config.QuantizerSettings()
                {
                    IsEnabled = false
                }
            });
            var canvas = await engine.RenderCanvasAsync(null, preprocessed, ColorMapperTopView.Value, palette);
            IExportFormatter exporter = new JpegFormatter();
            byte[] data = await exporter.ExportAsync(new PixelStackerProjectData()
            {
                CanvasData = canvas.CanvasData,
                IsSideView = false,
                MaterialPalette = palette,
                PreprocessedImage = canvas.PreprocessedImage,
                //WorldEditOrigin = new int[] { (int)canvas.WorldEditOrigin.X, (int)canvas.WorldEditOrigin.Y }
                WorldEditOrigin = null
            }, null);

            if (cacheKey != null)
                Cache.Set(cacheKey, data);

            return File(data, contentType);
        }

        private async Task<ActionResult> DoAdvanced(BaseRenderRequest model, SKBitmap bm)
        {
            var engine = new RenderCanvasEngine();
            var palette = MaterialPalette.FromResx();
            bool isv = model.IsSideView;
            using var preprocessed = await engine.PreprocessImageAsync(null, bm, new CanvasPreprocessorSettings()
            {
                IsSideView = isv,
                MaxHeight = model.MaxHeight ?? 300,
                MaxWidth = model.MaxWidth ?? 300,
                RgbBucketSize = model.RgbBucketSize,
                QuantizerSettings = new QuantizerSettings()
                {
                    IsEnabled = model.EnableDithering || model.QuantizedColorCount.HasValue,
                    Algorithm = QuantizerAlgorithm.WuColor,
                    DitherAlgorithm = model.EnableDithering ? "Atkinson" : "No dithering",
                    MaxParallelProcesses = 1,
                    MaxColorCount = model.QuantizedColorCount ?? 256
                }
            });

            var canvas = await engine.RenderCanvasAsync(null, preprocessed, isv ? ColorMapperSideView.Value : ColorMapperTopView.Value, palette);
            IExportFormatter exporter = model.Format.GetFormatter();
            byte[] data = await exporter.ExportAsync(new PixelStackerProjectData()
            {
                CanvasData = canvas.CanvasData,
                IsSideView = isv,
                MaterialPalette = palette,
                PreprocessedImage = canvas.PreprocessedImage,
                //WorldEditOrigin = new int[] { (int)canvas.WorldEditOrigin.X, (int)canvas.WorldEditOrigin.Y }
                WorldEditOrigin = null
            }, null);

            var (contentType, fileExt) = model.Format.GetContentTypeData();
            if (contentType == "application/octet-stream")
            {
                return File(data, contentType, "download" + fileExt);
            }
            else
            {
                // Image style
                return File(data, contentType);
            }
        }
    }
}
