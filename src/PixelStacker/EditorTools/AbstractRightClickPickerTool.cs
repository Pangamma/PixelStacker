using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public abstract class AbstractRightClickPickerTool: AbstractCanvasEditorTool
    {
        public AbstractRightClickPickerTool(CanvasEditor editor): base(editor)
        {
        }

        public abstract void OnLeftClick(MouseEventArgs e);

        public override void OnClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point loc = CanvasEditor.GetPointOnImage(e.Location, this.CanvasEditor.PanZoomSettings, EstimateProp.Floor);
                if (loc.X < 0 || loc.X > this.CanvasEditor.Canvas.Width - 1) return;
                if (loc.Y < 0 || loc.Y > this.CanvasEditor.Canvas.Height - 1) return;
                var cd = this.CanvasEditor.Canvas.CanvasData[loc.X, loc.Y];
                this.Options.Tools.PrimaryColor = cd;
                this.Options.Save();
            } 
            else if (e.Button == MouseButtons.Left)
            {
                this.OnLeftClick(e);
            }
        }
    }
}
