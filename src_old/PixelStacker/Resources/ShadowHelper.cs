using PixelStacker.Resources;
using System;
using System.Drawing;

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

    /// <summary>
    /// Where current block is in center, and adjacent blocks make up the enum value
    /// Given:
    /// X X X
    /// O c X
    /// O X X
    /// 
    /// You would have BL | L
    /// </summary>
    [Flags]
    public enum ShadeFrom
    {
        EMPTY = 0x000,     // 0000 0000
        TL = 0x001,     // 0000 0001
        T = 0x002,     // 0000 0010
        TR = 0x004,     // 0000 0100

        L = 0x008,     // 0000 1000
        R = 0x010,     // 0001 0000

        BL = 0x020,     // 0010 0000
        B = 0x040,     // 0100 0000
        BR = 0x080,     // 1000 0000
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
        public static System.Drawing.Bitmap GetSpriteSheet(int textureSize)
        {
            string resourceKey = $"sprite_x{textureSize}";
            return Shadows.ResourceManager.GetObject(resourceKey) as System.Drawing.Bitmap;
        }


        const int NUM_SHADE_TILES_X = 8;
        public static Rectangle GetSpriteRect(int textureSize, ShadeFrom dir)
        {
            int numDir = (int)dir;
            int xOffset = textureSize * (numDir % NUM_SHADE_TILES_X);
            int yOffset = textureSize * (numDir / NUM_SHADE_TILES_X);

            return new Rectangle(x: xOffset, y: yOffset, width: textureSize, height: textureSize);
        }

        private static Bitmap[] shadowSprites = new Bitmap[256];
        public static Bitmap GetSpriteIndividual(int textureSize, ShadeFrom dir)
        {
            int numDir = (int)dir;
            if (shadowSprites[numDir] == null)
            {
                var bmShadeSprites = ShadowHelper.GetSpriteSheet(textureSize);
                var rectDST = new Rectangle(0, 0, textureSize, textureSize);

                for (int i = 0; i < 256; i++)
                {
                    var rectSRC = GetSpriteRect(textureSize, (ShadeFrom)i);
                    var bm = new Bitmap(textureSize, textureSize);
                    using (Graphics g = Graphics.FromImage(bm))
                    {
                        g.DrawImage(image: bmShadeSprites,
                            srcRect: rectSRC,
                            destRect: rectDST,
                            srcUnit: GraphicsUnit.Pixel);
                    }

                    shadowSprites[i] = bm;
                }
            }

            return shadowSprites[numDir];
        }

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
