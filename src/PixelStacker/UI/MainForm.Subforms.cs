using PixelStacker.Logic.Utilities;
using PixelStacker.UI.Forms;
using System;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        private MaterialSelectWindow MaterialOptions { get; set; } = null;
        private ColorReducerForm ColorReducerForm { get; set; } = null;
        public MaterialPickerForm MaterialPickerForm { get; private set; }

        private void gridOptionsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            using var form = new GridSettingsForm();
            form.Options = this.Options;
            this.snapManager.RegisterChild(form);
            form.ShowDialog(this);
        }

        private void selectMaterialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MaterialOptions == null)
            {
                this.MaterialOptions = new MaterialSelectWindow(this.Options);

                this.MaterialOptions.OnColorPaletteRecompileRequested = (token) =>
                {
                    ProgressX.Report(40, Resources.Text.Progress_CompilingColorMap);
                    this.ColorMapper.SetSeedData(this.Palette.ToValidCombinationList(this.Options), this.Palette, this.Options.IsSideView);
                    ProgressX.Report(100, Resources.Text.Progress_CompiledColorMap);
                };
            }

            this.MaterialOptions.ShowDialog(this);
        }

        private void contributorsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var form = new Credits();
            form.ShowDialog(this);
        }

        private void sizingToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var form = new SizeForm(this.Options);
            form.ShowDialog(this);
        }

        private void preprocessingToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.ColorReducerForm == null || this.ColorReducerForm.IsDisposed)
            {
                this.ColorReducerForm = new ColorReducerForm(this, this.Options);
                this.snapManager.RegisterChild(this.ColorReducerForm);
            }

            if (!this.ColorReducerForm.Visible)
            {
                this.ColorReducerForm.Show(this);
            }
        }
    }
}
