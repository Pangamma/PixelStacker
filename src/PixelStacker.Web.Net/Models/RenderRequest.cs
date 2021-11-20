using Microsoft.AspNetCore.Http;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Web.Net.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.Models
{
    public class UrlRenderRequest : BaseRenderRequest
    {
        [Required]
        public string Url { get; set; }

        public string GetCacheKey()
        {
            List<string> keys = new List<string>();
            keys.Add((this.MaxWidth ?? 0).ToString());
            keys.Add((this.MaxHeight ?? 0).ToString());
            keys.Add(this.EnableDithering ? "1" : "0");
            keys.Add(this.Format.ToString());
            keys.Add(this.IsMultiLayer ? "1" : "0");
            keys.Add(this.IsSideView ? "1" : "0");
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
        public ExportFormat Format { get; set; } = ExportFormat.Jpeg;

        public bool IsSideView { get; set; } = false;
        public bool IsMultiLayer { get; set; } = true;

        public int? MaxHeight { get; set; }
        public int? MaxWidth { get; set; }


        [AcceptableIntValues(1, 5, 15, 17, 51)]
        public int RgbBucketSize { get; set; } = 1;

        [Range(2, 256)]
        public int? QuantizedColorCount { get; set; } = null;
        public bool EnableDithering { get; set; } = false;
    }
}
