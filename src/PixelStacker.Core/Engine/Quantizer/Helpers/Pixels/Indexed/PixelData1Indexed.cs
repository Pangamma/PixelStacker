using System;
using System.Runtime.InteropServices;

namespace PixelStacker.Core.Engine.Quantizer.Helpers.Pixels.Indexed
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct PixelData1Indexed : IIndexedPixel
    {
        // raw component values
        private byte index;

        // get - index method
        public byte GetIndex(int offset)
        {
            return (index & 1 << 7 - offset) != 0 ? Pixel.One : Pixel.Zero;
        }

        // set - index method
        public void SetIndex(int offset, byte value)
        {
            value = value == 0 ? Pixel.One : Pixel.Zero;

            if (value == 0)
            {
                index |= (byte)(1 << 7 - offset);
            }
            else
            {
                index &= (byte)~(1 << 7 - offset);
            }
        }
    }
}
