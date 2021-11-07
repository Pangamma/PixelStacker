using PixelStacker.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Forms
{
    public partial class ColorReducerForm: ILocalized
    {
        public void ApplyLocalization(CultureInfo locale)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_Instructions_Tooltip;
            lblRgbBucketSize.Text = Resources.Text.ColorReducer_RgbBucketSize;
            lblAlgorithm.Text = Resources.Text.ColorReducer_Algorithm;
            lblColorCount.Text = Resources.Text.ColorReducer_QColorCount;
            lblDither.Text = Resources.Text.ColorReducer_Dither;
            lblInstructionsTitle.Text = Resources.Text.ColorReducer_Instructions;
            lblParallel.Text = Resources.Text.ColorReducer_Parallel;
            cbxEnableQuantizer.Text = Resources.Text.Action_EnableQuantizer;
        }

        private void lblInstructionsTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            lblInstructions.Text = Resources.Text.ColorReducer_Instructions_Tooltip;
        }

        private void lblRgbBucketSize_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_RgbBucketSize_Tooltip;
        }

        private void lblAlgorithm_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_Algorithm_Tooltip;
        }

        private void lblColorCache_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_ColorCache_Tooltip;
        }

        private void lblColorCount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_ColorCount_Tooltip;
        }

        private void lblParallel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_Parallel_Tooltip;
        }

        private void lblDither_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblInstructions.Text = Resources.Text.ColorReducer_Dither_Tooltip;
        }
    }
}
