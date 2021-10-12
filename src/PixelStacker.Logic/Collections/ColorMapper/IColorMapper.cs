using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Collections
{
    public interface IColorMapper
    {
        bool IsSeeded();
        void SetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView);
        MaterialCombination FindBestMatch(Color c);
        List<MaterialCombination> FindBestMatches(Color c, int maxMatches);

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
