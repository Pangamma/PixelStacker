using Newtonsoft.Json;
using PixelStacker.IO.Formatters;
using System.Drawing;

namespace PixelStacker.Logic.Model
{
    public class RenderedCanvas
    {
        public Point WorldEditOrigin { get; set; } = new Point(0, 0);
        public int Width { get; set; }
        public int Height { get; set; }

        [JsonConverter(typeof(BitmapJsonTypeConverter))]
        public Bitmap OriginalImage { get; set; }

        public MaterialPalette MaterialPalette { get; set; }
        public object CanvasData { get; set; }
    }
}
