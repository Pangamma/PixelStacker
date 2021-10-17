using PixelStacker.Logic.Utilities;
using System;
using System.Linq;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        private MaterialSelectWindow MaterialOptions { get; set; } = null;

        private void selectMaterialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MaterialOptions == null)
            {
                this.MaterialOptions = new MaterialSelectWindow(this.Options);

                this.MaterialOptions.OnColorPaletteRecompileRequested = (token) => {
                    ProgressX.Report(40, Resources.Text.Progress_CompilingColorMap);
                    var opts = this.Options;

                    var combos = this.Palette.ToCombinationList()
                    .Where(mc => opts.IsMultiLayerRequired ? mc.IsMultiLayer : true)
                    .Where(mc => mc.Bottom.IsEnabledF(opts) && mc.Top.IsEnabledF(opts))
                    .Where(mc => opts.IsMultiLayer ? true : !mc.IsMultiLayer)
                    .ToList();

                    this.ColorMapper.SetSeedData(combos, this.Palette, this.Options.Preprocessor.IsSideView);
                    ProgressX.Report(100, Resources.Text.Progress_CompiledColorMap);
                };
            }

            this.MaterialOptions.ShowDialog(this);
        }
    }
}
