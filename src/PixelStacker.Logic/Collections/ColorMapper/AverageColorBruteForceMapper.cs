using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    public class AverageColorBruteForceMapper : IColorMapper
    {
        private Dictionary<Color, MaterialCombination> Cache { get; set; } = new Dictionary<Color, MaterialCombination>();
        public List<MaterialCombination> Combos { get; private set; }
        public bool IsSideView { get; private set; }
        public MaterialPalette Palette { get; private set; }

        public double AccuracyRating => 53.725;

        public double SpeedRating => 1425.7;

        public void SetSeedData(List<MaterialCombination> combos,  MaterialPalette mats, bool isSideView)
        {
            this.Cache = null;
            this.Cache = new Dictionary<Color, MaterialCombination>();
            this.Combos = combos;
            this.IsSideView = isSideView;
            this.Palette = mats;
        }

        public MaterialCombination FindBestMatch(Color c)
        {
            if (Cache.TryGetValue(c, out MaterialCombination mc))
            {
                return mc;
            }

            if (c.A < 32) return Palette[Constants.MaterialCombinationIDForAir];
            var found = Combos.MinBy(x => x.GetAverageColor(IsSideView).GetColorDistance(c));
            Cache[c] = found;
            return found;
        }

        /// <summary>
        /// Super SLOW! Avoid if you can help it. Not at all optimized.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="maxMatches"></param>
        /// <returns></returns>
        public List<MaterialCombination> FindBestMatches(Color c, int maxMatches)
        {
            if (c.A < 32) return new List<MaterialCombination>() { Palette[Constants.MaterialCombinationIDForAir] };
            var found = Combos.OrderBy(x => x.GetAverageColor(IsSideView).GetColorDistance(c))
                .Take(maxMatches).ToList();

            return found;
        }

        public bool IsSeeded() => Cache.Count > 0;
    }
}
