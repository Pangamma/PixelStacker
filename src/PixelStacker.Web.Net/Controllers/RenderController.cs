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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.Controllers
{
    public class FileNode
    {
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
        public string SuggestedFileName { get; set; }

        public FileNode(byte[] data, string ct)
        {
            this.Data = data;
            this.ContentType = ct;
        }
    }


    public static class ColorMapperContainer
    {
        public static Lazy<string> DefaultColorMapperAlgorithmTitle = new Lazy<string>(() => {
            IColorMapper instance = Activator.CreateInstance(typeof(KdTreeMapper)) as IColorMapper;
            return instance.AlgorithmTitle;
        });

        public static Lazy<Dictionary<string, Type>> ColorMapperTypes = new Lazy<Dictionary<string, Type>>(() => {
            var interfaceType = typeof(IColorMapper);
            var typesThatImplementIColorMapper = interfaceType.Assembly.GetExportedTypes()
                .Where(t => interfaceType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(x =>
                {
                    IColorMapper instance = Activator.CreateInstance(x) as IColorMapper;
                    return instance;
                })
                .Where(x => x.AlgorithmTitle.Contains("KdTree"))
                .ToDictionary(k => k.AlgorithmTitle, v => v.GetType());
            return typesThatImplementIColorMapper;
        });

        private static object Padlock = new { };
        private static Dictionary<string, IColorMapper> MapperCache = new Dictionary<string, IColorMapper>();

        public static IColorMapper GetColorMapper(bool isSideView, bool isMultiLayer, string colorMapperAlgorithm)
        {
            string cacheKey = string.Format("{0}:{1}:{2}", isSideView ? "1" : "0", isMultiLayer ? "1" : "0", colorMapperAlgorithm);

            if (MapperCache.TryGetValue(cacheKey, out var value)) 
            {
                return value;
            }

            lock(Padlock)
            {
                if (MapperCache.TryGetValue(cacheKey, out var value2))
                {
                    return value2;
                }

                var mapper = CreateColorMapper(isSideView, isMultiLayer, colorMapperAlgorithm);
                MapperCache[cacheKey] = mapper;
                return mapper;
            }
        }

        private static IColorMapper CreateColorMapper(bool isSideView, bool isMultiLayer, string colorMapperAlgorithm)
        {
            if (!ColorMapperTypes.Value.TryGetValue(colorMapperAlgorithm, out Type found))
            {
                throw new ArgumentException("This is not a supported algorithm type.", nameof(colorMapperAlgorithm));
            }

            var cm = Activator.CreateInstance(found) as IColorMapper;
            var palette = MaterialPalette.FromResx();
            var opts = new StaticJsonOptionsProvider().Load();
            var combosAvailable = palette.ToValidCombinationList(opts);
            if (!isMultiLayer) combosAvailable = combosAvailable.Where(x => !x.IsMultiLayer && x.Bottom.CanBeUsedAsBottomLayer).ToList();
            cm.SetSeedData(combosAvailable, palette, isSideView);
            return cm;
        }
    }

    public class RenderController : BaseApiController
    {
        // TODO make this into an LRU to avoid mem crashes and leaks from abusive actors
        const string contentType = "image/jpeg"; // PNG
        private static LruCache<string, FileNode> Cache { get; set; } = new LruCache<string, FileNode>(100, TimeSpan.FromHours(24));

        [HttpGet]
#if !DEBUG
        [Obsolete("Do not show this function.", false)]
#endif
        public Task<JsonResult> Stats()
        {
            var data = new Dictionary<string, object>();
            data["LRU_SIZE"] = Cache.Count;
            data["LRU_KEYS"] = Cache.Keys;
            return Task.FromResult(Json(data));
        }

        [HttpGet]
        public async Task<ActionResult> ByURL(string url)
        {
            if (Cache.TryGetValue(url, out FileNode cachedData))
            {
                return File(cachedData.Data, cachedData.ContentType);
            }

            //byte[] dataFromUrl = new WebClient().DownloadData(url);
            byte[] dataFromUrl = await this.DownloadDataAsync(url);
            var bm = SKBitmap.Decode(dataFromUrl);
            var rs = await this.DoSimple(bm);
            Cache.Set(url, rs);
            return File(rs.Data, rs.ContentType);
        }

        [HttpPost]
        public async Task<ActionResult> ByFile(IFormFile file)
        {
            SKBitmap bm = file.ToSKBitmap();
            var rs = await this.DoSimple(bm);
            return File(rs.Data, rs.ContentType);
        }

        private async Task<byte[]> DownloadDataAsync(string url)
        {
            using var client = new System.Net.Http.HttpClient();
            var reply = await client.GetByteArrayAsync(url);
            return reply;
        }

        [HttpGet]
        public async Task<ActionResult> ByURLAdvanced(UrlRenderRequest model)
        {
            string key = model.GetCacheKey();
            if (Cache.TryGetValue(key, out FileNode cd))
            {
                if (cd.SuggestedFileName == null)
                    return File(cd.Data, cd.ContentType);
                else
                    return File(cd.Data, cd.ContentType, cd.SuggestedFileName);
            }

            //byte[] dataFromUrl = new WebClient().DownloadData(model.Url);
            byte[] dataFromUrl = await this.DownloadDataAsync(model.Url);
            var bm = SKBitmap.Decode(dataFromUrl);
            var node = await DoAdvanced(model, bm);
            Cache.Set(key, node);
            return node.SuggestedFileName == null ? File(node.Data, node.ContentType) : File(node.Data, node.ContentType, node.SuggestedFileName);
        }

        [HttpPost]
        public async Task<ActionResult> ByFileAdvanced(FileRenderRequest model)
        {
            SKBitmap bm = model.File.ToSKBitmap();
            FileNode node = await DoAdvanced(model, bm);
            return node.SuggestedFileName == null
                ? File(node.Data, node.ContentType)
                : File(node.Data, node.ContentType, node.SuggestedFileName);
        }

        private async Task<FileNode> DoSimple(SKBitmap bm)
        {
            var engine = new RenderCanvasEngine();
            var palette = MaterialPalette.FromResx();
            var preprocessed = await engine.PreprocessImageAsync(null, bm, new Logic.IO.Config.CanvasPreprocessorSettings()
            {
                MaxHeight = 300,
                MaxWidth = 300,
                RgbBucketSize = 1,
                QuantizerSettings = new Logic.IO.Config.QuantizerSettings()
                {
                    IsEnabled = false
                }
            });

            bool isMultiLayer = true;
            bool isSideView = false;
            var mapper = ColorMapperContainer.GetColorMapper(isSideView, isMultiLayer, ColorMapperContainer.DefaultColorMapperAlgorithmTitle.Value);
            var canvas = await engine.RenderCanvasAsync(null, preprocessed, mapper, palette, isSideView);
            IExportFormatter exporter = new JpegFormatter();
            byte[] data = await exporter.ExportAsync(new PixelStackerProjectData()
            {
                CanvasData = canvas.CanvasData,
                IsSideView = isSideView,
                MaterialPalette = palette,
                PreprocessedImage = canvas.PreprocessedImage,
                //WorldEditOrigin = new int[] { (int)canvas.WorldEditOrigin.X, (int)canvas.WorldEditOrigin.Y }
                WorldEditOrigin = null
            }, null);

            return new FileNode(data, ExportFormat.Jpeg.GetContentTypeData().contentType);
        }

        private async Task<FileNode> DoAdvanced(BaseRenderRequest model, SKBitmap bm)
        {
            var engine = new RenderCanvasEngine();
            var palette = MaterialPalette.FromResx();
            bool isv = model.IsSideView;
            using var preprocessed = await engine.PreprocessImageAsync(null, bm, new CanvasPreprocessorSettings()
            {
                MaxHeight = Math.Clamp(model.MaxHeight ?? 200, 4, 4000),
                MaxWidth = Math.Clamp(model.MaxWidth ?? 200, 4, 4000),
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

            if (string.IsNullOrEmpty(model.ColorMapperAlgorithm) || !ColorMapperContainer.ColorMapperTypes.Value.ContainsKey(model.ColorMapperAlgorithm))
            {
                model.ColorMapperAlgorithm = ColorMapperContainer.DefaultColorMapperAlgorithmTitle.Value;
            }
            
            var mapper = ColorMapperContainer.GetColorMapper(isv, model.IsMultiLayer, model.ColorMapperAlgorithm);
            var canvas = await engine.RenderCanvasAsync(null, preprocessed, mapper, palette, isv);
            IExportFormatter exporter = model.Format.GetFormatter();


            var pxdat = new PixelStackerProjectData()
            {
                CanvasData = canvas.CanvasData,
                IsSideView = isv,
                MaterialPalette = palette,
                PreprocessedImage = canvas.PreprocessedImage,
                WorldEditOrigin = null
            };

            byte[] data = (exporter is IExportImageFormatter imgExporter)
                ? await imgExporter.ExportAsync(pxdat, new CanvasViewerSettings() { IsShadowRenderingEnabled = model.EnableShadows }, null)
                : await exporter.ExportAsync(pxdat, null);

            var (contentType, fileExt) = model.Format.GetContentTypeData();
            if (contentType == "application/octet-stream")
            {
                return new FileNode(data, contentType)
                {
                    SuggestedFileName = "download" + fileExt
                };
            }
            else
            {
                // Image style
                return new FileNode(data, contentType);
            }
        }
    }
}
