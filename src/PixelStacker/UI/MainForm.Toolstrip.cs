using PixelStacker.Extensions;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Utilities;
using PixelStacker.Resources.Themes;
using System.Threading.Tasks;

namespace PixelStacker.UI
{
    public class MainFormTags
    {
        // TODO: "IsFirstRenderRequired"
        public bool IsCanvasEditorRequired = false;
        public bool IsCanvasViewerRequired = false;
        public bool IsAdvancedOnly = false;
        public bool IsFileLoadPathRequired = false;
    }

    public partial class MainForm
    {

        private void TS_SetAllMenubarStatesBasedOnOptions(Options opts)
        {
            this.toggleGridToolStripMenuItem.Checked = opts.ViewerSettings.IsShowGrid;
            this.toggleBorderToolStripMenuItem.Checked = opts.ViewerSettings.IsShowBorder;
            this.horizontalToolStripMenuItem.Checked = !opts.IsSideView;
            this.verticalToolStripMenuItem.Checked = opts.IsSideView;

            this.showBottomLayerToolStripMenuItem.Checked = opts.ViewerSettings.ZLayerFilter == 0;
            this.showTopLayerToolStripMenuItem.Checked = opts.ViewerSettings.ZLayerFilter == 1;
            this.showBothLayersToolStripMenuItem.Checked = opts.ViewerSettings.ZLayerFilter == null;

            this.toggleShadowsToolStripMenuItem.Checked = opts.ViewerSettings.IsShadowRenderingEnabled;
            this.tsTextureSize16.Checked = opts.ViewerSettings.TextureSize == 16;
            this.tsTextureSize32.Checked = opts.ViewerSettings.TextureSize == 32;
            this.tsTextureSize64.Checked = opts.ViewerSettings.TextureSize == 64;
        }

        public void TS_SetTagObjects()
        {
            this.menuStrip1.ModifyRecursive((ts, mft) =>
            {
                ts.Tag = new MainFormTags();
            });

            this.fileToolStripMenuItem.Tag = new MainFormTags() { };
            this.editToolStripMenuItem.ModifyRecursive((x, tag) => tag.IsCanvasEditorRequired = true);
            this.viewToolStripMenuItem.ModifyRecursive((x, tag) => tag.IsCanvasEditorRequired = true);
            this.textureSizeToolStripMenuItem.ModifyRecursive((x, tag) => tag.IsAdvancedOnly = true);
            this.themesToolStripMenuItem.ModifyRecursive((x, tag) => tag.IsCanvasEditorRequired = false);

            this.layerFilteringToolStripMenuItem.ModifyRecursive((x, tag) => tag.IsCanvasEditorRequired = true);

            {
                MainFormTags mf = (MainFormTags)this.saveAsToolStripMenuItem.Tag;
                mf.IsCanvasEditorRequired = true;
            }
            {
                MainFormTags mf = (MainFormTags)this.saveToolStripMenuItem.Tag;
                mf.IsCanvasEditorRequired = true;
                mf.IsFileLoadPathRequired = true;
            }
            {
                MainFormTags mf = (MainFormTags)this.reOpenToolStripMenuItem.Tag;
                mf.IsFileLoadPathRequired = false;
            }


            ((MainFormTags)this.viewToolStripMenuItem.Tag).IsCanvasEditorRequired = false;
            ((MainFormTags)this.switchPanelsToolStripMenuItem.Tag).IsCanvasEditorRequired = true;
        }


        public void TS_SetMenuItemStatesByTagObjects()
        {
            bool isAdv = Options.IsAdvancedModeEnabled;
            bool isCanvas = this.IsCanvasEditorVisible;
            bool isLoadPathSet = !string.IsNullOrWhiteSpace(this.loadedImageFilePath);

            //this.swatchToolStripMenuItem.Visible = isCanvas && isAdv;
            this.menuStrip1.ModifyRecursive((ts, tag) =>
            {
                if (tag == null) return;

                if (tag.IsAdvancedOnly)
                {
                    ts.Visible = isAdv;
                    if (ts.BackColor != System.Drawing.Color.Aqua)
                    {
                        ts.BackColor = System.Drawing.Color.Aqua;
                    }
                }

                bool isEnabled = true;

                if (tag.IsCanvasEditorRequired)
                {
                    isEnabled &= isCanvas;
                }
                else if (tag.IsCanvasViewerRequired)
                {
                    isEnabled &= !isCanvas;
                }

                if (tag.IsFileLoadPathRequired)
                {
                    isEnabled &= isLoadPathSet;
                }

                ts.Enabled = isEnabled;
            });
        }

        public void TS_OnOpenFile()
        {
            string ext = this.loadedImageFilePath.GetFileExtension();
            saveToolStripMenuItem.Enabled = "pxlzip".Contains(ext);
            saveAsToolStripMenuItem.Enabled = true;
        }

        public void TS_OnRenderCanvas()
        {
            string ext = this.loadedImageFilePath?.GetFileExtension() ?? "NOPE";
            saveToolStripMenuItem.Enabled = "pxlzip".Contains(ext);
            saveAsToolStripMenuItem.Enabled = true;
        }

        private async void toggleShadowsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Options.ViewerSettings.IsShadowRenderingEnabled = !this.Options.ViewerSettings.IsShadowRenderingEnabled;
            this.toggleShadowsToolStripMenuItem.Checked = this.Options.ViewerSettings.IsShadowRenderingEnabled;
            this.Options.Save();
            await TriggerCanvasEditorRerender();
        }

        private void undoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.IsCanvasEditorVisible == false) return;
            if (this.canvasEditor.Painter.History.IsUndoEnabled)
            {
                this.canvasEditor.Painter.History.UndoChange();
                this.canvasEditor.RepaintRequested = true;
            }
        }

        private void redoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.IsCanvasEditorVisible == false) return;
            if (this.canvasEditor.Painter.History.IsRedoEnabled)
            {
                this.canvasEditor.Painter.History.RedoChange();
                this.canvasEditor.RepaintRequested = true;
            }
        }

        private void toggleGridToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Options.ViewerSettings.IsShowGrid = !this.Options.ViewerSettings.IsShowGrid;
            toggleGridToolStripMenuItem.Checked = this.Options.ViewerSettings.IsShowGrid;
            this.canvasEditor.Refresh();
        }
        private void toggleBorderToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Options.ViewerSettings.IsShowBorder = !this.Options.ViewerSettings.IsShowBorder;
            toggleBorderToolStripMenuItem.Checked = this.Options.ViewerSettings.IsShowBorder;
            this.canvasEditor.Refresh();
        }

        private void horizontalToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Options.IsSideView = false;
            this.horizontalToolStripMenuItem.Checked = !this.Options.IsSideView;
            this.verticalToolStripMenuItem.Checked = this.Options.IsSideView;
        }

        private void verticalToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Options.IsSideView = true;
            this.horizontalToolStripMenuItem.Checked = !this.Options.IsSideView;
            this.verticalToolStripMenuItem.Checked = this.Options.IsSideView;
        }

        private async void showBothLayersToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.Options.ViewerSettings.ZLayerFilter == null)
            {
                return;
            }

            this.Options.Tools.ZLayerFilter = ZLayer.Both;
            this.Options.ViewerSettings.ZLayerFilter = null;
            this.Options.Save();
            this.canvasEditor.Tools_SetLayerFilterImage();
            this.TS_SetAllMenubarStatesBasedOnOptions(this.Options);
            await TriggerCanvasEditorRerender();
        }

        private async void showBottomLayerToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.Options.ViewerSettings.ZLayerFilter == 0)
            {
                return;
            }

            this.Options.Tools.ZLayerFilter = ZLayer.Bottom;
            this.Options.ViewerSettings.ZLayerFilter = 0;
            this.Options.Save();
            this.canvasEditor.Tools_SetLayerFilterImage();
            this.TS_SetAllMenubarStatesBasedOnOptions(this.Options);
            await TriggerCanvasEditorRerender();
        }

        private async void showTopLayerToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.Options.ViewerSettings.ZLayerFilter == 1)
            {
                return;
            }

            this.Options.Tools.ZLayerFilter = ZLayer.Top;
            this.Options.ViewerSettings.ZLayerFilter = 1;
            this.Options.Save();
            this.canvasEditor.Tools_SetLayerFilterImage();
            this.TS_SetAllMenubarStatesBasedOnOptions(this.Options);
            await TriggerCanvasEditorRerender();
        }

        private async void tsTextureSize16_Click(object sender, System.EventArgs e)
        => await tsTextureSizeChangeClick(sender, e, 16);
        private async void tsTextureSize32_Click(object sender, System.EventArgs e)
        => await tsTextureSizeChangeClick(sender, e, 32);
        private async void tsTextureSize64_Click(object sender, System.EventArgs e)
        => await tsTextureSizeChangeClick(sender, e, 64);

        private async Task tsTextureSizeChangeClick(object sender, System.EventArgs e, int size)
        {
            this.Options.ViewerSettings.TextureSize = size;
            this.tsTextureSize16.Checked = this.Options.ViewerSettings.TextureSize == 16;
            this.tsTextureSize32.Checked = this.Options.ViewerSettings.TextureSize == 32;
            this.tsTextureSize64.Checked = this.Options.ViewerSettings.TextureSize == 64;
            this.Options.Save();
            this.TS_SetAllMenubarStatesBasedOnOptions(this.Options);
            await TriggerCanvasEditorRerender();
        }
    }
}
