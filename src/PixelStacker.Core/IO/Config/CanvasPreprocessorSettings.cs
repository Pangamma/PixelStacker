namespace PixelStacker.Core.IO.Config
{
    public class CanvasPreprocessorSettings
    {
        public int? MaxHeight { get; set; }
        public int? MaxWidth { get; set; }

        public QuantizerSettings QuantizerSettings { get; set; }

        /// <summary>
        /// R/5, G/5, B/5
        /// The value all RGB values should be divided by to achieve awesome truncating.
        /// Basically, 251 would become 250. This is used by the Color cache size
        /// settings dropdown in the pre-render/quantizer options menu.
        /// Valid values: 1, 5, 15, 17, 51
        /// </summary>
        public int RgbBucketSize { get; set; } = 1;
    }
}
