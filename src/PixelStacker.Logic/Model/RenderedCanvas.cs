using Newtonsoft.Json;
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
        public PxPoint WorldEditOrigin { get; set; }

        public int Height => CanvasData.Height;
        public int Width => CanvasData.Width;

        [JsonIgnore]
        public SKBitmap PreprocessedImage { get; set; }

        [JsonIgnore]
        public MaterialPalette MaterialPalette { get; set; }

        [JsonIgnore]
        public CanvasData CanvasData { get; set; }

        public bool IsInRange(int x, int y) => CanvasData?.IsInRange(x, y) ?? false;
    }
}
