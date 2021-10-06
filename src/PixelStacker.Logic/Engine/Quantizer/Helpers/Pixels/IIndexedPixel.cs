using System;

namespace SimplePaletteQuantizer.Helpers.Pixels
{
    public interface IIndexedPixel
    {
        // index methods
        byte GetIndex(int offset);
        void SetIndex(int offset, byte value);
    }
}
