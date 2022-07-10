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
            Bitmap pic = UIResources.paint_bucket_cur;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> Eraser = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.eraser_cur;
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
            Bitmap pic = UIResources.pencil_cur;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> Picker = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.eyedropper_cur;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> WorldEditOrigin = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.compass_tool_cur;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> Brush = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.paintbrush_cur;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });

        public static readonly Lazy<Cursor> ColorSuggester = new Lazy<Cursor>(() => {
            Bitmap pic = UIResources.color;
            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        });
    }
}
