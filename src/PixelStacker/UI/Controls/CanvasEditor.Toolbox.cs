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
            if (Options?.Tools != null)
            {
                Options.Tools.BrushWidth = tbxBrushWidth.Text.ToNullable<int>() ?? 1;
                Options.Save();
            }
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
            toolbox.OnClickFill += Toolbox_OnClickFill;
            toolbox.OnClickPanZoom += Toolbox_OnClickPanZoom;
            toolbox.OnClickPencil += Toolbox_OnClickPencil;
            toolbox.OnClickBrush += Toolbox_OnClickBrush;
            toolbox.OnClickPicker += Toolbox_OnClickPicker;
            toolbox.OnClickEraser += Toolbox_OnClickEraser;
            toolbox.OnClickWorldEditOrigin += Toolbox_OnClickWorldEditOrigin;
        }

        private void Toolbox_OnClickBrush(object sender, EventArgs e)
        {
            this.CurrentTool = new BrushTool(this);
            this.Cursor = this.CurrentTool.GetCursor();
        }

        private void Toolbox_OnClickPencil(object sender, EventArgs e)
        {
            this.CurrentTool = new PencilTool(this);
            this.Cursor = this.CurrentTool.GetCursor();
        }

        private void Toolbox_OnClickPicker(object sender, EventArgs e)
        {
            this.CurrentTool = new PickerTool(this);
            this.Cursor = this.CurrentTool.GetCursor();
        }

        private void Toolbox_OnClickFill(object sender, EventArgs e)
        {
            this.CurrentTool = new FillTool(this);
            this.Cursor = this.CurrentTool.GetCursor();
        }

        private void Toolbox_OnClickEraser(object sender, EventArgs e)
        {
            this.CurrentTool = new EraserTool(this);
            this.Cursor = this.CurrentTool.GetCursor();
        }

        private void Toolbox_OnClickWorldEditOrigin(object sender, EventArgs e)
        {
            this.CurrentTool = new WorldEditOriginTool(this);
            this.Cursor = this.CurrentTool.GetCursor();
        }

        private void Toolbox_OnClickPanZoom(object sender, EventArgs e)
        {
            this.CurrentTool = new PanZoomTool(this);
            this.Cursor = this.CurrentTool.GetCursor();
        }
    }
}
