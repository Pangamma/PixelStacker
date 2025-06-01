using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KdTree.Math;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.Model;
using SkiaSharp;

namespace PixelStacker.Logic.Collections.ColorMapper.DistanceFormulas
{
    public class SrgbDistanceFormula : IColorDistanceFormula
    {
        public TypeMath<float> KdTreeMath => new FloatMath();

        string IColorDistanceFormula.Label => "SRGB";

        public ColorDistanceFormulaType Key => ColorDistanceFormulaType.Srgb;

        public float[] CalculateDimensionsForKdTree(SKColor cOriginal)
        {
            var c = ExtendColor.ToSRGB(cOriginal);
            return new float[] { c.Red, c.Green, c.Blue };
        }

        int IColorDistanceFormula.CalculateColorDistance(SKColor c, SKColor toMatch)
        {
            int diff = (int) Math.Sqrt(KdTreeMath.DistanceSquaredBetweenPoints(
                CalculateDimensionsForKdTree(c),
                CalculateDimensionsForKdTree(toMatch)
            ));
            return diff;
        }
    }
}
