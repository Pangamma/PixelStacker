using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PixelStacker.Logic.Extensions
{
    // Sat: 0...1
    // Hue: 0...360
    // Brightness: 0...1
    public static class ExtendColor
    {
        /// <summary>
        /// Overlay the TOP color ontop of the BOTTOM color
        /// </summary>
        /// <param name="RGBA2_Bottom"></param>
        /// <param name="RGBA1_Top"></param>
        /// <returns></returns>
        public static Color OverlayColor(this Color RGBA2_Bottom, Color RGBA1_Top)
        {
            double alpha = Convert.ToDouble(RGBA1_Top.A) / 255;
            int R = (int) ((RGBA1_Top.R * alpha) + (RGBA2_Bottom.R * (1.0 - alpha)));
            int G = (int) ((RGBA1_Top.G * alpha) + (RGBA2_Bottom.G * (1.0 - alpha)));
            int B = (int) ((RGBA1_Top.B * alpha) + (RGBA2_Bottom.B * (1.0 - alpha)));
            return Color.FromArgb(255, R, G, B);
        }

        public static float GetDegreeDistance(float alpha, float beta)
        {
            float phi = Math.Abs(beta - alpha) % 360;       // This is either the distance or 360 - distance
            float distance = phi > 180 ? 360 - phi : phi;
            return distance;
        }

        /// <summary>
        /// Custom color matching algorithm
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float GetColorDistance(this Color c, Color toMatch)
        {
            int dR = (c.R - toMatch.R);
            int dG = (c.G - toMatch.G);
            int dB = (c.B - toMatch.B);
            float dHue = GetDegreeDistance(c.GetHue(), toMatch.GetHue());
            //float dSat = (c.GetSaturation() - toMatch.GetSaturation()) * 100;

            float diff = (
                (dR * dR)
                + (dG * dG)
                + (dB * dB)
                + (float)Math.Pow(dHue, 1.5)
                //+ (dSat * dSat)
                );

            return diff;
        }

        /// <summary>
        /// Does not normalize alpha channels
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Color Normalize(this Color c)
        {
            int F = Options.Get.PreRender_ColorCacheFragmentSize;
            int R = (int) Math.Min(255, Math.Round(Convert.ToDecimal(c.R) / F, 0) * F);
            int G = (int) Math.Min(255, Math.Round(Convert.ToDecimal(c.G) / F, 0) * F);
            int B = (int) Math.Min(255, Math.Round(Convert.ToDecimal(c.B) / F, 0) * F);

            return Color.FromArgb(c.A, R, G, B);
        }

        public static IEnumerable<Color> OrderByColor(this IEnumerable<Color> source)
        {
            return source.OrderByColor(c => c);
        }

        public static IEnumerable<TSource> OrderByColor<TSource>(this IEnumerable<TSource> source, Func<TSource, Color> colorSelector)
        {
            source.Select(x => colorSelector(x));
            var grayscale = source.Where(x => colorSelector(x).GetSaturation() <= 0.20
            || colorSelector(x).GetBrightness() <= 0.15
            || colorSelector(x).GetBrightness() >= 0.85)
            .OrderByDescending(x => colorSelector(x).GetBrightness());
            const int numHueFragments = 18;

            var colorsInOrder = grayscale.ToList();

            //// Sat: 0...1
            //// Hue: 0...360
            //// Brightness: 0...1
            bool isAscendingBrightness = false;
            source.Except(grayscale)
                .GroupBy(x => (int)Math.Round(colorSelector(x).GetHue()) / numHueFragments)
                .OrderBy(x => x.Key)
                .ToList().ForEach(grouping =>
                {
                    isAscendingBrightness = !isAscendingBrightness;

                    if (isAscendingBrightness)
                    {
                        colorsInOrder.AddRange(grouping.OrderBy(g => colorSelector(g).GetBrightness()));
                    }
                    else
                    {
                        colorsInOrder.AddRange(grouping.OrderByDescending(g => colorSelector(g).GetBrightness()));
                    }
                });

            return colorsInOrder;
        }
    }
}
