using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using PixelStacker.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public enum PxMouseButton
    {
        Left, Middle, Right
    }

    public class PxClickEvent
    {
        public PxPoint Location { get; set; }
        public PxMouseButton Button { get; set; }
        public bool IsShiftHeld { get; set; } = false;
        public bool IsControlHeld { get; set; } = false;
        public bool IsAltHeld { get; set; } = false;
    }

    public abstract class AbstractCanvasEditorTool<TSettings> : AbstractCanvasEditorTool where TSettings : class
    {
        public TSettings Settings { get; set; }
        protected AbstractCanvasEditorTool(CanvasEditor editor) : base(editor)
        {
        }
    }

    public abstract class AbstractCanvasEditorTool
    {
        public abstract bool UsesBrushWidth { get; }
        public int BrushWidth => Options.Tools?.BrushWidth ?? 1;
        protected MaterialPalette Palette => this.CanvasEditor.Canvas.MaterialPalette ?? MaterialPalette.FromResx();
        protected CanvasEditor CanvasEditor { get; }
        public Options Options { get; }

        public AbstractCanvasEditorTool(CanvasEditor editor)
        {
            this.CanvasEditor = editor;
            this.Options = editor.Options;
        }

        public abstract Cursor GetCursor();

        public abstract void OnClick(MouseEventArgs e);
        public abstract void OnMouseDown(MouseEventArgs e);
        public abstract void OnMouseUp(MouseEventArgs e);
        public abstract void OnMouseMove(MouseEventArgs e);


        private PxPoint GetPointOnImage(PxPoint pointOnPanel, PanZoomSettings pz, EstimateProp prop)
        {
            if (prop == EstimateProp.Ceil)
            {
                return new PxPoint((int)Math.Ceiling((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Ceiling((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
            }
            if (prop == EstimateProp.Floor)
            {
                return new PxPoint((int)Math.Floor((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Floor((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
            }
            return new PxPoint((int)Math.Round((pointOnPanel.X - pz.imageX) / pz.zoomLevel), (int)Math.Round((pointOnPanel.Y - pz.imageY) / pz.zoomLevel));
        }

        public static PxPoint GetPointOnPanel(PxPoint pointOnImage, PanZoomSettings pz)
        {
            if (pz == null)
            {
#if DEBUG
                throw new ArgumentNullException("PanZoomSettings are not set. So weird!");
#else
                return new PxPoint(0, 0);
#endif
            }

            return new PxPoint((int)Math.Round(pointOnImage.X * pz.zoomLevel + pz.imageX), (int)Math.Round(pointOnImage.Y * pz.zoomLevel + pz.imageY));
        }


        /// <summary>
        /// Can very easily return null. Be ready for it.
        /// </summary>
        /// <param name="layerFilter"></param>
        /// <param name="palette"></param>
        /// <param name="primaryColor"></param>
        /// <param name="toReplace"></param>
        /// <returns></returns>
        protected MaterialCombination GetMcToPaintWith(ZLayer layerFilter, MaterialPalette palette, MaterialCombination primaryColor, MaterialCombination toReplace)
        {
            if (layerFilter == ZLayer.Both)
            {
                return primaryColor;
            }

            Material pTop = toReplace.Top;
            Material pBottom = toReplace.Bottom;

            if (layerFilter == ZLayer.Top)
            {
                pTop = primaryColor.Top;
            }
            else if (layerFilter == ZLayer.Bottom)
            {
                pBottom = primaryColor.Bottom;
            }

            // When mode is on bottom and AIR:
            // GLASS on top? DELETED?? Weird!
            // Solid on bottom? Nothing! (WEIRD!)
            // Expected: Do nothing.
            if (layerFilter == ZLayer.Bottom && primaryColor.Bottom.PixelStackerID == "AIR")
            {
                return toReplace;
            }

            // If solid covers transparent, fix it so it is full solid.
            // Come to think of it, this should pretty much NEVER happen unless
            // working with AIR or pure glass. AIR is more likely.
            if (pBottom.CanCoverOtherBlocks && !pTop.CanCoverOtherBlocks)
            {
                pBottom = pTop;
            }

            // If it is "air" on top, then really... it should be whatever is below.
            if (pTop.PixelStackerID == "AIR" && pBottom.PixelStackerID != "AIR")
            {
                pTop = pBottom;
            }

            // If it is "air" on bottom, then really... it should be whatever is up top.
            if (pTop.PixelStackerID != "AIR" && pBottom.PixelStackerID == "AIR")
            {
                pBottom = pTop;
            }

            // What if both is AIR? Leave it alone. That's fine.

            // If both glass: our color palette doesn't currently support pure glass.
            // Should it?
            bool areBothGlass = pBottom.Category == "Glass" && pTop.Category == "Glass";
            if (areBothGlass)
            {
                pTop = Materials.Air;
                pBottom = Materials.Air;
            }

            // two different shades of glass ontop of each other? Yikes. 
            // Allow the preferred mode to override the other one.
            bool areBothSolid = pBottom.CanBeOnBottom && pTop.CanBeOnBottom;
            if (areBothSolid && pTop.PixelStackerID != pBottom.PixelStackerID)
            {
                if (layerFilter == ZLayer.Top)
                {
                    pBottom = pTop;
                }
                else
                {
                    pTop = pBottom;
                }
            }

            var result = palette.GetMaterialCombinationByMaterials(pBottom, pTop);

            if (result == null)
            {
#if DEBUG
                throw new Exception("Oh no! An invalid color was attempted.");
#else
                return primaryColor;
#endif
            }

            return result;
        }


        protected List<PxPoint> SquareExpansion(PxPoint seed, int brushWidth)
        {
            List<PxPoint> points = new List<PxPoint>();
            int xMin = seed.X - brushWidth / 2;
            int xMax = seed.X - brushWidth / 2 + brushWidth; // helps with rounding issues to do it this way. 
            int yMin = seed.Y - brushWidth / 2;
            int yMax = seed.Y - brushWidth / 2 + brushWidth;
            for (int x = xMin; x < xMax; x++)
            {
                for (int y = yMin; y < yMax; y++)
                {
                    points.Add(new PxPoint(x, y));
                }
            }

            return points;
        }

        /// <summary>
        /// Returns list of points in between from and to, excluding from and to.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected List<PxPoint> SquareExpansion(List<PxPoint> seeds, int brushWidth)
        {
            HashSet<PxPoint> points = new HashSet<PxPoint>();
            foreach (var seed in seeds)
            {
                int xMin = seed.X - brushWidth / 2;
                int xMax = seed.X - brushWidth / 2 + brushWidth; // helps with rounding issues to do it this way. 
                int yMin = seed.Y - brushWidth / 2;
                int yMax = seed.Y - brushWidth / 2 + brushWidth;
                for (int x = xMin; x < xMax; x++)
                {
                    for (int y = yMin; y < yMax; y++)
                    {
                        points.Add(new PxPoint(x, y));
                    }
                }
            }

            return points.ToList();
        }

        /// <summary>
        /// Returns list of points in between from and to, excluding from and to.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        protected static List<PxPoint> GetPointsBetween(PxPoint start, PxPoint end)
        {
            List<PxPoint> points = new List<PxPoint>();
            if (start == null || end == null) return points;
            if (start.Equals(end)) return points;

            int x = start.X;
            int y = start.Y;
            int x2 = end.X;
            int y2 = end.Y;

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                points.Add(new PxPoint(x, y));
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }

            return points;
        }
    }
}
