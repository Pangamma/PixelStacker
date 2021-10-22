using System;
using PixelStacker.Core.Model.Drawing;
using System.Runtime.InteropServices;
using PixelStacker.Core.Engine.Quantizer.Helpers.Pixels;
using PixelStacker.Core.Engine.Quantizer.Helpers;

namespace PixelStacker.Core.Engine.Quantizer.Helpers.Pixels.NonIndexed
{
    /// <summary>
    /// Name |     Blue     |      Green      |     Red      | 
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|
    /// Byte |00000000000000000000000|11111111111111111111111|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct PixelDataRgb565 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)] private byte blue;     // 00 - 04
        [FieldOffset(0)] private ushort green;  // 05 - 10
        [FieldOffset(2)] private byte red;      // 11 - 15

        // raw high-level values
        [FieldOffset(0)] private ushort raw;    // 00 - 15

        // processed component values
        public int Alpha { get { return 0xFF; } }
        public int Red { get { return red >> 3 & 0xF; } }
        public int Green { get { return green >> 5 & 0x1F; } }
        public int Blue { get { return blue & 0xF; } }

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
            red = (byte)(color.R >> 3);
            green = (byte)(color.G >> 2);
            blue = (byte)(color.B >> 3);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public ulong Value
        {
            get { return raw; }
            set { raw = (ushort)(value & 0xFFFF); }
        }
    }
}
