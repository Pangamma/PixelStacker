using KdTree;
using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System.Collections.Generic;
using SkiaSharp;
using System.Linq;
using System;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    public class SrgbKdTreeMapper : IColorMapper
    {
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
                    var c = cb.GetAverageColor(isSideView);
                    float[] metrics = new float[] { c.Red * c.Red, c.Green * c.Green, c.Blue * c.Blue };
                    KdTree.Add(metrics, cb);
                }
            }
        }

        public MaterialCombination FindBestMatch(SKColor c)
        {
            lock (Padlock)
            {
                if (Cache.TryGetValue(c, out MaterialCombination mc))
                {
                    return mc;
                }

                if (c.Alpha < 32)return Palette[Constants.MaterialCombinationIDForAir];
                var closest = KdTree.GetNearestNeighbours(new float[] { c.Red * c.Red, c.Green * c.Green, c.Blue * c.Blue }, 10);
                var found = closest.MinBy(x => SRGBGetAverageColorDistance(c, x.Value.GetColorsInImage(this.IsSideView)));
                Cache[c] = found.Value;
                return found.Value;
            }
        }

        /// <summary>
        /// Use for SUPER accurate color distance checks. Very slow, but also very accurate.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static int SRGBGetAverageColorDistance(SKColor target, List<Tuple<SKColor, int>> src)
        {
            long r = 0;
            long t = 0;

            foreach (var c in src)
            {
                int dist = target.GetColorDistanceSRGB(c.Item1);
                r += dist * c.Item2;
                t += c.Item2;
            }

            r /= t;
            return (int)r;
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
                var closest = KdTree.GetNearestNeighbours(new float[] { c.Red * c.Red, c.Green * c.Green, c.Blue * c.Blue }, 10);
                var found = closest.OrderBy(x => SRGBGetAverageColorDistance(c, x.Value.GetColorsInImage(this.IsSideView)))
                    .Take(maxMatches).Select(x => x.Value).ToList();

                return found;
            }
        }

        public bool IsSeeded() => this.KdTree != null;
    }
}
