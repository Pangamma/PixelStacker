using BenchmarkDotNet.Attributes;
using PixelStacker.Logic.Collections.ColorMapper;
using System.Drawing;

namespace PixelStacker.Benchmarks.ColorMap
{
    public class BestMatches: BaseColorMapBenchmarks
    {
        //[Params(/*1, 5, 15, 17, */ 51)]
        public int Increment { get; set; } = 17;

        /// <summary>
        /// The number of top matches to fetch. So like "get me the top 5 matches".
        /// </summary>
        //[Params(1, 5 /*,10, 20*/)]
        public int TopMatchN { get; set; } = 10;

        public BestMatches(): base() { }

        [Benchmark(Baseline = true)]
        public void AverageColorBruteForceMapper_BestMatches()
        {
            var mapper = new AverageColorBruteForceMapper();
            mapper.SetSeedData(EnabledMaterials, MaterialPalette, false);
            FindBestMatches(mapper);
        }

        [Benchmark]
        public void SeparateColorBruteForceMapper_BestMatches()
        {
            var mapper = new SeparateColorBruteForceMapper();
            mapper.SetSeedData(EnabledMaterials, MaterialPalette, false);
            FindBestMatches(mapper);
        }


        [Benchmark]
        public void KdTree()
        {
            var mapper = new KdTreeMapper();
            mapper.SetSeedData(EnabledMaterials, MaterialPalette, false);
            FindBestMatches(mapper);
        }

        private void FindBestMatches(IColorMapper mapper)
        {
            for (int r = 0; r < 256; r += Increment)
            {
                for (int g = 0; g < 256; g += Increment)
                {
                    for (int b = 0; b < 256; b += Increment)
                    {
                        var _ = mapper.FindBestMatches(Color.FromArgb(255, r, g, b), TopMatchN);
                    }
                }
            }
        }
    }
}
