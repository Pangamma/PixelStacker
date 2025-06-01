using PixelStacker.Logic.Model;
using System.Collections.Generic;
using SkiaSharp;
using PixelStacker.Logic.Collections.ColorMapper.DistanceFormulas;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    public enum TextureMatchingStrategy
    {
        /// <summary>
        /// Faster because it uses the average color of each texture when making comparisons. 
        /// Will appear rougher because some textures have a big range of unique colors in them.
        /// </summary>
        Rough,


        /// <summary>
        /// More accurate because it compares each pixel of every texture when making comparisons.
        /// Will appear smoother.
        /// </summary>
        Smooth
    }


    /// <summary>
    /// Consists of a color matching algorithm, and a storage algorithm.
    /// </summary>
    public abstract class IColorMapper
    {
        /// <summary>
        /// 0-100 how accurate this is when compared to the SeparateColorBruteForce algorithm.
        /// </summary>
        double AccuracyRating { get; } = 0.0;

        /// <summary>
        /// Milliseconds taken from a benchmark test.
        /// </summary>
        double SpeedRating { get; } = 0.0;

        string AlgorithmTitle { get; } = "";

        public IColorDistanceFormula DistanceFormula { get; }
        public TextureMatchingStrategy TextureMatchingStrategy { get; }

        public IColorMapper(IColorDistanceFormula distanceAlgorithm, TextureMatchingStrategy strategy)
        {
            this.DistanceFormula = distanceAlgorithm;
            this.TextureMatchingStrategy = strategy;
        }

        protected abstract bool OnSetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView);
        public abstract MaterialCombination FindBestMatch(SKColor c);
        public abstract List<MaterialCombination> FindBestMatches(SKColor c, int maxMatches);

        public bool IsSeeded { get; protected set; } = false;

        public void SetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView)
        {
            this.IsSeeded = this.OnSetSeedData(combos, mats, isSideView);
        }
    }
}
