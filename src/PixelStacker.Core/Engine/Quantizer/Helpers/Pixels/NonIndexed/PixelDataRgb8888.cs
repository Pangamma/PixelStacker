using System;
using PixelStacker.Core.Model.Drawing;
using System.Runtime.InteropServices;

namespace PixelStacker.Core.Engine.Quantizer.Helpers.Pixels.NonIndexed
{
    /// <summary>
    /// Name |          Blue         |        Green          |           Red         |         Unused        |
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|
    /// Byte |00000000000000000000000|11111111111111111111111|22222222222222222222222|33333333333333333333333|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct PixelDataRgb8888 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)] private readonly byte blue;    // 00 - 07
        [FieldOffset(1)] private readonly byte green;   // 08 - 15
        [FieldOffset(2)] private readonly byte red;     // 16 - 23
        [FieldOffset(3)] private readonly byte unused;  // 24 - 31

        // raw high-level values
        [FieldOffset(0)] private int raw;             // 00 - 23

        // processed component values
        public int Alpha { get { return 0xFF; } }
        public int Red { get { return red; } }
        public int Green { get { return green; } }
        public int Blue { get { return blue; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public int Argb
        {
            get { return Pixel.AlphaMask | raw; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public PxColor GetColor()
        {
            return PxColor.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(PxColor color)
        {
            raw = color.ToArgb() & Pixel.RedGreenBlueMask;
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public ulong Value
        {
            get { return (uint)raw; }
            set { raw = (int)(value & 0xFFFFFF); }
        }
    }
}
