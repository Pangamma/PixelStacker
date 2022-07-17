using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Factory;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Resources;
using System.Threading.Tasks;

namespace PixelStacker.Tests
{
    [TestClass]
    public class PixelFactoryTests
    {
        [TestMethod]
        public async Task RunThroughIt()
        {
            var foo = PixelStackerFactory
                .Builder()
                .WithOrientation(false)
                .WithColorMapper<KdTreeMapper>()
                .SetSeedData()
                .SkipPreprocessing()
                .Build();

            var output = await foo.ExportAsync(null, ExportFormat.Png, DevResources.psg);
        }
    }
}
