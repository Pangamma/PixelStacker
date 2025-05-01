using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.IO.Formatters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.API
{
    public class RenderRequest
    {
        /// <summary>
        /// This is optional.
        /// </summary>
        public IColorMapper CustomColorMapper { get; set; } = null;

        public ExportFormat Format { get; set; } = ExportFormat.Jpeg;

        public bool IsSideView { get; set; } = false;

        public bool IsMultiLayer { get; set; } = true;

        public bool EnableShadows { get; set; } = false;

        [Range(4, 20000)]
        public int? MaxHeight { get; set; } = 200;

        [Range(4, 20000)]
        public int? MaxWidth { get; set; } = 200;

        private int _rgbBucketSize = 1;
        private static readonly int[] ACCEPTABLE_RGB_BUCKET_SIZES = [1, 5, 15, 17, 51];
        public int RgbBucketSize
        {
            get => _rgbBucketSize;
            set
            {
                if (!ACCEPTABLE_RGB_BUCKET_SIZES.Contains(value))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(RgbBucketSize),
                        $"Expected value to be one of these values: [{string.Join(", ", ACCEPTABLE_RGB_BUCKET_SIZES)}]. Received ${value}."
                    );
                }
                _rgbBucketSize = value;
            }
        }

        [Range(2, 256)]
        public int? QuantizedColorCount { get; set; } = null;
        public bool EnableDithering { get; set; } = false;

        public static int[] ACCEPTABLE_RGB_BUCKET_SIZES1 => ACCEPTABLE_RGB_BUCKET_SIZES;
    }
}
