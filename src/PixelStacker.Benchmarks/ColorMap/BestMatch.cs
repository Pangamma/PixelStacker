using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PixelStacker.Logic;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using PixelStacker.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void AverageColorBruteForceMapper_BestMatch()
        {
            var mapper = new AverageColorBruteForceMapper();
            mapper.SetSeedData(EnabledMaterials, MaterialPalette, false);
            FindBestMatch(mapper);
        }

        [Benchmark]
        public void SeparateColorBruteForceMapper_BestMatch()
        {
            var mapper = new SeparateColorBruteForceMapper();
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
                        var _ = mapper.FindBestMatch(Color.FromArgb(255, r, g, b));
                    }
                }
            }
        }
    }
}
