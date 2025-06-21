using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Engine;
using PixelStacker.Logic.Engine.Quantizer.Enums;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using System.IO;
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
            var mapper = ColorMapperContainer.CreateColorMapper(TextureMatchingStrategy.Rough, Logic.Collections.ColorMapper.DistanceFormulas.ColorDistanceFormulaType.RgbWithHue);
            var combos = palette.ToValidCombinationList(opts);
            mapper.SetSeedData(combos, palette, false);

            var canvas = await engine.RenderCanvasAsync(null, img, mapper, palette, opts.IsSideView);
            var data = await new PixelStackerProjectFormatter().ExportAsync(new PixelStackerProjectData(canvas, opts), null);
            File.WriteAllBytes("RenderTest.pxlzip", data);
        }
    }
}
