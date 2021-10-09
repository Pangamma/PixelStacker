//using KdTree;
//using PixelStacker.Logic.Collections;
//using PixelStacker.Logic.Model;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using Supercluster.KDTree;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using PixelStacker.Extensions;

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
//    [TestClass]
//    public class KdTreeAccuracyTests
//    {
//        private int Increment = 51;
//        private KdTree<float, MaterialCombination> CodeAndCatsKdTree;
//        private KDTree<MaterialCombination> CustomKdTree;
//        private KDTree<int, MaterialCombination> SuperClusterTree;

//        public List<MaterialCombination> Materials { get; }
//        public List<int[]> ColorsToTryAndMatch { get; }

//        public KdTreeAccuracyTests()
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
//                points: this.Materials.Select(x =>
//                {
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

//        [TestMethod]
//        [TestCategory("Research")]
//        public void SuperClusterTest()
//        {
//            var mats = Materials.ToList().Select(x => new { Color = x.GetAverageColor(false), MC = x }).ToList();
//            int[] ErrorCounts = new int[3];
//            foreach (var q in this.ColorsToTryAndMatch)
//            {
//                var truth = mats.MinBy(m =>
//                {
//                    int r = m.Color.R - q[0];
//                    int g = m.Color.G - q[1];
//                    int b = m.Color.B - q[2];
//                    return r * r + g * g + b * b;
//                }).MC;

//                var nodesA = this.SuperClusterTree.NearestNeighbors(new int[] { q[0], q[1], q[2] }, 20).Select(x => x.Item2).ToArray();
//                var nodesB = this.CustomKdTree.Nearest(new float[] { q[0], q[1], q[2] }, 10).Select(x => x.Node.Value).ToArray();
//                var nodesC = this.CodeAndCatsKdTree.GetNearestNeighbours(new float[] { q[0], q[1], q[2] }, 10).Select(x => x.Value).ToArray();

//                if (!nodesA.Contains(truth)) ErrorCounts[0]++;
//                if (!nodesB.Contains(truth)) ErrorCounts[1]++;
//                if (!nodesC.Contains(truth)) ErrorCounts[2]++;
//            }
//        }
//    }
//}
