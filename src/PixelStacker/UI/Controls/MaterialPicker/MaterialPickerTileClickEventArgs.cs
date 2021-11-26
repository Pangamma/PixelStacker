using PixelStacker.UI.Controls.MaterialPicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls.MaterialPicker
{
    public class MaterialPickerTileClickEventArgs : EventArgs
    {
        public MaterialPickerTile Tile { get; set; }
        public string TileID { get; set; }
        public MouseButtons MouseButton { get; set; }

        public MaterialPickerTileClickEventArgs() { }
        public MaterialPickerTileClickEventArgs(MaterialPickerTile tile, string tileID, MouseButtons mouseButton)
        {
            this.Tile = tile;
            this.TileID = tileID;
            this.MouseButton = mouseButton;
        }
    }
}
