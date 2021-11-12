using PixelStacker.EditorTools;
using PixelStacker.UI.Forms;
using System;

namespace PixelStacker.UI.Controls
{
    public partial class CanvasEditor
    {
        private AbstractCanvasEditorTool CurrentTool { get; set; }
        private PanZoomTool PanZoomTool { get; }

        private async void timerBufferedChangeQueue_Tick(object sender, System.EventArgs e)
        {
            if (this.Painter == null) return;
            await this.Painter.DoRenderFromHistoryBuffer();
            this.RepaintRequested = true;
        }

        public void SetCanvasToolboxEvents(CanvasTools toolbox)
        {
            toolbox.OnClickPanZoom += Toolbox_OnClickPanZoom;
            toolbox.OnClickWorldEditOrigin += Toolbox_OnClickWorldEditOrigin;
            toolbox.OnClickEraser += Toolbox_OnClickEraser;
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
