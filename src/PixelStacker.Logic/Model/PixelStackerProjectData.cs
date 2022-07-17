using Newtonsoft.Json;
using PixelStacker.Logic.IO.Config;

namespace PixelStacker.Logic.Model
{
    public class PixelStackerProjectData
    {
        public bool IsSideView { get; set; } = false;
        public PxPoint WorldEditOrigin { get; set; }

        public int Height => CanvasData.Height;
        public int Width => CanvasData.Width;

        [JsonIgnore]
        public SkiaSharp.SKBitmap PreprocessedImage { get; set; }

        [JsonIgnore]
        public MaterialPalette MaterialPalette { get; set; }

        [JsonIgnore]
        public CanvasData CanvasData { get; set; }
        
        public PixelStackerProjectData() { }

        public PixelStackerProjectData(RenderedCanvas canvas, Options opts)
        {
            this.CanvasData = canvas.CanvasData;
            this.IsSideView = opts.IsSideView;
            this.PreprocessedImage = canvas.PreprocessedImage;
            this.WorldEditOrigin = canvas.WorldEditOrigin;
            this.MaterialPalette = canvas.MaterialPalette;
            this.CanvasData = canvas.CanvasData;
        }
    }
}
