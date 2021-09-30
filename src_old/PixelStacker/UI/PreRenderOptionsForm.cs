using PixelStacker.Logic;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.WIP;
using SimplePaletteQuantizer;
using SimplePaletteQuantizer.Quantizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class PreRenderOptionsForm : Form, ILocalized
    {
        private bool isInitializationComplete = false;
        private MainForm mainForm;

        public PreRenderOptionsForm(MainForm mainForm = null)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            ApplyLocalization(Thread.CurrentThread.CurrentUICulture);
            isInitializationComplete = false;
            this.ddlAlgorithm.SelectedItem = Options.Get.PreRender_Algorithm;
            this.ddlColorCache.SelectedItem = Options.Get.PreRender_ColorCache;
            this.ddlColorCount.SelectedItem = Options.Get.PreRender_ColorCount.ToString();
            this.ddlDither.SelectedItem = Options.Get.PreRender_Dither;
            this.ddlParallel.SelectedItem = Options.Get.PreRender_Parallel.ToString();

            this.ddlMaxNormalizedColorCount.SelectedItem = $"{255 / Options.Get.PreRender_ColorCacheFragmentSize}^3";

            foreach (var item in ddlMaxNormalizedColorCount.Items)
            {
                int bucketSize = item.ToString().Replace("^3", "").ToNullable<int>() ?? 51;
                if (bucketSize == 255 / Options.Get.PreRender_ColorCacheFragmentSize)
                {
                    ddlMaxNormalizedColorCount.SelectedItem = item;
                }
            }

            isInitializationComplete = true;

            btnEnablePreRender.Text = Options.Get.PreRender_IsEnabled ? Resources.Text.Action_DisableQuantizer : Resources.Text.Action_EnableQuantizer;
        }

        public void ApplyLocalization(System.Globalization.CultureInfo locale)
        {
            this.Text = Resources.Text.PreRenderOptions_QuantizerSettings;
            btnEnablePreRender.Text = Resources.Text.Action_EnableQuantizer;

            lblColorCacheSize.Text = Resources.Text.PreRenderOptions_ColorCacheSize;
            this.tooltip.SetToolTip(this.lblColorCacheSize, Resources.Text.PreRenderOptions_ColorCacheSize_Tooltip);
            
            lblAlgorithm.Text = Resources.Text.PreRenderOptions_Algorithm;
            this.tooltip.SetToolTip(this.lblAlgorithm, Resources.Text.PreRenderOptions_Algorithm_Tooltip);

            lblColorCache.Text = Resources.Text.PreRenderOptions_ColorCache;

            lblColorCount.Text = Resources.Text.PreRenderOptions_ColorCount;
            this.tooltip.SetToolTip(this.lblColorCount, Resources.Text.PreRenderOptions_ColorCount_Tooltip);

            lblParallel.Text = Resources.Text.PreRenderOptions_Parallel;
            this.tooltip.SetToolTip(this.lblParallel, Resources.Text.PreRenderOptions_Parallel_Tooltip);
           
            lblDither.Text = Resources.Text.PreRenderOptions_Dither;
            this.tooltip.SetToolTip(this.lblDither, Resources.Text.PreRenderOptions_Dither_Tooltip);
        }

        private async void btnEnablePreRender_Click(object sender, EventArgs e)
        {
            Options.Get.PreRender_IsEnabled = !Options.Get.PreRender_IsEnabled;
            btnEnablePreRender.Text = Options.Get.PreRender_IsEnabled ? Resources.Text.Action_DisableQuantizer : Resources.Text.Action_EnableQuantizer;

            TaskManager.SafeReport(0, Resources.Text.Progress_QuantizingImage);
            await TaskManager.Get.StartAsync((token) =>
            {
                mainForm?.PreRenderImage(true, token);
                Options.Save();
            });
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            MainForm.Self.konamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private async void ddlAlgorithm_SelectedValueChanged(object sender, EventArgs e)
        {
            Options.Get.PreRender_Algorithm = ddlAlgorithm.SelectedItem?.ToString();
            QuantizerEngine.Get.EnforceValidSettings(this);
            if (isInitializationComplete)
            {
                TaskManager.SafeReport(0, Resources.Text.Progress_QuantizingImage);
                await TaskManager.Get.StartAsync((token) =>
                {
                    mainForm?.PreRenderImage(true, token);
                    Options.Save();
                });
            }
        }

        private async void ddlColorCache_SelectedValueChanged(object sender, EventArgs e)
        {
            Options.Get.PreRender_ColorCache = ddlColorCache.SelectedItem?.ToString();
            QuantizerEngine.Get.EnforceValidSettings(this);
            if (isInitializationComplete)
            {
                TaskManager.SafeReport(0, Resources.Text.Progress_QuantizingImage);
                await TaskManager.Get.StartAsync((token) =>
                {
                    mainForm?.PreRenderImage(true, token);
                    Options.Save();
                });
            }
        }

        private async void ddlColorCount_SelectedValueChanged(object sender, EventArgs e)
        {
            Options.Get.PreRender_ColorCount = ddlColorCount.SelectedItem?.ToString().ToNullable<int>() ?? 256;
            QuantizerEngine.Get.EnforceValidSettings(this);
            if (isInitializationComplete)
            {
                TaskManager.SafeReport(0, Resources.Text.Progress_QuantizingImage);
                await TaskManager.Get.StartAsync((token) =>
                {
                    mainForm?.PreRenderImage(true, token);
                    Options.Save();
                });
            }
        }

        private async void ddlParallel_SelectedValueChanged(object sender, EventArgs e)
        {
            Options.Get.PreRender_Parallel = ddlParallel.SelectedItem?.ToString().ToNullable<int>() ?? 64;
            QuantizerEngine.Get.EnforceValidSettings(this);
            if (isInitializationComplete)
            {
                TaskManager.SafeReport(0, Resources.Text.Progress_QuantizingImage);
                await TaskManager.Get.StartAsync((token) =>
                {
                    mainForm?.PreRenderImage(true, token);
                    Options.Save();
                });
            }
        }

        private async void ddlDither_SelectedValueChanged(object sender, EventArgs e)
        {
            Options.Get.PreRender_Dither = ddlDither.SelectedItem?.ToString();
            QuantizerEngine.Get.EnforceValidSettings(this);
            if (isInitializationComplete)
            {
                TaskManager.SafeReport(0, Resources.Text.Progress_QuantizingImage);
                await TaskManager.Get.StartAsync((token) =>
                {
                    mainForm?.PreRenderImage(true, token);
                    Options.Save();
                });
            }
        }

        private async void ddlMaxNormalizedColorCount_SelectedValueChanged(object sender, EventArgs e)
        {
            int valFromDropDown = this.ddlMaxNormalizedColorCount.SelectedItem?.ToString().Replace("^3", "").ToNullable<int>() ?? 51;
            Options.Get.PreRender_ColorCacheFragmentSize = 255 / valFromDropDown;
            
            if (isInitializationComplete)
            {
                TaskManager.SafeReport(0, Resources.Text.Progress_QuantizingImage);
                await TaskManager.Get.StartAsync((token) =>
                {
                    Options.Save();
                    ColorMatcher.Get.CompileColorPalette(token, false, Materials.List)
                    .GetAwaiter().GetResult();
                    mainForm?.PreRenderImage(true, token);
                });
            }
        }
    }
}
