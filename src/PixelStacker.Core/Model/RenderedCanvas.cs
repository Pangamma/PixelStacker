using Newtonsoft.Json;
using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Model
{
    public class RenderedCanvas
    {
        /// <summary>
        /// True if the user has made any manual edits to the canvas.
        /// </summary>
        public bool IsCustomized { get; set; } = false;
        public PxPoint WorldEditOrigin { get; set; } = new PxPoint(0, 0);

        public int Height => CanvasData.Height;
        public int Width => CanvasData.Width;

        [JsonIgnore]
        public PxBitmap PreprocessedImage { get; set; }

        [JsonIgnore]
        public MaterialPalette MaterialPalette { get; set; }

        [JsonIgnore]
        public CanvasData CanvasData { get; set; }
    }
}
