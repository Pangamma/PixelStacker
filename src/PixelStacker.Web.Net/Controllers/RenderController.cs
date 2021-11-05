using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine;
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

            Cache.Set(url, data);

            return File(data, contentType);
        }


        [HttpPost]
        public async Task<ActionResult> Simple(IFormFile file)
        {
            var engine = new RenderCanvasEngine();
            SKBitmap bm = file.ToSKBitmap();
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

            //string contentType = "APPLICATION/octet-stream"; // download
            return File(data, contentType);
        }

        [HttpPost]
        public async Task<ActionResult> Advanced(RenderRequest model)
        {
            var engine = new RenderCanvasEngine();
            SKBitmap bm = model.File.ToSKBitmap();
            var palette = MaterialPalette.FromResx();
            using var preprocessed = await engine.PreprocessImageAsync(null, bm, model.PreprocessSettings);
            var canvas = await engine.RenderCanvasAsync(null, preprocessed, ColorMapperTopView.Value, palette);
            IExportFormatter exporter = model.Format.GetFormatter();
            byte[] data = await exporter.ExportAsync(new PixelStackerProjectData()
            {
                CanvasData = canvas.CanvasData,
                IsSideView = false,
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
