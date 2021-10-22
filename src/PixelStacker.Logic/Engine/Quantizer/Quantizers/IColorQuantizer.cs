using System.Collections.Generic;
using System.Drawing;
using PixelStacker.Logic.Engine.Quantizer.Helpers;
using PixelStacker.Logic.Engine.Quantizer.PathProviders;

namespace PixelStacker.Logic.Engine.Quantizer.Quantizers
{
    /// <summary>
    /// This interface provides a color quantization capabilities.
    /// </summary>
    public interface IColorQuantizer : IPathProvider
    {
        /// <summary>
        /// The label that can be used in the UI. Will NOT be localized ever.
        /// Can be used as an ID field.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets a value indicating whether to allow parallel processing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to allow parallel processing; otherwise, <c>false</c>.
        /// </value>
        bool AllowParallel { get; }

        /// <summary>
        /// Prepares the quantizer for image processing.
        /// </summary>
        void Prepare(ImageBuffer image);

        /// <summary>
        /// Adds the color to quantizer.
        /// </summary>
        void AddColor(Color color, int x, int y);

        /// <summary>
        /// Gets the palette with specified count of the colors.
        /// </summary>
        List<Color> GetPalette(int colorCount);

        /// <summary>
        /// Gets the index of the palette for specific color.
        /// </summary>
        int GetPaletteIndex(Color color, int x, int y);

        /// <summary>
        /// Gets the color count.
        /// </summary>
        int GetColorCount();

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Finish();
    }
}