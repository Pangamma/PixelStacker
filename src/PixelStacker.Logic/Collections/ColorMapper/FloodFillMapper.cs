using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using PixelStacker.Extensions;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    internal class FloodFillNode
    {
        public int R;
        public int G;
        public int B;
        public MaterialCombination Value;
        public int PaletteID;
        public FloodFillNode(int PaletteID, MaterialCombination mc, int r, int g, int b)
        {
            this.PaletteID = PaletteID;
            Value = mc;
            R = r;
            G = g;
            B = b;
        }

        public FloodFillNode(int PaletteID, MaterialCombination mc, SKColor c)
        {
            this.PaletteID = PaletteID;
            Value = mc;
            R = c.Red;
            G = c.Green;
            B = c.Blue;
        }

        public FloodFillNode Shift(int r, int g, int b)
        {
            return new FloodFillNode(this.PaletteID, this.Value, R + r, G + g, B + b);
        }
    }

    public class FloodFillMapper : IColorMapper
    {
        public bool IsSeeded() => Combos != null;

        // [R,G,B] => null || MaterialCombinationPaletteID
        private int?[,,] Cache = new int?[256,256,256];
        #region TRASH
        public List<MaterialCombination> Combos { get; private set; }
        public bool IsSideView { get; private set; }
        public MaterialPalette Palette { get; private set; }

        public double AccuracyRating => 100;
        public double SpeedRating => 120000;

        #endregion TRASH

        /*
        Preparing:
        1. Create a 256^3 array of block objects
        2. Create a queue of 3D point objects
        3. Populate seed cells of the array with block objects at the average position of each Block's texture. 
           If there is a conflict/overlap, prefer the block that is more uniform
        4. Add the coordinates of every seed cell to the queue (this can be done during #3's loop)
        ---------
        Flood-filling:
        1. Repeat all following steps until the queue is empty
        2. Pop a point from the queue and grab the block in the array at the location of the point
        3. Repeat 4...8 for all 6 axial directions
        4. Create a new point object based on the point from #2 but moved by 1 in the current direction
        5. If this point isn't in the bounds of the array or if the cell in the array at the location of the point from #4 is the same block as the block from #2, skip to the next direction
        6. If the cell in the array at the location of the point from #4 is null, set it to the block from #2 and add the point from #4 to the queue
        7. Otherwise, compare the distances between the block from #2 and the point from #4 and the currently occupying block and the point from #4
        8. If the block from #2 is closer, overwrite the cell at the point from #4 with the block and add the point to the queue
        */
        public void SetSeedData(List<MaterialCombination> combos, MaterialPalette mats, bool isSideView)
        {
            this.Cache = null;
            this.Cache = new int?[256, 256, 256];
            this.Combos = combos;
            this.IsSideView = isSideView;
            this.Palette = mats;
            Queue<FloodFillNode> queue = new Queue<FloodFillNode>();

            foreach(var mc in combos)
            {
                var avg = mc.GetAverageColor(isSideView);
                var node = new FloodFillNode(Palette[mc], mc, avg);
                queue.Enqueue(node);
            }

            while(queue.TryDequeue(out FloodFillNode node))
            {
                if (TrySet(node))
                {
                    EnqueueAdjacent(queue, node);
                }
            }
        }

        private void EnqueueAdjacent(Queue<FloodFillNode> queue, FloodFillNode node)
        {
            if (node.R < 255) queue.Enqueue(node.Shift(1, 0, 0));
            if (node.R > 0) queue.Enqueue(node.Shift(-1, 0, 0));
            if (node.G < 255) queue.Enqueue(node.Shift(0, 1, 0));
            if (node.G > 0) queue.Enqueue(node.Shift(0, -1, 0));
            if (node.B < 255) queue.Enqueue(node.Shift(0, 0, 1));
            if (node.B > 0) queue.Enqueue(node.Shift(0, 0, -1));
        }

        private bool TrySet(FloodFillNode node)
        {
            var existing = Cache[node.R, node.G, node.B];
            if (existing == null) { 
                Cache[node.R, node.G, node.B] = node.PaletteID;
                return true;
            }

            // Battle skipped. We are the same.
            if (existing.Value == node.PaletteID)
                return false;

            // BATTLE TIME
            SKColor c = new SKColor((byte)node.R, (byte)node.G, (byte)node.B, (byte) 255);
            long distExisting = c.GetAverageColorDistance(Palette[existing.Value].GetColorsInImage(this.IsSideView));
            long distNew = c.GetAverageColorDistance(node.Value.GetColorsInImage(this.IsSideView));
            if (distNew >= distExisting) return false;
            Cache[node.R, node.G, node.B] = node.PaletteID;
            return true;
        }


        #region TRASH2

        public MaterialCombination FindBestMatch(SKColor c)
        {
            if (c.Alpha < 32) return Palette[Constants.MaterialCombinationIDForAir]; // AIR

            int? val = Cache[c.Red, c.Green, c.Blue];
            if (val != null)
                return Palette[val.Value];

#if DEBUG
            throw new Exception("Impossible! Did you forget to set the seed data?");
#else
            var foundMc = Combos.MinBy(x => c.GetAverageColorDistance(x.GetColorsInImage(this.IsSideView)));
            SKColor found = foundMc.GetAverageColor(this.IsSideView);
            Cache[found.Red, found.Green, found.Blue] = Palette[foundMc];
            return foundMc;
#endif
        }

        /// <summary>
        /// Super SLOW! Avoid if you can help it. Not at all optimized.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="maxMatches"></param>
        /// <returns></returns>
        public List<MaterialCombination> FindBestMatches(SKColor c, int maxMatches)
        {
            var found = Combos.OrderBy(x => c.GetAverageColorDistance(x.GetColorsInImage(this.IsSideView)))
                .Take(maxMatches).ToList();

            return found;
        }
        #endregion TRASH2
    }
}
