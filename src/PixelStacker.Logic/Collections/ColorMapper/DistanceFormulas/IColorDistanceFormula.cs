using KdTree.Math;
using SkiaSharp;

namespace PixelStacker.Logic.Collections.ColorMapper.DistanceFormulas
{
    public enum ColorDistanceFormulaType
    {
        Hsl,
        Rgb,
        RgbWithHue,
        Srgb
    }

    public interface IColorDistanceFormula
    {
        /// <summary>
        /// The human friendly display name of the distance algorithm.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// The unique key for the distance algorithm.
        /// Used by code.
        /// </summary>
        public ColorDistanceFormulaType Key { get; }

        /// <summary>
        /// Used in FindBestMatch(es) after the KdTree finishes working its magic to get you a smaller pool of items to look at.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        int CalculateColorDistance(SKColor c1, SKColor c2);

        /// <summary>
        /// Just use FloatMath if you are not certain.
        /// </summary>
        public TypeMath<float> KdTreeMath { get; }
        /// <summary>
        /// Example: [R, G, B]
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public float[] CalculateDimensionsForKdTree(SKColor c);
    }
}
