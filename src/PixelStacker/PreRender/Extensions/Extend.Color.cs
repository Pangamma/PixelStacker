using PixelStacker.Logic;
using SimplePaletteQuantizer.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.PreRender.Extensions
{
    public static class ExtendColor
    {
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

            float diff = (
                (dR * dR)
                + (dG * dG)
                + (dB * dB)
                + (float) Math.Pow(dHue, 1.5)
                + ((c.GetSaturation() - c.GetSaturation()) * 100).Pow2()
                );

            return diff;
            //double diffd = 1000 *
            //   (
            //   //Math.Pow(Math.Abs(c.R - toMatch.R), Constants.rgbPower)
            //   //+ Math.Pow(Math.Abs(c.G - toMatch.G), Constants.rgbPower)
            //   //+ Math.Pow(Math.Abs(c.B - toMatch.B), Constants.rgbPower)
            //   //+ Math.Pow(GetDegreeDistance(c.GetHue(), toMatch.GetHue()) / 2, Constants.huePower)
            //   //+ Math.Pow(Math.Abs(c.GetSaturation() - toMatch.GetSaturation()), Constants.satPower)
        }
    }
}
