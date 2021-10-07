using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Extensions;
using PixelStacker.IO.Formatters;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using PixelStacker.Resources.Localization;
using System.Linq;
using System.Threading.Tasks;

namespace PixelStacker.Tools
{
    [TestClass]
    public class RenderTest
    {
        [TestMethod]
        public async Task RenderPinkGirl()
        {
            var engine = new RenderCanvasEngine();
            var img = await engine.PreprocessImageAsync(null, 
                UIResources.pink_girl.To32bppBitmap(),
                new Logic.Model.CanvasPreprocessorSettings()
                {
                    IsSideView = false,
                    RgbBucketSize = 15,
                    QuantizerSettings = new QuantizerSettings()
                    {
                        Algorithm = QuantizerAlgorithm.WuColor,
                        MaxColorCount = 4,
                        IsEnabled = true,
                        DitherAlgorithm = "No dithering"
                    }
                });
            var palette = ResxHelper.LoadJson<MaterialPalette>(DataResources.materialPalette);

            var mapper = new SeparateColorBruteForceMapper();
            var combos = palette.ToCombinationList().Where(x => x.Top.IsEnabled && x.Bottom.IsEnabled && x.IsMultiLayer).ToList();
            mapper.SetSeedData(combos, palette, false);

            var canvas = await engine.RenderCanvasAsync(null, img, mapper, palette);
            await new PixelStackerProjectFormatter().ExportAsync("Test.zip", canvas, null);
        }
    }
}
