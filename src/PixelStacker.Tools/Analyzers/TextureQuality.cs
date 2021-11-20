using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.Model;
using System.Linq;

namespace PixelStacker.Tools.Analyzers
{
    public class TextureQuality
    {
        [TestMethod]
        [TestCategory("Research")]
        public void AnalyzeRoughnessOfTextures()
        {
            var palette = MaterialPalette.FromResx().ToCombinationList();
            var data = palette.Select(mc => new
            {
                mc = mc,
                AvgColor = mc.GetAverageColor(false),
                ErrorDist = mc.GetAverageColor(false).GetAverageColorDistance(mc.GetColorsInImage(false))
            }).AverageByPercentile(5, x => x.ErrorDist);
        }

        [TestMethod]
        [TestCategory("Research")]
        public void AnalyzeColorDiversity()
        {
            var palette = MaterialPalette.FromResx().ToCombinationList();
            var data = palette.Select(mc => new
            {
                mc = mc,
                ColorsInImage = mc.GetColorsInImage(false)
            }).AverageByPercentile(5, x => x.ColorsInImage.Count);
        }
    }
}
