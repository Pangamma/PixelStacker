using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public class BrushTool : AbstractCanvasEditorTool
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

        public override void OnClick(MouseEventArgs e)
        {
            Point loc = CanvasEditor.GetPointOnImage(e.Location, this.CanvasEditor.PanZoomSettings, EstimateProp.Floor);
            if (e.Button == MouseButtons.Right)
            {
                if (loc.X < 0 || loc.X > this.CanvasEditor.Canvas.Width - 1) return;
                if (loc.Y < 0 || loc.Y > this.CanvasEditor.Canvas.Height - 1) return;
                var cd = this.CanvasEditor.Canvas.CanvasData[loc.X, loc.Y];
                this.Options.Tools.PrimaryColor = cd;
                return;
            }
            else if (e.Button == MouseButtons.Left)
            {
                var painter = this.CanvasEditor.Painter;
                var buffer = painter.HistoryBuffer;
                var colorToUse = Options.Tools.PrimaryColor;
                var pnts = this.SquareExpansion(new PxPoint(loc.X, loc.Y), this.BrushWidth);
                foreach (var pnt in pnts)
                {
                    if (pnt.X < 0 || pnt.X > this.CanvasEditor.Canvas.Width - 1) continue;
                    if (pnt.Y < 0 || pnt.Y > this.CanvasEditor.Canvas.Height - 1) continue;
                    var cd = this.CanvasEditor.Canvas.CanvasData[pnt.X, pnt.Y];
                    buffer.AppendChange(Palette[cd], Palette[colorToUse], pnt);
                }
            }

        }

        private bool IsDragging = false;

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            IsDragging = true;

            Point loc = CanvasEditor.GetPointOnImage(e.Location, this.CanvasEditor.PanZoomSettings, EstimateProp.Floor);
            prevMovePoint = new PxPoint(loc.X, loc.Y);
            var painter = this.CanvasEditor.Painter;
            var buffer = painter.HistoryBuffer;

            // Expand to square shape then add relevant points.
            var colorToUse = Options.Tools.PrimaryColor ?? this.Air;
            var pnts = this.SquareExpansion(new PxPoint(loc.X, loc.Y), this.BrushWidth);
            foreach (var pnt in pnts)
            {
                if (pnt.X < 0 || pnt.X > this.CanvasEditor.Canvas.Width - 1) continue;
                if (pnt.Y < 0 || pnt.Y > this.CanvasEditor.Canvas.Height - 1) continue;
                var cd = this.CanvasEditor.Canvas.CanvasData[pnt.X, pnt.Y];
                buffer.AppendChange(Palette[cd], Palette[colorToUse], new PxPoint(pnt.X, pnt.Y));
            }
        }
        public override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            IsDragging = false;

            var record = this.CanvasEditor.Painter.HistoryBuffer.ToHistoryRecord(true);
            this.CanvasEditor.Painter.History.AddChange(record);
            this.CanvasEditor.RepaintRequested = true;
            // TODO: Send it to the history buffer

        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (!IsDragging) return;

            Point loc = CanvasEditor.GetPointOnImage(e.Location, this.CanvasEditor.PanZoomSettings, EstimateProp.Floor);
            var pointsToAdd = GetPointsBetween(prevMovePoint, new PxPoint(loc.X, loc.Y));
            prevMovePoint = new PxPoint(loc.X, loc.Y);

            var painter = this.CanvasEditor.Painter;
            var buffer = painter.HistoryBuffer;

            pointsToAdd.Add(new PxPoint(loc.X, loc.Y));
            pointsToAdd = this.SquareExpansion(pointsToAdd, this.BrushWidth);
            var colorToUse = Options.Tools.PrimaryColor ?? this.Air;

            foreach (var p in pointsToAdd)
            {
                if (!this.CanvasEditor.Canvas.IsInRange(p.X, p.Y)) continue;
                var cd = this.CanvasEditor.Canvas.CanvasData[p.X, p.Y];
                buffer.AppendChange(Palette[cd], Palette[colorToUse], p);
            }
        }
    }
}
