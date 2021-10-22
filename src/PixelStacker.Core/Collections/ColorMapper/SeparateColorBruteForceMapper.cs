using PixelStacker.Core;
using PixelStacker.Core.Model;
using PixelStacker.Core.Model.Drawing;
using PixelStacker.Core.Model.Mats;

namespace PixelStacker.Core.Collections.ColorMapper
{
    public class SeparateColorBruteForceMapper : IColorMapper
    {
        private Dictionary<PxColor, MaterialCombination> Cache { get; set; } = new Dictionary<PxColor, MaterialCombination>();
        public List<MaterialCombination> Combos { get; private set; }
        public bool IsSideView { get; private set; }

        public double AccuracyRating => 100;

        public double SpeedRating => 4856.9;

        private MaterialPalette Palette;

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
            var found = Combos.MinBy(x => x.GetAverageColorDistance(IsSideView, c));
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
            var found = Combos.OrderBy(x => x.GetAverageColorDistance(IsSideView, c))
                .Take(maxMatches).ToList();

            return found;
        }

        public bool IsSeeded() => Combos != null;
    }
}
