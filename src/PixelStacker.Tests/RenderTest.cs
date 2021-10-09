using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Extensions;
using PixelStacker.IO.Config;
using PixelStacker.IO.Formatters;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using PixelStacker.Resources.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace PixelStacker.Tests
{
    [TestClass]
    public class RenderTest
    {
        [TestMethod]
        public async Task RenderYuuuuugeImage()
        {
            var engine = new RenderCanvasEngine();
            var img = await engine.PreprocessImageAsync(null, 
                UIResources.yuuuuge.To32bppBitmap(),
                new CanvasPreprocessorSettings()
                {
                    IsSideView = false,
                    RgbBucketSize = 1,
                    QuantizerSettings = new QuantizerSettings()
                    {
                        Algorithm = QuantizerAlgorithm.WuColor,
                        MaxColorCount = 64,
                        IsEnabled = false,
                        DitherAlgorithm = "No dithering"
                    }
                });
            var palette = ResxHelper.LoadJson<MaterialPalette>(DataResources.materialPalette);
            var mapper = new KdTreeMapper();
            var combos = palette.ToCombinationList().Where(x => x.Top.IsEnabled && x.Bottom.IsEnabled && x.IsMultiLayer).ToList();
            mapper.SetSeedData(combos, palette, false);

            var canvas = await engine.RenderCanvasAsync(null, ref img, mapper, palette);
            await new PixelStackerProjectFormatter().ExportAsync("Test.zip", canvas, null);
        }
    }
}
