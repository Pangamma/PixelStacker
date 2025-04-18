using PixelStacker.EditorTools;
using PixelStacker.Logic.CanvasEditor.History;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        private AbstractCanvasEditorTool CurrentTool { get; set; }
        private PointerTool PanZoomTool { get; set; }

        private async void bgWorkerBufferedChangeQueue_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!this.Painter.History.HasChunksToRender())
            {
                e.Cancel = true;
                return;
            }

            while (this.Painter.History.TryGetRenderChunks(out List<RenderRecord> records))
            {
                var rg = records.GroupBy(x => x.RenderMode);
                {
                    var tiles = rg.FirstOrDefault(x => x.Key == RenderRecord.RenderRecordType.BLOCKS_ONLY)?.ToList();
                    if (tiles != null)
                        await this.Painter.DoProcessRenderRecords(tiles);
                }
                {
                    var tiles = rg.FirstOrDefault(x => x.Key == RenderRecord.RenderRecordType.SHADOWS_ONLY)?.ToList();
                    if (tiles != null)
                        this.Painter.DoApplyShadowsForRenderRecords(tiles);
                }
                {
                    var tiles = rg.FirstOrDefault(x => x.Key == RenderRecord.RenderRecordType.ALL)?.ToList();
                    if (tiles != null)
                    {
                        await this.Painter.DoProcessRenderRecords(tiles);
                        this.Painter.DoApplyShadowsForRenderRecords(tiles);
                    }
                }
                await Task.Yield();
            }

            e.Result = Task.CompletedTask;
        }

        private void BgWorkerBufferedChangeQueue_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.RepaintRequested = false;
            }
        }


        [DebuggerStepThrough]
        private void timerBufferedChangeQueue_Tick(object sender, System.EventArgs e)
        {
            if (this.Painter == null) return;
            if (bgWorkerBufferedChangeQueue.IsBusy) return;
            if (this.RepaintRequested == false) return;
            bgWorkerBufferedChangeQueue.RunWorkerAsync();
            //if (this.Painter == null) return;

            //IsRenderingBuffer = true;
            //await this.Painter.DoRenderFromHistoryBuffer();
            //IsRenderingBuffer = false;
            //this.RepaintRequested = true;
        }

        private void setWorldEditOriginHereToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (PointerTool.LastRightClickedPointOnPanel != null)
            {
                var tool = new WorldEditOriginTool(this);
                tool.OnClick(new MouseEventArgs(MouseButtons.Left, 1, PointerTool.LastRightClickedPointOnPanel.Value.X, PointerTool.LastRightClickedPointOnPanel.Value.Y, 0));
            }
        }

        private void btnSuggester_Click(object sender, System.EventArgs e)
        {
            this.CurrentTool = new BrushTool(this);
            this.OnToolClicked(sender);
        }

        private void Toolbox_OnClickPointer(object sender, System.EventArgs e)
        {
            this.CurrentTool = new PointerTool(this);
            this.OnToolClicked(sender);
        }

        private void Toolbox_OnClickBrush(object sender, EventArgs e)
        {
            this.CurrentTool = new BrushTool(this);
            this.OnToolClicked(sender);
        }

        private void Toolbox_OnClickPencil(object sender, EventArgs e)
        {
            this.CurrentTool = new PencilTool(this);
            this.OnToolClicked(sender);
        }

        private void Toolbox_OnClickPicker(object sender, EventArgs e)
        {
            this.CurrentTool = new PickerTool(this);
            this.OnToolClicked(sender);
        }

        private void Toolbox_OnClickFill(object sender, EventArgs e)
        {
            this.CurrentTool = new FillTool(this);
            this.OnToolClicked(sender);
        }

        private void Toolbox_OnClickEraser(object sender, EventArgs e)
        {
            this.CurrentTool = new EraserTool(this);
            this.OnToolClicked(sender);
        }

        private void Toolbox_OnClickWorldEditOrigin(object sender, EventArgs e)
        {
            this.CurrentTool = new WorldEditOriginTool(this);
            this.OnToolClicked(sender);
        }

        private void OnToolClicked(object sender)
        {
            this.Cursor = this.CurrentTool.GetCursor();
            if (sender is ToolStripButton btn)
            {
                foreach (ToolStripItem item in tsCanvasTools.Items)
                {
                    if (item is ToolStripButton otherBtn)
                    {
                        if (item == sender)
                        {
                            otherBtn.Checked = true;
                            otherBtn.BackColor = Color.FromArgb(255, 191, 221, 245);
                        }
                        else
                        {
                            otherBtn.Checked = false;
                            otherBtn.BackColor = System.Drawing.SystemColors.Control;
                        }
                    }
                }
            }
        }
    }
}
