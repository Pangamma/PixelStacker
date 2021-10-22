using System;

namespace PixelStacker.Core.Engine.Quantizer.Quantizers.XiaolinWu
{
    internal class WuColorCube
    {
        /// <summary>
        /// Gets or sets the red minimum.
        /// </summary>
        /// <value>The red minimum.</value>
        public int RedMinimum { get; set; }

        /// <summary>
        /// Gets or sets the red maximum.
        /// </summary>
        /// <value>The red maximum.</value>
        public int RedMaximum { get; set; }

        /// <summary>
        /// Gets or sets the green minimum.
        /// </summary>
        /// <value>The green minimum.</value>
        public int GreenMinimum { get; set; }

        /// <summary>
        /// Gets or sets the green maximum.
        /// </summary>
        /// <value>The green maximum.</value>
        public int GreenMaximum { get; set; }

        /// <summary>
        /// Gets or sets the blue minimum.
        /// </summary>
        /// <value>The blue minimum.</value>
        public int BlueMinimum { get; set; }

        /// <summary>
        /// Gets or sets the blue maximum.
        /// </summary>
        /// <value>The blue maximum.</value>
        public int BlueMaximum { get; set; }

        /// <summary>
        /// Gets or sets the cube volume.
        /// </summary>
        /// <value>The volume.</value>
        public int Volume { get; set; }
    }
}
