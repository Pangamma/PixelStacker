using System;
using System.Drawing;

namespace SimplePaletteQuantizer.Quantizers.Popularity
{
    internal class PopularityColorSlot
    {
        #region | Fields |

        private int red;
        private int green;
        private int blue;

        #endregion

        #region | Properties |

        /// <summary>
        /// Gets or sets the pixel count.
        /// </summary>
        /// <value>The pixel count.</value>
        public int PixelCount { get; private set; }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="PopularityColorSlot"/> class.
        /// </summary>
        /// <param name="color">The color.</param>
        public PopularityColorSlot(Color color)
        {
            AddValue(color);
        }

        #endregion

        #region | Methods |

        /// <summary>
        /// Adds the value to the slot.
        /// </summary>
        /// <param name="color">The color to be added.</param>
        public PopularityColorSlot AddValue(Color color)
        {
            red += color.R;
            green += color.G;
            blue += color.B;
            PixelCount++;
            return this;
        }

        /// <summary>
        /// Gets the average, just simple value divided by pixel presence.
        /// </summary>
        /// <returns>The average color component value.</returns>
        public Color GetAverage()
        {
            // determines the components
            int finalRed = red/PixelCount;
            int finalGreen = green/PixelCount;
            int finalBlue = blue/PixelCount;

            // clamps the invalid values
            if (finalRed < 0) finalRed = 0;
            if (finalRed > 255) finalRed = 255;
            if (finalGreen < 0) finalGreen = 0;
            if (finalGreen > 255) finalGreen = 255;
            if (finalBlue < 0) finalBlue = 0;
            if (finalBlue > 255) finalBlue = 255;

            // returns the reconstructed color
            return Color.FromArgb(255, finalRed, finalGreen, finalBlue);
        }

        #endregion
    }
}