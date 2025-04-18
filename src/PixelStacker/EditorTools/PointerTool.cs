using PixelStacker.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public class PointerTool : AbstractCanvasEditorTool
    {
        private Point initialDragPoint = new Point(0, 0);
        private bool IsDragging;
        public override bool UsesBrushWidth => false;
        public override Cursor GetCursor() => CursorHelper.Pointer.Value;

        public PointerTool(CanvasEditor editor) : base(editor)
        {
        }

        public static Point? LastRightClickedPointOnPanel { get; set; } = null;
        public static Point? LastRightClickedPointOnImage { get; set; } = null;

        public override void OnClick(MouseEventArgs e)
        {
            if (!e.Button.HasFlag(MouseButtons.Right))
            {
                return;
            }

            bool isShiftHeld = Control.ModifierKeys.HasFlag(Keys.Shift);
            bool isControlHeld = Control.ModifierKeys.HasFlag(Keys.Control);
            bool isShiftOrControlHeld = isShiftHeld || isControlHeld;

            var loc = CanvasEditor.GetPointOnImage(e.Location, CanvasEditor.PanZoomSettings, Logic.Model.EstimateProp.Floor);
            LastRightClickedPointOnImage = loc;
            LastRightClickedPointOnPanel = e.Location;

            var image = CanvasEditor.Canvas.CanvasData;

            if (!image.IsInRange(loc.X, loc.Y))
            {
                return;
            }

            var mc = image[loc.X, loc.Y];
            var contextMenu = CanvasEditor.PointerContextMenuStrip;
            var filterMaterialsMenu = contextMenu.Items.Find("filterMaterialsToolStripMenuItem", false).OfType<ToolStripMenuItem>().FirstOrDefault();
            SetupMaterialFilterContextMenuItems(isShiftOrControlHeld, mc, filterMaterialsMenu);
            CanvasEditor.PointerContextMenuStrip.Show(Cursor.Position);
        }

        private void SetupMaterialFilterContextMenuItems(bool isShiftOrControlHeld, MaterialCombination mc, ToolStripMenuItem filterMaterialsMenu)
        {
            filterMaterialsMenu.DropDownItems.Clear();
            Material[] mats = mc.IsMultiLayer ? [mc.Top, mc.Bottom] : [mc.Top];
            var filter = Options.ViewerSettings.VisibleMaterialsFilter;
            bool isFilterEmpty = filter.Count == 0 || (filter.Count == 1 && filter.First() == Materials.Air.PixelStackerID);

            foreach (var m in mats)
            {
                bool contained = filter.Contains(m.PixelStackerID);
                
                string label;
                if (contained)
                {
                    label = string.Format(Resources.Text.CanvasEditor_Hide_0, m.Label);
                } 
                else
                {
                    if (isFilterEmpty)
                    {
                        label = string.Format(Resources.Text.CanvasEditor_ShowOnly_0, m.Label);
                    } 
                    else
                    {
                        label = string.Format(Resources.Text.CanvasEditor_Show_0, m.Label);
                    }
                }


                if (isShiftOrControlHeld) label += " (Skip re-render)";
                Image img = m.GetImage(Options.IsSideView).SKBitmapToBitmap();

                if (contained)
                {
                    filterMaterialsMenu.DropDownItems.Add(new ToolStripMenuItem(label, img, async (object ttsender, EventArgs evtArgs) =>
                    {
                        Options.ViewerSettings.VisibleMaterialsFilter.Remove(m.PixelStackerID);
                        if (!isShiftOrControlHeld) await CanvasEditor.MainForm.TriggerCanvasEditorRerender();
                    }));
                } 
                else
                {
                    filterMaterialsMenu.DropDownItems.Add(new ToolStripMenuItem(label, img, async (object ttsender, EventArgs evtArgs) =>
                    {
                        Options.ViewerSettings.VisibleMaterialsFilter.Add(m.PixelStackerID);
                        if (!isShiftOrControlHeld) await CanvasEditor.MainForm.TriggerCanvasEditorRerender();
                    }));
                }
            }

            {
                string skipReRenderString = isShiftOrControlHeld ? " (Skip re-render)" : "";
                filterMaterialsMenu.DropDownItems.Add(new ToolStripMenuItem(Resources.Text.ShowAll + skipReRenderString, null, async (object ttsender, EventArgs evtArgs) =>
                {
                    Options.ViewerSettings.VisibleMaterialsFilter.Clear();
                    if (!isShiftOrControlHeld) await CanvasEditor.MainForm.TriggerCanvasEditorRerender();
                }));

                // Add just AIR so that only air shows up.
                filterMaterialsMenu.DropDownItems.Add(new ToolStripMenuItem(Resources.Text.HideAll + skipReRenderString, null, async (object ttsender, EventArgs evtArgs) =>
                {
                    Options.ViewerSettings.VisibleMaterialsFilter.Clear();
                    Options.ViewerSettings.VisibleMaterialsFilter.Add(Materials.Air.PixelStackerID);
                    if (!isShiftOrControlHeld) await CanvasEditor.MainForm.TriggerCanvasEditorRerender();
                }));
            }

            {
                string label = Options.ViewerSettings.ShowFilteredMaterials
                    ? global::PixelStacker.Resources.Text.CanvasEditor_DisableGhostImages
                    : global::PixelStacker.Resources.Text.CanvasEditor_EnableGhostImages;

                filterMaterialsMenu.DropDownItems.Add(new ToolStripMenuItem(label, null, async (object ttsender, EventArgs evtArgs) =>
                {
                    Options.ViewerSettings.ShowFilteredMaterials = !Options.ViewerSettings.ShowFilteredMaterials;
                    if (!isShiftOrControlHeld) await CanvasEditor.MainForm.TriggerCanvasEditorRerender();
                }));
            }
        }

        public override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)
            {
                this.initialDragPoint = e.Location;
                this.CanvasEditor.PanZoomSettings.initialImageX = this.CanvasEditor.PanZoomSettings.imageX;
                this.CanvasEditor.PanZoomSettings.initialImageY = this.CanvasEditor.PanZoomSettings.imageY;
                this.IsDragging = true;
            }
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
