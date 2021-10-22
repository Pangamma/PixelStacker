using System;
using System.Runtime.InteropServices;

namespace PixelStacker.Core.Engine.Quantizer.Helpers.Pixels.Indexed
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct PixelData4Indexed : IIndexedPixel
    {
        // raw component values
        private byte index;

        // get - index method
        public byte GetIndex(int offset)
        {
            return (byte)GetBitRange(8 - offset - 4, 7 - offset);
        }

        // set - index method
        public void SetIndex(int offset, byte value)
        {
            SetBitRange(8 - offset - 4, 7 - offset, value);
        }

        private int GetBitRange(int startOffset, int endOffset)
        {
            int result = 0;
            byte bitIndex = 0;

            for (int offset = startOffset; offset <= endOffset; offset++)
            {
                int bitValue = 1 << bitIndex;
                result += GetBit(offset) ? bitValue : 0;
                bitIndex++;
            }

            return result;
        }

        private bool GetBit(int offset)
        {
            return (index & 1 << offset) != 0;
        }

        private void SetBitRange(int startOffset, int endOffset, int value)
        {
            byte bitIndex = 0;

            for (int offset = startOffset; offset <= endOffset; offset++)
            {
                int bitValue = 1 << bitIndex;
                SetBit(offset, (value & bitValue) != 0);
                bitIndex++;
            }
        }

        private void SetBit(int offset, bool value)
        {
            if (value)
            {
                index |= (byte)(1 << offset);
            }
            else
            {
                index &= (byte)~(1 << offset);
            }
        }
    }
}
