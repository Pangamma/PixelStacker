using PixelStacker.Extensions;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PixelStacker.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var prog = new Program();
            prog.Setup();
            await prog.IE_PixelStackerProjectFormat();
        }


        public async Task IE_PixelStackerProjectFormat()
        {
            var formatter = new PixelStackerProjectFormatter();
            await formatter.ExportAsync("io_test.zip", Canvas, null);
            var canv = await formatter.ImportAsync("io_test.zip", null);
        }

        private RenderedCanvas Canvas => Canvases["Heavy"].Value.Result;

        private Dictionary<string, AsyncLazy<RenderedCanvas>> Canvases = new Dictionary<string, AsyncLazy<RenderedCanvas>>();

        public void Setup()
        {
            Options opts = new MemoryOptionsProvider().Load();
            MaterialPalette palette = MaterialPalette.FromResx();
            var mapper = new KdTreeMapper();
            var combos = palette.ToCombinationList().Where(x => x.Top.IsEnabledF(opts) && x.Bottom.IsEnabledF(opts) && x.IsMultiLayer).ToList();
            mapper.SetSeedData(combos, palette, false);

            var engine = new RenderCanvasEngine();

            this.Canvases["Fast"] = new AsyncLazy<RenderedCanvas>(async () =>
            {
                var img = await engine.PreprocessImageAsync(null,
                    DevResources.pink_girl.To32bppBitmap(),
                    new CanvasPreprocessorSettings()
                    {
                        RgbBucketSize = 15,
                        MaxHeight = 10,
                        QuantizerSettings = new QuantizerSettings()
                        {
                            Algorithm = QuantizerAlgorithm.WuColor,
                            MaxColorCount = 64,
                            IsEnabled = false,
                            DitherAlgorithm = "No dithering"
                        }
                    });


                return await engine.RenderCanvasAsync(null, ref img, mapper, palette);
            });


            this.Canvases["Quantizer"] = new AsyncLazy<RenderedCanvas>(async () => {
                var img = await engine.PreprocessImageAsync(null,
                    DevResources.pink_girl.To32bppBitmap(),
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

                return await engine.RenderCanvasAsync(null, ref img, mapper, palette);
            });

            this.Canvases["Heavy"] = new AsyncLazy<RenderedCanvas>(async () => {
                var img = await engine.PreprocessImageAsync(null,
                    DevResources.pink_girl.To32bppBitmap(),
                    new CanvasPreprocessorSettings()
                    {
                        RgbBucketSize = 1,
                        MaxWidth = 1024,
                        QuantizerSettings = new QuantizerSettings()
                        {
                            Algorithm = QuantizerAlgorithm.WuColor,
                            MaxColorCount = 256,
                            IsEnabled = false,
                            DitherAlgorithm = "No dithering"
                        }
                    });

                return await engine.RenderCanvasAsync(null, ref img, mapper, palette);
            });

            var preLoadIt = this.Canvas;
        }
    }


}
