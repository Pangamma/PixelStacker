using Newtonsoft.Json;
using PixelStacker.Logic.IO.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Model
{
    public class PixelStackerProjectData
    {
        public bool IsSideView { get; set; } = false;
        public int[] WorldEditOrigin { get; set; } = new int[] { 0, 0 };

        public int Height => CanvasData.Height;
        public int Width => CanvasData.Width;

        [JsonIgnore]
        public Bitmap PreprocessedImage { get; set; }

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
            this.WorldEditOrigin = new int[] { canvas.WorldEditOrigin.X, canvas.WorldEditOrigin.Y };
            this.MaterialPalette = canvas.MaterialPalette;
            this.CanvasData = canvas.CanvasData;
        }
    }
}
