using PixelStacker.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelStacker.EditorTools
{
    public class CursorHelper
    {
        public static readonly Lazy<Cursor> Fill = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.paint_bucket;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> Eraser = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.eraser;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> PanZoom = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.all_directions;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> Pencil = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.pencil_1;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> Picker = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.eyedropper;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> WorldEditOrigin = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.compass_tool;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> Brush = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.paintbrush;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });
        //private Cursor crossCursor(Pen pen, Brush brush, string name, int x, int y)
        //{
        //    var pic = new Bitmap(x, y);
        //    Graphics gr = Graphics.FromImage(pic);

        //    var pathX = new GraphicsPath();
        //    var pathY = new GraphicsPath();
        //    pathX.AddLine(0, y / 2, x, y / 2);
        //    pathY.AddLine(x / 2, 0, x / 2, y);
        //    gr.DrawPath(pen, pathX);
        //    gr.DrawPath(pen, pathY);
        //    gr.DrawString(name, Font, brush, x / 2 + 5, y - 35);

        //    IntPtr ptr = pic.GetHicon();
        //    var c = new Cursor(ptr);
        //    return c;
        //}
    }
}
