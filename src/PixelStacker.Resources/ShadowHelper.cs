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

    /// <summary>
    /// Height of a material. Used when figuring out how to shade things properly.
    /// </summary>
    public enum MaterialHeight
    {
        L0_EMPTY = 0,
        L1_SOLID = 1, // single layer
        L2_MULTI = 2
    }

    public static class ShadowHelper
    {
        private static SKBitmap GetSpriteSheet(int textureSize)
        {
            string resourceKey = $"sprite_x{textureSize}";
            return SKBitmap.Decode((byte[])Shadows.ResourceManager.GetObject(resourceKey))
                .Copy(SKColorType.Rgba8888);
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
        private static object padlock = new object();
        public static SKBitmap GetSpriteIndividual(int textureSize, ShadeFrom dir)
        {
            int numDir = (int)dir;

            // Run a non locking quick check here.
            if (null == shadowSprites[numDir])
            {
                lock (padlock)
                {
                    // Double check once we have the lock. Has it been set yet?
                    if (null == shadowSprites[numDir])
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
                }
            }

            return shadowSprites[numDir];
        }
    }
}
