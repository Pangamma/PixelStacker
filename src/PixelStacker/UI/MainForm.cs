using PixelStacker.Logic;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.WIP;
using PixelStacker.Resources;
using PixelStacker.UI;
using SimplePaletteQuantizer;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker
{
    // Do not force items to open as design style, also allow some things to be design and other things to be code
    public partial class MainForm : Form
    {
        public static MainForm Self;
        public Bitmap LoadedImage { get; private set; } = UIResources.avatar.To32bppBitmap();
        public Bitmap PreRenderedImage { get; set; } = null;
        public BlueprintPA LoadedBlueprint { get; private set; }
        public static PanZoomSettings PanZoomSettings { get; set; } = null;
        public EditHistory History { get; set; }

        public MainForm()
        {
            Self = this;
            InitializeComponent();
            this.imagePanelMain.SetImage(LoadedImage);
            this.Text = this.Text + " v" + Constants.Version;
            this.History = new EditHistory(this);

            if (!Constants.IsFullVersion)
            {
#pragma warning disable CS0162 // Unreachable code detected
                this.togglePaletteToolStripMenuItem.Visible = false;
                this.saveColorPaletteToolStripMenuItem.Visible = false;
                mi_PreRenderOptions.Visible = false;
                this.Text = this.Text + " (Free Version)";
#pragma warning restore CS0162 // Unreachable code detected
            }
            SetViewModeCheckBoxStates();
        }
        private void SetViewModeCheckBoxStates()
        {
            toggleGridToolStripMenuItem.Checked = Options.Get.Rendered_IsShowGrid;
            togglePaletteToolStripMenuItem.Checked = Options.Get.Rendered_IsColorPalette;
            toggleBorderToolStripMenuItem.Checked = Options.Get.Rendered_IsShowBorder;
            toggleSolidColorsToolStripMenuItem.Checked = Options.Get.Rendered_IsSolidColors;
            toggleLayerFilterToolStripMenuItem.Checked = Options.Get.IsEnabled(Constants.RenderedZIndexFilter, false);
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
                    if (this.PreRenderedImage.Height > 256 && Options.Get.IsSideView)
                    {
                        MessageBox.Show("Minecraft cannot support images larger than 256 blocks in height.");
                        return;
                    }
                }

                BlueprintPA blueprint = BlueprintPA.GetBluePrintAsync(token, this.PreRenderedImage).Result;
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
