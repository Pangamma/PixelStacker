namespace PixelStacker.Logic.Engine.Quantizer.Helpers.Pixels
{
    public interface IIndexedPixel
    {
        // index methods
        byte GetIndex(int offset);
        void SetIndex(int offset, byte value);
    }
}
