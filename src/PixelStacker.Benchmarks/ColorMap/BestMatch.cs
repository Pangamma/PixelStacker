using BenchmarkDotNet.Attributes;
using PixelStacker.Logic.Collections.ColorMapper;
using SkiaSharp;

namespace PixelStacker.Benchmarks.ColorMap
{
    public class BestMatch: BaseColorMapBenchmarks
    {
        //[Params(15, 17, 51)]
        public int Increment { get; set; } = 15;

        /// <summary>
        /// The number of top matches to fetch. So like "get me the top 5 matches".
        /// </summary>
        //[Params(1, 5, 10, 20)]
        public int TopMatchN { get; set; } = 5;

        public BestMatch(): base() { }


        [Benchmark(Baseline = true)]
        public void AverageColorBruteForce()
        {
            var mapper = ColorMapperContainer.CreateColorMapper(TextureMatchingStrategy.Rough, Logic.Collections.ColorMapper.DistanceFormulas.ColorDistanceFormulaType.Rgb);
            mapper.SetSeedData(EnabledMaterials, MaterialPalette, false);
            FindBestMatch(mapper);
        }
        /*
        [Benchmark]
        public void SeparateColorBruteForce()
        {
            var mapper = new SeparateColorBruteForceMapper();
            mapper.SetSeedData(EnabledMaterials, MaterialPalette, false);
            FindBestMatch(mapper);
        }
        */

        [Benchmark]
        public void KdTree()
        {
            var mapper = ColorMapperContainer.CreateColorMapper(TextureMatchingStrategy.Smooth, Logic.Collections.ColorMapper.DistanceFormulas.ColorDistanceFormulaType.RgbWithHue);
            mapper.SetSeedData(EnabledMaterials, MaterialPalette, false);
            FindBestMatch(mapper);
        }

        private void FindBestMatch(IColorMapper mapper)
        {
            for (int r = 0; r < 256; r += Increment)
            {
                for (int g = 0; g < 256; g += Increment)
                {
                    for (int b = 0; b < 256; b += Increment)
                    {
                        var _ = mapper.FindBestMatch(new SKColor((byte)r, (byte)g, (byte)b, 255));
                    }
                }
            }
        }
    }
}
