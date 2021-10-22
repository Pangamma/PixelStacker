using PixelStacker.Extensions;
using PixelStacker.IO;
using PixelStacker.Logic.Collections.ColorMapper;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        private Options Options;
        private IColorMapper ColorMapper;
        private MaterialPalette Palette;
        public Bitmap LoadedImage { get; private set; } = UIResources.weird_intro.To32bppBitmap();
        private RenderedCanvas RenderedCanvas;
        private KonamiWatcher konamiWatcher;

        public MainForm()
        {
            this.Options = new LocalDataOptionsProvider().Load();
            this.Options.Preprocessor.RgbBucketSize = 1;
            this.ColorMapper = new KdTreeMapper();
            this.Palette = MaterialPalette.FromResx();

            InitializeComponent();
            this.konamiWatcher = new KonamiWatcher(() => {
                this.Options.IsAdvancedModeEnabled = !this.Options.IsAdvancedModeEnabled;
                MessageBox.Show("Advanced mode " + (this.Options.IsAdvancedModeEnabled ? "enabled" : "disabled") + "!");
                DoConfigureAdvancedMode();
            });
            this.imageViewer.SetImage(this.LoadedImage);
            ShowImageViewer();
            this.lblProgress.Parent = this.progressBar1;
            this.lblProgress.BackColor = Color.Transparent;


            // Localization
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(this.Options.Locale ?? "en-us");
            ApplyLocalization(System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        private void DoConfigureAdvancedMode()
        {
            //bool isAdv = this.Options.IsAdvancedModeEnabled;
            //mi_preRender.Visible = isAdv;
            //togglePaletteToolStripMenuItem.Visible = isAdv;
            //toggleProgressToolStripMenuItem.Visible = isAdv;
            //exportSettingsToolStripMenuItem.Visible = isAdv;
            this.MaterialOptions?.SetVisibleMaterials(Materials.List ?? new List<Material>());
        }

        [DebuggerStepThrough]
        private void timer1_Tick(object sender, EventArgs e)
        {
            ProgressX.UpdateStatus((val, str) =>
            {
                if (progressBar1.Value != val)
                {
                    progressBar1.Value = val;
                }

                if (lblProgress.Text != str)
                {
                    lblProgress.Text = str;
                }
            });
        }
    }
}
