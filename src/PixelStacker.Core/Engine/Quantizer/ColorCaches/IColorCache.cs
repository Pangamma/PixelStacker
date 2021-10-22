using System;
using System.Collections.Generic;
using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Engine.Quantizer.ColorCaches
{
    public interface IColorCache
    {
        /// <summary>
        /// Prepares color cache for next use.
        /// </summary>
        void Prepare();

        /// <summary>
        /// Called when a palette is about to be cached, or precached.
        /// </summary>
        /// <param name="palette">The palette.</param>
        void CachePalette(IList<PxColor> palette);

        /// <summary>
        /// Called when palette index is about to be retrieve for a given color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="paletteIndex">Index of the palette.</param>
        void GetColorPaletteIndex(PxColor color, out int paletteIndex);
    }
}
