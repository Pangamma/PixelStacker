using PixelStacker.Core;
using PixelStacker.Core.Model;
using PixelStacker.Core.Model.Drawing;
using PixelStacker.Core.Model.Mats;

namespace PixelStacker.Core.Collections.ColorMapper
{
    public class AverageColorBruteForceMapper : IColorMapper
    {
        private Dictionary<PxColor, MaterialCombination> Cache { get; set; } = new Dictionary<PxColor, MaterialCombination>();
        public List<MaterialCombination> Combos { get; private set; }
        public bool IsSideView { get; private set; }
        public MaterialPalette Palette { get; private set; }

        public double AccuracyRating => 53.725;

        public double SpeedRating => 1425.7;

        public void SetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView)
        {
            Cache = null;
            Cache = new Dictionary<PxColor, MaterialCombination>();
            Combos = combos;
            IsSideView = isSideView;
            Palette = mats;
        }

        public MaterialCombination FindBestMatch(PxColor c)
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
        public List<MaterialCombination> FindBestMatches(PxColor c, int maxMatches)
        {
            if (c.A < 32) return new List<MaterialCombination>() { Palette[Constants.MaterialCombinationIDForAir] };
            var found = Combos.OrderBy(x => x.GetAverageColor(IsSideView).GetColorDistance(c))
                .Take(maxMatches).ToList();

            return found;
        }

        public bool IsSeeded() => Cache.Count > 0;
    }
}
