using SkiaSharp;
using System;

namespace PixelStacker.Resources
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
        private static SKBitmap GetSpriteSheet(int textureSize)
        {
            string resourceKey = $"sprite_x{textureSize}";
            return Shadows.ResourceManager.GetObject(resourceKey) as SKBitmap;
        }


        const int NUM_SHADE_TILES_X = 8;
        private static SKRect GetSpriteRect(int textureSize, ShadeFrom dir)
        {
            int numDir = (int)dir;
            int xOffset = textureSize * (numDir % NUM_SHADE_TILES_X);
            int yOffset = textureSize * (numDir / NUM_SHADE_TILES_X);

            return new SKRect(left: xOffset, top: yOffset, right: xOffset + textureSize, bottom: yOffset + textureSize);
        }

        private static SKBitmap[] shadowSprites = new SKBitmap[256];
        public static SKBitmap GetSpriteIndividual(int textureSize, ShadeFrom dir)
        {
            int numDir = (int)dir;
            if (shadowSprites[numDir] == null)
            {
                var bmShadeSprites = GetSpriteSheet(textureSize);
                var rectDST = new SKRect(0, 0, textureSize, textureSize);

                for (int i = 0; i < 256; i++)
                {
                    var rectSRC = GetSpriteRect(textureSize, (ShadeFrom)i);
                    var bm = new SKBitmap(textureSize, textureSize);
                    
                    using (var g = new SKCanvas(bm))
                    {
                        g.DrawBitmap(bitmap: bmShadeSprites,
                            source: rectSRC,
                            dest: rectDST);
                    }

                    shadowSprites[i] = bm;
                }
            }

            return shadowSprites[numDir];
        }

        private static SKBitmap Get(int textureSize, ShadeDir direction)
        {
            if (!Enum.IsDefined(typeof(ShadeRez), textureSize / 4))
            {
                throw new ArgumentException($"Invalid texture size. Given: {textureSize}. Expected: 16, 32, 64, 128");
            }

            string resourceKey = $"d{textureSize / 4}_{direction}";

            try
            {
                return SKBitmap.Decode((byte[]) Shadows.ResourceManager.GetObject(resourceKey));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Allow debugging but still log to console
                throw new Exception("FIx it.");
                //return Textures.air;
            }
        }
    }
}
