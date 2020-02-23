using PixelStacker.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Properties
{
    /* Shadow resolution*/
    public enum ShadeRez
    {
        Depth4 = 4,
        Depth8 = 8,
        Depth16 = 16,
        Depth64 = 64
    }

    /* "ShadowSourceDirection" If block casting shadow is above, use T */
    public enum ShadeDir
    {
        T,
        R,
        B,
        L,
        TR, // for when shadow comes from diagonally top right
        TL, // for when shadow comes from diagonally top left
        BR, // for when shadow comes from diagonally bottom right
        BL, // for when shadow comes from diagonally bottom left
    }

    public static class ShadowHelper
    {
        public static System.Drawing.Bitmap Get(int textureSize, ShadeDir direction)
        {
            if (!Enum.IsDefined(typeof(ShadeRez), textureSize / 4))
            {
                throw new ArgumentException($"Invalid texture size. Given: {textureSize}. Expected: 16, 32, 64, 128");
            }

            string resourceKey = $"d{textureSize / 4}_{direction.ToString()}";

            try
            {
                return Shadows.ResourceManager.GetObject(resourceKey) as System.Drawing.Bitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Allow debugging but still log to console
                return Textures.air;
            }
        }
    }
}
