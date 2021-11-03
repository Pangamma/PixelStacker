using Newtonsoft.Json;
using PixelStacker.Logic.IO.JsonConverters;
using SkiaSharp;

namespace PixelStacker.Logic.Model
{
    public class RenderedCanvas
    {
        public bool IsSideView { get; set; }
        /// <summary>
        /// True if the user has made any manual edits to the canvas.
        /// </summary>
        public bool IsCustomized { get; set; } = false;
        public SKPoint WorldEditOrigin { get; set; } = new SKPoint(0, 0);

        public int Height => CanvasData.Height;
        public int Width => CanvasData.Width;

        [JsonIgnore]
        public SKBitmap PreprocessedImage { get; set; }

        //[JsonIgnore]
        //[JsonConverter(typeof(BitmapJsonTypeConverter))]
        //public Bitmap OriginalImage { get; set; }

        [JsonIgnore]
        public MaterialPalette MaterialPalette { get; set; }

        [JsonIgnore]
        public CanvasData CanvasData { get; set; }
    }
}
