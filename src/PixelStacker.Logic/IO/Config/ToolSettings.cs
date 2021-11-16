using PixelStacker.Logic.Model;

namespace PixelStacker.Logic.IO.Config
{
    public class ToolSettings
    {
        public int BrushWidth { get; set; } = 1;
        public MaterialCombination PrimaryColor { get; set; } = new MaterialCombination("AIR");
    }
}