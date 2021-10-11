using PixelStacker.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        private MaterialSelectWindow MaterialOptions { get; set; } = null;

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MaterialOptions == null)
            {
                this.MaterialOptions = new MaterialSelectWindow();

                this.MaterialOptions.OnColorPaletteRecompileRequested = (token) => {
                    this.ColorMapper.SetSeedData(null, this.Palette, this.Options.Preprocessor.IsSideView);
                };
            }

            this.MaterialOptions.ShowDialog(this);
        }
    }
}
