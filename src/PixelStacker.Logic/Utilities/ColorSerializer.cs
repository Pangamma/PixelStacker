using SkiaSharp;

namespace PixelStacker.Logic.Utilities
{
    /// <summary>
    /// ONLY EVER USE WHEN GOING TO AND FROM CANVAS VALUES
    /// </summary>
    public static class ColorSerializer
    {
        public static uint ToPaletteID(byte red, byte green, byte blue)
        {
            byte alpha = 0;
            uint rt = (unchecked((uint)(red << ARGBRedShift |
                         green << ARGBGreenShift |
                         blue << ARGBBlueShift |
                         alpha << ARGBAlphaShift))) & 0xffffffff;
            return rt;
        }

        public static uint ToPaletteID(SKColor c)
        {
            byte alpha = 0; //  c.Alpha;
            byte red = c.Red;
            byte green = c.Green;
            byte blue = c.Blue;
            
            uint rt = (unchecked((uint)(red << ARGBRedShift |
                         green << ARGBGreenShift |
                         blue << ARGBBlueShift |
                         alpha << ARGBAlphaShift))) & 0xffffffff;
            return rt;
        }

        /**
         * Shift count and bit mask for A, R, G, B components in ARGB mode!
         */
        private const int ARGBAlphaShift = 24;
        private const int ARGBRedShift = 16;
        private const int ARGBGreenShift = 8;
        private const int ARGBBlueShift = 0;
        public static SKColor FromPaletteID(uint Value)
        {
            byte r = (byte)((Value >> ARGBRedShift) & 0xFF);
            byte g = (byte)((Value >> ARGBGreenShift) & 0xFF);
            byte b = (byte)((Value >> ARGBBlueShift) & 0xFF);
            //byte a = (byte)((Value >> ARGBAlphaShift) & 0xFF);

            SKColor c = new SKColor(r, g, b, 255);
            return c;
        }
    }
}
