using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KdTree.Math;
using PixelStacker.Logic.Extensions;
using SkiaSharp;

namespace PixelStacker.Logic.Collections.ColorMapper.DistanceFormulas
{
    public class HslFloatMath : FloatMath
    {
        public override float DistanceSquaredBetweenPoints(float[] a, float[] b)
        {
            float dHue = ExtendColor.GetDegreeDistance(a[0], b[0]);
            float dSat = a[1] - b[1];
            float dLightness = a[2] - b[2];
            float sum
                = (float) Math.Sqrt(dHue * dHue * dHue)
                + dSat * dSat
                + dLightness * dLightness
                ;
            return sum;
        }
    }

    public class HslDistanceFormula : IColorDistanceFormula
    {
        public TypeMath<float> KdTreeMath => new HslFloatMath();

        public ColorDistanceFormulaType Key => ColorDistanceFormulaType.Hsl;

        string IColorDistanceFormula.Label => "HSL";

        /// <summary>
        /// Returns [H, S, L]
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public float[] CalculateDimensionsForKdTree(SKColor c)
        {
            float[] f1 = c.ToHslArray();
            return f1;
        }

        int IColorDistanceFormula.CalculateColorDistance(SKColor c, SKColor c2)
        {
            float[] f1 = CalculateDimensionsForKdTree(c);
            float[] f2 = CalculateDimensionsForKdTree(c2);
            float distanceSquared = this.KdTreeMath.DistanceSquaredBetweenPoints(f1, f2);
            return (int)Math.Sqrt(distanceSquared);
        }
    }
}
