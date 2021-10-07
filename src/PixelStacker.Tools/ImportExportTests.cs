using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic.Model;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PixelStacker.IO.Formatters;
using PixelStacker.Extensions;
using System.Text;
using PixelStacker.Resources;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Resources.Localization;
using PixelStacker.Logic.Collections.ColorMapper;
using System.Linq;

namespace PixelStacker.Tools
{
    [TestClass]
    public class ImportExportTests
    {
        private RenderedCanvas Canvas;

        [TestInitialize]
        public async Task Setup()
        {
            MaterialPalette palette = MaterialPalette.FromResx();
            var engine = new RenderCanvasEngine();
            var img = await engine.PreprocessImageAsync(null,
                UIResources.pink_girl.To32bppBitmap(),
                new CanvasPreprocessorSettings()
                {
                    IsSideView = false,
                    RgbBucketSize = 15,
                    MaxHeight = 10,
                    QuantizerSettings = new QuantizerSettings()
                    {
                        Algorithm = QuantizerAlgorithm.WuColor,
                        MaxColorCount = 256,
                        IsEnabled = true,
                        DitherAlgorithm = "No dithering"
                    }
                });

            var mapper = new SeparateColorBruteForceMapper();
            var combos = palette.ToCombinationList().Where(x => x.Top.IsEnabled && x.Bottom.IsEnabled && x.IsMultiLayer).ToList();
            mapper.SetSeedData(combos, palette, false);

            this.Canvas = await engine.RenderCanvasAsync(null, img, mapper, palette);
        }

        [TestMethod]
        [TestCategory("IO")]
        public async Task IE_PixelStackerProjectFormat()
        {
            var formatter = new PixelStackerProjectFormatter();
            await formatter.ExportAsync("test.zip", Canvas, null);
            var canv = await formatter.ImportAsync("test.zip", null);
            Assert.AreEqual(Canvas.WorldEditOrigin, canv.WorldEditOrigin);
            Assert.AreEqual(JsonConvert.SerializeObject(Canvas.MaterialPalette), JsonConvert.SerializeObject(canv.MaterialPalette));
            Assert.AreEqual(Canvas.PreprocessedImage.Height, canv.PreprocessedImage.Height);
        }

        [TestMethod]
        [TestCategory("IO")]
        public async Task IE_SvgFormat()
        {
            var formatter = new SvgFormatter();
            await formatter.ExportAsync("test.svg", Canvas, null);
        }
    }
}
