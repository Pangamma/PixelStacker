using PixelStacker.Extensions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PixelStacker.Logic.Extensions
{
    // Sat: 0...1
    // Hue: 0...360
    // Brightness: 0...1
    public static class ExtendColor
    {
        public static SKColor FromSRGB(this SKColor c)
        {
            //Convert color from 0..255 to 0..1
            Single r = c.Red / 255f;
            Single g = c.Green / 255f;
            Single b = c.Blue / 255f;

            //Inverse Red, Green, and Blue
            if (r > 0.04045) r = (float)Math.Pow((r + 0.055) / 1.055, 2.4); else r = (float)(r / 12.92);
            if (g > 0.04045) g = (float)Math.Pow((g + 0.055) / 1.055, 2.4); else g = (float)(g / 12.92);
            if (b > 0.04045) b = (float)Math.Pow((b + 0.055) / 1.055, 2.4); else b = (float)(b / 12.92);

            //return new color. Convert 0..1 back into 0..255
            SKColor result = new SKColor((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), 255);

            return result;
        }

        public static SKColor ToSRGB(this SKColor c)
        {
            //Convert color from 0..255 to 0..1
            Single r = c.Red / 255f;
            Single g = c.Green / 255f;
            Single b = c.Blue / 255f;

            //Apply companding to Red, Green, and Blue
            if (r > 0.0031308) r = 1.055f * (float)(Math.Pow(r, 1 / 2.4f) - 0.055f); else r = (float)(r * 12.92f);
            if (g > 0.0031308) g = 1.055f * (float)(Math.Pow(g, 1 / 2.4f) - 0.055f); else g = (float)(g * 12.92f);
            if (b > 0.0031308) b = 1.055f * (float)(Math.Pow(b, 1 / 2.4f) - 0.055f); else b = (float)(b * 12.92f);

            //return new color. Convert 0..1 back into 0..255
            SKColor result = new SKColor((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), c.Alpha);
            return result;
        }

        public static SKColor ToLAB(this SKColor color)
        {
            float[] xyz = new float[3];
            float[] lab = new float[3];
            float[] rgb = new float[] { color.Red, color.Green, color.Blue, color.Alpha };

            rgb[0] = color.Red / 255.0f;
            rgb[1] = color.Green / 255.0f;
            rgb[2] = color.Blue / 255.0f;

            if (rgb[0] > .04045f)
            {
                rgb[0] = (float)Math.Pow((rgb[0] + .055) / 1.055, 2.4);
            }
            else
            {
                rgb[0] = rgb[0] / 12.92f;
            }

            if (rgb[1] > .04045f)
            {
                rgb[1] = (float)Math.Pow((rgb[1] + .055) / 1.055, 2.4);
            }
            else
            {
                rgb[1] = rgb[1] / 12.92f;
            }

            if (rgb[2] > .04045f)
            {
                rgb[2] = (float)Math.Pow((rgb[2] + .055) / 1.055, 2.4);
            }
            else
            {
                rgb[2] = rgb[2] / 12.92f;
            }
            rgb[0] = rgb[0] * 100.0f;
            rgb[1] = rgb[1] * 100.0f;
            rgb[2] = rgb[2] * 100.0f;


            xyz[0] = ((rgb[0] * .412453f) + (rgb[1] * .357580f) + (rgb[2] * .180423f));
            xyz[1] = ((rgb[0] * .212671f) + (rgb[1] * .715160f) + (rgb[2] * .072169f));
            xyz[2] = ((rgb[0] * .019334f) + (rgb[1] * .119193f) + (rgb[2] * .950227f));


            xyz[0] = xyz[0] / 95.047f;
            xyz[1] = xyz[1] / 100.0f;
            xyz[2] = xyz[2] / 108.883f;

            if (xyz[0] > .008856f)
            {
                xyz[0] = (float)Math.Pow(xyz[0], (1.0 / 3.0));
            }
            else
            {
                xyz[0] = (xyz[0] * 7.787f) + (16.0f / 116.0f);
            }

            if (xyz[1] > .008856f)
            {
                xyz[1] = (float)Math.Pow(xyz[1], 1.0 / 3.0);
            }
            else
            {
                xyz[1] = (xyz[1] * 7.787f) + (16.0f / 116.0f);
            }

            if (xyz[2] > .008856f)
            {
                xyz[2] = (float)Math.Pow(xyz[2], 1.0 / 3.0);
            }
            else
            {
                xyz[2] = (xyz[2] * 7.787f) + (16.0f / 116.0f);
            }

            lab[0] = Math.Clamp((116.0f * xyz[1]) - 16.0f, 0, 255);
            lab[1] = Math.Clamp(500.0f * (xyz[0] - xyz[1]), 0, 255);
            lab[2] = Math.Clamp(200.0f * (xyz[1] - xyz[2]), 0, 255);

            return new SKColor((byte)lab[0], (byte)lab[1], (byte)lab[2], color.Alpha);
        }

        /// <summary>
        /// Returns the Hue-Saturation-Lightness (HSL) hue
        /// value, in degrees, for this <see cref='PixelStacker.Core.Model.Drawing.PxColor'/> .  
        /// If R == G == B, the hue is meaningless, and the return value is 0.
        /// </summary>
        public static double GetHue(this SKColor c)
        {
            byte R = c.Red;
            byte G = c.Green;
            byte B = c.Blue; if (R == G && G == B)
                return 0; // 0 makes as good an UNDEFINED value as any

            float r = R / 255.0f;
            float g = G / 255.0f;
            float b = B / 255.0f;

            float max, min;
            float delta;
            float hue = 0.0f;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            delta = max - min;

            if (r == max)
            {
                hue = (g - b) / delta;
            }
            else if (g == max)
            {
                hue = 2 + (b - r) / delta;
            }
            else if (b == max)
            {
                hue = 4 + (r - g) / delta;
            }
            hue *= 60;

            if (hue < 0.0f)
            {
                hue += 360.0f;
            }
            return hue;

        }

        /// <summary>
        /// Value = (v + 200f) / 3f, Saturation = 3.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static SKColor ToGhostColor(this SKColor c)
        {
            c.ToHsv(out float h, out float s, out float v);
            s = 3f;
            v = (v + 200f) / 3f;
            var nextColor = SKColor.FromHsv(h, s, v, c.Alpha);
            return nextColor;
        }

        /// <summary>
        ///   The Hue-Saturation-Lightness (HSL) saturation for this
        ///    <see cref='PixelStacker.Core.Model.Drawing.PxColor'/>
        /// </summary>
        public static float GetSaturation(this SKColor c)
        {
            byte R = c.Red;
            byte G = c.Green;
            byte B = c.Blue;
            float r = R / 255.0f;
            float g = G / 255.0f;
            float b = B / 255.0f;

            float max, min;
            float l, s = 0;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            // if max == min, then there is no color and
            // the saturation is zero.
            //
            if (max != min)
            {
                l = (max + min) / 2;

                if (l <= .5)
                {
                    s = (max - min) / (max + min);
                }
                else
                {
                    s = (max - min) / (2 - max - min);
                }
            }
            return s;
        }


        /// <summary>
        ///       Returns the Hue-Saturation-Lightness (HSL) lightness
        ///       for this <see cref='PixelStacker.Core.Model.Drawing.PxColor'/> .
        /// </summary>
        public static float GetBrightness(this SKColor c)
        {
            byte R = c.Red;
            byte G = c.Green;
            byte B = c.Blue;
            float r = R / 255.0f;
            float g = G / 255.0f;
            float b = B / 255.0f;

            float max, min;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            return (max + min) / 2;
        }

        /// <summary>
        /// Overlay the TOP color ontop of the BOTTOM color
        /// </summary>
        /// <param name="RGBA2_Bottom"></param>
        /// <param name="RGBA1_Top"></param>
        /// <returns></returns>
        public static SKColor OverlayColor(this SKColor RGBA2_Bottom, SKColor RGBA1_Top)
        {
            double alpha = Convert.ToDouble(RGBA1_Top.Alpha) / 255;
            byte R = (byte)(RGBA1_Top.Red * alpha + RGBA2_Bottom.Red * (1.0 - alpha));
            byte G = (byte)(RGBA1_Top.Green * alpha + RGBA2_Bottom.Green * (1.0 - alpha));
            byte B = (byte)(RGBA1_Top.Blue * alpha + RGBA2_Bottom.Blue * (1.0 - alpha));
            return new SKColor(R, G, B, 255);
        }

        public static SKColor AverageColors(this IEnumerable<SKColor> colors)
        {
            long r = 0;
            long g = 0;
            long b = 0;
            long a = 0;
            long total = colors.Count();
            foreach (var c in colors)
            {
                r += c.Red;
                g += c.Green;
                b += c.Blue;
                a += c.Alpha;
            }

            r /= total;
            g /= total;
            b /= total;
            a /= total;

            return new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
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
        public static int GetAverageColorDistance(this SKColor target, List<Tuple<SKColor, int>> src)
        {
            long r = 0;
            long t = 0;

            foreach (var c in src)
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
        public static int GetAverageColorDistance(this SKColor target, List<Tuple<SKColor, int>> src, Func<SKColor, SKColor, int> distFunc)
        {
            long r = 0;
            long t = 0;

            foreach (var c in src)
            {
                int dist = distFunc(target, c.Item1);
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
        public static long GetAverageColorDistance(this SKColor target, SKBitmap src)
        {
            long r = 0;
            long total = src.Width * src.Height;

            src.ToViewStream(null, (x, y, c) =>
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
        public static int GetColorDistance(this SKColor c, SKColor toMatch)
        {
            int dR = c.Red - toMatch.Red;
            int dG = c.Green - toMatch.Green;
            int dB = c.Blue - toMatch.Blue;
            int dHue = (int)GetDegreeDistance(c.GetHue(), toMatch.GetHue());

            int diff =
                dR * dR
                + dG * dG
                + dB * dB
                + (int)Math.Sqrt(dHue * dHue * dHue)
                ;

            return diff;
        }

        /// <summary>
        /// Custom color matching algorithm
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int GetColorDistanceSRGB(this SKColor c, SKColor toMatch)
        {
            int dR = c.Red * c.Red - toMatch.Red * toMatch.Red;
            int dG = c.Green * c.Green - toMatch.Green * toMatch.Green;
            int dB = c.Blue * c.Blue - toMatch.Blue * toMatch.Blue;
            int dHue = (int)GetDegreeDistance(c.GetHue(), toMatch.GetHue());

            int diff =
                dR * dR
                + dG * dG
                + dB * dB
                + (int)Math.Sqrt(dHue * dHue * dHue)
                ;

            return diff;
        }

        public static SKColor Normalize(this SKColor c, int fragmentSize)
            => c.NormalizeActual(fragmentSize);

        /// <summary>
        /// Does not normalize alpha channels
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static SKColor NormalizeActual(this SKColor c, int? fragmentSize = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            int F = fragmentSize ?? 1; // Options.GetInMemoryFallback.Preprocessor.RgbBucketSize;
#pragma warning restore CS0618 // Type or member is obsolete
            if (F < 2)
            {
                return c;
            }

            var R = (byte)Math.Min(255, Math.Round(Convert.ToDecimal(c.Red) / F, 0) * F);
            var G = (byte)Math.Min(255, Math.Round(Convert.ToDecimal(c.Green) / F, 0) * F);
            var B = (byte)Math.Min(255, Math.Round(Convert.ToDecimal(c.Blue) / F, 0) * F);

            return new SKColor(R, G, B, c.Alpha);
        }

        public static IEnumerable<SKColor> OrderByColor(this IEnumerable<SKColor> source)
        {
            return source.OrderByColor(c => c);
        }

        public static IEnumerable<TSource> OrderByColor<TSource>(this IEnumerable<TSource> source, Func<TSource, SKColor> colorSelector)
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
