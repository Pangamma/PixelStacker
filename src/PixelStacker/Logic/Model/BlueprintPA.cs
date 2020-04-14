using PixelStacker.Logic.Extensions;
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
        private Bitmap Image { get; set; }
        public int[,] BlocksMap { get; private set; } // x, y
        public Point WorldEditOrigin { get; set; } = new Point(0, 0);

        /// <summary>
        /// Calculated during the compilation process. Optimized to use fewest layers possible.
        /// </summary>
        public int MaxDepth { get; set; }
        public int Width { get { return this.BlocksMap.GetLength(0); } }
        public int Height { get { return this.BlocksMap.GetLength(1); } }

        /// <summary>
        /// ONLY use this for mapping to schematics and in-world coordinates
        /// </summary>
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
            var airColor = Materials.Air.getAverageColor(isSide).ToArgb();
            using (var padlock = await AsyncDuplicateLock.Get.LockAsync(src))
            {
                void viewActionPerPixel(int x, int y, Color cc)
                {
                    int r = cc.R;
                    int b = cc.B;
                    int gg = cc.G;
                    if (cc.A < 30 && ((r == 255 && b == 255 && gg == 255) || (r == 0 && b == 0 && gg == 0)))
                    {
                        blocksTemp[x, y] = airColor;
                    }
                    else
                    {
                        Color? c = Materials.FindBestMatch(availableColors, cc);
                        int ccii = c?.ToArgb() ?? 0;
                        if (c != null)
                        {
                            if (Materials.ColorMap.TryGetValue(c.Value, out Material[] found))
                            {
                                int len = found.Length;
                                if (len > maxDepth) { maxDepth = len; }
                            }
                            else
                            {
                                blocksTemp[x, y] = airColor;
                            }
                        }
                        blocksTemp[x, y] = ccii;
                    }
                }

                src.ToViewStream(_worker, viewActionPerPixel);
                TaskManager.SafeReport(100, "Finished rendering the blueprint.");
            }

            Materials.BestMatchCache.Save();

            return new BlueprintPA()
            {
                BlocksMap = blocksTemp,
                Image = src,
                MaxDepth = maxDepth
            };
        }


        /// <summary>
        /// Only works for top view
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Material[] GetMaterialsAt(int x, int y)
        {
#if RELEASE
            // If you're failing in dev, fix it. Release builds should not be held up by this...
            if (x > this.BlocksMap.GetLength(0) || x < 0) { return new Material[] { Materials.Air }; }
            if (y > this.BlocksMap.GetLength(1) || y < 0) { return new Material[] { Materials.Air }; }
#endif
            int ci = this.BlocksMap[x, y];
            Color c = Color.FromArgb(ci);
            var mm = (Materials.ColorMap.TryGetValue(c, out Material[] found) ? found : null) ?? new Material[] { Materials.Air };
            return mm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="zDepth"></param>
        /// <param name="isIndexZeroReturnedIfOutOfBounds">If true, mm[0] will be returned when mm[1] is not available.</param>
        /// <returns></returns>
        public Material GetMaterialAt(int x, int y, int zDepth, bool isIndexZeroReturnedIfOutOfBounds)
        {
            int x2, y2 = 0;
            x2 = x;
            y2 = y;

#if RELEASE
            // If you're failing in dev, fix it. Release builds should not be held up by this...
            if (x2 > this.BlocksMap.GetLength(0) || x2 < 0) { return Materials.Air; }
            if (y2 > this.BlocksMap.GetLength(1) || y2 < 0) { return Materials.Air; }
#endif

            int ci = this.BlocksMap[x2, y2];
            Color c = Color.FromArgb(ci);
            if (Materials.ColorMap.TryGetValue(c, out Material[] mm))
            {
                if (!(zDepth < 0 || zDepth >= mm.Length))
                {
                    return mm[zDepth];
                }
                else if (isIndexZeroReturnedIfOutOfBounds && mm.Length > 0)
                {
                    return mm[0];
                }
            }

            return Materials.Air;
        }

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
                int result;

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

            public Material GetMaterialAt(bool isSideView, int x, int y, int z)
            {
                int x2, y2, z2;
                x2 = x;
                y2 = isSideView ? y : z;
                int idx;
                int ci = this.blueprint.BlocksMap[x2, y2];
                Color c = Color.FromArgb(ci);
                var mm = (Materials.ColorMap.ContainsKey(c) ? Materials.ColorMap[c] : null) ?? new Material[] { Materials.Air };

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
