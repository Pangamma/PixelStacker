using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using PixelStacker.Logic.Engine.Quantizer.Helpers;
using PixelStacker.Logic.Engine.Quantizer.ColorCaches.Common;

namespace PixelStacker.Logic.Engine.Quantizer.ColorCaches.LocalitySensitiveHash
{
    public class LshColorCache : BaseColorCache
    {
        #region | Constants |

        private const byte DefaultQuality = 16; // 16
        private const long MaximalDistance = 4096;

        private const float NormalizedDistanceRGB = 1.0f / 196608.0f; // 256*256*3 (RGB) = 196608 / 768.0f
        private const float NormalizedDistanceHSL = 1.0f / 260672.0f; // 360*360 (H) + 256*256*2 (SL) = 260672 / 872.0f
        private const float NormalizedDistanceLab = 1.0f / 507.0f; // 13*13*3 = 507 / 300.0f

        #endregion

        #region | Fields |

        private byte quality;
        private long bucketSize;
        private long minBucketIndex;
        private long maxBucketIndex;
        private BucketInfo[] buckets;

        #endregion

        #region | Properties |

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        /// <value>The quality.</value>
        public byte Quality
        {
            get { return quality; }
            set
            {
                quality = value;

                bucketSize = MaximalDistance / quality;
                minBucketIndex = quality;
                maxBucketIndex = 0;

                buckets = new BucketInfo[quality];
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is color model supported.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is color model supported; otherwise, <c>false</c>.
        /// </value>
        public override bool IsColorModelSupported
        {
            get { return true; }
        }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="LshColorCache"/> class.
        /// </summary>
        public LshColorCache()
        {
            ColorModel = ColorModel.RedGreenBlue;
            Quality = DefaultQuality;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LshColorCache"/> class.
        /// </summary>
        /// <param name="colorModel">The color model.</param>
        /// <param name="quality">The quality.</param>
        public LshColorCache(ColorModel colorModel, byte quality)
        {
            ColorModel = colorModel;
            Quality = quality;
        }

        #endregion

        #region | Helper methods |

        private long GetColorBucketIndex(Color color)
        {
            float normalizedDistance = 0.0f;

            switch (ColorModel)
            {
                case ColorModel.RedGreenBlue: normalizedDistance = NormalizedDistanceRGB; break;
                case ColorModel.HueSaturationLuminance: normalizedDistance = NormalizedDistanceHSL; break;
                case ColorModel.LabColorSpace: normalizedDistance = NormalizedDistanceLab; break;
            }

            ColorModelHelper.GetColorComponents(ColorModel, color, out float componentA, out float componentB, out float componentC);
            float distance = componentA * componentA + componentB * componentB + componentC * componentC;
            float normalized = distance * normalizedDistance * MaximalDistance;
            long resultHash = (long)normalized / bucketSize;

            return resultHash;
        }

        private BucketInfo GetBucket(Color color)
        {
            long bucketIndex = GetColorBucketIndex(color);

            if (bucketIndex < minBucketIndex)
            {
                bucketIndex = minBucketIndex;
            }
            else if (bucketIndex > maxBucketIndex)
            {
                bucketIndex = maxBucketIndex;
            }
            else if (buckets[bucketIndex] == null)
            {
                bool bottomFound = false;
                bool topFound = false;
                long bottomBucketIndex = bucketIndex;
                long topBucketIndex = bucketIndex;

                while (!bottomFound && !topFound)
                {
                    bottomBucketIndex--;
                    topBucketIndex++;
                    bottomFound = bottomBucketIndex > 0 && buckets[bottomBucketIndex] != null;
                    topFound = topBucketIndex < quality && buckets[topBucketIndex] != null;
                }

                bucketIndex = bottomFound ? bottomBucketIndex : topBucketIndex;
            }

            return buckets[bucketIndex];
        }

        #endregion

        #region << BaseColorCache >>

        /// <summary>
        /// See <see cref="BaseColorCache.Prepare"/> for more details.
        /// </summary>
        public override void Prepare()
        {
            base.Prepare();
            buckets = new BucketInfo[quality];
        }

        /// <summary>
        /// See <see cref="BaseColorCache.OnCachePalette"/> for more details.
        /// </summary>
        protected override void OnCachePalette(IList<Color> palette)
        {
            int paletteIndex = 0;
            minBucketIndex = quality;
            maxBucketIndex = 0;

            foreach (Color color in palette)
            {
                long bucketIndex = GetColorBucketIndex(color);
                BucketInfo bucket = buckets[bucketIndex] ?? new BucketInfo();
                bucket.AddColor(paletteIndex++, color);
                buckets[bucketIndex] = bucket;

                if (bucketIndex < minBucketIndex) minBucketIndex = bucketIndex;
                if (bucketIndex > maxBucketIndex) maxBucketIndex = bucketIndex;
            }
        }

        /// <summary>
        /// See <see cref="BaseColorCache.OnGetColorPaletteIndex"/> for more details.
        /// </summary>
        protected override void OnGetColorPaletteIndex(Color color, out int paletteIndex)
        {
            BucketInfo bucket = GetBucket(color);
            int colorCount = bucket.Colors.Count();
            paletteIndex = 0;

            if (colorCount == 1)
            {
                paletteIndex = bucket.Colors.First().Key;
            }
            else
            {
                int index = 0;
                int colorIndex = ColorModelHelper.GetEuclideanDistance(color, ColorModel, bucket.Colors.Values.ToList());

                foreach (int colorPaletteIndex in bucket.Colors.Keys)
                {
                    if (index == colorIndex)
                    {
                        paletteIndex = colorPaletteIndex;
                        break;
                    }

                    index++;
                }
            }
        }

        #endregion
    }
}
