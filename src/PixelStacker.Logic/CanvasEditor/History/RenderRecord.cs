using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.CanvasEditor.History
{
    public class RenderRecord
    {
        public List<PxPoint> ChangedPixels { get; set; } = new List<PxPoint>();
        public int PaletteID { get; set; } = 0;
    }
}
