using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public class PickerTool : AbstractCanvasEditorTool
    {
        public override bool UsesBrushWidth => false;


        public PickerTool(CanvasEditor editor) : base(editor)
        {
        }

        public override void OnClick(MouseEventArgs e)
        {
            Point loc = CanvasEditor.GetPointOnImage(e.Location, this.CanvasEditor.PanZoomSettings, EstimateProp.Floor);
            if (loc.X < 0 || loc.X > this.CanvasEditor.Canvas.Width - 1) return;
            if (loc.Y < 0 || loc.Y > this.CanvasEditor.Canvas.Height - 1) return;
            var cd = this.CanvasEditor.Canvas.CanvasData[loc.X, loc.Y];
            this.Options.Tools.PrimaryColor = cd;
            this.Options.Save();
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

        public override Cursor GetCursor() => CursorHelper.Picker.Value;
    }
}
