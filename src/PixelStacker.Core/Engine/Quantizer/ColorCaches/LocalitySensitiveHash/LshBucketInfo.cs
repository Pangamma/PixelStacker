using System;
using System.Collections.Generic;
using PixelStacker.Core.Model.Drawing;

namespace PixelStacker.Core.Engine.Quantizer.ColorCaches.LocalitySensitiveHash
{
    public class BucketInfo
    {
        private readonly SortedDictionary<int, PxColor> colors;

        /// <summary>
        /// Gets the colors.
        /// </summary>
        /// <value>The colors.</value>
        public IDictionary<int, PxColor> Colors
        {
            get { return colors; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BucketInfo"/> class.
        /// </summary>
        public BucketInfo()
        {
            colors = new SortedDictionary<int, PxColor>();
        }

        /// <summary>
        /// Adds the color to the bucket informations.
        /// </summary>
        /// <param name="paletteIndex">Index of the palette.</param>
        /// <param name="color">The color.</param>
        public void AddColor(int paletteIndex, PxColor color)
        {
            colors.Add(paletteIndex, color);
        }
    }
}
