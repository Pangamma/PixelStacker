using PixelStacker.Logic;
using PixelStacker.Logic.Extensions;
using SimplePaletteQuantizer;
using SimplePaletteQuantizer.Quantizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class PreRenderOptionsForm : Form
    {
        private bool isInitializationComplete = false;
        private MainForm mainForm;

        public PreRenderOptionsForm(MainForm mainForm = null)
        {
            this.mainForm = mainForm;
            InitializeComponent();
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

            btnEnablePreRender.Text = Options.Get.PreRender_IsEnabled ? "Disable Quantizer" : "Enable Quantizer";
        }

        private async void btnEnablePreRender_Click(object sender, EventArgs e)
        {
            Options.Get.PreRender_IsEnabled = !Options.Get.PreRender_IsEnabled;
            btnEnablePreRender.Text = Options.Get.PreRender_IsEnabled ? "Disable Quantizer" : "Enable Quantizer";

            TaskManager.SafeReport(0, "Quantizing Image");
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
                TaskManager.SafeReport(0, "Quantizing Image");
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
                TaskManager.SafeReport(0, "Quantizing Image");
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
                TaskManager.SafeReport(0, "Quantizing Image");
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
                TaskManager.SafeReport(0, "Quantizing Image");
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
                TaskManager.SafeReport(0, "Quantizing Image");
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
                TaskManager.SafeReport(0, "Quantizing Image");
                await TaskManager.Get.StartAsync((token) =>
                {
                    Options.Save();
                    Materials.CompileColorMap(token, true);
                    mainForm?.PreRenderImage(true, token);
                });
            }
        }
    }
}
