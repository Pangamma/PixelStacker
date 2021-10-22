using System;
using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Engine.Quantizer.Helpers.Pixels
{
    public interface INonIndexedPixel
    {
        // components
        int Alpha { get; }
        int Red { get; }
        int Green { get; }
        int Blue { get; }

        // higher-level values
        int Argb { get; }
        ulong Value { get; set; }

        // color methods
        PxColor GetColor();
        void SetColor(PxColor color);
    }
}
