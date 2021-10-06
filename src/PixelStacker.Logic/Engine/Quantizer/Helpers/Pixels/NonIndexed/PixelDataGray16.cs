using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace SimplePaletteQuantizer.Helpers.Pixels.NonIndexed
{
    /// <summary>
    /// Name |                  Grayscale                    |
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15| 
    /// Byte |00000000000000000000000|11111111111111111111111|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct PixelDataGray16 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)] private ushort gray;   // 00 - 15

        // processed raw values
        public int Gray { get { return (0xFF >> 8) & 0xF; } }
        public int Alpha { get { return 0xFF; } }
        public int Red { get { return Gray; } }
        public int Green { get { return Gray; } }
        public int Blue { get { return Gray; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public int Argb
        {
            get { return (Pixel.AlphaMask) | Red << Pixel.RedShift | Green << Pixel.GreenShift | Blue; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            int argb = color.ToArgb() & Pixel.RedGreenBlueMask;
            gray = (byte) (argb >> Pixel.RedShift);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public ulong Value
        {
            get { return gray; }
            set { gray = (ushort) (value & 0xFFFF); }
        }
    }
}