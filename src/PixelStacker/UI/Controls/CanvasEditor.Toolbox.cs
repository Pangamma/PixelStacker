using PixelStacker.EditorTools;
using PixelStacker.Logic.Extensions;
using PixelStacker.UI.Forms;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        private AbstractCanvasEditorTool CurrentTool { get; set; }
        private PanZoomTool PanZoomTool { get; }

        private void tbxBrushWidth_TextChanged(object sender, System.EventArgs e)
        {
            Options.Tools.BrushWidth = tbxBrushWidth.Text.ToNullable<int>() ?? 1;
        }

        private void tbxBrushWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void bgWorkerBufferedChangeQueue_DoWork(object sender, DoWorkEventArgs e)
        {
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

        public void SetCanvasToolboxEvents(CanvasTools toolbox)
        {
            toolbox.OnClickPanZoom += Toolbox_OnClickPanZoom;
            toolbox.OnClickWorldEditOrigin += Toolbox_OnClickWorldEditOrigin;
            toolbox.OnClickEraser += Toolbox_OnClickEraser;
            toolbox.OnClickFill += Toolbox_OnClickFill;
        }

        private void Toolbox_OnClickFill(object sender, EventArgs e)
        {
            this.CurrentTool = new FillTool(this);
        }

        private void Toolbox_OnClickEraser(object sender, EventArgs e)
        {
            this.CurrentTool = new EraserTool(this);
        }

        private void Toolbox_OnClickWorldEditOrigin(object sender, EventArgs e)
        {
            this.CurrentTool = new WorldEditOriginTool(this);
        }

        private void Toolbox_OnClickPanZoom(object sender, EventArgs e)
        {
            this.CurrentTool = new PanZoomTool(this);
        }
    }
}
