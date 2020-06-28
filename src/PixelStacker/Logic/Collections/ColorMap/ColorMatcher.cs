using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.Great;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Collections
{

    // To function, this will require:
    // KD Tree for best match finding
    // Color palette list + materials map combo
    // Ability to get materials from a color

    public class ColorMatcher
    {
        private static ColorMatcher _self = null;
        public static ColorMatcher Get
        {
            get
            {
                if (ColorMatcher._self == null)
                {
                    ColorMatcher._self = new ColorMatcher();
                }
                return ColorMatcher._self;
            }
        }


        public BestMatchCacheMap BestMatchCache { get; private set; } = new BestMatchCacheMap().Load();
        private KDColorTree colorPalette = new KDColorTree();
        public Dictionary<Color, Material[]> ColorToMaterialMap { get; private set; } = new Dictionary<Color, Material[]>();

        public async Task CompileColorPalette(CancellationToken worker, bool isClearBestMatchCache, List<Material> materials)
        {
            bool isSide = Options.Get.IsSideView;
            bool isMultiLayer = Options.Get.IsMultiLayer;
            bool isMultiLayerRequired = Options.Get.IsMultiLayerRequired;
            bool isAllowGlassByItself = false && !materials.Any(x => x.IsEnabled && x.Category != "Glass" && x.Category != "Air");

            if (isClearBestMatchCache)
            {
                this.BestMatchCache.Clear();
                this.ColorToMaterialMap.Clear();
                this.colorPalette.Clear();
                this.colorPalette = new KDColorTree();
            }

            int n = 0;
            int maxN = materials.Count(x => x.IsEnabled && x.Category != "Glass");
            if (isMultiLayer) maxN *= materials.Count(x => x.IsEnabled && x.Category == "Glass"); if (isMultiLayer)
            {
                if (isMultiLayerRequired)
                {
                    maxN = maxN * materials.Count(x => x.IsEnabled && x.Category == "Glass");
                }
                else
                {

                    maxN += maxN * materials.Count(x => x.IsEnabled && x.Category == "Glass");
                }
            }

            maxN = (int) (maxN * 1.20); // 1 for base. Reserve 20% for remaining iterations.
            if (isMultiLayer) maxN *= materials.Count(x => x.IsEnabled && x.Category == "Glass");

            Dictionary<Color, Material[]> colorToMaterialMap = new Dictionary<Color, Material[]>();

            TaskManager.SafeReport(0, "Compiling Color Map based on selected materials.");

            var categoriesSelected = Materials.List.Where(m2 => m2.IsEnabled && m2.Category != "Air").Select(m2 => m2.Category).Distinct();

            var baseLayer = materials.Where(m2 => m2.IsEnabled && m2.Category != "Glass" && m2.Category != "Air").ToList();
            var parallelOptions = new ParallelOptions()
            {
                CancellationToken = worker,
                MaxDegreeOfParallelism = 4
            };

            #region Layer 1
            await Task.Run(() => Parallel.ForEach(baseLayer, parallelOptions, m =>
            {
                Color cAvg = m.getAverageColor(isSide);

                lock (colorToMaterialMap)
                {
                    colorToMaterialMap[cAvg] = new Material[1] { m };
                }

                Interlocked.Increment(ref n);
                if (n % 30 == 0)
                {
                    TaskManager.SafeReport(100 * n / maxN);
                    if (worker.SafeIsCancellationRequested())
                    {
                        lock (colorToMaterialMap)
                        {
                            colorToMaterialMap.Clear();

                        }

                        worker.SafeThrowIfCancellationRequested();
                    }
                }
            }));
            #endregion Layer 1

            #region Layer 2

            if (isMultiLayer)
            {
                Dictionary<Color, Material[]> toAdd = new Dictionary<Color, Material[]>();
                List<Material> glasses = Materials.List.Where(m2 => m2.Category == "Glass" && m2.IsEnabled).ToList();
                List<Material[]> layer1; lock (colorToMaterialMap) { layer1 = colorToMaterialMap.Values.Where(cm => cm.Length == 1).ToList(); }

                await Task.Run(() => Parallel.ForEach(layer1, parallelOptions, mArr =>
                {
                    Color mArrAvgColor = mArr[0].getAverageColor(isSide);

                    foreach (Material glassM in glasses)
                    {
                        Color combinedColor = OverlayColor(glassM.getAverageColor(isSide), mArrAvgColor);
                        Material[] matMap = new Material[mArr.Length + 1];

                        for (int i = 0; i < mArr.Length; i++)
                        {
                            matMap[i] = mArr[i];
                        }
                        matMap[matMap.Length - 1] = glassM;

                        lock (colorToMaterialMap)
                        {
                            // Either prefer single layer, or... allow replacing because multi layer is preferred.
                            if (isMultiLayerRequired || !colorToMaterialMap.ContainsKey(combinedColor))
                            {
                                lock (toAdd)
                                {
                                    toAdd[combinedColor] = matMap;
                                }
                            }
                        }

                        Interlocked.Increment(ref n);
                        if (n % 30 == 0)
                        {
                            TaskManager.SafeReport(100 * n / maxN);
                            if (worker.SafeIsCancellationRequested())
                            {
                                lock (colorToMaterialMap)
                                {
                                    colorToMaterialMap.Clear();
                                }

                                worker.SafeThrowIfCancellationRequested();
                            }
                        }
                    }
                }));

                lock (colorToMaterialMap)
                {
                    if (isMultiLayerRequired)
                    {
                        colorToMaterialMap.Clear();
                        toAdd.ToList().ForEach(kvp => colorToMaterialMap[kvp.Key] = kvp.Value);
                    }
                    else
                    {
                        foreach (Color c in toAdd.Keys)
                        {
                            if (!colorToMaterialMap.ContainsKey(c))
                            {
                                colorToMaterialMap[c] = toAdd[c];
                            }
                        }
                    }
                }

                toAdd.Clear();
            }

            #endregion Layer 2

            #region Finalize
            lock (colorToMaterialMap)
            {
                TaskManager.SafeReport(80);
                lock (this.colorPalette)
                {
                    this.colorPalette.Clear();
                    this.colorPalette = new KDColorTree();
                    colorToMaterialMap.Keys.Where(c => c.ToArgb() != 16777215 && c.A != 0)
                        .ToList()
                        .ForEach(c => this.colorPalette.Add(c));
                }

                colorToMaterialMap[Materials.Air.getAverageColor(isSide)] = new Material[1] { Materials.Air };
                colorToMaterialMap[Color.FromArgb(0, 255, 255, 255)] = new Material[1] { Materials.Air };
            }

            TaskManager.SafeReport(90);

            lock (this.BestMatchCache)
            {
                if (isClearBestMatchCache)
                {
                    BestMatchCache.Clear();
                }
                else
                {
                    this.BestMatchCache.ToList().ForEach(kvp =>
                    {
                        if (!colorToMaterialMap.ContainsKey(kvp.Value))
                        {
                            this.BestMatchCache.Remove(kvp.Key);
                        }
                    });
                }
            }

            #endregion Finalize

            TaskManager.SafeReport(100, "Color map finished compiling");
            this.ColorToMaterialMap = colorToMaterialMap;
        }


        public Color FindBestMatch(Color toMatch)
        {
            if (BestMatchCache.TryGetValue(toMatch, out Color found))
            {
                return found;
            }

            if (toMatch.A < 30)
            {
                var cc = Materials.Air.getAverageColor(true);
                BestMatchCache[toMatch] = cc;
                return cc;
            }

            var rt = this.colorPalette.FindBestMatch(toMatch);

            if (rt != default(Color))
            {
                BestMatchCache[toMatch] = rt;
            }

            return rt;
        }

        public List<Color> FindBestMatches(Color toMatch, int top)
        {
            var rt = this.colorPalette.Nearest(new double[] { toMatch.R, toMatch.G, toMatch.B }, top + 30)
                .OrderBy(x => x.Node.Value.GetColorDistance(toMatch))
                .Select(x => x.Node.Value)
                .Take(top)
                .ToList();

            return rt;
        }


        // Color blending is just a linear interpolation per channel, right?
        // So the math is pretty simple. If you have RGBA1 over RGB2, the 
        // effective visual result RGB3 will be:
        // r3 = r2 + (r1 - r2) * a1
        // g3 = g2 + (g1 - g2) * a1
        // b3 = b2 + (b1 - b2) * a1
        private static Color OverlayColor(Color RGBA1_Top, Color RGBA2_Bottom)
        {
            double alpha = Convert.ToDouble(RGBA1_Top.A) / 255;
            int R = (int) ((RGBA1_Top.R * alpha) + (RGBA2_Bottom.R * (1.0 - alpha)));
            int G = (int) ((RGBA1_Top.G * alpha) + (RGBA2_Bottom.G * (1.0 - alpha)));
            int B = (int) ((RGBA1_Top.B * alpha) + (RGBA2_Bottom.B * (1.0 - alpha)));
            return Color.FromArgb(255, R, G, B);
        }
    }
}
