using PixelStacker.Logic;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.Utilities;
using PixelStacker.Logic.WIP;
using PixelStacker.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI
{
    // Do not force items to open as design style, also allow some things to be design and other things to be code
    public partial class MainForm : Form
    {
        public static MainForm Self;
        public Bitmap LoadedImage { get; private set; } = UIResources.weird_intro.To32bppBitmap();
        public Bitmap PreRenderedImage { get; set; } = null;
        public BlueprintPA LoadedBlueprint { get; private set; }
        public static PanZoomSettings PanZoomSettings { get; set; } = null;
        public EditHistory History { get; set; }

        public KonamiWatcher konamiWatcher;
        public MainForm()
        {
            Self = this;
            InitializeThreadLocale();
            InitializeComponent();
            this.konamiWatcher = new KonamiWatcher(() => {
                Options.Get.IsAdvancedModeEnabled = !Options.Get.IsAdvancedModeEnabled;
                MessageBox.Show("Advanced mode " + (Options.Get.IsAdvancedModeEnabled ? "enabled" : "disabled") + "!");
                doConfigureAdvancedMode();
            });
            this.imagePanelMain.SetImage(LoadedImage);
            this.Text = this.Text + " v" + Constants.Version;
#if !RELEASE
            this.Text += " (Debug)";
#endif
            this.History = new EditHistory(this);

            if (!Constants.IsFullVersion)
            {
#pragma warning disable CS0162 // Unreachable code detected
                this.togglePaletteToolStripMenuItem.Visible = false;
                this.saveColorPaletteToolStripMenuItem.Visible = false;
                mi_PreRenderOptions.Visible = false;
                this.Text += " (Free Version)";
#pragma warning restore CS0162 // Unreachable code detected
            }
            SetViewModeCheckBoxStates();
        }

        #region Auto update checker + Error Reporting
        private void MainForm_Load(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // We do not need to wait for this to complete before exiting our synchronized method. Fire and forget.
            TaskManager.Get.StartAsync(cancelToken => UpdateChecker.CheckForUpdates(cancelToken));
            //TaskManager.Get.StartAsync(cancelToken => PdbLoader.Load(cancelToken));
#pragma warning restore CS4014
        }
#endregion
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            this.konamiWatcher.ProcessKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SetViewModeCheckBoxStates()
        {
            toggleGridToolStripMenuItem.Checked = Options.Get.Rendered_IsShowGrid;
            togglePaletteToolStripMenuItem.Checked = Options.Get.Rendered_IsColorPalette;
            toggleBorderToolStripMenuItem.Checked = Options.Get.Rendered_IsShowBorder;
            toggleSolidColorsToolStripMenuItem.Checked = Options.Get.Rendered_IsSolidColors;
            toggleLayerFilterToolStripMenuItem.Checked = Options.Get.IsEnabled(Constants.RenderedZIndexFilter, false);
            doConfigureAdvancedMode();
        }

        private void doConfigureAdvancedMode()
        {
            bool isAdv = Options.Get.IsAdvancedModeEnabled;
            mi_preRender.Visible = isAdv;
            togglePaletteToolStripMenuItem.Visible = isAdv;
            toggleProgressToolStripMenuItem.Visible = isAdv;
            exportSettingsToolStripMenuItem.Visible = isAdv;
            this.MaterialOptions?.SetVisibleMaterials(Materials.List ?? new List<Material>());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TaskManager.Get.UpdateStatus(this);
        }

        public async Task RenderImageAndShowIt()
        {
            await TaskManager.Get.StartAsync((token) =>
            {
                TaskManager.SafeReport(0, "Optimizing...");

                PreRenderImage(false, token);

                if (this.PreRenderedImage != null)
                {
                    if (this.PreRenderedImage.Height > Constants.WORLD_HEIGHT && Options.Get.IsSideView)
                    {
                        MessageBox.Show(string.Format(Resources.Text.Error_CannotExceedWorldHeight_0, Constants.WORLD_HEIGHT));
                        return;
                    }
                }
                TaskManager.SafeReport(0, "Getting Blueprint...");
                BlueprintPA blueprint = BlueprintPA.GetBluePrintAsync(token, this.PreRenderedImage).GetAwaiter().GetResult();
                TaskManager.SafeReport(0, "Rendering Blueprint...");
                Bitmap renderedImage = RenderedImagePanel.RenderBitmapFromBlueprint(token, blueprint, out int? textureSize);
                if (textureSize == null) return;
                if (renderedImage == null) return;

                try
                {
                    MainForm.Self.InvokeEx(x =>
                    {
                        x.LoadedBlueprint = blueprint;
                        x.ShowRenderedImagePanel();
                        x.renderedImagePanel.SetBluePrint(blueprint, renderedImage, textureSize);
                        TaskManager.SafeReport(0, "Finished.");
                    });
                }
                catch (OperationCanceledException) { }

            });
        }
    }
}
