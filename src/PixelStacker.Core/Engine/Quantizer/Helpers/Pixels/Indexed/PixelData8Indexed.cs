using System;
using System.Runtime.InteropServices;

namespace PixelStacker.Core.Engine.Quantizer.Helpers.Pixels.Indexed
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct PixelData8Indexed : IIndexedPixel
    {
        // raw component values
        private byte index;

        // index methods 
        public byte GetIndex(int offset) { return index; }
        public void SetIndex(int offset, byte value) { index = value; }
    }
}
