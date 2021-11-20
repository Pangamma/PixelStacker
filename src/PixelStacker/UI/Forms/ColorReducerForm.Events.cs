using PixelStacker.IO;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Forms
{
    public partial class ColorReducerForm
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            KonamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// </summary>
        private void OnSettingsChange()
        {
            Options.Save();
            MainForm.DoPreprocessLoadedImage();
        }

        private void ColorReducerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void cbxEnableQuantizer_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = cbxEnableQuantizer.Checked;
            ddlAlgorithm.Enabled = isChecked;
            ddlDither.Enabled = isChecked;
            ddlColorCount.Enabled = isChecked;
            ddlParallel.Enabled = isChecked;
            Options.Preprocessor.QuantizerSettings.IsEnabled = isChecked;
            RevalidateOptions(Options);
            OnSettingsChange();
        }

        private void ddlAlgorithm_SelectedValueChanged(object sender, EventArgs e)
        {
            if (IsNoisy) return;
            string val = ddlAlgorithm.SelectedItem as string;
            if (val == Options.Preprocessor.QuantizerSettings.Algorithm) return;
            if (val != null)
            {
                Options.Preprocessor.QuantizerSettings.Algorithm = val;
                RevalidateOptions(Options);
                OnSettingsChange();
            }
        }


        private void ddlRgbBucketSize_SelectedValueChanged(object sender, EventArgs e)
        {
            if (IsNoisy) return;
            if (ddlRgbBucketSize.SelectedItem == null) return;
            if (!MaxUniqueColorOptions.TryGetValue(ddlRgbBucketSize.SelectedItem as string, out int val)){
                val = 1;
            }

            if (val == Options.Preprocessor.RgbBucketSize) return;
            Options.Preprocessor.RgbBucketSize = val;
            RevalidateOptions(Options);
            OnSettingsChange();
        }

        private void ddlColorCount_SelectedValueChanged(object sender, EventArgs e)
        {
            if (IsNoisy) return;
            Options.Preprocessor.QuantizerSettings.MaxColorCount = (ddlColorCount.SelectedItem as string).ToNullable<int>() ?? 256;
            RevalidateOptions(Options);
            OnSettingsChange();
        }

        private void ddlDither_SelectedValueChanged(object sender, EventArgs e)
        {
            if (IsNoisy) return;
            Options.Preprocessor.QuantizerSettings.DitherAlgorithm = ddlDither.SelectedItem as string;

            RevalidateOptions(Options);
            OnSettingsChange();
        }

        private void ddlParallel_SelectedValueChanged(object sender, EventArgs e)
        {
            if (IsNoisy) return;
            Options.Preprocessor.QuantizerSettings.MaxParallelProcesses = (ddlParallel.SelectedItem as string).ToNullable<int>() ?? 1;

            RevalidateOptions(Options);
            OnSettingsChange();
        }
    }
}
