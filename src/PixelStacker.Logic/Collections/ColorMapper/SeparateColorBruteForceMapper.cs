using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    public class SeparateColorBruteForceMapper : IColorMapper
    {
        private Dictionary<SKColor, MaterialCombination> Cache { get; set; } = new Dictionary<SKColor, MaterialCombination>();
        public List<MaterialCombination> Combos { get; private set; }
        public bool IsSideView { get; private set; }

        public double AccuracyRating => 100;

        public double SpeedRating => 4856.9;

        private MaterialPalette Palette;

        public void SetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView)
        {
            this.Cache = null;
            this.Cache = new Dictionary<SKColor, MaterialCombination>();
            this.Combos = combos;
            this.IsSideView = isSideView;
            this.Palette = mats;
        }

        public MaterialCombination FindBestMatch(SKColor c)
        {
            if (Cache.TryGetValue(c, out MaterialCombination mc))
            {
                return mc;
            }

            if (c.Alpha < 32) return Palette[Constants.MaterialCombinationIDForAir];
            var found = Combos.MinBy(x => c.GetAverageColorDistance(x.GetColorsInImage(this.IsSideView)));
            Cache[c] = found;
            return found;
        }

        /// <summary>
        /// Super SLOW! Avoid if you can help it. Not at all optimized.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="maxMatches"></param>
        /// <returns></returns>
        public List<MaterialCombination> FindBestMatches(SKColor c, int maxMatches)
        {
            if (c.Alpha < 32) return new List<MaterialCombination>() { Palette[Constants.MaterialCombinationIDForAir] };
            var found = Combos.OrderBy(x => c.GetAverageColorDistance(x.GetColorsInImage(this.IsSideView)))
                .Take(maxMatches).ToList();

            return found;
        }

        public bool IsSeeded() => Combos != null;
    }
}
