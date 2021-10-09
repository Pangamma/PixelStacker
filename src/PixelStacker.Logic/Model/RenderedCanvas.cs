using Newtonsoft.Json;
using PixelStacker.IO.JsonConverters;
using System.Drawing;

namespace PixelStacker.Logic.Model
{
    public class RenderedCanvas
    {
        /// <summary>
        /// True if the user has made any manual edits to the canvas.
        /// </summary>
        public bool IsCustomized { get; set; } = false;
        public Point WorldEditOrigin { get; set; } = new Point(0, 0);

        public int Height => CanvasData.Height;
        public int Width => CanvasData.Width;

        [JsonIgnore]
        [JsonConverter(typeof(BitmapJsonTypeConverter))]
        public Bitmap PreprocessedImage { get; set; }

        [JsonIgnore]
        public MaterialPalette MaterialPalette { get; set; }

        [JsonIgnore]
        public CanvasData CanvasData { get; set; }
    }
}
