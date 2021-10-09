using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Tools.Analyzerss
{
    [TestClass]
    public class ColorMapperAccuracy
    {
        private List<ColorMapComparison> Mappers;
        private const int Increment = 51;

        [TestInitialize]
        public void Setup()
        {
            this.Mappers = new List<ColorMapComparison>(){
                new ColorMapComparison(new SeparateColorBruteForceMapper(), true),
                new ColorMapComparison(new AverageColorBruteForceMapper()),
                new ColorMapComparison(new KdTreeMapper(), false),
                //new ColorMapComparison(new FloodFillMapper())
            };

            var palette = MaterialPalette.FromResx();
            foreach (var m in Mappers)
            {
                m.Mapper.SetSeedData(palette.ToCombinationList(), palette, false);
            }
        }


        [TestMethod]
        public void FindDifferences()
        {
            var baseline = Mappers.FirstOrDefault(x => x.IsBaseline);
            var other = Mappers.Where(x => !x.IsBaseline).ToList();

            for (int r = 0; r < 256; r += Increment)
            {
                for (int g = 0; g < 256; g += Increment)
                {
                    for (int b = 0; b < 256; b += Increment)
                    {
                        Color c = Color.FromArgb(255, r, g, b);
                        var expected = baseline.Mapper.FindBestMatch(c);
                        ++baseline.Hits;

                        foreach (var mapper in other)
                        {
                            Debug.WriteLine($"{mapper.Label} {r}-{g}-{b}");
                            var actual = mapper.Mapper.FindBestMatch(c);
                            if (actual == expected)
                            {
                                ++mapper.Hits;
                            }
                        }
                    }
                }
            }

            var results = Mappers.Select(x => new
            {
                M = x,
                Algorithm = x.Label,
                Accuracy = Math.Round(100 * ((double)x.Hits) / (baseline.Hits), 3)
            }).OrderByDescending(x => x.Accuracy);

            foreach (var r in results)
            {
                Debug.WriteLine($"{r.Accuracy.ToString().PadLeft(3, '0')}    {r.Algorithm}");
                Console.WriteLine($"{r.Accuracy.ToString().PadLeft(3, '0')}    {r.Algorithm}");
            }
        }
    }

    public class ColorMapComparison
    {
        public string Label { get; }
        public ColorMapComparison(IColorMapper mapper, bool isBaseline = false)
        {
            this.Mapper = mapper;
            this.IsBaseline = isBaseline;
            this.Label = mapper.GetType().Name;
        }

        public IColorMapper Mapper { get; set; }
        public bool IsBaseline { get; set; }
        public long Hits { get; set; }
    }
}
