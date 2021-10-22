using PixelStacker.Core.Model;
using PixelStacker.Core.Model.Drawing;
using PixelStacker.Core.Model.Mats;

namespace PixelStacker.Core.Collections.ColorMapper
{
    public interface IColorMapper
    {
        bool IsSeeded();
        void SetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView);
        MaterialCombination FindBestMatch(PxColor c);
        List<MaterialCombination> FindBestMatches(PxColor c, int maxMatches);

        /// <summary>
        /// 0-100 how accurate this is when compared to the SeparateColorBruteForce algorithm.
        /// </summary>
        double AccuracyRating { get; }

        /// <summary>
        /// Milliseconds taken from a benchmark test.
        /// </summary>
        double SpeedRating { get; }
    }
}
