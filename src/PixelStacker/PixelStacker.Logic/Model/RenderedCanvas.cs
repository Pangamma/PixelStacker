using Newtonsoft.Json;
using PixelStacker.IO.JsonConverters;
using System.Drawing;

namespace PixelStacker.Logic.Model
{
    public class RenderedCanvas
    {
        public Point WorldEditOrigin { get; set; } = new Point(0, 0);

        [JsonConverter(typeof(BitmapJsonTypeConverter))]
        public Bitmap OriginalImage { get; set; }

        public MaterialPalette MaterialPalette { get; set; }
        public CanvasData CanvasData { get; set; }
    }
}
