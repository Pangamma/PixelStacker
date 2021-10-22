using System;
using PixelStacker.Core.Engine.Quantizer.Helpers;
using PixelStacker.Core.Engine.Quantizer.PathProviders;
using PixelStacker.Core.Engine.Quantizer.Quantizers;

namespace PixelStacker.Core.Engine.Quantizer.Ditherers
{
    public interface IColorDitherer : IPathProvider
    {
        /// <summary>
        /// Gets a value indicating whether this ditherer uses only actually process pixel.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this ditherer is inplace; otherwise, <c>false</c>.
        /// </value>
        bool IsInplace { get; }

        /// <summary>
        /// Prepares this instance.
        /// </summary>
        void Prepare(IColorQuantizer quantizer, int colorCount, ImageBuffer sourceBuffer, ImageBuffer targetBuffer);

        /// <summary>
        /// Processes the specified buffer.
        /// </summary>
        bool ProcessPixel(Pixel sourcePixel, Pixel targetPixel);

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        void Finish();
    }
}
