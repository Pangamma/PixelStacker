using PixelStacker.Logic.Model;

namespace PixelStacker.Logic.IO.Config
{
    public class ToolSettings
    {
        public int BrushWidth { get; set; } = 1;
        public MaterialCombination PrimaryColor { get; set; } = MaterialPalette.Air;

        /// <summary>
        /// NULL = both layers are edited
        /// 0 = only bottom layer is editedd
        /// 1 = only top layer is edited
        /// </summary>
        public ZLayer ZLayerFilter { get; set; } = ZLayer.Both;
    }
}