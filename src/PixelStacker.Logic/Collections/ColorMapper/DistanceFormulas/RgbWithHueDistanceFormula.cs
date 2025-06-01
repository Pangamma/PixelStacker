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
    public class RgbWithHueDistanceFormula : IColorDistanceFormula
    {
        public TypeMath<float> KdTreeMath => new FloatMath();

        string IColorDistanceFormula.Label => "RGB with Hue";

        public ColorDistanceFormulaType Key => ColorDistanceFormulaType.RgbWithHue;

        public float[] CalculateDimensionsForKdTree(SKColor c)
        {
            float[] a = new float[] { c.Red, c.Green, c.Blue };
            return a;
        }

        int IColorDistanceFormula.CalculateColorDistance(SKColor c, SKColor toMatch)
        {
            int dR = c.Red - toMatch.Red;
            int dG = c.Green - toMatch.Green;
            int dB = c.Blue - toMatch.Blue;
            double dHue = ExtendColor.GetDegreeDistance(c.GetHue(), toMatch.GetHue());

            int diff =
                 dR * dR
                 + dG * dG
                 + dB * dB
                 + (int)Math.Sqrt(dHue * dHue * dHue)
                 ;

            int diffSquare = (int)Math.Sqrt(diff);
            return diffSquare;
        }
    }

    public class RgbWithHueFloatMath : FloatMath
    {
        public override float DistanceSquaredBetweenPoints(float[] a, float[] b)
        {
            float R = a[0] - b[0];
            float G = a[1] - b[1];
            float B = a[2] - b[2];
            float dHue = ExtendColor.GetDegreeDistance(a[3], b[3]);

            float sum
                = (float)Math.Sqrt(dHue * dHue * dHue)
                + R * R
                + G * G
                + B * B
                ;

            return sum;
        }
    }

    public class RgbWithHueDistanceFormula2 : IColorDistanceFormula
    {
        public TypeMath<float> KdTreeMath => new RgbWithHueFloatMath();

        string IColorDistanceFormula.Label => "RGB with Hue";

        public ColorDistanceFormulaType Key => ColorDistanceFormulaType.RgbWithHue;

        public float[] CalculateDimensionsForKdTree(SKColor c)
        {
            float[] a = new float[] { c.Red, c.Green, c.Blue, (float)c.GetHue() };
            return a;
        }

        int IColorDistanceFormula.CalculateColorDistance(SKColor c, SKColor toMatch)
        {
            //int dR = c.Red - toMatch.Red;
            //int dG = c.Green - toMatch.Green;
            //int dB = c.Blue - toMatch.Blue;
            //double dHue = ExtendColor.GetDegreeDistance(c.GetHue(), toMatch.GetHue());

            //int diff =
            //     dR * dR
            //     + dG * dG
            //     + dB * dB
            //     + (int)Math.Sqrt(dHue * dHue * dHue)
            //     ;

            //return (int)Math.Sqrt(diff);

            int diff = (int)Math.Sqrt(KdTreeMath.DistanceSquaredBetweenPoints(
                CalculateDimensionsForKdTree(c),
                CalculateDimensionsForKdTree(toMatch)
            ));

            return diff;
        }
    }
}
