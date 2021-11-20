//using BenchmarkDotNet.Attributes;
//using KdTree;
//using PixelStacker.Logic.Collections;
//using PixelStacker.Logic.Model;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using Supercluster.KDTree;

///// <summary>
///// https://en.wikipedia.org/wiki/Nearest_neighbor_search
///// TODO: KDTree from old program
///// Metric Tree
///// BK Tree
///// VP Tree
///// BSP Tree
///// </summary>
//namespace PixelStacker.Benchmarks.ColorMap
//{

//    [HtmlExporter]
//    [Config(typeof(FastAndDirtyConfig))]
//    //[ShortRunJob]
//    //[SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 2)]
//    [BaselineColumn, MinColumn, MaxColumn,
//        MeanColumn,
//        MedianColumn, IterationsColumn]
//    public class KdTreeBenchmarks
//    {
//        private int Increment = 51;
//        private KdTree<float, MaterialCombination> CodeAndCatsKdTree;
//        private KDTree<MaterialCombination> CustomKdTree;
//        private KDTree<int, MaterialCombination> SuperClusterTree;

//        public List<MaterialCombination> Materials { get; }
//        public List<int[]> ColorsToTryAndMatch { get; }

//        public KdTreeBenchmarks()
//        {
//            this.Materials = MaterialPalette.FromResx().ToCombinationList();
//            this.Materials.Select(x => x.GetAverageColor(false));
//            this.ColorsToTryAndMatch = new List<int[]>();

//            for (int r = 0; r < 256; r += Increment)
//            {
//                for (int g = 0; g < 256; g += Increment)
//                {
//                    for (int b = 0; b < 256; b += Increment)
//                    {
//                        ColorsToTryAndMatch.Add(new int[] { r, g, b });
//                    }
//                }
//            }

//            this.CodeAndCatsKdTree = new KdTree<float, MaterialCombination>(3, new KdTree.Math.FloatMath());
//            this.CustomKdTree = new KDTree<MaterialCombination>(3);
//            this.SuperClusterTree = new Supercluster.KDTree.KDTree<int, MaterialCombination>(
//                dimensions: 3,
//                points: this.Materials.Select(x => {
//                    var cc = x.GetAverageColor(false);
//                    return new int[] { cc.R, cc.G, cc.B };
//                }).ToArray(),
//                nodes: this.Materials.ToArray(),
//                metric: (a, b) =>
//                {
//                    float distance = 0;
//                    int dimensions = a.Length;

//                    // Return the absolute distance bewteen 2 hyper points
//                    for (var dimension = 0; dimension < dimensions; dimension++)
//                    {
//                        float distOnThisAxis = a[dimension] - b[dimension];
//                        float distOnThisAxisSquared = distOnThisAxis * distOnThisAxis;

//                        distance += distOnThisAxisSquared;
//                    }

//                    return distance;
//                },


//            //(double) Color.FromArgb(fA[0], fA[1],fA[2])
//            //    .GetColorDistance(Color.FromArgb(fB[0], fB[1], fB[2])),
//                searchWindowMaxValue: default(int),
//                searchWindowMinValue: default(int)
//            );

//            foreach (var mc in this.Materials)
//            {
//                Color c = mc.GetAverageColor(false);
//                this.CodeAndCatsKdTree.Add(new float[] { c.R, c.G, c.B }, mc);
//                this.CustomKdTree.Add(new float[] { c.R, c.G, c.B }, mc);
//            }

//        }

//        // https://github.com/codeandcats/KdTree
//        [Benchmark]
//        public void CodeAndCats()
//        {
//            foreach(var q in this.ColorsToTryAndMatch)
//            {
//                var nodes = this.CodeAndCatsKdTree.GetNearestNeighbours(new float[] { q[0], q[1], q[2] }, 10);
//            }
//        }

//        [Benchmark]
//        public void CustomKdTreeTest()
//        {
//            foreach (var q in this.ColorsToTryAndMatch)
//            {
//                var nodes = this.CustomKdTree.Nearest(new float[] { q[0], q[1], q[2] }, 10);
//            }
//        }

//        [Benchmark]
//        public void SuperClusterTest()
//        {
//            foreach (var q in this.ColorsToTryAndMatch)
//            {
//                var nodes = this.SuperClusterTree.NearestNeighbors(new int[] { q[0], q[1], q[2] }, 10);
//            }
//        }
//    }
//}
