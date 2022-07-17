using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
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
#pragma warning disable CS0618 // Type or member is obsolete
            var opts = new Options(new MemoryOptionsProvider());
#pragma warning restore CS0618 // Type or member is obsolete
            var engine = new RenderCanvasEngine();
            var img = await engine.PreprocessImageAsync(null,
                DevResources.colorwheel.Copy(),
                new CanvasPreprocessorSettings()
                {
                    RgbBucketSize = 1,
                    QuantizerSettings = new QuantizerSettings()
                    {
                        Algorithm = QuantizerAlgorithm.WuColor,
                        MaxColorCount = 64,
                        IsEnabled = false,
                        DitherAlgorithm = "No dithering"
                    }
                });
            var palette = MaterialPalette.FromResx();
            var mapper = new KdTreeMapper();
            var combos = palette.ToCombinationList().Where(x => x.Top.IsEnabledF(opts) && x.Bottom.IsEnabledF(opts) && x.IsMultiLayer).ToList();
            mapper.SetSeedData(combos, palette, false);

            var canvas = await engine.RenderCanvasAsync(null, img, mapper, palette);
            await new PixelStackerProjectFormatter().ExportAsync("Test.pxlzip", new PixelStackerProjectData(canvas, opts), null);
        }
    }
}
