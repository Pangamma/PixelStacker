using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using KdTree;
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

    [HtmlExporter]
    //[ShortRunJob]
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1)]
    [BaselineColumn, MinColumn, MaxColumn,
        //MeanColumn,
        MedianColumn, IterationsColumn]
    public class KdTreeBenchmarks
    {
        private int Increment = 51;
        public List<MaterialCombination> Materials { get; }
        public List<int[]> ColorsToTryAndMatch { get; }

        public KdTreeBenchmarks()
        {
            this.Materials = MaterialPalette.FromResx().ToCombinationList();
            this.Materials.Select(x => x.GetAverageColor(false));
            this.ColorsToTryAndMatch = new List<int[]>();

            for (int r = 0; r < 256; r += Increment)
            {
                for (int g = 0; g < 256; g += Increment)
                {
                    for (int b = 0; b < 256; b += Increment)
                    {
                        ColorsToTryAndMatch.Add(new int[] { r, g, b });
                    }
                }
            }
        }

        // https://github.com/codeandcats/KdTree
        [Benchmark]
        public void CodeAndCats()
        {
            var tree = new KdTree<float, MaterialCombination>(3, new KdTree.Math.FloatMath());
            foreach(var mc in this.Materials)
            {
                Color c = mc.GetAverageColor(false);
                tree.Add(new float[] { c.R, c.G, c.B }, mc);
            }

            foreach(var q in this.ColorsToTryAndMatch)
            {
                var nodes = tree.GetNearestNeighbours(new float[] { q[0], q[1], q[2] }, 10);
            }
        }

        //public void SuperCluster()
        //{
        //    var tree = new Supercluster.KDTree.KDTree<int,MaterialCombination>(3, )
        //    tree.Add(new[] { 50.0f, 80.0f }, 100);
        //    tree.Add(new[] { 20.0f, 10.0f }, 200);

        //    var nodes = tree.GetNearestNeighbours(new[] { 30.0f, 20.0f }, 1);
        //}
    }
}
