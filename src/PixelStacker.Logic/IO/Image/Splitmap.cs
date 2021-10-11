using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    /// <summary>
    /// Stitch a series of 640x640 bitmaps together to make a giant bitmap tile set that renders quickly.
    /// </summary>
    public class Splitmap
    {
        List<Bitmap[,]> Bitmaps = new List<Bitmap[,]>();
        public Splitmap(int w, int h)
        {
            this.Width = w;
            this.Height = h;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
