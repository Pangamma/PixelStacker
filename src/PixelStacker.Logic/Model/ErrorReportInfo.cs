using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.IO.Config;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Model
{
    public class ErrorReportInfo
    {
        public Exception Exception { get; set; }
        public string HashCode => $"[{Exception.StackTrace.GetHashCode()}] {Exception.Message}";
        public Options Options { get; set; }
        public RenderedCanvas RenderedCanvas { get; set; }
        public SKBitmap LoadedImage { get; set; }
        public SKBitmap PreprocessedImage { get; set; }
        public IColorMapper ColorMapper { get; set; }
    }
}
