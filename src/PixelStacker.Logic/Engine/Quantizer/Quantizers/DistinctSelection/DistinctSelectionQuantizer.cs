#define UseDictionary

using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using SimplePaletteQuantizer.ColorCaches;
using SimplePaletteQuantizer.ColorCaches.Octree;
using SimplePaletteQuantizer.Helpers;
using PixelStacker.Extensions;

#if (UseDictionary)
using System.Collections.Concurrent;
#endif

namespace SimplePaletteQuantizer.Quantizers.DistinctSelection
{
    /// <summary>
    /// This is my baby. Read more in the article on the Code Project:
    /// http://www.codeproject.com/KB/recipes/SimplePaletteQuantizer.aspx
    /// </summary>
    public class DistinctSelectionQuantizer : BaseColorCacheQuantizer
    {
        #region | Fields |

        private List<Color> palette;
        private int foundColorCount;

#if (UseDictionary)
        private ConcurrentDictionary<int, DistinctColorInfo> colorMap;
#else
        private DistinctBucket rootBucket;
#endif

        #endregion

        #region | Methods |

        private static bool ProcessList(int colorCount, List<DistinctColorInfo> list, ICollection<IEqualityComparer<DistinctColorInfo>> comparers, out List<DistinctColorInfo> outputList)
        {
            IEqualityComparer<DistinctColorInfo> bestComparer = null;
            int maximalCount = 0;
            outputList = list;

            foreach (IEqualityComparer<DistinctColorInfo> comparer in comparers)
            {
                List<DistinctColorInfo> filteredList = list.
                    Distinct(comparer).
                    ToList();

                int filteredListCount = filteredList.Count;

                if (filteredListCount > colorCount && filteredListCount > maximalCount)
                {
                    maximalCount = filteredListCount;
                    bestComparer = comparer;
                    outputList = filteredList;

                    if (maximalCount <= colorCount) break;
                }
            }

            comparers.Remove(bestComparer);
            return comparers.Count > 0 && maximalCount > colorCount;
        }

        #endregion

        #region << BaseColorCacheQuantizer >>

        /// <summary>
        /// See <see cref="IColorQuantizer.Prepare"/> for more details.
        /// </summary>
        protected override void OnPrepare(ImageBuffer image)
        {
            base.OnPrepare(image);

            OnFinish();
        }

        /// <summary>
        /// See <see cref="BaseColorCacheQuantizer.OnCreateDefaultCache"/> for more details.
        /// </summary>
        protected override IColorCache OnCreateDefaultCache()
        {
            // use OctreeColorCache best performance/quality
            return new OctreeColorCache();
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnAddColor"/> for more details.
        /// </summary>
        protected override void OnAddColor(Color color, int key, int x, int y)
        {
#if (UseDictionary)
            colorMap.AddOrUpdate(key,
                colorKey => new DistinctColorInfo(color),
                (colorKey, colorInfo) => colorInfo.IncreaseCount());
#else
            color = QuantizationHelper.ConvertAlpha(color);
            rootBucket.StoreColor(color);
#endif
        }

        /// <summary>
        /// See <see cref="BaseColorCacheQuantizer.OnGetPaletteToCache"/> for more details.
        /// </summary>
        protected override List<Color> OnGetPaletteToCache(int colorCount)
        {
            // otherwise calculate one
            palette.Clear();

            // lucky seed :)
            FastRandom random = new FastRandom(13);

#if (UseDictionary)
            List<DistinctColorInfo> colorInfoList = colorMap.Values.ToList();
#else
            List<DistinctColorInfo> colorInfoList = rootBucket.GetValues().ToList();
#endif

            foundColorCount = colorInfoList.Count;

            if (foundColorCount >= colorCount)
            {
                // shuffles the colormap
                colorInfoList = colorInfoList.
                    OrderBy(entry => random.Next(foundColorCount)).
                    ToList();

                // workaround for backgrounds, the most prevalent color
                DistinctColorInfo background = colorInfoList.MaxBy(info => info.Count);
                colorInfoList.Remove(background);
                colorCount--;

                ColorHueComparer hueComparer = new ColorHueComparer();
                ColorSaturationComparer saturationComparer = new ColorSaturationComparer();
                ColorBrightnessComparer brightnessComparer = new ColorBrightnessComparer();

                // generates catalogue
                List<IEqualityComparer<DistinctColorInfo>> comparers = new List<IEqualityComparer<DistinctColorInfo>> { hueComparer, saturationComparer, brightnessComparer };

                // take adequate number from each slot
                while (ProcessList(colorCount, colorInfoList, comparers, out colorInfoList)) { }

                int listColorCount = colorInfoList.Count();

                if (listColorCount > 0)
                {
                    int allowedTake = Math.Min(colorCount, listColorCount);
                    colorInfoList = colorInfoList.Take(allowedTake).ToList();
                }

                // adds background color first
                palette.Add(Color.FromArgb(background.Color));
            }

            // adds the selected colors to a final palette
            palette.AddRange(colorInfoList.Select(colorInfo => Color.FromArgb(colorInfo.Color)));

            // returns our new palette
            return palette;
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.GetColorCount"/> for more details.
        /// </summary>
        protected override int OnGetColorCount()
        {
            return foundColorCount;
        }

        /// <summary>
        /// See <see cref="BaseColorQuantizer.OnFinish"/> for more details.
        /// </summary>
        protected override void OnFinish()
        {
            base.OnFinish();

            palette = new List<Color>();

#if (UseDictionary)
            colorMap = new ConcurrentDictionary<int, DistinctColorInfo>();
#else
            rootBucket = new DistinctBucket();
#endif
        }

        #endregion

        #region << IColorQuantizer >>

        /// <summary>
        /// See <see cref="IColorQuantizer.Label"/> for more details.
        /// </summary>
        public override string Label => "HSL distinct selection";

        /// <summary>
        /// See <see cref="IColorQuantizer.AllowParallel"/> for more details.
        /// </summary>
        public override bool AllowParallel
        {
            get { return true; }
        }

        #endregion

        #region | Helper classes (comparers) |

        /// <summary>
        /// Compares a hue components of a color info.
        /// </summary>
        public class ColorHueComparer : IEqualityComparer<DistinctColorInfo>
        {
            public bool Equals(DistinctColorInfo x, DistinctColorInfo y)
            {
                return x.Hue == y.Hue;
            }

            public int GetHashCode(DistinctColorInfo colorInfo)
            {
                return colorInfo.Hue.GetHashCode();
            }
        }

        /// <summary>
        /// Compares a saturation components of a color info.
        /// </summary>
        public class ColorSaturationComparer : IEqualityComparer<DistinctColorInfo>
        {
            public bool Equals(DistinctColorInfo x, DistinctColorInfo y)
            {
                return x.Saturation == y.Saturation;
            }

            public int GetHashCode(DistinctColorInfo colorInfo)
            {
                return colorInfo.Saturation.GetHashCode();
            }
        }

        /// <summary>
        /// Compares a brightness components of a color info.
        /// </summary>
        public class ColorBrightnessComparer : IEqualityComparer<DistinctColorInfo>
        {
            public bool Equals(DistinctColorInfo x, DistinctColorInfo y)
            {
                return x.Brightness == y.Brightness;
            }

            public int GetHashCode(DistinctColorInfo colorInfo)
            {
                return colorInfo.Brightness.GetHashCode();
            }
        }

        #endregion
    }
}


