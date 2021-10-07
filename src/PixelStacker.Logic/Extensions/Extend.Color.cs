using PixelStacker.IO.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace PixelStacker.Extensions
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
            int R = (int)((RGBA1_Top.R * alpha) + (RGBA2_Bottom.R * (1.0 - alpha)));
            int G = (int)((RGBA1_Top.G * alpha) + (RGBA2_Bottom.G * (1.0 - alpha)));
            int B = (int)((RGBA1_Top.B * alpha) + (RGBA2_Bottom.B * (1.0 - alpha)));
            return Color.FromArgb(255, R, G, B);
        }

        public static Color AverageColors(this IEnumerable<Color> colors)
        {
            long r = 0;
            long g = 0;
            long b = 0;
            long a = 0;
            long total = colors.Count();
            foreach (var c in colors)
            {
                r += c.R;
                g += c.G;
                b += c.B;
                a += c.A;
            }

            r /= total;
            g /= total;
            b /= total;
            a /= total;

            return Color.FromArgb((int)a, (int)r, (int)g, (int)b);
        }

        public static float GetDegreeDistance(float alpha, float beta)
        {
            float phi = Math.Abs(beta - alpha) % 360;       // This is either the distance or 360 - distance
            float distance = phi > 180 ? 360 - phi : phi;
            return distance;
        }

        public static double GetDegreeDistance(double alpha, double beta)
        {
            double phi = Math.Abs(beta - alpha) % 360;       // This is either the distance or 360 - distance
            double distance = phi > 180 ? 360 - phi : phi;
            return distance;
        }

        /// <summary>
        /// Use for SUPER accurate color distance checks. Very slow, but also very accurate.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static long GetAverageColorDistance(this Color target, List<Tuple<Color, int>> src)
        {
            long r = 0;
            long t = 0;

            foreach(var c in src)
            {
                int dist = target.GetColorDistance(c.Item1);
                r += dist * c.Item2;
                t += c.Item2;
            }

            r /= t;
            return (int)r;
        }

        /// <summary>
        /// Use for SUPER accurate color distance checks. Very slow, but also very accurate.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static long GetAverageColorDistance(this Color target, Bitmap src)
        {
            long r = 0;
            long total = src.Width * src.Height;

            src.ToViewStreamParallel(null, (int x, int y, Color c) =>
            {
                int dist = target.GetColorDistance(c);
                Interlocked.Add(ref r, dist);
            });

            r /= total;
            return (int)r;
        }

        /// <summary>
        /// Custom color matching algorithm
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int GetColorDistance(this Color c, Color toMatch)
        {
            int dR = (c.R - toMatch.R);
            int dG = (c.G - toMatch.G);
            int dB = (c.B - toMatch.B);
            int dHue = (int)GetDegreeDistance(c.GetHue(), toMatch.GetHue());
            //float dSat = (c.GetSaturation() - toMatch.GetSaturation()) * 100;
            //float dBri = (c.GetBrightness() - toMatch.GetBrightness()) * 100;

            int diff = (
                (dR * dR)
                + (dG * dG)
                + (dB * dB)
                + (int)(Math.Sqrt(dHue * dHue * dHue))
                //+ (int) (dBri * dBri)
                //+ (dSat * dSat)
                );

            return diff;
        }

        /// <summary>
        /// Does not normalize alpha channels
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Color Normalize(this Color c, int? fragmentSize = null)
        {
            int F = fragmentSize ?? Options.Get.PreRender_ColorCacheFragmentSize;
            if (F < 2)
            {
                return c;
            }

            int R = (int)Math.Min(255, Math.Round(Convert.ToDecimal(c.R) / F, 0) * F);
            int G = (int)Math.Min(255, Math.Round(Convert.ToDecimal(c.G) / F, 0) * F);
            int B = (int)Math.Min(255, Math.Round(Convert.ToDecimal(c.B) / F, 0) * F);

            return Color.FromArgb(c.A, R, G, B);
        }

        public static IEnumerable<Color> OrderByColor(this IEnumerable<Color> source)
        {
            return source.OrderByColor(c => c);
        }

        public static IEnumerable<TSource> OrderByColor<TSource>(this IEnumerable<TSource> source, Func<TSource, Color> colorSelector)
        {
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
