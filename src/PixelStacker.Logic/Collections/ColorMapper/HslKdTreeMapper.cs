using KdTree;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System.Collections.Generic;
using SkiaSharp;
using System.Linq;
using System;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    public class HslKdTreeMapper : IColorMapper
    {
        public string AlgorithmTitle => "HSL Unique Color KdTree";
        public double AccuracyRating => 99.127;
        public double SpeedRating => 232.1;

        private Dictionary<SKColor, MaterialCombination> Cache { get; set; } = new Dictionary<SKColor, MaterialCombination>();

        private bool IsSideView;
        private MaterialPalette Palette;
        private KdTree<float, MaterialCombination> KdTree;
        private object Padlock = new { };

        public void SetSeedData(List<MaterialCombination> combos, MaterialPalette palette, bool isSideView)
        {
            lock (Padlock)
            {
                this.Cache = null;
                this.Cache = new Dictionary<SKColor, MaterialCombination>();
                this.IsSideView = isSideView;
                this.Palette = palette;
                this.KdTree = new KdTree<float, MaterialCombination>(3, new KdTree.Math.FloatMath());

                foreach (var cb in combos)
                {
                    float[] metrics = cb.GetAverageColor(isSideView).ToHslArray();
                    KdTree.Add(metrics, cb);
                }
            }
        }

        public MaterialCombination FindBestMatch(SKColor c)
        {
            if (Cache.TryGetValue(c, out MaterialCombination mc))
            {
                return mc;
            }

            lock (Padlock)
            {
                if (c.Alpha < 32) return Palette[Constants.MaterialCombinationIDForAir]; 
                float[] floats = c.ToHslArray();
                var closest = KdTree.GetNearestNeighbours(floats, 10);
                var found = closest.MinBy(x => c.GetAverageColorDistance(x.Value.GetColorsInImage(this.IsSideView), this.CalculateColorDistance));
                Cache[c] = found.Value;
                return found.Value;
            }
        }

        /// <summary>
        /// Super SLOW! Avoid if you can help it. Not at all optimized.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="maxMatches"></param>
        /// <returns></returns>
        public List<MaterialCombination> FindBestMatches(SKColor c, int maxMatches)
        {
            lock (Padlock)
            {
                if (c.Alpha < 32) return new List<MaterialCombination>() { Palette[Constants.MaterialCombinationIDForAir] };
                float[] floats = c.ToHslArray(); 
                var closest = KdTree.GetNearestNeighbours(floats, 10);
                var found = closest.OrderBy(x => c.GetAverageColorDistance(x.Value.GetColorsInImage(this.IsSideView), this.CalculateColorDistance))
                    .Take(maxMatches).Select(x => x.Value).ToList();

                return found;
            }
        }

        public bool IsSeeded() => this.KdTree != null;

        public int CalculateColorDistance(SKColor c, SKColor c2)
        {
            float[] f1 = c.ToHslArray();
            float[] f2 = c2.ToHslArray();
            float[] delta = new float[] { ExtendColor.GetDegreeDistance(f1[0], f2[0])/180*100, f1[1] - f2[1], f1[2] - f2[2] };
            float diff
                = (float)Math.Sqrt(delta[0] * delta[0] * delta[0])
                + delta[1] * delta[1]
                + delta[2] * delta[2];
            return (int)Math.Sqrt(diff);

            //float diff = delta.Sum(n => Math.Abs(n));
            //return (int)diff;
        }
    }
}
