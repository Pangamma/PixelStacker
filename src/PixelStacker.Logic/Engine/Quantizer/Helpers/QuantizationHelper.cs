using System.Drawing;

namespace PixelStacker.Logic.Engine.Quantizer.Helpers
{
    public class QuantizationHelper
    {
        private const int Alpha = 255 << 24;
        private static readonly Color BackgroundColor;
        private static readonly double[] Factors;

        static QuantizationHelper()
        {
            BackgroundColor = Color.FromArgb(0, 255, 255, 255);
            Factors = PrecalculateFactors();
        }

        /// <summary>
        /// Precalculates the alpha-fix values for all the possible alpha values (0-255).
        /// </summary>
        private static double[] PrecalculateFactors()
        {
            double[] result = new double[256];

            for (int value = 0; value < 256; value++)
            {
                result[value] = value / 255.0;
            }

            return result;
        }

        /// <summary>
        /// Converts the alpha blended color to a non-alpha blended color.
        /// </summary>
        /// <param name="color">The alpha blended color (ARGB).</param>
        /// <returns>The non-alpha blended color (RGB).</returns>
        internal static Color ConvertAlpha(Color color)
        {
            return ConvertAlpha(color, out _);
        }

        /// <summary>
        /// Converts the alpha blended color to a non-alpha blended color.
        /// </summary>
        internal static Color ConvertAlpha(Color color, out int argb)
        {
            Color result = color;

            if (color.A < 255)
            {
                // performs a alpha blending (second color is BackgroundColor, by default a Control color)
                double colorFactor = Factors[color.A];
                double backgroundFactor = Factors[255 - color.A];
                int red = (int)(color.R * colorFactor + BackgroundColor.R * backgroundFactor);
                int green = (int)(color.G * colorFactor + BackgroundColor.G * backgroundFactor);
                int blue = (int)(color.B * colorFactor + BackgroundColor.B * backgroundFactor);
                argb = red << 16 | green << 8 | blue;
                result = Color.FromArgb(Alpha | argb);
            }
            else
            {
                argb = color.R << 16 | color.G << 8 | color.B;
            }

            return result;
        }
    }
}
