using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public class PencilTool : AbstractRightClickPickerTool
    {
        private MaterialCombination Air { get; }

        public override Cursor GetCursor() => CursorHelper.Pencil.Value;
        public override bool UsesBrushWidth => false;

        PxPoint prevMovePoint = null;

        public PencilTool(CanvasEditor editor) : base(editor)
        {
            var palette = editor.Canvas?.MaterialPalette ?? MaterialPalette.FromResx();
            this.Air = palette[Constants.MaterialCombinationIDForAir];
        }

        public override void OnLeftClick(MouseEventArgs e)
        {
        }

        private bool IsDragging = false;

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            IsDragging = true;
            Point loc = CanvasEditor.GetPointOnImage(e.Location, this.CanvasEditor.PanZoomSettings, EstimateProp.Floor);
            prevMovePoint = new PxPoint(loc.X, loc.Y);
            if (loc.X < 0 || loc.X > this.CanvasEditor.Canvas.Width - 1) return;
            if (loc.Y < 0 || loc.Y > this.CanvasEditor.Canvas.Height - 1) return;
            var cd = this.CanvasEditor.Canvas.CanvasData[loc.X, loc.Y];
            var painter = this.CanvasEditor.Painter;
            var colorToUseTmp = Options.Tools.PrimaryColor ?? this.Air;
            var colorToUse = MaterialCombination.GetMcToPaintWith(this.Options.Tools.ZLayerFilter, Palette, colorToUseTmp, cd);
            painter.History.AppendToVisualBuffer(Palette[cd], Palette[colorToUse], new PxPoint(loc.X, loc.Y));
        }

        public override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            IsDragging = false;
            this.CanvasEditor.Painter.History.FlushHistoryBufferAndFlushVisualBufferThenRenderIt();
            this.CanvasEditor.RepaintRequested = true;
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (!IsDragging) return;

            Point loc = CanvasEditor.GetPointOnImage(e.Location, this.CanvasEditor.PanZoomSettings, EstimateProp.Floor);
            var pointsToAdd = GetPointsBetween(prevMovePoint, new PxPoint(loc.X, loc.Y));
            prevMovePoint = new PxPoint(loc.X, loc.Y);

            if (!this.CanvasEditor.Canvas.CanvasData.IsInRange(loc.X, loc.Y)) return;
            var painter = this.CanvasEditor.Painter;
            var colorToUseTmp = Options.Tools.PrimaryColor ?? this.Air;

            {
                var cd = this.CanvasEditor.Canvas.CanvasData[loc.X, loc.Y];
                var colorToUse = MaterialCombination.GetMcToPaintWith(this.Options.Tools.ZLayerFilter, Palette, colorToUseTmp, cd);
                painter.History.AppendToVisualBuffer(Palette[cd], Palette[colorToUse], new PxPoint(loc.X, loc.Y));
            }

            foreach (var p in pointsToAdd)
            {
                if (!this.CanvasEditor.Canvas.CanvasData.IsInRange(p.X, p.Y)) continue;
                var cd = this.CanvasEditor.Canvas.CanvasData[p.X, p.Y];
                var colorToUse = MaterialCombination.GetMcToPaintWith(this.Options.Tools.ZLayerFilter, Palette, colorToUseTmp, cd);
                painter.History.AppendToVisualBuffer(Palette[cd], Palette[colorToUse], p);
            }
        }
    }
}
