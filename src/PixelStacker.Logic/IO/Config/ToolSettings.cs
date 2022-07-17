using PixelStacker.Logic.Model;
using System;

namespace PixelStacker.Logic.IO.Config
{
    public class ToolSettings
    {
        public int BrushWidth { get; set; } = 5;
#if !DEBUG
        public MaterialCombination PrimaryColor { get; set; } = MaterialPalette.Air;
#else
        private MaterialCombination _mat = MaterialPalette.Air;
        public MaterialCombination PrimaryColor
        {
            get => _mat;
            set
            {
                if (value == null) throw new ArgumentNullException("FAIL");
                _mat = value;
            }
        }
#endif


        /// <summary>
        /// NULL = both layers are edited
        /// 0 = only bottom layer is editedd
        /// 1 = only top layer is edited
        /// </summary>
        public ZLayer ZLayerFilter { get; set; } = ZLayer.Both;
    }
}