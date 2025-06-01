using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KdTree;
using PixelStacker.Logic.Collections.ColorMapper.DistanceFormulas;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using SkiaSharp;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    public class KdTreeColorMapper : IColorMapper
    {
        private Dictionary<SKColor, MaterialCombination> Cache { get; set; } = new Dictionary<SKColor, MaterialCombination>();

        private bool IsSideView;
        private MaterialPalette Palette;
        private KdTree<float, MaterialCombination> KdTree;
        private object Padlock = new { };

        public KdTreeColorMapper(IColorDistanceFormula distanceAlgorithm, TextureMatchingStrategy strategy) : base(distanceAlgorithm, strategy)
        {
        }

        public override MaterialCombination FindBestMatch(SKColor c)
        {
            if (Cache.TryGetValue(c, out MaterialCombination mc))
            {
                return mc;
            }

            lock (Padlock)
            {
                if (c.Alpha < 32) return Palette[Constants.MaterialCombinationIDForAir];
                float[] metrics = this.DistanceFormula.CalculateDimensionsForKdTree(c);
                var closest = KdTree.GetNearestNeighbours(metrics, 10);

                KdTreeNode<float, MaterialCombination> found;
                if (this.TextureMatchingStrategy == TextureMatchingStrategy.Smooth)
                {
                    found = closest.MinBy(x => c.GetAverageColorDistance(x.Value.GetColorsInImage(this.IsSideView), this.DistanceFormula.CalculateColorDistance));
                    
                } 
                else
                {
                    found = closest.MinBy(x => this.DistanceFormula.CalculateColorDistance(c, x.Value.GetAverageColor(this.IsSideView)));
                }

                Cache[c] = found.Value;
                return found.Value;
            }
        }

        public override List<MaterialCombination> FindBestMatches(SKColor c, int maxMatches)
        {
            lock (Padlock)
            {
                if (c.Alpha < 32) return new List<MaterialCombination>() { Palette[Constants.MaterialCombinationIDForAir] };
                float[] metrics = this.DistanceFormula.CalculateDimensionsForKdTree(c);
                var closest = KdTree.GetNearestNeighbours(metrics, 10);
                List<MaterialCombination> found;

                if (this.TextureMatchingStrategy == TextureMatchingStrategy.Smooth)
                {
                    found = closest.OrderBy(x => c.GetAverageColorDistance(x.Value.GetColorsInImage(this.IsSideView), this.DistanceFormula.CalculateColorDistance))
                    .Take(maxMatches).Select(x => x.Value).ToList();

                }
                else
                {
                    found = closest.OrderBy(x => this.DistanceFormula.CalculateColorDistance(c, x.Value.GetAverageColor(this.IsSideView)))
                    .Take(maxMatches).Select(x => x.Value).ToList();
                }

                return found;
            }
        }

        protected override bool OnSetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView)
        {
            lock (Padlock)
            {
                this.Cache = null;
                this.Cache = new Dictionary<SKColor, MaterialCombination>();
                this.IsSideView = isSideView;
                this.Palette = mats;
                int numDimensions = this.DistanceFormula.CalculateDimensionsForKdTree(SKColor.Empty).Length;
                this.KdTree = new KdTree<float, MaterialCombination>(numDimensions, this.DistanceFormula.KdTreeMath);

                foreach (var cb in combos)
                {
                    var c = cb.GetAverageColor(isSideView);
                    var metrics = this.DistanceFormula.CalculateDimensionsForKdTree(c);
                    KdTree.Add(metrics, cb);
                }
            }

            return true;
        }
    }
}
