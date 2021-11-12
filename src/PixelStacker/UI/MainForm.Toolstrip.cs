using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using System.Collections.Generic;
using System.Windows.Forms;
using PixelStacker.Extensions;

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
        private void undoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.IsCanvasEditorVisible == false) return;
            if (this.canvasEditor.Painter.History.IsUndoEnabled)
            {
                var toRender = this.canvasEditor.Painter.History.UndoChange();
                this.canvasEditor.Painter.DoProcessRenderRecords(toRender);
            }
        }

        private void redoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.IsCanvasEditorVisible == false) return;
            if (this.canvasEditor.Painter.History.IsRedoEnabled)
            {
                var toRender = this.canvasEditor.Painter.History.RedoChange();
                this.canvasEditor.Painter.DoProcessRenderRecords(toRender);
            }
        }

        private void TS_SetAllMenubarStatesBasedOnOptions(Options opts)
        {
            this.toggleGridToolStripMenuItem.Checked = opts.ViewerSettings.IsShowGrid;
            this.toggleBorderToolStripMenuItem.Checked = opts.ViewerSettings.IsShowBorder;
            this.horizontalToolStripMenuItem.Checked = !opts.IsSideView;
            this.verticalToolStripMenuItem.Checked = opts.IsSideView;
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
            this.canvasEditorToolsToolStripMenuItem.ModifyRecursive((x, tag) => tag.IsCanvasEditorRequired = true);
            this.swatchToolStripMenuItem.ModifyRecursive((x, tag) => tag.IsCanvasEditorRequired = true);
            this.gridOptionsToolStripMenuItem.ModifyRecursive((x, tag) => tag.IsAdvancedOnly = true);
            this.shadowRenderingToolStripMenuItem.ModifyRecursive((x, tag) => tag.IsAdvancedOnly = true);
            this.layerFilteringToolStripMenuItem.ModifyRecursive((x, tag) => tag.IsAdvancedOnly = true);

            {
                MainFormTags mf = (MainFormTags)this.saveAsToolStripMenuItem.Tag;
                mf.IsCanvasEditorRequired = true;
            }
            {
                MainFormTags mf = (MainFormTags)this.saveToolStripMenuItem.Tag;
                mf.IsCanvasEditorRequired = true;
            }

            ((MainFormTags)this.viewToolStripMenuItem.Tag).IsCanvasEditorRequired = false;
            ((MainFormTags)this.switchPanelsToolStripMenuItem.Tag).IsCanvasEditorRequired = false;
            ((MainFormTags)this.reOpenToolStripMenuItem.Tag).IsFileLoadPathRequired = true;
        }


        public void TS_SetMenuItemStatesByTagObjects()
        {
            bool isAdv = Options.IsAdvancedModeEnabled;
            bool isCanvas = this.IsCanvasEditorVisible;
            bool isLoadPathSet = !string.IsNullOrWhiteSpace(this.loadedImageFilePath);
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

                if (tag.IsCanvasEditorRequired)
                {
                    ts.Enabled = isCanvas;
                }
                else if (tag.IsCanvasViewerRequired)
                {
                    ts.Enabled = !isCanvas;
                }

                if (tag.IsFileLoadPathRequired)
                {
                    ts.Enabled = isLoadPathSet;
                }
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
    }
}
