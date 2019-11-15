using PixelStacker.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public class BlueprintPA
    {
        /// <summary>
        /// Calculated during the compilation process. Optimized to use fewest layers possible.
        /// </summary>
        public int MaxDepth { get; set; }
        private Bitmap Image { get; set; }
        public int[,] BlocksMap { get; private set; } // x, y
        public Point WorldEditOrigin { get; set; } = new Point(0, 0);
        public CoordinateMapper Mapper { get; set; }

        public BlueprintPA()
        {
            this.Mapper = new CoordinateMapper(this);
        }

        public static async Task<BlueprintPA> GetBluePrintAsync(CancellationToken _worker, Bitmap src)
        {
            int maxDepth = 0;
            bool isMultiLayer = Options.Get.IsMultiLayer;
            bool isSide = Options.Get.IsSideView;
            if (Materials.ColorMap.Count == 0)
            {
                TaskManager.SafeReport(0, "Compiling the color map");
                Materials.CompileColorMap(_worker, false);
            }
            TaskManager.SafeReport(100, "Colormap is compiled");
            _worker.SafeThrowIfCancellationRequested();

            int[,] blocksTemp = new int[src.Width, src.Height];

            List<Color> availableColors = Materials.ColorMap.Keys.ToList();
            TaskManager.SafeReport(0, "Rendering the blueprint...");
            using (var padlock = await AsyncDuplicateLock.Get.LockAsync(src))
            {
                src.ToViewStream(_worker, (x, y, cc) =>
                {
                    int r = cc.R;
                    int b = cc.B;
                    int gg = cc.G;
                    if (((r == 255 && b == 255 && gg == 255) || (r == 0 && b == 0 && gg == 0)) && cc.A < 30)
                    {
                        var mi = Materials.Air.getAverageColor(isSide).ToArgb();
                        blocksTemp[x, y] = mi;
                    }
                    else
                    {
                        Color? c = Materials.FindBestMatch(availableColors, cc);
                        int ccii = c?.ToArgb() ?? 0;
                        if (c != null)
                        {
                            int len = Materials.ColorMap[c.Value].Length;
                            if (len > maxDepth) { maxDepth = len; }
                        }
                        blocksTemp[x, y] = ccii;
                    }

                    return cc;
                });
            }

            Materials.BestMatchCache.Save();

            return new BlueprintPA()
            {
                BlocksMap = blocksTemp,
                Image = src,
                MaxDepth = maxDepth
            };
        }

        public int Width { get { return this.BlocksMap.GetLength(0); } }
        public int Height { get { return this.BlocksMap.GetLength(1); } }

        public Color GetColor(int x, int y)
        {
            if (x < this.BlocksMap.GetLength(0) && y < this.BlocksMap.GetLength(1) && x > -1 && y > -1)
            {
                int cc = this.BlocksMap[x, y];
                return Color.FromArgb(cc);
            }
            return Color.Transparent;
        }


        public class CoordinateMapper
        {
            private BlueprintPA blueprint;

            public CoordinateMapper(BlueprintPA blueprintPA)
            {
                this.blueprint = blueprintPA;
            }

            public int GetXLength(bool isSideView)
            {
                var result = blueprint.BlocksMap.GetLength(0);
                return result;
            }

            public int GetYLength(bool isSideView)
            {
                var result = 0;

                if (isSideView)
                {
                    result = blueprint.BlocksMap.GetLength(1);
                }
                else
                {
                    result = blueprint.MaxDepth;
                }

                return result;
            }

            public int GetZLength(bool isSideView)
            {
                var result = 0;

                if (isSideView)
                {
                    // 1 = 1
                    // 2 = 3
                    // 3 = 5
                    int z = blueprint.MaxDepth;
                    result = (z * 2) - 1;
                }
                else
                {
                    result = blueprint.BlocksMap.GetLength(1);
                }

                return result;
            }

            /// <summary>
            /// Only works for side view.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public Material[] GetMaterialsAt(int x, int y)
            {
                int x2, y2 = 0;
                x2 = x;
                y2 = y;

                int ci = this.blueprint.BlocksMap[x2, y2];
                Color c = Color.FromArgb(ci);
                var mm = (Materials.ColorMap.ContainsKey(c) ? Materials.ColorMap[c] : null) ?? new Material[] { Materials.Air };
                return mm;
            }

            public Material GetMaterialAt(bool isSideView, int x, int y, int z)
            {
                int x2, y2, z2 = 0;
                x2 = x;
                y2 = isSideView ? y : z;

                int ci = this.blueprint.BlocksMap[x2, y2];
                Color c = Color.FromArgb(ci);
                var mm = (Materials.ColorMap.ContainsKey(c) ? Materials.ColorMap[c] : null) ?? new Material[] { Materials.Air };

                int idx = 0;
                if (isSideView)
                { 
                    // sizeActual = sizeVirtual (zMax)
                    // 1 = 1
                    // 2 = 3
                    // 3 = 5
                    int zMax = (blueprint.MaxDepth * 2) - 1; //calculates virtual max

                    // idxVirtual = idxActual [zMax = 3]
                    // 0 => 1
                    // 1 => 0
                    // 2 => 0
                    //z2 = Math.Abs(z - zMax / 2);

                    z2 = Math.Abs(z - (zMax / 2));
                    idx = Math.Max(0, Math.Min(z2, mm.Length - 1));
                    // 0 = solids
                    // 1 = glass / empty

                }
                else
                {
                    //int zMax = blueprint.MaxDepth;
                    z2 = y;
                    z2 = Math.Abs((mm.Length - 1) - z2); // flip the blocks over on Y axis
                    idx = Math.Max(0, Math.Min(z2, mm.Length - 1));

                }

                Material m = mm[idx];
                return m;
            }
        }
    }
}
