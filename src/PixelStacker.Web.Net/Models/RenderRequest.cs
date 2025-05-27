using Microsoft.AspNetCore.Http;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Web.Net.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PixelStacker.Web.Net.Models
{
    public class UrlRenderRequest : BaseRenderRequest
    {
        [Required]
        public string Url { get; set; }

        public string GetCacheKey()
        {
            List<string> keys = new List<string>();
            int bitFlags = 0;
            bitFlags |= this.EnableShadows ? 1 : 0;
            bitFlags |= this.EnableDithering ? 2 : 0;
            bitFlags |= this.IsMultiLayer ? 4 : 0;
            bitFlags |= this.IsSideView ? 8 : 0;
            keys.Add(bitFlags.ToString());

            keys.Add($"{(this.MaxWidth ?? 0)}x{this.MaxHeight}");

            keys.Add(this.Format.ToString());
            keys.Add((this.QuantizedColorCount ?? 0).ToString());
            keys.Add(this.RgbBucketSize.ToString());
            keys.Add(this.Url);
            return String.Join("|", keys);
        }
    }

    public class FileRenderRequest : BaseRenderRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }

    public class BaseRenderRequest
    {
        [AcceptableStringValues("Average Color KdTree", "HSL Unique Color KdTree", "Unique Color KdTree", "Srgb KdTree")]
        public string ColorMapperAlgorithm { get; set; } = null;
        public ExportFormat Format { get; set; } = ExportFormat.Jpeg;

        public bool IsSideView { get; set; } = false;

        public bool IsMultiLayer { get; set; } = true;

        public bool EnableShadows { get; set; } = false;

        [Range(4, 4000)]
        public int? MaxHeight { get; set; } = 200;

        [Range(4, 4000)]
        public int? MaxWidth { get; set; } = 200;


        [AcceptableIntValues(1, 5, 15, 17, 51)]
        public int RgbBucketSize { get; set; } = 1;

        [Range(2, 256)]
        public int? QuantizedColorCount { get; set; } = null;
        public bool EnableDithering { get; set; } = false;
    }
}
