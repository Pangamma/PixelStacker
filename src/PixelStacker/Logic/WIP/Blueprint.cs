using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public class Blueprint
    {
        public Bitmap SourceImage { get; set; }
        public Point WorldEditOrigin { get; set; } = new Point(0, 0);
        public BlockMap Map { get; set; }


        public Color GetColor(int x, int y)
        {
            if (x < this.Map.BlocksMap.GetLength(0) && y < this.Map.BlocksMap.GetLength(1) && x > -1 && y > -1)
            {
                Color cc = this.Map.BlocksMap[x, y];
                return cc;
            }
            return Color.Transparent;
        }
    }

    public class BlockMap
    {
        public Color[,] BlocksMap { get; set; } // x, y
        public int MaxDepth { get; set; }
    }
}
