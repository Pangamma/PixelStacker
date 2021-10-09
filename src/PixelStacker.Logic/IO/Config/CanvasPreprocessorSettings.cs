namespace PixelStacker.IO.Config
{
    public class CanvasPreprocessorSettings
    {
        public int? MaxHeight { get; set; }
        public int? MaxWidth { get; set; }
        public bool IsSideView { get; set; } = false;
        public QuantizerSettings QuantizerSettings { get; set; }

        /// <summary>
        /// Valid values: 1, 5, 15, 17, 51
        /// </summary>
        public int RgbBucketSize { get; set; } = 1;
    }
}
