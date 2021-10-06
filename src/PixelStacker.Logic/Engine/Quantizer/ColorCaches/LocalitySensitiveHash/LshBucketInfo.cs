using System;
using System.Collections.Generic;
using System.Drawing;

namespace SimplePaletteQuantizer.ColorCaches.LocalitySensitiveHash
{
    public class BucketInfo
    {
        private readonly SortedDictionary<int, Color> colors;

        /// <summary>
        /// Gets the colors.
        /// </summary>
        /// <value>The colors.</value>
        public IDictionary<int, Color> Colors
        {
            get { return colors; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BucketInfo"/> class.
        /// </summary>
        public BucketInfo()
        {
            colors = new SortedDictionary<int, Color>();
        }

        /// <summary>
        /// Adds the color to the bucket informations.
        /// </summary>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <param name="color">The color.</param>
        public void AddColor(int paletteIndex, Color color)
        {
            colors.Add(paletteIndex, color);
        }
    }
}
