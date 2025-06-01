using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Logic.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PixelStacker.Logic.Collections.ColorMapper;
using System.Threading;
using PixelStacker.Logic.Collections.ColorMapper.DistanceFormulas;

namespace PixelStacker.Logic.API
{
    public class PixelStackerApi
    {
        public async Task<FileNode> RenderFromBytes(byte[] imageBytes, RenderRequest model, CancellationToken? worker = null)
        {
            SKBitmap bm = SKBitmap.Decode(imageBytes);
            FileNode node = await DoAdvanced(model, bm, worker);
            return node;
        }

        public async Task<FileNode> RenderFromSkBitmap(SKBitmap bm, RenderRequest model, CancellationToken? worker = null)
        {
            FileNode node = await DoAdvanced(model, bm, worker);
            return node;
        }

        private async Task<FileNode> DoAdvanced(RenderRequest model, SKBitmap bm, CancellationToken? worker)
        {
            var engine = new RenderCanvasEngine();
            var palette = MaterialPalette.FromResx();
            bool isv = model.IsSideView;
            using var preprocessed = await engine.PreprocessImageAsync(null, bm, new CanvasPreprocessorSettings()
            {
                MaxHeight = Math.Clamp(model.MaxHeight ?? 200, 4, 20000),
                MaxWidth = Math.Clamp(model.MaxWidth ?? 200, 4, 20000),
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

            IColorMapper mapper = null;
            if (model.CustomColorMapper == null)
            {
                mapper = ColorMapperContainer.GetColorMapper(isv, model.IsMultiLayer, model.TextureMatchingStrategy, model.ColorDistanceFormulaType);
            }
            else
            {
                mapper = model.CustomColorMapper;
                if (!mapper.IsSeeded)
                {
                    throw new ArgumentException("CustomColorMapper must be seeded already.");
                }
            }
                
            var canvas = await engine.RenderCanvasAsync(worker, preprocessed, mapper, palette, isv);
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
                ? await imgExporter.ExportAsync(pxdat, new CanvasViewerSettings() { IsShadowRenderingEnabled = model.EnableShadows }, worker)
                : await exporter.ExportAsync(pxdat, worker);

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
