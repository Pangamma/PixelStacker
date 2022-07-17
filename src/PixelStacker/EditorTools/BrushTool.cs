using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public class BrushTool : AbstractRightClickPickerTool
    {
        private MaterialCombination Air { get; }
        public override Cursor GetCursor() => CursorHelper.Brush.Value;

        public override bool UsesBrushWidth => true;

        PxPoint prevMovePoint = null;

        public BrushTool(CanvasEditor editor) : base(editor)
        {
            var palette = editor.Canvas.MaterialPalette ?? MaterialPalette.FromResx();
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
            var painter = this.CanvasEditor.Painter;

            // Expand to square shape then add relevant points.
            var colorToUseTmp = Options.Tools.PrimaryColor ?? this.Air;
            var pnts = this.SquareExpansion(new PxPoint(loc.X, loc.Y), this.BrushWidth);
            foreach (var pnt in pnts)
            {
                if (pnt.X < 0 || pnt.X > this.CanvasEditor.Canvas.Width - 1) continue;
                if (pnt.Y < 0 || pnt.Y > this.CanvasEditor.Canvas.Height - 1) continue;
                var cd = this.CanvasEditor.Canvas.CanvasData[pnt.X, pnt.Y];
                var colorToUse = MaterialCombination.GetMcToPaintWith(this.Options.Tools.ZLayerFilter, base.Palette, colorToUseTmp, cd);
                painter.History.AppendToVisualBuffer(Palette[cd], Palette[colorToUse], new PxPoint(pnt.X, pnt.Y));
            }
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

            var painter = this.CanvasEditor.Painter;

            pointsToAdd.Add(new PxPoint(loc.X, loc.Y));
            pointsToAdd = this.SquareExpansion(pointsToAdd, this.BrushWidth);
            var colorToUseTmp = Options.Tools.PrimaryColor ?? this.Air;

            foreach (var p in pointsToAdd)
            {
                if (!this.CanvasEditor.Canvas.IsInRange(p.X, p.Y)) continue;
                var cd = this.CanvasEditor.Canvas.CanvasData[p.X, p.Y];
                var colorToUse = MaterialCombination.GetMcToPaintWith(this.Options.Tools.ZLayerFilter, base.Palette, colorToUseTmp, cd);
                painter.History.AppendToVisualBuffer(Palette[cd], Palette[colorToUse], p);
            }
        }
    }
}
