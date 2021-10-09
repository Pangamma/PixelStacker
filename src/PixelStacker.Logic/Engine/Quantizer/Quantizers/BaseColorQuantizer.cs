using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using SimplePaletteQuantizer.Helpers;
using SimplePaletteQuantizer.PathProviders;

namespace SimplePaletteQuantizer.Quantizers
{
    public abstract class BaseColorQuantizer : IColorQuantizer
    {
        #region | Constants |

        /// <summary>
        /// This index will represent invalid palette index.
        /// </summary>
        protected const int InvalidIndex = -1;

        #endregion

        #region | Fields |

        private bool paletteFound;
        private long uniqueColorIndex;
        private IPathProvider pathProvider;
        protected readonly ConcurrentDictionary<int, short> UniqueColors;

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseColorQuantizer"/> class.
        /// </summary>
        protected BaseColorQuantizer()
        {
            pathProvider = null;
            uniqueColorIndex = -1;
            UniqueColors = new ConcurrentDictionary<int, short>();
        }

        #endregion

        #region | Methods |

        /// <summary>
        /// Changes the path provider.
        /// </summary>
        /// <param name="pathProvider">The path provider.</param>
        public void ChangePathProvider(IPathProvider pathProvider)
        {
            this.pathProvider = pathProvider;
        }

        #endregion

        #region | Helper methods |

        private IPathProvider GetPathProvider()
        {
            // if there is no path provider, it attempts to create a default one; integrated in the quantizer
            IPathProvider result = pathProvider ?? (pathProvider = OnCreateDefaultPathProvider());

            // if the provider exists; or default one was created for these purposes.. use it
            if (result == null)
            {
                string message = string.Format("The path provider is not initialized! Please use SetPathProvider() method on quantizer.");
                throw new ArgumentNullException(message);
            }

            // provider was obtained somehow, use it
            return result;
        }

        #endregion

        #region | Abstract/virtual methods |

        /// <summary>
        /// Called when quantizer is about to be prepared for next round.
        /// </summary>
        protected virtual void OnPrepare(ImageBuffer image)
        {
            uniqueColorIndex = -1;
            paletteFound = false;
            UniqueColors.Clear();
        }

        /// <summary>
        /// Called when color is to be added.
        /// </summary>
        protected virtual void OnAddColor(Color color, int key, int x, int y)
        {
            UniqueColors.AddOrUpdate(key,
                colorKey => (byte) Interlocked.Increment(ref uniqueColorIndex), 
                (colorKey, colorIndex) => colorIndex);
        }

        /// <summary>
        /// Called when a need to create default path provider arisen.
        /// </summary>
        protected virtual IPathProvider OnCreateDefaultPathProvider()
        {
            pathProvider = new StandardPathProvider();
            return new StandardPathProvider();
        }

        /// <summary>
        /// Called when quantized palette is needed.
        /// </summary>
        protected virtual List<Color> OnGetPalette(int colorCount)
        {
            // early optimalization, in case the color count is lower than total unique color count
            if (UniqueColors.Count > 0 && colorCount >= UniqueColors.Count)
            {
                // palette was found
                paletteFound = true;

                // generates the palette from unique numbers
                return UniqueColors.
                    OrderBy(pair => pair.Value).
                    Select(pair => Color.FromArgb(pair.Key)).
                    Select(color => Color.FromArgb(255, color.R, color.G, color.B)).
                    ToList();
            }

            // otherwise make it descendant responsibility
            return null;
        }

        /// <summary>
        /// Called when get palette index for a given color should be returned.
        /// </summary>
        protected virtual void OnGetPaletteIndex(Color color, int key, int x, int y, out int paletteIndex)
        {
            // by default unknown index is returned
            paletteIndex = InvalidIndex;

            // if we previously found palette quickly (without quantization), use it
            if (paletteFound && UniqueColors.TryGetValue(key, out short foundIndex))
            {
                paletteIndex = foundIndex;
            }
        }

        /// <summary>
        /// Called when get color count.
        /// </summary>
        protected virtual int OnGetColorCount()
        {
            return UniqueColors.Count;
        }

        /// <summary>
        /// Called when about to clear left-overs after quantization.
        /// </summary>
        protected virtual void OnFinish()
        {
            // do nothing here
        }

        #endregion

        #region << IPathProvider >>

        /// <summary>
        /// See <see cref="IPathProvider.GetPointPath"/> for more details.
        /// </summary>
        public IList<Point> GetPointPath(int width, int heigth)
        {
            return GetPathProvider().GetPointPath(width, heigth);
        }

        #endregion

        #region << IColorQuantizer >>

        /// <summary>
        /// See <see cref="IColorQuantizer.AllowParallel"/> for more details.
        /// </summary>
        public abstract bool AllowParallel { get; }

        /// <summary>
        /// See <see cref="IColorQuantizer.Label"/> for more details.
        /// </summary>
        public abstract string Label { get; }

        /// <summary>
        /// See <see cref="IColorQuantizer.Prepare"/> for more details.
        /// </summary>
        public void Prepare(ImageBuffer image)
        {
            OnPrepare(image);
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.AddColor"/> for more details.
        /// </summary>
        public void AddColor(Color color, int x, int y)
        {
            color = QuantizationHelper.ConvertAlpha(color, out int key);
            OnAddColor(color, key, x, y);
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.GetColorCount"/> for more details.
        /// </summary>
        public int GetColorCount()
        {
            return OnGetColorCount();
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.GetPalette"/> for more details.
        /// </summary>
        public List<Color> GetPalette(int colorCount)
        {
            return OnGetPalette(colorCount);
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.GetPaletteIndex"/> for more details.
        /// </summary>
        public int GetPaletteIndex(Color color, int x, int y)
        {
            color = QuantizationHelper.ConvertAlpha(color, out int key);
            OnGetPaletteIndex(color, key, x, y, out int result);
            return result;
        }

        /// <summary>
        /// See <see cref="IColorQuantizer.Finish"/> for more details.
        /// </summary>
        public void Finish()
        {
            OnFinish();
        }

        #endregion
    }
}
