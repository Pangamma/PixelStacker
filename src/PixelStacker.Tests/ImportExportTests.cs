using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Resources;
using PixelStacker.Logic.IO.Config;

namespace PixelStacker.Tests
{
    [TestClass]
    public class ImportExportTests
    {
        private RenderedCanvas Canvas => Canvases["Fast"].Value.Result;

        private Dictionary<string, AsyncLazy<RenderedCanvas>> Canvases = new Dictionary<string, AsyncLazy<RenderedCanvas>>();
        private Options Options;

        [TestInitialize]
        public void Setup()
        {
            Options opts = new MemoryOptionsProvider().Load();
            MaterialPalette palette = MaterialPalette.FromResx();
            this.Options = opts;
            var mapper = new KdTreeMapper();
            var combos = palette.ToValidCombinationList(opts);
            mapper.SetSeedData(combos, palette, false);

            var engine = new RenderCanvasEngine();

            this.Canvases["Fast"] = new AsyncLazy<RenderedCanvas>(async () => {
                var img = await engine.PreprocessImageAsync(null,
                    DevResources.pink_girl,
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


                return await engine.RenderCanvasAsync(null, img, mapper, palette);
            });


            this.Canvases["Quantizer"] = new AsyncLazy<RenderedCanvas>(async () => {
                var img = await engine.PreprocessImageAsync(null,
                    DevResources.pink_girl,
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

            //this.Canvases["Heavy"] = new AsyncLazy<RenderedCanvas>(async () => {
            //    var img = await engine.PreprocessImageAsync(null,
            //        DevResources.planet_8k,
            //        new CanvasPreprocessorSettings()
            //        {
            //            RgbBucketSize = 1,
            //            MaxWidth = 1024,
            //            QuantizerSettings = new QuantizerSettings()
            //            {
            //                Algorithm = QuantizerAlgorithm.WuColor,
            //                MaxColorCount = 256,
            //                IsEnabled = false,
            //                DitherAlgorithm = "No dithering"
            //            }
            //        });

            //    return await engine.RenderCanvasAsync(null, ref img, mapper, palette);
            //});

            var preLoadIt = this.Canvas;
        }

        [TestMethod]
        [TestCategory("IO")]
        public async Task IE_PixelStackerProjectFormat()
        {
            var formatter = new PixelStackerProjectFormatter();
            await formatter.ExportAsync("io_test.zip", new PixelStackerProjectData(Canvas, this.Options), null);
            var canv = await formatter.ImportAsync("io_test.zip", null);
            Assert.AreEqual(Canvas.WorldEditOrigin, canv.WorldEditOrigin);
            Assert.AreEqual(JsonConvert.SerializeObject(Canvas.MaterialPalette), JsonConvert.SerializeObject(canv.MaterialPalette));
            Assert.AreEqual(Canvas.PreprocessedImage.Height, canv.PreprocessedImage.Height);
        }


        [TestMethod]
        [TestCategory("IO")]
        public async Task IE_PngFormat()
        {
            var formatter = new PngFormatter();
            await formatter.ExportAsync("io_test.png", new PixelStackerProjectData(Canvas, this.Options), null);
        }
    }
}
