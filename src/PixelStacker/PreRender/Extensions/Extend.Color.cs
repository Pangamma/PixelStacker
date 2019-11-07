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
    }
}
