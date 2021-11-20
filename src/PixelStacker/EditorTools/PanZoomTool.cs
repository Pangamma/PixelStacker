using PixelStacker.Logic.IO.Config;
using PixelStacker.UI.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public class PanZoomTool : AbstractCanvasEditorTool
    {
        private Point initialDragPoint = new Point(0,0);
        private bool IsDragging;
        public override bool UsesBrushWidth => false;
        public override Cursor GetCursor() => CursorHelper.PanZoom.Value;

        public PanZoomTool(CanvasEditor editor) : base(editor)
        {
        }

        public override void OnClick(MouseEventArgs e)
        {
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            this.initialDragPoint = e.Location;
            this.CanvasEditor.PanZoomSettings.initialImageX = this.CanvasEditor.PanZoomSettings.imageX;
            this.CanvasEditor.PanZoomSettings.initialImageY = this.CanvasEditor.PanZoomSettings.imageY;
            this.IsDragging = true;
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
            this.IsDragging = false;
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (IsDragging)
            {
                Point point = e.Location;
                this.CanvasEditor.PanZoomSettings.imageX = this.CanvasEditor.PanZoomSettings.initialImageX - (this.initialDragPoint.X - point.X);
                this.CanvasEditor.PanZoomSettings.imageY = this.CanvasEditor.PanZoomSettings.initialImageY - (this.initialDragPoint.Y - point.Y);
                this.CanvasEditor.RepaintRequested = true;
            }
        }
    }
}
