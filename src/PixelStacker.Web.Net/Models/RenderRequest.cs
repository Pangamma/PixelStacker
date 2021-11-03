using Microsoft.AspNetCore.Http;
using PixelStacker.Logic.IO.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PixelStacker.Web.Net.Models
{
    public class RenderRequest
    {
        [Required]
        public IFormFile File { get; set; }

        public CanvasPreprocessorSettings PreprocessSettings { get; set; } = new CanvasPreprocessorSettings()
        {
            IsSideView = false,
            MaxHeight = 300,
            MaxWidth = 300,
            RgbBucketSize = 1, 
            QuantizerSettings = new QuantizerSettings()
            {
                IsEnabled = false
            }
        };
    }
}
