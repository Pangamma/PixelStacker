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
using SkiaSharp;
using PixelStacker.UI.Helpers;

namespace PixelStacker.UI
{
    public partial class MainForm
    {
        public readonly Options Options;
        private IColorMapper ColorMapper;
        private MaterialPalette Palette;
        public SKBitmap LoadedImage { get; private set; } = UIResources.weird_intro.BitmapToSKBitmap();
        public SKBitmap PreprocessedImage { get; private set; } = UIResources.weird_intro.BitmapToSKBitmap(); // UIResources.weird_intro.BitmapToSKBitmap();
        private RenderedCanvas RenderedCanvas;
        private SnapManager snapManager { get; }

        public MainForm()
        {
            this.Options = new LocalDataOptionsProvider().Load();
            this.ColorMapper = new KdTreeMapper();
            this.Palette = MaterialPalette.FromResx();
            InitializeComponent();
            InitializeKonamiCodeWatcher();
            this.snapManager = new SnapManager(this);

            this.canvasEditor.Options = this.Options;
            this.imageViewer.SetImage(this.LoadedImage);
            ShowImageViewer();
            //dlgSave.Filter = "Schem (1.13+)|*.schem|PNG|*.png|Block Counts CSV|*.csv|PixelStacker Project|*.pxlzip";
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            // Localization
            InitializeLocalization();
            ApplyLocalization(System.Threading.Thread.CurrentThread.CurrentUICulture);

            // Menubar initialization
            TS_SetTagObjects();
            TS_SetMenuItemStatesByTagObjects();
            TS_SetAllMenubarStatesBasedOnOptions(this.Options);

#pragma warning disable CS4014 // We do not need to wait for this to complete before exiting our synchronized method. Fire and forget.
            TaskManager.Get.StartAsync(cancelToken => UpdateChecker.CheckForUpdates(this.Options, cancelToken));
            //TaskManager.Get.StartAsync(cancelToken => PdbLoader.Load(cancelToken));
#pragma warning restore CS4014
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KonamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void InitializeKonamiCodeWatcher()
        {
            KonamiWatcher.OnCodeEntry = () =>
            {
                this.InvokeEx(c =>
                {
                    c.Options.IsAdvancedModeEnabled = !c.Options.IsAdvancedModeEnabled;
                    MessageBox.Show("Advanced mode " + (c.Options.IsAdvancedModeEnabled ? "enabled" : "disabled") + "!");
                    c.MaterialOptions?.SetVisibleMaterials(Materials.List ?? new List<Material>());
                    c.TS_SetMenuItemStatesByTagObjects();
                });
            };
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
