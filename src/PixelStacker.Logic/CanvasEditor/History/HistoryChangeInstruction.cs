using PixelStacker.Logic.Model;
using System.Collections.Generic;

namespace PixelStacker.Logic.CanvasEditor.History
{
    public class HistoryChangeInstruction
    {
        public List<PxPoint> ChangedPixels { get; set; } = new List<PxPoint>();
        public int PaletteIDBefore { get; set; }
        public int PaletteIDAfter { get; set; }

        public RenderRecord ToRenderRecord()
        {
            return new RenderRecord()
            {
                PaletteID = PaletteIDAfter,
                ChangedPixels = ChangedPixels
            };
        }
    }
}
