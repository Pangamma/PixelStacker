using System;
using System.Collections.Generic;
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
    public class RgbDistanceFormula : IColorDistanceFormula
    {
        public TypeMath<float> KdTreeMath => new FloatMath();

        string IColorDistanceFormula.Label => "RGB";

        public ColorDistanceFormulaType Key => ColorDistanceFormulaType.Rgb;

        public float[] CalculateDimensionsForKdTree(SKColor c)
        {
            return new float[] { c.Red, c.Green, c.Blue };
        }

        int IColorDistanceFormula.CalculateColorDistance(SKColor c, SKColor toMatch)
        {
            int dR = c.Red - toMatch.Red;
            int dG = c.Green - toMatch.Green;
            int dB = c.Blue - toMatch.Blue;

            int diff =
                 dR * dR
                 + dG * dG
                 + dB * dB
                 ;

            return (int)Math.Sqrt(diff);
        }
    }
}
