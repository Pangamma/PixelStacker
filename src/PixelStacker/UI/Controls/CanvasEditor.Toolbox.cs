using PixelStacker.EditorTools;
using PixelStacker.Logic.Extensions;
using PixelStacker.UI.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        private AbstractCanvasEditorTool CurrentTool { get; set; }
        private PanZoomTool PanZoomTool { get; set; }

        private void bgWorkerBufferedChangeQueue_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.Painter.HistoryBuffer.RenderCount > 0)
                e.Result = this.Painter.DoRenderFromHistoryBuffer();
        }

        private void BgWorkerBufferedChangeQueue_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.RepaintRequested = true;
        }

        private void timerBufferedChangeQueue_Tick(object sender, System.EventArgs e)
        {
            if (this.Painter == null) return;
            if (bgWorkerBufferedChangeQueue.IsBusy) return;
            bgWorkerBufferedChangeQueue.RunWorkerAsync();
            //if (this.Painter == null) return;

            //IsRenderingBuffer = true;
            //await this.Painter.DoRenderFromHistoryBuffer();
            //IsRenderingBuffer = false;
            //this.RepaintRequested = true;
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

        private void Toolbox_OnClickPanZoom(object sender, EventArgs e)
        {
            this.CurrentTool = new PanZoomTool(this);
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
                            //otherBtn.FlatAppearance.BorderColor = Color.FromArgb(255, 1, 121, 215);
                            //otherBtn.FlatAppearance.BorderSize = 1;
                        }
                        else
                        {
                            otherBtn.Checked = false;
                            otherBtn.BackColor = System.Drawing.SystemColors.Control;
                            //otherBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLightLight;
                            //otherBtn.FlatAppearance.BorderSize = 0;
                        }
                    }
                }
            }
        }
    }
}
