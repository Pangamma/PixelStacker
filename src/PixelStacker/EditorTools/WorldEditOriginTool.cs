using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public class WorldEditOriginTool : AbstractCanvasEditorTool
    {
        public override bool UsesBrushWidth => false;
        public WorldEditOriginTool(CanvasEditor editor) : base(editor)
        {
        }

        public override void OnClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point loc = CanvasEditor.GetPointOnImage(e.Location, this.CanvasEditor.PanZoomSettings, EstimateProp.Floor);
                this.CanvasEditor.Canvas.WorldEditOrigin = new PxPoint(loc.X, loc.Y);
                this.CanvasEditor.Refresh();
            }
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
        }

        public override Cursor GetCursor() => CursorHelper.WorldEditOrigin.Value;
    }
}
