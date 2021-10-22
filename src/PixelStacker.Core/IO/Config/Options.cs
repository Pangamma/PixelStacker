using Newtonsoft.Json;

namespace PixelStacker.Core.IO.Config
{
    public class Options
    {

        [JsonIgnore]
        public IOptionsProvider StorageProvider { get; set; }
        public Options(IOptionsProvider storageProvider)
        {
            this.StorageProvider = storageProvider;
        }

        public string Locale { get; set; } = "en-us";
        public bool IsAdvancedModeEnabled { get; set; } = false;

        public MaterialSelectSettings Materials { get; set; } = new MaterialSelectSettings();

        public CanvasPreprocessorSettings Preprocessor { get; set; } = new CanvasPreprocessorSettings()
        {
            MaxHeight = null,
            MaxWidth = null,
            RgbBucketSize = 1,
            QuantizerSettings = new QuantizerSettings()
            {
                IsEnabled = false,
                //Algorithm = QuantizerAlgorithm.HslDistinctSelection,
                ColorCache = "Octree search",
                MaxColorCount = 256,
                DitherAlgorithm = "No dithering",
                MaxParallelProcesses = 1
            }
        };

        private static Options _self;
        [Obsolete(Constants.Obs_Static)]
        public static Options Get
        {
            get
            {
                if (_self == null)
                {
                    _self = new Options(new MemoryOptionsProvider());
                }
                return _self;
            }
        }
    }
}
