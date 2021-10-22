using System.Drawing;
using System.Runtime.InteropServices;

namespace PixelStacker.Logic.Engine.Quantizer.Helpers.Pixels.NonIndexed
{
    /// <summary>
    /// Name |          Blue         |        Green          |           Red         | 
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23|
    /// Byte |00000000000000000000000|11111111111111111111111|22222222222222222222222|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public struct PixelDataRgb888 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)] private byte blue;    // 00 - 07
        [FieldOffset(1)] private byte green;   // 08 - 15
        [FieldOffset(2)] private byte red;     // 16 - 23

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
            get { return Pixel.AlphaMask | Red << Pixel.RedShift | Green << Pixel.GreenShift | Blue; }
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
            red = color.R;
            green = color.G;
            blue = color.B;
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public ulong Value
        {
            get { return (uint)Argb; }
            set
            {
                red = (byte)(value >> Pixel.RedShift & 0xFF);
                green = (byte)(value >> Pixel.GreenShift & 0xFF);
                blue = (byte)(value >> Pixel.BlueShift & 0xFF);
            }
        }
    }
}
