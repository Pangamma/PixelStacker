﻿using System;
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
            float sum = 0;

            for(int i = 0; i < a.Length; i++)
            {
                float tmp = Subtract(a[i], b[i]);
                sum += tmp * tmp;
            }


            return sum;
        }
    }

    public class HslDistanceFormula : IColorDistanceFormula
    {
        public TypeMath<float> KdTreeMath => new HslFloatMath();

        public ColorDistanceFormulaType Key => ColorDistanceFormulaType.Hsl;

        string IColorDistanceFormula.Label => "HSL";

        private static float[] ConvertHslToXYZ(float[] hsl)
        {
            const double fromDegreesToRadians = (Math.PI / 180);
            float rSaturation = hsl[1]; // Saturation
            double hueRadians = (hsl[0] * fromDegreesToRadians);

            float[] xyz = [
                (float)(rSaturation * Math.Cos(hueRadians)),
                (float)(rSaturation * Math.Sin(hueRadians)),
                hsl[2] // Lightness is up/down on the cylinder.
            ];

            return xyz;
        }


        /// <summary>
        /// Returns [H, S, L]
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public float[] CalculateDimensionsForKdTree(SKColor c)
        {
            float[] f1 = c.ToHslArray();
            f1 = ConvertHslToXYZ(f1);
            return f1;
        }

        int IColorDistanceFormula.CalculateColorDistance(SKColor c, SKColor c2)
        {
            float[] f1 = CalculateDimensionsForKdTree(c);
            float[] f2 = CalculateDimensionsForKdTree(c2);
            float distanceSquared = this.KdTreeMath.DistanceSquaredBetweenPoints(f1, f2);
            return (int)(Math.Sqrt(distanceSquared) * 1000);
        }
    }
}
