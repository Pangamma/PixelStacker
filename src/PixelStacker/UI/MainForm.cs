using PixelStacker.Logic;
using PixelStacker.Properties;
using PixelStacker.UI;
using SimplePaletteQuantizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker
{
    public partial class MainForm : Form
    {
        public static MainForm Self;
        public Bitmap LoadedImage { get; private set; } = Resources.psg.To32bppBitmap();
        public Bitmap PreRenderedImage { get; set; } = null;
        public BlueprintPA LoadedBlueprint { get; private set; }
        private string loadedImageFilePath { get; set; }
        private MaterialOptionsWindow MaterialOptions { get; set; } = null;
        public static PanZoomSettings PanZoomSettings { get; set; } = null;
        private ColorPaletteStyle SelectedColorPaletteStyle { get; set; } = ColorPaletteStyle.DetailedGrid;

        public MainForm()
        {
            Self = this;
            InitializeComponent();
            this.imagePanelMain.SetImage(LoadedImage);
            this.Text = this.Text + " v" + Constants.Version;
            if (!Constants.IsFullVersion)
            {
                togglePaletteToolStripMenuItem.Visible = false;
                mi_PreRenderOptions.Visible = false;
                this.Text = this.Text + " (Free Version)";
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetViewModeCheckBoxStates();

            #pragma warning disable CS4014 // We do not need to wait for this to complete before exiting our synchronized method. Fire and forget.
            TaskManager.Get.StartAsync(cancelToken => UpdateChecker.CheckForUpdates(cancelToken));
            #pragma warning restore CS4014
        }

        #region Events
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MaterialOptions == null)
            {
                this.MaterialOptions = new MaterialOptionsWindow(this);
            }

            TaskManager.Get.CancelTasks(null);
            this.MaterialOptions.ShowDialog(this);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgOpen.ShowDialog();
        }

        private void dlgOpen_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog dialog = (OpenFileDialog)sender;
            this.loadedImageFilePath = dialog.FileName;
            this.reOpenToolStripMenuItem.Enabled = true;
            using (Bitmap img = (Bitmap)Bitmap.FromFile(this.loadedImageFilePath))
            {
                this.LoadedImage.DisposeSafely();
                this.LoadedImage = img.To32bppBitmap(); // creates a clone of the img, but in the 32bpp format.
            }

            MainForm.PanZoomSettings = null;
            this.imagePanelMain.SetImage(this.LoadedImage);
            this.PreRenderedImage.DisposeSafely();
            this.PreRenderedImage = null;
            ShowImagePanel();
        }

        public void PreRenderImage(bool clearCache, CancellationToken? cancelToken)
        {
            if (clearCache)
            {
                this.PreRenderedImage.DisposeSafely();
                this.PreRenderedImage = null;
            }

            if (this.PreRenderedImage == null)
            {
                var engine = QuantizerEngine.Get;
                // Let's figure out sizing now.

                var LIM = this.LoadedImage;
                int mH = Math.Min(Options.Get.MaxHeight ?? LIM.Height, LIM.Height);
                int mW = Math.Min(Options.Get.MaxWidth ?? LIM.Width, LIM.Width);

                int H = LIM.Height;
                int W = LIM.Width;

                if (mW < mH)
                {
                    H = mW * H / W;
                    W = mW;
                }
                else
                {
                    W = mH * W / H;
                    H = mH;
                }

                var srcImage = this.LoadedImage.To32bppBitmap(W, H);

                Bitmap img = null;

                if (Options.Get.PreRender_IsEnabled)
                {
                    try
                    {
                        img = engine.RenderImage(srcImage);
                        srcImage.DisposeSafely();
                        cancelToken?.SafeThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    // We can simplify this so that we cut total color counts down massively.
                    srcImage.ToEditStream(cancelToken, (int x, int y, Color c) =>
                    {
                        int a, r, b, g = 0;
                        a = (c.A < 32) ? 0 : 255;
                        r = (int)Math.Round(Convert.ToDouble(c.R) / Constants.ColorFragmentSize, 0) * Constants.ColorFragmentSize;
                        g = (int)Math.Round(Convert.ToDouble(c.G) / Constants.ColorFragmentSize, 0) * Constants.ColorFragmentSize;
                        b = (int)Math.Round(Convert.ToDouble(c.B) / Constants.ColorFragmentSize, 0) * Constants.ColorFragmentSize;
                        return Color.FromArgb(a, r, g, b);
                    });

                    img = srcImage;
                }

                // Resize based on new size
                {
                    var imgForSize = this.PreRenderedImage ?? this.LoadedImage;
                    if (W != imgForSize.Width || H != imgForSize.Height)
                    {
                        MainForm.PanZoomSettings = null;
                    }

                }

                this.PreRenderedImage = img;
                this.InvokeEx((c) =>
                {
                    c.imagePanelMain.SetImage(PreRenderedImage);
                    c.ShowImagePanel();
                });
            }
        }

        private void SetViewModeCheckBoxStates()
        {
            toggleGridToolStripMenuItem.Checked = Options.Get.Rendered_IsShowGrid;
            togglePaletteToolStripMenuItem.Checked = Options.Get.Rendered_IsColorPalette;
            toggleBorderToolStripMenuItem.Checked = Options.Get.Rendered_IsShowBorder;
            toggleSolidColorsToolStripMenuItem.Checked = Options.Get.Rendered_IsSolidColors;
            toggleLayerFilterToolStripMenuItem.Checked = Options.Get.IsEnabled(Constants.RenderedZIndexFilter, false);
        }

        private async void renderSchematicToolStripMenuItem_Click(object _sender, EventArgs _e)
        {
            await RenderImageAndShowIt();
        }

        private void saveMenuClick(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint != null)
            {
                this.setupSaveForSchematics();
                dlgSave.ShowDialog();
            }
            else
            {
                this.exportSchematicToolStripMenuItem.Enabled = false;
            }
        }

        private void toggleGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options.Get.Rendered_IsShowGrid = !Options.Get.Rendered_IsShowGrid;
            Options.Save();
            SetViewModeCheckBoxStates();
            Refresh();
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

        private async void toggleSolidColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options.Get.Rendered_IsSolidColors = !Options.Get.Rendered_IsSolidColors;
            if (Options.Get.Rendered_IsSolidColors) Options.Get.Rendered_IsColorPalette = false;
            Options.Save();
            SetViewModeCheckBoxStates();
            await this.renderedImagePanel.ForceReRender();
        }

        private void toggleBorderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options.Get.Rendered_IsShowBorder = !Options.Get.Rendered_IsShowBorder;
            Options.Save();
            SetViewModeCheckBoxStates();
            Refresh();
        }

        private async void togglePaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options.Get.Rendered_IsColorPalette = !Options.Get.Rendered_IsColorPalette;
            if (Options.Get.Rendered_IsColorPalette) Options.Get.Rendered_IsSolidColors = false;
            Options.Save();
            SetViewModeCheckBoxStates();
            await this.renderedImagePanel.ForceReRender();
        }

        private void dlgSave_FileOk(object sender, CancelEventArgs e)
        {
            if (this.LoadedBlueprint != null)
            {
                SaveFileDialog dlg = (SaveFileDialog)sender;
                string fName = dlg.FileName;
                int chosenFilterIndex = dlg.FilterIndex;

                if (fName.ToLower().EndsWith(".schem"))
                {
                    SchemFormatter.writeBlueprint(fName, this.LoadedBlueprint);
                }
                else if (fName.ToLower().EndsWith(".schematic"))
                {
                    SchematicFormatter.writeBlueprint(fName, this.LoadedBlueprint);
                }
                else if (fName.ToLower().EndsWith(".png"))
                {
                    if (this.renderedImagePanel != null)
                    {
                        this.renderedImagePanel.SaveToPNG(fName);
                    }
                }
                else if (fName.ToLower().EndsWith(".csv"))
                {
                    Dictionary<Material, int> materialCounts = new Dictionary<Material, int>();
                    bool isv = Options.Get.IsSideView;
                    int xM = this.LoadedBlueprint.Mapper.GetXLength(isv);
                    int yM = this.LoadedBlueprint.Mapper.GetYLength(isv);
                    int zM = this.LoadedBlueprint.Mapper.GetZLength(isv);
                    for (int x = 0; x < xM; x++)
                    {
                        for (int y = 0; y < yM; y++)
                        {
                            for (int z = 0; z < zM; z++)
                            {
                                Material m = this.LoadedBlueprint.Mapper.GetMaterialAt(isv, x, y, z);
                                if (m != Materials.Air)
                                {
                                    if (!materialCounts.ContainsKey(m))
                                    {
                                        materialCounts.Add(m, 0);
                                    }

                                    materialCounts[m] = materialCounts[m] + 1;
                                }
                            }
                        }
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("\"Material\",\"Block Count\",\"Full Stacks needed\"");
                    sb.AppendLine("\"Total\"," + materialCounts.Values.Sum());
                    foreach (var kvp in materialCounts.OrderByDescending(x => x.Value))
                    {
                        sb.AppendLine($"\"{kvp.Key.GetBlockNameAndData(isv).Replace("\"", "\"\"")}\",{kvp.Value},{kvp.Value / 64} stacks and {kvp.Value % 64} remaining blocks");
                    }
                    File.WriteAllText(fName, sb.ToString());
                }
            }
            else
            {
                this.exportSchematicToolStripMenuItem.Enabled = false;
            }
        }

        private void dlgSave_FileOk_ColorPalettes(object sender, CancelEventArgs e)
        {
            if (this.LoadedBlueprint != null)
            {
                SaveFileDialog dlg = (SaveFileDialog)sender;
                string fName = dlg.FileName;
                ColorPaletteFormatter.writeBlueprint(fName, this.LoadedBlueprint, SelectedColorPaletteStyle);
            }
        }

        private void reOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.loadedImageFilePath != null)
            {
                using (Bitmap img = (Bitmap)Bitmap.FromFile(this.loadedImageFilePath))
                {
                    this.LoadedImage.DisposeSafely();
                    this.LoadedImage = img.To32bppBitmap();
                }
                MainForm.PanZoomSettings = null;
                this.imagePanelMain.SetImage(this.LoadedImage);
                ShowImagePanel();
                this.PreRenderedImage.DisposeSafely();
                this.PreRenderedImage = null;
            }
        }
        #endregion

        #region Utility / Draw methods
        public void ShowImagePanel()
        {
            this.imagePanelMain.Show();
            this.renderedImagePanel.Hide();
            this.imagePanelMain.BringToFront();
            this.exportSchematicToolStripMenuItem.Enabled = false;
            this.saveColorPaletteToolStripMenuItem.Enabled = false;
            this.toggleBorderToolStripMenuItem.Enabled = false;
            this.toggleGridToolStripMenuItem.Enabled = false;
            this.toggleSolidColorsToolStripMenuItem.Enabled = false;
            this.togglePaletteToolStripMenuItem.Enabled = false;
            this.toggleLayerFilterToolStripMenuItem.Enabled = false;
            this.up1LayerToolStripMenuItem.Enabled = false;
            this.down1LayerToolStripMenuItem.Enabled = false;
            this.layerFilteringToolStripMenuItem.Enabled = false;
        }

        public void ShowRenderedImagePanel()
        {
            this.renderedImagePanel.Show();
            this.renderedImagePanel.BringToFront();
            this.imagePanelMain.Hide();
            this.exportSchematicToolStripMenuItem.Enabled = true;
            this.saveColorPaletteToolStripMenuItem.Enabled = true;
            this.toggleBorderToolStripMenuItem.Enabled = true;
            this.toggleGridToolStripMenuItem.Enabled = true;
            this.toggleSolidColorsToolStripMenuItem.Enabled = true;
            this.toggleLayerFilterToolStripMenuItem.Enabled = Constants.IsFullVersion;
            this.up1LayerToolStripMenuItem.Enabled = Constants.IsFullVersion;
            this.down1LayerToolStripMenuItem.Enabled = Constants.IsFullVersion;
            this.layerFilteringToolStripMenuItem.Enabled = Constants.IsFullVersion;
            this.togglePaletteToolStripMenuItem.Enabled = Constants.IsFullVersion;
        }

        private void drawGrid(Bitmap bm, Graphics g, int blockSize, Pen p)
        {
            int numHorizBlocks = (bm.Width / blockSize);
            int numVertBlocks = (bm.Height / blockSize);
            g.DrawLine(p, 0, 0, 0, bm.Height * Constants.TextureSize);
            g.DrawLine(p, 0, bm.Height * Constants.TextureSize, bm.Width * Constants.TextureSize, bm.Height * Constants.TextureSize);
            g.DrawLine(p, bm.Width * Constants.TextureSize, bm.Height * Constants.TextureSize, bm.Width * Constants.TextureSize, 0);
            g.DrawLine(p, bm.Width * Constants.TextureSize, 0, 0, 0);
            for (int x = 0; x < numHorizBlocks; x++)
            {
                g.DrawLine(p, x * blockSize * Constants.TextureSize, 0, x * blockSize * Constants.TextureSize, bm.Height * Constants.TextureSize);
            }
            for (int y = 0; y < numVertBlocks; y++)
            {
                g.DrawLine(p, 0, y * blockSize * Constants.TextureSize, bm.Width * Constants.TextureSize, y * blockSize * Constants.TextureSize);
            }
        }
        #endregion

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new AboutForm();
            about.ShowDialog(this);
        }

        private void prerenderOptions_Click(object sender, EventArgs e)
        {
            var about = new PreRenderOptionsForm(this);
            about.Show();
        }

        private async void mi_preRender_Click(object sender, EventArgs e)
        {
            await TaskManager.Get.StartAsync((token) =>
            {
                PreRenderImage(true, token);
            });
        }

        private void toggleProgressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isShowing = this.progressBar.Visible == false;
            int deltaY = !isShowing ? this.progressBar.Height : -this.progressBar.Height;
            this.progressBar.Visible = isShowing;
            {
                var oSize = this.imagePanelMain.Size;
                var nSize = new Size(oSize.Width, oSize.Height + deltaY);
                this.imagePanelMain.Size = nSize;
            }
            {
                var oSize = this.renderedImagePanel.Size;
                var nSize = new Size(oSize.Width, oSize.Height + deltaY);
                this.renderedImagePanel.Size = nSize;
            }
        }

        private async void toggleLayerFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool orig = Options.Get.IsEnabled(Constants.RenderedZIndexFilter, false);
            Options.Get.SetEnabled(Constants.RenderedZIndexFilter, !orig);
            this.up1LayerToolStripMenuItem.Enabled = !orig;
            this.down1LayerToolStripMenuItem.Enabled = !orig;
            await this.renderedImagePanel.ForceReRender();
        }

        private async void up1LayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Options.Get.IsEnabled(Constants.RenderedZIndexFilter, false))
            {
                int nVal = Math.Max(Options.Get.Rendered_RenderedZIndexToShow - 1, 0);
                Options.Get.Rendered_RenderedZIndexToShow = nVal;
                await this.renderedImagePanel.ForceReRender();
            }
        }

        private async void down1LayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Options.Get.IsEnabled(Constants.RenderedZIndexFilter, false))
            {
                int nVal = Math.Min((Options.Get.Rendered_RenderedZIndexToShow) + 1, this.LoadedBlueprint.MaxDepth - 1);
                Options.Get.Rendered_RenderedZIndexToShow = nVal;
                await this.renderedImagePanel.ForceReRender();
            }
        }

        private void otherOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskManager.Get.CancelTasks(null);
            // Cancel from UI thread
            var about = new OtherOptionsWindow();
            about.ShowDialog(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TaskManager.Get.UpdateStatus(this);
        }

        private void setupSaveForSchematics()
        {
            dlgSave.FileOk -= this.dlgSave_FileOk;
            dlgSave.FileOk -= this.dlgSave_FileOk_ColorPalettes;
            dlgSave.Filter = "Schem (1.13+)|*.schem|PNG|*.png|Schematic|*.schematic|Block Counts CSV|*.csv";
            dlgSave.FileOk += this.dlgSave_FileOk;
        }

        private void openSaveForColorPalettes(int filterIndex)
        {
            dlgSave.FileOk -= this.dlgSave_FileOk;
            dlgSave.FileOk -= this.dlgSave_FileOk_ColorPalettes;
            string[] availableExtensions = new string[] { "Color Palette Graph | *.png", "Color Palette Brick|*.png", "Color Palette All (compact)|*.png", "Color Palette All (detailed)|*.png" };
            dlgSave.Filter = availableExtensions[filterIndex];
            dlgSave.FileOk += this.dlgSave_FileOk_ColorPalettes;
            dlgSave.DefaultExt = availableExtensions[filterIndex].Substring(availableExtensions[filterIndex].LastIndexOf("*.") + 2);
            dlgSave.ShowDialog(this);
        }

        private void graphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint == null)
            {
                this.saveColorPaletteToolStripMenuItem.Enabled = false;
                return;
            }

            this.SelectedColorPaletteStyle = ColorPaletteStyle.CompactGraph;
            this.openSaveForColorPalettes(0);
        }

        private void brickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint == null)
            {
                this.saveColorPaletteToolStripMenuItem.Enabled = false;
                return;
            }

            this.SelectedColorPaletteStyle = ColorPaletteStyle.CompactBrick;
            this.openSaveForColorPalettes(1);
        }

        private void allPossibilitiescompactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint == null)
            {
                this.saveColorPaletteToolStripMenuItem.Enabled = false;
                return;
            }

            this.SelectedColorPaletteStyle = ColorPaletteStyle.CompactGrid;
            this.openSaveForColorPalettes(2);
        }

        private void allColorsdetailedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.LoadedBlueprint == null)
            {
                this.saveColorPaletteToolStripMenuItem.Enabled = false;
                return;
            }

            this.SelectedColorPaletteStyle = ColorPaletteStyle.DetailedGrid;
            this.openSaveForColorPalettes(3);
        }
    }
}
