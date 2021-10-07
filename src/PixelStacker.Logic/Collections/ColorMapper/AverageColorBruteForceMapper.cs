using PixelStacker.Extensions;
using PixelStacker.Logic.Model;
using System;
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

        public void SetSeedData(List<MaterialCombination> combos,  MaterialPalette mats, bool isSideView)
        {
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
            var found = Combos.OrderBy(x => x.GetAverageColor(IsSideView).GetColorDistance(c))
                .Take(maxMatches).ToList();

            return found;
        }
    }
}
