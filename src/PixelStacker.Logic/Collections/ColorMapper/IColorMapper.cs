using PixelStacker.Logic.Model;
using System.Collections.Generic;
using SkiaSharp;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    public interface IColorMapper
    {
        bool IsSeeded();
        void SetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView);
        MaterialCombination FindBestMatch(SKColor c);
        List<MaterialCombination> FindBestMatches(SKColor c, int maxMatches);

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
