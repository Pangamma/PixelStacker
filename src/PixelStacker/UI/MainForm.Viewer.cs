using PixelStacker.Logic;
using PixelStacker.Logic.Collections;
using PixelStacker.Logic.Extensions;
using PixelStacker.UI;
using SimplePaletteQuantizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker
{
    partial class MainForm
    {
        #region Toolstrip
        private async void renderSchematicToolStripMenuItem_Click(object _sender, EventArgs _e)
        {
            await RenderImageAndShowIt();
        }

        private void toggleGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options.Get.Rendered_IsShowGrid = !Options.Get.Rendered_IsShowGrid;
            Options.Save();
            SetViewModeCheckBoxStates();
            Refresh();
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
                int nVal = Math.Min((Options.Get.Rendered_RenderedZIndexToShow) + 1, this.LoadedBlueprint.MaxDepth - 1);
                Options.Get.Rendered_RenderedZIndexToShow = nVal;
                await this.renderedImagePanel.ForceReRender();
            }
        }

        private async void down1LayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Options.Get.IsEnabled(Constants.RenderedZIndexFilter, false))
            {
                int nVal = Math.Max(Options.Get.Rendered_RenderedZIndexToShow - 1, 0);
                Options.Get.Rendered_RenderedZIndexToShow = nVal;
                await this.renderedImagePanel.ForceReRender();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.History.UndoChange();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.History.RedoChange();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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


        public void PreRenderImage(bool clearCache, CancellationToken _worker)
        {
            if (clearCache)
            {
                this.PreRenderedImage.DisposeSafely();
                this.PreRenderedImage = null;
                this.History.Clear();
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

                if (Options.Get.PreRender_ColorCacheFragmentSize > 1)
                {
                    // We can simplify this so that we cut total color counts down massively.
                    srcImage.ToEditStream(_worker, (int x, int y, Color c) =>
                    {
                        int a = (c.A < 32) ? 0 : 255;
                        return Color.FromArgb(a, c.Normalize());
                    });
                }

                img = srcImage;

                if (Options.Get.PreRender_IsEnabled)
                {
                    try
                    {
                        img = engine.RenderImage(srcImage);
                        srcImage.DisposeSafely();
                        _worker.SafeThrowIfCancellationRequested();
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

                // Do this step BEFORE setting panzoom to null. Very important.
                this.PreRenderedImage = RenderedImagePanel.RenderPlaceholderBitmapFromBlueprint(_worker, img);

                // Resize based on new size
                {
                    var imgForSize = this.PreRenderedImage ?? this.LoadedImage;
                    if (W != imgForSize.Width || H != imgForSize.Height)
                    {
                        MainForm.PanZoomSettings = null;
                    }

                }

                this.InvokeEx((c) =>
                {
                    c.imagePanelMain.SetImage(c.PreRenderedImage);
                    c.ShowImagePanel();
                });
            }
        }

        #endregion

        private async void mi_preRender_Click(object sender, EventArgs e)
        {
            await TaskManager.Get.StartAsync((token) =>
            {
                PreRenderImage(true, token);
            });
        }

    }
}
