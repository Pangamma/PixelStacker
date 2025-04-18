using BenchmarkDotNet.Attributes;
using PixelStacker.Logic.CanvasEditor;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PixelStacker.Benchmarks.ColorMap
{

    [AsciiDocExporter]
    //[CsvExporter]
    //[CsvMeasurementsExporter]
    [HtmlExporter]
    [PlainExporter]
    //[RPlotExporter]
    //[MarkdownExporter]
    //[ShortRunJob]

    [JsonExporterAttribute.Brief]
    //[SimpleJob(launchCount: 1, warmupCount: 2, targetCount: 2)]
    [Config(typeof(FastAndDirtyConfig))]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn, IterationsColumn]
    public class BaseRenderedCanvasPainterBenchmarks
    {
        protected Dictionary<string, AsyncLazy<RenderedCanvas>> Canvases = new Dictionary<string, AsyncLazy<RenderedCanvas>>();

        public BaseRenderedCanvasPainterBenchmarks()
        {
            this.MaterialPalette = ResxHelper.LoadJson<MaterialPalette>(Resources.Data.materialPalette);
            this.EnabledMaterials = MaterialPalette.ToCombinationList();

            var opts = new MemoryOptionsProvider().Load();
            MaterialPalette palette = MaterialPalette.FromResx();
            var mapper = new KdTreeMapper();
            var combos = palette.ToValidCombinationList(opts);
            mapper.SetSeedData(combos, palette, false);
            var engine = new RenderCanvasEngine();

            this.Canvases["Fast"] = new AsyncLazy<RenderedCanvas>(async () =>
            {
                var img = await engine.PreprocessImageAsync(null,
                    DevResources.elsa,
                    new CanvasPreprocessorSettings()
                    {
                        RgbBucketSize = 15,
                        MaxHeight = 10,
                        MaxWidth = 500,
                        QuantizerSettings = new QuantizerSettings()
                        {
                            Algorithm = QuantizerAlgorithm.WuColor,
                            MaxColorCount = 64,
                            IsEnabled = false,
                            DitherAlgorithm = "No dithering"
                        }
                    });


                return await engine.RenderCanvasAsync(null, img, mapper, palette);
            });

            this.Canvases["Quantizer"] = new AsyncLazy<RenderedCanvas>(async () => {
                var img = await engine.PreprocessImageAsync(null,
                    DevResources.elsa,
                    new CanvasPreprocessorSettings()
                    {
                        RgbBucketSize = 1,
                        QuantizerSettings = new QuantizerSettings()
                        {
                            Algorithm = QuantizerAlgorithm.WuColor,
                            MaxColorCount = 32,
                            IsEnabled = true,
                            DitherAlgorithm = "No dithering"
                        }
                    });

                return await engine.RenderCanvasAsync(null, img, mapper, palette);
            });

            this.Canvases["Heavy"] = new AsyncLazy<RenderedCanvas>(async () => {
                var img = await engine.PreprocessImageAsync(null,
                    DevResources.elsa,
                    new CanvasPreprocessorSettings()
                    {
                        RgbBucketSize = 1,
                        QuantizerSettings = new QuantizerSettings()
                        {
                            Algorithm = null,
                            MaxColorCount = 256,
                            IsEnabled = false,
                            DitherAlgorithm = "No dithering"
                        }
                    });

                return await engine.RenderCanvasAsync(null, img, mapper, palette);
            });
        }

        public MaterialPalette MaterialPalette { get; }
        public List<MaterialCombination> EnabledMaterials { get; }

        [Benchmark]
        public async Task Render_Heavy()
        {
            var canvas = await this.Canvases["Heavy"].Value;
            var srs = new CanvasViewerSettings()
            {
                IsShadowRenderingEnabled = true,
                IsShowBorder = true,
                TextureSize = 16,
            };

            var painter = await RenderedCanvasPainter.Create(System.Threading.CancellationToken.None, canvas, srs);
        }

        [Benchmark]
        public async Task Render_Heavy_W_MaterialFilter()
        {
            var canvas = await this.Canvases["Heavy"].Value;
            var srs = new CanvasViewerSettings()
            {
                IsShadowRenderingEnabled = true,
                IsShowBorder = true,
                TextureSize = 16,
                VisibleMaterialsFilter = new HashSet<string>() { "GLASS_00", "DIRT" }
            };

            var painter = await RenderedCanvasPainter.Create(System.Threading.CancellationToken.None, canvas, srs);
        }
    }
}
