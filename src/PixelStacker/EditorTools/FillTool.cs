using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.Logic.Utilities;
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
    public class FillTool : AbstractCanvasEditorTool
    {
        public override bool UsesBrushWidth => false;
        public FillTool(CanvasEditor editor) : base(editor)
        {
        }

        static void TryEnqueue(Queue<CanvasIteratorData> queue, int x, int y, CanvasData cd, MaterialPalette palette, int colorToRemove, HashSet<PxPoint> visited)
        {
            if (!cd.IsInRange(x, y)) return;
            var p = new PxPoint(x, y);
            if (visited.Contains(p)) return;
            visited.Add(p);
            MaterialCombination mc = cd[x, y];
            int curColor = palette[mc];
            if (curColor != colorToRemove) return;
            queue.Enqueue(new CanvasIteratorData()
            {
                X = x,
                Y = y,
                PaletteID = curColor
            });
        }

        public override void OnClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point loc = CanvasEditor.GetPointOnImage(e.Location, this.CanvasEditor.PanZoomSettings, EstimateProp.Floor);
                if (loc.X < 0 || loc.X > this.CanvasEditor.Canvas.Width - 1) return;
                if (loc.Y < 0 || loc.Y > this.CanvasEditor.Canvas.Height - 1) return;
                var cd = this.CanvasEditor.Canvas.CanvasData[loc.X, loc.Y];
                this.Options.Tools.PrimaryColor = cd;
                return;
            }

            Task.Run(() => TaskManager.Get.StartAsync((worker) =>
            {
                Point loc = CanvasEditor.GetPointOnImage(e.Location, this.CanvasEditor.PanZoomSettings, Logic.Model.EstimateProp.Floor);
                if (!this.CanvasEditor.Canvas.IsInRange(loc.X, loc.Y)) return;
                if (MouseButtons.Middle == e.Button) return; // Impossible, but may as well handle it.
                bool isShiftHeld = Control.ModifierKeys == Keys.Shift;

                var cd = this.CanvasEditor.Canvas.CanvasData;
                var painter = this.CanvasEditor.Painter;

                MaterialCombination mc = Options.Tools.PrimaryColor;
                mc ??= Palette[Constants.MaterialCombinationIDForAir];

                int colorToSet = Palette[mc];
                int colorToRemove = Palette[cd[loc.X, loc.Y]];
                if (colorToSet == colorToRemove) return;


                if (!isShiftHeld)
                {
                    Queue<CanvasIteratorData> queue = new Queue<CanvasIteratorData>();
                    queue.Enqueue(new CanvasIteratorData()
                    {
                        X = loc.X,
                        Y = loc.Y,
                        PaletteID = colorToRemove
                    });

                    HashSet<PxPoint> visited = new HashSet<PxPoint>();

                    while (queue.TryDequeue(out CanvasIteratorData data))
                    {
                        if (data.PaletteID != colorToRemove) continue;

                        painter.HistoryBuffer.AppendChange(colorToRemove, colorToSet, new PxPoint(data.X, data.Y));

                        TryEnqueue(queue, data.X + 1, data.Y, cd, Palette, colorToRemove, visited);
                        TryEnqueue(queue, data.X - 1, data.Y, cd, Palette, colorToRemove, visited);
                        TryEnqueue(queue, data.X, data.Y + 1, cd, Palette, colorToRemove, visited);
                        TryEnqueue(queue, data.X, data.Y - 1, cd, Palette, colorToRemove, visited);
                    }
                }
                else
                {
                    foreach (var cdi in cd)
                    {
                        if (cdi.PaletteID == colorToRemove)
                        {
                            painter.HistoryBuffer.AppendChange(colorToRemove, colorToSet, new PxPoint(cdi.X, cdi.Y));
                        }
                    }
                }

                var ri = painter.HistoryBuffer.ToRenderInstructions(true);
                this.CanvasEditor.Painter.DoProcessRenderRecords(ri);

                var hr = painter.HistoryBuffer.ToHistoryRecord(true);
                this.CanvasEditor.Painter.History.AddChange(hr);
            }));
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

        public override Cursor GetCursor()
        {
            return CursorHelper.Fill.Value;
        }
    }
}
